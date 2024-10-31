using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetButton : MonoBehaviour
{

    [SerializeField] private ConsumerController3d consumerCon;
    [SerializeField] private GeneratorController3d generatorCon;

    [SerializeField] private Animator animController;

    public void onClick()
    {
        if (animController)
        {
            if (!animController.IsInTransition(0))
            {
                //play press anim
                animController.SetTrigger("buttonPressed");
            }
        }

        consumerCon.resetAll();
        generatorCon.resetAll();
    }
}
