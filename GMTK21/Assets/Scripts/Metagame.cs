using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Metagame : MonoBehaviour
{
    private AudioSource src;
    public AudioClip click;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        src = GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        src.PlayOneShot(click);
        SceneManager.LoadScene("Gameplay");
    }
}
