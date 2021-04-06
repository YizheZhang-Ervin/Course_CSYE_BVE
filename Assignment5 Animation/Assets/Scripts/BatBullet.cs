using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBullet : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    public float destroyTime = 2f;
    Damage damage;

    private void Awake(){
        rb = transform.GetComponent<Rigidbody2D>();
        sr = transform.GetComponent<SpriteRenderer>();
        damage = transform.GetComponent<Damage>();
    }

    public void SetSpeed(Vector2 velocity){
        rb.velocity = velocity;
    }


    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag!="Enemy"){
            // damage other
            damage.OnDamage(collision.gameObject);
            rb.velocity = Vector2.zero;
            transform.GetComponent<Collider2D>().enabled = false;
             // destroy
            Destroy(gameObject,destroyTime);
        }
    }
}
