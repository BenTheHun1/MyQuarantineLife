﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Miner : MonoBehaviour
{
    public OverseerGlobalProcesses overseer;

    public int totalTrans = 0;
    public float totalIn = 0;
    public int secTrans = 1;
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
        txtSecIn.text = secIn.ToString("N2");
		if (isActive) { txtSecTrans.text = secTrans.ToString(); }
        txtTotalIn.text = totalIn.ToString("N2");
        txtTotalTrans.text = totalTrans.ToString();
    }

    IEnumerator TickMiner()
    {
        while (overseer.isAlive)
        {
			yield return new WaitForSeconds(1);
			if (isActive)
			{
				secIn = secTrans * 0.01f;
				totalTrans += secTrans;
				totalIn += secIn;
				overseer.coin += secIn;
			}
        }
    }
}
