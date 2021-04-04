using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwitchStatus{
    Close = 0,
    Open = 1
}

public class Door : MonoBehaviour
{
    public SwitchStatus switchStatus = SwitchStatus.Close;
    public Sprite[] statusSprites;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = transform.GetComponent<SpriteRenderer>();
        sr.sprite = statusSprites[(int)switchStatus];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open(){
        switchStatus = SwitchStatus.Open;
        sr.sprite = statusSprites[(int)switchStatus];
        transform.GetComponent<BoxCollider2D>().enabled = false;
    }
}
