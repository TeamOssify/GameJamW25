using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovementRenderer : MonoBehaviour {
    private const int TILE_SIZE = 16;

    [SerializeField]
    [Range(0, 9)]
    [Tooltip("The number of tiles from the center tile (radius - 1)")]
    private int fieldRadius = 3;

    private StubSizedTileComponent _tileComponent;

    private Texture2D _texture1;
    private Texture2D _texture2;
    private Sprite _sprite1;
    private Sprite _sprite2;
    private readonly Dictionary<UnitComponent, (MoveSet FirstMove, MoveSet NormalMove)> _cachedMoves = new();

    private void Start() {
        if (_tileComponent) {
            Destroy(_tileComponent);
        }

        _tileComponent = gameObject.AddComponent<StubSizedTileComponent>();
        _tileComponent.GridRadius = fieldRadius;

        var tileCount = fieldRadius * 2 + 1;
        _texture1 = new Texture2D(TILE_SIZE * tileCount, TILE_SIZE * tileCount);
        _texture1.filterMode = FilterMode.Point;
        _sprite1 = Sprite.Create(_texture1, new Rect(0, 0, _texture1.width, _texture1.height), Vector2.zero);

        _texture2 = new Texture2D(TILE_SIZE * tileCount, TILE_SIZE * tileCount);
        _texture2.filterMode = FilterMode.Point;
        _sprite2 = Sprite.Create(_texture2, new Rect(0, 0, _texture2.width, _texture2.height), Vector2.zero);
    }

    public (Sprite FirstMove, Sprite NormalMove) RenderUnitMovement(UnitComponent unit) {
        // Debug.Log($"Rendering {unit.UnitName} moves to {_texture.width}x{_texture.height} texture.");

        var moveSet = GetOrGenerateMoveSet(unit);

        RenderMoveSet(moveSet.FirstMove, _texture1);
        RenderMoveSet(moveSet.NormalMove, _texture2);

        return (_sprite1, _sprite2);
    }

    private void RenderMoveSet(MoveSet moveSet, Texture2D texture) {
        var pixels = new Color32[TILE_SIZE * TILE_SIZE];

        for (var y = -fieldRadius; y <= fieldRadius; y++)
        for (var x = -fieldRadius; x <= fieldRadius; x++) {
            var x1 = x + fieldRadius;
            var y1 = y + fieldRadius;

            var background = ((x1 % 2) ^ (y1 % 2)) == 0 ? byte.MinValue : byte.MaxValue;
            Array.Fill(pixels, new Color32(background, background, background, 255));

            if (y == 0 && x == 0) {
                DrawSquare(TILE_SIZE, 2, pixels, 0x37, 0x7F, 0xFF);
            }
            else {
                var gridPos = new Vector3Int(x, y);

                if (moveSet.NormalMoves.Contains(gridPos)) {
                    DrawSquare(TILE_SIZE, 3, pixels, 0, 0xFF, 0x7F);
                }

                if (moveSet.JumpMoves.Contains(gridPos)) {
                    DrawLines(TILE_SIZE, 2, 2, pixels, 0xFF, 0x3F, 0xFF);
                }
            }

            texture.SetPixels32(x1 * TILE_SIZE, y1 * TILE_SIZE, TILE_SIZE, TILE_SIZE, pixels);
        }

        texture.Apply();
    }

    private (MoveSet FirstMove, MoveSet NormalMove) GetOrGenerateMoveSet(UnitComponent unit) {
        if (_cachedMoves.TryGetValue(unit, out var moveSet)) {
            return moveSet;
        }

        var unitObject = Instantiate(unit, new Vector3(1000, 1000, 1000), Quaternion.identity);

        var firstMove = unitObject.GetUnitMoves(_tileComponent, _ => false, _ => false);
        unitObject.Move(new Vector3(1000, 1000, 1000), Vector3Int.zero);
        var normalMove = unitObject.GetUnitMoves(_tileComponent, _ => false, _ => false);

        Destroy(unitObject.gameObject);

        moveSet = (firstMove, normalMove);
        _cachedMoves[unit] = moveSet;

        return moveSet;
    }

    private static void DrawSquare(int tileSize, int borderSize, Color32[] pixels, byte r, byte g, byte b) {
        for (var y = borderSize; y < tileSize - borderSize; y++) {
            Array.Fill(pixels, new Color32(r, g, b, 255), y * tileSize + borderSize, tileSize - borderSize * 2);
        }
    }

    private static void DrawLines(int tileSize, int borderX, int borderY, Color32[] pixels, byte r, byte g, byte b) {
        for (var y = borderY; y < tileSize - borderY; y++)
        for (var x = borderX; x < tileSize - borderX; x++) {
            if ((x + y) % 4 < 2) {
                pixels[y * tileSize + x] = new Color32(r, g, b, 255);
            }
        }
    }
}