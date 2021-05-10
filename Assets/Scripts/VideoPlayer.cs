using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
class VideoPlayer:MonoBehaviour
{
    GameObject vPlayer;
    RawImage rImgContainer;
    void Awake()
    {
        vPlayer = gameObject;
        rImgContainer = vPlayer.GetComponent<RawImage>();
    }
}
