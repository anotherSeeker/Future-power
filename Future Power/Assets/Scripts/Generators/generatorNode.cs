using TMPro;
using UnityEngine;


public class powerGeneratorNode : MonoBehaviour
{
    [SerializeField] private powerGen generator;
    private float maxPower;
    private float responseSpeed;

    //num between 0 and 1 determining our desired power output
    [SerializeField] private float targetGeneration = 0f;

    //number respreseting the flat targeted value
    private float desiredGeneration = 0f;

    //num between 0 and 1 determining our current power output
    [SerializeField] private float currentGeneration = 0f;

    public TextMeshProUGUI genName;
    public TextMeshProUGUI genDescription;

    void Start()
    {
        maxPower = generator.genCapacity;
        responseSpeed = generator.responseSpeed;

        genName.text = generator.GenName;
        genDescription.text = "Target Power: "+desiredGeneration+"\nCurrent Power: "+currentGeneration; 
    }
    
    void Update()
    {
        updatePower();
    }

    void updatePower()
    {
        desiredGeneration = maxPower * targetGeneration;
        currentGeneration = Mathf.MoveTowards(currentGeneration, desiredGeneration, responseSpeed * Time.deltaTime);
        genDescription.text = "Target Power: "+desiredGeneration+"\nCurrent Power: "+currentGeneration;
    }

    //Called when we change the target power slider on the ui
    public void updateTargetPower(float newTarget)
    {
        targetGeneration = newTarget;
    }
}
