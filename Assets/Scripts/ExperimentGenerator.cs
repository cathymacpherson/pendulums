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

    UXF.Session session;

    /// <summary>
    /// generates the trials and blocks for the session
    /// </summary>
    /// <param name="experimentSession"></param>
    public void GenerateExperiment(Session experimentSession)
    {
        // save reference to session
        session = experimentSession;

        // retrieve the n_main_trials setting, which was loaded from our .json file into our session settings
        int numTrials = 1; // session.settings.GetInt("n_main_trials");

        // create main block
        Block mainBlock = session.CreateBlock(numTrials);

    }

  /*  public void StartLoop()
    {
        // called from OnSessionBegin, hence starting the trial loop when the session starts
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        foreach (Trial trial in session.Trials)
        {
            trial.Begin();
            PresentStimulus(trial);
            yield return new WaitForSeconds(80f);
            trial.End();
        }

        session.End();
    } */


    void PresentStimulus(Trial trial) // here we can imagine presentation of some stimulus
    {
        Debug.Log("Running trial!");


        Invoke("EndAndPrepare", 70);

        //distances and manipulation
        //add click of button here which moves you to the next trial


        // we can access our settings to (e.g.) modify our scene
        // for more information about retrieving settings see the documentation

        //float size = trial.settings.GetFloat("size");
        //Debug.LogFormat("The 'size' for this trial is: {0}", size);

        // record custom values...
        // string observation = UnityEngine.Random.value.ToString();
        // Debug.Log(string.Format("We observed: {0}", observation));
        // trial.result["some_variable"] = observation;

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