using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Player stats
    public int health = 100;
    public float movementSpeed = 5f;
    public float shotBuffer = 0.5f;

    public Rigidbody2D rb;
    public Animator animator;
    public GameObject projectilePrefab;
    public Transform projectileSpawn;

    private Vector2 movementDirection = new Vector2(0, -1);
    private Vector2 facingDirection = Vector2.zero;
    private float nextShot = 0f;

    void Update()
    {
        movementDirection.x = Input.GetAxisRaw("Horizontal");
        movementDirection.y = Input.GetAxisRaw("Vertical");

        if (movementDirection != Vector2.zero)
        {
            facingDirection = movementDirection.normalized;
        }

        animator.SetFloat("Horizontal", movementDirection.x);
        animator.SetFloat("Vertical", movementDirection.y);
        animator.SetFloat("Speed", movementDirection.sqrMagnitude);

        if(Input.GetButton("Fire1") && (Time.time >= nextShot))
        {
            nextShot = Time.time + shotBuffer;
            Shoot();
        }

    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementDirection.normalized * movementSpeed * Time.fixedDeltaTime);
    }


    void Shoot()
    {
        GameObject singleBullet = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation) as GameObject;
        if (facingDirection != Vector2.zero)
        {
            singleBullet.SendMessage("SetDirection", facingDirection);
        }
    }
}
