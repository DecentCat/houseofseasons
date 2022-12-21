using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Player stats
    public int health = 100;
    public float movementSpeed = 5f;
    public float shotBuffer = 0.5f;

    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 movementDirection = new Vector2(0, -1);
    private Vector2 facingDirection = Vector2.zero;

    private WeaponScript primaryWeapon;
    private WeaponScript secondaryWeapon;
    private WeaponScript tertiaryWeapon;

    private List<WeaponScript> weapons;
    private int weapon_index = 0;

    private LaserScript laser;

    private void Awake()
    {
        // init weapons
        primaryWeapon = gameObject.transform.Find("BasicWeapon").GetComponent<WeaponScript>(); //GetChild(0).GetComponent<WeaponScript>();
        secondaryWeapon = gameObject.transform.Find("HeavyWeapon").GetComponent<WeaponScript>();
        tertiaryWeapon = gameObject.transform.Find("AssaultWeapon").GetComponent<WeaponScript>();

        weapons = new List<WeaponScript> { 
            primaryWeapon,
            secondaryWeapon,
            tertiaryWeapon
        };

        // init laser
        laser = gameObject.transform.Find("Laser").GetComponent<LaserScript>();
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
                weapon_index = weapon_index < weapons.Count - 1 ? weapon_index + 1 : 0;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                weapon_index = weapon_index > 0 ? weapon_index - 1 : weapons.Count - 1;
            }
        }

        animator.SetFloat("Horizontal", movementDirection.x);
        animator.SetFloat("Vertical", movementDirection.y);
        animator.SetFloat("Speed", movementDirection.sqrMagnitude);

        // fire equipped weapon
        if (Input.GetButton("Fire1"))
        {
            var currentWeapon = weapons[weapon_index];
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePos - currentWeapon.transform.position;
            currentWeapon.Shoot(direction.normalized);
        }

        // fire laser
        if (Input.GetButton("Fire2"))
        {
            laser.Shoot();
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            laser.StopShooting();
        }

    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementDirection.normalized * movementSpeed * Time.fixedDeltaTime);
    }

}
