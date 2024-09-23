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

    public float GetGeneratorPower()
    {
        power = 0;
        
        foreach (Transform child in children)
        {   
            if (child.GetComponent<GenNodeController3d>())             
                power = power + child.GetComponent<GenNodeController3d>().getCurrentPower();
        }

        return power;
    }

    private List<Transform> GetChildren(Transform parent)
    {
        List<Transform> children = new List<Transform>();

        foreach(Transform child in parent)
        {
            children.Add(child);
        }

        return children;
    }

    private void UpdateGenerators(GameObject newGenerator, GameObject targetNode)
    {

        //update the list of children now that we've changed that list
        children = GetChildren(transform);
    }

}