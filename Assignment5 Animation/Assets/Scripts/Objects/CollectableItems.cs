using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItems : MonoBehaviour
{
    GameObject bomb;
    SpriteRenderer sr;
    AudioSource audioSound;

    // Start is called before the first frame update
    void Start()
    {   
        bomb = transform.Find("bomb").gameObject;
        sr = transform.GetComponent<SpriteRenderer>();
        audioSound = GameObject.Find("Sound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag=="Player"){
            // close check
            transform.GetComponent<BoxCollider2D>().enabled = false;
            sr.enabled = false;
            
            // sound
            AudioClip ac = Resources.Load<AudioClip>("SoundEffects/magic_01");
            if(ac==null){
                return;
            }   
            audioSound.PlayOneShot(ac);

            // bomb
            bomb.SetActive(true);
            // GameObject.Destroy(bomb,1f);
            GameObject.Destroy(gameObject,1f);
        }
    }
}
