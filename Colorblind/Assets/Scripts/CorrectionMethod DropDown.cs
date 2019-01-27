using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorrectionMethod : MonoBehaviour
{

    List<string> correction_methods = new List<string>() { "Select an Option", "Daltonization", "ColorPopper", "Texture" };

    public Dropdown dropdown;

    public void DropDown_IndexChanged(int index)
    {
        if (index == 1)
        {
            Colorblind.correction_Method = 0;
        }
        else if (index == 2)
        {
            Colorblind.correction_Method = 1;
        }
        else if (index == 3)
        {
            Colorblind.correction_Method = 2;
        }
        else
        {
            // TODO: Is Daltonization the standard one
            Colorblind.correction_Method = 0;
        }
    }

    void Start()
    {
        PopulateList();
        Colorblind.correction_Method = 0;
    }

    void Update()
    {
        
    }
    void PopulateList()
    {
        dropdown.AddOptions(correction_methods);
    }
}
