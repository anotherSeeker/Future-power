using System.Collections.Generic;
using UnityEngine;
public class GeneratorController3d : MonoBehaviour
{
    [SerializeField]private float power = 0;

    [SerializeField]List<Transform> children;

    void Start()
    {
        children = GetChildren(transform);
    }

    public void setupScenario(Scenario scenario)
    {
        foreach (Transform child in children)
        {
            if (child.GetComponent<GenNodeController3d>())
                child.GetComponent<GenNodeController3d>().populateDropdown(scenario);
        }
    }

    public bool hasTwoRenewables()
    {
        int count = 0;
        foreach (Transform child in children)
        {
            GenNodeController3d node = child.GetComponent<GenNodeController3d>();
            if (node)
            {
                if (node.isRenewable())
                    count++;
                
                if (count >= 2)
                    return true;
            }
        }

        return false;
    }

    public float GetCost()
    {
        float cost = 0;

        foreach (Transform child in children)
        {
            if (child.GetComponent<GenNodeController3d>())             
                cost += child.GetComponent<GenNodeController3d>().GetCost();
        }

        return cost;
    }

    public float GetGeneratorPower()
    {
        power = 0;
        
        foreach (Transform child in children)
        {   
            if (child.GetComponent<GenNodeController3d>())             
                power = power + child.GetComponent<GenNodeController3d>().GetCurrentPower();
        }

        return power;
    }

    private List<Transform> GetChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();

        foreach(Transform child in parent)
        {
            if (child.GetComponent<GenNodeController3d>())
                children.Add(child);
        }

        return children;
    }

    private void UpdateGenerators()
    {
        //update the list of children now that we've changed that list
        children = GetChildren(transform);
    }

    public void resetAll()
    {
        foreach (Transform child in children)
        {
            if (child.GetComponent<GenNodeController3d>())
                child.GetComponent<GenNodeController3d>().resetNode();
        }
    }

}