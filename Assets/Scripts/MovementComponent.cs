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
    public bool _blocked = false;
    private bool _stop = false;
    internal bool moveCommand = false;
    [SerializeField]
    private GameObject _parent;
    private static bool _finalCollisionHappened = false;

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

        if (_stop)
        {
            Velocity = Vector3.zero;
            _finalCollisionHappened = false;
            _stop = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        MovementComponent collidedWith = other.gameObject.GetComponent<MovementComponent>();

        if ((!IsWall) && (collidedWith.IsWall)) {
            Debug.Log("Wall collision!");
            sendBlockedToParents(1, -Velocity);
            return;
        }

        //be pushed when collided with an object of greater velocity
        if ((IsMovable) && (!collidedWith._blocked))
        {
            Vector3 otherVelocity = collidedWith.Velocity;
            if (Vector3.Magnitude(Velocity) < Vector3.Magnitude(otherVelocity))
            {
                _blocked = false;
                _parent = other.gameObject;
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
            _stop = true;
        }
    }

    // Move one unit in direction of current velocity
    private void Move()
    {
        gameObject.GetComponent<Rigidbody2D>().MovePosition(transform.position + Velocity);
    }

    private void sendBlockedToParents(int level, Vector3 reverseVelocity)
    {

        _blocked = true;
        Velocity = reverseVelocity;
        if (_parent != null) // move up the parent chain
        {
            _parent.GetComponent<MovementComponent>().sendBlockedToParents(level+1, reverseVelocity);
        }
        else if((IsPlayer)&&(!_finalCollisionHappened)) // if reached the player, play the collision animation
        {
            Vector3 position = transform.position + (-reverseVelocity / 2); // place it between the player and the closest collider
            GameObject collisionAnimation = (GameObject)Instantiate(Resources.Load("Ouch"), position, Quaternion.identity, transform);
            Debug.Log("Ouch! reverseVelocity = "+ reverseVelocity);
            _finalCollisionHappened = true;
        }
        // reset:
        resetAll();
    }

    private void resetAll()
    {
        _parent = null;
        moveCommand = true;
        _stop = true;
        _blocked = false;
    }

}
