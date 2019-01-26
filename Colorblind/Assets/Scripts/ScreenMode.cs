using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMode : MonoBehaviour
{

    List<string> screen_mode = new List<string>() { "Select an Option", "Full Screen", "Magnifying glass"};

    public Dropdown dropdown;

    //TODO: change correction methods here
    public void DropDown_IndexChanged(int index)
    {
        if (index == 1)
        {
            Colorblind.full_Screen = 1;
        }
        else if (index == 2)
        {
            Colorblind.full_Screen = 0;
        }       
        else
        {
           // Do nothing. Use the same screen mode as before.
        }
    }

    void Start()
    {
        PopulateList();
        // App starts in magnifying glass mode.
        Colorblind.full_Screen = 0;
    }

    void Update()
    {
        
    }
    void PopulateList()
    {
        dropdown.AddOptions(screen_mode);
    }
}
