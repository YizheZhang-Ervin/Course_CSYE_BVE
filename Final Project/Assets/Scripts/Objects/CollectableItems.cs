using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableItems : MonoBehaviour
{
    GameObject bomb;
    SpriteRenderer sr;
    AudioSource audioSound;
    Text txt;

    // Start is called before the first frame update
    void Start()
    {   
        bomb = transform.Find("bomb").gameObject;
        sr = transform.GetComponent<SpriteRenderer>();
        audioSound = GameObject.Find("Sound").GetComponent<AudioSource>();
        txt = GameObject.Find("keyNum").GetComponent<Text>();
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
            
            // change coin quantity
            txt.text = (int.Parse(txt.text)+1).ToString();

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
