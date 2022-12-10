using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [SerializeField]
    public Vector3 Velocity;
    [SerializeField]
    public bool IsMovable = true;
    private bool stop = false;

    private void Awake()
    {
        Velocity = new Vector3(0, 0, 0);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stop)
        {
            Velocity = Vector3.zero;
            stop = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //be pushed when collided with an object of greater velocity
        if (IsMovable)
        {
            Vector3 otherVelocity = other.gameObject.GetComponent<MovementComponent>().Velocity;
            if(Vector3.Magnitude(Velocity) < Vector3.Magnitude(otherVelocity))
            {
                Velocity = otherVelocity;
                Move();
            }
           
        }
    }

    // for synchronization safety stop the movement on the next frame
    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsMovable)
        {
            stop = true;
        }
    }

    // Move one unit in direction of current velocity
    private void Move()
    {
        gameObject.GetComponent<Rigidbody2D>().MovePosition(transform.position + Velocity);
    }

}
