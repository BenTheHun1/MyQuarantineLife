using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    public OverseerGlobalProcesses overseer;
    public GameObject declined;
    public GameObject notEnoughCoins;

    public Text moneyDisplay;
    public Text mvDisplay;
    public Button SellNow;
    public Button BuyNow;
    public Button aTSPlus;
    public Button aTSMinus;
    public Button aTSMax;
    public Text aTSDisplay;

    private int amntToSell;

    private void Start()
    {
        SellNow.onClick.AddListener(delegate { Sell(amntToSell); });
        BuyNow.onClick.AddListener(delegate { Buy(amntToSell); });
        aTSPlus.onClick.AddListener(delegate { ChangeAmount(amntToSell + 1); });
        aTSMinus.onClick.AddListener(delegate { ChangeAmount(amntToSell - 1); });
        aTSMax.onClick.AddListener(delegate { ChangeAmount(Mathf.FloorToInt(overseer.coin)); });
    }


    void Update()
    {
        moneyDisplay.text = overseer.coin.ToString("N2");
        mvDisplay.text = "$" + overseer.coinMarketValue.ToString("N2");
        aTSDisplay.text = amntToSell.ToString();
    }

    void ChangeAmount(int amntToChange)
    {
        amntToSell = amntToChange;
        if (amntToSell < 0)
        {
            amntToSell = 0;
        }
    }

    void Buy(float amnt)
    {
        if (overseer.money >= overseer.coinMarketValue * amnt)
        {
            overseer.coin += amnt;
            overseer.money -= overseer.coinMarketValue;
        }
        else
        {
            if (declined.activeSelf != true)
            {
                StartCoroutine(Declined());
            }
        }
    }

    void Sell(float amnt)
    {
        if (amnt <= overseer.coin)
        {
            overseer.money += amnt * overseer.coinMarketValue;
            overseer.coin -= amnt;
        }
        else
        {
            if (notEnoughCoins.activeSelf != true)
            {
                StartCoroutine(DeclinedCoin());
            }
        }
    }

    IEnumerator DeclinedCoin()
    {
        notEnoughCoins.SetActive(true);
        yield return new WaitForSeconds(1);
        notEnoughCoins.SetActive(false);
    }


    IEnumerator Declined()
    {
        declined.SetActive(true);
        yield return new WaitForSeconds(1);
        declined.SetActive(false);
    }
}
