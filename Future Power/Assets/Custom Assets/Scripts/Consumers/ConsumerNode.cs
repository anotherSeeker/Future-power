using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ConsumerNode : MonoBehaviour
{
    [SerializeField] private PowerCons consumer; 
    [SerializeField] private MeshRenderer leverBaseMeshRenderer;
    [SerializeField] private Material ringMaterialOn;
    [SerializeField] private Material ringMaterialOff;
    [SerializeField] private Material ringMaterialRequired;
    [SerializeField] private String triggerName = "switchState";
    private Animator animController;

    private bool onState = false;

    private Color redCol = new Color(255, 14, 27);
    private Color greenCol = new Color(20, 255, 60);

    void Start()
    {  
        animController = transform.GetComponent<Animator>();
    }

    public float getRequestedPower()
    {
        if (onState)
            return consumer.powerDraw;

        return 0f;
    }

    public void setState(bool state)
    {
        this.onState = state;
    }

    private void SetColour()
    {
        List<Material> newMaterials = new List<Material>();
        leverBaseMeshRenderer.GetSharedMaterials(newMaterials);

        if (onState)
            newMaterials[1] = ringMaterialOn;
        else
            newMaterials[1] = ringMaterialOff;

        leverBaseMeshRenderer.SetMaterials(newMaterials);
        //Debug.Log("Bibbidi");
    }

    public void toggleState()
    {
        if (animController)
        {
            Debug.Log("spipp");
            animController.SetTrigger(triggerName);
        }
        setState(!onState);
        SetColour();
    }

    public void onClick()
    {
        Debug.Log("ConsNode: "+name+"was clicked on");
        toggleState();
    } 
}
