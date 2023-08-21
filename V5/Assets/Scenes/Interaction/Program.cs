using UnityEngine;
using System;
using System.Net.Http;
using System.Threading.Tasks;


class Endpoint
{
    public string s;
    public async void FunctEndPoint()
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

                    s = responseBody;

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
