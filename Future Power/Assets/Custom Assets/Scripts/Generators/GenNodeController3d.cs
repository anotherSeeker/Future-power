using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GenNodeController3d : MonoBehaviour
{
    [SerializeField] public List<GameObject> generators;
    private GeneratorDial GenDial;
    [SerializeField] public TMP_Dropdown dropdown;
    [SerializeField] public TMP_Text descriptionUi;
    [SerializeField] private GameObject currentGenerator;
    //number between 0 and 1, denotes a percentage of the current generators maximum power
    float desiredGeneration;
    float currentGeneration;

    void Awake()
    {
        setupGenDial();
        populateDropdown();
        //sets the dropdown to the default value, index 0 in the list
        defaultDropdown();
    }

    void Update()
    {
        updatePower();
    }

    void updatePower()
    {
        desiredGeneration = GetMaxPower() * GetTargetGeneration();
        currentGeneration = Mathf.MoveTowards(currentGeneration, desiredGeneration, GetResponseSpeed() * Time.deltaTime);
        descriptionUi.text = "Target:  "+desiredGeneration.ToString("F2")+"\nCurrent:"+currentGeneration.ToString("F2");
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
        

        Debug.Log(canvas.name+" isActive: "+canvas.activeSelf);
    }

    public float GetCurrentPower()
    {
        return currentGeneration;
    }
    public float GetResponseSpeed()
    {
        return currentGenerator.GetComponent<GeneratorNode3d>().getResponseSpeed();
    }

    public float GetTargetGeneration()
    {
        return GenDial.GetTargetGeneration();
    }

    private Transform GetCurrentGen(Transform parent)
    {
        foreach(Transform child in parent)
        {
            if (child.GetComponent<GeneratorNode3d>())
                return child;
        }

        return null;
    }

    public float GetMaxPower()
    {
        return currentGenerator.GetComponent<GeneratorNode3d>().getMaxPower();
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

    public void resetNode()
    {
        GenDial.ResetDial();
        
    }
}
