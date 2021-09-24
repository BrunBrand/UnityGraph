using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphPanel : MonoBehaviour{

    [SerializeField] private Sprite circleSprite;
    private RectTransform container;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;

    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;

    private void Awake(){
        container = transform.Find("container").GetComponent<RectTransform>();
        labelTemplateX  = container.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY  = container.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = container.Find("dashTemplateY").GetComponent<RectTransform>();
        dashTemplateY = container.Find("dashTemplateX").GetComponent<RectTransform>();

        List<int> valueList = new 
            List<int>(){5,38,16,75,30,22,17,15,13,17,25,37,40,36,33,100, 22, 56, 10};

        ShowGraph(valueList);
    }

    private GameObject CreateCircle(Vector2 anchoredPosition){
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(container, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return gameObject;
    }


    public void ShowGraph(List<int> list)
    {
        float graphHeight = container.sizeDelta.y;
        float graphWidth = container.sizeDelta.x;
        float yMaximum = 100f;
        float xSize = 30f;

        GameObject lastCircle = null;

        for (int i = 0; i < list.Count; i++) {

            float xPosition = i * xSize;
            float yPosition = (list[i] / yMaximum) * graphHeight;

            GameObject circle = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastCircle != null)
                CreateLineConnection(lastCircle.GetComponent<RectTransform>().anchoredPosition, circle.GetComponent<RectTransform>().anchoredPosition);


            lastCircle = circle;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(container);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -10f);
            labelX.GetComponent<Text>().text = i.ToString();
            /*
            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(container, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -7f);
            */
        }

        int separatorCount = 10;
        for(int i = 0; i < separatorCount; i++){
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(container, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = Mathf.RoundToInt(normalizedValue * yMaximum).ToString();
            /*
            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(container, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
            */
            

        }


        
    }


    private void CreateLineConnection(Vector2 positionA, Vector2 positionB)
    {
        GameObject obj = new GameObject("dotConnection", typeof(Image));
        
        obj.transform.SetParent(container, false);
        obj.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        Vector2 dir = (positionB - positionA).normalized;
        float distance = Vector2.Distance(positionA, positionB);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = positionA + dir  * distance * .5f;
        
        rectTransform.localEulerAngles = new Vector3(0, 0, angle);

    }


    // Update is called once per frame
    void Update(){
        
    }
}
