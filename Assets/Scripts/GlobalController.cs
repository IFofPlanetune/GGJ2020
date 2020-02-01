using System.Collections;
using System.Collections.Generic;
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

    public Image red_flash;

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
        test_light.GetComponent<BoxCollider2D>().enabled = false;
        SceneManager.LoadScene("GameOver",LoadSceneMode.Additive); 
    }
}
