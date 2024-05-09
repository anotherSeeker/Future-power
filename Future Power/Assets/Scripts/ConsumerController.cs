using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ConsumerController : MonoBehaviour
{

    //As is the number of consumers cannot change once the game is initialized, but generators can

    private List<consumerNode> consumers;//= new List<consumerNode>();
    private int numChildren = 0;
    private float power = 0f;

    private void Start() 
    {
        consumers = new List<consumerNode>();
        setupConsumers();
    }

    private void setupConsumers()
    {
        numChildren = gameObject.transform.childCount;
        for (int i = 0; i < numChildren; i++)
        {
            //get the group of consumers (eg Dom1 Group or Dom2 Group), then grab the consumers from that
            Transform conGroup = gameObject.transform.GetChild(i);
            
            var cons = conGroup.GetComponentsInChildren<consumerNode>().ToList();
            consumers.Concat(cons);
        }
    }

    public float GetRequestedPower()
    {
        power = 0;
        print(consumers.Count);
        //setupConsumers();
        foreach (consumerNode consumer in consumers)
        {
            power = power + consumer.getRequestedPower();
            print(power);
        }

        return power;
    }

}