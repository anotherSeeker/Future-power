using System;
using System.Collections.Generic;
using UnityEngine;

public class ConsumerNode : MonoBehaviour
{
    [SerializeField] private PowerCons consumer; 
    [SerializeField] public bool required = false;
    [SerializeField] private MeshRenderer leverBaseMeshRenderer;
    [SerializeField] private Material ringMaterialOn;
    [SerializeField] private Material ringMaterialOff;
    [SerializeField] private Material ringMaterialRequired;
    [SerializeField] private String toggleTrigger = "changeState";
    [SerializeField] private String resetTrigger = "Reset";
    private Animator animController;

    private bool onState = false;

    void Start()
    {  
        animController = transform.GetComponent<Animator>();
    }

    void Update()
    {
        SetColour();
    }

    public float getRequestedPower()
    {
        if (onState)
            return consumer.powerDraw;

        return 0f;
    }

    public bool getState()
    {
        return onState;
    }

    public void setState(bool state)
    {
        this.onState = state;
    }

    private void SetColour()
    {
        //swap ring colours
        List<Material> newMaterials = new List<Material>();
        leverBaseMeshRenderer.GetSharedMaterials(newMaterials);

        if (onState)
            newMaterials[1] = ringMaterialOn;
        else if (required)
            newMaterials[1] = ringMaterialRequired;
        else
            newMaterials[1] = ringMaterialOff;

        leverBaseMeshRenderer.SetMaterials(newMaterials);
    }

    public void toggleState()
    {
        if (!animController.IsInTransition(0))
        {
            animController.SetTrigger(toggleTrigger);
        
            setState(!onState);
            SetColour();
        }
        else
        {
            Debug.Log("animController.IsInTransition(0) is true");
        }
    }

    public void onClick()
    {
        Debug.Log("ConsNode: "+name+"was clicked on");
        toggleState();
    } 

    public void resetNode()
    {
        if (onState!=false)
        {
            animController.SetTrigger(resetTrigger);
        }
        //required = false;
        setState(false);
        SetColour();
    }
}
