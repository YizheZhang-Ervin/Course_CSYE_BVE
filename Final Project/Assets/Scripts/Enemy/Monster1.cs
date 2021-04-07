using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum EnemyStatus{
    Idle,
    Walk,
    Run,
    Attack,
    Dead,
    Hurt
}

public class Monster1 : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 5f;
    Transform checkCanMovePoint;
    public bool isCanMove = true;
    public EnemyStatus es;
    float idleTimer = 0;
    SpriteRenderer sr;
    Animator animator;
    Damage damage;
    Damageable damageable;
    public float attackRange = 2;
    public float listenRange = 2;
    public Transform attackTarget;
    GameObject keyItem;
    AudioSource audioSound;

    void Start(){
        rb = transform.GetComponent<Rigidbody2D>();
        checkCanMovePoint = transform.Find("checkCanMovePoint");
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
        CheckCanMove();
        UpdateStatus();  // update self status
        UpdateListener();  // listen player
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

    public void CheckCanMove(){
        RaycastHit2D rh = Physics2D.Raycast(checkCanMovePoint.position,Vector2.down,1,1<<8);
        isCanMove = rh;
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
                if(!isCanMove){
                    speed=-speed;
                    checkCanMovePoint.localPosition = new Vector3(-checkCanMovePoint.localPosition.x,checkCanMovePoint.localPosition.y,checkCanMovePoint.localPosition.z);
                }
                break;
            case EnemyStatus.Run:
                animator.SetBool("isRun",true);
                // run to player
                if(isCanMove){
                    // on the right
                    if(attackTarget.position.x-transform.position.x>0){
                        speed=Mathf.Abs(speed);
                        checkCanMovePoint.localPosition = new Vector3(Mathf.Abs(checkCanMovePoint.localPosition.x),checkCanMovePoint.localPosition.y,checkCanMovePoint.localPosition.z);
                    // on the left
                    }else{
                        speed=-Mathf.Abs(speed);
                        checkCanMovePoint.localPosition = new Vector3(-Mathf.Abs(checkCanMovePoint.localPosition.x),checkCanMovePoint.localPosition.y,checkCanMovePoint.localPosition.z);
                    }
                    SetSpeedX(speed);
                    
                }else{
                    // SetSpeedX(0);
                    es = EnemyStatus.Idle;
                }
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
        if(es!=EnemyStatus.Run){
            animator.SetBool("isRun",false);
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

    public void UpdateListener(){
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
        }
        
    }

    private void OnDrawGizmosSelected(){
        // Handles.color = new Color(Color.red.r,Color.red.g,Color.red.b,0.2f);
        // Handles.DrawSolidDisc(transform.position,Vector3.forward,attackRange);
        // Handles.color = new Color(Color.green.r,Color.green.g,Color.green.b,0.2f);
        // Handles.DrawSolidDisc(transform.position,Vector3.forward,listenRange);
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

        // drop key
        keyItem = Resources.Load<GameObject>("Prefab/Key");
        GameObject key1 =  GameObject.Instantiate(keyItem);
        key1.transform.position = transform.position;

        Destroy(gameObject,2f);

    }
}
