using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class monsterDragon : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 20f;
    public EnemyStatus es;
    float idleTimer = 0;
    SpriteRenderer sr;
    Animator animator;
    Damage damage;
    Damageable damageable;
    public Transform attackTarget;
    AudioSource audioSound;

    void Start(){
        rb = transform.GetComponent<Rigidbody2D>();
        es = EnemyStatus.Idle;
        sr = transform.GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();
        damage = transform.GetComponent<Damage>();
        attackTarget = GameObject.Find("Player").transform;
        damageable = transform.GetComponent<Damageable>();
        damageable.OnDead += OnDead;
        damageable.OnHurt += OnHurt;
        audioSound = GameObject.Find("Sound").GetComponent<AudioSource>();
    }

    void Update(){
        UpdateStatus();  // update self status
    }

    public void SetSpeedX(float speedX){
        if(speedX>0){
            sr.flipX = false;
        }else if(speedX<0){
            sr.flipX = true;
        }
        // sr.flipX = speedX<0;
        rb.velocity = new Vector2(speedX,rb.velocity.y);
    }

    public void RecoverZeroRotatation(){
        if(transform.position.x>0){
            sr.flipX = true;
        }else{
            sr.flipX = false;
        }
        transform.eulerAngles = new Vector3(0,0,0);
    }

    public void RandomAttack(){
        int action = Random.Range(0,3);
        sr.enabled=true;
        // top->bottom
        if(action==0){
            es = EnemyStatus.Idle;
            int pos = Random.Range(0,3);
            if(!sr.flipX){
                transform.eulerAngles = new Vector3(0,0,-90);
            }else{
                transform.eulerAngles = new Vector3(0,0,90);
            }
            // left
            if(pos==0){
                transform.position = new Vector3(-7,6,0);
                rb.velocity = new Vector2(0,-speed);
            // middle
            }else if(pos==1){
                transform.position = new Vector3(0,6,0);
                rb.velocity = new Vector2(0,-speed);
            // right
            }else if(pos==2){
                transform.position = new Vector3(7,6,0);
                rb.velocity = new Vector2(0,-speed);
            }
            Invoke("RecoverZeroRotatation",0.5f);
            
        // left<-->right
        }else if(action==1){
            es = EnemyStatus.Walk;
            int pos = Random.Range(0,2);
            transform.eulerAngles = new Vector3(0,0,0);
            // left->right
            if(pos==0){
                transform.position = new Vector3(-10,-3,0);
                SetSpeedX(speed);
            // right->left
            }else if(pos==1){
                transform.position = new Vector3(10,-3,0);
                SetSpeedX(-speed);
            }
        // boomerage
        }else if(action==2){
            es = EnemyStatus.Attack;
            transform.eulerAngles = new Vector3(0,0,0);
            int pos = Random.Range(0,2);
            // left
            if(pos==0){
                transform.position = new Vector3(-16,4,0);
                sr.flipX = false;

            // right
            }else if(pos==1){
                transform.position = new Vector3(16,4,0);
                sr.flipX = true;
            }
        }
    }

    public void UpdateStatus(){
        switch(es){
            case EnemyStatus.Idle:
                idleTimer+=Time.deltaTime;
                if(idleTimer>2){
                    idleTimer = 0;
                    RandomAttack();
                }
                break;
            case EnemyStatus.Walk:
                idleTimer+=Time.deltaTime;
                if(idleTimer>2){
                    idleTimer = 0;
                    RandomAttack();
                }
                animator.SetBool("isWalk",true);
                break;
            case EnemyStatus.Attack:
                idleTimer+=Time.deltaTime;
                if(idleTimer>2){
                    idleTimer = 0;
                    RandomAttack();
                }
                animator.SetBool("isAttack",true);
                break;
            case EnemyStatus.Dead:
                animator.SetBool("isDead",true);
                break;
            case EnemyStatus.Hurt:
                es = EnemyStatus.Idle;
                break;
        }
        if(es!=EnemyStatus.Walk){
            animator.SetBool("isWalk",false);
        }
        if(es!=EnemyStatus.Attack){
            animator.SetBool("isAttack",false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        // if(collision.collider.tag=="Player"){
        //     damage.OnDamage(collision.gameObject);
        // }
    }

    public void PutDamage(){
        damage.OnDamage(attackTarget.gameObject);
        es = EnemyStatus.Idle;
        AudioClip ac = Resources.Load<AudioClip>("SoundEffects/monsterSounds/wolf2atk");
        audioSound.PlayOneShot(ac);
    }

    public void OnHurt(){
        SetSpeedX(0);
        es = EnemyStatus.Hurt;
        animator.SetTrigger("hurtTrigger");
    }

    public void OnDead(string resetPos){
        SetSpeedX(0);
        es = EnemyStatus.Dead;
        animator.SetTrigger("deadTrigger");
        transform.GetComponent<BoxCollider2D>().enabled = false;
        rb.gravityScale = 0;

        Destroy(gameObject,2f);

    }
}
