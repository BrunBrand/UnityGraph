using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Threading.Tasks;

public class WeatherController : MonoBehaviour
{
    private const string API_KEY = "a1c49c845cafeca063fe22b35f8b6a6f";
    private const float API_CHECK_MAXTIME = 10 * 60.0f; // 10 minutes
    private float apiCheckCountdown = API_CHECK_MAXTIME;

    public string cityName;
    public string stateCode;


    public GameObject SnowSystem;


    private async Task<WeatherInfo> GetWeather()
    {
        HttpWebRequest request =
            (HttpWebRequest)WebRequest.Create(string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}", cityName, API_KEY));

        HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(jsonResponse);
        return info;



    }



    public async void CheckSnowStatus(){

        WeatherInfo weatherInfo = await GetWeather();
        string _main;

        _main = weatherInfo.weather[0].main;

        bool snowing = _main.Equals("Snow");
        bool clear = _main.Equals("Clear");

        ParticleSystem.MainModule settings = SnowSystem.GetComponent<ParticleSystem>().main;

        if (snowing)
        {

            SnowSystem.SetActive(true);
            settings.startColor = new ParticleSystem.MinMaxGradient( new Color(255,255,255));
            Debug.Log("it is snowing");
        }

        if (clear)
        {
            SnowSystem.SetActive(true);
            settings.startColor = new ParticleSystem.MinMaxGradient(new Color(232, 221, 121));
            Debug.Log("it is clear sky");
        }

        else
        {
            SnowSystem.SetActive(false);
            Debug.Log(_main);
        }
    }

    private void Start()
    {
        CheckSnowStatus();
    }

    private void Update()
    {
        apiCheckCountdown -= Time.deltaTime;
        if (apiCheckCountdown <= 0)
        {
            CheckSnowStatus();
            apiCheckCountdown = API_CHECK_MAXTIME;
        }


    }






}


