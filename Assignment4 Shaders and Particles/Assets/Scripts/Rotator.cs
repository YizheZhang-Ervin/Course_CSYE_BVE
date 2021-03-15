using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    void Update()
    {
        // check if current object is Pick Up
        if(CompareTag("Pick Up")){
            // rotate
            transform.Rotate(new Vector3(15,30,45) * Time.deltaTime);
        // check if current object is Barrier
        }else if(CompareTag("Barrier")){
            // rotate different angular and faster than Pick Up
            // deltaTime: time when finished last frame
            transform.Rotate(new Vector3(0,30,0) * Time.deltaTime * 10);
        }
        
    }
}
