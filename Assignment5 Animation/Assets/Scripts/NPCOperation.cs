using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCOperation : MonoBehaviour
{
    public GameObject handLight;
    public string dialogContent;

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag=="Player"){
            handLight.SetActive(true);
            Dialog._instance.ShowDialog(dialogContent);
        }
    }
    private void OnTriggerExit2D(Collider2D collision){
        if(collision.tag=="Player"){
            handLight.SetActive(false);
            Dialog._instance.HideDialog();
        }
    }
}
