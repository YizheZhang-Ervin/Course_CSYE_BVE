using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Accelerate : MonoBehaviour
{
    public Text statusText;

    void Start(){
        // define status text to null
        statusText.text = "";
    }

    void OnCollisionStay(Collision collision){
        // check if collision object is player
        if(collision.gameObject.CompareTag("Player")){
            // get rigidbody
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            // get keyboard input
            float moveH = Input.GetAxis("Horizontal");
            float moveV = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveH,0.0f,moveV);
            // add larger force for moving faster
            rb.AddForce(movement * 25);
            // display status text
            statusText.text = "You are accelerated!";
            // disappear status text
            Invoke("changeText",3.0f);
        }
    }

    void changeText(){
        statusText.text = "";
    }
}
