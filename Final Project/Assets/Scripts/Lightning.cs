using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    public float speed = 15f;
    public float destroyTime = 2f;
    Damage damage;

    private void Awake(){
        rb = transform.GetComponent<Rigidbody2D>();
        sr = transform.GetComponent<SpriteRenderer>();
        damage = transform.GetComponent<Damage>();
    }

    public void SetDirection(bool isRight){
        sr.flipX = !isRight;
        rb.velocity = new Vector2(isRight?speed:-speed,0);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag!="Player"){
            // damage other
            damage.OnDamage(collision.gameObject);
             // destroy
            Destroy(gameObject,destroyTime);
        }
    }
}
