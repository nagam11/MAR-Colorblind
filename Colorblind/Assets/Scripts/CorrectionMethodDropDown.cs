using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;

public class CorrectionMethodDropDown : MonoBehaviour
{

    List<string> correction_methods = new List<string>() { "Select an Option", "Daltonization", "ColorPopper", "Texture", "Enhancing" };

    public Dropdown dropdown;

    public void DropDown_IndexChanged(int index)
    {

        if (index == 0 || index == 1 || index == 3 || index == 4)
        {
            Colorblind.correction_Method = 0;
            GameObject parentObject = GameObject.Find("CanvasAR");
            GameObject obj = parentObject.FindObject("Color Panel");
            obj.SetActive(false);           
        }
        else if (index == 2)
        {
            Colorblind.correction_Method = 1;
            GameObject parentObject = GameObject.Find("CanvasAR");
            GameObject obj = parentObject.FindObject("Color Panel");
            obj.SetActive(true);
        }       
        else
        {
            // TODO: Is Daltonization the standard one
            Colorblind.correction_Method = 6;
        }
    }

    void Start()
    {
        PopulateList();
    }

    void Update()
    {
        
    }
    void PopulateList()
    {
        dropdown.AddOptions(correction_methods);
    }
   
}

namespace ExtensionMethods
{
    public static class GO
    {
        public static GameObject FindObject(this GameObject parent, string name)
        {
            Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trs)
            {
                if (t.name == name)
                {
                    return t.gameObject;
                }
            }
            return null;
        }
    }
}