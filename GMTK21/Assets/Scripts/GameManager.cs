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

    // Start is called before the first frame update
    void Start()
    {
        
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

        yield return new WaitForSeconds(1f);
        currentState = State.Aiming;
    }
}
