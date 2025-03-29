using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [Header("Blocks Object")]
    [SerializeField] private GameObject blockBig;
    [SerializeField] private GameObject blockAi;


    [Header("Auto Generate Block List")]
    [SerializeField] private List<GameObject> bigBlockList;
    [SerializeField] private List<GameObject> aiBlockList;

    private float blockDistance;
    private GameObject bigBlockParent;
    private GameObject blockAiParent;
    private int blockNum;

    // getter & setter
    public GameObject BlockBig { get { return blockBig; } set { blockBig = value; } }
    public GameObject BlockAi { get { return blockAi; } set { blockAi = value; } }

    // Start is called before the first frame update
    void Awake()
    {
        // instantiate empty gameobject as blocks' parents
        bigBlockParent = new GameObject();
        bigBlockParent.name = "Block_Big";
        bigBlockParent = Instantiate(bigBlockParent, Vector3.zero, Quaternion.identity, transform);
        bigBlockParent.transform.position = Vector3.zero;

        blockAiParent = new GameObject();
        blockAiParent.name = "Block_Ai";
        blockAiParent = Instantiate(blockAiParent, Vector3.zero, Quaternion.identity, transform);
        blockAiParent.transform.position = Vector3.zero;
    }

    public void GenerateAllBlocks(int numBlock)
    {
        blockNum = numBlock;

        float roadLength = 32;
        blockDistance = roadLength / blockNum;

        if (blockBig != null && blockAi != null)
        {
            for (int i = 0; i < blockNum; i++)
            {
                // all the block_big
                GameObject spawnedObject = Instantiate(blockBig, (Vector3.forward * blockDistance * i) + transform.position, Quaternion.identity, bigBlockParent.transform);
                bigBlockList.Add(spawnedObject);
                spawnedObject.SetActive(false);

                // all the block_ai
                GameObject aiBlock = Instantiate(blockAi, (Vector3.forward * blockDistance * i) + transform.position, Quaternion.identity, blockAiParent.transform);
                aiBlockList.Add(aiBlock);
                aiBlock.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("Blocks Unassigned");
        }
    }

    public void BeginGenerate(float bigBlockFrequency, float blockFrequency)
    {
        DisableObstacles();

        if (blockBig != null && blockAi != null)
        {
            for (int i = 0; i < blockNum; i++)
            {
                if(Random.value < blockFrequency)
                {
                    if (Random.value < bigBlockFrequency)
                    {
                        bigBlockList[i].SetActive(true);
                    }
                    else
                    {
                        aiBlockList[i].SetActive(true);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Blocks Unassigned");
        }
    }

    // set all child JiZhuangXiang not active
    public void DisableObstacles()
    {
        foreach (GameObject gameObject in bigBlockList)
        {
            gameObject.SetActive(false);
        }

        foreach (GameObject gameObject in aiBlockList)
        {
            gameObject.SetActive(false);
        }
    }
}
