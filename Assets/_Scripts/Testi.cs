using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testi : MonoBehaviour {

    public AnimationCurve curveY, curveX;


    float second;
    float startY, startX;
	// Use this for initialization
	void Start ()
    {
        startY = transform.position.y;
        startX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update ()
    {
        second += Time.deltaTime;
        if (second > 1)
            second -= 1;

        Vector3 pos = transform.position;
        pos.y = startY + curveY.Evaluate(second);
        pos.x = startX + curveX.Evaluate(second);
        transform.position = pos;
	}
}
