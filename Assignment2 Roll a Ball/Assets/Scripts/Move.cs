using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // decide move forward or back
    private bool flag;
    // decide move speed
    public float speed = 2;

    void Start(){
        // true = move forward
        flag = true;
    }

    void Update(){
        // check if bigger than -5, it mean object need to move back
        if(transform.position.z>=-5){
            flag = false;
        // check if smaller than -7, it mean object need to move forward
        }else if(transform.position.z<=-7){
            flag = true;
        }
        transform.position += (flag?Vector3.forward:Vector3.back) * Time.deltaTime * speed;
    }
}
