using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusController : MonoBehaviour
{
    private bool playerNearby;

    private void Start()
    {
        playerNearby = false;
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.F))
        {
            GameManager.master.takeVirus();
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            GameManager.master.showAdvise("Aprete la tecla F para conseguir el objeto");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            GameManager.master.showAdvise(string.Empty);
        }   
    }
}
