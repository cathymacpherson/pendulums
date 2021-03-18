using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.ComponentModel;
using UnityEngine.UI;
using UnityEngine.Experimental.UIElements;
using UXF;


public class ExperimentGenerator : MonoBehaviour
{
    Quaternion _start, _end;
    public GameObject pendulum;
    private GameObject participantA;
    private GameObject participantB;
    private GameObject wholePendulum;
    private float _angle = 60.0f;
    private float _startTime = 0.0f;
    private string metType;

    //private Dropdown pendType;
    private float bpm;
    private string ppt;

    public Session session; // generates the trials and blocks for the session

    void Awake()
    {
        _start = PendulumRotation(-_angle);
        _end = PendulumRotation(_angle);
        pendulum = GameObject.Find("/Hinge1/Pendulum1/Ball1");
        wholePendulum = GameObject.Find("/Hinge1");
        participantA = GameObject.Find("/Knuckle (0)");
        participantB = GameObject.Find("/Knuckle (1)");
        wholePendulum.SetActive(true);
       // print(bpm);
    }

     void GenerateExperiment(Session expSession)
    {
        session = expSession; // save reference to session
        int numTrials = 1; //set number of trials
        Block mainBlock = session.CreateBlock(numTrials); // create main block
        metType = (string)session.participantDetails["met_type"]; //

        /// <param name="ppid"></param>
       // pendType = (Dropdown)session.participantDetails["pend_type"];
        ppt = (string)session.participantDetails["ppt"];
        print(metType);
        print(ppt);
        // bpm = pendType ? 150.0f : 50.0f; //sets bpm to 150 if pendType is T and 50 if pendType is F (pendType is taken from the UI)
        //bpm = metType == "Fast" ? 120.0f : 60.0f; 
        
        if(metType == "Fast")
        {
            bpm = 120.0f; //*Time.deltaTime? this is the time it took to complete the last frame, and is used for continuous movement - but this is already accounted for in the code below?
        }
        else if(metType == "Slow")
        {
            bpm = 60.0f;
        }
        else if(metType == "N/A")
        {
            bpm = 0.0f;
            wholePendulum.SetActive(false);
        }
        
        
        
        print(bpm);
        session.FirstTrial.Begin();
        Debug.Log("Running trial!");
    }

     void setTrialLength(Trial trial) // can be called from OnTrialBegin in the Session inspector
    {
       // print(bpm);
        Invoke("EndAndPrepare", 55);
    }

    //public IEnumerator IncreaseDelay()
    //{
       // yield return null;
       // print(bpm);
       // yield return new WaitForSeconds(5f);
       // Debug.Log("stopping coroutine");
       // yield return new WaitForSeconds(5f);
       // StopCoroutine(IncreaseDelay());
    //}

    void FixedUpdate()
    {
        // moves the pendulum:
        _startTime += Time.deltaTime;
        transform.rotation = Quaternion.Lerp(_start, _end, (Mathf.Sin(_startTime * (bpm / 60) + Mathf.PI / 2) + 1.0f) / 2.0f);

        // reads out the position of the pendulum and trackers:
        Vector3 pendulumPosition = new Vector3(pendulum.transform.position.x, pendulum.transform.position.y, pendulum.transform.position.z);
        Vector3 participantPositionA = new Vector3(participantA.transform.position.x, participantA.transform.position.y, participantA.transform.position.z);
        Vector3 participantPositionB = new Vector3(participantB.transform.position.x, participantB.transform.position.y, participantB.transform.position.z);
        var line = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
                                DateTime.Now,
                                pendulumPosition[0], pendulumPosition[1], pendulumPosition[2],
                                participantPositionA[0], participantPositionA[1], participantPositionA[2],
                                participantPositionB[0], participantPositionB[1], participantPositionB[2]);
        var fileName = ppt + ".txt";
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

