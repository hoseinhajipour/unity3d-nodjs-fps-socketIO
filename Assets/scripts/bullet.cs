using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioClip hit_sound;
    public AudioSource audio;
    void Start()
    {
        audio = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        audio.PlayOneShot(hit_sound);
        Destroy(gameObject,0.5f);
    }

}
