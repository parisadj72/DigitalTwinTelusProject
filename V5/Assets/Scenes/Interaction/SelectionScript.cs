using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectionScript : MonoBehaviour
{
    public UnityEvent action;
    // Start is called before the first frame update
    void Start()
    {

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GameObject obj = hit.collider.gameObject;
                Debug.Log(obj.name);
                Color newColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                obj.GetComponent<MeshRenderer>().material.color = newColor;
                //obj.GetComponent<SelectionScript>().action.Invoke();
            }
        }
        /*if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            //Debug.Log("Tap is Working..........");
     
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log("Physics.Raycast is Working..........");
                if (hit.transform.tag == "box")
                {
                    Debug.Log("hit is Working..........");
                    Color newColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                    hit.collider.GetComponent<MeshRenderer>().material.color = newColor;

                    //hit.collider.GetComponent<RotateBox>().ChangeBoo1();
                }
            }
        }*/
    }
           
}
