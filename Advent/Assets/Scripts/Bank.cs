using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bank : MonoBehaviour
{
    public OverseerGlobalProcesses overseer;

    public Text moneyDisplay;

    void FixedUpdate()
    {
        moneyDisplay.text = "$" + overseer.money.ToString("N2");
    }
}
