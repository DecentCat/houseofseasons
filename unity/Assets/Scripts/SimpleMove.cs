using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class SimpleMove : MonoBehaviour
{
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnValidate() {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float speed = 0.0001f;
        Vector2 forceVec = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);
        _rb.AddForce(forceVec, ForceMode2D.Impulse);
    }
}
