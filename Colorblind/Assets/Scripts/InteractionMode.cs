// This script controls the different interaction modes of the user.
using UnityEngine;
using System.Collections;

public class InteractionMode : MonoBehaviour
{
    public enum CameraMode { Full_Screen, Magnifying_Glass}
    public CameraMode mode = CameraMode.Full_Screen;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case CameraMode.Full_Screen:
                Colorblind.full_Screen = 1;
                break;
            case CameraMode.Magnifying_Glass:
                Colorblind.full_Screen = 0;
                break;            
        }
    }
}
