/* Script attached to the glass mesh of the magnifying glass.
 * This script is needed in order to render different blindness types only on
 * the glass part of the magnifying glass.
 */

using UnityEngine;
using System.Collections;

public class MagnifyingGlass : MonoBehaviour
{
    public enum Blindness { normal, protanopia, deuteranopia, tritanopia }   

    Vector3 normalRed = new Vector3(1.00f, 0.00f, 0.00f);
    Vector3 normalGreen = new Vector3(0.00f, 1.00f, 0.00f);
    Vector3 normalBlue = new Vector3(.00f, 0.00f, 1.00f);
    Vector3 protoRed = new Vector3(0.2f, 0.99f, -0.19f);
    Vector3 protoGreen = new Vector3(0.16f, 0.79f, 0.04f);
    Vector3 protoBlue = new Vector3(0.01f, -0.01f, 1.00f);
    Vector3 deutoRed = new Vector3(0.43f, 0.72f, -0.15f);
    Vector3 deutoGreen = new Vector3(0.34f, 0.57f, 0.09f);
    Vector3 deutoBlue = new Vector3(-0.02f, 0.03f, 1.00f);
    Vector3 tritoRed = new Vector3(0.97f, 0.11f, -0.08f);
    Vector3 tritoGreen = new Vector3(0.02f, 0.82f, 0.16f);
    Vector3 tritoBlue = new Vector3(-0.06f, 0.88f, 0.18f);   
    private Renderer rend;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();       
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toRed = Vector3.zero;
        Vector3 toGreen = Vector3.zero;
        Vector3 toBlue = Vector3.zero;
        // Change the rendering of the magnifying glass according to the blindness type selected in the AR Camera.
        //TODO: doesn't work with normal vision. it needs parameters of which colorblind has to simulate
        switch (GameObject.Find("ARCamera").GetComponent<Colorblind>().blindness)
        {
            case Colorblind.Blindness.protanopia:
                toRed = protoRed;
                toGreen = protoGreen;
                toBlue = protoBlue;
                break;
            case Colorblind.Blindness.deuteranopia:
                toRed = deutoRed;
                toGreen = deutoGreen;
                toBlue = deutoBlue;
                break;
            case Colorblind.Blindness.tritanopia:
                toRed = tritoRed;
                toGreen = tritoGreen;
                toBlue = tritoBlue;
                break;
            default:
                toRed = normalRed;
                toGreen = normalGreen;
                toBlue = normalBlue;
                break;
        }
        // Set material of render to the selected blindness type
        rend.material.SetFloat("_rr", toRed.x);
        rend.material.SetFloat("_gr", toRed.y);
        rend.material.SetFloat("_br", toRed.z);
        rend.material.SetFloat("_rg", toGreen.x);
        rend.material.SetFloat("_gg", toGreen.y);
        rend.material.SetFloat("_bg", toGreen.z);
        rend.material.SetFloat("_rb", toBlue.x);
        rend.material.SetFloat("_gb", toBlue.y);
        rend.material.SetFloat("_bb", toBlue.z);
    }
}
