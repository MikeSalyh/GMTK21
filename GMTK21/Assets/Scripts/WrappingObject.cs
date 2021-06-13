using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrappingObject : MonoBehaviour
{
    protected RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Vector2 newPos = rect.localPosition;

        //if offscreen, wrap around!
        if (rect.localPosition.y > GameManager.SCREENHEIGHT / 2 + rect.sizeDelta.y)
        {
            newPos.y = (-GameManager.SCREENHEIGHT / 2) - rect.sizeDelta.y;
        }
        else if (rect.localPosition.y < -GameManager.SCREENHEIGHT / 2 - rect.sizeDelta.y)
        {
            newPos.y = (GameManager.SCREENHEIGHT / 2) + rect.sizeDelta.y;
        }

        if (rect.localPosition.x > GameManager.SCREENWIDTH / 2 + rect.sizeDelta.x)
        {
            newPos.x = (-GameManager.SCREENHEIGHT / 2) - rect.sizeDelta.x;
        }
        else if (rect.localPosition.x < -GameManager.SCREENWIDTH / 2 - rect.sizeDelta.x)
        {
            newPos.x = (GameManager.SCREENWIDTH / 2) + rect.sizeDelta.x;
        }

        rect.localPosition = newPos;
    }
}
