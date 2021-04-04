using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class key : MonoBehaviour
{
    
    SpriteRenderer sr;
    public Door door;
    // Start is called before the first frame update
    void Start()
    {
        sr = transform.GetComponent<SpriteRenderer>();
        door = GameObject.Find("Door1").GetComponent<Door>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag=="Player"){
            sr.sprite = null;
            // close check
            transform.GetComponent<BoxCollider2D>().enabled = false;
            // open door
            if(door!=null){
                door.Open();
            }
        }
    }
}
