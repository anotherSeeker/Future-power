using UnityEngine;

[CreateAssetMenu(fileName = "ConsumerData", menuName = "Consumer Data", order = 0)]
 
public class PowerCons : ScriptableObject
{
    [SerializeField] public string ConsumerName = "Default Consumer";
    //15MW default for domestic 1
    [SerializeField] public float powerDraw = 15f;

}
