using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public enum ToolType
    {
        minus,plus,hand
    }

    public GlobalController controller;
    public ToolType type;
    public GameObject highlight;

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
        controller.selected_tool = type;
        highlight.transform.position = this.transform.position;
        highlight.SetActive(true);
    }

}
