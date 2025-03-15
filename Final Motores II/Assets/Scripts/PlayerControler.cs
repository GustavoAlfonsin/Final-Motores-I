using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private float horizontalMovement = 0f;
    [SerializeField] private float movementSpeed;
    [Range(0, 0.3f)][SerializeField] private float motionSmoothing;

    private Vector3 speed = Vector3.zero;
    private bool lookingToTheRight = true;

    //Disparo
    [SerializeField] private Transform gunController;
    [SerializeField] private float range;
    private bool isShooting;
    private float waitTime = 0.517f;

    // Vida jugador
    private int hp = 100;
    private int currentHP;
    private bool isTakingDamage;
    [SerializeField] private bool isDeath;
    private float damagaTime = 0.350f;
    private float deathTime = 1.017f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = hp;
        isShooting = false;
        isTakingDamage = false;
        isDeath = false;
        animator = GetComponent<Animator>();
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsShooting", false);
        animator.SetBool("Damage", false);
        animator.SetBool("IsDeath", false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && isShooting == false && isDeath == false)
        {
            isShooting = true;
            rb.velocity = Vector2.zero;
            Debug.Log("MUERE, MUERE, MUERE");
            animator.SetBool("IsShooting", true);
            StartCoroutine("waitShot");
            fire();
        }
        //BORRAR LO SIGUIENTE
        if (Input.GetKeyDown(KeyCode.E))
        {
            getDamage(20);
            Debug.Log($"{currentHP}");
        }
        horizontalMovement = Input.GetAxisRaw("Horizontal") * movementSpeed;
    }

    private void FixedUpdate()
    {
        if (isShooting == false && isTakingDamage == false && isDeath == false)
        {
            Move(horizontalMovement * Time.fixedDeltaTime);
        }
    }

    //################## MOVIMIENTO DEL PERSONAJE #######################
    private void Move(float movement)
    {
        Vector3 targetSpeed = new Vector2(movement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetSpeed, ref speed, motionSmoothing);
        if (movement != 0)
        {
            animator.SetBool("IsWalking", true);
            if (movement > 0 && !lookingToTheRight)
            {
                spin();
            }else if (movement < 0 && lookingToTheRight)
            {
                spin();
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
            animator.SetBool("IsWalking", false);
        }
    }

    private void spin()
    {
        lookingToTheRight = !lookingToTheRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    //#################### CONTROL DE LA VIDA DEL PERSONAJE ###############################
    public void getDamage(int injury)
    {
        currentHP -= injury;

        if (currentHP <= 0)
        {
            Debug.Log("Esta muerta");
            death();
            return;
        }

        rb.velocity = Vector2.zero;
        isTakingDamage = true;
        animator.SetBool("Damage", true);
        StartCoroutine("waitDamage");
    }

    private void death()
    {
        isDeath = true;
        isTakingDamage = false;
        animator.SetBool("Damage", false);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsShooting", false);
        animator.SetBool("IsDeath", true);
        StartCoroutine("showDeath");
    }

    // ########################### ATAQUE DEL PERSONAJE #####################################
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

    IEnumerator waitShot()
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("IsShooting", false);
        isShooting = false;
    }

    IEnumerator waitDamage()
    {
        yield return new WaitForSeconds(damagaTime);
        isTakingDamage = false;
        animator.SetBool("Damage", false);
    }

    IEnumerator showDeath()
    {
        yield return new WaitForSeconds(deathTime);
    }
}
