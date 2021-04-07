using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Menu : MonoBehaviour
{
    public GameObject obj;
    public PlayerController pc;

    public void OnStartGameClick(){
        obj.SetActive(false);
        pc.canInput = true;
    }

    public void OnExitClick(){
        // if(Application.isEditor){
        //     EditorApplication.isPlaying = false;
        // }else{
        //     Application.Quit();
        // }
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    
    }


}
