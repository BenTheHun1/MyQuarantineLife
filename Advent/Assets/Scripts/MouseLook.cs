using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseLook : MonoBehaviour
{
    public OverseerGlobalProcesses overseer;

    public float mouseSensitivity;
    public Slider sensitivitySlider;
    public Transform playerBody;
    private float xRotation = 0f;
    private RaycastHit hit;
    public GameObject context;
    public Camera CompCam;

    public Transform objectHit;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (overseer.isAlive)
        {
            Ray ray = gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                overseer.ray = hit;
                objectHit = hit.transform;
                if (objectHit.gameObject.CompareTag("Object") && hit.distance < 2.5f)
                {
                    context.GetComponent<TextMeshProUGUI>().text = objectHit.gameObject.GetComponent<Interact>().HUD;
                    overseer.highlightedObject = objectHit.gameObject;
                }
                else
                {
                    context.GetComponent<TextMeshProUGUI>().text = "";
                    overseer.highlightedObject = null;
                }

            }


            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);

        }
        mouseSensitivity = sensitivitySlider.value;
    }
}
