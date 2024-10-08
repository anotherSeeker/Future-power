using TMPro;
using UnityEngine;

public class ConsumerNode : MonoBehaviour
{
    [SerializeField] private PowerCons consumer; 

    private bool consumerIsActive = false;

    void Start()
    {    }

    public float getRequestedPower()
    {
        if (consumerIsActive)
            return consumer.powerDraw;

        return 0f;
    }

    public void setState(bool state)
    {
        consumerIsActive = state;
    }

    public void toggleState()
    {
        consumerIsActive = !consumerIsActive;
    }

    public void onClick()
    {
        Debug.Log("ConsNode: "+name+"was clicked on");
        toggleState();
    }
}
