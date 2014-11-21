using UnityEngine;
using System.Collections;

public class WorldSoundEffect : MonoBehaviour
{
    private AudioSource source;
    
    
    public void Start()
    {
        source = GetComponent<AudioSource>();
        source.Play();
    }
    public void Update()
    {
        if (!source.isPlaying)
        {
            GameObject.Destroy(this);
        }
    }
}
