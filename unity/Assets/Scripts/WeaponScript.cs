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

    //--------------------------------
    // 3 - Shooting from another script
    //--------------------------------

    /// <summary>
    /// Create a new projectile if possible
    /// </summary>
    public void Shoot(Vector2 dir)
    {
        if (CanAttack)
        {
            cooldown = 1/shootingRate;

            // Create a new shot
            var shot = Instantiate(shotPrefab) as GameObject;

            // Assign position
            shot.transform.position = transform.position;

            // set properties of shot
            ProjectileScript shotScript = shot.GetComponent<ProjectileScript>();
            if (shot != null)
            {
                shotScript.movementDirection = Rotate(dir, Random.Range(bulletSpread/(-2f), bulletSpread/2f));
            }
        }
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
}
