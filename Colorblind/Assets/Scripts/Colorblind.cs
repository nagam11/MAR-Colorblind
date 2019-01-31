// Script attached to AR Camera
using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class Colorblind : MonoBehaviour
{
    public enum Blindness { normal, protanopia, deuteranopia, tritanopia }
    public Blindness blindness = Blindness.normal;
    // The attribute prevents the camera from rendering the correction in the whole frame if the magnifying glass is used.
    // TODO: BUG??
    public static int full_Screen = 1;
    public static int correction_Method = 0;
    // 0: red 1: green 2: blue 3: yellow
    public static int selectedColor;

    [Range(1,10)]
    public int Strength = 10;

    Vector3 normalRed = new Vector3(1.00f, 0.00f, 0.00f);
    Vector3 normalGreen = new Vector3(0.00f, 1.00f, 0.00f);
    Vector3 normalBlue = new Vector3(0.00f, 0.00f, 1.00f);

    /*
    Matrices for simulating colorblindness from:
    Machado et al., A Physiologically-based Model for
    Simulation of Color Vision Deficiency.
    
        http://www.inf.ufrgs.br/~oliveira/pubs_files/CVD_Simulation/CVD_Simulation.html
     
    */

    Vector3 protanomalyRed10 = new Vector3(0.856167f, 0.182038f, -0.038205f);
    Vector3 protanomalyGreen10 = new Vector3(0.029342f, 0.955115f, 0.015544f);
    Vector3 protanomalyBlue10 = new Vector3(-0.00288f, -0.001563f, 1.004443f);
    Vector3 protanomalyRed20 = new Vector3(0.734766f, 0.334872f, -0.069637f);
    Vector3 protanomalyGreen20 = new Vector3(0.05184f, 0.919198f, 0.028963f);
    Vector3 protanomalyBlue20 = new Vector3(-0.004928f, -0.004209f, 1.009137f);
    Vector3 protanomalyRed30 = new Vector3(0.630323f, 0.465641f, -0.095964f);
    Vector3 protanomalyGreen30 = new Vector3(0.069181f, 0.890046f, 0.040773f);
    Vector3 protanomalyBlue30 = new Vector3(-0.006308f, -0.007724f, 1.014032f);
    Vector3 protanomalyRed40 = new Vector3(0.539009f, 0.579343f, -0.118352f);
    Vector3 protanomalyGreen40 = new Vector3(0.082546f, 0.866121f, 0.051332f);
    Vector3 protanomalyBlue40 = new Vector3(-0.007136f, -0.011959f, 1.019095f);
    Vector3 protanomalyRed50 = new Vector3(0.458064f, 0.679578f, -0.137642f);
    Vector3 protanomalyGreen50 = new Vector3(0.092785f, 0.846313f, 0.060902f);
    Vector3 protanomalyBlue50 = new Vector3(-0.007494f, -0.016807f, 1.024301f);
    Vector3 protanomalyRed60 = new Vector3(0.38545f, 0.769005f, -0.154455f);
    Vector3 protanomalyGreen60 = new Vector3(0.100526f, 0.829802f, 0.069673f);
    Vector3 protanomalyBlue60 = new Vector3(-0.007442f, -0.02219f, 1.029632f);
    Vector3 protanomalyRed70 = new Vector3(0.319627f, 0.849633f, -0.169261f);
    Vector3 protanomalyGreen70 = new Vector3(0.106241f, 0.815969f, 0.07779f);
    Vector3 protanomalyBlue70 = new Vector3(-0.007025f, -0.028051f, 1.035076f);
    Vector3 protanomalyRed80 = new Vector3(0.259411f, 0.923008f, -0.18242f);
    Vector3 protanomalyGreen80 = new Vector3(0.110296f, 0.80434f, 0.085364f);
    Vector3 protanomalyBlue80 = new Vector3(-0.006276f, -0.034346f, 1.040622f);
    Vector3 protanomalyRed90 = new Vector3(0.203876f, 0.990338f, -0.194214f);
    Vector3 protanomalyGreen90 = new Vector3(0.112975f, 0.794542f, 0.092483f);
    Vector3 protanomalyBlue90 = new Vector3(-0.005222f, -0.041043f, 1.046265f);
    Vector3 protanomalyRed100 = new Vector3(0.152286f, 1.052583f, -0.204868f);
    Vector3 protanomalyGreen100 = new Vector3(0.114503f, 0.786281f, 0.099216f);
    Vector3 protanomalyBlue100 = new Vector3(-0.003882f, -0.048116f, 1.051998f);

    Vector3 deuteranomalyRed10 = new Vector3(0.866435f, 0.177704f, -0.044139f);
    Vector3 deuteranomalyGreen10 = new Vector3(0.049567f, 0.939063f, 0.01137f);
    Vector3 deuteranomalyBlue10 = new Vector3(-0.003453f, 0.007233f, 0.99622f);
    Vector3 deuteranomalyRed20 = new Vector3(0.760729f, 0.319078f, -0.079807f);
    Vector3 deuteranomalyGreen20 = new Vector3(0.090568f, 0.889315f, 0.020117f);
    Vector3 deuteranomalyBlue20 = new Vector3(-0.006027f, 0.013325f, 0.992702f);
    Vector3 deuteranomalyRed30 = new Vector3(0.675425f, 0.43385f, -0.109275f);
    Vector3 deuteranomalyGreen30 = new Vector3(0.125303f, 0.847755f, 0.026942f);
    Vector3 deuteranomalyBlue30 = new Vector3(-0.00795f, 0.018572f, 0.989378f);
    Vector3 deuteranomalyRed40 = new Vector3(0.605511f, 0.52856f, -0.134071f);
    Vector3 deuteranomalyGreen40 = new Vector3(0.155318f, 0.812366f, 0.032316f);
    Vector3 deuteranomalyBlue40 = new Vector3(-0.009376f, 0.023176f, 0.9862f);
    Vector3 deuteranomalyRed50 = new Vector3(0.547494f, 0.607765f, -0.155259f);
    Vector3 deuteranomalyGreen50 = new Vector3(0.181692f, 0.781742f, 0.036566f);
    Vector3 deuteranomalyBlue50 = new Vector3(-0.01041f, 0.027275f, 0.983136f);
    Vector3 deuteranomalyRed60 = new Vector3(0.498864f, 0.674741f, -0.173604f);
    Vector3 deuteranomalyGreen60 = new Vector3(0.205199f, 0.754872f, 0.039929f);
    Vector3 deuteranomalyBlue60 = new Vector3(-0.011131f, 0.030969f, 0.980162f);
    Vector3 deuteranomalyRed70 = new Vector3(0.457771f, 0.731899f, -0.18967f);
    Vector3 deuteranomalyGreen70 = new Vector3(0.226409f, 0.731012f, 0.042579f);
    Vector3 deuteranomalyBlue70 = new Vector3(-0.011595f, 0.034333f, 0.977261f);
    Vector3 deuteranomalyRed80 = new Vector3(0.422823f, 0.781057f, -0.203881f);
    Vector3 deuteranomalyGreen80 = new Vector3(0.245752f, 0.709602f, 0.044646f);
    Vector3 deuteranomalyBlue80 = new Vector3(-0.011843f, 0.037423f, 0.974421f);
    Vector3 deuteranomalyRed90 = new Vector3(0.392952f, 0.82361f, -0.216562f);
    Vector3 deuteranomalyGreen90 = new Vector3(0.263559f, 0.69021f, 0.046232f);
    Vector3 deuteranomalyBlue90 = new Vector3(-0.01191f, 0.040281f, 0.97163f);
    Vector3 deuteranomalyRed100 = new Vector3(0.367322f, 0.860646f, -0.227968f);
    Vector3 deuteranomalyGreen100 = new Vector3(0.280085f, 0.672501f, 0.047413f);
    Vector3 deuteranomalyBlue100 = new Vector3(-0.01182f, 0.04294f, 0.968881f);

    Vector3 tritanomalyRed10 = new Vector3(0.92667f, 0.092514f, -0.019184f);
    Vector3 tritanomalyGreen10 = new Vector3(0.021191f, 0.964503f, 0.014306f);
    Vector3 tritanomalyBlue10 = new Vector3(0.008437f, 0.054813f, 0.93675f);
    Vector3 tritanomalyRed20 = new Vector3(0.89572f, 0.13333f, -0.02905f);
    Vector3 tritanomalyGreen20 = new Vector3(0.029997f, 0.9454f, 0.024603f);
    Vector3 tritanomalyBlue20 = new Vector3(0.013027f, 0.104707f, 0.882266f);
    Vector3 tritanomalyRed30 = new Vector3(0.905871f, 0.127791f, -0.033662f);
    Vector3 tritanomalyGreen30 = new Vector3(0.026856f, 0.941251f, 0.031893f);
    Vector3 tritanomalyBlue30 = new Vector3(0.01341f, 0.148296f, 0.838294f);
    Vector3 tritanomalyRed40 = new Vector3(0.948035f, 0.08949f, -0.037526f);
    Vector3 tritanomalyGreen40 = new Vector3(0.014364f, 0.946792f, 0.038844f);
    Vector3 tritanomalyBlue40 = new Vector3(0.010853f, 0.193991f, 0.795156f);
    Vector3 tritanomalyRed50 = new Vector3(1.017277f, 0.027029f, -0.044306f);
    Vector3 tritanomalyGreen50 = new Vector3(-0.006113f, 0.958479f, 0.047634f);
    Vector3 tritanomalyBlue50 = new Vector3(0.006379f, 0.248708f, 0.744913f);
    Vector3 tritanomalyRed60 = new Vector3(1.104996f, -0.046633f, -0.058363f);
    Vector3 tritanomalyGreen60 = new Vector3(-0.032137f, 0.971635f, 0.060503f);
    Vector3 tritanomalyBlue60 = new Vector3(0.001336f, 0.317922f, 0.680742f);
    Vector3 tritanomalyRed70 = new Vector3(1.193214f, -0.109812f, -0.083402f);
    Vector3 tritanomalyGreen70 = new Vector3(-0.058496f, 0.97941f, 0.079086f);
    Vector3 tritanomalyBlue70 = new Vector3(-0.002346f, 0.403492f, 0.598854f);
    Vector3 tritanomalyRed80 = new Vector3(1.257728f, -0.139648f, -0.118081f);
    Vector3 tritanomalyGreen80 = new Vector3(-0.078003f, 0.975409f, 0.102594f);
    Vector3 tritanomalyBlue80 = new Vector3(-0.003316f, 0.501214f, 0.502102f);
    Vector3 tritanomalyRed90 = new Vector3(1.278864f, -0.125333f, -0.153531f);
    Vector3 tritanomalyGreen90 = new Vector3(-0.084748f, 0.957674f, 0.127074f);
    Vector3 tritanomalyBlue90 = new Vector3(-0.000989f, 0.601151f, 0.399838f);
    Vector3 tritanomalyRed100 = new Vector3(1.255528f, -0.076749f, -0.178779f);
    Vector3 tritanomalyGreen100 = new Vector3(-0.078411f, 0.930809f, 0.147602f);
    Vector3 tritanomalyBlue100 = new Vector3(0.004733f, 0.691367f, 0.3039f);

    Vector3 protanomalyErrMapRed = new Vector3(0f, 0f, 0f);
    Vector3 protanomalyErrMapGreen = new Vector3(0.7f, 1f, 0f);
    Vector3 protanomalyErrMapBlue = new Vector3(0.7f, 0f, 1f);

    Vector3 deuteranomalyErrMapRed = new Vector3(1f, 0.7f, 0f);
    Vector3 deuteranomalyErrMapGreen = new Vector3(0f, 0f, 0f);
    Vector3 deuteranomalyErrMapBlue = new Vector3(0f, 0.7f, 1f);

    Vector3 tritanomalyErrMapRed = new Vector3(1f, 0f, 0.7f);
    Vector3 tritanomalyErrMapGreen = new Vector3(0f, 1f, 0.7f);
    Vector3 tritanomalyErrMapBlue = new Vector3(0f, 0f, 0f);

    private Material material;

void Awake()
{       
material = new Material(Shader.Find("SimulationCamera"));

}

public float changeStrength {
        get { return Strength; }
        set { Strength = (int) value; }
    }

// Postprocess the image
void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Vector3 toRed = Vector3.zero;
        Vector3 toGreen = Vector3.zero;
        Vector3 toBlue = Vector3.zero;
        Vector3 toErrRed = Vector3.zero;
        Vector3 toErrGreen = Vector3.zero;
        Vector3 toErrBlue = Vector3.zero;
        switch (blindness)
        {
        case Blindness.protanopia:
            toErrRed = protanomalyErrMapRed;
            toErrGreen = protanomalyErrMapGreen;
            toErrBlue = protanomalyErrMapBlue;
                switch (Strength*10)
                {
                    case 10:
                        toRed = protanomalyRed10;
                        toGreen = protanomalyGreen10;
                        toBlue = protanomalyBlue10;
                        break;
                    case 20:
                        toRed = protanomalyRed20;
                        toGreen = protanomalyGreen20;
                        toBlue = protanomalyBlue20;
                        break;
                    case 30:
                        toRed = protanomalyRed30;
                        toGreen = protanomalyGreen30;
                        toBlue = protanomalyBlue30;
                        break;
                    case 40:
                        toRed = protanomalyRed40;
                        toGreen = protanomalyGreen40;
                        toBlue = protanomalyBlue40;
                        break;
                    case 50:
                        toRed = protanomalyRed50;
                        toGreen = protanomalyGreen50;
                        toBlue = protanomalyBlue50;
                        break;
                    case 60:
                        toRed = protanomalyRed60;
                        toGreen = protanomalyGreen60;
                        toBlue = protanomalyBlue60;
                        break;
                    case 70:
                        toRed = protanomalyRed70;
                        toGreen = protanomalyGreen70;
                        toBlue = protanomalyBlue70;
                        break;
                    case 80:
                        toRed = protanomalyRed80;
                        toGreen = protanomalyGreen80;
                        toBlue = protanomalyBlue80;
                        break;
                    case 90:
                        toRed = protanomalyRed90;
                        toGreen = protanomalyGreen90;
                        toBlue = protanomalyBlue90;
                        break;
                    case 100:
                        toRed = protanomalyRed100;
                        toGreen = protanomalyGreen100;
                        toBlue = protanomalyBlue100;
                        break;
                    default:
                        toRed = protanomalyRed100;
                        toGreen = protanomalyGreen100;
                        toBlue = protanomalyBlue100;
                        break;
                }
        break;
    case Blindness.deuteranopia:
                toErrRed = deuteranomalyErrMapRed;
                toErrGreen = deuteranomalyErrMapGreen;
                toErrBlue = deuteranomalyErrMapBlue;
                switch (Strength*10)
                {
                    case 10:
                        toRed = deuteranomalyRed10;
                        toGreen = deuteranomalyGreen10;
                        toBlue = deuteranomalyBlue10;
                        break;
                    case 20:
                        toRed = deuteranomalyRed20;
                        toGreen = deuteranomalyGreen20;
                        toBlue = deuteranomalyBlue20;
                        break;
                    case 30:
                        toRed = deuteranomalyRed30;
                        toGreen = deuteranomalyGreen30;
                        toBlue = deuteranomalyBlue30;
                        break;
                    case 40:
                        toRed = deuteranomalyRed40;
                        toGreen = deuteranomalyGreen40;
                        toBlue = deuteranomalyBlue40;
                        break;
                    case 50:
                        toRed = deuteranomalyRed50;
                        toGreen = deuteranomalyGreen50;
                        toBlue = deuteranomalyBlue50;
                        break;
                    case 60:
                        toRed = deuteranomalyRed60;
                        toGreen = deuteranomalyGreen60;
                        toBlue = deuteranomalyBlue60;
                        break;
                    case 70:
                        toRed = deuteranomalyRed70;
                        toGreen = deuteranomalyGreen70;
                        toBlue = deuteranomalyBlue70;
                        break;
                    case 80:
                        toRed = deuteranomalyRed80;
                        toGreen = deuteranomalyGreen80;
                        toBlue = deuteranomalyBlue80;
                        break;
                    case 90:
                        toRed = deuteranomalyRed90;
                        toGreen = deuteranomalyGreen90;
                        toBlue = deuteranomalyBlue90;
                        break;
                    case 100:
                        toRed = deuteranomalyRed100;
                        toGreen = deuteranomalyGreen100;
                        toBlue = deuteranomalyBlue100;
                        break;
                    default:
                        toRed = deuteranomalyRed100;
                        toGreen = deuteranomalyGreen100;
                        toBlue = deuteranomalyBlue100;
                        break;
                }
            break;
    case Blindness.tritanopia:
                toErrRed = tritanomalyErrMapRed;
                toErrGreen = tritanomalyErrMapGreen;
                toErrBlue = tritanomalyErrMapBlue;
                switch (Strength*10)
                {
                    case 10:
                            toRed = tritanomalyRed10;
                            toGreen = tritanomalyGreen10;
                            toBlue = tritanomalyBlue10;
                            break;
                    case 20:
                            toRed = tritanomalyRed20;
                            toGreen = tritanomalyGreen20;
                            toBlue = tritanomalyBlue20;
                            break;
                    case 30:
                            toRed = tritanomalyRed30;
                            toGreen = tritanomalyGreen30;
                            toBlue = tritanomalyBlue30;
                            break;
                    case 40:
                            toRed = tritanomalyRed40;
                            toGreen = tritanomalyGreen40;
                            toBlue = tritanomalyBlue40;
                            break;
                    case 50:
                            toRed = tritanomalyRed50;
                            toGreen = tritanomalyGreen50;
                            toBlue = tritanomalyBlue50;
                            break;
                    case 60:
                            toRed = tritanomalyRed60;
                            toGreen = tritanomalyGreen60;
                            toBlue = tritanomalyBlue60;
                            break;
                    case 70:
                            toRed = tritanomalyRed70;
                            toGreen = tritanomalyGreen70;
                            toBlue = tritanomalyBlue70;
                            break;
                    case 80:
                            toRed = tritanomalyRed80;
                            toGreen = tritanomalyGreen80;
                            toBlue = tritanomalyBlue80;
                            break;
                    case 90:
                            toRed = tritanomalyRed90;
                            toGreen = tritanomalyGreen90;
                            toBlue = tritanomalyBlue90;
                            break;
                    case 100:
                            toRed = tritanomalyRed100;
                            toGreen = tritanomalyGreen100;
                            toBlue = tritanomalyBlue100;
                            break;
                    default:
                        toRed = tritanomalyRed100;
                        toGreen = tritanomalyGreen100;
                        toBlue = tritanomalyBlue100;
                        break;
                }
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

        material.SetFloat("_err", toErrRed.x);
        material.SetFloat("_egr", toErrRed.y);
        material.SetFloat("_ebr", toErrRed.z);
        material.SetFloat("_erg", toErrGreen.x);
        material.SetFloat("_egg", toErrGreen.y);
        material.SetFloat("_ebg", toErrGreen.z);
        material.SetFloat("_erb", toErrBlue.x);
        material.SetFloat("_egb", toErrBlue.y);
        material.SetFloat("_ebb", toErrBlue.z);

        material.SetInt("fullScreen", full_Screen);
        material.SetInt("correctionMethod", correction_Method);       
        material.SetInt("Blindness Strength (Int)", Strength);
        Graphics.Blit(source, destination, material);
        
    }

    public void NextMode()
{
    var length = System.Enum.GetNames(typeof(Blindness)).Length;
    blindness = (Blindness)(
        ((int)blindness + 1) % length
);

}

public void ChangeColor(string name)
{
    if (name == "red")
        {
            material.SetInt("selectedColor", 0);
            selectedColor = 0;
        }
    else if (name == "green")
        {
            material.SetInt("selectedColor", 1);
            selectedColor = 1;
        }
    else if (name == "blue")
        {
            material.SetInt("selectedColor", 2);
            selectedColor = 2;
        }
    else if (name == "yellow")
        {
            material.SetInt("selectedColor", 3);
            selectedColor = 3;
        }
    }
}