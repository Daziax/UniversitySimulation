using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    GameObject tree, water, heart, whiteWall, redWall;
    List<Vector2> positionList;
    private int levelCount { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        tree = (GameObject)Resources.Load("Prefabs/Map/Tree");
        water = (GameObject)Resources.Load("Prefabs/Map/Water");
        heart = (GameObject)Resources.Load("Prefabs/Map/Heart");
        whiteWall = (GameObject)Resources.Load("Prefabs/Map/WhiteWall");
        redWall = (GameObject)Resources.Load("Prefabs/Map/RedWall");

        positionList = new List<Vector2>(10) { new Vector2(-1.5f, -6.5f), new Vector2(-0.5f, -6.5f), new Vector2(0.5f, -6.5f), new Vector2(1.5f, -6.5f), new Vector2(-1.5f, -7.5f), new Vector2(-0.5f, -7.5f), new Vector2(0.5f, -7.5f), new Vector2(1.5f, -7.5f), new Vector2(-0.5f, 1.5f), new Vector2(8.5f, 7.5f), new Vector2(-8.5f, 7.5f), new Vector2(-9.5f, 7.5f) };

        CreateRandomMap();
    }
    public void CreateCommand()
    {
        Destroy(gameObject);
        Instantiate(Resources.Load("Prefabs/MapCreation"));
        GameObject.FindGameObjectWithTag("Player").SendMessage("Die");
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.SendMessage("NewGame");
        }
    }
    void CreateRandomMap()
    {
        int whiteWallCount, redWallCount,treeCount,waterCount;
        redWallCount = Random.Range(0,51);
        whiteWallCount = Random.Range(0, 31);
        treeCount = Random.Range(0, 51);
        waterCount = Random.Range(0, 31);

        CreateWall(whiteWallCount,whiteWall);
        CreateWall(redWallCount,redWall);
        CreateWall(treeCount, tree);
        CreateWall(waterCount, water);
    }
    bool IsExisted(Vector2 position)
    {
        if(positionList.Contains(position))
        {
            return true;
        }
        return false;
    }
    void CreateWall(int count,GameObject whichWall)
    {
        GameObject mapPiece;
        for (int i = 0; i < count; i++)
        {
            Vector2 position = new Vector2(Random.Range(-9, 11) - 0.5f, Random.Range(-7, 9) - 0.5f);

            if (!IsExisted(position))
            {
                mapPiece = GameObject.Instantiate(whichWall, transform, true);
                positionList.Add(position);
                mapPiece.transform.position = position;
            }
            else
            {
                i--;
            }
        }
    }
}
