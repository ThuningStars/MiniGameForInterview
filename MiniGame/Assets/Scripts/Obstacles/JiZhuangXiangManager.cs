using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiZhuangXiangManager : MonoBehaviour
{
    [SerializeField] private GameObject JiZhuangXiang;
    [SerializeField] private List<GameObject> JiZhuangXiangChild;

    // Start is called before the first frame update
    void Awake()
    {
        DisableObstacles();
    }

    // random generate child JiZhuangXiang(max 3) in a spawn probability
    public void BeginGenerate(bool isWithSlope, float childSpawnProbability)
    {
        DisableObstacles();

        if (isWithSlope)
        {
            JiZhuangXiang.SetActive(true);
        }

        if (Random.value < childSpawnProbability)
        {
            JiZhuangXiangChild[0].SetActive(true);

        }
        if (Random.value < childSpawnProbability)
        {
            JiZhuangXiangChild[1].SetActive(true);

        }
        if (Random.value < childSpawnProbability)
        {
            JiZhuangXiangChild[2].SetActive(true);
        }

    }

    // set all child JiZhuangXiang not active
    public void DisableObstacles()
    {
        JiZhuangXiang.SetActive(false);

        foreach (GameObject gameObject in JiZhuangXiangChild)
        {
            gameObject.SetActive(false);
        }
    }

}
