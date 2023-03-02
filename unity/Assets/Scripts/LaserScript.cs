using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public float maxRange;
    public LayerMask layersToHit;
    public int damage;
    public float charge;
    public bool infiniteCharge;
    public float maxCharge = 100;

    private LineRenderer lineRenderer;
    private float damageDelay = 1f / 2f;
    private bool doDamage = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;

        InvokeRepeating("EnableDamage", 0, damageDelay);
    }

    void Update()
    {
        Vector2 mousePos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxRange, layersToHit);
        Debug.DrawLine(transform.position, hit.point);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, hit.point);

        if (hit.collider != null && lineRenderer.enabled)
        {
            //Debug.Log(hit.collider.gameObject.name);
            if (doDamage)
            {
                if (!infiniteCharge)
                {
                    charge -= 1f;
                }

                RandomItemPickup itemPickup;
                if (hit.collider.tag == "Enemy")
                {
                    hit.collider.gameObject.GetComponent<EnemyBehavior>().TakeDamage(damage, new Vector2(0, 0));
                }
                else if (hit.collider.gameObject.tag != "Enemy" && hit.collider.gameObject.TryGetComponent<RandomItemPickup>(out itemPickup))
                {
                    itemPickup.PlacePickup(true);
                }

                doDamage = false;
            }
            
            lineRenderer.enabled = false;
        }
    }

    public void Shoot()
    {
        if (charge > 0 || infiniteCharge)
        {
            lineRenderer.enabled = true;
        }
    }

    public void Charge(float charge)
    {
        this.charge += charge;
        if (this.charge > maxCharge)
        {
            this.charge = maxCharge; 
        };
    }

    public float GetCharge
    {
        get
        {
            if (infiniteCharge)
            {
                return -1f;
            }

            return charge;
        }
    }

    private void EnableDamage()
    {
        doDamage = true;
    }
}
