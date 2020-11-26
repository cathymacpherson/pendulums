﻿using UnityEngine;
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

public class pendulumManager : MonoBehaviour
{

    private bool pendType;
    Quaternion _start, _end;
    public GameObject pendulum;
    private GameObject participantA;
    private GameObject participantB;
    private GameObject wholePendulum;
    private float _angle = 60.0f;
    public float bpm;
    private float _startTime = 0.0f;


    public Session session;

    public void selectPendulum(Trial trial) // can be called from OnTrialBegin in the Session inspector
    {
        Invoke("EndAndPrepare", 20);
        pendType = (bool)session.participantDetails["pend_type"];
    }

     void Awake()
    {
        _start = PendulumRotation(-_angle);
        _end = PendulumRotation(_angle);
        pendulum = GameObject.Find("/Hinge1/Pendulum1/Ball1");
        wholePendulum = GameObject.Find("/Hinge1");
        participantA = GameObject.Find("/Knuckle (0)");
        participantB = GameObject.Find("/Knuckle (1)");
        wholePendulum.SetActive(false);
        

        if (pendType == true) // show specific pendulum (slow/fast), depending on the trial
       {
            bpm = 150.0f;
            Debug.Log("it's true!");
        }
        else if (pendType == false)
        {
            bpm = 50.0f;
            Debug.Log("it's false!");
        }

    }


        void FixedUpdate()
    {
        _startTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(_start, _end, (Mathf.Sin(_startTime * (bpm / 60) + Mathf.PI / 2) + 1.0f) / 2.0f);

            Vector3 pendulumPosition = new Vector3(pendulum.transform.position.x, pendulum.transform.position.y, pendulum.transform.position.z);
            Vector3 participantPositionA = new Vector3(participantA.transform.position.x, participantA.transform.position.y, participantA.transform.position.z);
            Vector3 participantPositionB = new Vector3(participantB.transform.position.x, participantB.transform.position.y, participantB.transform.position.z);
            var line = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
                                    DateTime.Now,
                                    pendulumPosition[0], pendulumPosition[1], pendulumPosition[2],
                                    participantPositionA[0], participantPositionA[1], participantPositionA[2],
                                    participantPositionB[0], participantPositionB[1], participantPositionB[2]);
            var fileName = "backup" + ".txt";
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

    void EndAndPrepare()
    {
            Debug.Log("Ending trial");
            session.CurrentTrial.End();
            if (session.CurrentTrial == session.LastTrial)
            {
                session.End();
            }
            else
            {
                session.BeginNextTrial();
            }
    }
}
    



