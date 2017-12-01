using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    public SpriteRenderer srenderer;
    Player player;
    public  float groundAcceleration,  airAcceleration;
    public  float groundDecceleration, airDecceleration;
    public  float xSpeed;
    public  float xCurrent;
    public  bool  attacking;
    public  float maxAttack;
    private float attackTime;

    private CollisionInfo collisions;

    void Start()
    {
        player = GetComponent<Player>();
        if (xSpeed <= 0)
            xSpeed = 0.01f;
        if (groundAcceleration == 0)
            groundAcceleration = 0.01f;
        else if (groundAcceleration < 0)
            groundAcceleration = Mathf.Abs(groundAcceleration);

        if (groundDecceleration == 0)
            groundDecceleration = 0.01f;
        else if (groundDecceleration < 0)
            groundDecceleration = Mathf.Abs(groundDecceleration);

        xCurrent = 0;

        if (!srenderer)
        {
            srenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        AttackInput();
        MoveInput();
        HandleAnimation();
        collisions = player.controller.collisions;
    }

    void AttackInput()
    {
        if (attacking)
        {
            attackTime += Time.deltaTime;
            if (attackTime >= maxAttack)
            {
                attackTime = 0;
                attacking = false;
            }
            return;
        }
        else
        {
            if (Input.GetButtonDown("Attack"))
            {
                attacking = true;
            }
        }
    }

    void MoveInput()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (!attacking && directionalInput.x != 0)
        {
            if (directionalInput.x > 0)
            {
                if (collisions.below)
                    xCurrent += groundAcceleration * Time.deltaTime;
                else
                    xCurrent += airAcceleration * Time.deltaTime;

                if (collisions.right)
                    xCurrent = 0.2f;

                if (xCurrent > xSpeed)
                    xCurrent = xSpeed;
            }
            else if (directionalInput.x < 0)
            {
                if (collisions.below)
                    xCurrent -= groundAcceleration * Time.deltaTime;
                else
                    xCurrent -= airAcceleration * Time.deltaTime;

                if (collisions.left)
                    xCurrent = -0.2f;

                if (xCurrent < -xSpeed)
                    xCurrent = -xSpeed;
            }
        }
        else
        {
            if (xCurrent > 0)
            {
                if (collisions.below)
                    xCurrent -= groundDecceleration * Time.deltaTime;
                else
                    xCurrent -= airDecceleration * Time.deltaTime;

                if (xCurrent < 0)
                    xCurrent = 0;
            }
            else if (xCurrent < 0)
            {
                if (collisions.below)
                    xCurrent += groundDecceleration * Time.deltaTime;
                else
                    xCurrent += airDecceleration * Time.deltaTime;

                if (xCurrent > 0)
                    xCurrent = 0;
            }
        }

        directionalInput.x = xCurrent;
        player.SetDirectionalInput(directionalInput);

        if (!attacking)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.OnJumpInputDown();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                player.OnJumpInputUp();
            }
        }
    }

    void HandleAnimation()
    {
        if (xCurrent > 0)
            srenderer.flipX = false;
        else if (xCurrent < 0)
            srenderer.flipX = true;

        if (collisions.below)
        {
            if (attacking)
            {
                Debug.Log("Attack Grounded");
            }
            else
            {
                Debug.Log("Jotaki muuta ground");
            }
        }
        else
        {
            if (attacking)
            {
                Debug.Log("Attack Aerial");
            }
            else
            {
                Debug.Log("Jotaki muuta air");
            }
        }
    }
}