using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBehaviour : MonoBehaviour
{
    [SerializeField] private float distroyPosZ;
    [SerializeField] private float respawnPosZ;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float interval;
    [SerializeField] private float multiplier;

    private float originMoveSpeed;
    private bool canMove = true;
    private ObstacleManager obstacleManager;

    //Getter & Setter
    public float DistroyPosZ { get { return distroyPosZ; } set { distroyPosZ = value; } }
    public float RespawnPosZ { get { return respawnPosZ; } set { respawnPosZ = value; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public float Interval { get { return interval; } set { interval = value; } }
    public float Multiplier { get { return multiplier; } set { multiplier = value; } }

    private void Awake()
    {
        obstacleManager = GetComponent<ObstacleManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        originMoveSpeed = moveSpeed;
        // increase speed in every time interval
        StartCoroutine(IncreasingSpeed());
    }

    private void FixedUpdate()
    {
        if (canMove)
            Move();
    }

    private void Move()
    {
        if(transform.position.z <= distroyPosZ)
        {
            Respawn();
        }

        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
    }

    private void Respawn()
    {
        // generate obstacles
        obstacleManager.RandomGenerateObstacles();
        transform.Translate(Vector3.forward * respawnPosZ);
    }

    IEnumerator IncreasingSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            moveSpeed *= multiplier;
        }
    }

    public void GenerateObstacles()
    {
        // generate obstacles
        if (obstacleManager != null) { obstacleManager.RandomGenerateObstacles(); }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    public void ResetMoveSpeed()
    {
        moveSpeed = originMoveSpeed;
    }

    public void StopMoving()
    { 
        StopAllCoroutines();
        moveSpeed = 0;
        canMove = false;
    }
}
