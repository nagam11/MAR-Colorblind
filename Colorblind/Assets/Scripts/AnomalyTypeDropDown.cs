using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;

public class AnomalyTypeDropDown : MonoBehaviour
{

    List<string> names = new List<string>() { "Normal", "Deutranopia", "Protanopia", "Tritanopia" };

    public Dropdown dropdown;

    public void DropDown_IndexChanged(int index)
    {
        if (index == 1) // deutranopia
        {
            Colorblind.blindness = Colorblind.Blindness.deuteranopia;
            colorName.whichColor = 0;
            GameObject parentObject = GameObject.Find("CanvasAR");
            GameObject label = parentObject.FindObject("ColorNameSpace");
            label.SetActive(false);
        }
        else if (index == 2)
        {
            Colorblind.blindness = Colorblind.Blindness.protanopia;
            colorName.whichColor = 0;
            GameObject parentObject = GameObject.Find("CanvasAR");
            GameObject label = parentObject.FindObject("ColorNameSpace");
            label.SetActive(false);

        }
        else if (index == 3)
        {
            Colorblind.blindness = Colorblind.Blindness.tritanopia;
            colorName.whichColor = 0;
            GameObject parentObject = GameObject.Find("CanvasAR");
            GameObject label = parentObject.FindObject("ColorNameSpace");
            label.SetActive(false);
        }
        else
        {
            Colorblind.blindness = Colorblind.Blindness.normal;
            colorName.whichColor = 1;
            GameObject parentObject = GameObject.Find("CanvasAR");
            GameObject label = parentObject.FindObject("ColorNameSpace");
            label.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        PopulateList();
        Colorblind.blindness = Colorblind.Blindness.normal;
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
