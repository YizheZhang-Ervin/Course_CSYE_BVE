using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveObj : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float speed;
    bool isMoveToEnd = true;
    Rigidbody2D rb;
    public ContactFilter2D contactFilter;
    // save other objects rigidbody
    ContactPoint2D[] contactPoint = new ContactPoint2D[10];

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    // obj follow paltform
    public void FollowObjects(){
        int count = rb.GetContacts(contactFilter,contactPoint);
        for(int i=0;i<count;i++){
            contactPoint[i].rigidbody.velocity += new Vector2(isMoveToEnd?speed:-speed,0);
        }
    }

    void LateUpdate(){
        FollowObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoveToEnd){
            transform.position = Vector3.MoveTowards(transform.position,endPos,speed*Time.deltaTime);
            if(transform.position==endPos){
                isMoveToEnd = false;
            }
        }else{
            transform.position = Vector3.MoveTowards(transform.position,startPos,speed*Time.deltaTime);
            if(transform.position==startPos){
                isMoveToEnd = true;
            }
        }
    }
}
