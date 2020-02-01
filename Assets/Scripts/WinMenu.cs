using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{

    // Start is called before the first frame update
    public Text win_text;
    int levelcount;
    void Start()
    {
        StreamReader sr = new StreamReader("Assets/Levels/counter.txt");
        levelcount = int.Parse(sr.ReadLine().Split('\n')[0]);
        win_text.text = "Level " + levelcount + ": You reapaired it!";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Reload()
    {
        FileStream stream = File.Open("Assets/Levels/counter.txt", FileMode.Create);
        StreamWriter sr = new StreamWriter(stream);
        sr.WriteLine(levelcount+1);
        sr.WriteLine();
        sr.Close();
        SceneManager.LoadScene("Game");
    }
}