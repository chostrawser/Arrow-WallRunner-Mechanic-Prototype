using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController instance; //assets that utilize singleton
    public GameObject player;

    public event Action Escape = delegate { };
    public event Action Backspace = delegate { };

    void Awake()
    {
        //singleton
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
            Destroy(gameObject);

    }

    private void OnEnable()
    {
        Escape += quitGame;
        Backspace += restartLevel;
    }

    private void OnDisable()
    {
        Escape -= quitGame;
        Backspace -= restartLevel;
    }

    void Update()
    {
        checkEscape();
        checkBackspace();
    }

    private void checkEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Escape?.Invoke();    
    }

    private void checkBackspace()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete)) 
            Backspace?.Invoke();
    }

    public void quitGame()
    { Application.Quit(); }

    public void restartLevel()
    { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }

}
