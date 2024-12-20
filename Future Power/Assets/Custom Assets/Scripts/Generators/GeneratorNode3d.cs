using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GeneratorNode3d : MonoBehaviour
{
    [SerializeField] private PowerGen generator;
    [SerializeField] private bool noGenerator;
    private float maxPower;
    private float responseSpeed;
    [SerializeField] private Sprite genSprite;

    //num between 0 and 1 determining our desired power output NOW HANDLED BY THE NODE CONTROLLER
    //[SerializeField] private float targetGeneration = 0f;

    //number respreseting the flat targeted value
    private float desiredGeneration = 0f;

    //num between 0 and 1 determining our current power output
    [SerializeField] private float currentGeneration = 0f; 

    //private string genDescription;

    void Start()
    {
        if (!noGenerator)
        {
            maxPower = generator.genCapacity;
            responseSpeed = generator.responseSpeed;

            //genDescription = "Target Power: "+desiredGeneration.ToString("F2")+"\nCurrent Power: "+currentGeneration.ToString("F2"); 
        }
    }
    
    // void Update()
    // {
    //     //NodeController and Dial handle power now, node merely holds data
    //     //if (!noGenerator)
    //         //updatePower();
    // }

    /*void updatePower()
    {
        desiredGeneration = maxPower * targetGeneration;
        currentGeneration = Mathf.MoveTowards(currentGeneration, desiredGeneration, responseSpeed * Time.deltaTime);
    }*/

    public TMP_Dropdown onClick(TMP_Dropdown activeDropdown)
    {
        if (GetComponentInParent<GenNodeController3d>())
        {
            try
            {
                TMP_Dropdown dropdown = transform.parent.GetComponent<GenNodeController3d>().dropdown;

                activeDropdown = dropdown;

                dropdown.gameObject.SetActive(true);
                dropdown.Show();

                return activeDropdown;
            }
            catch (System.Exception ex)
            {
                 Debug.Log(ex);
                 if (activeDropdown)
                    return activeDropdown;
            }
        }
        return null;
    }

    //Called when we change the target power slider on the ui
    /*public void updateTargetPower(float newTarget)
    {
        targetGeneration = newTarget;
    }*/

    /*public float getCurrentPower()
    {
        return currentGeneration;
    }*/

    public bool isNoGen()
    {
        return noGenerator;
    }

    public float getInitialCost()
    {
        return generator.initialCost;
    }

    public float getPerWattCost()
    {
        return generator.perWattCost;
    }

    public float getMaxPower()
    {
        return maxPower;
    }

    public string GetName()
    {
        return generator.GenName;
    }
    public string GetDescription()
    {
        return "Generating: "+desiredGeneration.ToString("F2")+" \\ "+currentGeneration.ToString("F2");
    }
    public float getResponseSpeed()
    {
        return responseSpeed;
    }
    public Sprite GetSprite()
    {
        return genSprite;
    }
    public void resetNode()
    {
        desiredGeneration = 0;
        currentGeneration = 0;
    }
}
