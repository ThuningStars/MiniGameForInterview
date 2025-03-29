using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [SerializeField] private GameObject floor;
    [SerializeField] private float floorDistance = 32f;
    [SerializeField] private int floorNum = 10;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float interval = 10f;
    [SerializeField] private float multiplier = 1.2f;
    [SerializeField] private List<GameObject> floorList;

    private float resetPosZ;

    // Start is called before the first frame update
    void Start()
    {
        resetPosZ = floorDistance * (floorNum - 1);
        InitialFloors();
    }

    private void InitialFloors()
    {
        if (floor != null)
        {
            for (int i = 0; i < floorNum; i++)
            {
                GameObject spawnedObject = Instantiate(floor, Vector3.forward * floorDistance * i, Quaternion.identity, transform);

                FloorBehaviour floorBehaviour = spawnedObject.GetComponent<FloorBehaviour>();

                // set floor properties
                if(floorBehaviour != null)
                {
                    // floor properties
                    floorBehaviour.DistroyPosZ = -floorDistance;
                    floorBehaviour.RespawnPosZ = resetPosZ;
                    floorBehaviour.MoveSpeed = moveSpeed;
                    floorBehaviour.Interval = interval;
                    floorBehaviour.Multiplier = multiplier;

                    // generate obstacles(except first floor)
                    if (i != 0)
                    {
                        floorBehaviour.GenerateObstacles();
                    }
                }

                floorList.Add(spawnedObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetFloorSpeed()
    {
        foreach (GameObject floor in floorList)
        {
            floor.GetComponent<FloorBehaviour>().ResetMoveSpeed();
        }
    }
    public void StopFloorMoving()
    {
        foreach (GameObject floor in floorList)
        {
            floor.GetComponent<FloorBehaviour>().StopMoving();
        }
    }
}
