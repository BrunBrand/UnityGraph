using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Weather
{

    public int id;
    public string main;
    public string description;
    public string icon;


}

[Serializable]
public class WeatherInfo
{
    public int id;
    public string name;
    public List<Weather> weather;
}

