using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface ITeleporter
{
    Tilemap GetTileMap();
    Vector2 GetLocation();
    Teleporter.Direction GetFacingDirection();
}
