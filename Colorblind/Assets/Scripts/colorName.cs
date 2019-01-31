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

    void Start()
    {
        parentObject = GameObject.Find("ColorNameSpace");
        textValue = parentObject.GetComponentsInChildren<Text>();
        cc = Color.red;
    }

    void Update()
    {
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
            rgbtoText(cc);

            camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            Destroy(screenShot);
        }
        /*
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

                rgbtoText(cc);

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
        */
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
        
    }

    void rgbtoText2(Color32 c)
    {
        print(c.r);
        print(c.g);
        print(c.b);
        /* RED */
        if (c.r > 0.6 && c.g < 0.4 && c.b < 0.4)
        {
            showColorName("red");
        }
        /* GREEN */
        if (c.r < 0.4 && c.g > 0.6 && c.b < 0.4)
        {
            showColorName("green");
        }
        /* BLUE */
        if (c.r < 0.4 && c.g < 0.4 && c.b > 0.6)
        {
            showColorName("blue");
        }
        /* YELLOW */
        if (c.r > 0.6 && c.g > 0.6 && c.b < 0.4)
        {
            showColorName("yellow");
        }
    }

    void showColorName(string n)
    {
        textValue[0].text = n;
    }

}
