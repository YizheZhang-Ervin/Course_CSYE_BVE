using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCOperationL2 : MonoBehaviour
{
    public GameObject handLight;
    public string dialogContent;
    public int goalNum = 3;
    Text txt;
    GameObject dialogPanel;

    private void Start(){
        txt = GameObject.Find("keyNum").GetComponent<Text>();
        dialogPanel = GameObject.Find("DialogPanel");
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag=="Player"){
            handLight.SetActive(true);
            if(int.Parse(txt.text)>=goalNum){
                Dialog._instance.ShowDialog(dialogContent);
                dialogPanel.transform.Find("ExitBtn").gameObject.SetActive(true);
            }else{
                Dialog._instance.ShowDialog("Your have to defeat the monsters first!");
            }
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
        if(collision.tag=="Player"){
            handLight.SetActive(false);
            Dialog._instance.HideDialog();
            dialogPanel.transform.Find("ExitBtn").gameObject.SetActive(false);
        }
    }
}
