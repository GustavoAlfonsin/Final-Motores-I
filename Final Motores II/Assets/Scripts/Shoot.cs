using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Transform gunController;
    [SerializeField] private float range;

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("MUERE, MUERE, MUERE");
            fire();
        }
    }

    private void fire()
    {
        RaycastHit2D ray = Physics2D.Raycast(gunController.position, gunController.right, range);
        if (ray)
        {
            if (ray.transform.CompareTag("Zombie"))
            {
                //Hacer que la barra de vida del zombie baje
            }
        }
    }
}
