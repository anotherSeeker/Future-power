using UnityEngine;

[CreateAssetMenu(fileName = "ScenarioData", menuName = "Scenario Data", order = 0)]
 
public class Scenario : ScriptableObject
{
    [SerializeField] public string GenName = "Default Scenario";

    //number of consumers of each type that much be powered on and have sufficient power to run
    [SerializeField] public int numDomestic1 = 1; 
    [SerializeField] public int numDomestic2 = 1; 
    [SerializeField] public int numLightInd = 1;
    [SerializeField] public int numCommercial = 1;  
    [SerializeField] public int numHeavyInd = 1; 
    [SerializeField] public int numServices = 1; 
 

    [SerializeField] public bool banSolar = false; 
    [SerializeField] public bool banHydro = false; 
    [SerializeField] public bool banWind = false; 
    [SerializeField] public bool banNuclear = false; 
    [SerializeField] public bool banCoal = false; 
    [SerializeField] public bool banGas = false; 
    [SerializeField] public bool requires2Renewables = false; 
}
