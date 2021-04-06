using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlatform : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;
    bool isMoveToEnd = false;
    public float speed = 2f;
    Rigidbody2D rb;
    public ContactFilter2D cf;
    ContactPoint2D[] cp = new ContactPoint2D[10];
    public bool triggerMove = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    public void FollowObjects(){
        if(!triggerMove){
            return;
        }
        int count = rb.GetContacts(cf,cp);
        for(int i=0;i<count;i++){
            cp[i].rigidbody.velocity += new Vector2(isMoveToEnd?-speed:speed,0);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(triggerMove){
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

    void LateUpdate(){
        FollowObjects();
    }
}
