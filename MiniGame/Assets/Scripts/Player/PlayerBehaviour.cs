using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject scoreObj;
    [SerializeField] private int score;

    private TMP_Text scoreText;
    private float lastTriggerTime;
    private PlayerController playerController;
    private GroundManager groundManager;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        groundManager = FindObjectOfType<GroundManager>();
        scoreText = scoreObj.GetComponent<TMP_Text>();

        score = 0;

        // start adding score
        if (playerController != null && scoreText != null)
        {
            StartCoroutine(IncreaseScore());
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerController != null && groundManager != null)
        {
            if (other.CompareTag("Wall"))
            {
                // cool down for trigger
                if (Time.time - lastTriggerTime < playerController.PunishTime)
                    return;

                lastTriggerTime = Time.time;

                // player punishment
                playerController.IsPunishing = true;
                groundManager.ResetFloorSpeed();

                Vector3 otherPos = other.transform.position;

                // the obstacle is at the same lane as player
                if (otherPos.x == (int)transform.position.x)
                {
                    // player die
                    PlayerDie();

                    Debug.Log("Game Over");
                }
                // push player back to the original lane
                else
                {
                    // reduce score
                    score -= 100;
                    UpdateScoreText();

                    // obstacle at left
                    if (otherPos.x < (int)transform.position.x)
                    {
                        // to the right lane
                        playerController.MoveToLeftLane(false);
                    }
                    // obstacle at right
                    else
                    {                        
                        // to the left lane
                        playerController.MoveToLeftLane(true);
                    }
                }

            }
            else if (other.CompareTag("Obstacle"))
            {
                PlayerDie();

                Debug.Log("Game Over");
            }
        }
        else
        {
            if (playerController == null)
            {
                Debug.LogError("Cannot find PlayerController ");
            }

            if (groundManager != null)
            {
                Debug.LogError("Cannot find GroundManager ");
            }
        }
    }

    IEnumerator IncreaseScore()
    {
        while (playerController.PlayerState != State.Die)
        {
            yield return new WaitForSeconds(1.5f);
            score += 150;
            UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score : " + score.ToString();
    }

    private void PlayerDie()
    {
        groundManager.StopFloorMoving();
        playerController.PlayerState = State.Die;
    }

}
