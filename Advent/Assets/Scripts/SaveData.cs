using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    public float[] time;
    public int days;

    public float health;
    public float hunger;
    public float thirst;

    public bool heardPhoneCall;

    public float sanity;
    public string lastSanityActivity;
    public int lastSanityActivityRepetition;

    public float money;
    public float coin;
    public float coinMarketValue;
    public float points;

    public float[,] objectPos;
    public float[,] objectRotation;
    public string[] objectName;

    public float[] playerPos;
    public float playerRotationY;
    public float cameraRotationX;

    public bool modeWalk;

    public float clickStrength;
    public float ptsPerSec;
    public int[] upClickStrength;
    public int[] upAutoClick;
    public int[] upMK1;

    public string currentSite;
    public bool isMinerActive;
    public int totalTrans;
    public float totalIn;


    public SaveData(OverseerGlobalProcesses overseer, GameObject player, Clicker clicker, Browser browser, Miner miner)
    {
        time = new float[3];
        time[0] = overseer.time.x;
        time[1] = overseer.time.y;
        time[2] = overseer.time.z;

        health = overseer.health;
        hunger = overseer.hunger;
        thirst = overseer.thirst;

        heardPhoneCall = overseer.heardPhoneCall;

        sanity = overseer.sanity;
        lastSanityActivity = overseer.lastSanityActivity;
        lastSanityActivityRepetition = overseer.lastSanityActivityRepetition;

        money = overseer.money;
        coin = overseer.coin;
        coinMarketValue = overseer.coinMarketValue;
        points = overseer.points;

        int i = 0;
        objectPos = new float[overseer.allObjects.Count, 3];
        objectRotation = new float[overseer.allObjects.Count, 3];
        objectName = new string[overseer.allObjects.Count];
        foreach (GameObject obj in overseer.allObjects)
        {

            objectPos[i, 0] = obj.transform.position.x;
            objectPos[i, 1] = obj.transform.position.y;
            objectPos[i, 2] = obj.transform.position.z;
            objectRotation[i, 0] = obj.transform.rotation.eulerAngles.x;
            objectRotation[i, 1] = obj.transform.rotation.eulerAngles.y;
            objectRotation[i, 2] = obj.transform.rotation.eulerAngles.z;
            objectName[i] = obj.GetComponent<Interact>().saveName;
            i++;
        }
        
        playerPos = new float[3];
        playerPos[0] = player.transform.position.x;
        playerPos[1] = player.transform.position.y;
        playerPos[2] = player.transform.position.z;
        playerRotationY = player.transform.rotation.eulerAngles.y;
        cameraRotationX = player.transform.GetChild(1).transform.localRotation.eulerAngles.x;

        modeWalk = overseer.modeWalk;

        clickStrength = clicker.clickStrength;
        ptsPerSec = clicker.ptsPerSec;
        upClickStrength = new int[2];
        upClickStrength[0] = clicker.upClickStrength[0];
        upClickStrength[1] = clicker.upClickStrength[1];
        upAutoClick = new int[2];
        upAutoClick[0] = clicker.upAutoClick[0];
        upAutoClick[1] = clicker.upAutoClick[1];
        upMK1 = new int[2];
        upMK1[0] = clicker.upMK1[0];
        upMK1[1] = clicker.upMK1[1];

        currentSite = browser.curSite.name;
        isMinerActive = miner.isActive;
        totalIn = miner.totalIn;
        totalTrans = miner.totalTrans;
    }
}
