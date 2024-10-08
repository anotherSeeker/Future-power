using TMPro;
using UnityEngine;

public class GeneratorNode3d : MonoBehaviour
{
    [SerializeField] private powerGen generator;
    [SerializeField] private bool noGenerator;
    private float maxPower;
    private float responseSpeed;
    [SerializeField] private TMP_SpriteAsset genSprite;

    //num between 0 and 1 determining our desired power output
    [SerializeField] private float targetGeneration = 0f;

    //number respreseting the flat targeted value
    private float desiredGeneration = 0f;

    //num between 0 and 1 determining our current power output
    [SerializeField] private float currentGeneration = 0f; 

    private string genName;
    private string genDescription;

    void Start()
    {
        if (!noGenerator)
        {
            maxPower = generator.genCapacity;
            responseSpeed = generator.responseSpeed;

            genName = generator.GenName;
            genDescription = "Target Power: "+desiredGeneration.ToString("F2")+"\nCurrent Power: "+currentGeneration.ToString("F2"); 
        }
    }
    
    void Update()
    {
        if (!noGenerator)
            updatePower();
    }

    void updatePower()
    {
        desiredGeneration = maxPower * targetGeneration;
        currentGeneration = Mathf.MoveTowards(currentGeneration, desiredGeneration, responseSpeed * Time.deltaTime);
        genDescription = "Target Power: "+desiredGeneration.ToString("F2")+"\nCurrent Power: "+currentGeneration.ToString("F2");
    }

    public void onClick()
    {
        Debug.Log("GenNode: "+genName+"was clicked on");
    }

    //Called when we change the target power slider on the ui
    public void updateTargetPower(float newTarget)
    {
        targetGeneration = newTarget;
    }

    public float getCurrentPower()
    {
        return currentGeneration;
    }

    public string GetName()
    {
        return generator.GenName;
    }
}
