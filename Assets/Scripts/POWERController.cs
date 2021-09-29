using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using UnityEngine.UI;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

public class POWERController : MonoBehaviour{

    [SerializeField] private string parameters;
    [SerializeField] private string community;
    [SerializeField] private double longitude;
    [SerializeField] private double altitude;
    [SerializeField] private int start;
    [SerializeField] private int end;

    private List<DateTime> dateValues = new List<DateTime>();
    private List<double> valueData = new List<double>();

    public List<DateTime> DateValues
    {
        set { dateValues = value; }
        get { return dateValues; }
    }

    public List<double> ValueData
    {
        set { valueData = value; }
        get { return valueData; }
    }

    public async Task<List<double>> getValueData()
    {

        return valueData;
    }
    
    
    private async Task<Dictionary<string,double>> GetData()
    {
        
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
            string.Format("https://power.larc.nasa.gov/api/temporal/daily/point?parameters={0}&community={1}&longitude={2}&latitude={3}&start={4}&end={5}&format=JSON", parameters, community, longitude, altitude, start, end));
        HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();


        dynamic json = Newtonsoft.Json.Linq.JValue.Parse(jsonResponse);
        
        JObject t2m = json.properties.parameter.T2M;

        Dictionary<string, double> values = t2m.ToObject<Dictionary<string, double>>();

        return values;
    }
    

    public async Task<List<double>> CallAPI(){
        Dictionary<string, double> energyData = await GetData();
        Dictionary<string,double>.KeyCollection keys = energyData.Keys;
        Dictionary<string, double>.ValueCollection values = energyData.Values;

        int index = 0;
        

        List<DateTime> dateValues = new List<DateTime>(new DateTime[keys.Count]);
        List<double> valueData = new List<double>(new double[values.Count]);


        foreach(string date in keys){
            dateValues[index] = DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);
            index++;
        }
        index = 0;

        foreach (double value in values)
        {
            valueData[index] = value;
            index++;
        }

        foreach (DateTime da in dateValues)
        {
            //Debug.Log(da);
           
        }
        foreach (double val in valueData)
        {
            //Debug.Log(val);

        }
        return valueData;
     


    }


    private void Start()
    {
        //gameObject.GetComponent<Button>().onClick.AddListener(CallAPI);
        

    }




}
