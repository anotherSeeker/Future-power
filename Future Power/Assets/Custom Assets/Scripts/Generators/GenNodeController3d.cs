using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GenNodeController3d : MonoBehaviour
{
    [SerializeField] public List<GameObject> generators;
    private GeneratorDial GenDial;
    [SerializeField] public TMP_Dropdown dropdown;
    [SerializeField] private GameObject currentGenerator;

    void Awake()
    {
        setupGenDial();
        populateDropdown();
        //sets the dropdown to the default value, index 0 in the list
        defaultDropdown();
    }


    public void populateDropdown()
    {
        foreach(GameObject gen in generators)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(gen.GetComponent<GeneratorNode3d>().GetName(), gen.GetComponent<GeneratorNode3d>().GetSprite()));
        }
    }
    
    private void defaultDropdown()
    {
        //dropdown.GetComponentInParent<Canvas>().worldCamera = Camera.main;  
        
        dropdown.value = 0;
        ChangeNode();
        
        dropdown.Show();
    }

    public void ChangeNode()
    {
        //Called OnValueChanged()
        int newChild = dropdown.value;

        //zero out the dial
        GenDial.ResetDial();

        Transform childGenerator = GetCurrentGen(transform);
        Destroy(childGenerator.gameObject);

        currentGenerator = Instantiate(generators[newChild], transform);

        GameObject canvas = dropdown.transform.parent.gameObject;
        canvas.SetActive(false);
        

        Debug.Log(canvas.name+"false");
    }

    private Transform GetCurrentGen(Transform parent)
    {
        List<Transform> children = new List<Transform>();

        foreach(Transform child in parent)
        {
            if (child.GetComponent<GeneratorNode3d>())
                return child;
        }

        return null;
    }

    public float GetCurrentPower()
    {
        return GetMaxPower()*GetTargetGeneration();
    }

    public float GetTargetGeneration()
    {
        return GenDial.GetTargetGeneration();
    }

    public float GetMaxPower()
    {
        List<Transform> children = GetChildren(transform);
        float power = 0;

        foreach (Transform child in children)
        {   
            if (child.GetComponent<GeneratorNode3d>())
            {
                currentGenerator = child.gameObject;             
                power = power + child.GetComponent<GeneratorNode3d>().getMaxPower();
            }
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

    private void setupGenDial()
    {
        foreach(Transform child in transform)
        {
            if (child.GetComponent<GeneratorDial>())
                GenDial = child.GetComponent<GeneratorDial>();
        }
    }

    public void reset()
    {
        GenDial.ResetDial();
        
    }
}
