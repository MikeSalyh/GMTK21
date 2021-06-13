using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : WrappingObject
{
    public Vector2 velocity;
    public float minLaunchSpeed, maxLaunchSpeed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        transform.Translate(velocity * Time.deltaTime);
    }

    public void Configure(Vector2 velocity)
    {
        float a2 = (GameManager.SCREENHEIGHT / 2) * (GameManager.SCREENHEIGHT / 2);
        float b2 = (GameManager.SCREENWIDTH / 2) * (GameManager.SCREENWIDTH / 2);
        float hyp = Mathf.Sqrt(a2 + b2);

        float normalizeDist = Mathf.InverseLerp(0, hyp, velocity.magnitude);
        float speedOnLaunch = Mathf.Lerp(minLaunchSpeed, maxLaunchSpeed, normalizeDist);

        this.velocity = velocity.normalized * speedOnLaunch;
    }
}
