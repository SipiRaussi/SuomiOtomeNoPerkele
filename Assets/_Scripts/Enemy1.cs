using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{


    public float speed;
    void Start()
    {

    }

    // Update is called once per frame
    public float amplitudeX = 5.0f;
    public float amplitudeY = 1.0f;
    public float omegaX = 1.0f;
    public float omegaY = 1.0f;
    float index;
    public void Update()
    {
        index += Time.deltaTime;
        float x = amplitudeX * Mathf.Cos(omegaX * index);
        float y = Mathf.Abs(amplitudeY * Mathf.Sin(omegaY * index));
        transform.localPosition = new Vector3(x, y, 0);
    }
    void OnCollisionEnter2D(Collision2D col)
    {



        GameObject.Destroy(gameObject);

    }


}
