using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerDamage : MonoBehaviour
{
    Damage damage;
    // Start is called before the first frame update
    void Start()
    {
        damage = transform.GetComponent<Damage>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision){
        damage.OnDamage(collision.gameObject);
    }
}
