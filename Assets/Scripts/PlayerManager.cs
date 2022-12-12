using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PlayerManager : MonoBehaviour
{
    private Vector3 _tapPosDown;
    private Vector3 _tapPosUp;

    private Direction _curDirection;
    [SerializeField]
    internal MovementComponent MovementComponent;

    private void Awake()
    {

        transform.position = Vector3.zero;
    }
    // Start is called before the first frame update
    void Start()
    {
        MovementComponent = gameObject.GetComponent<MovementComponent>();
        MovementComponent.IsMovable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _tapPosDown = Input.mousePosition;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _tapPosUp = Input.mousePosition;
            Vector3 direction = Vector3.Normalize(_tapPosUp - _tapPosDown);
            if (direction.magnitude != 0) {
                _curDirection = ProcessDirection(direction);
                Move(_curDirection);
            }
            
        }
    }

    private Direction ProcessDirection(Vector3 direction)
    {
        int shiftedQuarter = 0;
        float angleRad = Mathf.Acos(direction.x)+Mathf.PI/4;
        Vector3 refDiag = Vector3.Normalize(new Vector3(1, 1, 0));
        if (Vector3.Dot(direction, refDiag) >= 0) { shiftedQuarter = (int)Mathf.Ceil((angleRad ) / (Mathf.PI / 2));  }
        else if(Vector3.Dot(direction, refDiag) < 0) { shiftedQuarter = (int)Mathf.Ceil((2*Mathf.PI + Mathf.PI / 2 - angleRad) / (Mathf.PI / 2));  }
  
       Direction newDirection = (Direction)(shiftedQuarter-1);
        return newDirection;
    }

    private void Move(Direction direction)
    {
        switch (direction)
        {
            case Direction.Right:
                MovementComponent.Velocity = new Vector3(1, 0, 0);
                break;
            case Direction.Up:
                MovementComponent.Velocity = new Vector3(0, 1, 0);
                break;
            case Direction.Left:
                MovementComponent.Velocity = new Vector3(-1, 0, 0);
                break;
            case Direction.Down:
                MovementComponent.Velocity = new Vector3(0, -1, 0);
                break;

        }

        MovementComponent.blocked = false;
        MovementComponent.moveCommand = true;
    }
}
