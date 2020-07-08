namespace _3Gear.Core.Engine //author = 3Gear (Youtube)
{

    using UnityEngine;
    using System.Collections;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;


    public class pendulumPerceptionTask : MonoBehaviour
    {
        //Private variables
        Quaternion _start, _end; //Quaternions are Euler angles in the 4th dimension
                                 // Quaternion _startAngle;
        private GameObject pendulum; //used to gain position of the pendulum later in code
        private GameObject participantA; //used to gain position of pendulum A later in code
        private GameObject participantB; //used to gain position of pendulum B later in code

        [SerializeField, Range(0.0f, 360f)]
        private float _angle = 60.0f;

        [SerializeField, Range(0.0f, 10.0f)]
        private float _speed = 2.0f;

        [SerializeField, Range(0.0f, 120.0f)]
        private float _startTime = 0.0f;

        [SerializeField, Range(-90.0f, 90.0f)]
        private float _startAngle = 90.0f;

        //Public variables
        //public float[] relativePhase = new float[] { -90f, -60f, -30f, 0f, 30f, 60f, 90f }; FIX!!
        public Transform coupledTarget;
        public Vector3 couplingStrength = new Vector3(.5f, .5f);
        public bool useRigidbodyVelocity = false;
        public bool inMotion = false;
        public int Subject = 99;

        [System.Serializable]
        public enum trialType {solo, dyad}
        public trialType trialtype;
        

        //Start is called on the frame when a script is enabled just beore any of the Update Methods are called the first time.
        //Like the Awake function, Start is called exactly once in the lifetime of the script.
        private void Start()
        {
            _start = PendulumRotation(-_angle);
            _end = PendulumRotation(_angle);
            //_startAngle = pendulum.transform.rotation.z;
            pendulum = GameObject.Find("/Hinge1/Pendulum1/Ball1"); //finds the GameObject Ball1
            participantA = GameObject.Find("/Knuckle (0)");
            participantB = GameObject.Find("/Knuckle (1)");

            //  PendulumStartRotation = pendulum.transform.rotation.z;

            var fileName = Subject + ".txt";
            StreamWriter writer = new StreamWriter(fileName, true);
            var headings = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
                                         "dateTime", "pendulumPositionX", "pendulumPositionY", "pendulumPositionZ",
                                         "participantAPositionX", "participantAPositionY", "participantAPositionZ",
                                         "participantBPositionX", "participantBPositionY", "participantBPositionZ");
            writer.WriteLine(headings);
            writer.Close();

        }

        //Update is called every frame, if the MonoBehaviour is enabled.
        //Update is the most commonly used function to implement any kind of game behaviour.
        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (inMotion == true)
                {
                    inMotion = false;
                }
                else
                {
                    inMotion = true;
                }
            }
        }

        //This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        //FixedUpdate should be used instead of Update when dealing with a Rigidbody.
        //For example when adding a force to a rigidbody, you have to apply the force every
        //fixed frame instead FixedUpdate instead of every frame inside Update.
        //always do physics related tasks in here!
        private void FixedUpdate()


        {

            _startTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(_start, _end, (Mathf.Sin(_startTime * _speed + Mathf.PI / 2) + 1.0f) / 2.0f);


            Vector3 pendulumPosition = new Vector3(pendulum.transform.position.x, pendulum.transform.position.y, pendulum.transform.position.z);
            Vector3 participantPositionA = new Vector3(participantA.transform.position.x, participantA.transform.position.y, participantA.transform.position.z);
            Vector3 participantPositionB = new Vector3(participantB.transform.position.x, participantB.transform.position.y, participantB.transform.position.z);
            var line = string.Format("{0}, {1}, {2}, {3}", DateTime.Now, pendulumPosition, participantPositionA, participantPositionB);
            var fileName = Subject + ".txt";
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
