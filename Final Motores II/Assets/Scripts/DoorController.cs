using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    protected Animator animator;
    protected bool isOpen = false;
    [SerializeField]protected bool playerNearby = false;

    [SerializeField] private GameObject connectedDoor;
    [SerializeField] protected string playerTag = "Player";
    [SerializeField] private float teleportDelay = 0.50f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        UpdateDoorState();
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(teleportPlayer());
        }
    }

    private void UpdateDoorState()
    {
        animator.SetBool("IsOpen", isOpen);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpen)
        {
            playerNearby = true;
            OpenDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isOpen)
        {
            playerNearby = false;
            CloseDoor();
        }
    }

    protected virtual void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            UpdateDoorState();
        }
    }

    protected virtual void CloseDoor()
    {
        if (isOpen)
        {
            isOpen = false;
            UpdateDoorState();
        }
    }

    public void PlayerEntered()
    {
        playerNearby = true;
        OpenDoor();
    }

    protected IEnumerator teleportPlayer()
    {
        if (connectedDoor != null)
        {
            DoorController connectedDoorController = connectedDoor.GetComponent<DoorController>();
            if (connectedDoorController != null)
            {
                connectedDoorController.OpenDoor();
            }
            yield return new WaitForSeconds(teleportDelay);

            Transform player = GameObject.FindGameObjectWithTag(playerTag).transform;
            Vector3 destiny = connectedDoor.transform.position;
            destiny.y = destiny.y - 0.55f;
            player.position = destiny;

            if (connectedDoorController != null)
            {
                connectedDoorController.PlayerEntered();
            }
        }
    }
}
