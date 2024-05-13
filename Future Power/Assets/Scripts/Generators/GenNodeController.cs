using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GenNodeController : MonoBehaviour
{
    [SerializeField] public List<GameObject> generators;
    [SerializeField] public TMP_Dropdown dropdown;

    void Start()
    {
        populateDropdown();
    }

    private void ChangeNode(GameObject newChild)
    {
        Transform childGenerator = GetCurrentGen(transform);
        Destroy(childGenerator.gameObject);

        Instantiate(newChild, transform);
    }

    private Transform GetCurrentGen(Transform parent)
    {
        List<Transform> children = new List<Transform>();

        foreach(Transform child in parent)
        {
            children.Add(child);
            if (child.GetComponent<GeneratorNode>())
                return child;
        }

        return null;
    }

    public void populateDropdown()
    {
        foreach(GameObject gen in generators)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(gen.GetComponent<GeneratorNode>().GetName(), null));
        }
    }

    public void NodeDropdown(int index)
    {
        
    }

}
