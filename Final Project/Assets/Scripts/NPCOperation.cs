using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCOperation : MonoBehaviour
{
    public GameObject handLight;
    public string dialogContent;
    public int goalNum = 8;
    Text txt;

    private void Start(){
        txt = GameObject.Find("keyNum").GetComponent<Text>();
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag=="Player"){
            handLight.SetActive(true);
            if(int.Parse(txt.text)>=goalNum){
                Dialog._instance.ShowDialog(dialogContent);
            }else{
                Dialog._instance.ShowDialog("Your Coins are not enough, 8 coins are needed for next map!");
            }
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
        if(collision.tag=="Player"){
            handLight.SetActive(false);
            Dialog._instance.HideDialog();
        }
    }
}
