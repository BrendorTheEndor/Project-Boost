using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] float thrustSpeed = 1f;
    [SerializeField] float rotationSpeed = 1f;

    Rigidbody myRigidbody;
    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start() {
        myRigidbody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        ProcessInput();
    }

    private void ProcessInput() {
        if(Input.GetKey(KeyCode.Space)) {
            myRigidbody.AddRelativeForce(new Vector3(0f, thrustSpeed * Time.deltaTime, 0f));
            if(!myAudioSource.isPlaying) {
                myAudioSource.Play();
            }
        }
        else {
            myAudioSource.Stop();
        }

        if(Input.GetKey(KeyCode.D)) {
            transform.Rotate(new Vector3(0f, 0f, -rotationSpeed * Time.deltaTime));
        }
        else if(Input.GetKey(KeyCode.A)) {
            transform.Rotate(new Vector3(0f, 0f, rotationSpeed * Time.deltaTime));
        }
    }
}
