using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectChart : MonoBehaviour{

    private int indexDropDown;
    GraphPanel graphPanel;
    [SerializeField]Dropdown dropDown;

   

    // Update is called once per frame
    void Update(){

        gameObject.GetComponent<GraphPanel>().setChart(dropDown.value);
    }
}
