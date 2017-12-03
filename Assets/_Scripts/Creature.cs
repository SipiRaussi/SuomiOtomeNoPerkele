using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public float hp = 3;
    protected float invis;
    public float invisTime = 0.4f;
    public bool isPlayer = false;
    protected Color hue;

    public SpriteRenderer srenderer;
    // Use this for initialization
    public virtual void Start()
    {
        isPlayer = false;
        GameObject g = GameObject.FindWithTag("Player");
        if (g)
        {
            if (g == transform.gameObject)
            {
                isPlayer = true;
            }
        }

        if (!srenderer)
        {
            if (transform.childCount > 0)
            {
                srenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            }
        }

        hue = Color.white;
    }

    public virtual void Update()
    {
        if (invis > 0)
        {
            invis -= Time.deltaTime;

            if (srenderer)
            {
                if (invis > 0)
                {
                    Color col = new Color(1.0f, 0.4f, 0.4f, 0.5f);
                    srenderer.color = col;
                }
            }
            else
            {
                Debug.Log("No spriterendere " + name);
            }
        }
        else
        {
            if (srenderer)
                srenderer.color = hue;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (invis > 0)
            return;

        invis = invisTime;
        StopAttack();
        hp -= damage;
        if (!isPlayer && hp <= 0)
            Destroy(this.gameObject);
    }

    public virtual void StopAttack()
    {

    }
}
