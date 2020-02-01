using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Light : MonoBehaviour
{
    // Start is called before the first frame update
    private bool on;
    public Sprite onSprite;
    public Sprite offSprite;

    public GlobalController controller;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLight(bool b)
    {
        on = b;
        if(b)
        {
            this.GetComponent<SpriteRenderer>().sprite = onSprite;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = offSprite;
        }
    }

    private void OnMouseDown()
    {
        if(on)
        {
            controller.TakeDamage();
        }
    }

    
}
