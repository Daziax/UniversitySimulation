using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public Sprite heartBroken;
    // Start is called before the first frame update
    void Die()
    {
        GetComponent<SpriteRenderer>().sprite = heartBroken;
        GameObject.Find("Player").SendMessage("DieCommand");
    }
}
