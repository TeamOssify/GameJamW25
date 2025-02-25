using UnityEngine;
using System.Collections.Generic;


public class UnitMovement {
    public Vector2Int Direction;
    public int MaxDistance = 1;
    public bool FirstMoveOnly = false;
    public bool CaptureOnly = false;
        
}

public class UnitMoveData {
    private Dictionary<UnitType, List<UnitMovement>> _unitMovements = new Dictionary<UnitType, List<UnitMovement>> {
        {
            UnitType.Pawn, new List<UnitMovement> {
                new UnitMovement { Direction = new Vector2Int(0, 1) },
                new UnitMovement { Direction = new Vector2Int(0, 2), FirstMoveOnly = true },
                new UnitMovement { Direction = new Vector2Int(1, 1), CaptureOnly = true },
                new UnitMovement { Direction = new Vector2Int(-1, 1), CaptureOnly = true },
            }
        }, {
            UnitType.Knight, new List<UnitMovement> {
                new UnitMovement { Direction = new Vector2Int(1, 2) },
                new UnitMovement { Direction = new Vector2Int(2, 1) },
                new UnitMovement { Direction = new Vector2Int(2, -1) },
                new UnitMovement { Direction = new Vector2Int(1, -2) },
                new UnitMovement { Direction = new Vector2Int(-1, -2) },
                new UnitMovement { Direction = new Vector2Int(-2, -1) },
                new UnitMovement { Direction = new Vector2Int(-2, 1) },
                new UnitMovement { Direction = new Vector2Int(-1, 2) }
            }
        }, {
            UnitType.Bishop, new List<UnitMovement> {
                new UnitMovement { Direction = new Vector2Int(1, 1), MaxDistance = -1 },
                new UnitMovement { Direction = new Vector2Int(1, -1), MaxDistance = -1 },
                new UnitMovement { Direction = new Vector2Int(-1, -1), MaxDistance = -1 },
                new UnitMovement { Direction = new Vector2Int(-1, 1), MaxDistance = -1 }
            }
        }, {
            UnitType.Rook, new List<UnitMovement> {
                new UnitMovement { Direction = new Vector2Int(0, 1), MaxDistance = -1 },
                new UnitMovement { Direction = new Vector2Int(1, 0), MaxDistance = -1 },
                new UnitMovement { Direction = new Vector2Int(0, -1), MaxDistance = -1 },
                new UnitMovement { Direction = new Vector2Int(-1, 0), MaxDistance = -1 }
            }
        },
        { UnitType.Queen, new List<UnitMovement>() }, {
            UnitType.King, new List<UnitMovement> {
                new UnitMovement { Direction = new Vector2Int(0, 1) },
                new UnitMovement { Direction = new Vector2Int(1, 1) },
                new UnitMovement { Direction = new Vector2Int(1, 0) },
                new UnitMovement { Direction = new Vector2Int(1, -1) },
                new UnitMovement { Direction = new Vector2Int(0, -1) },
                new UnitMovement { Direction = new Vector2Int(-1, -1) },
                new UnitMovement { Direction = new Vector2Int(-1, 0) },
                new UnitMovement { Direction = new Vector2Int(-1, 1) }
            }
        },
        { UnitType.Barbarian, new List<UnitMovement>() },
        { UnitType.Duke, new List<UnitMovement>() }
    };


}