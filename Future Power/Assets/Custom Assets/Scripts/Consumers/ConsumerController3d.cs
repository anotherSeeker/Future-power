using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class ConsumerController3d : MonoBehaviour
{
    //As is the number of consumers cannot change once the game is initialized, but generators can

    [SerializeField] private List<Transform> consumerGroups;
    [SerializeField] private List<Transform> consumers;
    [SerializeField] private float power = 0f;
    public bool canWin = false;

    private void Start() 
    {
        //add the consumer groups
        consumerGroups = GetChildren(transform);

        //add the actual consumers
        foreach (Transform group in consumerGroups)
        {
            consumers.AddRange(GetChildren(group));
        }
    }

    void Update()
    {
        canWin = allRequiredActive();
    }

    public float GetRequestedPower()
    {
        //This is different to the generator method as our consumers are nested within "groups" dom1 group -> 6 dom1 consumer nodes
        power = 0;
        foreach (Transform consumer in consumers)
        {
            if (consumer.GetComponent<ConsumerNode>())
                power = power + consumer.GetComponent<ConsumerNode>().getRequestedPower();
        }

        return power;
    }

    public void resetAll()
    {
        foreach (Transform consumer in consumers)
        {
            //technically you could have a consumer without the correct component and cause a crash here. We simply ignore it if it's lacking a component
            if (consumer.GetComponent<ConsumerNode>())
                consumer.GetComponent<ConsumerNode>().resetNode();
        }
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

    private bool allRequiredActive()
    {
        bool state = true;
        foreach (Transform consumer in consumers)
        {
            ConsumerNode node = consumer.GetComponent<ConsumerNode>();
            if (node && (!node.getState() && node.required))
            {
                //the node is required but not on we cannot win until all required nodes are active
                state = false;
                return state;
            }
        }
        return state;
    }

    public void setupScenario(Scenario scenario)
    {
        //reset all "required" states
        foreach (Transform consumer in consumers)
        {
            consumer.GetComponent<ConsumerNode>().required = false;
        }

        //set required required states
        foreach (Transform group in consumerGroups)
        {
            switch (group.tag)
            {
                case  "Domestic 1":
                    flagRequired(group, scenario.numDomestic1);
                    break;
                case  "Domestic 2":
                    flagRequired(group, scenario.numDomestic2);
                    break;
                case  "Light Industry":
                    flagRequired(group, scenario.numLightInd);
                    break;
                case  "Heavy Industry":
                    flagRequired(group, scenario.numHeavyInd);
                    break;
                case  "Commercial":
                    flagRequired(group, scenario.numCommercial);
                    break;
                case  "Services":
                    flagRequired(group, scenario.numServices);
                    break;
                default:
                    break;
            }
        }
    }

    private void flagRequired(Transform group, int num)
    {
        int count = 0;
        foreach (Transform child in group)
        {
            if (count == num)
                return;

            if (child.GetComponent<ConsumerNode>())
            {
                //makes the node a required node
                child.GetComponent<ConsumerNode>().required = true;
                count++;
            }
        }
    }


}