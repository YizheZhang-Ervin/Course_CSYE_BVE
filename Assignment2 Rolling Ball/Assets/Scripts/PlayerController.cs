using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // total score of all pick ups
    private int totalScore = 8;
    // player move speed
    public float speed = 10;
    // player rigidbody
    private Rigidbody rb;
    // count real time palyer score
    private int count;
    // text for counting score
    public Text countText;
    // text for display win
    public Text winText;
    // text for counting time
    public Text timeText;

    void Start(){
        // initialize rigidbody
        rb = GetComponent<Rigidbody>();
        // initialize score
        count = 0;
        // display score 
        SetCountText();
        // display win 
        winText.text = "";
        // display time
        timeText.text = "";
    }

    void Update(){
        // display real time 
        timeText.text = "Play Time" + Time.realtimeSinceStartup.ToString("0.0") + "s";
    }

    void FixedUpdate(){
        // move player according to input
        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveH,0.0f,moveV);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other){
        // check if collider is Pick Up
        if(other.gameObject.CompareTag("Pick Up")){
            // let Pick Up inactive
            other.gameObject.SetActive(false);
            // score add 1
            count += 1;
            // if it is not the last 4 Pick Ups
            if(count<totalScore-3){
                // touch one Pick Up get bigger
                transform.localScale += new Vector3(0.1f,0.1f,0.1f);
                // touch one Pick Up change color to green
                GetComponent<Renderer>().material.color = Color.green;
            // if it is the last 4 Pick Ups
            }else{
                // touch one Pick Up recover to original size
                transform.localScale = new Vector3(1f,1f,1f);
                // touch one Pick Up recover to original color
                GetComponent<Renderer>().material.color = Color.red;
            }
            // display score
            SetCountText();
            // check if player win
            if(count>=totalScore){
                winText.text = "You win! Total play time:"+ Time.realtimeSinceStartup.ToString("0.0") + "s";
            }
        }
    }

    void SetCountText(){
        // set score
        countText.text = "Count:"+count.ToString();
    }
}
