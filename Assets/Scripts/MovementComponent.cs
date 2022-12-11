using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [SerializeField]
    public Vector3 Velocity;
    [SerializeField]
    public bool IsMovable = true;
    public bool IsPlayer = false;
    public bool IsWall = false;
    [SerializeField]
    public bool blocked = false;
    private bool stop = false;
    public bool moveCommand = false;
    [SerializeField]
    private GameObject parent;

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


        if (moveCommand)
        {
           Move();
           moveCommand = false;
           //return;
        }

        if (stop)
        {
            Velocity = Vector3.zero;
            stop = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        MovementComponent collidedWith = other.gameObject.GetComponent<MovementComponent>();

        if ((!IsWall) && (collidedWith.IsWall)) {
            Debug.Log("collided with a wall!");
            sendBlockedToParents(1, -Velocity);
            return;
        }

        //be pushed when collided with an object of greater velocity
        if ((IsMovable) && (!collidedWith.blocked))
        {
            Vector3 otherVelocity = collidedWith.Velocity;
            Debug.Log("Puching");
            if (Vector3.Magnitude(Velocity) < Vector3.Magnitude(otherVelocity))
            {
                blocked = false;
                parent = other.gameObject;
                Velocity = otherVelocity;
                moveCommand = true;
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

    private void sendBlockedToParents(int level, Vector3 reverseVelocity)
    {

        blocked = true;
        Velocity = reverseVelocity;
        if (parent != null)
        {
            parent.GetComponent<MovementComponent>().sendBlockedToParents(level+1, reverseVelocity);
        }
        // reset:
        parent = null;
        moveCommand = true;
        stop = true;
        blocked = false;
    }

}
