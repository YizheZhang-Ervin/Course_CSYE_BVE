using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damage;
    public string resetPos;

    public void OnDamage(GameObject gameObject){
        Damageable damageable = gameObject.GetComponent<Damageable>();
        if(damageable==null){
            return;
        }
        damageable.TakeDamage(damage,resetPos);
    }

    public void OnDamage(GameObject[] gameObjects){
        for(int i=0;i<gameObjects.Length;i++){
            OnDamage(gameObjects[i]);
        }
    }
}
