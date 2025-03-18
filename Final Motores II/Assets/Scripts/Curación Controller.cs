using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuraciónController : MonoBehaviour
{
    private bool playerNearby;
    private GameObject player;

    private void Start()
    {
        playerNearby = false;
        player = null;
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.F))
        {
            player.GetComponent<PlayerControler>().recuperarSalud(25);
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            player = collision.gameObject;
            GameManager.master.showAdvise("Aprete la tecla F para curar la salud");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            player = null;
            GameManager.master.showAdvise(string.Empty);
        }
    }
}
