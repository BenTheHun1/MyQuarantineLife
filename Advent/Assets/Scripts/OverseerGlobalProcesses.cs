using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.EventSystems;

public class OverseerGlobalProcesses : MonoBehaviour
{

    public Vector3 time = new Vector3(0, 8, 0);
    public GameObject PauseMenu;
    public Button btnResume;
    public Button btnQuit;
    public Button btnSave;
    public Button btnLoad;
    public bool paused = false;

    public bool heardPhoneCall;
    public AudioSource phoneAudioSource;

    public bool modeWalk;
    public GameObject highlightedObject;
    public List<GameObject> lights;

    public RaycastHit ray;
    public GameObject GameOver;
    public float health;
    public float hunger;
    public float thirst;
    public bool isAlive;
    public float money;
    public float coin;
    public float coinMarketValue;
    public float points;

    public float sanity;
    public string lastSanityActivity = "nothing";
    public int lastSanityActivityRepetition = 0;

    public GameObject heldObject;
    public GameObject walkModeOnly;
    public GameObject compModeOnly;
    public GameObject player;
    public MouseLook cam;

    public Clicker clicker;
    public Browser browser;
    public Miner miner;

    public GameObject computer;
    public InputField computerURL;
    public PostProcessVolume bloom;

    public TextMeshProUGUI controls;

    public Button btnMute;
    public Sprite spriteMute;
    public Sprite spriteSound;

    public List<GameObject> objPrefabs;
    public List<GameObject> allObjects;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine("Timekeeping");
        StartCoroutine("LightFlicker");

        btnResume.onClick.AddListener(PauseGame);
        btnSave.onClick.AddListener(SaveGame);
        btnLoad.onClick.AddListener(LoadGame);
        btnQuit.onClick.AddListener(delegate { Application.Quit(); });
        btnMute.onClick.AddListener(Mute);


        if (SaveSystem.DoLoad == true)
        {
            LoadGame();
        }
    }

    public void Mute()
    {
        AudioListener.pause = !AudioListener.pause;
        if (AudioListener.pause)
        {
            btnMute.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = spriteMute;
        }
        else
        {
            btnMute.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = spriteSound;
        }
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(this, player, clicker, browser, miner);
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (paused)
        {
            PauseGame();
        }
        
        time = new Vector3(data.time[0], data.time[1], data.time[2]);

        health = data.health;
        hunger = data.hunger;
        thirst = data.thirst;

        heardPhoneCall = data.heardPhoneCall;
        if (heardPhoneCall)
        {
            phoneAudioSource.playOnAwake = false;
            phoneAudioSource.Stop();
        }

        sanity = data.sanity;
        lastSanityActivity = data.lastSanityActivity;
        lastSanityActivityRepetition = data.lastSanityActivityRepetition;

        money = data.money;
        coin = data.coin;
        coinMarketValue = data.coinMarketValue;
        points = data.points;

        foreach (GameObject obj in allObjects)
        {
            Destroy(obj);
        }
        allObjects.Clear();


        if (data.objectName.Length > 0)
        {
            for (int i = 0; i < data.objectName.Length; i++)
            {
                GameObject objectToCreate = null;
                if (data.objectName[i] == "Flashlight")
                {
                    objectToCreate = objPrefabs[0];
                }
                else if (data.objectName[i] == "Ball")
                {
                    objectToCreate = objPrefabs[1];
                }
                else if (data.objectName[i] == "FastFood")
                {
                    objectToCreate = objPrefabs[2];
                }
                else if (data.objectName[i] == "ChineseFood")
                {
                    objectToCreate = objPrefabs[3];
                }
                else if (data.objectName[i] == "Soda")
                {
                    objectToCreate = objPrefabs[4];
                }
                allObjects.Add(Instantiate(objectToCreate, new Vector3(data.objectPos[i, 0], data.objectPos[i, 1], data.objectPos[i, 2]), Quaternion.Euler(data.objectRotation[i, 0], data.objectRotation[i, 1], data.objectRotation[i, 2])));

            }
        }
        //Debug.Log(data.playerPos[0]);
        Vector3 newPos;
        newPos.x = data.playerPos[0];
        newPos.y = data.playerPos[1];
        newPos.z = data.playerPos[2];
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = newPos;
        player.GetComponent<CharacterController>().enabled = true;
        //Debug.Log(player.transform.localPosition);
        //Debug.Log(player.transform);
        player.transform.rotation = Quaternion.Euler(0, data.playerRotationY, 0);
        player.transform.GetChild(1).transform.localRotation = Quaternion.Euler(data.cameraRotationX, 0, 0);
        //Debug.Log(player.transform.GetChild(1).name);

        if (data.modeWalk)
        {
            ModeChange("Walk");
        }
        else
        {
            ModeChange("Computer");
        }

        clicker.clickStrength = data.clickStrength;
        clicker.ptsPerSec = data.ptsPerSec;
        clicker.upClickStrength = data.upClickStrength;
        clicker.upAutoClick = data.upAutoClick;
        clicker.upMK1 = data.upMK1;

        browser.GoSite(data.currentSite);
        miner.ActiveToggle.isOn = data.isMinerActive;
        miner.totalIn = data.totalIn;
        miner.totalTrans = data.totalTrans;
		miner.secTrans = data.secTrans;
    }


    public void GainSanity(string activity, float value)
    {
        float sanityToAdd = value;
        if (lastSanityActivity == activity)
        {
            lastSanityActivityRepetition += 1;
            sanityToAdd /= lastSanityActivityRepetition;
        }
        else
        {
            lastSanityActivityRepetition = 0;
        }
        sanity = Mathf.Clamp(sanity + sanityToAdd, 0f, 100f);
        lastSanityActivity = activity;
        SanityCheck();
    }

    public void ModeChange(string newMode)
    {
        if (newMode == "Walk")
        {
            modeWalk = true;
            walkModeOnly.SetActive(true);
            compModeOnly.SetActive(false);
            player.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            computer.transform.GetChild(0).gameObject.SetActive(false);
            controls.text = "";
            
        }
        else
        {
            modeWalk = false;
            walkModeOnly.SetActive(false);
            compModeOnly.SetActive(true);
            player.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            computer.transform.GetChild(0).gameObject.SetActive(true);
            controls.text = "[TAB] Stand";
        }
    }

    IEnumerator LightFlicker()
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(1);
            if (Random.value <= 0.1)
            {
                int randomChoice = Random.Range(0, lights.Count);
                if (lights[randomChoice].activeSelf != false)
                {
                    lights[randomChoice].GetComponent<Light>().intensity /= 2f;
                    yield return new WaitForSeconds(0.1f);
                    lights[randomChoice].GetComponent<Light>().intensity *= 2f;
                }
            }
        }
    }

    IEnumerator Timekeeping()
    {
        while (isAlive)
        {
            if (Time.timeScale != 0)
            {
                yield return new WaitForSeconds(1);
                time = new Vector3(time.x, time.y, time.z + 1);
                if (time.z == 60)
                {
                    time = new Vector3(time.x, time.y + 1, 0);
                }
                if (time.y == 24)
                {
                    time = new Vector3(time.x + 1, 0, time.z);
                }

                if (!(hunger <= 0f))
                {
                    hunger -= 0.1f;
                }
                else
                {
                    hunger = 0;
                }

                if (!(thirst <= 0f))
                {
                    thirst -= 0.2f;
                }
                else
                {
                    thirst = 0;
                }

                if (!(sanity <= 0f))
                {
                    sanity -= 0.1f;
                }
                else
                {
                    sanity = 0;
                }
                SanityCheck();

                if (thirst == 0)
                {
                    health -= 0.1f;
                }
                if (hunger == 0)
                {
                    health -= 0.1f;
                }
                if (health <= 0)
                {
                    GameOver.SetActive(true);
                    isAlive = false;
                    if (PlayerPrefs.HasKey("d")) {
                        if (PlayerPrefs.GetFloat("d") < time.x)
                        {
                            PlayerPrefs.SetFloat("d", time.x);
                            PlayerPrefs.SetFloat("h", time.y);
                            PlayerPrefs.SetFloat("m", time.z);
                        }
                        else if (PlayerPrefs.GetFloat("d") == time.x && PlayerPrefs.GetFloat("h") < time.y)
                        {
                            PlayerPrefs.SetFloat("h", time.y);
                            PlayerPrefs.SetFloat("m", time.z);
                        }
                        else if (PlayerPrefs.GetFloat("d") == time.x && PlayerPrefs.GetFloat("h") == time.y && PlayerPrefs.GetFloat("m") < time.z)
                        {
                            PlayerPrefs.SetFloat("m", time.z);
                        }
                    }
                    else
                    {
                        PlayerPrefs.SetFloat("d", time.x);
                        PlayerPrefs.SetFloat("h", time.y);
                        PlayerPrefs.SetFloat("m", time.z);
                    }
                }

                coinMarketValue = Mathf.Clamp((Random.Range(-100, 101) / 10) + coinMarketValue, 0f, 10000000000f);
            }
        }

    }

    void SanityCheck()
    {
        bloom.profile.GetSetting<ChromaticAberration>().intensity.value = 5 - ((sanity / 100f) * 5);
        bloom.profile.GetSetting<Grain>().intensity.value = 5 - ((sanity / 100f) * 5);
        //Debug.Log(bloom.profile.GetSetting<Grain>().intensity.value);
        //Debug.Log(bloom.profile.GetSetting<ChromaticAberration>().intensity.value);

    }


    // Update is called once per frame
    void Update()
    {
        GameObject.Find("Health Bar").GetComponent<Slider>().value = health;
        GameObject.Find("HP Text").GetComponent<TextMeshProUGUI>().text = (Mathf.RoundToInt(health)).ToString();

        GameObject.Find("Hunger Bar").GetComponent<Slider>().value = hunger;
        GameObject.Find("Hunger Text").GetComponent<TextMeshProUGUI>().text = (Mathf.RoundToInt(hunger)).ToString();

        GameObject.Find("Thirst Bar").GetComponent<Slider>().value = thirst;
        GameObject.Find("Thirst Text").GetComponent<TextMeshProUGUI>().text = (Mathf.RoundToInt(thirst)).ToString();

        GameObject.Find("Psyche Bar").GetComponent<Slider>().value = sanity;
        GameObject.Find("Psyche Text").GetComponent<TextMeshProUGUI>().text = (Mathf.RoundToInt(sanity)).ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }


        if (Input.GetKeyDown(KeyCode.Tab) && !modeWalk && isAlive)
        {
            ModeChange("Walk");
			EventSystem.current.SetSelectedGameObject(null);
        }
    }

    void PauseGame ()
    {
        if (paused)
        {
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
            if (modeWalk)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            paused = false;
        }
        else
        {
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            paused = true;
        }
    }
}
