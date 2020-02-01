using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    public enum BoltType
    {
        plus,minus
    }
    public bool hasBolt = true;
    public GlobalController controller;
    public BoltType type;

    public Sprite minusSprite;
    public Sprite plusSprite;
    public Sprite emptySprite;

    private AudioSource source;
    public AudioClip screw;
    public AudioClip wrong;

    // Start is called before the first frame update
    void Start()
    {
        source = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasBolt)
        {
            if(type == BoltType.minus)
            {
                this.GetComponent<SpriteRenderer>().sprite = minusSprite;
            }
            else
            {
                this.GetComponent<SpriteRenderer>().sprite = plusSprite;
            }
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = emptySprite;
        }
    }

    private void OnMouseDown()
    {
        if (controller)
        {
            if (controller.reset)
                return;
        }

        if (hasBolt)
        {
            RemoveBolt();
        }
        else
        {
            AddBolt();
        }
    }

    void RemoveBolt()
    {
        if((type == BoltType.minus && controller.selected_tool == Tool.ToolType.minus) ||
           (type == BoltType.plus && controller.selected_tool == Tool.ToolType.plus))
        {
            source.clip = screw;
            source.Play();
            hasBolt = false;
        }
        else
        {
            source.clip = wrong;
            source.Play();
        }
    }

    void AddBolt()
    {
        if ((type == BoltType.minus && controller.selected_tool == Tool.ToolType.minus) ||
           (type == BoltType.plus && controller.selected_tool == Tool.ToolType.plus))
        {
            source.clip = screw;
            source.Play();
            hasBolt = false;
            hasBolt = true;
        }
        else
        {
            source.clip = wrong;
            source.Play();
        }
    }
}
