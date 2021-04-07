using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType{
    Attack,Shoot
}

public class PlayerController : MonoBehaviour
{
    public float speedX = 5f;
    public float speedY = 7f;
    public float speedTimes = 7f;
    GameObject playerTrail;
    GameObject wings;
    GameObject shield;
    GameObject fireShield;
    Rigidbody2D rb;
    SpriteRenderer sr;
    SpriteRenderer wingsSR;
    Animator animator;
    float timerY;
    bool isGround;
    bool canJump;
    Damageable playerDamageable;  // can be damage
    public bool canInput = false;
    string resetPos;
    AttackRange attackRange;
    Damage playerDamage;  // attack other objs
    public AttackType attackType;
    Transform bulletPos;
    GameObject lightningPrefab;
    GameObject arrowPrefab;
    float attackTime = 2;  // attack cd time
    bool attackIsReady = true; 
    AudioSource audioSound;

    // Start is called before the first frame update
    void Start(){
        rb = transform.GetComponent<Rigidbody2D>();
        sr = transform.GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();
        playerTrail = transform.Find("Trail").gameObject;
        wings = transform.Find("Wings").gameObject;
        shield = transform.Find("Shield").gameObject;
        fireShield = transform.Find("fireShield").gameObject;
        playerDamageable = transform.GetComponent<Damageable>();
        playerDamageable.OnHurt += this.OnHurt;
        playerDamageable.OnDead += this.OnDead;
        GamePanel._instance.InitHP(playerDamageable.health);
        attackRange = transform.Find("attackRange").GetComponent<AttackRange>();
        bulletPos = transform.Find("bulletPos");
        playerDamage = transform.GetComponent<Damage>();
        audioSound = transform.GetComponent<AudioSource>();
        wingsSR = transform.Find("Wings").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update(){
        if(canInput){
            // left movement & right movement
            SetSpeedX(Input.GetAxisRaw("Horizontal") * speedX);
            
            // jump logic
            checkCanJump();

            // set jump animator
            checkGroud();

            // check attack
            checkAttack();
        }
    }

    // speed horizontal
    public void SetSpeedX(float x){
        x = sprint(x);

        // set animation
        animator.SetBool("IsRun",x>speedX||x<-speedX);
        shield.SetActive(x>speedX||x<-speedX);

        animator.SetBool("IsWalk",x<=speedX&&x>=-speedX&&x!=0);
        
        // check flip
        if(x<0){
            sr.flipX = true;
            attackRange.transform.localPosition = new Vector3(-4.8f,attackRange.transform.localPosition.y,0);
        }else if(x>0){
            sr.flipX = false;
            attackRange.transform.localPosition = new Vector3(4.8f,attackRange.transform.localPosition.y,0);
        }
        rb.velocity = new Vector2(x,rb.velocity.y);
    }

    // speed vertical
    public void SetSpeedY(float y){
        rb.velocity = new Vector2(rb.velocity.x,y);
    }

    public float sprint(float x){
        // check Sprint
        if(Input.GetButton("Sprint")){
            x *= speedTimes;
            playerTrail.SetActive(true);
            fireShield.SetActive(true);
        }else{
            playerTrail.SetActive(false);
            fireShield.SetActive(false);
        }
        return x;
    }

    public void checkGroud(){
        RaycastHit2D rh = Physics2D.Raycast(transform.position,Vector3.down,1.5f,1<<8);
        // Debug.DrawLine(transform.position,transform.position+Vector3.down*1.5f,Color.red);
        isGround = rh;
        animator.SetBool("IsJump",!isGround);
        wings.SetActive(!isGround);
        wingsSR.flipX = sr.flipX;
    }

    public void checkCanJump(){
        // Jump & avoid jump several times
        if(Input.GetButtonDown("Jump") && isGround){
            timerY = 0;
            canJump = true;
        }
        // avoid keep press Jump
        if(Input.GetButton("Jump") && canJump){
            timerY +=Time.deltaTime;
            if(timerY<0.2f){
                canJump = true;
            }else{
                canJump = false;
            }
        }
        // release button
        if(Input.GetButtonUp("Jump")){
            canJump = false;
        }

        // check whether can jump
        if(canJump){
            SetSpeedY(speedY);
        }
    }

    public void OnHurt(){
        if(shield.activeSelf){
            return;
        }
        GamePanel._instance.UpdateHP(playerDamageable.health);
        animator.SetTrigger("hurtTrigger");
        // Debug.Log(GamePanel._instance.currentHP);
    }
    public void OnDead(string resetPos){
        if(shield.activeSelf){
            return;
        }
        GamePanel._instance.UpdateHP(playerDamageable.health);
        animator.SetBool("IsDead",true);
        wings.SetActive(false);
        shield.SetActive(false);

        animator.SetTrigger("deadTrigger");
        // set status
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        canInput = false;

        // set reset position
        this.resetPos = resetPos;
        Invoke("ResetDead",1.8f);
    }

    public void ResetDead(){
        // reset status
        animator.SetBool("IsDead",false);
        rb.gravityScale = 1;
        canInput = true;

        // reset HP
        GamePanel._instance.ResetHP();
        playerDamageable.ResetHealth();
        // Resurrection
        transform.position = GameObject.Find(this.resetPos).transform.position;
    }

    void CreateLightning(){
        if(lightningPrefab==null){
            lightningPrefab = Resources.Load<GameObject>("Prefab/Lightning");
        }
        GameObject go =  GameObject.Instantiate(lightningPrefab);
        if(sr.flipX){
            bulletPos.localPosition = new Vector3(-Mathf.Abs(bulletPos.localPosition.x),bulletPos.localPosition.y,bulletPos.localPosition.z);
        }

        go.transform.position = bulletPos.position;
        go.GetComponent<Lightning>().SetDirection(!sr.flipX);
    }

    void CreateArrow(){
        if(arrowPrefab==null){
            arrowPrefab = Resources.Load<GameObject>("Prefab/Arrow");
        }
        GameObject go =  GameObject.Instantiate(arrowPrefab);
        if(sr.flipX){
            bulletPos.localPosition = new Vector3(-Mathf.Abs(bulletPos.localPosition.x),bulletPos.localPosition.y,bulletPos.localPosition.z);
        }

        go.transform.position = bulletPos.position;
        go.GetComponent<Lightning>().SetDirection(!sr.flipX);
    }

    public void checkAttack(){
        if(Input.GetButtonDown("Attack")){
            Attack(AttackType.Attack);
            Invoke("CreateArrow",0.02f);
        }
        if(Input.GetButtonDown("Shoot")){
            Attack(AttackType.Shoot);
            Invoke("CreateLightning",0.02f);
        }
    }

    public void Attack(AttackType at){
        attackType = at;
        animator.SetTrigger("attackTrigger");
        // shoot has CD, attack no CD
        if(at==AttackType.Shoot){
            if(!attackIsReady){
                return;
            }
            attackIsReady = false;
            Invoke("ResetAttackIsReady",attackTime);
        }
    }

    public void ResetAttackIsReady(){
        attackIsReady = true;
    }

    public void AttackDamage(){
        // get all damageable objs
        if(attackType==AttackType.Attack){
            GameObject[] damageables= attackRange.GetDamageableGameObjects();
            if(damageables!=null && damageables.Length!=0){
                playerDamage.OnDamage(damageables);
            }
        }else if(attackType==AttackType.Shoot){

        }

    }

    public void AttackSound(){
        AudioClip ac = Resources.Load<AudioClip>("SoundEffects/electronic_02");
        if(ac==null){
            return;
        }
        audioSound.PlayOneShot(ac);
    }
}

