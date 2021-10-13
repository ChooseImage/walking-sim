using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Light;

public class Light : MonoBehaviour
{
    public Light lt;


    //Range
    public bool changeRange = false;
    public float rangeSpeed = 1.0f;
    public float maxRange = 10.0f;

    //Intensity
    public bool changeIntensity = false;
    public float intensitySpeed = 1.0f;
    public float maxIntensity = 10.0f;

    //Color
    public bool changeColors = false;
    public float colorSpeed = 1.0f;
    public Color startColor;
    public Color endColor;

    float startTime;

    void Start()
    {
         lt = GetComponent<Light>();
         startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(changeRange){
            lt.range = Mathf.PingPong(Time.time * rangeSpeed, maxRange);
        }

        if(changeIntensity){
            lt.intensity = Mathf.PingPong(Time.time * intensitySpeed, maxIntensity);
        }

        if(changeColors){
            float t = (Mathf.Sin(Time.time - startTime * colorSpeed));
            lt.color = Color.Lerp(startColor, endColor, t); 
        }
    }
}
