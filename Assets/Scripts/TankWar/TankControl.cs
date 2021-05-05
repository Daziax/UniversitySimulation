using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControl : MonoBehaviour
{
    public float speed = 11f; //子弹速度
    float x, y;
    public Sprite[] turning;//4张转向图
    private SpriteRenderer spriteRenderer;//SpriteRenderer组件
    public GameObject bulletInstance;//子弹Prefab
    public Vector3 bulletRotation;//子弹旋转角度
    public short bulletMove;//子弹移动方向
    public GameObject shieldInstance;//护盾
    public GameObject explosionPrefab;//爆炸特效
    public GameObject bornPrefab;//重生特效
    private float defenceTimer = 2.3f;//防御计时器
    public float DefenceTimer
    {
        get => defenceTimer;
        //set => defenceTimer = isRevive == true ? value : 0;
        set => defenceTimer = value;
    }
    float attackTimeVal = 0.1f;
    private float attackTimer = 0.1f;//攻击计时器
    public float AttackTimer
    {
        get => attackTimer;
        set
        {
            if (attackTimer > 0.4f)
                attackTimer = 0;
            else if (isAttack)
                attackTimer = value;
        }
    }
    bool isDied, isAttack, isRevive, isDefend;
    
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

    public AudioClip audioAttack,audioMove,audioStop;
    private AudioSource audioTank;

    
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        audioTank = GetComponent<AudioSource>();
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        objectPool = new ObjectPool<BulletClass>();
        bulletList = new BulletClass[20];

        isDied = false;
        isAttack = false;
        isRevive = false;
        isDefend = false;
    }
    // Update is called once pe14r frame
    void Update()
    {
        Attack();
        //Die();
        //Revive();
    }
    bool lastMove;//记录上一次的移动按键
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
        audioTank.clip = audioMove;
        if (!audioTank.isPlaying)
            audioTank.Play();
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
        audioTank.clip = audioMove;
        if (!audioTank.isPlaying)
            audioTank.Play();
        transform.Translate(Vector3.up * y * speed * Time.deltaTime);
    }
    private void TankMove()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        if (isDied == true)
        {

        }
        else if (x != 0 && y == 0)
        {
            HorizontalMove();
            lastMove = false;
        }
        else if (y != 0 && x == 0)
        {
            VerticalMove();
            lastMove = true;
        }
        else if (x != 0 && y != 0)
        {
            if (lastMove)
                HorizontalMove();
            else
                VerticalMove();
        }
        else
        {
            audioTank.clip = audioStop;
            if (!audioTank.isPlaying)
                audioTank.Play();
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&!isDied)
        {
            isAttack = true;
            AudioSource.PlayClipAtPoint(audioAttack, transform.position);
            //AttackTimer = 0.4f;
        }
        if (isAttack && (AttackTimer += Time.deltaTime) > attackTimeVal)
        {
            BulletClass bulletClass = objectPool.New(gameObject, bulletInstance, bulletMove, bulletRotation, false);
            
            bulletQueue.Enqueue(bulletClass);
            bulletCount++;
            AttackTimer = 0;
            isAttack = false;
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
        //DeathTimer += Time.deltaTime;
    }
    private void Die()
    {
        //if ((DeathTimer+=Time.deltaTime)>2f)
        //{
        //DeathTimer = 0;
        transform.position = new Vector3(0, 0, 0);
        spriteRenderer.sprite = turning[0];
        Destroy(Instantiate(bornPrefab, transform.position, Quaternion.Euler(0, 0, 0)), BornAnimeTimer);
        Invoke("Revive", BornAnimeTimer);
        //}
    }
    private void DieCommand()
    {
        if (!isDefend && !isDied)
        {
            isDied = true;
            Destroy(Instantiate(explosionPrefab, transform), DeathTimer);
            Invoke("Die", DeathTimer);
        }
    }
    private void Defend()
    {

        //if ((DefenceTimer += Time.deltaTime) > 3.3f)
        //{
        shieldInstance.SetActive(false);
        //defenceTimer = 0;
        isRevive = false;
        isDefend = false;
        //}
    }
    private void Revive()
    {
        //if (isRevive)
        //{
        isDied = false;
        isRevive = true;
        isDefend = true;
        shieldInstance.SetActive(true);
        Invoke("Defend", DefenceTimer);
        //}
    }
}
//public class ObjectPool<T> where T : class
//{
//    BulletClass bulletClass;//实例化子弹
//    Bullet bulletScript;//子弹身上脚本

//    private Stack<BulletClass> m_objectStack = new Stack<BulletClass>();

//    public BulletClass New(GameObject player, GameObject bulletInstance, short bulletMove, Vector3 bulletRotation, bool isEnemy)
//    {
//        if (m_objectStack.Count == 0)
//        {
//            bulletClass = new BulletClass(player, bulletInstance, bulletMove, bulletRotation, isEnemy);
//        }
//        else
//        {
//            bulletClass = m_objectStack.Pop();
//            bulletClass.bulletMove = bulletMove;
//            bulletClass.bullet.transform.position = player.transform.position;
//            bulletClass.bullet.transform.rotation = Quaternion.Euler(bulletRotation);
//        }
//        bulletScript = bulletClass.bullet.GetComponent<Bullet>();
//        bulletScript.isEnemy = isEnemy;
//        bulletClass.isEnemy = isEnemy;


//        //bulletClass.bullet.SetActive(true);
//        return bulletClass;
//    }
//    public void Store(BulletClass t)
//    {
//        m_objectStack.Push(t);
//    }
//}
