using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouse : MonoBehaviour{


    void OnMouseOver(){
        Debug.Log("mouse is over it!");
    }

    void OnMouseExit()
    {
        Debug.Log("mouse has existed the object!");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
