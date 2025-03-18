using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class ExitDoorController : DoorController
{
    [SerializeField] private LeverController redLever;
    [SerializeField] private LeverController greenLever;
    [SerializeField] private LeverController blueLever;
    [SerializeField] private GameObject RedPanel;
    [SerializeField] private GameObject greenPanel;
    [SerializeField] private GameObject bluePanel;

    [SerializeField] private bool isOn = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        isOpen = false;
        UpdateAnimator();
    }

    private void Update()
    {
        checkLevers();
        if (isOn && playerNearby && Input.GetKeyDown(KeyCode.F))
        {
            GameManager.master.levelComplete();
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

    private void checkLevers()
    {
        if (redLever.isActive && greenLever.isActive && blueLever.isActive)
        {
            isOn = true;
        }
        if (redLever.isActive)
        {
            RedPanel.GetComponent<UnityEngine.UI.Image>().color = Color.red;
        }
        else 
        {
            RedPanel.GetComponent<UnityEngine.UI.Image>().color = Color.gray;
        }

        if (greenLever.isActive)
        {
            greenPanel.GetComponent<UnityEngine.UI.Image>().color = Color.green;
        }
        else
        {
            greenPanel.GetComponent<UnityEngine.UI.Image>().color = Color.gray;
        }

        if (blueLever.isActive)
        {
            bluePanel.GetComponent<UnityEngine.UI.Image>().color = Color.blue;
        }
        else
        {
            bluePanel.GetComponent<UnityEngine.UI.Image>().color = Color.gray;
        }
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        animator.SetBool("BlueOn", blueLever.isActive);
        animator.SetBool("RedOn", redLever.isActive);
        animator.SetBool("GreenOn", greenLever.isActive);
    }

    private void ToggleDoor(bool open)
    {
        isOpen = open;
        animator.SetBool("IsOpen", isOpen);
    }
}
