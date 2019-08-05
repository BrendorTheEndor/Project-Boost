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
        Rotate();
    }

    private void FixedUpdate() {
        Thrust();
    }

    private void OnCollisionEnter(Collision collision) {
        switch(collision.gameObject.tag) {
            case "Friendly":
                Debug.Log("Fine");
                break;
            default:
                Debug.Log("Dead");
                break;
        }
    }

    private void ProcessInput() {
        Thrust();
        Rotate();
    }

    private void Thrust() {
        if(Input.GetKey(KeyCode.Space)) {
            myRigidbody.AddRelativeForce(new Vector3(0f, thrustSpeed, 0f)); // functionally same as vector3.up, no time.deltatime because it's a physics calculation
            if(!myAudioSource.isPlaying) {
                myAudioSource.Play();
            }
        }
        else {
            myAudioSource.Stop();
        }
    }

    private void Rotate() {

        myRigidbody.freezeRotation = true; // Take manual control of rotation

        if(Input.GetKey(KeyCode.D)) {
            transform.Rotate(new Vector3(0f, 0f, -rotationSpeed * Time.deltaTime));
        }
        else if(Input.GetKey(KeyCode.A)) {
            transform.Rotate(new Vector3(0f, 0f, rotationSpeed * Time.deltaTime));
        }

        myRigidbody.freezeRotation = false; // Resume standard rotation (physics controlled)
    }
}
