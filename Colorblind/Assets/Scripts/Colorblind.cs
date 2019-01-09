// Script attached to AR Camera
using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class Colorblind : MonoBehaviour
{
    public enum Blindness { normal, protanopia, deuteranopia, tritanopia }
    public Blindness blindness = Blindness.normal;
    // The attribute prevents the camera from rendering the correction in the whole frame if the magnifying glass is used.
    public static int full_Screen = 1;

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

    private Material material;

    void Awake()
    {       
        material = new Material(Shader.Find("SimulationCamera"));
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Vector3 toRed = Vector3.zero;
        Vector3 toGreen = Vector3.zero;
        Vector3 toBlue = Vector3.zero;
        switch (blindness)
        {
            case Blindness.protanopia:
                toRed = protoRed;
                toGreen = protoGreen;
                toBlue = protoBlue;
                break;
            case Blindness.deuteranopia:
                toRed = deutoRed;
                toGreen = deutoGreen;
                toBlue = deutoBlue;
                break;
            case Blindness.tritanopia:
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

        material.SetFloat("_rr", toRed.x);
        material.SetFloat("_gr", toRed.y);       
        material.SetFloat("_br", toRed.z);
        material.SetFloat("_rg", toGreen.x);
        material.SetFloat("_gg", toGreen.y);
        material.SetFloat("_bg", toGreen.z);
        material.SetFloat("_rb", toBlue.x);
        material.SetFloat("_gb", toBlue.y);
        material.SetFloat("_bb", toBlue.z);
        material.SetInt("fullScreen", full_Screen);
        Graphics.Blit(source, destination, material);
    }

    public void NextMode()
    {
        var length = System.Enum.GetNames(typeof(Blindness)).Length;
        blindness = (Blindness)(
            ((int)blindness + 1) % length
        );
    }
}