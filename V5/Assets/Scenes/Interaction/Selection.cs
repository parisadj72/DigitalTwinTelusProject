using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;
using System.Net.Http;

public class SelectionScript : MonoBehaviour
{
    public UnityEvent action;
    public GameObject label;
    string throughput = "";
    //string UploadSpeed = "";

    // Start is called before the first frame update
    void Start()
    {
         //label = GameObject.Find("Label");

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
                Debug.Log(obj.transform.position);

                
                GameObject labelDup = Instantiate(GameObject.Find("Label"));
                //labelDup..GetComponent<Renderer>() = "Please Wait...";
                //labelDup.GetComponent<TextMeshProUGUI>().text  = "Please Wait...";
                labelDup.GetComponent<TextMeshPro>().SetText("Please Wait...");
                labelDup.transform.position = new Vector3(obj.GetComponent<Renderer>().bounds.center.x, obj.GetComponent<Renderer>().bounds.center.y + 1f, obj.GetComponent<Renderer>().bounds.center.z);

                
                FunctEndPoint();

                labelDup.GetComponent<TextMeshPro>().SetText(throughput);

                /*GameObject newGO = new GameObject("myTextGO");
                obj.transform.SetParent(this.transform);

                Text myText = obj.AddComponent<Text>();
                myText.text = "Ta-dah!";

                myText.transform.position = obj.transform.position;
                */

                //changing color
                //Color newColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                //obj.GetComponent<MeshRenderer>().material.color = newColor;

                //obj.GetComponent<SelectionScript>().action.Invoke();
            }
        }
    //    if (Input.touchCount >= 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    //    {
    //        Debug.Log("Tap is Working..........");
     
    //        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
    //        RaycastHit hit;
    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            Debug.Log("Physics.Raycast is Working..........");
    //            if (hit.transform.tag == "box")
    //            {
    //                Debug.Log("hit is Working..........");
    //                Color newColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    //                hit.collider.GetComponent<MeshRenderer>().material.color = newColor;

    //                //hit.collider.GetComponent<RotateBox>().ChangeBoo1();
    //            }
    //        }
    //    }
    }

    public async void FunctEndPoint()
    {
        string endpointUrl = "https://c17d-136-159-160-121.ngrok-free.app";

        using (HttpClient client = new HttpClient())
        {
            try
            {

                HttpResponseMessage response = await client.GetAsync(endpointUrl);


                if (response.IsSuccessStatusCode)
                {

                    string responseBody = await response.Content.ReadAsStringAsync();
                    throughput = responseBody;
                    Debug.Log(responseBody);

                }
                else
                {
                    Console.WriteLine($"Failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

}
