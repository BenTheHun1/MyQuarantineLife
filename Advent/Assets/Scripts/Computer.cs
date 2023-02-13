using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Computer : MonoBehaviour
{
    public OverseerGlobalProcesses overseer;

    public GameObject TimeDisplay;

    public Button BrowserButton;
    public GameObject BrowserWindow;
    public Button CloseBrowser;

    public Button MinerButton;
    public GameObject MinerWindow;
    public Button CloseMiner;

    public AudioSource keyboardsounds;


    // Start is called before the first frame update
    void Start()
    {
        BrowserButton.onClick.AddListener(OpenBrowser);
        CloseBrowser.onClick.AddListener(OpenBrowser);

        MinerButton.onClick.AddListener(OpenMiner);
        CloseMiner.onClick.AddListener(OpenMiner);

        StartCoroutine(keySounds());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 time = overseer.time;
        TimeDisplay.GetComponent<Text>().text = time.y.ToString() + ":" + time.z.ToString("00");

        
    }

    IEnumerator keySounds()
    {
        while (overseer.isAlive)
        {
            if (overseer.modeWalk == false && Input.anyKey && !Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1))
            {
                keyboardsounds.mute = false;
            }
            else
            {
                keyboardsounds.mute = true;
            }

            yield return new WaitForSeconds(.1f);

        }
        
    }

    void OpenBrowser()
    {
        if (overseer.isAlive)
        {
            if (BrowserWindow.transform.localPosition.x == -10000f)
            {
                BrowserWindow.transform.localPosition = new Vector3(0f, BrowserWindow.transform.localPosition.y, 0);
            }
            else
            {
                BrowserWindow.transform.localPosition = new Vector3(-10000f, BrowserWindow.transform.localPosition.y, 0);
            }
        }
       
    }

    void OpenMiner()
    {
        if (overseer.isAlive)
        {
            Debug.Log(MinerWindow.transform.localPosition.x);
            if (MinerWindow.transform.localPosition.x == -10000f)
            {
                MinerWindow.transform.localPosition = new Vector3(-200f, MinerWindow.transform.localPosition.y, 0);
            }
            else
            {
                MinerWindow.transform.localPosition = new Vector3(-10000f, MinerWindow.transform.localPosition.y, 0);
            }
            Debug.Log(MinerWindow.transform.localPosition.x);
        }
    }
}
