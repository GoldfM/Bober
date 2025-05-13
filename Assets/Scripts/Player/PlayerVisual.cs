using System;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        ChangePlayerFacingDirection();
    }

    private void ChangePlayerFacingDirection()
    {
        Vector3 playerPos = Player.Instance.GetPlayerScreenPosition();
        Vector3 mousePosition = GameInput.Instance.GetMousePos();
        if (mousePosition.x < playerPos.x)
        {
            transform.parent.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.parent.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}