using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class GameBoard : MonoBehaviour  
{
    [SerializeField] private GeneratorController3d genController;
    [SerializeField] private ConsumerController3d conController;
    
    [SerializeField] float overloadBuffer = 50f;
    [SerializeField] float overloadRatio = 0.1f;
    [SerializeField] private TextMeshProUGUI descriptionFront;
    [SerializeField] private TextMeshProUGUI descriptionBack;

    [Header("Power Generation")]
    [SerializeField] private float genPower = 0f;
    [SerializeField] private float reqPower = 0f;
    [SerializeField] bool overloaded = false;

    void Start()
    {
        descriptionFront.text = "Requested Power: "+reqPower.ToString("F2")+"\nGenerated Power: "+genPower.ToString("F2");
        descriptionBack.text = "Requested Power: "+reqPower.ToString("F2")+"\nGenerated Power: "+genPower.ToString("F2");
    }

    void Update()
    {
        genPower = genController.GetGeneratorPower();
        reqPower = conController.GetRequestedPower();

        descriptionFront.text = "Requested Power: "+reqPower.ToString("F2")+"\nGenerated Power: "+genPower.ToString("F2");
        descriptionBack.text = "Requested Power: "+reqPower.ToString("F2")+"\nGenerated Power: "+genPower.ToString("F2");

        overloaded = checkOverloaded();
        if (overloaded)
        {
            descriptionFront.text = "Grid Overload Reset Required\nReq.: "+reqPower.ToString("F2")+" Gen.: "+genPower.ToString("F2");
            descriptionBack.text = "Grid Overload Reset Required\nReq.: "+reqPower.ToString("F2")+" Gen.: "+genPower.ToString("F2");
        }
    }

    private bool checkOverloaded()
    {
        //if difference between our request and generation is > our buffer + overloadRatio*genPower we overload
        //The more power the system currently has the more generous we get so our ratio is small
        if (math.abs(genPower-reqPower) > overloadBuffer + overloadRatio*genPower)
        {
            return true;
        }        

        //if we are overloaded AND our genPower AND reqPower are not 0 we stay overloaded, both must be 0 to reset the system
        if (overloaded && !(genPower == 0 && reqPower == 0))
        {
            return true;
        }

        //neither of our conditions are true so we are not overloaded
        return false;
    }
}