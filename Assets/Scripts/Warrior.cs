using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Warrior : MonoBehaviour
{
    private Animator animator;

    private int horizontal_input;
    private int vertical_input;

    private bool isMoving;
    private bool isMovingDelayed;
    private Vector3 moveLocation;

    private readonly float MOVE_SPEED = 3f;
    private readonly float HOLD_KEY_LENGTH = 0.1f;

    private string LeftTile;
    private string RightTile;
    private string UpTile;
    private string DownTile;

    private bool isNoInput;
    private float holdKeyDelay;

    private void Start()
    {
        animator = GetComponent<Animator>();

        moveLocation = Vector2.zero;
    }

    private void Update()
    {
        GetInput();
        Move();
    }

    private void GetInput()
    {
        if (Input.GetAxis("Horizontal") > 0)
            horizontal_input = 1;
        else if (Input.GetAxis("Horizontal") < 0)
            horizontal_input = -1;
        else
            horizontal_input = 0;

        if (Input.GetAxis("Vertical") > 0)
            vertical_input = 1;
        else if (Input.GetAxis("Vertical") < 0)
            vertical_input = -1;
        else
            vertical_input = 0;
    }

    private void Move()
    {
        if (isMovingDelayed)
            return;

        if (!isMoving)
        {
            Vector3 newDirection = Vector3.zero;

            if (horizontal_input == -1)
            {
                animator.Play("WalkLeft");
                newDirection = new Vector3(-1, 0, 0);
            }
            else if (horizontal_input == 1)
            {
                animator.Play("WalkRight");
                newDirection = new Vector3(1, 0, 0);
            }
            else if (vertical_input == 1)
            {
                animator.Play("WalkUp");
                newDirection = new Vector3(0, 1, 0);
            }
            else if (vertical_input == -1)
            {
                animator.Play("WalkDown");
                newDirection = new Vector3(0, -1, 0);
            }
            else
            {
                isNoInput = true;
                return;
            }

            if (!KeyHeldLongEnough())
                return;

            if (Grid.Instance.TileIsMoveable(transform.position + newDirection))
            {
                moveLocation += newDirection;
            }

            isMoving = true;
        }

        transform.position = Vector2.MoveTowards(transform.position, moveLocation, Time.deltaTime * MOVE_SPEED);

        if (Vector2.Distance(transform.position, moveLocation) < 0.01f)
        {
            transform.position = moveLocation;
            isMoving = false;

            transform.position = Grid.Instance.CheckTeleport(transform.position);

            float delayTime = Grid.Instance.GetMoveDelay(transform.position);
            if (delayTime > 0)
            {
                isMovingDelayed = true;
                Invoke("RemoveDelay", delayTime);
            }
        }
    }

    private void RemoveDelay()
    {
        isMovingDelayed = false;
    }

    private bool KeyHeldLongEnough()
    {
        if (isNoInput)
        {
            isNoInput = false;
            holdKeyDelay = Time.time + HOLD_KEY_LENGTH;
            return false;
        }
        else if (Time.time < holdKeyDelay)
        {
            return false;
        }
        return true;
    }
}
