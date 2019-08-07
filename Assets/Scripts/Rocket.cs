using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float thrustSpeed = 1f;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] float timeToDelayLevelLoad = 1f;

    [SerializeField] AudioClip thrustSFX;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip levelCompleteSFX;

    [SerializeField] ParticleSystem thrustVFX;
    [SerializeField] ParticleSystem deathVFX;
    [SerializeField] ParticleSystem levelCompleteVFX;

    enum State { Alive, Dying, Transcending }
    State currentState = State.Alive;
    bool collisionsEnabled;

    Rigidbody myRigidbody;
    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start() {
        collisionsEnabled = true;
        myRigidbody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        ProcessInput();
        if(currentState == State.Alive) {
            Rotate();
        }
    }

    private void FixedUpdate() {
        if(currentState == State.Alive) {
            Thrust();
        }
    }

    private void OnCollisionEnter(Collision collision) {

        if((currentState != State.Alive) || !collisionsEnabled) { return; }

        switch(collision.gameObject.tag) {
            case "Friendly":
                break;
            case "Finish":
                currentState = State.Transcending;
                myAudioSource.Stop();
                myAudioSource.PlayOneShot(levelCompleteSFX);
                levelCompleteVFX.Play();
                Invoke("LoadNextScene", timeToDelayLevelLoad);
                break;
            default:
                currentState = State.Dying;
                myAudioSource.Stop();
                myAudioSource.PlayOneShot(deathSFX);
                deathVFX.Play();
                Invoke("ReloadLevel", timeToDelayLevelLoad);
                break;
        }
    }

    private void ReloadLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextScene() {
        if(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else {
            SceneManager.LoadScene(0);
        }
    }

    private void ProcessInput() {

        if(!Debug.isDebugBuild) { return; }

        if(Input.GetKeyDown(KeyCode.L)) {
            LoadNextScene();
        }
        else if(Input.GetKeyDown(KeyCode.C)) {
            collisionsEnabled = !collisionsEnabled;
        }
    }

    private void Thrust() {
        if(Input.GetKey(KeyCode.Space)) {
            myRigidbody.AddRelativeForce(new Vector3(0f, thrustSpeed, 0f)); // functionally same as vector3.up, no time.deltatime because it's a physics calculation
            if(!myAudioSource.isPlaying) {
                myAudioSource.PlayOneShot(thrustSFX);
            }
            thrustVFX.Play();
        }
        else {
            myAudioSource.Stop();
            thrustVFX.Stop();
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
