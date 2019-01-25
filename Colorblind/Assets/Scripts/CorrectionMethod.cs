using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorrectionMethod : MonoBehaviour
{

    List<string> correction_methods = new List<string>() { "Select an Option", "Daltonization", "Popper", "Other" };

    public Dropdown dropdown;

    //TODO: change corrrection methods here
    public void DropDown_IndexChanged(int index)
    {
        if (index == 1)
        {

            
        }
        else if (index == 2)
        {
               

        }
        else if (index == 3)
        {
           
        }
        else
        {
           
        }
    }

    void Start()
    {
        PopulateList();
        //TODO : set correction method from here
    }

    void Update()
    {
        
    }
    void PopulateList()
    {
        dropdown.AddOptions(correction_methods);
    }
}
