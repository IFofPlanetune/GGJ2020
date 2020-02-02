using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    private bool level_loaded = false;
    public int level_count = 0;
    public bool current = false;
    public float clock_timer = 0.1f;
    private float timer = 0f;
    public int health = 3;
    public bool reset = false;
    public Tool.ToolType selected_tool = Tool.ToolType.hand;
    public Light.LightColors tool_color = Light.LightColors.yellow;
    public Tool plus_tool;
    public Tool minus_tool;
    public int screw_amount = 0;

    public Image red_flash;
    bool dmg_done = true;

    public Healthbar healthbar;

    AudioSource source;
    public AudioClip damage;
    public AudioClip win;
    AudioSource impulseS;
    public AudioSource tick;
    public AudioSource music;


    private string level_file = "Assets/Levels/";

    public Timer t;

    public GameObject lamp_prefab;
    public GameObject bolt_prefab;
    public GameObject lever_prefab;
    public List<Light> light_list;
    public List<Bolt> bolt_list;
    public List<Lever> lever_list;

    //debug things
    public Transform anchor;

    // Start is called before the first frame update
    void Start()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

        StreamReader sr = new StreamReader("Assets/Levels/counter.txt");
        level_count = int.Parse(sr.ReadLine().Split('\n')[0]);
        sr.Close();
        source = this.GetComponents<AudioSource>()[0];
        impulseS = this.GetComponents<AudioSource>()[1];
        light_list = new List<Light>();
        bolt_list = new List<Bolt>();
        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        CheatCodeChecker();

        if (!level_loaded)
            return;
        if (reset)
            return;

        timer += Time.deltaTime;


        bool leverb = true;
        foreach (Lever l in lever_list)
        {
            leverb = leverb && l.on;
        }

        if (!leverb)
        {
            if (current)
            {
                timer = 0;
                current = !current;
                impulseS.Stop();
                foreach (Light l in light_list)
                    l.SetLight(false);
            }
            return;
        }

        if (timer >= clock_timer)
        {
            current = !current;
            timer = 0;

            
            if(!current)
            {
                impulseS.Stop();
                foreach (Light l in light_list)
                    l.SetLight(false);
            }
            else
            {
                impulseS.Play();
            }
        }


        if (current)
        {
            bool victory = true;
            int i = 1;
            foreach (Light l in light_list)
            {
                victory = victory && l.isCorrect();
            }

            if (victory)
            {
                print("Victory!");
                LevelCompleted();
            }

            foreach (Light l in light_list)
                if (!l.SetLight(true))
                {
                    reset = true;
                    TakeDamage();
                    ResetLevel();
                    return;
                }
        }
    }

    public void ChangeScrews(int i)
    {
        screw_amount += i;
        if(selected_tool == Tool.ToolType.minus)
        {
            minus_tool.ChangeScrews(screw_amount);
        }
        else
        {
            plus_tool.ChangeScrews(screw_amount);
        }
    }

    public void TakeDamage()
    {
        if (!dmg_done)
            return;
        dmg_done = false;
        screw_amount = 0;
        plus_tool.ChangeScrews(0);
        minus_tool.ChangeScrews(0);
        health--;
        healthbar.SetHearts(health);
        source.clip = damage;
        source.Play();
        if(health <= 0)
        {
            GameOver();
            return;
        }
        StartCoroutine(FlashRed());
        StartCoroutine(DmgCD());
    }

    IEnumerator DmgCD()
    {
        yield return new WaitForSeconds(0.2f);
        dmg_done = true;
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
        t.time_remaining = 0;
        level_loaded = false;
        impulseS.Stop();
        tick.Stop();
        foreach(Light light in light_list)
            light.GetComponent<BoxCollider2D>().enabled = false;
        foreach (Bolt bolt in bolt_list)
            bolt.GetComponent<BoxCollider2D>().enabled = false;
        foreach (Lever lever in lever_list)
            lever.GetComponent<BoxCollider2D>().enabled = false;
        SceneManager.LoadScene("GameOver",LoadSceneMode.Additive); 
    }

    void LevelCompleted()
    {
        t.time_remaining = 0;
        level_loaded = false;
        impulseS.Stop();
        tick.Stop();
        LoadNextLevel();
    }

    void LoadNextLevel()
    {
        level_loaded = false;
        foreach (Light light in light_list)
            light.GetComponent<BoxCollider2D>().enabled = false;
        foreach (Bolt bolt in bolt_list)
            bolt.GetComponent<BoxCollider2D>().enabled = false;
        foreach (Lever lever in lever_list)
            lever.GetComponent<BoxCollider2D>().enabled = false;
        SceneManager.LoadScene("LevelWin",LoadSceneMode.Additive);
    }

    public void ResetLevel()
    {
        foreach(Light l in light_list)
        {
            Destroy(l.gameObject);
        }
        foreach (Bolt b in bolt_list)
        {
            Destroy(b.gameObject);
        }
        foreach (Lever l in lever_list)
        {
            Destroy(l.gameObject);
        }
        light_list = new List<Light>();
        bolt_list = new List<Bolt>();
        lever_list = new List<Lever>();
        LoadLevel();
        reset = false;
    }
    void LoadLevel()
    {
        print(level_count);
        StreamReader sr = new StreamReader(level_file+level_count+".txt");
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            switch (line.Split(';')[0])
            {
                case "lamp":
                    LoadLamp(line);
                    break;
                case "bolt":
                    LoadBolt(line);
                    break;
                case "timer":
                    t.time_remaining = int.Parse(line.Split(new char[]{ ';', '\n'})[1]);
                    break;
                case "impulse":
                    clock_timer = float.Parse(line.Split(new char[] { ';', '\n' })[1]);
                    tick.Play();
                    //music.Play();
                    break;
                case "lever":
                    LoadLever(line);
                    break;
                default:
                    break;
            }
        }
        level_loaded = true;
    }

    void LoadLamp(string line)
    {
        string[] param = line.Split(';');
        Light light = Instantiate(lamp_prefab).GetComponent<Light>();
        for (int i = 1; i < param.Length; i++)
        {
            switch (i) {
                case 1:
                    print(float.Parse(param[i]));
                    light.transform.position = new Vector3(anchor.position.x+float.Parse(param[i]), anchor.position.y+float.Parse(param[i + 1]));
                    i++;
                    continue;

                case 3:
                    switch (param[i])
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
                    continue;
                case 4:
                    switch (param[i])
                    {
                        case "y":
                            light.color_goal = Light.LightColors.yellow;
                            light.current_color = Light.LightColors.yellow;
                            break;
                        case "r":
                            light.color_goal = Light.LightColors.red;
                            light.current_color = Light.LightColors.red;
                            break;
                        default:
                            break;
                    }
                    continue;
                default:
                    light.bolt_list.Add(bolt_list[int.Parse(param[i].Split('\n')[0]) - 1]);
                    continue;
            }
        }
        light.controller = this;
        light_list.Add(light);
    }

    void LoadBolt(string line)
    {
        string[] param = line.Split(';');
        Bolt bolt = Instantiate(bolt_prefab).GetComponent<Bolt>();
        for(int i = 1; i < param.Length; i++)
        {
            switch (i)
            {
                case 1:
                    bolt.transform.position = new Vector3(anchor.position.x+float.Parse(param[i]), anchor.position.y+float.Parse(param[i + 1]));
                    i++;
                    continue;

                case 3:
                    switch (param[i])
                    {
                        case "p":
                            bolt.type = Bolt.BoltType.plus;
                            break;
                        case "m":
                            bolt.type = Bolt.BoltType.minus;
                            break;
                        default:
                            break;
                    }
                    continue;
                default:
                    continue;
            }
        }
        bolt.controller = this;
        bolt_list.Add(bolt);
    }

    void LoadLever(string line)
    {
        string[] param = line.Split(';');
        Lever lever = Instantiate(lever_prefab).GetComponent<Lever>();
        for (int i = 1; i < param.Length; i++)
        {
            switch (i)
            {
                case 1:
                    lever.transform.position = new Vector3(anchor.position.x + float.Parse(param[i]), anchor.position.y + float.Parse(param[i + 1]));
                    i++;
                    continue;

                case 3:
                    switch (param[i])
                    {
                        case "t":
                            lever.on = true;
                            lever.GetComponent<SpriteRenderer>().sprite = lever.on_sprite;
                            break;
                        case "f":
                            lever.on = false;
                            lever.GetComponent<SpriteRenderer>().sprite = lever.off_sprite;
                            break;
                        default:
                            break;
                    }
                    continue;

                case 4:
                    switch (param[i])
                    {
                        case "v":
                            break;
                        case "h":
                            lever.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                            break;
                        default:
                            break;
                    }
                    continue;

                default:
                    continue;
            }
        }
        lever_list.Add(lever);
    }

    void CheatCodeChecker()
    {
        int level_no = 0;
        bool level_change = false;
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            level_change = true;
            level_no = 10;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            print("1 pressed");
            level_change = true;
            level_no = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            level_change = true;
            level_no = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            level_change = true;
            level_no = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            level_change = true;
            level_no = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            level_change = true;
            level_no = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            level_change = true;
            level_no = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            level_change = true;
            level_no = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            level_change = true;
            level_no = 8;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            level_change = true;
            level_no = 9;
        }

        if (level_change)
        {
            FileStream stream = File.Open("Assets/Levels/counter.txt", FileMode.Create);
            StreamWriter sr = new StreamWriter(stream);
            sr.WriteLine(level_no);
            sr.WriteLine();
            sr.Close();
            SceneManager.LoadScene("Game");
        }
        return;
    }
}
