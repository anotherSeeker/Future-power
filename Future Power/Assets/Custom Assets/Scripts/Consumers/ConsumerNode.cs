using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ConsumerNode : MonoBehaviour
{
    [SerializeField] private PowerCons consumer; 
    [SerializeField] private bool required = false;
    [SerializeField] private MeshRenderer leverBaseMeshRenderer;
    [SerializeField] private Material ringMaterialOn;
    [SerializeField] private Material ringMaterialOff;
    [SerializeField] private Material ringMaterialRequired;
    //[SerializeField] private String triggerName = "changeState";
    private Animator animController;

    private bool onState = false;

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
        if (animController)
        {
            if (! (animController.GetCurrentAnimatorStateInfo(0).IsName("BaseState.Turn On") || animController.GetCurrentAnimatorStateInfo(0).IsName("BaseState.Turn Off")))
            {
                //return;//if we're currently changing states can't click again
            }
            animController.SetTrigger("changeState");
        }
        setState(!onState);
        SetColour();
    }

    public void onClick()
    {
        Debug.Log("ConsNode: "+name+"was clicked on");
        toggleState();
    } 

    public void resetNode()
    {
        animController.SetTrigger("reset");
        setState(false);
        SetColour();
    }
}
