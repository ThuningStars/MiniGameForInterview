using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [Header("Obstacles")]
    [SerializeField] private GameObject lane;
    [SerializeField] private GameObject blockBig;
    [SerializeField] private GameObject blockAi;

    [Header("Blocks vs JiZhuangXiang Appear Frequency")]
    [Range(0, 2)]
    [SerializeField] private int maxJiZhuangXiangLane = 2;
    [SerializeField] private float jiZhuangXiangProbability = 0.5f;

    [Header("Blocks Properties")]
    [SerializeField] private int blockNum = 5;
    [SerializeField] private float blockFrequency = 0.4f;
    [SerializeField] private float bigBlockFrequency = 0.5f;

    [Header("JiZhuangXiang Properties")]
    [SerializeField] private float childSpawnProbability = 0.35f;
    [SerializeField] private float withSlopeProbability = 0.4f;

    [Header("Auto Generate Block List")]
    [SerializeField] private List<LaneManager> laneList;


    private GameObject obstacleParent;

    // Start is called before the first frame update
    void OnEnable()
    {        
        // instantiate empty gameobject as blocks' parents
        obstacleParent = new GameObject();
        obstacleParent.name = "Obstacles";
        obstacleParent = Instantiate(obstacleParent, Vector3.zero, Quaternion.identity, transform);

        //Generate 3 Lanes
        if (lane != null && blockBig != null && blockAi != null)
        {
            //Left Lane
            GameObject leftLaneObj = Instantiate(lane, (Vector3.right * -2) + transform.position, Quaternion.identity, obstacleParent.transform);
            leftLaneObj.name = "LeftLane";
            LaneManager leftLane = leftLaneObj.GetComponent<LaneManager>();
            leftLane.LaneType = Lane.Left;
            leftLane.InitialAllKindBlocks(blockBig, blockAi, blockNum);
            laneList.Add(leftLane);

            //Middle Lane
            GameObject MiddleLaneObj = Instantiate(lane, Vector3.zero + transform.position, Quaternion.identity, obstacleParent.transform);
            MiddleLaneObj.name = "MiddleLane";
            LaneManager MiddleLane = MiddleLaneObj.GetComponent<LaneManager>();
            MiddleLane.LaneType = Lane.Middle;
            MiddleLane.InitialAllKindBlocks(blockBig, blockAi, blockNum);
            laneList.Add(MiddleLane);

            //Right Lane
            GameObject RightLaneObj = Instantiate(lane, (Vector3.right * 2) + transform.position, Quaternion.identity, obstacleParent.transform);
            RightLaneObj.name = "RightLane";
            LaneManager RightLane = RightLaneObj.GetComponent<LaneManager>();
            RightLane.LaneType = Lane.Right;
            RightLane.InitialAllKindBlocks(blockBig, blockAi, blockNum);
            laneList.Add(RightLane);
        }
        else
        {
            Debug.LogError("Obstacles Unassigned");
        }

    }

    // random generate
    public void RandomGenerateObstacles()
    {
        List<int> laneIndex = new List<int>{ 0, 1, 2 };

        // generate JiZhuangXiang
        for (int i = 0; i < maxJiZhuangXiangLane; i++)
        {
            // if need to generate JiZhuangXiang
            if (Random.value < jiZhuangXiangProbability) 
            {
                // randomly choose a lane to generate
                int randomIndex = Random.Range(0, laneIndex.Count);

                laneList[laneIndex[randomIndex]].GenerateJiZhuangXiang((Random.value < withSlopeProbability), childSpawnProbability);
                laneIndex.RemoveAt(randomIndex);
            }
        }

        // generate Blocks at the lane that left
        for (int i = 0; i < laneIndex.Count; i++)
        {
            laneList[laneIndex[i]].GenerateObstacles(bigBlockFrequency, blockFrequency);
        }
    }
}
