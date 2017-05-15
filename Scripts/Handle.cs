using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEditor;
using System;

public class Handle : MonoBehaviour {

    public Material HandleMat;
    public Material HandleHoverMat;
    public Material HandleFocusMat;
    public Renderer Render;

    // Use Hover(), Unhover(), Focus(), Unfocus() methods
    private bool IsHover = false;
    private bool IsFocus = false;

    void Start()
    {
        Render = gameObject.GetComponent<MeshRenderer>();
        Render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        Render.receiveShadows = false;

        // No hover, no focus to start
        Render.material = HandleMat;
    }

    public void Hover()
    {
        IsHover = true;
        Render.material = HandleHoverMat;

        // Invoke FocusHandler Hint
        string Hint = gameObject.GetComponent<FocusHandler>().Hint();
        gameObject.transform.FindChild("Hint").GetComponent<TextMesh>().text = Hint;
        gameObject.transform.FindChild("Hint").transform.LookAt(GameObject.Find("FPSController").transform);
    }

    public void Unhover()
    {
        IsHover = false;
        Render.material = HandleMat;
        gameObject.transform.FindChild("Hint").GetComponent<TextMesh>().text = "";
    }

    public void Focus()
    {
        IsFocus = true;
        Render.material = HandleFocusMat;
        GameObject.Find("FPSController").GetComponent<FirstPersonController>().LockPlayer();
        // Invoke FocusHandler
        gameObject.GetComponent<FocusHandler>().HandleFocus();
    }

    // Invoked from within FocusHandler
    public void Unfocus()
    {
        IsFocus = false;
        Render.material = HandleHoverMat;
        GameObject.Find("FPSController").GetComponent<FirstPersonController>().UnlockPlayer();
    }

    void Update()
    {
        // Must hover first to focus
        if (IsHover)
        {
            // Left mouse was clicked since previous frame
            if (Input.GetMouseButtonDown(0))
            {
                if (!IsFocus)
                {
                    Focus();
                }
            }
        }
    }

}