using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Warrior : MonoBehaviour
{
    public Tilemap tilemap;

    private Animator animator;

    private int horizontal_input;
    private int vertical_input;

    private bool isMoving;
    private bool isMovingDelayed;
    private Vector2 moveLocation;

    private readonly float MOVE_SPEED = 3f;

    private List<string> MoveableTiles;
    private Dictionary<string, float> MoveDelay;

    private string LeftTile;
    private string RightTile;
    private string UpTile;
    private string DownTile;

    private void Start()
    {
        animator = GetComponent<Animator>();

        MoveableTiles = new List<string>();
        MoveableTiles.Add("plain");
        MoveableTiles.Add("trees");
        MoveableTiles.Add("hills");

        MoveDelay = new Dictionary<string, float>();
        MoveDelay.Add("plain", 0f);
        MoveDelay.Add("trees", 0f);
        MoveDelay.Add("hills", 0.1f);

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
            LeftTile = tilemap.GetTile(new Vector3Int(Mathf.FloorToInt(transform.position.x) - 1,
                                                      Mathf.FloorToInt(transform.position.y), 0)).name;
            RightTile = tilemap.GetTile(new Vector3Int(Mathf.FloorToInt(transform.position.x) + 1,
                                                       Mathf.FloorToInt(transform.position.y), 0)).name;
            UpTile = tilemap.GetTile(new Vector3Int(Mathf.FloorToInt(transform.position.x),
                                                    Mathf.FloorToInt(transform.position.y) + 1, 0)).name;
            DownTile = tilemap.GetTile(new Vector3Int(Mathf.FloorToInt(transform.position.x),
                                                      Mathf.FloorToInt(transform.position.y) - 1, 0)).name;

            if (horizontal_input == -1 && MoveableTiles.Contains(LeftTile))
            {
                moveLocation += new Vector2(-1, 0);
                animator.Play("WalkLeft");
            }
            else if (horizontal_input == 1 && MoveableTiles.Contains(RightTile))
            {
                moveLocation += new Vector2(1, 0);
                animator.Play("WalkRight");
            }
            else if (vertical_input == 1 && MoveableTiles.Contains(UpTile))
            {
                moveLocation += new Vector2(0, 1);
                animator.Play("WalkUp");
            }
            else if (vertical_input == -1 && MoveableTiles.Contains(DownTile))
            {
                moveLocation += new Vector2(0, -1);
                animator.Play("WalkDown");
            }
            else
            {
                return;
            }

            isMoving = true;
        }

        transform.position = Vector2.MoveTowards(transform.position, moveLocation, Time.deltaTime * MOVE_SPEED);

        if (Vector2.Distance(transform.position, moveLocation) < 0.01f)
        {
            transform.position = moveLocation;
            isMoving = false;
            float delayTime = MoveDelay[(tilemap.GetTile(new Vector3Int(Mathf.FloorToInt(transform.position.x),
                                                          Mathf.FloorToInt(transform.position.y), 0)).name)];
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
}
