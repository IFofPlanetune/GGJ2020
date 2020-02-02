using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    private List<SpriteRenderer> heart_list;
    private int latest_heart;

    // Start is called before the first frame update
    void Start()
    {
        heart_list = new List<SpriteRenderer>();
        heart_list.Add(GetComponentsInChildren<SpriteRenderer>()[0]);
        heart_list.Add(GetComponentsInChildren<SpriteRenderer>()[1]);
        heart_list.Add(GetComponentsInChildren<SpriteRenderer>()[2]);
        latest_heart = 2;
    }

    // Update is called once per fram

    public void SetHearts(int hearts)
    {
        print(hearts);
        if (latest_heart == hearts - 1)
            return;
        if (latest_heart > hearts - 1)
        {
            while(latest_heart > hearts - 1)
            {
                heart_list[latest_heart].enabled = false;
                latest_heart--;
            }
        }
        else
        {
            while (latest_heart < hearts - 1)
            {
                latest_heart++;
                heart_list[latest_heart].enabled = true;
            }
        }
    }

    void Update()
    {
        
    }
}
