using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Movable : MonoBehaviour
{
    public Vector3 Velocity { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

       // transform.Translate(Time.deltaTime * speed);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Velocity = other.gameObject.GetComponent<PlayerMovementManager>().Velocity;
        Move();

    }

    private void Move()
    {
        // to-do:
        // add wall collision check
        transform.position += Velocity;
    }
}
