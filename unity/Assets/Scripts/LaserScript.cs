using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public float maxRange;
    public LayerMask layersToHit;
    public int damage;

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

            if (hit.collider.tag == "Enemy" && doDamage)
            {
                hit.collider.gameObject.GetComponent<EnemyBehavior>().TakeDamage(damage, new Vector2(0,0));
                doDamage = false;
            }
        }
    }

    public void Shoot()
    {
        lineRenderer.enabled = true;
    }

    public void StopShooting()
    {
        lineRenderer.enabled = false;
    }

    private void EnableDamage()
    {
        doDamage = true;
    }
}
