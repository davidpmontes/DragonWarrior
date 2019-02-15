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
    private Vector3 moveToLocation;

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

        moveToLocation = transform.position;
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
                moveToLocation += newDirection;
            }

            isMoving = true;
        }

        transform.position = Vector2.MoveTowards(transform.position, moveToLocation, Time.deltaTime * MOVE_SPEED);

        if (Vector2.Distance(transform.position, moveToLocation) < 0.01f)
        {
            transform.position = moveToLocation;
            isMoving = false;

            LayerMask layerMask = 1 << LayerMask.NameToLayer("teleports");

            RaycastHit2D hit = Physics2D.BoxCast(transform.position + new Vector3(0.5f, 0.5f, 0), new Vector2(0.5f, 0.5f), 0, Vector2.zero, 0, layerMask);

            if (hit.collider)
            {
                Grid.Instance.TeleportChangeMap(hit.collider.gameObject.GetComponent<ITeleporter>().GetTileMap());
                transform.position = hit.collider.gameObject.GetComponent<ITeleporter>().GetLocation();
                moveToLocation = transform.position;
                switch (hit.collider.gameObject.GetComponent<ITeleporter>().GetFacingDirection())
                {
                    case Teleporter.Direction.LEFT:
                        animator.Play("WalkLeft");
                        break;
                    case Teleporter.Direction.RIGHT:
                        animator.Play("WalkRight");
                        break;
                    case Teleporter.Direction.UP:
                        animator.Play("WalkUp");
                        break;
                    case Teleporter.Direction.DOWN:
                        animator.Play("WalkDown");
                        break;
                }
            }

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
