using System;
using UnityEngine;

public class UnitMovementRenderer : MonoBehaviour {
    [SerializeField]
    [Range(0, 9)]
    [Tooltip("The number of tiles from the center tile (radius - 1)")]
    private int fieldRadius = 3;

    private StubSizedTileComponent _tileComponent;

    private void Start() {
        if (_tileComponent) {
            Destroy(_tileComponent);
        }

        _tileComponent = gameObject.AddComponent<StubSizedTileComponent>();
        _tileComponent.GridRadius = fieldRadius;
    }

    public Texture2D RenderUnitMovement(UnitComponent unit) {
        var moveSet = unit.GetUnitMoves(_tileComponent, _ => true, _ => true);

        const int TILE_SIZE = 8;
        var tileCount = fieldRadius * 2 + 1;
        var texture = new Texture2D(TILE_SIZE * tileCount, TILE_SIZE * tileCount);

        var pixels = new Color32[TILE_SIZE * TILE_SIZE];

        for (var y = -fieldRadius; y <= fieldRadius; y++)
        for (var x = -fieldRadius; x <= fieldRadius; x++) {
            var vec = new Vector3Int(x, y);

            var background = ((x % 2) ^ (y % 2)) == 0 ? byte.MinValue : byte.MaxValue;
            Array.Fill(pixels, new Color32(background, background, background, 255));

            if (y == 0 && x == 0) {
                DrawSquare(TILE_SIZE, pixels, 0x37, 0x7F, 0xFF);
            }
            else {
                if (moveSet.NormalMoves.Contains(vec)) {
                    DrawSquare(TILE_SIZE, pixels, 0, byte.MaxValue, byte.MaxValue);
                }

                if (moveSet.JumpMoves.Contains(vec)) {
                    DrawLines(TILE_SIZE, pixels, byte.MaxValue, byte.MaxValue / 4, byte.MaxValue);
                }
            }

            texture.SetPixels32(x * TILE_SIZE, y * TILE_SIZE, TILE_SIZE, TILE_SIZE, pixels);
        }

        return texture;
    }

    private static void DrawSquare(int tileSize, Color32[] pixels, byte r, byte g, byte b) {
        for (var y = 1; y < tileSize - 1; y++) {
            Array.Fill(pixels, new Color32(r, g, b, 255), y * tileSize + 1, tileSize - 2);
        }
    }

    private static void DrawLines(int tileSize, Color32[] pixels, byte r, byte g, byte b) {
        for (var y = 0; y < tileSize; y++) {
            if (y % 2 == 0) {
                continue;
            }

            Array.Fill(pixels, new Color32(r, g, b, 255), y * tileSize + 1, tileSize - 2);
        }
    }
}