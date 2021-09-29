using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;



[Serializable]

public class T2MValue
{
    public int value;
}

[Serializable]
public class T2MKey{
    public string key;
}

[Serializable]
public class T2MClass
{
    public Dictionary<string, int> data;
}


[Serializable]
public class Parameter {
    public T2MClass T2M;
}


[Serializable]
public class Properties{
    public Parameter parameter;

}


[Serializable]
public class EnergyData {
    public Properties properties;
    
}



