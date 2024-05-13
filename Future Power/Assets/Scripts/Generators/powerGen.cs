using UnityEngine;

[CreateAssetMenu(fileName = "GeneratorData", menuName = "Generator Data", order = 0)]
 
public class powerGen : ScriptableObject
{
    [SerializeField] public string GenName = "Default Generator";
    //450MW default for coal gen
    [SerializeField] public float genCapacity = 450f; 
    //Yeah no idea this will probably need to be redone later
    [SerializeField] public float responseSpeed = 1f;
}
