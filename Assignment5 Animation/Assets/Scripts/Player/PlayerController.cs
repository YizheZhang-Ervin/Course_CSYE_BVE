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
    Rigidbody2D rb;
    SpriteRenderer sr;
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
    GameObject bulletPrefab;
    float attackTime = 2;  // attack cd time
    bool attackIsReady = true; 

    // Start is called before the first frame update
    void Start(){
        rb = transform.GetComponent<Rigidbody2D>();
        sr = transform.GetComponent<SpriteRenderer>();
        animator = transform.GetComponent<Animator>();
        playerTrail = transform.Find("Trail").gameObject;
        playerDamageable = transform.GetComponent<Damageable>();
        playerDamageable.OnHurt += this.OnHurt;
        playerDamageable.OnDead += this.OnDead;
        GamePanel._instance.InitHP(playerDamageable.health);
        attackRange = transform.Find("attackRange").GetComponent<AttackRange>();
        bulletPos = transform.Find("bulletPos");
        playerDamage = transform.GetComponent<Damage>();
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
        animator.SetBool("IsRun",x>=speedX||x<=-speedX);
        animator.SetBool("IsWalk",x<speedX&&x>-speedX&&x!=0);

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
        }else{
            playerTrail.SetActive(false);
        }
        return x;
    }

    public void checkGroud(){
        RaycastHit2D rh = Physics2D.Raycast(transform.position,Vector3.down,1.5f,1<<8);
        // Debug.DrawLine(transform.position,transform.position+Vector3.down*1.5f,Color.red);
        isGround = rh;
        animator.SetBool("IsJump",!isGround);
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
        GamePanel._instance.UpdateHP(playerDamageable.health);
        animator.SetTrigger("hurtTrigger");
        // Debug.Log(GamePanel._instance.currentHP);
    }
    public void OnDead(string resetPos){
        GamePanel._instance.UpdateHP(playerDamageable.health);
        animator.SetBool("IsDead",true);
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

    void CreateBullet(){
        if(bulletPrefab==null){
            bulletPrefab = Resources.Load<GameObject>("Prefab/Lightning");
        }
        GameObject go =  GameObject.Instantiate(bulletPrefab);
        if(sr.flipX){
            bulletPos.localPosition = new Vector3(-bulletPos.localPosition.x,bulletPos.localPosition.y,bulletPos.localPosition.z);
        }

        go.transform.position = bulletPos.position;
        go.GetComponent<Lightning>().SetDirection(!sr.flipX);
    }

    public void checkAttack(){
        if(Input.GetButtonDown("Attack")){
            Attack(AttackType.Attack);
        }
        if(Input.GetButtonDown("Shoot")){
            Attack(AttackType.Shoot);
            Invoke("CreateBullet",0.02f);
        }
    }

    public void Attack(AttackType at){
        attackType = at;
        animator.SetTrigger("attackTrigger");
        // shoot has CD, other no CD
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
}

