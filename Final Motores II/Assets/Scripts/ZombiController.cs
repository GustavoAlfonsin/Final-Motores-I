using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombiController : MonoBehaviour
{
    public float patrolSpeed = 4f;
    public float waitTime = 1f;
    public float attackRange;
    public float attackCoolDown;
    public int maxHealth = 50;
    [SerializeField] private int currentHealth;

    private Rigidbody2D rb;
    private Animator _animator;
    private bool isFacingRight = true;
    private bool isChasing = false;
    private bool isAttacking = false;
    private bool isWaiting = false;
    private bool hasDetectedPlayer = false;
    private bool isDead = false;
    [SerializeField] private Transform player;

    public GameObject HealthBarUI;
    public Slider healthSlider;

    public Transform detectionZone;
    public float detectionRadius = 2f;
    public LayerMask playerLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        HealthBarUI.SetActive(false); 
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    private void Update()
    {
        detectedPlayer();

        if (isChasing && !isDead && player != null)
        {
            ChasePlayer();
        } else if (!isWaiting && !isDead)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        _animator.SetBool("IsWalking", true);
        rb.velocity = new Vector2((isFacingRight ? 1 : -1) * patrolSpeed, rb.velocity.y);

        Vector2 origen = new Vector2(transform.position.x + (isFacingRight ? 0.5f : -0.5f),transform.position.y);
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        Debug.DrawRay(origen, direction*0.5f, Color.red, 10f);
        RaycastHit2D hit = Physics2D.Raycast(origen, direction, 0.5f);
        if (hit.collider != null && (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Zombie")))
        {
            StartCoroutine(IdleBeforeTurning());
        }
    }

    private IEnumerator IdleBeforeTurning()
    {
        isWaiting = true;
        rb.velocity = Vector2.zero;
        _animator.SetBool("IsWalking", false);
        yield return new WaitForSeconds(waitTime);
        TurnAround();
        isWaiting = false;
    }

    private void TurnAround()
    {
        isFacingRight = !isFacingRight;
        transform.eulerAngles = new Vector3(0, isFacingRight ? 0 : 180, 0);
    }

    private void detectedPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(detectionZone.position, detectionRadius, playerLayer);
        if (playerCollider != null && playerCollider.CompareTag("Player"))
        {
            player = playerCollider.transform;
            hasDetectedPlayer = true;
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }
    }

    private void ChasePlayer()
    {
        _animator.SetBool("IsWalking", true);

        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.velocity = new Vector2(direction * patrolSpeed, rb.velocity.y);

        if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
        {
            TurnAround();
        }

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero;
        _animator.SetBool("IsWalking", false);
        _animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(1f);
        player.GetComponent<PlayerControler>().getDamage(10);
        yield return new WaitForSeconds(attackCoolDown);
        _animator.SetBool("IsAttacking", false);
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        HealthBarUI.SetActive(true);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        
        isDead = true;

        rb.velocity = Vector2.zero;
        HealthBarUI.SetActive(false);

        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        _animator.SetTrigger("Dead");
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionZone.position, detectionRadius);
    }
}
