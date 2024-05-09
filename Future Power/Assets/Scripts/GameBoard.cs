using UnityEngine;
using TMPro;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private GeneratorController genController;
    [SerializeField] private ConsumerController conController;

    private float genPower = 0f;
    private float reqPower = 0f;

    public TextMeshProUGUI description;

    void Start()
    {
        description.text = "Requested Power: "+reqPower.ToString("F2")+"\nGenerated Power: "+genPower.ToString("F2");
    }

    void Update()
    {
        genPower = genController.GetGeneratorPower();
        reqPower = conController.GetRequestedPower();

        description.text = "Requested Power: "+reqPower.ToString("F2")+"\nGenerated Power: "+genPower.ToString("F2");
    }
}