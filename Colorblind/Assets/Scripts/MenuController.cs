using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public void LoadSimulation()
    {
        SceneManager.LoadScene("SimulatorScene");
    }

    public void LoadNormal()
    {
        SceneManager.LoadScene("NormalScene");
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
