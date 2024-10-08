using System.Collections.Generic;
using UnityEngine;
public class ConsumerController3d : MonoBehaviour
{
    //As is the number of consumers cannot change once the game is initialized, but generators can

    [SerializeField] private List<Transform> children;

    [SerializeField] private float power = 0f;

    private void Start() 
    {
        children = GetChildren(transform);
    }

    public float GetRequestedPower()
    {
        //This is different to the generator method as our consumers are nested within "groups" dom1 group -> 6 dom1 consumer nodes
        power = 0;
        List<Transform> consumers;

        foreach (Transform child in children)
        {
            consumers = GetChildren(child);
            foreach (Transform consumer in consumers)
            {
                if (consumer.GetComponent<ConsumerNode>())
                    power = power + consumer.GetComponent<ConsumerNode>().getRequestedPower();
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
}