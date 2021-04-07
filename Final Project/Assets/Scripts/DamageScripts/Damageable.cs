using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Damageable : MonoBehaviour
{
    public int health;
    int defaultHealth;
    public Action OnHurt;
    public Action<string> OnDead;
    
    private void Start(){
        this.defaultHealth = health;
    }

    public void TakeDamage(int damage,string resetPos){
        health--;
        if(health==0){
            if(OnDead!=null){
                OnDead(resetPos);
            }
        }else if(health>0){
            if(OnHurt!=null){
                OnHurt();
            }
        }
    }

    public void ResetHealth(){
        this.health = defaultHealth;
    }
}
