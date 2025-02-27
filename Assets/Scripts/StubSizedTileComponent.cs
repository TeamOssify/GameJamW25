using System;
using UnityEngine;

public class StubSizedTileComponent : TileComponent {
    public int GridRadius { get; set; }

    public override bool IsValidTile(Vector3Int pos) {
        return Math.Abs(pos.x) <= GridRadius && Math.Abs(pos.y) <= GridRadius;
    }

    public override bool IsUnobstructedTile(Vector3Int pos) {
        return IsValidTile(pos);
    }
}