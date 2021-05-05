using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Exit()
    {
        Application.Quit();
        Debug.LogError("退出异常！");
    }
    public void NewGame()
    {
        SceneManager.LoadSceneAsync("NewScene",LoadSceneMode.Single);
    }
    public void LoadGame()
    {
        //SceneManager.LoadSceneAsync("NewScene", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync(PlayerPrefs.GetString("SavedScene"), LoadSceneMode.Single);//读取存储的场景
    }
}
