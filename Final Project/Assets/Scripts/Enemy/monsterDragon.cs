using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;
using UnityEngine.UI;

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
    // public Transform attackTarget;
    AudioSource audioSound;
    GameObject circle;
    GameObject circleTrail;
    GameObject boomeragePrefab;
    Transform bulletPos;
    GameObject cinemachine;
    GameObject npc;
    Text txt;
    Material material;
	float fade = 1f;

    void Start(){
        rb = transform.GetComponent<Rigidbody2D>();
        es = EnemyStatus.Idle;
        sr = transform.GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();
        damage = transform.GetComponent<Damage>();
        circle = transform.Find("Circle").gameObject;
        circleTrail = transform.Find("CircleTrail").gameObject;
        // attackTarget = GameObject.Find("Player").transform;
        damageable = transform.GetComponent<Damageable>();
        damageable.OnDead += OnDead;
        damageable.OnHurt += OnHurt;
        audioSound = GameObject.Find("Sound").GetComponent<AudioSource>();
        bulletPos = transform.Find("BulletPos");
        cinemachine = GameObject.Find("CineMachine");
        npc = GameObject.Find("NPC");
        txt = GameObject.Find("keyNum").GetComponent<Text>();
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update(){
        UpdateStatus();  // update self status
    }

    public void SetSpeedX(float speedX){
        if(speedX>0){
            FlipSprite(false);
        }else if(speedX<0){
            FlipSprite(true);
        }

        rb.velocity = new Vector2(speedX,rb.velocity.y);
    }

    public void SetSpeedY(float speedY){
        rb.velocity = new Vector2(rb.velocity.x,speedY);
    }

    public void RecoverZeroRotatation(){
        if(transform.position.x>0){
            sr.flipX = true;
        }else{
            sr.flipX = false;
        }
        transform.eulerAngles = new Vector3(0,0,0);
    }

    public void SetGravity(int gravityNum){
        rb.gravityScale = gravityNum;
    }

    public void FlipSprite(bool boolVal){
        sr.flipX = boolVal;
    }

    public void SetPosition(Vector3 vector){
        transform.position = vector;
    }

    public void SetRotation(Vector3 vector){
        transform.eulerAngles = vector;
    }

    public void PlaySoundEffect(string soundStr){
        AudioClip ac = Resources.Load<AudioClip>(soundStr);
        audioSound.PlayOneShot(ac);
    }

    public void SetShieldCircleStaus(bool boolVal){
        circle.SetActive(boolVal);
        circleTrail.SetActive(boolVal);
    }

    public int RandomRange(int num){
        return Random.Range(0,num);
    }

    public void TriggerStatusChange(){
        idleTimer+=Time.deltaTime;
        if(idleTimer>2){
            idleTimer = 0;
            RandomAttack();
        }
    }

    void CreateBoomerage(){
        if(boomeragePrefab==null){
            boomeragePrefab = Resources.Load<GameObject>("Prefab/Boomerage");
        }
        GameObject go =  GameObject.Instantiate(boomeragePrefab);
        if(sr.flipX){
            bulletPos.localPosition = new Vector3(-Mathf.Abs(bulletPos.localPosition.x),bulletPos.localPosition.y,bulletPos.localPosition.z);
        }

        go.transform.position = bulletPos.position;
        go.GetComponent<boomerage>().SetDirection(!sr.flipX);
    }

    public void RandomAttack(){
        int action = RandomRange(3);
        SetGravity(1);
        RecoverZeroRotatation();
        SetShieldCircleStaus(false);
        // top->bottom
        if(action==0){
            es = EnemyStatus.Idle;
            PlaySoundEffect("SoundEffects/monsterSounds/wolf2");
            int pos = RandomRange(3);
            SetShieldCircleStaus(true);

            if(!sr.flipX){
                SetRotation(new Vector3(0,0,-90));
            }else{
                SetRotation(new Vector3(0,0,90));
            }
            SetSpeedY(-speed);
            // left
            if(pos==0){
                SetPosition(new Vector3(-7,6,0));
            // middle
            }else if(pos==1){
                SetPosition(new Vector3(0,6,0));
            // right
            }else if(pos==2){
                SetPosition(new Vector3(7,6,0));
            }
            Invoke("RecoverZeroRotatation",0.5f);
            
        // left<-->right
        }else if(action==1){
            es = EnemyStatus.Walk;
            PlaySoundEffect("SoundEffects/monsterSounds/wolf2");
            int pos = RandomRange(2);
            SetShieldCircleStaus(true);
            
            // left->right
            if(pos==0){
                SetPosition(new Vector3(-10,-3,0));
                SetSpeedX(speed);
            // right->left
            }else if(pos==1){
                SetPosition(new Vector3(10,-3,0));
                SetSpeedX(-speed);
            }
        // boomerage
        }else if(action==2){
            SetGravity(0);
            es = EnemyStatus.Attack;
            PlaySoundEffect("SoundEffects/monsterSounds/wolf2atk");
            int pos = RandomRange(2);

            // left
            if(pos==0){
                SetPosition(new Vector3(-16,3,0));
                FlipSprite(false);
            // right
            }else if(pos==1){
                SetPosition(new Vector3(16,3,0));
                FlipSprite(true);
            }
            CreateBoomerage();
        }
    }

    public void UpdateStatus(){
        switch(es){
            case EnemyStatus.Idle:
                TriggerStatusChange();
                break;
            case EnemyStatus.Walk:
                TriggerStatusChange();
                animator.SetBool("isWalk",true);
                break;
            case EnemyStatus.Attack:
                TriggerStatusChange();
                animator.SetBool("isAttack",true);
                break;
            case EnemyStatus.Dead:
                animator.SetBool("isDead",true);
                fade -= Time.deltaTime;
                if(fade<=0f){
                    fade = 0f;
                }
                material.SetFloat("_Fade",fade);
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
        if(collision.collider.tag=="Player"){
            damage.OnDamage(collision.gameObject);
        }
    }

    public void PutDamage(){
        // damage.OnDamage(attackTarget.gameObject);
        // es = EnemyStatus.Idle;
    }

    public void OnHurt(){
        SetSpeedX(0);
        es = EnemyStatus.Hurt;
        animator.SetTrigger("hurtTrigger");
        PlaySoundEffect("SoundEffects/monsterSounds/wolf2die");
    }

    public void OnDead(string resetPos){
        SetSpeedX(0);
        es = EnemyStatus.Dead;
        animator.SetTrigger("deadTrigger");
        transform.GetComponent<BoxCollider2D>().enabled = false;
        SetGravity(0);
        PlaySoundEffect("SoundEffects/monsterSounds/wolf2die");
        txt.text = (int.Parse(txt.text)+1).ToString();
        ShowDialog();
        Destroy(gameObject,2f);

    }

    public void ShowDialog(){
        string leftNum = (npc.GetComponent<NPCOperationL2>().goalNum - int.Parse(txt.text)).ToString();
        string dialogContent = "Good Job, Yong man!You have "+ leftNum +" monsters left!";
        Dialog._instance.ShowDialog(dialogContent);
        Invoke("DisappearDialog",1);
    }

    public void DisappearDialog(){
        Dialog._instance.HideDialog();
    }
}
