using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public enum ToolType
    {
        minus,plus,hand,lamp
    }

    public GlobalController controller;
    public SpriteRenderer light;
    public ToolType type;
    public GameObject highlight;
    public Light.LightColors color;
    public List<Light.LightColors> color_list;
    private int list_id;

    // Start is called before the first frame update
    void Start()
    {
        color_list = new List<Light.LightColors>();
        color_list.Add(Light.LightColors.yellow);
        color_list.Add(Light.LightColors.red);
        list_id = 0;
        color = color_list[list_id];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        this.GetComponent<AudioSource>().Play();
        controller.selected_tool = type;
        controller.tool_color = color;
        highlight.transform.position = this.transform.position;
    }

    public void ChangeColor(int i)
    {
        list_id += 1;
        if(list_id >= color_list.Count)
        {
            list_id -= color_list.Count;
        }
        if(list_id < 0)
        {
            list_id = color_list.Count - list_id;
        }
        color = color_list[list_id];
        light.color = Light.getColorFromType(color);
    }

}
