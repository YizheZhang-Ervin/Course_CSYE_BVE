using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void Click(){
        // reload the game scene
        SceneManager.LoadScene(0);
    }
}
