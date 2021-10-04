using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    public List<Transform> cp = new List<Transform>();
    
    public Material activeCP;
    public Material inactiveCP;
    public Material finishCP;

    public Text checkpointCollected;
    public Text menuCheckpointCollected;

    private void Start()
    {
        //make CPs inactive
        foreach (Transform t in transform)
        {
            cp.Add(t);
            t.GetComponent<MeshRenderer>().material = inactiveCP;
        }

        //Make sure theres CP in the level
        if (cp.Count == 0)
        {
            return;
        }
        checkpointCollected.text = "Checkpoints: " + SaveManager.Instance.state.cpCollected + "/" + cp.Count;
    }

    public void NextCP()
    {


        //Up the int of checkpoints collected
        SaveManager.Instance.state.cpCollected++;

        if(SaveManager.Instance.state.cpCollected == cp.Count)
        {
            Finish();
            return;
        }

        if(SaveManager.Instance.state.cpCollected == cp.Count - 1)
        {
            cp[SaveManager.Instance.state.cpCollected].GetComponent<MeshRenderer>().material = finishCP;
        } else
        {
            cp[SaveManager.Instance.state.cpCollected].GetComponent<MeshRenderer>().material = activeCP;

            
        }
        cp[SaveManager.Instance.state.cpCollected].GetComponent<CheckPoint>().ActiveCP();
        checkpointCollected.text = "Checkpoints: " + SaveManager.Instance.state.cpCollected + "/" + cp.Count;
    }

    private void Finish()
    {
        menuCheckpointCollected.text = "Checkpoints: " + SaveManager.Instance.state.cpCollected + "/" + cp.Count;
        FindObjectOfType<GameController>().CompleteLevel();
       

    }

}
