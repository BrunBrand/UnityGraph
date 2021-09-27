using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphPanel : MonoBehaviour{

    private int indexChart;

    [SerializeField] private Sprite dotSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;

    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;

    private List<GameObject> gameObjectList;

    [SerializeField] Dropdown dropdown;
    GameObject dropdowGameObject;

    private void Awake(){

        dropdowGameObject = GameObject.Find("Dropdown");
        dropdown = dropdowGameObject.GetComponent<Dropdown>();

        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });

        graphContainer = transform.Find("container").GetComponent<RectTransform>();
        labelTemplateX  = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY  = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();

        gameObjectList = new List<GameObject>();

        
        List<int> valueList = new 
            List<int>(){5,160,16,75,30,22,17,15,13,17,25,37,40,36,33,100, 22};


        IGraphVisual graphVisual;
        switch (indexChart)
        {

            case 0:
                graphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.green, Color.white);
                break;
            case 1:
                graphVisual = new BarChartVisual(graphContainer, Color.green, .7f);
                break;
            default:
                Debug.Log(indexChart);
                return;

        }

        ShowGraph(valueList, graphVisual,-1 ,(int _i) => "Day " + (_i + 1), (float _f) => "$" + Mathf.RoundToInt(_f));


    }

    void DropdownValueChanged(Dropdown change){

        List<int> valueList = new
            List<int>() { 5, 160, 16, 75, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33, 100, 22 };

        IGraphVisual graphVisual;
        switch (change.value)
        {

            case 0:
                graphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.green, Color.white);
                break;
            case 1:
                graphVisual = new BarChartVisual(graphContainer, Color.green, .7f);
                break;
            default:
                Debug.Log(indexChart);
                return;

        }

        ShowGraph(valueList, graphVisual, -1, (int _i) => "Day " + (_i + 1), (float _f) => "$" + Mathf.RoundToInt(_f));


    }




    public void ShowGraph(List<int> list, IGraphVisual graphVisual ,int maxVisibleValueAmount = -1 ,Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null){
        
        if(getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }

        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        maxVisibleValueAmount = maxVisibleValueAmount <= 0 ? list.Count : maxVisibleValueAmount;

        foreach(GameObject obj in gameObjectList)
        {
            Destroy(obj);
        }
        gameObjectList.Clear();



        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;

        

        float yMaximum = list[0];
        float yMinimum = list[0];

        for(int i = Mathf.Max(list.Count - maxVisibleValueAmount,0); i<list.Count;i++)
        {
            int value = list[i];
            if(value > yMaximum) yMaximum = value;
            if (value < yMinimum) yMinimum = value;
        }


        float yDifference = yMaximum - yMinimum;
        yDifference = yDifference <= 0 ? 5f : yDifference;

        yMaximum += yDifference * .2f;
        yMinimum = 0f;

        float xSize = graphWidth/(maxVisibleValueAmount +1);

        //GameObject lastCircle = null;


        int xIndex = 0;

       
        for (int i = Mathf.Max(list.Count - maxVisibleValueAmount, 0); i < list.Count; i++) {

            float xPosition = xIndex * xSize;
            float yPosition = ((list[i] - yMinimum) / (yMaximum -yMinimum)) * graphHeight;

            //gameObjectList.AddRange(barChartVisual.AddGraphVisual(new Vector2(xPosition, yPosition), xSize));
            gameObjectList.AddRange(graphVisual.AddGraphVisual(new Vector2(xPosition, yPosition), xSize));

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -10f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);

            /*
            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(container, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -7f);
            */
            xIndex++;
        }

        int separatorCount = 10;
        separatorCount = separatorCount > (int)yDifference ? (int) yDifference : separatorCount;

        for(int i = 0; i <= separatorCount; i++){
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));

            gameObjectList.Add(labelY.gameObject);

            /*
            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(container, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
            */


        }


        
    }




    public interface IGraphVisual
    {
        List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth);
    }





    private class BarChartVisual : IGraphVisual
    {

        private RectTransform graphContainer;
        private Color barColor;
        private float barWidthMultiplier;


        public BarChartVisual(RectTransform graphContainer, Color barColor, float barWidthMultiplier)
        {
            this.graphContainer = graphContainer;
            this.barColor = barColor;
            this.barWidthMultiplier = barWidthMultiplier;
        }

        public List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPoisitionWidth)
        {
            GameObject barGameObject = CreateBar(graphPosition, graphPoisitionWidth);
            return new List<GameObject> { barGameObject };
        }

        private GameObject CreateBar(Vector2 graphPosition, float barWdith)
        {
            GameObject obj = new GameObject("bar", typeof(Image));

            obj.transform.SetParent(graphContainer, false);
            obj.GetComponent<Image>().color = barColor;

            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(barWdith *barWidthMultiplier, graphPosition.y);
            rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
            rectTransform.pivot = new Vector2(0f, 0f);
            return obj;
        }
    }


    private class LineGraphVisual : IGraphVisual
    {

        private RectTransform graphContainer;
        private Sprite dotSprite;
        private GameObject lastDotConnection;
        private Color dotColor;
        private Color dotColorConnection;
        private Color fade;


        public LineGraphVisual(RectTransform graphContainer, Sprite dotSprite, Color dotColor, Color dotColorConnection)
        {
            this.graphContainer = graphContainer;
            this.dotSprite = dotSprite;
            this.dotColorConnection = dotColorConnection;
            this.dotColor = dotColor;
            lastDotConnection = null;
        }

        public LineGraphVisual(RectTransform graphContainer, Sprite dotSprite, Color dotColor, Color dotColorConnection, Color fade)
        {
            this.graphContainer = graphContainer;
            this.dotSprite = dotSprite;
            this.dotColorConnection = dotColorConnection;
            this.dotColor = dotColor;
            this.fade = fade;
            lastDotConnection = null;
        }


        public List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth)
        {

            List<GameObject> gameObjectList = new List<GameObject>();
            
            GameObject dotGameObject = CreateDot(graphPosition);
            gameObjectList.Add(dotGameObject);
            if (lastDotConnection != null){
                GameObject dotConnection = CreateLineConnection(lastDotConnection.GetComponent<RectTransform>().anchoredPosition, dotGameObject.GetComponent<RectTransform>().anchoredPosition);
                gameObjectList.Add(dotConnection);      
            }

            lastDotConnection = dotGameObject;
            return gameObjectList;
            
        }

      
        private GameObject CreateDot(Vector2 anchoredPosition)
        {
            GameObject gameObject = new GameObject("dot", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().sprite = dotSprite;
            gameObject.GetComponent<Image>().color = dotColor;


            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(11, 11);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);

            return gameObject;
        }

        private GameObject CreateLineConnection(Vector2 positionA, Vector2 positionB)
        {
            GameObject obj = new GameObject("dotConnection", typeof(Image));

            obj.transform.SetParent(graphContainer, false);
            obj.GetComponent<Image>().color = dotColorConnection;
            Vector2 dir = (positionB - positionA).normalized;
            float distance = Vector2.Distance(positionA, positionB);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(distance, 3f);
            rectTransform.anchoredPosition = positionA + dir * distance * .5f;

            rectTransform.localEulerAngles = new Vector3(0, 0, angle);

            return obj;
        }




    }


    public void setChart(int index)
    {
        this.indexChart = index;
    }

    public int getChart()
    {
        return this.indexChart;
    }


    // Update is called once per frame
    void Update(){
        
    }
}
