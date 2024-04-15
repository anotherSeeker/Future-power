using UnityEngine;

public class powerGen : MonoBehaviour
{
    [SerializeField] private float genCapacity;
    [SerializeField] private float responseSpeed = 1f;

    //num between 0 and 1 determining our desired power output
    [SerializeField] private float targetGeneration = 0f;

    //num between 0 and 1 determining our current power output
    [SerializeField] private float currentGeneration = 0f;
}
