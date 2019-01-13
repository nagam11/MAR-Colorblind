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

    /*
	RGBtoProt =

		0.1127    0.8891    0.0196
		0.1124    0.8878    0.0001
		-0.0166   -0.1311   -0.0114


	RGBtodeut =

		0.3030    0.7336    0.0222
		0.2887    0.6974   -0.0008
		-0.0427   -0.1040   -0.0113


	RGBtotrit =

		0.4238    0.5420    0.0175
		0.5020    0.5304    0.0024
		-0.0389   -0.1100   -0.0115
	*/

    Vector3 normalRed = new Vector3(1.00f, 0.00f, 0.00f);
    Vector3 normalGreen = new Vector3(0.00f, 1.00f, 0.00f);
    Vector3 normalBlue = new Vector3(0.00f, 0.00f, 1.00f);

    /*
    Vector3 protoRed = new Vector3(0.2f, 0.99f, -0.19f);
    Vector3 protoGreen = new Vector3(0.16f, 0.79f, 0.04f);
    Vector3 protoBlue = new Vector3(0.01f, -0.01f, 1.00f);
    */

    Vector3 protoRed = new Vector3(0.1139f, 0.8990f, 0.0066f);
    Vector3 protoGreen = new Vector3(0.1064f, 0.8400f, -0.0050f);
    Vector3 protoBlue = new Vector3(0.0119f, 0.0554f, 1.0066f);

    /*
    Vector3 deutoRed = new Vector3(0.43f, 0.72f, -0.15f);
    Vector3 deutoGreen = new Vector3(0.34f, 0.57f, 0.09f);
    Vector3 deutoBlue = new Vector3(-0.02f, 0.03f, 1.00f);
    */

    Vector3 deutoRed = new Vector3(0.3030f, 0.7336f, 0.0222f);
    Vector3 deutoGreen = new Vector3(0.2887f, 0.6974f, -0.0008f);
    Vector3 deutoBlue = new Vector3(-0.0427f, -0.1040f, -0.0113f);

    /*
    Vector3 tritoRed = new Vector3(0.97f, 0.11f, -0.08f);
    Vector3 tritoGreen = new Vector3(0.02f, 0.82f, 0.16f);
    Vector3 tritoBlue = new Vector3(-0.06f, 0.88f, 0.18f);
    */

    Vector3 tritoRed = new Vector3(0.4238f, 0.5420f, 0.0175f);
    Vector3 tritoGreen = new Vector3(0.5020f, 0.5304f, 0.0024f);
    Vector3 tritoBlue = new Vector3(-0.0389f, -0.1100f, -0.0115f);

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