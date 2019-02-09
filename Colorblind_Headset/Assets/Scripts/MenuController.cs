using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ExtensionMethods;

public class MenuController : MonoBehaviour
{

    public void LoadSimulation()
    {
        SceneManager.LoadScene("SimulatorScene");
        Colorblind.simulated = 1;
        ResetAllSettings();
    }

    public void LoadNormal()
    {
        SceneManager.LoadScene("NormalScene");
        Colorblind.simulated = 0;
        colorName.whichColor = 1;
        ResetAllSettings();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void ResetAllSettings()
    {
        // ANOMALY
        Colorblind.blindness = Colorblind.Blindness.normal;
        colorName.whichColor = 1;

        // CORRECTION
        Colorblind.correction_Method = 6;

        // SCREEN MODE
        Colorblind.full_Screen = 1;
        InteractionMode.mode = InteractionMode.CameraMode.Full_Screen;
        Destroy(GameObject.Find("Clone"));
    }
}


