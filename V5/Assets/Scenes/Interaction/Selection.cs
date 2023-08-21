using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public class SelectionScript : MonoBehaviour
{
    public UnityEvent action;
    public GameObject label;
    public GameObject Sphere;
    static string throughput = "";

    float download = 0;

    float red = 0;
    float green = 0;
    float blue = 0;

    //string UploadSpeed = "";

    // Start is called before the first frame update
    void Start()
    {
        Sphere = GameObject.Find("Sphere");

    }
    async void Update()
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
                labelDup.transform.position = new Vector3(obj.GetComponent<Renderer>().bounds.center.x, obj.GetComponent<Renderer>().bounds.max.y + 1.5f, obj.GetComponent<Renderer>().bounds.center.z);

                string ans = await FunctEndPoint();

                var match = Regex.Match(ans, @"([-+]?[0-9]*\.?[0-9]+)");
                if (match.Success)
                    download = Convert.ToSingle(match.Groups[1].Value);

                //Debug.Log("Download::::::" + download);
                float value = ((download - 25) / (100 - 25));
                getValueBetweenTwoFixedColors(value);

                labelDup.GetComponent<TextMeshPro>().SetText(ans);
                GameObject SphereDup = Instantiate(GameObject.Find("Sphere"));
                SphereDup.transform.position = new Vector3(obj.GetComponent<Renderer>().bounds.center.x, obj.GetComponent<Renderer>().bounds.max.y + 0.5f, obj.GetComponent<Renderer>().bounds.center.z);

                SphereDup.GetComponent<Renderer>().material.SetColor("_Color", new Color(red, green, blue));


                // Call SetColor using the shader property name "_Color" and setting the color to red

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

    public async Task<string> FunctEndPoint()
    {
        string endpointUrl = "https://55c3-136-159-160-121.ngrok-free.app";

        using (HttpClient client = new HttpClient())
        {
            try
            {

                HttpResponseMessage response = await client.GetAsync(endpointUrl);


                if (response.IsSuccessStatusCode)
                {

                    string responseBody = await response.Content.ReadAsStringAsync();
                    throughput = responseBody;
                    //Debug.Log(responseBody);
                    return throughput;

                }
                else
                {
                    Console.WriteLine($"Failed: {response.StatusCode}");
                    return " ";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return " ";
            }
        }
    }

    void getValueBetweenTwoFixedColors(float value)
    {
        int aR = 0; int aG = 0; int aB = 255;  // RGB for our 1st color (blue in this case).
        int bR = 255; int bG = 0; int bB = 0;    // RGB for our 2nd color (red in this case).

        red = (float)(bR - aR) * value + aR;      // Evaluated as -255*value + 255.
        green = (float)(bG - aG) * value + aG;      // Evaluates as 0.
        blue = (float)(bB - aB) * value + aB;      // Evaluates as 255*value + 0.
    }

}