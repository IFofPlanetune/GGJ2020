using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Light : MonoBehaviour
{
    private bool on;
    public LightColors color;
    public Sprite offSprite;
    public Sprite brokenSprite;
    public Sprite emptySprite;
    public LightStatus status;
    public GlobalController controller;

    public List<Bolt> bolt_list = new List<Bolt>();

    public enum LightStatus
    {
        none,working,broken
    }

    public enum LightColors
    {
        yellow,red
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

        bool bolt_test = true;
        foreach (Bolt bolt in bolt_list)
        {
            bolt_test = bolt_test && bolt.hasBolt;
        }
        if (!bolt_test)
            return false;

        if (status == LightStatus.working)
        {
            if (b)
            {
                this.GetComponent<SpriteRenderer>().color = getColorFromType();
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

    //Add new color values here
    Color getColorFromType()
    {
        switch(color)
        {
            case LightColors.yellow:
                return new Color(255,255,0,1);
            case LightColors.red:
                return new Color(255, 0, 0, 1);
            default:
                return new Color(255, 255, 255, 1);
        }
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
        bool bolt_check = false;
        foreach(Bolt bolt in bolt_list)
            bolt_check = bolt_check || bolt.hasBolt;
        if (bolt_check)
            return;
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
        bool bolt_check = false;
        foreach (Bolt bolt in bolt_list)
            bolt_check = bolt_check || bolt.hasBolt;
        if (bolt_check)
            return;
        SwitchStatus(LightStatus.working);
    }
}
