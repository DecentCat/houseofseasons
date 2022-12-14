using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public float maxRange;
    public LayerMask layersToHit;
    public float damage;

    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //this.gameObject.SetActive(false);
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        //transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxRange, layersToHit);
        Debug.DrawLine(transform.position, hit.point);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, hit.point);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);

            if (hit.collider.tag == "Enemy")
            {
                // damage enemy
            }
        }
    }

    public void Shoot()
    {
        //this.gameObject.SetActive(true);
        lineRenderer.enabled = true;
    }

    public void StopShooting()
    {
        //this.gameObject.SetActive(false);
        lineRenderer.enabled = false;
    }
}
