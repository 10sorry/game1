using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    public Animator animator;
    public GameObject soundObject;
    private AudioSource audioSource;
    private AudioSource soundSource;
    private bool isDead = false;
    public int maxHealth = 100;
    int currentHealth;
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;
    public float speed;
    private Transform currentTarget;
    public float detectionRadius = 5.0f; // радиус обнаружения персонажа врагом
    public float attackRadius = 2.0f; // радиус атаки врага
    public LayerMask playerLayer; // слой игрока
    private Transform playerTransform; // трансформ игрока
    private bool playerDetected; // обнаружен ли игрок
    private bool attacking; // атакует ли враг
    private bool playerDissaper; // игрок ушел из поля зрения, либо удалился на расстьояние больше 15f от точки патрулирования

    void Start() {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        soundSource = soundObject.GetComponent<AudioSource>();
        currentTarget = pointA;
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // получаем трансформ игрока по тегу
        playerDetected = false;
        attacking = false;
        
    }

    void Update()
    {
        if (isDead == false)
        {
            Patrol();
            Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
            float distanceToTargetC = Vector2.Distance(transform.position, pointC.position);

            if (distanceToTargetC >= 15f)
            {
                Debug.Log("return to patrol");
                currentTarget = pointC;
                playerDetected = false;
                attacking = false;
                playerDissaper = true;
                return;
            }

            else if (playerCollider != null && distanceToTargetC <= 15f)
            {
                // обнаружен игрок
                playerDetected = true;
                Debug.Log("Player detected!");

                if (!attacking && Vector2.Distance(transform.position, playerTransform.position) <= attackRadius)
                {
                    // игрок в зоне атаки
                    attacking = true;
                }

                else if (Vector2.Distance(transform.position, pointC.position) >= 15f)
                {
                    playerDissaper = true;
                }

                else
                {
                    Chasing();
                    if (playerDissaper)
                    {
                        return;
                    }

                }

            }
        }

            // рисуем луч от врага к игроку для визуальной проверки
            Debug.DrawLine(transform.position, playerTransform.position, Color.red);

            // если враг атакует, то вызываем функцию атаки
            if (attacking)
            {
                Attack();
            }
        }

        
    void Chasing()
    {

        animator.SetBool("attacking", false);
        attacking = false;
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        if (transform.position.x < playerTransform.position.x)
        {
            gameObject.transform.localScale = new Vector3(3, 3, 3);
        }
        else if (transform.position.x > playerTransform.position.x)
        {
            gameObject.transform.localScale = new Vector3(-3, 3, 3);
        }
        Debug.Log("chasing!");

    }

    void Patrol()
    {
        playerDissaper = false;
        if (!playerDetected)
        {
            // Перемещаем врага к текущей цели
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

            // Если враг достиг текущей цели, меняем цель
            if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
            {
                currentTarget = (currentTarget == pointA) ? pointB : pointA;
            }

            animator.SetBool("running", Vector2.Distance(transform.position, currentTarget.position) > 0.1f);

            if (transform.position.x < currentTarget.position.x)
            {
                gameObject.transform.localScale = new Vector3(3, 3, 3);
            }
            else if (transform.position.x > currentTarget.position.x)
            {
                gameObject.transform.localScale = new Vector3(-3, 3, 3);
            }
        }

        if (isDead == true)
        {
            return;
        }
    }

    void Attack()
    {
        // атакуем игрока
        Debug.Log("Player attacking!");
        animator.SetBool("attacking", true);
        if ( playerDetected == false || playerDissaper == true)
        return;
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("hurting");
        soundSource.Play();
        currentHealth -= damage;
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }

    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Death");
        audioSource.Play();
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        Debug.Log("Enemy died!");

        attacking = false;
        playerDetected = false;
        playerDissaper = true;

    }

}