using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clicker : MonoBehaviour
{
    public OverseerGlobalProcesses overseer;
    public float clickStrength = 1f;
    public float ptsPerSec;

    public Text pointDisplay;
    public Button button;
    //public GameObject tooltip;


    public GameObject shopClickStrength;
    public GameObject shopAutoClick;
    public GameObject shopMK1;


    public int[] upClickStrength;
    public int[] upAutoClick;
    public int[] upMK1;




    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Tick());
        button.onClick.AddListener(Click);
        shopClickStrength.transform.GetComponentInChildren<Button>().onClick.AddListener(delegate { Buy("Click Strength"); });
        shopAutoClick.transform.GetComponentInChildren<Button>().onClick.AddListener(delegate { Buy("Auto Click"); });
        shopMK1.transform.GetComponentInChildren<Button>().onClick.AddListener(delegate { Buy("MK-1"); });
    }

    // Update is called once per frame
    void Update()
    {
        pointDisplay.text = overseer.points.ToString();
        shopClickStrength.transform.GetChild(1).gameObject.GetComponent<Text>().text = upClickStrength[0].ToString();
        shopAutoClick.transform.GetChild(1).gameObject.GetComponent<Text>().text = upAutoClick[0].ToString();
        shopMK1.transform.GetChild(1).gameObject.GetComponent<Text>().text = upMK1[0].ToString();
        //Vector2 localPoint;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(tooltip.GetComponent<RectTransform>(), Input.mousePosition, overseer.computer.transform.GetChild(0).gameObject.GetComponent<Camera>(), out localPoint);
        //tooltip.transform.localPosition = localPoint;
    }

    void Buy(string item)
    {
        if (item == "Click Strength" && upClickStrength[0] <= overseer.points)
        {
            clickStrength++;
            overseer.points -= upClickStrength[0];
            upClickStrength[0] = Mathf.FloorToInt(upClickStrength[0] * 1.25f);
        }
        else if (item == "Auto Click" && upAutoClick[0] <= overseer.points)
        {
            ptsPerSec += 1;
            overseer.points -= upAutoClick[0];
            upAutoClick[1]++; 
            upAutoClick[0] = Mathf.FloorToInt(upAutoClick[0] * 1.5f);
        }
        else if (item == "MK-1" && upMK1[0] <= overseer.points)
        {
            ptsPerSec += 10;
            overseer.points -= upMK1[0];
            upMK1[1]++;
            upMK1[0] = Mathf.FloorToInt(upMK1[0] * 1.75f);
        }
    }



    IEnumerator Tick()
    {
        while (overseer.isAlive)
        {
            yield return new WaitForSeconds(1);
            overseer.points += ptsPerSec;
        }
    }

    void Click()
    {
        if (overseer.isAlive)
        {
            overseer.points += clickStrength;
        }
    }


}
