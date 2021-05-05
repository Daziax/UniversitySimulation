using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletClass
{
    public GameObject bullet;//实例化子弹
                             //public Vector3 bulletRotation;//子弹旋转角度
    public short bulletMove;//子弹移动方向
    public bool isEnemy;
    private Vector3 hideBullet = new Vector3(20, 20, 0);
    public float bulletSpeed=11f;
    public BulletClass(GameObject player, GameObject bulletInstance, short bulletMove, Vector3 bulletRotation, bool isEnemy)//下一个子弹的参数
    {
        bullet = UnityEngine.GameObject.Instantiate(bulletInstance);
        this.isEnemy = isEnemy;
        this.bulletMove = bulletMove;
        bullet.transform.position = player.transform.position;
        bullet.transform.rotation = Quaternion.Euler(bulletRotation);

    }
    public bool Move()
    {

        if (Mathf.Abs( bullet.transform.position.x) > 9.5 || Mathf.Abs(bullet.transform.position.y) > 7.5 )
        {
            //bullet.SetActive(false);
            bullet.transform.position = hideBullet;
            return false;
        }
        switch (bulletMove)
        {
            case 0:
                bullet.transform.Translate(Vector3.up * bulletSpeed * Time.deltaTime, Space.World);
                break;
            case 1:
                bullet.transform.Translate(Vector3.down * bulletSpeed * Time.deltaTime, Space.World);
                break;
            case 2:
                bullet.transform.Translate(Vector3.left * bulletSpeed * Time.deltaTime, Space.World);
                break;
            case 3:
                bullet.transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime, Space.World);
                break;
        }
        return true;
    }
}
