namespace _3Gear.Core.Engine //author = 3Gear (Youtube)
{

    using UnityEngine;
    using System.Collections;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.ComponentModel;
    using UnityEngine.UI;
    using UnityEngine.Experimental.UIElements;
    using UXF;

    public class OLDpendulumAndParticipantMovement : MonoBehaviour
    {
        public Session session; //reference to the UXF session so we can start the trial

        Quaternion _start, _end;
        private GameObject pendulum;
        private GameObject participantA;
        private GameObject participantB;
        private GameObject wholePendulum;
        private float _angle = 60.0f;
        private float _speed = 2.0f;
        private float _startTime = 0.0f;

        public int Pair = 99;
        public static string GetCodeName(System.Enum e)
        {
            var nm = e.ToString();
            var tp = e.GetType();
            var field = tp.GetField(nm);
            var attrib = System.Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (attrib != null)
                return attrib.Description;
            else
                return nm;
        }

        [System.Serializable]
        public enum ppt
        {
            [Description("A")]
            participantA,
            [Description("B")]
            participantB,
            [Description("AB")]
            bothParticipants
        }
        public ppt participant;

        [System.Serializable]
        public enum met
        {
            [Description("NM")]
            noMetronome,
            [Description("M")]
            metronome
        }
        public met metronome;

        [System.Serializable]
        public enum social
        {
            [Description("NSP")]
            noSocialPresence,
            [Description("SP")]
            socialPresence
        }
        public social socialPresence;

        [System.Serializable]
        public enum coordination
        {
            [Description("SPONT")]
            spontaneous,
            [Description("INTENT")]
            intentional,
            [Description("NA")]
            NA
        }
        public coordination coordType;



        void Awake() //change back to start?
        {
            _start = PendulumRotation(-_angle);
            _end = PendulumRotation(_angle);
            pendulum = GameObject.Find("/Hinge1/Pendulum1/Ball1");
            wholePendulum = GameObject.Find("/Hinge1");
            participantA = GameObject.Find("/Knuckle (0)");
            participantB = GameObject.Find("/Knuckle (1)");

            wholePendulum.SetActive(false);
        }

        IEnumerator Countdown()
        {
            yield return new WaitForSeconds(0.3f);
            wholePendulum.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            session.EndCurrentTrial();
        }


        private void FixedUpdate()

        {

            _startTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(_start, _end, (Mathf.Sin(_startTime * _speed + Mathf.PI / 2) + 1.0f) / 2.0f);

            Vector3 pendulumPosition = new Vector3(pendulum.transform.position.x, pendulum.transform.position.y, pendulum.transform.position.z);
            Vector3 participantPositionA = new Vector3(participantA.transform.position.x, participantA.transform.position.y, participantA.transform.position.z);
            Vector3 participantPositionB = new Vector3(participantB.transform.position.x, participantB.transform.position.y, participantB.transform.position.z);
            var line = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
                                    DateTime.Now,
                                    pendulumPosition[0], pendulumPosition[1], pendulumPosition[2],
                                    participantPositionA[0], participantPositionA[1], participantPositionA[2],
                                    participantPositionB[0], participantPositionB[1], participantPositionB[2]);
            var fileName = Pair + "_" + GetCodeName(participant) + "_" + GetCodeName(metronome) + "_" + GetCodeName(socialPresence) + "_" + GetCodeName(coordType) + ".txt";
            StreamWriter writer = new StreamWriter(fileName, true);
            writer.WriteLine(line);
            {
                if (writer != null)
                    writer.Close();
            }

        }

        Quaternion PendulumRotation(float angle)
        {
            var pendulumRotation = transform.rotation;
            var angleZ = pendulumRotation.eulerAngles.z + angle;

            if (angleZ > 180)
                angleZ -= 360;
            else if (angleZ < -180)
                angleZ += 360;

            pendulumRotation.eulerAngles = new Vector3(pendulumRotation.eulerAngles.x, pendulumRotation.eulerAngles.y, angleZ);
            return pendulumRotation;
        }


    }
}
