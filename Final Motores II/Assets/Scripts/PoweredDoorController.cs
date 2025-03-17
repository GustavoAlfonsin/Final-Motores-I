using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredDoorController : DoorController
{
    [SerializeField] private bool isOn = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsOn", false);
        animator.SetBool("IsOpen", false);
    }

    public void setPowerState(bool powerState)
    {
        isOn = powerState;
        animator.SetBool("IsOn", isOn);

        if (!isOn && isOpen)
        {
            ToggleDoor(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            playerNearby = true;

            if (isOn && !isOpen)
            {
                ToggleDoor(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            playerNearby = false;

            if (isOn && isOpen)
            {
                ToggleDoor(false);
            }
        }
    }

    private void Update()
    {
        if (playerNearby && isOn && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(teleportPlayer());
        }
    }

    private void ToggleDoor(bool open)
    {
        isOpen = open;
        animator.SetBool("IsOpen", isOpen);
    }
}
