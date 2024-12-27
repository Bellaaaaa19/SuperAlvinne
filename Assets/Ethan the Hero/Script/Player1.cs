using System.Collections;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    public float moveSpeed = 2f; // Movement speed of the player
    public float attackRange = 1.5f; // Range at which the player will attack the enemy
    public int damage = 1; // Damage dealt to the enemy

    private Transform enemy;
    private Vector3 startPosition;
    private bool isAttacking = false; // Prevents overlapping actions
    private Animator animator; // Reference to the Animator component

    public int maxHealth = 3; // Maximum health ng kalaban
    private int currentHealth;

    


    void Start()
    {
        // Find the enemy in the scene (ensure Enemy1 tag is set on the enemy)
        enemy = GameObject.FindGameObjectWithTag("Enemy1").transform;
        startPosition = transform.position;

        // Get the Animator component
        animator = GetComponent<Animator>();


    }

    void Update()
    {
        if (isAttacking) return; // Prevent movement while attacking

        float distanceToEnemy = Vector3.Distance(transform.position, enemy.position);

        if (distanceToEnemy > attackRange)
        {
            MoveTowardsEnemy(); // Move closer to the enemy
        }
        else
        {
            StartAttack(); // Attack when in range
        }
    }



    private void Die()
    {
        Debug.Log("Enemy1 has died!");
        Destroy(gameObject); // Destroy the enemy object when health reaches 0
    }

    void MoveTowardsEnemy()
    {
        // Lock the y-axis to prevent upward movement
        Vector3 targetPosition = new Vector3(enemy.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Trigger movement animation
        animator.SetBool("isMoving", true);
    }

    void StartAttack()
    {
        // Stop movement and trigger attack logic
        animator.SetBool("isMoving", false); // Stop moving animation
        animator.SetBool("isAttacking", true); // Start attack animation
        StartCoroutine(AttackAndRetreat());
    }

    IEnumerator AttackAndRetreat()
    {
        isAttacking = true;

        // Wait for the attack animation to complete (adjust the duration to match your attack animation)
        yield return new WaitForSeconds(0.5f);

        // Damage the enemy
        Enemy1 enemyScript = enemy.GetComponent<Enemy1>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage); // Reduce enemy health
        }

        // Stop attack animation
        animator.SetBool("isAttacking", false);

        // Instantly teleport back to the starting position
        transform.position = startPosition;

        // Ensure the character is in the idle state after teleporting
        animator.SetBool("isMoving", false);
        isAttacking = false;
    }
}