using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    private Vector3 tapPosDown;
    private Vector3 tapPosUp;
    enum Direction
    {
        Right,
        Up,
        Left,
        Down
    };
    Direction curDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            tapPosDown = Input.mousePosition;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            tapPosUp = Input.mousePosition;
            Vector3 direction = Vector3.Normalize(tapPosUp - tapPosDown);
            curDirection = ProcessDirection(direction);
            Move(curDirection);
        }
    }

    private Direction ProcessDirection(Vector3 direction)
    {
        int shiftedQuarter = 0;
        float angleRad = Mathf.Acos(direction.x)+Mathf.PI/4;
        Vector3 refDiag = Vector3.Normalize(new Vector3(1, 1, 0));
        if (Vector3.Dot(direction, refDiag) >= 0) { shiftedQuarter = (int)Mathf.Ceil((angleRad ) / (Mathf.PI / 2));  }
        else if(Vector3.Dot(direction, refDiag) < 0) { shiftedQuarter = (int)Mathf.Ceil((2*Mathf.PI + Mathf.PI / 2 - angleRad) / (Mathf.PI / 2));  }
        Debug.Log("angle = " + angleRad + " , newDirection = " + shiftedQuarter);
        Direction newDirection = (Direction)(shiftedQuarter-1);
        return newDirection;
    }

    private void Move(Direction direction)
    {
        switch (direction)
        {
            case Direction.Right:
                transform.position += new Vector3(1, 0, 0);
                break;
            case Direction.Up:
                transform.position += new Vector3(0, 1, 0);
                break;
            case Direction.Left:
                transform.position += new Vector3(-1, 0, 0);
                break;
            case Direction.Down:
                transform.position += new Vector3(0, -1, 0);
                break;

        }
    }
}
