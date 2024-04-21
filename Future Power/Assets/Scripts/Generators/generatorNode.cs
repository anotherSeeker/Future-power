using System;
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

    void Start()
    {
        maxPower = generator.genCapacity;
        responseSpeed = generator.responseSpeed;
    }
    
    void Update()
    {
        updatePower();
    }

    void updatePower()
    {
        desiredGeneration = maxPower * targetGeneration;
        currentGeneration = Mathf.MoveTowards(currentGeneration, desiredGeneration, responseSpeed * Time.deltaTime);
    }
}
