using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    private Animator _animator;
    public bool isActive = false;
    [SerializeField] private bool playerNearby = false;
    [SerializeField] private GameManager _master;

    [SerializeField] private string playerTag = "Player";
    [SerializeField] private List<GameObject> poweredDoors;
    [SerializeField] private List<GameObject> paredes;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("IsOn", false);
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.F))
        {
            ToggleLever();
        }
    }

    private void ToggleLever()
    {
        isActive = !isActive;
        _animator.SetBool("IsOn", isActive);

        foreach (GameObject door in poweredDoors)
        {
            if (door != null)
            {
                PoweredDoorController doorController = door.GetComponent<PoweredDoorController>();
                if (doorController != null)
                {
                    doorController.setPowerState(isActive);
                }
            }
        }

        foreach(GameObject wall in paredes){
            if (wall != null)
            {
                ParedEspecialController wallController = wall.GetComponent<ParedEspecialController>();
                if (wallController != null)
                {
                    wallController.chaceStateWall(!isActive);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            _master.showAdvise("Use la tecla F para usar la palanca");
            playerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            _master.showAdvise(string.Empty);
            playerNearby = false;
        }
    }
}
