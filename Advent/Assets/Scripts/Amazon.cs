using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Amazon : MonoBehaviour
{
    public OverseerGlobalProcesses overseer;

    public GameObject declined;

    public List<Button> itemButtons;
    public List<GameObject> items;
    public List<float> prices;
    public AudioSource bell;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Button item in itemButtons)
        {
            item.onClick.AddListener(delegate { BuyItem(item); });
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void BuyItem(Button itemToBuy)
    {
        if (overseer.isAlive)
        {
            int index = itemButtons.IndexOf(itemToBuy);
            if (overseer.money >= prices[index])
            {
                overseer.allObjects.Add(Instantiate(items[index]));

                overseer.money -= prices[index];
                overseer.GainSanity("purchase", 5);
                bell.Play();
            }
            else
            {
                if (declined.activeSelf != true)
                {
                    StartCoroutine(Declined());
                }
            }
        }
    }

    IEnumerator Declined()
    {
        declined.SetActive(true);
        yield return new WaitForSeconds(1);
        declined.SetActive(false);
    }
}
