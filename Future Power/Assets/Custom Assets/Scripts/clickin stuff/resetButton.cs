using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetButton : MonoBehaviour
{

    [SerializeField] private ConsumerController3d consumerCon;
    [SerializeField] private GeneratorController3d generatorCon;

    public void onClick()
    {
        consumerCon.resetAll();
        generatorCon.resetAll();
    }
}
