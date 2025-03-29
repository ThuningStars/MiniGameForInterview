using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public enum State { Run, Jump, Slide, Die }

public class PlayerController : MonoBehaviour
{
    [Header("PunishTime")]
    [SerializeField] private float punishTime = 0.5f; 
    [SerializeField] private bool isPunishing = false;

    [Header("States")]
    [SerializeField] private State playerState = State.Run;
    [SerializeField] private Lane playerLane = Lane.Middle;
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private bool isGrounded = false;

    // Jump
    [Header("Jump")]
    [SerializeField] private int JumpTimer = 0;
    [SerializeField] private float jumpForce = 6f;
    private LayerMask groundLayer;

    // Back to Ground
    [Header("Back to Ground")]
    [SerializeField] private float onGrouondForce = 6f;

    //Slide
    private BoxCollider playerCollider;
    private Vector3 originalColliderSize;
    private Vector3 originalColliderCenter;
    private float slideTime = 0.6f;

    // Player self Components
    private Rigidbody rb;
    private Animation anim;

    //Change lane
    [Header("Lane")]
    [SerializeField] private float laneDistance = 2f;
    [SerializeField] private float switchSpeed = 8f;
    [SerializeField] private float targetXPos;
    [SerializeField] private bool isPlayerTurnLeft = false;

    //Getter & Setter
    public float PunishTime { get { return punishTime; } set { punishTime = value; } }
    public bool IsPunishing { get { return isPunishing; } set { isPunishing = value; } }
    public State PlayerState { get { return playerState; } set { playerState = value; } }
    public bool IsPlayerTurnLeft { get { return isPlayerTurnLeft; } set { isPlayerTurnLeft = value; } }

    void Start()
    {
        //initialize
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animation>();
        if(anim != null) 
            anim.clip = anim.GetClip("run");
        groundLayer = LayerMask.GetMask("Ground");
        playerCollider = GetComponent<BoxCollider>();
        if(playerCollider != null)
        {
            originalColliderSize = playerCollider.size;
            originalColliderCenter = playerCollider.center;
        }
    }

    void Update()
    {
        if (playerState != State.Die)
        {
            //Check is player on ground
            isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

            // if not punish then player can move
            if (!isPunishing)
            {
                // Player Movements
                if (isGrounded)
                {
                    // Jump
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        Jump();
                    }
                    else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        if(playerState != State.Slide)
                            StartCoroutine(Slide());
                    }

                }
                else
                {

                    if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        OnGround();
                    }
                }

                // Player switch lanes
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    MoveToLeftLane(true);
                }
                else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    MoveToLeftLane(false);
                }
            }
            // if punish then wait until punish time over
            else
            {
                StartCoroutine(PunishTimeBegin());
            }

            //Play Animation
            PlayAnimation();
        }
        else
        {
            StopAllCoroutines();

            anim.Play("die");
        }
    }
    void FixedUpdate()
    {
        ChangeLaneMovement();
    }
    private void Jump()
    {
        ChangePlayerState(State.Jump);

        // Reset jump timer
        JumpTimer = 0;

        // Jump
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private IEnumerator Slide()
    {
        ChangePlayerState(State.Slide);
        playerCollider.size = playerCollider.size - (Vector3.up * playerCollider.size.y / 1.5f);
        playerCollider.center = playerCollider.center - (Vector3.up * playerCollider.center.y / 1.5f);
        anim.Play("roll");

        // wait 0.5s back to run
        yield return new WaitForSeconds(slideTime);
        playerCollider.size = originalColliderSize;
        playerCollider.center = originalColliderCenter;

        ChangePlayerState(State.Run);
        PlayAnimation();
    }
    private void OnGround()
    {
        ChangePlayerState(State.Run);

        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.down * onGrouondForce, ForceMode.Impulse);
    }

    private IEnumerator PunishTimeBegin()
    {
        yield return new WaitForSeconds(punishTime);
        IsPunishing = false;
    }

    // Check and play the right animation
    private void PlayAnimation()
    {
        switch(playerState)
        {
            case State.Run:

                if (!anim.isPlaying)
                {
                    anim.Play();
                }

                break;
            case State.Jump:

                AnimationControl(ref JumpTimer, 2, "fly");

                break;
        }

    }

    //Control how many time if the animation change back to Run animation
    private void AnimationControl(ref int timer, int totalTimes, string animName)
    {
        // play once at first time
        if (timer == 0)
        {
            anim.Play(animName);
            timer++;
        }
        else
        {
            // if is done play the first time, check if needed to play more times
            if (!anim.isPlaying)
            {
                if (timer < totalTimes)
                {
                    anim.Play(animName);
                    timer++;
                }
                // Turn back to Run state
                else if (timer == totalTimes)
                {
                    ChangePlayerState(State.Run);
                    timer = 0;
                    anim.Play();

                }
            }
        }
    }
    public void MoveToLeftLane(bool isLeft)
    {
        isPlayerTurnLeft = isLeft;

        // move left
        if (isLeft)
        {
            switch (playerLane)
            {
                case Lane.Left:
                    return;
                case Lane.Middle:
                    playerLane = Lane.Left;

                    break;
                case Lane.Right:
                    playerLane = Lane.Middle;

                    break;
            }
            targetXPos -= laneDistance;
        }
        // move right
        else
        {
            switch (playerLane)
            {
                case Lane.Left:
                    playerLane = Lane.Middle;

                    break;
                case Lane.Middle:
                    playerLane = Lane.Right;

                    break;
                case Lane.Right:
                    return;
            }
            targetXPos += laneDistance;
        }
    }
    private void ChangeLaneMovement()
    {
        Vector3 targetPos = new Vector3(
            targetXPos,
            rb.position.y,
            rb.position.z
        );

        rb.MovePosition(Vector3.MoveTowards(
            rb.position,
            targetPos,
            switchSpeed * Time.fixedDeltaTime
        ));
    }

    public void ChangePlayerState(State state)
    {
        playerState = state;
    }

}
