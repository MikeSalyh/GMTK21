using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public GameObject shotFab, boomFab;
    public Transform shotParent;

    private Bullet currentBullet;
    public List<Bullet> allActive = new List<Bullet>();
    public bool nextIsMatter = true;
    public TextMeshProUGUI scoreTxt;

    public AudioClip shoot0, shoot1, combine, destroy;
    private AudioSource src;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        cursor.colorMode = Color.white;
        src = GetComponent<AudioSource>();
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

        src.PlayOneShot(nextIsMatter? shoot0 : shoot1);

        //Spawn a new bullet:
        currentBullet = GameObject.Instantiate(shotFab, shotParent).GetComponent<Bullet>();
        currentBullet.transform.localPosition = Vector3.zero;
        currentBullet.Configure(nextIsMatter, cursor.transform.localPosition);
        allActive.Add(currentBullet);


        cursor.colorMode = new Color(0.3207547f, 0.3207547f, 0.3207547f, 1f);
        yield return new WaitForSeconds(1f);

        nextIsMatter = UnityEngine.Random.value > 0.5f;
        cursor.colorMode = nextIsMatter ? Color.white : Color.black;
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
                    if (allActive[i].isMatter == allActive[j].isMatter)
                    {
                        allActive[j].flaggedForRemoval = true;
                        allActive[i].Combine(allActive[j]);
                        src.PlayOneShot(combine);
                        break;
                    }
                    else
                    {
                        allActive[i].flaggedForRemoval = true;
                        allActive[j].flaggedForRemoval = true;
                        GameObject particles = GameObject.Instantiate(boomFab, shotParent);
                        particles.transform.position = allActive[i].transform.position;
                        Destroy(particles, 2f);
                        src.PlayOneShot(destroy);
                        break;
                    }
                }
            }
        }

        int score = 0;
        for (int i = 0; i < allActive.Count; i++)
        {
            if (allActive[i].flaggedForRemoval)
            {
                GameObject.Destroy(allActive[i].gameObject);
                allActive.RemoveAt(i);
            }
            else
            {
                score += allActive[i].value;
            }
        }
        scoreTxt.text = score.ToString();
    }
}
