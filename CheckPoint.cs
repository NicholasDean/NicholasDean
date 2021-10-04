using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    private Objective objectiveScript;
    private bool cpActive = false;

    public GameObject Checkpoints;

    private void Start()
    {
        objectiveScript = FindObjectOfType<Objective>();
    }

    public void ActiveCP()
    {
        cpActive = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        objectiveScript.NextCP();
        Destroy(gameObject);

    }
}
