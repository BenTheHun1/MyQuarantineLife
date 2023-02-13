using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public OverseerGlobalProcesses overseer;

    public CharacterController controller;
    public MouseLook ml;

    private float speed = 3f;
    private float gravity = -9.81f * 3;
    private float jumpHeight = 0.4f;

    public Transform groundCheck;
    private float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Vector3 velocity;
    bool isOnGround;

    public GameObject model;
    private float desired_height = 0.5f;
    public float crouchStep;

    public AudioSource steps;

    // Update is called once per frame
    void Update()
    {
        if (overseer.isAlive)
        {
            isOnGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isOnGround && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            //Debug.Log(controller.velocity);
            if ((controller.velocity.x >= 1f || controller.velocity.x <= -1f)  && !steps.isPlaying)
            {
                steps.Play();
            }
            else if (controller.velocity == Vector3.zero && steps.isPlaying)
            {
                steps.Stop();
            }

            if (Input.GetButtonDown("Jump") && isOnGround)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }


            //This is sketchy as f*ck, but it works so :shrug:
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (crouchStep > 1)
                {
                    crouchStep -= 1;
                }
                //model.transform.localScale = new Vector3(1, 1 - (desired_height / crouchStep), 1);
                controller.height = (1 - (desired_height / crouchStep)) * 2;
                speed = 1f;
            }
            else
            {
                model.transform.localScale = new Vector3(1, 1, 1);
                if (crouchStep < 50)
                {
                    crouchStep += 1;
                }
                //model.transform.localScale = new Vector3(1, desired_height + (1 / crouchStep), 1);
                controller.height = (desired_height + (1 / crouchStep)*(crouchStep / 1)) * 2 - 1;
                speed = 3f;
            }


            if (Input.GetKeyDown(KeyCode.F) && (overseer.heldObject.name == "Flashlight" || overseer.heldObject.name == "Flashlight(Clone)"))
            {

                overseer.heldObject.transform.GetChild(0).gameObject.SetActive(!overseer.heldObject.transform.GetChild(0).gameObject.activeSelf);
                overseer.GainSanity("fidget", 0.1f);
                overseer.heldObject.GetComponent<AudioSource>().Play();
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }

    }
}
