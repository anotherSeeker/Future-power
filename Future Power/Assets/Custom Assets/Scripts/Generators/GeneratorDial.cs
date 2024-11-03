using UnityEngine;

public class GeneratorDial : MonoBehaviour
{
    [SerializeField] private float currTargetGeneration = 0f;
    [SerializeField] private float baseRotationSpeed = 1f;
    [SerializeField] private float slowSpinMultiplier = 0.1f;
    [SerializeField] private int numRotationsForFullPower = 4;

    void Update()
    {

    }

    //max value of 1 min of 0; we'll increment by a value depending on if we're 
    public void SpinDial(bool isSlowSpin, float distance)
    {
        //let's say that from 0 - 1 is four rotations. to remap that we'll want 4*360 degrees
        float maximumSpin = numRotationsForFullPower * 360;
       
        if (distance != 0)
        {
            float speed = baseRotationSpeed;
            if (isSlowSpin)
                speed*=slowSpinMultiplier;

            float max = 500f;
            float min = -max;
            
            float newMax = speed;
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

    public float GetTargetGeneration()
    {
        return currTargetGeneration;
    }
    
    public void ResetDial()
    {
        currTargetGeneration = 0f;
        transform.rotation = Quaternion.Euler(0,0,0);
    }
    public void selected()
    {
        //turns on the dial glow when clicked
        Material dialMat = GetComponent<Renderer>().material;
        if (dialMat.HasFloat("_Glow"))
        {
            dialMat.SetFloat("_Glow",1);
        }
    }
    public void deselected()
    {
        //turns off the dial glow when released
        Material dialMat = GetComponent<Renderer>().material;
        if (dialMat.HasFloat("_Glow"))
        {
            dialMat.SetFloat("_Glow",0);
        }
    }
}
