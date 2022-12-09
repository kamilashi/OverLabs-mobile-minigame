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
        if (IsMovable)
        {

            Vector3 otherVelocity = other.gameObject.GetComponent<MovementComponent>().Velocity;
            if(Vector3.Magnitude(Velocity) < Vector3.Magnitude(otherVelocity))
            {
                Velocity = otherVelocity;
                Debug.Log("new V = " + Velocity);
                Move();
            }
           
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsMovable)
        {
            stop = true;
        }
    }

    private void Move()
    {
        gameObject.GetComponent<Rigidbody2D>().MovePosition(transform.position + Velocity);
    }

}
