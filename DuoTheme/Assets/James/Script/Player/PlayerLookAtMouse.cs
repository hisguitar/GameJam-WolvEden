using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookAtMouse : MonoBehaviour
{
    [SerializeField] private Transform cursor;
    [SerializeField] private Transform playerSprite;
    private Vector3 mousePosition;

    public Vector3 MousePosition
    {
        get { return mousePosition; }
    }
    private void Update()
    {
        MouseLook();
    }

    private void MouseLook()
    {
        mousePosition = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = MathF.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        cursor.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
