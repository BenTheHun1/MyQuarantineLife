using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Miner : MonoBehaviour
{
    public OverseerGlobalProcesses overseer;

    public int totalTrans = 0;
    public float totalIn = 0;
    private int secTrans = 0;
    private float secIn = 0;

    public Text txtTotalTrans;
    public Text txtTotalIn;
    public Text txtSecTrans;
    public Text txtSecIn;

    public Toggle ActiveToggle;
    public bool isActive;

    public Button helpButton;
    public GameObject helpWindow;
    public Button closeHelpWindow;

    // Start is called before the first frame update
    void Start()
    {
        helpButton.onClick.AddListener(delegate { helpWindow.SetActive(!helpWindow.activeSelf); });
        closeHelpWindow.onClick.AddListener(delegate { helpWindow.SetActive(!helpWindow.activeSelf); });
        ActiveToggle.onValueChanged.AddListener(delegate { isActive = ActiveToggle.isOn; });

        StartCoroutine(TickMiner());
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive && overseer.isAlive)
        {
            secTrans = 1;
        }
        else
        {
            secTrans = 0;
        }

        txtSecIn.text = secIn.ToString("N2");
        txtSecTrans.text = secTrans.ToString();
        txtTotalIn.text = totalIn.ToString("N2");
        txtTotalTrans.text = totalTrans.ToString();
    }

    IEnumerator TickMiner()
    {
        while (overseer.isAlive)
        {
            yield return new WaitForSeconds(1);
            secIn = secTrans * 0.01f;
            totalTrans += secTrans;
            totalIn += secIn;
            overseer.coin += secIn;
        }
    }
}
