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

        if (!isOn && isOpen) //cierra la puerta en caso de que este abierta
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
                _master.showAdvise("Use la tecla F para usar la puerta");
                ToggleDoor(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            playerNearby = false;
            _master.showAdvise(string.Empty);
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
