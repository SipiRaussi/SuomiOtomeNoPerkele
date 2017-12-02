﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Player))]
public class PlayerInput : Creature
{
    public Animator animator;
    Player player;
    public  float groundAcceleration,  airAcceleration;
    public  float groundDecceleration, airDecceleration;
    public  float xSpeed;
    public  float xCurrent;
    public  bool  attacking;
    public  float maxAttack;
    private float attackTime;
    bool facingRight;
    Vector2 hitBoxCenter, hitBoxSize;
    bool gameOver;
    float gameOverTimer;

    private CollisionInfo collisions;

    public override void Start()
    {
        gameOver = false;
        gameOverTimer = 0;

        facingRight = true;
        if (hp <= 0)
            hp = 1;

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

        if (!animator)
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
        }

        hitBoxSize = new Vector2(0.5f, 0.5f);

        base.Start();
    }

    public override void Update()
    {
        AttackInput();
        MoveInput();
        HandleAnimation();
        collisions = player.controller.collisions;

        if (gameOver)
        {
            gameOverTimer += Time.deltaTime;
            if (gameOverTimer >= 5)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        base.Update();
    }

    void OnDrawGizmos()
    {
        if (attacking)
        {
            Gizmos.color = new Color(0, 1, 0, .5f);
            Gizmos.DrawCube(hitBoxCenter, hitBoxSize);
        }
    }

    void AttackInput()
    {
        hitBoxCenter = new Vector2(transform.position.x, transform.position.y) +
                     new Vector2(facingRight ? 0.15f : -0.15f, 0);

        if (attacking)
        {
            AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
            if (asi.IsName("attackground") || asi.IsName("attackair"))
            {
                List<RaycastHit2D> hits = Physics2D.BoxCastAll(hitBoxCenter, hitBoxSize, 0,
                                          Vector2.zero, 0).ToList();

                if (hits.Count > 0)
                {
                    foreach(RaycastHit2D hit in hits)
                    {
                        Creature c = hit.transform.gameObject.GetComponent<Creature>();
                        if (c && c != this)
                        {
                            c.TakeDamage(1.0f);
                        }
                    }
                }
            }

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
            if (hp > 0 && Input.GetButtonDown("Attack"))
            {
                animator.SetBool("airattack",   !collisions.below);
                animator.SetBool("groundattack", collisions.below);
                attacking = true;
            }
        }
    }

    void MoveInput()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (hp <= 0)
            directionalInput = Vector2.zero;

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

        if (xCurrent > 0)      facingRight = true;
        else if (xCurrent < 0) facingRight = false;
        directionalInput.x = xCurrent;
        player.SetDirectionalInput(directionalInput);

        if (!attacking && hp > 0)
        {
            if (Input.GetButtonDown("Jump"))
            {
                player.OnJumpInputDown();
            }
            if (Input.GetButtonUp("Jump"))
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
        attacking  = false;
        attackTime = 0;
    }

    public override void TakeDamage(float damage)
    {
        if (invis > 0)
            return;

        player.Push(new Vector2(facingRight ? -3.34f : 3.34f, 4f));
        StopAttack();
        hp -= damage;

        invis = invisTime;
        if (hp <= 0)
        {
            hue = Color.black;
            srenderer.flipY = true;
            Vector3 pos = transform.GetChild(0).transform.localPosition;
            pos.y = 0.31f;
            transform.GetChild(0).transform.localPosition = pos;
        }
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverTimer = 0;
    }
}