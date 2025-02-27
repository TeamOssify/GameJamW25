using System;
using System.Linq;
using UnityEngine;

public class UnitMovementRenderer : MonoBehaviour {
    private const int TILE_SIZE = 16;

    [SerializeField]
    [Range(0, 9)]
    [Tooltip("The number of tiles from the center tile (radius - 1)")]
    private int fieldRadius = 3;

    private StubSizedTileComponent _tileComponent;

    private Texture2D _texture;
    private Sprite _sprite;

    private void Start() {
        if (_tileComponent) {
            Destroy(_tileComponent);
        }

        _tileComponent = gameObject.AddComponent<StubSizedTileComponent>();
        _tileComponent.GridRadius = fieldRadius;

        var tileCount = fieldRadius * 2 + 1;
        _texture = new Texture2D(TILE_SIZE * tileCount, TILE_SIZE * tileCount);
        _texture.filterMode = FilterMode.Point;
        _sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.zero);
    }

    public Sprite RenderUnitMovement(UnitComponent unit) {
        Debug.Log($"Rendering {unit.UnitName} moves to {_texture.width}x{_texture.height} texture.");

        var moveSet = unit.GetUnitMoves(_tileComponent, _ => false, _ => false);

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

                // TODO: Find out how to calculate move sets. Might need to instantiate or rewrite
                if (moveSet.NormalMoves.Contains(gridPos)) {
                    DrawSquare(TILE_SIZE, 3, pixels, 0, byte.MaxValue, byte.MaxValue);
                }

                if (moveSet.JumpMoves.Contains(gridPos)) {
                    DrawLines(TILE_SIZE, 2, 2, pixels, byte.MaxValue, byte.MaxValue / 4, byte.MaxValue);
                }
            }

            _texture.SetPixels32(x1 * TILE_SIZE, y1 * TILE_SIZE, TILE_SIZE, TILE_SIZE, pixels);
        }

        _texture.Apply();

        return _sprite;
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