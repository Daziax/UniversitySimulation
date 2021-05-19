using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5f; //子弹速度
    float x, y;//坦克移动方向
    int random;//坦克随机移动值
    float moveTimer = 0;
    float moveTime = 2f;
    public Sprite[] turning;//4张转向图
    private SpriteRenderer spriteRenderer;//SpriteRenderer组件
    public GameObject bulletInstance;//子弹Prefab
    Vector3 bulletRotation;//子弹旋转角度
    short bulletMove;//子弹移动方向
    public GameObject explosionPrefab;//爆炸特效
    public GameObject bornPrefab;//重生特效
    private float attackTimer = 0;//攻击计时器
    private float attackTimeVal = 1f;//攻击间隔
    //private int DieCount { get; set; } = 0;//死亡计次
    public float AttackTimer
    {
        get => attackTimer;
        set => attackTimer = value;
    }
    bool isDied;

    private float deathTimer = 0.8f;//死亡计时器
    public float DeathTimer
    {
        get => deathTimer;
        //set => deathTimer = isDied ? value : 0;
        set => deathTimer = value;
    }

    public float BornAnimeTimer
    {
        get;
        set;
    } = 0.8f;
    int bulletCount = 0;//已创建子弹数量


    private Queue<BulletClass> bulletQueue = new Queue<BulletClass>();

    BulletClass[] bulletList;

    ObjectPool<BulletClass> objectPool;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        objectPool = new ObjectPool<BulletClass>();
        bulletList = new BulletClass[20];

        isDied = false;

    }
    // Update is called once pe14r frame
    void Update()
    {
        Attack();
    }
    private void FixedUpdate()
    {
        TankMove();
    }
    private void HorizontalMove()
    {
        if (x < 0)
        {
            spriteRenderer.sprite = turning[3];
            bulletRotation = new Vector3(0, 0, 90);
            bulletMove = 2;
        }
        else if (x > 0)
        {
            spriteRenderer.sprite = turning[1];
            bulletRotation = new Vector3(0, 0, -90);
            bulletMove = 3;
        }

        transform.Translate(Vector3.right * x * speed * Time.deltaTime);
    }
    private void VerticalMove()
    {
        if (y < 0)
        {
            spriteRenderer.sprite = turning[2];
            bulletRotation = new Vector3(0, 0, 180);
            bulletMove = 1;
        }
        if (y > 0)
        {
            spriteRenderer.sprite = turning[0];
            bulletRotation = new Vector3(0, 0, 0);
            bulletMove = 0;
        }

        transform.Translate(Vector3.up * y * speed * Time.deltaTime);
    }
    private void TankMove()
    {
        if ((moveTimer += Time.deltaTime) > moveTime)
        {
            moveTimer = 0;
            random = Random.Range(0, 5);
        }
        if (isDied == true)
        {
        }
        else if (random <= 1)
        { y = -1; x = 0; VerticalMove(); }
        else if (random == 2)
        { y = 1; x = 0; VerticalMove(); }
        else if (random == 3)
        { y = 0; x = 1; HorizontalMove(); }
        else
        { y = 0; x = -1; HorizontalMove(); }
        
        

    }


    private void Attack()
    {

        //if ((AttackTimer += Time.deltaTime) > attackTimeVal&&!isDied)
        //{
        //    BulletClass bulletClass = objectPool.New(gameObject, bulletInstance, bulletMove, bulletRotation, true);
        //    bulletQueue.Enqueue(bulletClass);
        //    //bulletCount++;
        //    AttackTimer = 0;
        //}
        //if (bulletQueue.Count > 0)// 发射子弹数量
        //{
        //    foreach (var bulletClass in bulletQueue.ToArray())
        //    {
        //        if (!bulletClass.Move())
        //            objectPool.Store(bulletQueue.Dequeue());
        //    }
        //}

        if ((AttackTimer += Time.deltaTime) > attackTimeVal&& !isDied)
        {
            BulletClass bulletClass = objectPool.New(gameObject, bulletInstance, bulletMove, bulletRotation, true);
            bulletQueue.Enqueue(bulletClass);
            bulletCount++;
            AttackTimer = 0;
        }
        if (bulletCount > 0)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                bulletList[i] = bulletQueue.Dequeue();
                if (!bulletList[i].Move())
                {
                    objectPool.Store(bulletList[i]);
                    i--;
                    bulletCount--;
                }
                else
                    bulletQueue.Enqueue(bulletList[i]);
            }
        }

    }
    private void Die()
    {
        if(Random.Range(0, 2) == 0)
            transform.position = new Vector2(9.2f, 7.2f);
        else
            transform.position = new Vector2(-9.2f, 7.2f);
        spriteRenderer.sprite = turning[0];
        Destroy(Instantiate(bornPrefab,transform.position, Quaternion.Euler(0, 0, 0)), BornAnimeTimer);
        isDied = false;

        //DieCount++;
        //if (DieCount > 1)
        //{ GameObject.FindWithTag("MapCreation").SendMessage("CreateCommand"); }

    }
    private void DieCommand()
    {
        if (!isDied)
        {
            isDied = true;
            Destroy(Instantiate(explosionPrefab, transform), DeathTimer);
            Invoke("Die", DeathTimer);
        }
    }

    private void NewGame()
    {
        //DieCount = 0;
        if (Random.Range(0, 2) == 0)
            transform.position = new Vector2(9.2f, 7.2f);
        else
            transform.position = new Vector2(-9.2f, 7.2f);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
            GameObject.Find("Player").SendMessage("DieCommand");
        if (x > 0)
            random = 4;
        else if (x < 0)
            random = 3;
        else if (y < 0)
            random = 2;
        else if (y > 0)
            random = 1;
    }
    void OnCollisionStay2D(Collision2D collision)
    { 
        //if (x > 0)
        //    random = 4;
        //else if (x < 0)
        //    random = 3;
        //else if (y < 0)
        //    random = 2;
        //else if (y > 0)
        //    random = 1;

    }
}
