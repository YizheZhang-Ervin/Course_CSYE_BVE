using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class key : MonoBehaviour
{
    
    SpriteRenderer sr;
    GameObject doorObj;
    GameObject doorObj2;
    GameObject player;
    Door door;
    Door door2;
    GameObject cinemachine;

    // Start is called before the first frame update
    void Start()
    {
        sr = transform.GetComponent<SpriteRenderer>();
        doorObj = GameObject.Find("Door1");
        doorObj2 = GameObject.Find("Door2");
        player = GameObject.Find("Player");
        door = doorObj.GetComponent<Door>();
        door2 = doorObj2.GetComponent<Door>();
        cinemachine = GameObject.Find("cinemachine");
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
            if(doorObj.transform.GetComponent<BoxCollider2D>().enabled){
                cinemachine.GetComponent<CinemachineVirtualCamera>().Follow = doorObj.transform;
                Invoke("SetCameraBackToPlayer",2);
                door.Open();
            }else{
                cinemachine.GetComponent<CinemachineVirtualCamera>().Follow = doorObj2.transform;
                Invoke("SetCameraBackToPlayer",2);
                door2.Open();
            }
        }
    }

    public void SetCameraBackToPlayer(){
        cinemachine.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
    }
}
