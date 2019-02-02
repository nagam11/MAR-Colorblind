using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colorName : MonoBehaviour
    {
        Color cc;
        int resWidth = Screen.width;
        int resHeight = Screen.height;
        GameObject parentObject;
        Text[] textValue;
        public static int whichColor;

    void Start()
    {
        parentObject = GameObject.Find("ColorNameSpace");
        textValue = parentObject.GetComponentsInChildren<Text>();
        cc = Color.red;
    }

    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Camera camera = Camera.main;
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

            cc = screenShot.GetPixel((int)Input.mousePosition.x, (int)Input.mousePosition.y);
            //cc = screenShot.GetPixel((int)Input.GetTouch(0).position.x, (int)Input.GetTouch(0).position.y);
            //rgbtoText(cc);
            checkColor(cc);

            camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            Destroy(screenShot);
        }
        */
        if (whichColor == 1)
        {
            if (Input.touchCount > 0)
            {
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    Camera camera = Camera.main;
                    RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
                    camera.targetTexture = rt;
                    Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
                    camera.Render();
                    RenderTexture.active = rt;
                    screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

                    cc = screenShot.GetPixel((int)Input.GetTouch(0).position.x, (int)Input.GetTouch(0).position.y);

                    //rgbtoText(cc);
                    checkColor(cc);

                    camera.targetTexture = null;
                    RenderTexture.active = null; // JC: added to avoid errors
                    Destroy(rt);
                    Destroy(screenShot);

                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    GameObject parentObject = GameObject.Find("ColorNameSpace");
                    textValue = parentObject.GetComponentsInChildren<Text>();
                    textValue[0].text = "";
                }
            }
         }
    }


    void rgbtoText(Color c)
    {
        float H, S, V;
        Color.RGBToHSV(c, out H, out S, out V);
        Vector3 hsv_color = new Vector3(H, S, V);
        print(hsv_color.x);
        print(hsv_color.y);
        print(hsv_color.z);
        /* RED */
        if ((hsv_color.x < 0.034 || hsv_color.x > 0.8) && (hsv_color.y > 0.78) && (hsv_color.z > 0.30))
        {
            showColorName("red");
        }
        /* GREEN */
        if ((hsv_color.x < 0.45 && hsv_color.x > 0.27) && (hsv_color.y > 0.65) && (hsv_color.z > 0.10))
        {
            showColorName("green");
        }
        /* BLUE */
        if ((0.527 < hsv_color.x && hsv_color.x < 0.694) && (hsv_color.y > 0.75) && (hsv_color.z > 0.3))
        {
            showColorName("blue");
        }
        /* YELLOW */
        //if ((0.115 < hsv_color.x && hsv_color.x < 0.183) && (hsv_color.y > 0.50) && (hsv_color.z > 0.2))
        if ((0.09 < hsv_color.x && hsv_color.x < 0.30))
        {

                showColorName("yellow");
            }
        //showColorName(c.ToString());
    }

    Vector3 rgbToLAB(Color c)
    {
        float fx, fy, fz, xr, yr, zr;
        float Ls, As, Bs;
        float eps = 216f / 24389f;
        //float k = 903.2963f;
        float k = 24389f / 27f;

        float r = Mathf.Pow(c.r, 2.2f);
        float g = Mathf.Pow(c.g, 2.2f);
        float b = Mathf.Pow(c.b, 2.2f);
        /*
        float r = c.r;
        float g = c.g;
        float b = c.b;
        */

        if (r > eps) fx = Mathf.Pow(r, 1f / 3f);
        else fx = ((k * r) + 16f) / 116f;
        if (g > eps) fy = Mathf.Pow(g, 1f / 3f);
        else fy = ((k * g) + 16f) / 116f;
        if (b > eps) fz = Mathf.Pow(b, 1f / 3f);
        else fz = ((k * b) + 16f) / 116f;

        Ls = (116f * fy) - 16f;
        As = 500f * (fx - fy);
        Bs = 200f * (fy - fz);

        xr = (2.55f * Ls + 0.5f);
        yr = (As + 0.5f);
        zr = (Bs + 0.5f);
        /*
        xr = Ls;
        yr = As;
        zr = Bs;
        */

        return (new Vector3(xr, yr, zr));

    }

    float diffColors(Color a, Color b)
    {
        Vector3 l1 = rgbToLAB(a);
        Vector3 l2 = rgbToLAB(b);
        return Mathf.Sqrt(Mathf.Pow((l1.x - l2.x), 2) + Mathf.Pow((l1.y - l2.y), 2) + Mathf.Pow((l1.z - l2.z), 2));
    }

    void checkColor(Color c)
    {
        float minDiff = 70;
        float diff = float.MaxValue;
        string nameC = "Not sure";
        for(int i=0; i<17; i++)
        {
            switch (i)
            {
                case 0:
                    diff = diffColors(c, Color.red);
                    if (diff < minDiff)
                    {
                        nameC = "red";
                        minDiff = diff;
                    }
                    break;
                case 1:
                    diff = diffColors(c, Color.green);
                    if (diff < minDiff)
                    {
                        nameC = "green";
                        minDiff = diff;
                    }
                    break;
                case 2:
                    diff = diffColors(c, Color.blue);
                    if (diff < minDiff)
                    {
                        nameC = "blue";
                        minDiff = diff;
                    }
                    break;
                case 4:
                    diff = diffColors(c, Color.cyan);
                    if (diff < minDiff)
                    {
                        nameC = "cyan";
                        minDiff = diff;
                    }
                    break;
                case 5:
                    diff = diffColors(c, Color.magenta);
                    if (diff < minDiff)
                    {
                        nameC = "magenta";
                        minDiff = diff;
                    }
                    break;
                case 6:
                    diff = diffColors(c, Color.yellow);
                    if (diff < minDiff)
                    {
                        nameC = "yellow";
                        minDiff = diff;
                    }
                    break;
                case 7:
                    diff = diffColors(c, Color.white);
                    if (diff < minDiff)
                    {
                        nameC = "white";
                        minDiff = diff;
                    }
                    break;
                case 8:
                    diff = diffColors(c, Color.black);
                    if (diff < minDiff)
                    {
                        nameC = "black";
                        minDiff = diff;
                    }
                    break;
                case 9:
                    diff = diffColors(c, Color.grey);
                    if (diff < minDiff)
                    {
                        nameC = "grey";
                        minDiff = diff;
                    }
                    break;
                case 10:
                    diff = diffColors(c, new Color(0.75f, 0.75f, 0.75f, 1f));
                    if (diff < minDiff)
                    {
                        nameC = "silver";
                        minDiff = diff;
                    }
                    break;
                case 11:
                    diff = diffColors(c, new Color(0.5f, 0f, 0f, 1f));
                    if (diff < minDiff)
                    {
                        nameC = "dark red";
                        minDiff = diff;
                    }
                    break;
                case 12:
                    diff = diffColors(c, new Color(0.5f, 0.5f, 0f, 1f));
                    if (diff < minDiff)
                    {
                        nameC = "dark green";
                        minDiff = diff;
                    }
                    break;
                case 13:
                    diff = diffColors(c, new Color(0.5f, 0f, 0.5f, 1f));
                    if (diff < minDiff)
                    {
                        nameC = "purple";
                        minDiff = diff;
                    }
                    break;
                case 14:
                    diff = diffColors(c, new Color(0f, 0f, 0.5f, 1f));
                    if (diff < minDiff)
                    {
                        nameC = "dark blue";
                        minDiff = diff;
                    }
                    break;
                case 15:
                    diff = diffColors(c, new Color(1f, 0.8f, 0f, 1f));
                    if (diff < minDiff)
                    {
                        nameC = "orange";
                        minDiff = diff;
                    }
                    break;
                case 16:
                    diff = diffColors(c, new Color(1f, 0.8f, 0.9f, 1f));
                    if (diff < minDiff)
                    {
                        nameC = "pink";
                        minDiff = diff;
                    }
                    break;
                case 17:
                    diff = diffColors(c, new Color(0.5f, 0.3f, 0.04f, 1f));
                    if (diff < minDiff)
                    {
                        nameC = "brown";
                        minDiff = diff;
                    }
                    break;
                case 18:
                    diff = diffColors(c, new Color(0.5f, 0.3f, 0.04f, 1f));
                    if (diff < minDiff)
                    {
                        nameC = "brown";
                        minDiff = diff;
                    }
                    break;
            }
        }
        showColorName(nameC);
        print(minDiff);
    }

    void showColorName(string n)
    {
        textValue[0].text = n;
    }

}
