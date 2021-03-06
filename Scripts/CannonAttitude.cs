﻿using UnityEngine;
using System.Collections;

public class CannonAttitude : FocusHandler
{
    public float CummulativeAngleDelta;

    public Vector3 CursorStartPosition;
    public Vector3 ObjectStartRotation;

    public Vector3 CursorLastPosition;
    public Vector3 ObjectLastRotation;

    public Vector3 CursorPosition;

    private TextMesh Label;
    private bool IsRotating = false;

    void Start()
    {
        Label = GameObject.Find("RotateLabel").GetComponent<TextMesh>();
    }

    public override void HandleFocus()
    {
        Debug.Log("Entering CannonAttitude...");

        IsRotating = true;
        Cursor.visible = true;

        // Prepare/reset everything

        CummulativeAngleDelta = 0f;
        Label.text = CummulativeAngleDelta.ToString("N2") + "°";

        CursorStartPosition = CursorLastPosition = Input.mousePosition;
        ObjectStartRotation = ObjectLastRotation = transform.eulerAngles;
    }

    void Update()
    {
        if (IsRotating)
        {
            // Left mouse click
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Exiting CannonAttitude...");

                IsRotating = false;
                Cursor.visible = false;
                Label.text = null; // Clear

                gameObject.GetComponent<Handle>().Unfocus();
            }
            else
            {
                CursorPosition = Input.mousePosition;

                // Since Last OnMouseDrag()
                float dX = CursorPosition.x - CursorLastPosition.x;

                // Since Start
                float dY = CursorPosition.y - CursorStartPosition.y; // e.g. -400 px

                // Vary rotation sensitivity

                float ScrubSpeed;
                if (dY < 0)
                {
                    ScrubSpeed = 0.5f / Mathf.Ceil(dY / -50f); // In multiples of -50px
                }
                else
                {
                    ScrubSpeed = 0.5f;
                }

                // Rotate
                float AngleDelta = ScrubSpeed * -dX;
                transform.Rotate(new Vector3(0, AngleDelta, 0), Space.Self);

                // To display onscreen
                CummulativeAngleDelta += AngleDelta;
                Label.text = CummulativeAngleDelta.ToString("N2") + "°";

                CursorLastPosition = Input.mousePosition;
                ObjectLastRotation = transform.eulerAngles;
            }
        }
    }
}
