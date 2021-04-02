using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : MonoBehaviour
{
    public static GamePanel _instance;
    public GameObject hp_item_prefab;
    Transform hp_parent;
    public GameObject[] hp_items;
    public int currentHP;

    private void Awake(){
        _instance = this;
    }

    public void Start(){
        hp_parent = transform.Find("HP");
    }

    public void ResetHP(){
        for(int i=0;i<hp_items.Length;i++){
            hp_items[i].SetActive(true);
        }
    }

    public void InitHP(int hp){
        currentHP = hp;
        hp_items = new GameObject[hp];
        for(int i=0;i<hp;i++){
            hp_items[i] = GameObject.Instantiate(hp_item_prefab,hp_parent);   
        }
    }
    public void UpdateHP(int hp){
        if(hp<0){
            return;
        }
        if(currentHP>hp){
            for(int i=hp;i<hp_items.Length;i++){
                if(hp_items[i].activeSelf){
                    hp_items[i].SetActive(false);
                }
            }
        }else{
            for(int i=hp;i<hp_items.Length;i++){
                hp_items[i].SetActive(true);
            }
        }
        currentHP = hp;
    }
}
