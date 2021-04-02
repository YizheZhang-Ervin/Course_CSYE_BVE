using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class link : MonoBehaviour
{
    public string url;

    public void OpenURL(){
        if(string.IsNullOrEmpty(url)){
            return;
        }
        Application.OpenURL(url);
    }


}
