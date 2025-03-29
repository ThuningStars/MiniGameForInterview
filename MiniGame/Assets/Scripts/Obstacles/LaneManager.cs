using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Lane { Left, Middle, Right }

public class LaneManager : MonoBehaviour
{
    [SerializeField] private Lane laneType;

    private JiZhuangXiangManager jiZhuangXiangManager;
    private BlockManager blockManager;

    // getter & setter
    public Lane LaneType { get { return laneType; } set { laneType = value; } }

    // Start is called before the first frame update
    void Awake()
    {
        jiZhuangXiangManager = GetComponentInChildren<JiZhuangXiangManager>();
        blockManager = GetComponentInChildren<BlockManager>();
    }

    //Initial all the kinds of blocks
    public void InitialAllKindBlocks(GameObject blockBig, GameObject blockAi, int blockNum)
    {
        blockManager.BlockBig = blockBig;
        blockManager.BlockAi = blockAi;
        blockManager.GenerateAllBlocks(blockNum);
    }

    public void GenerateObstacles(float bigBlockFrequency, float blockFrequency)
    {
        jiZhuangXiangManager.DisableObstacles();

        blockManager.BeginGenerate(bigBlockFrequency, blockFrequency);
    }

    public void GenerateJiZhuangXiang(bool isWithSlope, float childSpawnProbability)
    {
        blockManager.DisableObstacles();

        jiZhuangXiangManager.BeginGenerate(isWithSlope, childSpawnProbability);
    }
}
