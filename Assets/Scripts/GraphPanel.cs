using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class GraphPanel : MonoBehaviour{

    private int indexChart;

    private static GraphPanel instance;
    [SerializeField] private Sprite dotSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;

    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;

    private GameObject tooltipGameObject;

    private List<GameObject> gameObjectList;

    [SerializeField] Dropdown dropdown;
    GameObject dropdowGameObject;

    private List<int> valueList;
    private IGraphVisual graphVisual;
    private int maxVisibleValueAmount;
    private Func<int, string> getAxisLabelX = null;
    private Func<float, string> getAxisLabelY = null;


    private void Awake(){
        instance = this;
        dropdowGameObject = GameObject.Find("Dropdown");
        dropdown = dropdowGameObject.GetComponent<Dropdown>();

        graphContainer = transform.Find("container").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();

        tooltipGameObject = graphContainer.Find("tooltip").gameObject;

        dropdown.onValueChanged.AddListener(delegate {
            //DropdownValueChanged(dropdown);
        });

        List<int> valueList = new
            List<int>() { 5, 160, 16, 75, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33, 100, 22 };

        IGraphVisual graphVisualObj;

        gameObjectList = new List<GameObject>();

        graphVisualObj = new BarChartVisual(graphContainer, Color.green, .8f);

        transform.Find("barChartBtn").GetComponent<Button>().onClick.AddListener(()=> {
            graphVisualObj = new BarChartVisual(graphContainer, Color.green, .8f);
            SetGraphVisual(graphVisualObj);
        });

        transform.Find("lineChartBtn").GetComponent<Button>().onClick.AddListener(()=> {
            graphVisualObj = new LineGraphVisual(graphContainer, dotSprite, new Color(0, 0, 0, 0.0f), Color.white);
            SetGraphVisual(graphVisualObj);
           
        });

        transform.Find("increaseVisibleAmountBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            SetVisibleAmount(true);
        });

        transform.Find("decreaseVisibleAmountBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            SetVisibleAmount(false);
        });

        ShowGraph(valueList, graphVisualObj, -1, (int _i) => "Day " + (_i + 1), (float _f) => "$" + Mathf.RoundToInt(_f));


        switch (indexChart)
        {

            case 0:
                //graphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.green, Color.white);
                break;
            case 1:
                //graphVisual = new BarChartVisual(graphContainer, Color.green, .7f);
                break;
            default:
                //Debug.Log(indexChart);
                return;

        }

        //ShowGraph(valueList, graphVisual,-1 ,(int _i) => "Day " + (_i + 1), (float _f) => "$" + Mathf.RoundToInt(_f));


    }

    /*
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


    }*/

    public static void HideTooltip_static()
    {
        instance.HideTooltip();
    }


    public static void ShowTooltip_static(string tooltipText, Vector2 anchoredPosition)
    {
        instance.ShowTooltip(tooltipText, anchoredPosition);
    }

    private void ShowTooltip(string tooltipText, Vector2 anchoredPosition){
        tooltipGameObject.SetActive(true);

        tooltipGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

        Text tooltipUIText = tooltipGameObject.transform.Find("text").GetComponent<Text>();
        
        tooltipUIText.text = tooltipText;
        float textPaddingSize = 4f;
        Vector2 backgroundSize = 
            new Vector2(
                tooltipUIText.preferredWidth + textPaddingSize *2f,
                tooltipUIText.preferredHeight + textPaddingSize * 2f);

        tooltipGameObject.transform.Find("background").GetComponent<RectTransform>().sizeDelta = backgroundSize;
        tooltipGameObject.transform.SetAsLastSibling();
    
    }

    private void HideTooltip()
    {
        tooltipGameObject.SetActive(false);
    }

    private void SetGetAxisLabelX(Func<int, string> getAxisLabelX)
    {

    }

    private void SetGetAxisLabelY(Func<float, string> getAxisLabelY){

    }


    private void SetVisibleAmount(bool plus){
        Debug.Log(graphVisual.GetType());
        if(plus)
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount+1, this.getAxisLabelX, this.getAxisLabelY);
        else if(maxVisibleValueAmount>1 && graphVisual is BarChartVisual)
            ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount-1, this.getAxisLabelX, this.getAxisLabelY);
        else if (maxVisibleValueAmount>2 && graphVisual is LineGraphVisual)
            ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount - 1, this.getAxisLabelX, this.getAxisLabelY);
    }




    private void SetGraphVisual(IGraphVisual graphVisual)
    {
        ShowGraph(this.valueList, graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, this.getAxisLabelY);
    }


    public void ShowGraph(List<int> list, IGraphVisual graphVisual ,int maxVisibleValueAmount = -1 ,Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null){
        this.valueList = list;
        this.graphVisual = graphVisual;
        
        this.getAxisLabelX = getAxisLabelX;
        this.getAxisLabelY = getAxisLabelY;


        if(getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }

        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        if(maxVisibleValueAmount <= 0)
        {
            maxVisibleValueAmount = list.Count;
        }

        if(maxVisibleValueAmount > list.Count)
        {
            maxVisibleValueAmount = list.Count;
        }

       
        this.maxVisibleValueAmount = maxVisibleValueAmount;

        foreach (GameObject obj in gameObjectList)
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
        graphVisual.DeleteNode();

        for (int i = Mathf.Max(list.Count - maxVisibleValueAmount, 0); i < list.Count; i++) {

            float xPosition = xIndex * xSize;
            float yPosition = ((list[i] - yMinimum) / (yMaximum -yMinimum)) * graphHeight;

            string tooltipText = getAxisLabelY(list[i]);
            gameObjectList.AddRange(graphVisual.AddGraphVisual(new Vector2(xPosition, yPosition), xSize, tooltipText));

            foreach(GameObject obj in gameObjectList){
                if(obj.name == "dot")obj.transform.SetAsLastSibling();
            }

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
        List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth, string tooltipText);
        void DeleteNode();

    }

    private interface IGraphVisualObject
    {
        void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipText);
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

        public void DeleteNode() {}

        public List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPoisitionWidth, string tooltipText)
        {
            GameObject barGameObject = CreateBar(graphPosition, graphPoisitionWidth);
            Button_UI barButtonUI = barGameObject.AddComponent<Button_UI>();


            barButtonUI.MouseOverOnceFunc += () => {
                ShowTooltip_static(tooltipText,graphPosition);
            };

            barButtonUI.MouseOutOnceFunc += () =>
            {
                HideTooltip_static();
            };



            return new List<GameObject> { barGameObject };
        }

        public GameObject CreateBar(Vector2 graphPosition, float barWdith)
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

        public void DeleteNode()
        {
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


        public List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth, string tooltipText)
        {

            List<GameObject> gameObjectList = new List<GameObject>();
            
            GameObject dotGameObject = CreateDot(graphPosition);

            Button_UI dotButtonUI = dotGameObject.AddComponent<Button_UI>();


            dotButtonUI.MouseOverOnceFunc += () => {
                ShowTooltip_static(tooltipText, graphPosition);
            };

            dotButtonUI.MouseOutOnceFunc += () =>
            {
                HideTooltip_static();
            };


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
