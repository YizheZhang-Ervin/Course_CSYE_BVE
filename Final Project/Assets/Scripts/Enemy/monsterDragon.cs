using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class monsterDragon : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 5f;
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

    public void UpdateStatus(){
        switch(es){
            case EnemyStatus.Idle:
                SetSpeedX(0);
                idleTimer+=Time.deltaTime;
                if(idleTimer>2){
                    idleTimer = 0;
                    es = EnemyStatus.Walk;
                }
                break;
            case EnemyStatus.Walk:
                SetSpeedX(speed);
                animator.SetBool("isWalk",true);
                break;
            case EnemyStatus.Attack:
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
        AudioClip ac = Resources.Load<AudioClip>("SoundEffects/cannon_02");
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
