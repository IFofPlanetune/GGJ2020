using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    // Start is called before the first frame update
    public GlobalController controller;
    public bool on;
    public Sprite on_sprite;
    public Sprite off_sprite;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (controller.selected_tool == Tool.ToolType.hand)
        {
            on = !on;
            if (on)
                GetComponent<SpriteRenderer>().sprite = on_sprite;
            else
                GetComponent<SpriteRenderer>().sprite = off_sprite;
        }
    }
}
