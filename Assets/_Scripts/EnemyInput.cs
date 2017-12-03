using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class EnemyInput : Creature
{
    public Animator animator;
    PlayerInput player;
    Player creature;
    public float groundAcceleration, airAcceleration;
    public float groundDecceleration, airDecceleration;
    public float xSpeed;
    public float xCurrent;
    public bool attacking;
    public float maxAttack;
    private float attackTime;

    private CollisionInfo collisions;

    public override void Start()
    {
        if (hp <= 0)
            hp = 1;

        if (!player)
        {
            GameObject g = GameObject.FindWithTag("Player");
            if (g)
            {
                PlayerInput pi = g.GetComponent<PlayerInput>();
                if (pi)
                    player = pi;
            }
        }

        creature = GetComponent<Player>();
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

        if (!animator)
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
        }

        base.Start();
    }

    public override void Update()
    {
        AttackInput();
        MoveInput();
        HandleAnimation();
        collisions = creature.controller.collisions;
        base.Update();
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
            //if (Input.GetButtonDown("Attack"))
            //{
            //    animator.SetBool("airattack", !collisions.below);
            //    animator.SetBool("groundattack", collisions.below);
            //    attacking = true;
            //}
        }
    }

    void MoveInput()
    {
        Vector2 directionalInput = Vector2.zero; //new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (directionalInput.x != 0)
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
        creature.SetDirectionalInput(directionalInput);

        if (!attacking)
        {
            //if (Input.GetButtonDown("Jump"))
            //{
            //    creature.OnJumpInputDown();
            //}
            //if (Input.GetButtonUp("Jump"))
            //{
            //    creature.OnJumpInputUp();
            //}
        }
    }

    void HandleAnimation()
    {
        if (xCurrent > 0)
            srenderer.flipX = false;
        else if (xCurrent < 0)
            srenderer.flipX = true;

        animator.SetBool("air", !collisions.below);
        animator.SetBool("run", Mathf.Abs(xCurrent) > 0);
        animator.SetBool("attack", attacking);

        //if (collisions.below)
        //{
        //    if (attacking)
        //    {
        //        Debug.Log("Attack Grounded");
        //    }
        //    else
        //    {
        //        Debug.Log("Jotaki muuta ground");
        //    }
        //}
        //else
        //{
        //    if (attacking)
        //    {
        //        Debug.Log("Attack Aerial");
        //    }
        //    else
        //    {
        //        Debug.Log("Jotaki muuta air");
        //    }
        //}
    }

    public override void StopAttack()
    {
        attacking = false;
        attackTime = 0;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }
}