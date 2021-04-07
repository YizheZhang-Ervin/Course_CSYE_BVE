using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public int targetScene;
    NPCOperation npc2;
    Text txt;

    private void Start(){
         txt = GameObject.Find("keyNum").GetComponent<Text>();
         npc2 = GameObject.Find("NPC2").GetComponent<NPCOperation>();
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag=="Player"){
            if(int.Parse(txt.text)>=npc2.goalNum){
                SceneManager.LoadSceneAsync(targetScene);
            }else{
                Debug.Log("Not Open");
            }
        }
    }
}
