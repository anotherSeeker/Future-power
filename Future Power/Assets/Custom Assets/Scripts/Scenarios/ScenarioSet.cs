using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScenarioSet : MonoBehaviour
{
    [SerializeField] private List<Scenario> Set;

    private int currentIndex = 0;

    public Scenario GetCurrentScenario() 
    {
        return Set[currentIndex];
    }

    public int stepScenarioIndex()
    {
        currentIndex++;

        if (currentIndex >= Set.Count)
        {
            //we've done every scenario in the list and can use an output of -1 to say we should stop looking for a new scenario
            return -1;
        }

        return currentIndex;
    }
}
