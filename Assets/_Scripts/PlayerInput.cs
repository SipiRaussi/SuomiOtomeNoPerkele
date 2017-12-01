using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    public SpriteRenderer srenderer;
    Player player;
    public float acceleration;
    public float decceleration;
    public float xSpeed;
    public float xCurrent;

    void Start()
    {
        player = GetComponent<Player>();
        if (xSpeed <= 0)
            xSpeed = 0.01f;
        if (acceleration == 0)
            acceleration = 0.01f;
        else if (acceleration < 0)
            acceleration = Mathf.Abs(acceleration);

        if (decceleration == 0)
            decceleration = 0.01f;
        else if (decceleration < 0)
            decceleration = Mathf.Abs(decceleration);

        xCurrent = 0;

        if (!srenderer)
        {
            srenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (directionalInput.x != 0)
        {
            if (directionalInput.x > 0)
                xCurrent = xCurrent + acceleration > xSpeed ? xSpeed : xCurrent + acceleration;
            else
                xCurrent = xCurrent - acceleration < -xSpeed ? -xSpeed : xCurrent - acceleration;
        }
        else
        {
            if (Mathf.Abs(xCurrent) - decceleration <= 0)
                xCurrent = 0;
            else
                xCurrent = xCurrent < 0 ? xCurrent + decceleration : xCurrent - decceleration;
        }

        directionalInput.x = xCurrent;
        player.SetDirectionalInput(directionalInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.OnJumpInputDown();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            player.OnJumpInputUp();
        }

        HandleAnimation();
    }

    void HandleAnimation()
    {
        if (xCurrent > 0)
        {
            srenderer.flipX = false;
        }
        else if (xCurrent < 0)
        {
            srenderer.flipX = true;
        }
    }
}
