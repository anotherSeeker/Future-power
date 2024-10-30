using System.Collections.Generic;
using UnityEngine;
public class ConsumerController3d : MonoBehaviour
{
    //As is the number of consumers cannot change once the game is initialized, but generators can

    [SerializeField] private List<Transform> consumerGroups;
    [SerializeField] private List<Transform> consumers;

    [SerializeField] private float power = 0f;

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
}