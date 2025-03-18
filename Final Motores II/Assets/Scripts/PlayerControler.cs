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
    private float waitTime = 0.497f;

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
        GameManager.master.iniciarHP(hp);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && !isShooting && !isDeath && !isTakingDamage)
        {
            isShooting = true;
            rb.velocity = Vector2.zero;
            animator.SetBool("IsShooting", true);
            fire();
            StartCoroutine("waitShot");
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
        GameManager.master.UpdateHpUI(currentHP);

        if (currentHP <= 0)
        {
            Debug.Log("Esta muerta");
            death();
            return;
        }
        if (!isDeath && !isTakingDamage)
        {
            rb.velocity = Vector2.zero;
            isTakingDamage = true;
            animator.SetTrigger("Damage");
            StartCoroutine("waitDamage");
        }
    }

    private void death()
    {
        isDeath = true;
        isTakingDamage = false;
        isShooting = false;
        rb.velocity = Vector2.zero;

        animator.SetBool("Damage", false);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsShooting", false);

        animator.SetTrigger("Death");


        GameManager.master.gameOver(deathTime);
    }

    public void recuperarSalud( int salud)
    {
        currentHP += salud;
       // Debug.Log($"salud: {currentHP}");
        if (currentHP > hp) 
        {
            currentHP = hp;
        }
        GameManager.master.UpdateHpUI(currentHP);
    }
    // ########################### ATAQUE DEL PERSONAJE #####################################
    private void fire()
    {
        //Debug.DrawRay(gunController.position, gunController.right * range, Color.red, 10f);
        RaycastHit2D ray = Physics2D.Raycast(gunController.position, gunController.right, range);
        if (ray)
        {
            Debug.Log($"Golpeo algo: {ray.transform.tag}");
            if (ray.transform.CompareTag("Zombie"))
            {
                Debug.Log("Golpeo al zombi");
                ray.transform.GetComponent<ZombiController>().TakeDamage(10);
            }
        }
    }

    // ########################## Corutinas #################################################
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
    }

    
}
