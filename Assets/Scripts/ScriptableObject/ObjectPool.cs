using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ObjectPool<T> where T : class
{
    BulletClass bulletClass;//实例化子弹
    Bullet bulletScript;//子弹身上脚本

    private Stack<BulletClass> m_objectStack = new Stack<BulletClass>();

    public BulletClass New(GameObject player, GameObject bulletInstance, short bulletMove, Vector3 bulletRotation, bool isEnemy)
    {
        if (m_objectStack.Count == 0)
        {
            bulletClass = new BulletClass(player, bulletInstance, bulletMove, bulletRotation, isEnemy);
            //bulletClass = ScriptableObject.CreateInstance<BulletClass>();
        }
        else
        {
            bulletClass = m_objectStack.Pop();
            bulletClass.bulletMove = bulletMove;
            bulletClass.bullet.transform.position = player.transform.position;
            bulletClass.bullet.transform.rotation = Quaternion.Euler(bulletRotation);
            bulletClass.isEnemy = isEnemy;
        }
        bulletScript = bulletClass.bullet.GetComponent<Bullet>();
        bulletScript.isEnemy = isEnemy;
        


        //bulletClass.bullet.SetActive(true);
        return bulletClass;
    }
    public void Store(BulletClass t)
    {
        m_objectStack.Push(t);
    }
}
