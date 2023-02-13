using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePage : MonoBehaviour
{
    public Browser browser;
    public List<Button> homeButtons;

    void Start()
    {
        foreach (Button b in homeButtons)
        {
            if (b.gameObject.name == "Amazing")
            {
                b.onClick.AddListener(delegate { browser.GoSite("amazing.com"); });
            }
            else if (b.gameObject.name == "GrubRun")
            {
                b.onClick.AddListener(delegate { browser.GoSite("grubrun.com"); });
            }
            else if (b.gameObject.name == "Bank")
            {
                b.onClick.AddListener(delegate { browser.GoSite("bank.com"); });
            }
            else if (b.gameObject.name == "COIN")
            {
                b.onClick.AddListener(delegate { browser.GoSite("coin.com"); });
            }
            else if (b.gameObject.name == "Click")
            {
                b.onClick.AddListener(delegate { browser.GoSite("click.com"); });
            }
        }
    }
}
