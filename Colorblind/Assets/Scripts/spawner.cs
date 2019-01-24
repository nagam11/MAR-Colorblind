using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{

    public GameObject cube;
    public GameObject clone;
    public Camera nonVRCamera;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            
            //Vector3 clickPosition = -Vector3.one;
            //clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 20f)); //5f
            //Instantiate(cube, clickPosition, Quaternion.identity);
            //Debug.Log(clickPosition);
            
            Vector3 clickPosition = -Vector3.one;
            Vector3 v = Input.mousePosition;
            Debug.Log(v);
            v.z = 10;
            clickPosition = Camera.main.ScreenToWorldPoint(v); //5f
            Instantiate(cube, clickPosition, Quaternion.identity);
            Debug.Log(clickPosition);
        }
        */
        /*
        if (Input.GetMouseButtonDown(0))
        {
            print(Input.mousePosition);
            Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            print(p);
          

            Instantiate(cube, new Vector3(p.x, p.y, 0.0f), Quaternion.identity);

        }
        */
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
