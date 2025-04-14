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
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
}
