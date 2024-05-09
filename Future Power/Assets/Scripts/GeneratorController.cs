using UnityEngine;
public class GeneratorController : MonoBehaviour
{
    [SerializeField]private float power = 0;
    private Component[] genNodes;

    public float GetGeneratorPower()
    {
        power = 0;
        genNodes = GetComponentsInChildren<powerGeneratorNode>();
        foreach (powerGeneratorNode node in genNodes)
        {
            power = power + node.getCurrentPower();
        }

        return power;
    }

}