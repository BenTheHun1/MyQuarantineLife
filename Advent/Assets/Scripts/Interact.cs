using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public string saveName;

    public string HUD;
    public string HUDAction;
    private OverseerGlobalProcesses overseer;

    public bool isComputer;

    public bool isPickable;
    public bool isThrowable;
    public bool isKickable;

    public bool isUseable;
    public GameObject toggler;
    public AudioSource toggleSound;
    public bool muteInstead;

    public bool isConsumable;
    public string consumeType;
    public float sanityModifier;
    public float consumeValue;

    public bool isPhone;
    public bool isPhoneTalking;
    public AudioClip ring;
    public AudioClip pickUp;
    public AudioClip call;

    // Start is called before the first frame update
    void Start()
    {
        overseer = GameObject.Find("Overseer").GetComponent<OverseerGlobalProcesses>();
    }

    // Update is called once per frame
    void Update()
    {
        if (overseer.isAlive)
        {
            if (isPickable && Input.GetKeyDown(KeyCode.E) && overseer.heldObject == null && overseer.highlightedObject == gameObject)
            {
                overseer.heldObject = overseer.cam.objectHit.gameObject;
                overseer.heldObject.transform.parent = overseer.cam.gameObject.transform;
                overseer.heldObject.transform.localPosition = new Vector3(0.5f, -0.3f, 0.75f);
                overseer.heldObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                overseer.heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                overseer.heldObject.GetComponent<Rigidbody>().freezeRotation = true;
                overseer.heldObject.GetComponent<Rigidbody>().useGravity = false;
                overseer.heldObject.transform.GetComponentInChildren<Collider>().enabled = false;
            }

            if (overseer.heldObject == gameObject)
            {
                if (isThrowable)
                {
                    overseer.controls.text = "[Q] Throw\n" + HUDAction;
                }
                else
                {
                    overseer.controls.text = HUDAction;
                }
            }

            if (isComputer && Input.GetKeyDown(KeyCode.E) && overseer.highlightedObject == gameObject) {
                overseer.ModeChange("Comp");
            }

            if (isThrowable && Input.GetKeyDown(KeyCode.Q) && overseer.heldObject == gameObject)
            {
                gameObject.GetComponent<Rigidbody>().freezeRotation = false;
                gameObject.transform.GetComponentInChildren<Collider>().enabled = true;
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                gameObject.GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * 5, overseer.ray.point, ForceMode.Impulse);
                gameObject.transform.parent = gameObject.transform.parent.transform.parent.transform.parent;
                overseer.heldObject = null;
                overseer.controls.text = "";

            }
            if (isKickable && Input.GetKeyDown(KeyCode.E) && overseer.highlightedObject == gameObject)
            {
                gameObject.GetComponent<Rigidbody>().rotation = GameObject.Find("Player").transform.rotation;
                gameObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 20, ForceMode.Impulse);
                overseer.GainSanity("exercise", sanityModifier);
            }

            if (isUseable && Input.GetKeyDown(KeyCode.E) && overseer.highlightedObject == gameObject)
            {
                toggler.SetActive(!toggler.activeSelf);
                if (toggleSound != null)
                {
                    if (!muteInstead)
                    {
                        toggleSound.Play();
                    }
                    else
                    {
                        toggleSound.mute = !toggleSound.mute;
                    }
                }
               
            }

            if (isConsumable && Input.GetKeyDown(KeyCode.R) && overseer.heldObject == gameObject)
            {
                if (consumeType == "health")
                {
                    overseer.health = Mathf.Clamp(overseer.health + consumeValue, 0f, 100f);
                }
                else if (consumeType == "hunger")
                {
                    overseer.hunger = Mathf.Clamp(overseer.hunger + consumeValue, 0f, 100f);
                }
                else if (consumeType == "thirst")
                {
                    overseer.thirst = Mathf.Clamp(overseer.thirst + consumeValue, 0f, 100f);
                }
                overseer.allObjects.Remove(gameObject);
                Destroy(gameObject);
                overseer.GainSanity("food", sanityModifier);
                overseer.heldObject = null;
                overseer.controls.text = "";
            }

            if (isPhone && Input.GetKeyDown(KeyCode.E) && overseer.highlightedObject == gameObject && !isPhoneTalking && !overseer.heardPhoneCall)
            {
                isPhoneTalking = true;
                StartCoroutine(phone());
            }

        }

    }
    IEnumerator phone()
    {
        AudioSource phoneAudio = gameObject.GetComponent<AudioSource>();
        phoneAudio.clip = pickUp;
        phoneAudio.loop = false;
        phoneAudio.Play();
        yield return new WaitForSeconds(1);
        phoneAudio.clip = call;
        phoneAudio.Play();
        overseer.heardPhoneCall = true;
    }
}
