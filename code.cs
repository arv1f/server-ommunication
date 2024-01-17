using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using System.Net.Http.Headers;
using System;

public class api : MonoBehaviour
{
    private const string URL = "http://194.87.25.110:8000/api/ping";
    private static readonly HttpClient client = new HttpClient();
    private static TextMeshProUGUI textMeshProUGUI;
    private static bool hasInternet = false;

    private async void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        await GetQuery();
    }

    static async Task GetQuery()
    {
        fetch("ping", QueryType.GET);//Вызов значения пинга
        fetch("users/register", QueryType.POST, new string[] {SystemInfo.deviceUniqueIdentifier, "testuser"});// Регистрация пользователя
    }
    private static async Task fetch(string content, QueryType type, string[] data = null)
    {
        string URL = $"http://194.87.25.110:8000/api/{content}";
        switch (content)
        {
            case "ping":
                String pingContent = await client.GetStringAsync(URL);
                if (pingContent.Length > 0)
                {
                    setText(pingContent);
                    hasInternet = true;
                }
                else
                {
                    hasInternet = false;
                    textError();
                }
                break;
            case "users/register":
                if(!hasInternet) 
                {
                    textError();
                }
                var myContent = "{\"id\": \"" + data[0] + "\", \"username\": \"" + data[1] + "\"}";
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = client.PostAsync(URL, byteContent).Result;
                string registerContent = result.Content.ReadAsStringAsync().Result;
                setText(registerContent);
                break;
        }
    }     
    public static void textError()
    {
        setText("You can't get answer from server. Check your internet.\"");
    }

    private static void setText(string text)
    {
        textMeshProUGUI.text = text;
    }

}



