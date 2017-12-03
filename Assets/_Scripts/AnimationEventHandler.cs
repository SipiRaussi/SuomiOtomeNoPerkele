using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour {

    PlayerInput player;
    EnemyInput  enemy;
	// Use this for initialization
	void Start ()
    {
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
        if (!player && !enemy)
        {
            GameObject g = transform.parent.gameObject;
            if (g)
            {
                EnemyInput ei = g.GetComponent<EnemyInput>();
                if (ei)
                    enemy = ei;
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void AttackEnd()
    {
        if (player)
            player.StopAttack();
        if (enemy)
            enemy.StopAttack();
    }
}
