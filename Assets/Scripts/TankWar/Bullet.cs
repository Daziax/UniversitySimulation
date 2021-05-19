using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet:MonoBehaviour
{
    //public GameObject bullet;//实例化子弹
    ////public Vector3 bulletRotation;//子弹旋转角度
    public short bulletMove;//子弹移动方向
    public bool isEnemy;
    public bool isDefend;
    private Vector3 hideBullet;
    public AudioClip audioHit;


    // Start is called before the first frame update
    void Start()
    {
        hideBullet = new Vector3(20, 20, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                if (isEnemy)
                {
                    gameObject.transform.position = hideBullet;
                    GameObject.Find("Player").SendMessage("DieCommand");
                }
                break;
            case "Enemy":
                if (!isEnemy)
                {
                    GameObject.Find("Player").SendMessage("EnemyDieCommand");
                    gameObject.transform.position = hideBullet;
                    GameObject.Find(collision.name).SendMessage("DieCommand");
                }
                break;
            case "Bullet":
                gameObject.transform.position = hideBullet;
                AudioSource.PlayClipAtPoint(audioHit, transform.position);
                break;
            case "RedWall":
                gameObject.transform.position=hideBullet;
                AudioSource.PlayClipAtPoint(audioHit, transform.position);
                collision.gameObject.SetActive(false);
                break;
            case "WhiteWall":
                gameObject.transform.position = hideBullet;
                //AudioSource.PlayClipAtPoint(audioHit, transform.position);
                break;
            case "Tree":
                break;
            case "Water":
                break;
            case "Heart":
                gameObject.transform.position = hideBullet;
                GameObject.Find("Heart").SendMessage("Die");
                break;

        }    
    }

}