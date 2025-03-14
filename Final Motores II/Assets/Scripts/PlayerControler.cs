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

    private int hp = 100;
    private int currentHP;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = hp;
        animator = GetComponent<Animator>();
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsShooting", false);
        animator.SetBool("Damage", false);
        animator.SetBool("IsDeath", false);
    }

    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal") * movementSpeed;
    }

    private void FixedUpdate()
    {
        Move(horizontalMovement * Time.fixedDeltaTime);
    }

    private void Move(float movement)
    {
        Vector3 targetSpeed = new Vector2(movement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetSpeed, ref speed, motionSmoothing);
        if (movement != 0)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsShooting", false);
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
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsShooting", false);
        }
    }

    private void spin()
    {
        lookingToTheRight = !lookingToTheRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    public void getDamage(int injury)
    {
        currentHP -= injury;
        if (currentHP <= 0)
        {
            death();
        }
    }

    private void death()
    {
        // hace la animación de muerte y muestra el menu de reinicio de nivel
    }
}
