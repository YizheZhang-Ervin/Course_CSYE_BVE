using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public int targetScene;

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag=="Player"){
            // SceneManager.LoadSceneAsync(targetScene);
            Debug.Log("Not Open");
        }
    }
}
