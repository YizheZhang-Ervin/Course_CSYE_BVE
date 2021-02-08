using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseFall : MonoBehaviour
{
    // decide move up or down
    private bool flag;
    // decide move speed
    public float speed = 1;

    void Start(){
        // true = move up
        flag = true;
    }

    void Update(){
        // check if higher than 2.2, it mean object need to move down
        if(transform.position.y>=2.2){
            flag = false;
        // check if lower than 1.1, it mean object need to move up
        }else if(transform.position.y<=1.1){
            flag = true;
        }
        // change position
        transform.position += (flag?Vector3.up:Vector3.down) * Time.deltaTime * speed;
    }

}