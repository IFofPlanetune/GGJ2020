using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    public GlobalController controller;

    public SpriteRenderer first_slot;
    public SpriteRenderer second_slot;
    public SpriteRenderer third_slot;
    public SpriteRenderer fourth_slot;

    public float time_remaining = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time_remaining == 0)
            return;
        time_remaining -= Time.deltaTime;
        if(time_remaining < 0)
        {
            time_remaining = 0;
            ShowTime();
            controller.GameOver();
        }
        ShowTime();
    }

    void ShowTime()
    {
        int minutes = (int)time_remaining / 60;
        int minuteA = minutes / 10;
        int minuteB = minutes % 10;

        int seconds = (int)time_remaining % 60;
        int secondA = seconds / 10;
        int secondB = seconds % 10;

        first_slot.sprite = Resources.Load<Sprite>("Stelle 1/Stelle 1_" + minuteA);
        second_slot.sprite = Resources.Load<Sprite>("Stelle 2/Stelle 2_" + minuteB);
        third_slot.sprite = Resources.Load<Sprite>("Stelle 3/Stelle 3_" + secondA);
        fourth_slot.sprite = Resources.Load<Sprite>("Stelle 4/Stelle 4_" + secondB);
    }

}
