using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource audiosource_musicadefundo;

    public AudioClip[] musicasdefundo; 

    void Start()
    {
        if (musicasdefundo.Length > 0)
        {
            AudioClip musicadefundo = musicasdefundo[0];
            audiosource_musicadefundo.clip = musicadefundo;

            audiosource_musicadefundo.Play();
        }
        else
        {
        }
    }

    void Update()
    {
        
    }
}
