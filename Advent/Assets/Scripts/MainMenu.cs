using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public Button newGame;
    public Button quitGame;
    public Button continueGame;
    public Button deleteRecord;
    public Text record;


    // Start is called before the first frame update
    void Start()
    {
        newGame.onClick.AddListener(NewGame);
        quitGame.onClick.AddListener(Quit);
        continueGame.onClick.AddListener(Continue);
        deleteRecord.onClick.AddListener(DeleteRecord);
        string path = Application.persistentDataPath + "/game.sav";
        if (File.Exists(path))
        {
            continueGame.interactable = true;
        }
        else
        {
            continueGame.interactable = false;
        }

        if (PlayerPrefs.HasKey("d"))
        {
            record.text = PlayerPrefs.GetFloat("d").ToString() + " Days " + PlayerPrefs.GetFloat("h").ToString() + " Hours " + PlayerPrefs.GetFloat("m").ToString() + " Minutes";
        }


    }


    void DeleteRecord()
    {
        PlayerPrefs.DeleteAll();
        record.text = "None";
    }
    void Continue()
    {
        SaveSystem.DoLoad = true;
        SceneManager.LoadScene(1);
    }

    void NewGame()
    {
        SaveSystem.DoLoad = false;
        SceneManager.LoadScene(1);
    }

    void Quit()
    {
        Application.Quit();
    }
}
