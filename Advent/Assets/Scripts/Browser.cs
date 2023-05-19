using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Browser : MonoBehaviour
{
    public OverseerGlobalProcesses overseer;

    public InputField URL;
    public GameObject fourOhFour;
    public GameObject home;
    public Button homeButton;

    public Button clickRanking;

    public GameObject searchResult;

    public List<GameObject> allSites;
    public GameObject curSite;

    // Start is called before the first frame update
    void Start()
    {
        homeButton.onClick.AddListener(GoHome);
        clickRanking.onClick.AddListener(delegate { GoSite("click.com/leaderboard"); });
        URL.onEndEdit.AddListener(delegate { GoSite(URL.text); });

        foreach (GameObject i in allSites)
        {
            i.SetActive(false);
        }
        curSite = home;
        GoHome();
    }

    public void GoSite(string term)
    {
		if (term == "HomePage")
		{
			GoHome();
			return;
		}
        bool foundIt = false;
        foreach (GameObject i in allSites)
        {
            if (term == i.name  || term == "https://" + i.name || term == "https://www." + i.name)
            {
                URL.text = "https://www." + i.name;
                curSite.SetActive(false);
                curSite = i;
                curSite.SetActive(true);
                foundIt = true;
                break;
            }
        }
        if (!foundIt)
        {
			if (curSite)
			{
				curSite.SetActive(false);
			}
            curSite = allSites[4];
            curSite.SetActive(true);
            Search(URL.text);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //URL.DeactivateInputField();
        }
    }


    void Search(string term)
    {
        foreach (Transform child in allSites[4].transform.GetChild(1).transform)
        {
            if (child.gameObject.name != "Text")
            {
                Destroy(child.gameObject);
            }
        }

        bool foundAtLeastOne = false;
        int nextY = -100;
        //List<GameObject> results;

        foreach (GameObject i in allSites)
        {
            if (i.name.Contains(term))
            {
                GameObject newResult = Instantiate(searchResult, new Vector3(0, nextY, 0), searchResult.transform.rotation);
                newResult.transform.GetChild(0).GetComponent<Text>().text = i.name;
                newResult.GetComponent<Button>().onClick.AddListener(delegate { GoSite(i.name); });
                newResult.transform.SetParent(allSites[4].transform.GetChild(1).transform, false);
                foundAtLeastOne = true;
                nextY -= 60;
            }
        }
        if (!foundAtLeastOne)
        {
            curSite.SetActive(false);
            curSite = fourOhFour;
            curSite.SetActive(true);
        }
    }

    void GoHome()
    {
        URL.text = "HomePage";
        curSite.SetActive(false);
        curSite = home;
        curSite.SetActive(true);
    }
}
