using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Script category: tutorial

    This script performs rotation of a cube object using randomizer and timer.
    In particular, the cube is rotated around the x/y/z axis with a random speed and updated every 0.5 to 2 seconds.  
    Additionally, the cube color is randomized at the start of the game.

*/
public class Cube : MonoBehaviour
{
    public MeshRenderer Renderer;

    private int _colorLength = 4;

    private float _futureTime = 0.0f;
    private float _rotateSpeedX = 10.0f;
    private float _rotateSpeedY = 0.0f;
    private float _rotateSpeedZ = 0.0f;
    
    void Start()
    {
        transform.position = new Vector3(3, 4, 1);
        transform.localScale = Vector3.one * 2f;
        
        UpdateColor();
    }

    void UpdateColor()
    {
        Material material = Renderer.material;
        // randomize the cube color - keep the alpha value at 1
        Color tempColor = material.color;
        for (int i = 0; i < _colorLength; i++)
        {
            tempColor[i] = Random.Range(0.0f, 1.0f);
        }
        if (tempColor[3] < 0.2f) {
            tempColor[3] = 0.2f;
        }
        material.color = tempColor;
    }

    void UpdateRotationSpeed() {
        _rotateSpeedX = Random.Range(10.0f, 100.0f);
        _rotateSpeedY = Random.Range(10.0f, 100.0f);
        _rotateSpeedZ = Random.Range(10.0f, 100.0f);
    }
    
    void Update()
    {
        // timer to change the rotation speed
        if (Time.time > _futureTime)
        {
            UpdateColor();
            UpdateRotationSpeed();

            _futureTime = Time.time + Random.Range(0.5f, 2.0f);
            Debug.Log("Future Time: " + Time.time);
        }

        transform.Rotate(_rotateSpeedX * Time.deltaTime, _rotateSpeedY * Time.deltaTime, _rotateSpeedZ * Time.deltaTime);
    }
}
