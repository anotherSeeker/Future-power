using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GenNodeController3d : MonoBehaviour
{
    [SerializeField] private List<GameObject> generators;
    public List<GameObject> workingGenerators;
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
        workingGenerators = generators;
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

    /*public void populateDropdown() 
    {
        dropdown.ClearOptions();
        workingGenerators = new List<GameObject>();

        foreach(GameObject gen in generators)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(gen.GetComponent<GeneratorNode3d>().GetName(), gen.GetComponent<GeneratorNode3d>().GetSprite()));
            workingGenerators.Add(gen);
        }

        defaultDropdown();
    }*/

    public void populateDropdown(Scenario scenario)
    {
        dropdown.ClearOptions();
        workingGenerators = new List<GameObject>();

        foreach(GameObject gen in generators)
        {
            bool addToDropdown = true;
            string name = gen.GetComponent<GeneratorNode3d>().GetName();

            //an addittedly bad setup to do this check, if there were more time I would rewrite this to be much cleaner
            if (scenario.banCoal && name == "Coal")
            {
                addToDropdown = false;
            }
            else if (scenario.banGas && name == "Gas")
            {
                addToDropdown = false;
            }
            else if (scenario.banNuclear && name == "Nuclear")
            {
                addToDropdown = false;
            }
            else if (scenario.banSolar && name == "Solar")
            {
                addToDropdown = false;
            }
            else if (scenario.banWind && name == "Wind")
            {
                addToDropdown = false;
            }
            else if (scenario.banHydro && name == "Hydro")
            {
                addToDropdown = false;
            }



            if (addToDropdown)
            {            
                dropdown.options.Add(new TMP_Dropdown.OptionData(gen.GetComponent<GeneratorNode3d>().GetName(), gen.GetComponent<GeneratorNode3d>().GetSprite()));
                workingGenerators.Add(gen);
            }
        }

        defaultDropdown();
    }
    
    private void defaultDropdown()
    {        
        dropdown.value = 0;
        ChangeNode();
        
        dropdown.Show();
        dropdown.Hide();
    }

    public void ChangeNode()
    {
        //Called OnValueChanged()
        int newChild = dropdown.value;

        //zero out the dial
        GenDial.ResetDial();

        //zero out power generation
        desiredGeneration = 0;
            currentGeneration = 0;

        //delete old generator
        GameObject childGenerator = currentGenerator;
        Destroy(childGenerator);

        //make new generator
        currentGenerator = Instantiate(workingGenerators[newChild], transform);

        //hide dropdown
        dropdown.Hide();
        dropdown.gameObject.SetActive(false);
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

    /*private Transform GetCurrentGen(Transform parent)
    {
        foreach(Transform child in parent)
        {
            if (child.GetComponent<GeneratorNode3d>())
                return child;
        }

        return null;
    }*/

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
