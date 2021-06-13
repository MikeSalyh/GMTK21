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

    public State currentState = State.Aiming;
    public CursorFollowLauncher cursor;

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
        yield return new WaitForSeconds(1f);
        currentState = State.Aiming;
    }
}
