using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GenNodeController3d : MonoBehaviour
{
    [SerializeField] public List<GameObject> generators;
    [SerializeField] public TMP_Dropdown dropdown;

    void Start()
    {
        populateDropdown();

        //sets the dropdown to the default value, index 0 in the list
        defaultDropdown();
    }


    public void populateDropdown()
    {
        foreach(GameObject gen in generators)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(gen.GetComponent<GeneratorNode3d>().GetName(), null));
        }
    }
    
    private void defaultDropdown()
    {
        dropdown.value = 0;
        ChangeNode();
    }

    public void ChangeNode()
    {
        //Called OnValueChanged()
        int newChild = dropdown.value;

        Transform childGenerator = GetCurrentGen(transform);
        Destroy(childGenerator.gameObject);

        Instantiate(generators[newChild], transform);
    }

    private Transform GetCurrentGen(Transform parent)
    {
        List<Transform> children = new List<Transform>();

        foreach(Transform child in parent)
        {
            children.Add(child);
            if (child.GetComponent<GeneratorNode3d>())
                return child;
        }

        return null;
    }



    public float getCurrentPower()
    {
        List<Transform> children = GetChildren(transform);
        float power = 0;

        foreach (Transform child in children)
        {   
            if (child.GetComponent<GeneratorNode3d>())             
                power = power + child.GetComponent<GeneratorNode3d>().getCurrentPower();
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
}
