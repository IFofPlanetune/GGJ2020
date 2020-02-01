using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool current = false;
    public float clock_timer = 0.1f;
    private float timer = 0f;
    public int health = 3;
    public bool reset = false;
    public Tool.ToolType selected_tool;

    public Image red_flash;

    public string level_file = "Assets/Levels/Test.txt";

    public GameObject lamp_prefab;
    public List<Light> light_list;

    //debug things

    void Start()
    {
        light_list = new List<Light>();
        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (reset)
            return;
        timer += Time.deltaTime;
        if(timer >= clock_timer)
        {
            current = !current;
            timer = Mathf.Max(0f, timer - clock_timer);

            if(current)
            {
                foreach(Light l in light_list)
                    if(!l.SetLight(true))
                    {
                        print(timer);
                        reset = true;
                        TakeDamage();
                        ResetLevel();
                        return;
                    }
            }
            else
            {
                foreach (Light l in light_list)
                    if(!l.SetLight(false))
                    {
                        print(timer);
                        reset = true;
                        TakeDamage();
                        ResetLevel();
                        return;
                    }
            }
        }
    }

    public void setTool(Tool.ToolType t)
    {
        selected_tool = t;
    }

    public void TakeDamage()
    {
        health--;
        if(health <= 0)
        {
            GameOver();
            return;
        }
        StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        print("coroutine started");
        red_flash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        red_flash.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        foreach(Light light in light_list)
            light.GetComponent<BoxCollider2D>().enabled = false;
        SceneManager.LoadScene("GameOver",LoadSceneMode.Additive); 
    }

    public void ResetLevel()
    {
        print("Lamps destroyed");
        foreach(Light l in light_list)
        {
            Destroy(l.gameObject);
        }
        light_list = new List<Light>();
        LoadLevel();
        reset = false;
    }
    public void LoadLevel()
    {
        StreamReader sr = new StreamReader(level_file);
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            switch (line.Split(';')[0])
            {
                case "lamp":
                    LoadLamp(line);
                    break;
                default:
                    break;
            }
        }
    }

    public void LoadLamp(string line)
    {
        string[] param = line.Split(';');
        Light light = Instantiate(lamp_prefab).GetComponent<Light>();
        for(int i = 1; i < param.Length; i++)
        {
            if(i == 1)
            {
                light.transform.position = new Vector3(float.Parse(param[i]),float.Parse(param[i+1]));
                i++;
                continue;
            }
            if(i == 3)
            {
                switch(param[i])
                {
                    case "w":
                        light.SwitchStatus(Light.LightStatus.working);
                        break;
                    case "b":
                        light.SwitchStatus(Light.LightStatus.broken);
                        break;
                    case "n":
                        light.SwitchStatus(Light.LightStatus.none);
                        break;
                    default:
                        break;
                }
            }
        }
        light.controller = this;
        light_list.Add(light);
    }
}
