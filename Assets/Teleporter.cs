using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Teleporter : MonoBehaviour, ITeleporter
{
    public enum Direction
    {
        LEFT, RIGHT, UP, DOWN
    }

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private Vector2 location;

    [SerializeField]
    private Direction facingDirection;

    public Tilemap GetTileMap()
    {
        return tilemap;
    }

    public Vector2 GetLocation()
    {
        return location;
    }

    public Direction GetFacingDirection()
    {
        return facingDirection;
    }
}
