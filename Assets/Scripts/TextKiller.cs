using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextKiller : MonoBehaviour
{
    public AudioClip musicClip;
    public AudioSource musicSource;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2);

        musicSource = GetComponent<AudioSource>();
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
