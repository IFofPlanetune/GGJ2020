using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool current = false;
    public float clock_timer = 0.1f;
    private float timer = 0f;

    //debug things
    public Light test_light;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= clock_timer)
        {
            current = !current;
            timer = Mathf.Max(0f, timer - clock_timer);

            if(current)
            {
                test_light.SetLight(true);
            }
            else
            {
                test_light.SetLight(false);
            }
        }
    }
}
