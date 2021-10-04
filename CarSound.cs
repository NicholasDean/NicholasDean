using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{

    AudioSource honk;

    private void Start()
    {
        honk = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        honk.Play();
    }

}
