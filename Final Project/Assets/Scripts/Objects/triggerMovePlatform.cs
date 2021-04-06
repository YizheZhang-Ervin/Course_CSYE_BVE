using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerMovePlatform : MonoBehaviour
{
    public GameObject moveplatform;
    // Start is called before the first frame update
    void Awake()
    {
        moveplatform = GameObject.Find("movePlatformH");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag=="Box"){
            moveplatform.GetComponent<movePlatform>().triggerMove = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if(collision.tag=="Box"){
            moveplatform.GetComponent<movePlatform>().triggerMove = false;
        }
    }
}
