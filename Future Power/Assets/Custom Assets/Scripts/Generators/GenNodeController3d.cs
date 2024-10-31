using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;

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
        //populateDropdown();
    }

    void Update()
    {
        updatePower();
    }

    public bool isRenewable()
    {
        //should realistically put a property on the generator type that we can reference instead
        if (currentGenerator.GetComponent<GeneratorNode3d>().GetName() == "Solar")
        {
            return true;
        }
        else if (currentGenerator.GetComponent<GeneratorNode3d>().GetName() == "Wind")
        {
            return true;
        }
        else if (currentGenerator.GetComponent<GeneratorNode3d>().GetName() == "Hydro")
        {
            return true;
        }

        return false;
    }

    public float GetCost()
    {
        GeneratorNode3d node = currentGenerator.GetComponent<GeneratorNode3d>();
        if (node)
            return node.getInitialCost() + currentGeneration * node.getPerWattCost();

        return 0f;
    }

    private void updatePower()
    {
        desiredGeneration = GetMaxPower() * GetTargetGeneration();
        currentGeneration = Mathf.MoveTowards(currentGeneration, desiredGeneration, GetResponseSpeed() * Time.deltaTime);
        descriptionUi.text = "Target:  "+desiredGeneration.ToString("F2")+"\nCurrent:"+currentGeneration.ToString("F2");
    }

    public void populateDropdown()
    {
        dropdown.ClearOptions();

        foreach(GameObject gen in generators)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(gen.GetComponent<GeneratorNode3d>().GetName(), gen.GetComponent<GeneratorNode3d>().GetSprite()));
        }

        defaultDropdown();
    }

    public void populateDropdown(Scenario scenario)
    {
        foreach(GameObject gen in generators)
        {
            bool addToDropdown = true;

            //an addittedly bad setup to do this check, if there were more time I would rewrite this to be much cleaner
            if (scenario.banCoal && gen.GetComponent<GeneratorNode3d>().GetName() == "Coal")
            {
                addToDropdown = false;
            }
            else if (scenario.banGas && gen.GetComponent<GeneratorNode3d>().GetName() == "Gas")
            {
                addToDropdown = false;
            }
            else if (scenario.banNuclear && gen.GetComponent<GeneratorNode3d>().GetName() == "Nuclear")
            {
                addToDropdown = false;
            }
            else if (scenario.banSolar && gen.GetComponent<GeneratorNode3d>().GetName() == "Solar")
            {
                addToDropdown = false;
            }
            else if (scenario.banWind && gen.GetComponent<GeneratorNode3d>().GetName() == "Wind")
            {
                addToDropdown = false;
            }
            else if (scenario.banHydro && gen.GetComponent<GeneratorNode3d>().GetName() == "Hydro")
            {
                addToDropdown = false;
            }



            if (addToDropdown)            
                dropdown.options.Add(new TMP_Dropdown.OptionData(gen.GetComponent<GeneratorNode3d>().GetName(), gen.GetComponent<GeneratorNode3d>().GetSprite()));
        }

        defaultDropdown();
    }
    
    private void defaultDropdown()
    {        
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
