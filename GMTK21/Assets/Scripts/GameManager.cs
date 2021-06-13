using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        Aiming,
        Firing
    }

    public const int SCREENWIDTH = 600, SCREENHEIGHT = 600; //forgive me for this sin.

    public State currentState = State.Aiming;
    public CursorFollowLauncher cursor;
    public GameObject shotFab;
    public Transform shotParent;

    private Bullet currentBullet;
    public List<Bullet> allActive = new List<Bullet>();

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        cursor.ready = currentState == State.Aiming;
        if (currentState == State.Aiming)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
        }
        else if (currentState == State.Firing)
        {
        }

        HandleCollisions();
    }

    private void Fire()
    {
        StartCoroutine(HandleFiringCoroutine());
    }

    private IEnumerator HandleFiringCoroutine()
    {
        currentState = State.Firing;
        Debug.Log("Shooting!");

        //Spawn a new bullet:
        currentBullet = GameObject.Instantiate(shotFab, shotParent).GetComponent<Bullet>();
        currentBullet.transform.localPosition = Vector3.zero;
        currentBullet.Configure(cursor.transform.localPosition);
        allActive.Add(currentBullet);

        yield return new WaitForSeconds(1f);
        currentState = State.Aiming;
    }

    private void HandleCollisions()
    {
        //This is o(N^2), so don't make too many bullets for the love of god!
        for (int i = 0; i < allActive.Count; i++)
        {
            if (allActive[i].flaggedForRemoval) continue;

            for (int j = 0; j < allActive.Count; j++)
            {
                if (i == j) continue;
                if (allActive[j].flaggedForRemoval == true) continue;

                Rect rect1 = new Rect(allActive[i].rect.localPosition.x, allActive[i].rect.localPosition.y, allActive[i].rect.sizeDelta.x, allActive[i].rect.sizeDelta.y);
                Rect rect2 = new Rect(allActive[j].rect.localPosition.x, allActive[j].rect.localPosition.y, allActive[j].rect.sizeDelta.x, allActive[j].rect.sizeDelta.y);

                if (rect1.Overlaps(rect2))
                {
                    allActive[j].flaggedForRemoval = true;
                    allActive[i].Combine(allActive[j]);
                    break;
                }
            }
        }

        for (int i = 0; i < allActive.Count; i++)
        {
            if (allActive[i].flaggedForRemoval)
            {
                GameObject.Destroy(allActive[i].gameObject);
                allActive.RemoveAt(i);
            }
        }
    }
}
