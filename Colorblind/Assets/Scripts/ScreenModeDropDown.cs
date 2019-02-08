using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenModeDropDown : MonoBehaviour
{

    List<string> screen_mode = new List<string>() { "Full Screen", "Magnifying glass"};

    public Dropdown dropdown;

    public void DropDown_IndexChanged(int index)
    {
        if (index == 0)
        {
            Colorblind.full_Screen = 1;
            GameObject.Find("ARCamera").GetComponent<InteractionMode>().mode = InteractionMode.CameraMode.Full_Screen;
            Destroy(GameObject.Find("Clone"));
                
        }
        else if (index == 1)
        {
            Colorblind.full_Screen = 0;
            GameObject.Find("ARCamera").GetComponent<InteractionMode>().mode = InteractionMode.CameraMode.Magnifying_Glass;
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
        //TODO: stays in full screen mode always ?!
        Colorblind.full_Screen = 1;
    }

    void Update()
    {
        
    }
    void PopulateList()
    {
        dropdown.AddOptions(screen_mode);
    }
}
