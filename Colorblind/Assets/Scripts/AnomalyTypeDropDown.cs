using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnomalyTypeDropDown : MonoBehaviour
{

    List<string> names = new List<string>() { "Normal", "Deutranopia", "Protanopia", "Tritanopia" };

    public Dropdown dropdown;

    public void DropDown_IndexChanged(int index)
    {
        if (index == 1) // deutranopia
        {
            GameObject.Find("ARCamera").GetComponent<Colorblind>().blindness = Colorblind.Blindness.deuteranopia;
            colorName.whichColor = 0;
        }
        else if (index == 2)
        {
            GameObject.Find("ARCamera").GetComponent<Colorblind>().blindness = Colorblind.Blindness.protanopia;
            colorName.whichColor = 0;

        }
        else if (index == 3)
        {
            GameObject.Find("ARCamera").GetComponent<Colorblind>().blindness = Colorblind.Blindness.tritanopia;
            colorName.whichColor = 0;
        }
        else
        {
            GameObject.Find("ARCamera").GetComponent<Colorblind>().blindness = Colorblind.Blindness.normal;
            colorName.whichColor = 1;
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
