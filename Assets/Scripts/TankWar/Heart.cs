using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Heart : MonoBehaviour
{
    public Sprite heartBroken;
    private GameObject lose;

    // Start is called before the first frame update
    void Awake()
    {
        lose = GameObject.Find("Lose");
        lose.SetActive(false);

    }

    void Die()
    {
        GetComponent<SpriteRenderer>().sprite = heartBroken;
        //GameObject.Find("Player").SendMessage("DieCommand");
        //ViewBase viewBase = new ViewBase();
        //viewBase.StartShowMessage("抱歉")
        lose.SetActive(true);
        //Invoke("ReturnToMain", 3f);
        StartCoroutine(ReturnToMain());
    }
    IEnumerator ReturnToMain()
    {
        yield return new WaitForSeconds(3);
        //yield return new WaitForEndOfFrame();
        SceneManager.UnloadSceneAsync("Tankwar");
    }
    
}
