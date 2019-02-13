using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenHalf : short
{
    None,
    Left,
    Right
}

enum Axis : short
{
    None,
    X,
    Y,
    Z
}

public class InputController : MonoBehaviour
{

    float dragLockLimit = 5.0f;
    Vector2 drag = Vector2.zero;
    Vector2 previousMousePosition = Vector2.zero;
    bool isDragLockedToAxis = false;
    Axis lockedAxis = Axis.None;
    bool isMouseDown = false;
    bool hasDragged = false;
    ScreenHalf dragScreenHalf = ScreenHalf.None;
    private Controllable[] controllables;

    // Use this for initialization
    void Start()
    {
        controllables = GameObject.FindObjectsOfType<Controllable>();
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseInput();

        if (isMouseDown)
        {
            UpdateDrag();
        }
    }

    void GetMouseInput()
    {
        /*
        * Mouse Down
        */
        if (Input.GetMouseButtonDown(0))
        {
            // Reset drag variables
            ResetInputVars();
            // Get drag screen half
            dragScreenHalf = GetPointScreenHalf(Input.mousePosition);
        }

        /*
        * Mouse Drag
        */
        isMouseDown = Input.GetMouseButton(0);
        if (isMouseDown)
        {
            // Only add to drag once the mouse has moved
            if (hasDragged)
            {
                drag += (Vector2)Input.mousePosition - previousMousePosition;
            }
            previousMousePosition = Input.mousePosition;
            hasDragged = true;

            UpdateControllables();
        }

        /*
        * Mouse Up
        */
        if (Input.GetMouseButtonUp(0))
        {
            // TODO: release event
        }
    }

    void UpdateDrag()
    {
        if (drag.magnitude > dragLockLimit)
        {
            if (!isDragLockedToAxis)
            {
                LockDragToAxis();
            }
        }
    }

    void LockDragToAxis()
    {
        isDragLockedToAxis = true;

        bool isDragHorizontal;

        // In the event both drags are equal, pick an axis randomly
        if (drag.x == drag.y)
        {
            isDragHorizontal = Random.Range(0, 1) > 0.5 ? true : false;
        }
        else
        {
            isDragHorizontal = Mathf.Abs(drag.x) > Mathf.Abs(drag.y);
        }

        // Choose axis to lock to depending on the screen half and the drag direction
        switch (dragScreenHalf)
        {
            case ScreenHalf.Left:
                {
                    lockedAxis = isDragHorizontal ? Axis.Y : Axis.Z;
                }
                break;
            case ScreenHalf.Right:
                {
                    lockedAxis = isDragHorizontal ? Axis.Y : Axis.X;
                }
                break;
        }
    }

    ScreenHalf GetPointScreenHalf(Vector2 position)
    {
        Vector2 screenCenter = new Vector2(Screen.width, Screen.height) / 2.0f;

        float a = Vector2.SignedAngle(Vector2.up, screenCenter - position);

        if (a > 0 && a <= 180)
        {
            dragScreenHalf = ScreenHalf.Right;
        }
        else if (a > -180 && a <= 0)
        {
            dragScreenHalf = ScreenHalf.Left;
        }
        else
        {
            dragScreenHalf = ScreenHalf.None;
        }

        return dragScreenHalf;
    }

    Quaternion CalculateDragRotation()
    {

        Vector2 dragAngle = CalculateDragAngle();

        switch (lockedAxis)
        {
            case Axis.X:
                {
                    return Quaternion.AngleAxis(dragAngle.y * Mathf.Rad2Deg, Vector3.right);
                }
            case Axis.Y:
                {
                    return Quaternion.AngleAxis(dragAngle.x * Mathf.Rad2Deg, -Vector3.up);
                }
            case Axis.Z:
                {
                    return Quaternion.AngleAxis(dragAngle.y * Mathf.Rad2Deg, -Vector3.forward);
                }
            default:
                {
                    return Quaternion.Euler(0, 0, 0);
                }
        }
    }

    Vector2 CalculateDragAngle()
    {
        return drag / 100.0f; // TODO magic number
    }

    void UpdateControllables() {
        
        Quaternion rotation =  CalculateDragRotation();

        foreach(Controllable controllable in controllables) {
            controllable.SetRotation(rotation);
        }
    }

    void ResetInputVars()
    {
        drag = Vector2.zero;
        hasDragged = false;
        isDragLockedToAxis = false;
        lockedAxis = Axis.None;
    }
}
