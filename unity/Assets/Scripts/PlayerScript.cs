using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    // Player stats
    public int health = 100;
    public int maxHealth = 100;
    public Healthbar healthbar;
    public float movementSpeed = 5f;
    public float shotBuffer = 0.5f;

    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 movementDirection = new Vector2(0, -1);
    private Vector2 facingDirection = Vector2.zero;

    public WeaponManagerScript weaponManager;
    private Level _level;
    public SpriteRenderer rend;

    private void Awake()
    {
        weaponManager = GetComponent<WeaponManagerScript>();
        _level = GetComponentInParent<Level>();
        health = CrossSceneInformation.PlayerHealth;
        maxHealth = CrossSceneInformation.PlayerMaxHealth;
        healthbar.SetUiMaxHealth(maxHealth);
        healthbar.SetUiHealth(maxHealth);
    }

    void Update()
    {
        movementDirection.x = Input.GetAxisRaw("Horizontal");
        movementDirection.y = Input.GetAxisRaw("Vertical");

        if (movementDirection != Vector2.zero)
        {
            facingDirection = movementDirection.normalized;
        }

        // scroll through weapon roster
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                weaponManager.SwitchWeaponPrevious();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                weaponManager.SwitchWeaponNext();
            }
        }

        animator.SetFloat("Horizontal", movementDirection.x);
        animator.SetFloat("Vertical", movementDirection.y);
        animator.SetFloat("Speed", movementDirection.sqrMagnitude);

        // fire equipped weapon
        if (Input.GetButton("Fire1"))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePos - transform.position;
            weaponManager.ShootEquippedWeapon(direction.normalized);
        }

        // secondary 
        if (Input.GetButton("Fire2"))
        {
            // bombs? items?
        }
       
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementDirection.normalized * movementSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        health -= damage;
        healthbar.SetUiHealth(health);

        rb.MovePosition(rb.position + knockbackDirection * (damage * 2) * Time.fixedDeltaTime);

        if (health <= 0)
        {
            Destroy(gameObject);
            _level.LevelQuit(this, true);
        }

        Flicker();
    }

    public void HealDamage(int healing)
    {
        health += healing;
        health = health > maxHealth ? maxHealth : health;
        healthbar.SetUiHealth(health);
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
