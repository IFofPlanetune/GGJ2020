using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Light : MonoBehaviour
{
    private bool on;
    public Color on_color;
    public Sprite offSprite;
    public Sprite brokenSprite;
    public Sprite emptySprite;
    public LightStatus status;
    public GlobalController controller;

    public enum LightStatus
    {
        none,working,broken
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool SetLight(bool b)
    {
        on = b;
        if (status == LightStatus.working)
        {
            if (b)
            {
                this.GetComponent<SpriteRenderer>().color = on_color;
            }
            else
            {
                this.GetComponent<SpriteRenderer>().color = new Vector4(255,255,255,1);
            }
            return true;
        }
        if (status == LightStatus.none)
        {
            if (b)
            {
                return false;
            }
            return true;
        }
        return true;
    }

    public void SwitchStatus(LightStatus s)
    {
        status = s;
        switch (s) {
            case LightStatus.broken:
                this.GetComponent<SpriteRenderer>().sprite = brokenSprite;
                break;
            case LightStatus.none:
                this.GetComponent<SpriteRenderer>().sprite = emptySprite;
                break;
            case LightStatus.working:
                this.GetComponent<SpriteRenderer>().sprite = offSprite;
                break;
            default:
                break;
        }
        SetLight(on);
    }

    private void OnMouseDown()
    {
        if (controller)
        {
            if (controller.reset)
                return;
        }

        switch (status)
        {
            case LightStatus.broken:
                clickBroken();
                break;
            case LightStatus.none:
                clickNone();
                break;
            case LightStatus.working:
                clickWorking();
                break;
            default:
                break;
        }
    }

    void clickBroken()
    {
        if(controller.selected_tool == Tool.ToolType.hand)
            SwitchStatus(LightStatus.none);
    }

    void clickWorking()
    {
        if(on)
        {
            controller.TakeDamage();
        }
    }

    void clickNone()
    {
        SwitchStatus(LightStatus.working);
    }
}
