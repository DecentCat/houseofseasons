using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotScript : MonoBehaviour
{
    public float movementSpeed = 10f;
    public int dealtDamage = 3;
    public Vector2 movementDirection = new Vector2(0, -1);

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 7);
    }

    private void FixedUpdate()
    {
        Vector2 position = transform.position;
        position += movementDirection * movementSpeed * Time.fixedDeltaTime;
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerScript playerScript;
        if (!collision.gameObject.TryGetComponent<PlayerScript>(out playerScript))
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector2 direction)
    {
        movementDirection = direction;
    }
}
