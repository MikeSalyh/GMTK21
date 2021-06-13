using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : WrappingObject
{
    public Vector2 velocity;
    public float minLaunchSpeed, maxLaunchSpeed;
    public bool flaggedForRemoval;

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

    public void Combine(Bullet other)
    {
        //Note: this method doesn't do removal. Just adds the others properties to this.

        //Add sizes:
        float myArea = rect.sizeDelta.x * rect.sizeDelta.y;
        float theirArea = other.rect.sizeDelta.x * other.rect.sizeDelta.y;
        float newLength = Mathf.Sqrt(myArea + theirArea);
        rect.sizeDelta = new Vector2(newLength, newLength);

        //Add velocities, using fake mass:
        Vector2 v1 = velocity * myArea;
        Vector2 v2 = other.velocity * theirArea;
        Vector2 output = (v1 + v2) / (myArea + theirArea);
        velocity = output;
    }
}
