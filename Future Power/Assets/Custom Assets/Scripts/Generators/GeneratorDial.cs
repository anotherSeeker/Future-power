using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GeneratorDial : MonoBehaviour
{
    [SerializeField] private float currTargetGeneration = 0f;
    [SerializeField] private float baseRotationSpeed = 1f;
    [SerializeField] private float slowSpinMultiplier = 0.1f;
    [SerializeField] private int numRotationsForFullPower = 4;

    //max value of 1 min of 0; we'll increment by a value depending on if we're 
    public void SpinDial(bool isSlowSpin, float distance)
    {
        //let's say that from 0 - 1 is four rotations. to remap that we'll want 4*360 degrees
        float maximumSpin = numRotationsForFullPower * 360;
       
        if (distance != 0)
        {
            float max = 500f;
            float min = -max;
            
            float newMax = baseRotationSpeed;
            float newMin = -newMax;

            float stepDistance = (distance-min) / (max-min) * (newMax-newMin) + newMin;
            float newTarget = currTargetGeneration+stepDistance;

            currTargetGeneration += (newTarget-currTargetGeneration)*Time.deltaTime;
            currTargetGeneration = Mathf.Clamp(currTargetGeneration, 0,1);

            float desiredRotation;
            desiredRotation = maximumSpin*currTargetGeneration;

            transform.rotation = Quaternion.Euler(desiredRotation,0,0);
        }
    }

    /*public void SpinDialOld(bool isSlowSpin, float distance)
    {
        //let's say that from 0 - 1 is four rotations. to remap that we'll want 4*360 degrees
        float maximumSpin = numRotationsForFullPower * 360;
       
        if (distance != 0)
        {
            float max = 500f;
            float min = -max;
            
            float newMax = baseRotationSpeed;
            float newMin = -newMax;

            float stepDistance = (distance-min) / (max-min) * (newMax-newMin) + newMin;
            float newTarget = currTargetGeneration+stepDistance;

            currTargetGeneration += (newTarget-currTargetGeneration)*Time.deltaTime;
            currTargetGeneration = Mathf.Clamp(currTargetGeneration, 0,1);

            float desiredRotation;
            desiredRotation = maximumSpin*currTargetGeneration;

            transform.rotation = Quaternion.Euler(desiredRotation,0,0);
        }
    }*/

    public float GetTargetGeneration()
    {
        return currTargetGeneration;
    }
    
    public void ResetDial()
    {
        currTargetGeneration = 0f;
        transform.rotation = Quaternion.Euler(0,0,0);
    }
}


//             float generationUpdate;
//             float newTargetGeneration;
            
//             generationUpdate = distance*baseRotationSpeed;
//             newTargetGeneration = currTargetGeneration+generationUpdate;
            

//             newTargetGeneration = spinSmooth(currTargetGeneration, newTargetGeneration, 1);

            
//             float currentSpin = maximumSpin / newTargetGeneration;
            
//             Quaternion newRot = Quaternion.Euler(currentSpin,0f,0f);
//             transform.rotation = newRot;