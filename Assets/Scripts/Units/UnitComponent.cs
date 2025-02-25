
using UnityEngine;

public enum UnitType {Pawn, Knight, Bishop, Rook, Queen, King, Barbarian, Duke}

public class UnitComponent : MonoBehaviour {
    public UnitType type;
    public Vector3Int position;

    private bool HasMoved;
    
    [SerializeField] private UnitHandler unitHandler;

    public void Select() {
        DisplayValidMoves();
    }

    public void Deselect() {
        
    }

    public bool IsValidMove(Vector3Int gridPosition) {
        Debug.Log("Yo");
        return false;
    }

    public void Move(Vector3Int gridPosition) {
        //if the move is valid (do later lule)
        transform.position = unitHandler.GetWorldPositionFromGrid(gridPosition);
        HasMoved = true;
    }

    private void GetValidMoves() {
        
    }

    private void DisplayValidMoves() {
        
    }

    private void ClearValidMoves() {
        
    }

    void Start() {
        
    }
}