using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public int attackDamage = 20; 
    public float attackRange = 0.5f;


    public LayerMask enemyLayers;
    public Transform Weapon;
    private float Running;
    public Animator animator;
    private Rigidbody2D rb;

    private AudioSource audioSource;
    private AudioSource soundSource;
    private AudioSource stepsSource;
    private AudioSource musicSource;
    public GameObject soundObject;
    public GameObject stepsObject;
    public GameObject musicObject;

    private bool senoDetected = false;

    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        soundSource = soundObject.GetComponent<AudioSource>();
        stepsSource = stepsObject.GetComponent<AudioSource>();
        musicSource = musicObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        Running = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Jump"))
        {
            Debug.Log("isJumping");
            rb.velocity =  new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("Jumping");
            audioSource.Play();
            animator.SetBool("jumpTofalling", true);

        }

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Sliding");
        }

        if(Input.GetButtonDown("Fire2"))
        {
            Attack();
        }

        if(Input.GetButton("Fire3"))
        {
            if(senoDetected == true)
            {
                animator.SetBool("crouching", true);
            }

            else
            {
                animator.SetBool("crouching", false);
            }
 
        }


    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(Running * moveSpeed, rb.velocity.y);
        animator.SetFloat("Running", Mathf.Abs(Running));
        stepsSource.Play();
        if (Running > 0f)
        {
            gameObject.transform.localScale = new Vector3(4, 4, 4);
        }

        if (Running < 0f)
        {
            gameObject.transform.localScale = new Vector3(-4, 4, 4);
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attacking");
        soundSource.Play();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(Weapon.position, attackRange, enemyLayers);
        
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Crab>().TakeDamage(attackDamage);
        }
    }

    private void  OnDrawGizmosSelected()
    {
        if(Weapon == null)
            return;
        Gizmos.DrawWireSphere(Weapon.position, attackRange);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "seno")
        {
            senoDetected = true;
            Debug.Log("senoDetected!");
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            animator.SetBool("jumpTofalling", false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "seno")
        {
            senoDetected = false;
            Debug.Log("senoWasNotDetected");
            animator.SetBool("crouching", false);
        }
    }

    


}
