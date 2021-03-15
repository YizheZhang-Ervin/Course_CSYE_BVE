using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Protect : MonoBehaviour
{
    public Text statusText;

    void Start(){
        // define status text to null
        statusText.text = "";
    }

    void OnTriggerStay(Collider collider){
        // check if collision object is player
        if(collider.gameObject.CompareTag("Player")){
            // get rigidbody
            Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
            // right3,up-2,backward-3
            Vector3 movement = new Vector3(3,-2,-3);
            // add a reverse force to prevent flying out of map
            rb.AddForce(movement * 10);
            // display status text
            statusText.text = "You are Protected!";
            // disappear status text
            Invoke("changeText",3.0f);
        }

    }

    void changeText(){
        statusText.text = "";
    }
}
