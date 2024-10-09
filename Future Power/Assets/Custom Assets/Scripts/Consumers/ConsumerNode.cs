using TMPro;
using UnityEngine;

public class ConsumerNode : MonoBehaviour
{
    [SerializeField] private PowerCons consumer; 
    [SerializeField] private Material ringMaterial;

    private bool consumerIsActive = false;

    private Color redCol = new Color(255, 14, 27);
    private Color greenCol = new Color(20, 255, 60);

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

    private void SetColour()
    {
        if (ringMaterial)
        {
            if (consumerIsActive)
                ringMaterial.color = greenCol;
            else
                ringMaterial.color = redCol;
        }
    }

    public void toggleState()
    {
        consumerIsActive = !consumerIsActive;

        SetColour();
    }

    public void onClick()
    {
        Debug.Log("ConsNode: "+name+"was clicked on");
        toggleState();
    } 
}
