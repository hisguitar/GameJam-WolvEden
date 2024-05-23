using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookAtMouse : MonoBehaviour
{
    [SerializeField] private Transform playerSprite;
    [SerializeField] private Transform cursor;
    private void Update()
    {
        MouseLook();
    }

    private void MouseLook()
    {
        var mousePosition = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = MathF.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        cursor.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        if (transform.position.x > mousePosition.x)
        {
            playerSprite.localScale = new Vector3(-1, 1, 1);
        }
        else if(transform.position.x < mousePosition.x)
        {
            playerSprite.localScale = new Vector3(1, 1, 1);
        }
    }
}
