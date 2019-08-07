using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [Range(0, 1)] [SerializeField] float movementFactor; // Serialized for debug, basically the % of oscillation completed
    [SerializeField] Vector3 movementVector; // the end offset of the desired transform (oscillates to this offset)
    [SerializeField] float period; // Our desired period of oscillation

    Vector3 startingPos;

    // Start is called before the first frame update
    void Start() {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update() {

        if(period <= Mathf.Epsilon) { return; }

        float cycles = Time.time / period; // Grows continuously from 0, how many cycles we've completed so far
        const float tau = Mathf.PI * 2f; // About 6.28, just a constant
        float rawSinWave = Mathf.Sin(cycles * tau); // Varies from -1 to 1, basically getting a sin wave by converting cycles to radians
        movementFactor = (rawSinWave / 2f) + .5f; // Goes from 0 to 1, just manipulates the sin wave output to be from 0 to 1

        Vector3 offset = movementVector * movementFactor; // Calculates the actual offset to place the object at
        transform.position = startingPos + offset; // Moves the object
    }
}
