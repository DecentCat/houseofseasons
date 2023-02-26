using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    /// <summary>
    /// Projectile Prefab
    /// </summary>
    public GameObject shotPrefab;

    /// <summary>
    /// number of shots per second
    /// </summary>
    public float shootingRate = 2.00f;

    /// <summary>
    /// defines how much each shot can spread from the ideal line (in degrees)
    /// </summary>
    public float bulletSpread = 0;

    /// <summary>
    /// Specifies how many projectiles are fired per shot.
    /// each shot has its own spray. Setting this value >1 will emulate a shotgun.
    /// </summary>
    public uint projectilesPerShot = 1;

    /// <summary>
    /// Amount of bullets in the weapon.
    /// </summary>
    public int bullets;

    /// <summary>
    /// Infinite bullets.
    /// </summary>
    public bool infiniteBullets;

    // privates
    // cooldown in seconds
    private float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Create a new projectile if possible
    /// </summary>
    public void Shoot(Vector2 dir)
    {
        if (CanAttack && (bullets > 0 || infiniteBullets))
        {
            cooldown = 1 / shootingRate;

            for (int i = 0; i < projectilesPerShot; i++)
            {
                SpawnProjectile(dir);
            }

            bullets--;
        }
    }

    public void AddBullets(int amount)
    {
        bullets += amount;
    }

    /// <summary>
    /// Is the weapon ready to create a new projectile?
    /// </summary>
    public bool CanAttack
    {
        get
        {
            return cooldown <= 0f;
        }
    }

    private Vector2 Rotate(Vector2 vector, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = vector.x;
        float ty = vector.y;
        vector.x = (cos * tx) - (sin * ty);
        vector.y = (sin * tx) + (cos * ty);
        return vector;
    }

    private void SpawnProjectile(Vector2 direction)
    {
        // Create a new shot
        var shot = Instantiate(shotPrefab) as GameObject;

        // Assign position
        shot.transform.position = transform.position;

        // set properties of shot
        ShotScript shotScript = shot.GetComponent<ShotScript>();
        if (shot != null)
        {
            shotScript.movementDirection = Rotate(direction, Random.Range(bulletSpread / (-2f), bulletSpread / 2f));
        }
    }
}
