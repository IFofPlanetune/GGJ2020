using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Tool lamp_tool;
    public bool is_left;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        this.GetComponent<AudioSource>().Play();
        if (is_left)
        {
            lamp_tool.ChangeColor(-1);
        }
        else
        {
            lamp_tool.ChangeColor(1);
        }
    }
}
