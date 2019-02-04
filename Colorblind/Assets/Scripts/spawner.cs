using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spawner : MonoBehaviour
{

    public GameObject cube;
    public GameObject clone;
    public Camera nonVRCamera;
    private Button _button;
    private Button _buttonSave;
    private int flag;


    public void buttonHandler()
    {
        if (clone != null)
            Destroy(clone);
        Debug.Log("click");
        flag = 1;
    }

    public void buttonSaveHandler()
    {
        Debug.Log("saved");
        flag = 0;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        _button = GameObject.Find("Settings Button").GetComponent<Button>();
        _button.onClick.AddListener(buttonHandler);

        if (flag == 0)
        {

            if (GameObject.Find("ARCamera").GetComponent<InteractionMode>().mode == InteractionMode.CameraMode.Magnifying_Glass)
            {
                if (Input.touchCount > 0)
                {
                    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        Vector3 p = nonVRCamera.ScreenToWorldPoint(new Vector3((Input.GetTouch(0).position.x + 20), (Input.GetTouch(0).position.y + 20), 5f));
                        print(p);

                        //p.y = p.y + 15;
                        //p.x = p.x + 15;
                        clone = Instantiate(cube, p, Quaternion.identity) as GameObject;


                    }
                    else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        Vector3 p = nonVRCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 5f));
                        clone.transform.position = p;
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        Destroy(clone);
                    }

                }
            }
            else
            {
                if (clone != null)
                    Destroy(clone);
            }

        }

        if(GameObject.Find("Save Button").GetComponent<Button>())
        {
            _buttonSave = GameObject.Find("Save Button").GetComponent<Button>();
            _buttonSave.onClick.AddListener(buttonSaveHandler);
        }
       
       


        /*else if (GameObject.Find("ARCamera").GetComponent<InteractionMode>().mode == InteractionMode.CameraMode.Full_Screen)
        {
            //Destroy(GetComponent<MagnifyingGlass>());
            //cube = GameObject.FindWithTag("Magnifying Glass_Phone");
            //clone = GameObject.FindWithTag("Magnifying Glass_Phone");
            //Destroy(cube);
            //Destroy(clone);
            //clone = GameObject.FindWithTag("Magnifying Glass_Phone");
            //Vector3 position = GameObject.FindWithTag("Magnifying Glass_Phone").transform.position;
            //clone.transform.position = new Vector3((position.x + 100), (position.y + 100), 5f);
        }*/


        /*
        if (Input.touchCount > 0)
        {

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Vector3 fingerPos = Input.GetTouch(0).position;
                fingerPos.z = 5;
                Vector3 objPos = Camera.main.ScreenToWorldPoint(fingerPos);
                Instantiate(cube, objPos, Quaternion.identity);
            }


            
        } 
        */
    }
}
