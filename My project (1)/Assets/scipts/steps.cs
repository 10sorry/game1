using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class steps : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    public void stepz()
    {
        audioSource.Play();
    }

}
