using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrappingObject : MonoBehaviour
{
    public RectTransform rect;

    private void Awake()
    {
        if(rect == null)
            rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Vector2 newPos = rect.localPosition;

        //if offscreen, wrap around!
        if (rect.localPosition.y > GameManager.SCREENHEIGHT / 2 + Radius)
        {
            newPos.y = (-GameManager.SCREENHEIGHT / 2) - Radius;
            newPos.x *= -1;
        }
        else if (rect.localPosition.y < -GameManager.SCREENHEIGHT / 2 - Radius)
        {
            newPos.y = (GameManager.SCREENHEIGHT / 2) + Radius;
            newPos.x *= -1;
        }

        if (rect.localPosition.x > GameManager.SCREENWIDTH / 2 + Radius)
        {
            newPos.x = (-GameManager.SCREENHEIGHT / 2) - Radius;
            newPos.y *= -1;
        }
        else if (rect.localPosition.x < -GameManager.SCREENWIDTH / 2 - Radius)
        {
            newPos.x = (GameManager.SCREENWIDTH / 2) + Radius;
            newPos.y *= -1;
        }

        rect.localPosition = newPos;
    }

    public float Radius
    {
        get { return rect.sizeDelta.x; }
    }
}
