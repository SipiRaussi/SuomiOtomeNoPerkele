using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : Creature
{
    public float damage;
    public Vector2 velocity;
    public float gravity;
    public bool canBeDestroyed;
 
    Vector2 hitBoxSize;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 pos = transform.position;
        pos.x += velocity.x;
        pos.y += velocity.y;

        velocity.y += gravity;

        transform.position = pos;


        List<RaycastHit2D> hits = Physics2D.BoxCastAll(transform.position, hitBoxSize, 0,
                          Vector2.zero, 0).ToList();

        if (hits.Count > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                Creature c = hit.transform.gameObject.GetComponent<Creature>();
                if (c && c != this)
                {
                    if (isPlayer && !c.isPlayer)
                        c.TakeDamage(damage);
                    else if (!isPlayer && c.isPlayer)
                        c.TakeDamage(damage);
                }
            }
        }
    }

    public void Initialize(float dmg, Vector2 vel, float grav, bool player, bool destroy)
    {
        hp = 0.0001f;
        damage = dmg;
        velocity = vel;
        gravity  = grav;
        isPlayer = player;
        canBeDestroyed = destroy;

        BoxCollider2D box = GetComponent<BoxCollider2D>();
        hitBoxSize = box.size;
    }
    public override void TakeDamage(float damage)
    {
        if (canBeDestroyed)
            base.TakeDamage(damage);
    }
}
