using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    private Rigidbody2D rb;
    private float horizontalMovement = 0f;
    [SerializeField] private float movementSpeed;
    [Range(0, 0.3f)][SerializeField] private float motionSmoothing;

    private Vector3 speed = Vector3.zero;
    private bool lookingToTheRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (movement > 0 && !lookingToTheRight)
        {
            spin();
        }else if (movement < 0 && lookingToTheRight)
        {
            spin();
        }
    }

    private void spin()
    {
        lookingToTheRight = !lookingToTheRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }
}
