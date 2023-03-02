using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    //public Transform target;
    public Rigidbody2D rb;
    public Animator animator;
    public float movementSpeed = 3f;
    public float awareDistance = 4f;
    public float spawnPointDistance = 2f;

    public int health = 100;
    public bool melee = true;
    public float attackDistance = 1f;
    public int attackPower = 5;
    public SpriteRenderer rend;

    GameObject[] players;
    Vector2 movementDirection;
    Vector2 spawnPoint;
    bool following;
    bool attacking;
    float lastConnectedHit = 0;
    WeaponScript primaryWeapon;

    private void Awake()
    {
        if (!melee)
        {
            primaryWeapon = gameObject.transform.Find("BasicEnemyWeapon").GetComponent<WeaponScript>();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        spawnPoint = transform.position;
        InvokeRepeating("PassiveMovement", 0, 0.30f);
    }

    // Update is called once per frame
    void Update()
    {
        HandleWalkAnimations();
        FlipSprite();
    }

    void FixedUpdate()
    {
        foreach (GameObject player in players)
        {
            Attack(player);
            if (!attacking)
            {
                Movement(player);
            }
            
        }
    }


    private void Movement(GameObject player)
    {
        //Debug.Log(player.transform.position + " " + transform.position);
        if (player != null && Vector2.Distance(player.transform.position, transform.position) <= awareDistance)
        {
            movementDirection = (player.transform.position - transform.position).normalized;
            rb.MovePosition(rb.position + movementDirection.normalized * movementSpeed * Time.fixedDeltaTime);
            following = true;
        }
        else
        {
            rb.velocity = movementDirection;
            following = false;
        }
    }


    private void PassiveMovement()
    {
        if (following)
        {
            return;
        }

        if (Vector2.Distance(spawnPoint, transform.position) > spawnPointDistance)
        {
            movementDirection = (spawnPoint - (Vector2)transform.position).normalized;
        }
        else if (Random.value < 0.85f)
        {
            movementDirection = Vector2.zero;
        }
        else
        {
            float random = Random.Range(0f, 300f);
            Vector2 randomMovementDirection = new Vector2(Mathf.Cos(random), Mathf.Sin(random)).normalized;

            if (Vector2.Angle(movementDirection, randomMovementDirection) <= 22.5)
            {
                movementDirection = (movementDirection + randomMovementDirection).normalized;
            }
        }
    }


    private void HandleWalkAnimations()
    {
        animator.SetFloat("Horizontal", movementDirection.x);
        animator.SetFloat("Speed", movementDirection.sqrMagnitude);
    }


    private void FlipSprite()
    {
        if (movementDirection.x >= 0.1f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }


    public void Attack(GameObject player)
    {
        if (lastConnectedHit > Time.time + 20)
        {
            return;
        }

        if (player != null && Vector2.Distance(player.transform.position, transform.position) <= attackDistance)
        {
            if (melee)
            {
                player.GetComponent<PlayerScript>().TakeDamage(attackPower, movementDirection);
                lastConnectedHit = Time.time;
            }
            else
            {
                Vector2 direction = player.transform.position - transform.position;
                primaryWeapon.Shoot(direction.normalized);
            }

            attacking = true;
            return;
        }
        else
        {
            attacking = false;
        }
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        health -= damage;

        rb.MovePosition(rb.position + knockbackDirection * (damage * 2) * Time.fixedDeltaTime);

        if (health <= 0)
        {
            RandomItemPickup itemPickup;
            if(gameObject.TryGetComponent<RandomItemPickup>(out itemPickup))
            {
                itemPickup.PlacePickup(false);
            }
            Destroy(gameObject);
        }

        Flicker();
    }


    private void Flicker()
    {
        Invoke("RenderRed", 0f);
        Invoke("RenderNormal", 0.1f);
        Invoke("RenderRed", 0.15f);
        Invoke("RenderNormal", 0.2f);
    }

    private void RenderRed()
    {
        rend.color = new Color(1, 0, 0, 1);
    }

    private void RenderNormal()
    {
        rend.color = new Color(1, 1, 1, 1);
    }
}
