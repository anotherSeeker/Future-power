using UnityEngine;
using TMPro;
using Unity.Mathematics;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class GameBoard : MonoBehaviour  
{
    [SerializeField] private GeneratorController3d genController;
    [SerializeField] private ConsumerController3d conController;
    
    [SerializeField] private ScenarioSet scenarioSetA;
    [SerializeField] private ScenarioSet scenarioSetB;

    [SerializeField] float overloadBuffer = 50f;
    [SerializeField] float overloadRatio = 0.1f;
    [SerializeField] private TextMeshProUGUI descriptionFront;
    [SerializeField] private TextMeshProUGUI descriptionBack;
    [SerializeField] private Button winButton;
    [SerializeField] private GameObject winTextPanel;
    [SerializeField] private TextMeshProUGUI winText;

    [Header("Power Generation")]
    [SerializeField] private float genPower = 0f;
    [SerializeField] private float reqPower = 0f;
    [SerializeField] bool overloaded = false;
    [SerializeField] bool requires2Renewables = false;

    private ScenarioSet setInUse;
    List<float> savedScores = new List<float>();

    void Start()
    {
        descriptionFront.text = "Requested Power: "+reqPower.ToString("F2")+"\nGenerated Power: "+genPower.ToString("F2");
        descriptionBack.text = "Requested Power: "+reqPower.ToString("F2")+"\nGenerated Power: "+genPower.ToString("F2");
    }

    void Update()
    {
        genPower = genController.GetGeneratorPower();
        reqPower = conController.GetRequestedPower();

        descriptionFront.text = "Requested Power: "+reqPower.ToString("F2")+"\nGenerated Power: "+genPower.ToString("F2");
        descriptionBack.text = "Requested Power: "+reqPower.ToString("F2")+"\nGenerated Power: "+genPower.ToString("F2");

        overloaded = checkOverloaded();
        if (overloaded)
        {
            descriptionFront.text = "Grid Overload Reset Required\nReq.: "+reqPower.ToString("F2")+" Gen.: "+genPower.ToString("F2");
            descriptionBack.text = "Grid Overload Reset Required\nReq.: "+reqPower.ToString("F2")+" Gen.: "+genPower.ToString("F2");
        }

        calculateScore();
    }

    public void winScenario()
    {
        //save our score to be displayed later
        float score = calculateScore();
        savedScores.Add(score);

        //update to the next scenario, reset the game board and initalise the new scenario
        if (setInUse.stepScenarioIndex() == -1) //increments which individual scenario we're using within the list
        {
            //if we received -1 we're out of scenarios to tackle and can display the final score
            setCompleted();
        }
        else
        {
            Scenario newScenario = setInUse.GetCurrentScenario();
            
            conController.resetAll();
            genController.resetAll();

            genController.setupScenario(newScenario);
            conController.setupScenario(newScenario);

            requires2Renewables = newScenario.requires2Renewables;
        }
    }

    private void setCompleted()
    {
        String scoreString = "";
        int count = 0; 
        for (int i = 0; i<savedScores.Count; i++)
        {
            count++;
            float score = savedScores[i];

            scoreString += "Scenario "+i+" Score: "+score.ToString()+"\n";
        }

        winTextPanel.SetActive(true);
        winText.text = "Congratulations!\nYou Completed "+count+" Scenarios!\n\nIf you'd like to try the other set of challenges click the button below!\n\n"+scoreString;
    }

    public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void playSetA()
    {
        setInUse = scenarioSetA;
        Scenario newScenario = setInUse.GetCurrentScenario();

        genController.setupScenario(newScenario);
        conController.setupScenario(newScenario);

        requires2Renewables = newScenario.requires2Renewables;
    }

    public void playSetB()
    {
        setInUse = scenarioSetB;
        Scenario newScenario = setInUse.GetCurrentScenario();

        genController.setupScenario(newScenario);
        conController.setupScenario(newScenario);

        requires2Renewables = newScenario.requires2Renewables;
    }


    private bool checkOverloaded()
    {
        //if difference between our request and generation is > our buffer + overloadRatio*genPower we overload
        //The more power the system currently has the more generous we get so our ratio is small
        if (math.abs(genPower-reqPower) > overloadBuffer + overloadRatio*genPower)
        {
            return true;
        }        

        //if we are overloaded AND our genPower AND reqPower are not 0 we stay overloaded, both must be 0 to reset the system
        if (overloaded && !(genPower == 0 && reqPower == 0))
        {
            return true;
        }

        //neither of our conditions are true so we are not overloaded
        return false;
    }


    private float calculateScore()
    {
        float score;
        //current scoring algorithm is 
            // for each generator cost = initialCost+perWattCost*powerGenerated
            // add each cost together, do (1000 - (requestedPower-generatedPower)) / totalCost and you want to maximise this number
        float totalCost = genController.GetCost();
        score = 100*(reqPower / totalCost);

        //if we have all the required consumers activated, we are not overloaded and we requesting less power than we are generating
        if (conController.canWin && !overloaded && reqPower - genPower <= 0)
        {
            //if we require 2 renewables and don't have that we cannot win
            if (requires2Renewables) 
            {
                if (!genController.hasTwoRenewables())
                {
                    descriptionFront.text = "Scenario Requires 2 Renewables\nReq.: "+reqPower.ToString("F2")+" Gen.: "+genPower.ToString("F2");
                    descriptionBack.text = "Scenario Requires 2 Renewables\nReq.: "+reqPower.ToString("F2")+" Gen.: "+genPower.ToString("F2");
                    winButton.gameObject.SetActive(false);
                    return score;
                }
            }
                //we are allowed to save this score and win the game
                descriptionFront.text = "Score: "+score.ToString("F2")+"\nReq.: "+reqPower.ToString("F2")+" Gen.: "+genPower.ToString("F2");
                descriptionBack.text = "Score: "+score.ToString("F2")+"\nReq.: "+reqPower.ToString("F2")+" Gen.: "+genPower.ToString("F2");

                winButton.gameObject.SetActive(true);           
        }
        else
            winButton.gameObject.SetActive(false);

        return score;
    }
}