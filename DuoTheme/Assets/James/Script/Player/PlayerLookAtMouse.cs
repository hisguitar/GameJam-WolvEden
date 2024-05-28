using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerLookAtMouse : NetworkBehaviour
{
    [SerializeField] private Transform cursor;
    [SerializeField] private Transform playerSprite;
    private Vector3 mousePosition;

    public Vector3 MousePosition { get { return mousePosition; } }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            cursor.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        MouseLook();
    }

    private void MouseLook()
    {
        mousePosition = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = MathF.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        cursor.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
