using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum TwoHeadedState
{
    IdleFly,
    FlyAttack,
    ProjectileAttack,
    Dead
}

public class TwoHeaded : Creature
{
    public float   timeCounter, stateChangeTime = 3;
    public float   speed = 0;
    float          xSpeed;
    public float   speedMultiplier = 1;
    public         TwoHeadedState state;
    Animator       animator;

    PlayerInput    player;
    public float   amplitude;
    public float   amplitudeSpeed;
    bool           goRight;
    bool goUp;

    public float    distanceToPlayer;
    public float    distanceToPlayerX;
    public Vector2  directionToPlayer;
    public bool     playerIsOnRightSide;

    private float startFlyAttack;
    Vector2 lockDir;
    Vector2 lockTarget;
    Vector2 lockStart;

    bool returnToSky;
    public float skyY;
    float deadTime;

    Vector2 deadVelocity;

    // Use this for initialization
    public override void Start()
    {
        goRight = true;
        returnToSky = false;
        state = TwoHeadedState.IdleFly;

        if (!animator)
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
        }

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

        base.Start();
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        Vector2 heading     = player.transform.position - transform.position;
        distanceToPlayer    = heading.magnitude;
        distanceToPlayerX   = Mathf.Abs(player.transform.position.x - transform.position.x);
        directionToPlayer   = distanceToPlayer != 0 ? heading / distanceToPlayer : Vector2.zero;
        playerIsOnRightSide = player.transform.position.x >= transform.position.x;

        timeCounter += Time.deltaTime;

        switch (state)
        {
            case TwoHeadedState.IdleFly:
                UpdateIdle();
                break;
            case TwoHeadedState.FlyAttack:
                UpdateFlyAttack();
                break;
            case TwoHeadedState.ProjectileAttack:
                UpdateProjectileAttack();
                break;
            case TwoHeadedState.Dead:
                UpdateDead();
                break;
        }

        base.Update();
	}

    public void UpdateIdle()
    {
        if (hp > 0)
        {
            AnimatorSetBools(false, false, false);
            
        }

        if (goRight)
        {
            if (xSpeed < 0)
                xSpeed += Time.deltaTime * 0.5f * speedMultiplier;
            else
                xSpeed += Time.deltaTime * speedMultiplier;
            if (xSpeed > speed)
                xSpeed = speed;
        }
        else
        {
            if (xSpeed > 0)
                xSpeed -= Time.deltaTime * 0.5f * speedMultiplier;
            else
                xSpeed -= Time.deltaTime * speedMultiplier;
            if (xSpeed < -speed)
                xSpeed = -speed;
        }

        if (distanceToPlayerX >= 2)
        {
            if (playerIsOnRightSide && !goRight)
            {
                goRight = !goRight;
                if (timeCounter > 2)
                    timeCounter -= 1;
            }
            else if (!playerIsOnRightSide && goRight)
            {
                goRight = !goRight;
                if (timeCounter > 2)
                    timeCounter -= 1;
            }
        }


        Vector3 pos = transform.position;
        pos.y = pos.y + (amplitude * Mathf.Sin(amplitudeSpeed * Time.time));
        pos.x += xSpeed;

        transform.position = pos;

        if (timeCounter >= stateChangeTime)
        {
            timeCounter = 0;
            switch (Random.Range(0, 8))
            {
                case 0: case 1: state = TwoHeadedState.FlyAttack; xSpeed = 0; break;
                case 2: case 3: state = TwoHeadedState.ProjectileAttack; xSpeed = 0; break;
                default: goRight = !goRight; break;
            }
        }
    }

    public void UpdateFlyAttack()
    {
        if (startFlyAttack < 2)
        {
            if (hp > 0)
                AnimatorSetBools(false, false, false);
            startFlyAttack += Time.deltaTime;

            Vector3 pos = transform.position;
            pos.x += Random.Range(-0.05f, 0.05f);
            pos.y += Random.Range(-0.05f, 0.05f);
            transform.position = pos;

            if (startFlyAttack >= 2)
            {
                lockDir      = directionToPlayer;
                lockTarget   = player.transform.position;
                lockStart    = transform.position;
                goRight      = transform.position.x < player.transform.position.x;
                goUp         = transform.position.y > player.transform.position.y;
                hue = Color.white;
            }
            else
            {
                float percent = startFlyAttack / 2;
                if (percent > 1) percent = 1;
                else if (percent < 0) percent = 0;
                hue = Color.Lerp(Color.white, Color.red, percent);
            }
        }
        else
        {
            if (hp > 0)
                AnimatorSetBools(true, false, false);

            if (returnToSky)
            {
                Vector3 pos = transform.position;
                pos.y += Time.deltaTime;

                if (pos.y > skyY)
                {
                    state          = TwoHeadedState.IdleFly;
                    timeCounter    = 0;
                    startFlyAttack = 0;
                    returnToSky    = false;
                }

                transform.position = pos;
            }
            else
            {
                Vector3 pos = transform.position;
                pos.x += lockDir.x * 0.2f;
                pos.y += lockDir.y * 0.2f;
                transform.position = pos;


                List<RaycastHit2D> hits = Physics2D.BoxCastAll(transform.position, new Vector2(0.8f, 1.0f), 0,
                          Vector2.zero, 0).ToList();

                if (hits.Count > 0)
                {
                    foreach (RaycastHit2D hit in hits)
                    {
                        Creature c = hit.transform.gameObject.GetComponent<Creature>();
                        if (c && c != this && c.isPlayer)
                        {
                            c.TakeDamage(1.0f);
                        }
                    }
                }

                if (goRight)
                {
                    if (transform.position.x >= lockTarget.x)
                    {
                        if (goUp)
                        {
                            if (transform.position.y < lockTarget.y)
                                returnToSky = true;
                        }
                        else
                        {
                            if (transform.position.y > lockTarget.y)
                                returnToSky = true;
                        }
                    }
                }
                else
                {
                    if (transform.position.x <= lockTarget.x)
                    {
                        if (goUp)
                        {
                            if (transform.position.y < lockTarget.y)
                                returnToSky = true;
                        }
                        else
                        {
                            if (transform.position.y > lockTarget.y)
                                returnToSky = true;
                        }
                    }
                }
            }
        }
    }

    public void UpdateProjectileAttack()
    {
        if (hp > 0)
        {
            Debug.Log("Ei oo projectile attackia, takas idleen siitä :DDDD");
            AnimatorSetBools(false, true, false);
            state = TwoHeadedState.IdleFly;
        }
    }

    public void UpdateDead()
    {
        if (deadTime < 5)
        {
            deadTime += Time.deltaTime;
            float percentage = deadTime / 5;
            if (percentage > 1) percentage = 1;
            else if (percentage < 0) percentage = 0;

            hue = Color.Lerp(Color.white, new Color(0.5f, 0.5f, 0.5f, 1.0f), percentage);
            if (deadTime >= 5)
            {
                deadVelocity = new Vector2(Random.Range(-0.05f, 0.05f), Random.Range(0.05f, 0.15f));
                srenderer.flipY = true;
                Vector3 posc = transform.GetChild(0).transform.localPosition;
                posc.y = 1.25f;
                transform.GetChild(0).transform.localPosition = posc;
            }

            Vector3 pos = transform.position;
            pos.x += Random.Range(-0.025f, 0.025f);
            pos.y += Random.Range(-0.025f, 0.025f);
            pos.y += Time.deltaTime * 0.2f;
            transform.position = pos;
        }
        else
        {
            Vector3 pos = transform.position;
            pos.x += deadVelocity.x;
            pos.y += deadVelocity.y;
            deadVelocity.y -= Time.deltaTime * 0.25f;
            transform.position = pos;

            if (pos.y <= -100)
            {
                player.GameOver();
                Destroy(this.gameObject);
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        if (invis > 0 || hp <= 0)
            return;

        hp -= damage;
        invis = invisTime;

        if (hp <= 0)
        {
            deadTime = 0;
            hp = 0;
            state = TwoHeadedState.Dead;
            AnimatorSetBools(false, false, true);
        }
    }

    void AnimatorSetBools(bool fly, bool projectile, bool dead)
    {
        animator.SetBool("flyattack", fly);
        animator.SetBool("projectileattack", projectile);
        animator.SetBool("dead", dead);
    }
}
