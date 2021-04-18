using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// public enum EnemyStatus{
//     Idle,
//     Attack,
//     Dead
// }

public class Bat : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    Damage damage;
    Damageable damageable;
    public EnemyStatus es;
    public float attackRange = 2;
    public float listenRange = 2;
    public Transform attackTarget;
    GameObject bulletPrefab;
    public Transform bulletPos;
    GameObject coinItem;
    AudioSource audioSound;
    Material material;
	float fade = 1f;

    void Start(){
        rb = transform.GetComponent<Rigidbody2D>();
        es = EnemyStatus.Idle;
        sr = transform.GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();
        damage = transform.GetComponent<Damage>();
        attackTarget = GameObject.Find("Player").transform;
        damageable = transform.GetComponent<Damageable>();
        damageable.OnDead += OnDead;
        audioSound = GameObject.Find("Sound").GetComponent<AudioSource>();
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update(){
        UpdateStatus();  // update self status
        UpdateListener();  // listen player
        UpdateDirection();
        // CheckCanAttack();  // not attack when above it
        
    }

    public void CheckCanAttack(){
        if(es == EnemyStatus.Attack){
            if(attackTarget.position.y>transform.position.y + 2){
                es = EnemyStatus.Idle;
            }
        }
    }

    public void UpdateStatus(){
        switch(es){
            case EnemyStatus.Idle:
                break;
            case EnemyStatus.Attack:
                animator.SetBool("isAttack",true);
                break;
            case EnemyStatus.Dead:
                animator.SetBool("isDead",true);
                fade -= Time.deltaTime;
                if(fade<=0f){
                    fade = 0f;
                }
                material.SetFloat("_Fade",fade);
                Debug.Log(fade);
                break;
        }
        if(es!=EnemyStatus.Attack){
            animator.SetBool("isAttack",false);
        }
    }

    public void UpdateDirection(){
        if(attackTarget.position.x - transform.position.x>0){
            sr.flipX = true;
        }else if(attackTarget.position.x - transform.position.x<0){
            sr.flipX = false;
        }
    }

    public void UpdateListener(){
        if(es == EnemyStatus.Dead){
            return;
        }
        if(attackTarget==null){
            return;
        }

        // find player
        if(Vector3.Distance(transform.position,attackTarget.position)<=attackRange){
            es = EnemyStatus.Attack;
            return;
        }
        if(Vector3.Distance(transform.position,attackTarget.position)<=listenRange){
            // run to player
            es = EnemyStatus.Run;
        }else{
            es = EnemyStatus.Idle;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision){
        // if(collision.collider.tag=="Player"){
        //     damage.OnDamage(collision.gameObject);
        // }
    }



    private void OnDrawGizmosSelected(){
        // Handles.color = new Color(Color.red.r,Color.red.g,Color.red.b,0.2f);
        // Handles.DrawSolidDisc(transform.position,Vector3.forward,attackRange);
        // Handles.color = new Color(Color.green.r,Color.green.g,Color.green.b,0.2f);
        // Handles.DrawSolidDisc(transform.position,Vector3.forward,listenRange);
    }

    public void PutDamage(){
        if(bulletPrefab==null){
            bulletPrefab = Resources.Load<GameObject>("Prefab/BatBullet");
        }
        GameObject bullet = GameObject.Instantiate(bulletPrefab);
        bullet.transform.position = bulletPos.position;
        
        AudioClip ac = Resources.Load<AudioClip>("SoundEffects/laser_02");
        audioSound.PlayOneShot(ac);

        float g = Mathf.Abs(Physics2D.gravity.y) * bullet.transform.GetComponent<Rigidbody2D>().gravityScale;
        float v0 = 8; // vertical speed
        float t0 = v0 / g;
        float y0 = 0.5f*g*t0*t0;
        float v = 0;
        // bullet velocity
        float x = attackTarget.position.x - transform.position.x + Random.Range(-1.5f,1.5f);

        if(transform.position.y+y0 > attackTarget.position.y){
            float y = Mathf.Abs(transform.position.y - attackTarget.position.y) + y0;
            float t = Mathf.Sqrt((y*2) /g) + t0;
            v = x/t;
        }else if(transform.position.y+y0 < attackTarget.position.y){
            float y = Mathf.Abs(transform.position.y - attackTarget.position.y);
            float t = Mathf.Sqrt((y*2) /g);
            v0 = g*t;
            v = x/t;
        }

        bullet.GetComponent<BatBullet>().SetSpeed(new Vector2(v,v0));
    }

    public void OnDead(string resetPos){
        es = EnemyStatus.Dead;
        transform.GetComponent<BoxCollider2D>().enabled = false;
        rb.gravityScale = 0;
    
        // drop coins
        coinItem = Resources.Load<GameObject>("Prefab/Coin");
        GameObject coin =  GameObject.Instantiate(coinItem);
        coin.transform.position = transform.position;

        Destroy(gameObject,1f);
    }

}
