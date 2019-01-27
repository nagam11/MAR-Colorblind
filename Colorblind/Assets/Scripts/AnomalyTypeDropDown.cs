using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnomalyTypeDropDown : MonoBehaviour
{

    List<string> names = new List<string>() { "Select an Option", "Deutranopia", "Protanopia", "Tritanopia" };

    public Dropdown dropdown;

    public void DropDown_IndexChanged(int index)
    {
        if (index == 1) // deutranopia
        {
            GameObject.Find("ARCamera").GetComponent<Colorblind>().blindness = Colorblind.Blindness.deuteranopia;
            
        }
        else if (index == 2)
        {
            GameObject.Find("ARCamera").GetComponent<Colorblind>().blindness = Colorblind.Blindness.protanopia;     

        }
        else if (index == 3)
        {
            GameObject.Find("ARCamera").GetComponent<Colorblind>().blindness = Colorblind.Blindness.tritanopia;
        }
        else
        {
            GameObject.Find("ARCamera").GetComponent<Colorblind>().blindness = Colorblind.Blindness.normal;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        PopulateList();
        GameObject.Find("ARCamera").GetComponent<Colorblind>().blindness = Colorblind.Blindness.normal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PopulateList()
    {
        dropdown.AddOptions(names);
    }
}
