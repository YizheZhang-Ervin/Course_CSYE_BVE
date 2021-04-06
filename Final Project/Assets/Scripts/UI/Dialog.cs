using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public static Dialog _instance;
    GameObject dialogImg;
    GameObject dialogText;

    private void Awake(){
        _instance = this;
    }

    private void Start(){
        dialogImg = transform.Find("dialogImg").gameObject;
        dialogText = transform.Find("dialogText").gameObject;
        dialogImg.SetActive(false);
        dialogText.SetActive(false);
    }

    public void ShowDialog(string content){
        dialogImg.SetActive(true);
        dialogText.SetActive(true);
        dialogText.GetComponent<Text>().text = content;
    }

    public void HideDialog(){
        dialogImg.SetActive(false);
        dialogText.SetActive(false);
    }
}
