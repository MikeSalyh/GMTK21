using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : WrappingObject
{
    public Vector2 velocity;
    public float minLaunchSpeed, maxLaunchSpeed;
    public bool flaggedForRemoval;
    public ParticleSystem collisionParticles, speedParticles;

    public enum Type
    {
        Red,
        Green,
        Blue
    }
    public Color[] colors;
    public Type myType;


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

    public void Configure(Type t, Vector2 velocity)
    {
        this.myType = t;
        GetComponent<RawImage>().color = colors[(int)t];
        var a = collisionParticles.main;
        a.startColor = colors[(int)t];
        var b = speedParticles.main;
        b.startColor = colors[(int)t];


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

        var a = speedParticles.emission;
        a.rateOverTime = velocity.magnitude / 10f;

        var b = collisionParticles.emission;

        b.SetBursts(
              new ParticleSystem.Burst[] {
                  new ParticleSystem.Burst(0f, theirArea / 50f)
        });
        collisionParticles.Play();
    }
}
