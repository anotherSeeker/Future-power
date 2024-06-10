using TMPro;
using UnityEngine;

public class ConsumerNode : MonoBehaviour
{
    [SerializeField] private PowerCons consumer; 

    private bool toggleState = false;

    public TextMeshProUGUI genName;
    public TextMeshProUGUI genDescription;

    void Start()
    {
        genName.text = consumer.ConsumerName;
        genDescription.text = consumer.powerDraw+"MW"; 
    }

    public float getRequestedPower()
    {
        if (toggleState)
            return consumer.powerDraw;

        return 0f;
    }

    //Called when the toggle button is pressed
    public void setState(bool state)
    {
        toggleState = state;
        genDescription.text = consumer.powerDraw+"MW"; 
    }
}
