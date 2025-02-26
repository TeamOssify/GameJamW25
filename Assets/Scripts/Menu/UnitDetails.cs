using TMPro;
using UnityEngine;

public class UnitDetails : MonoBehaviour {
    public string UnitName => unitName;

    [SerializeField]
    private string unitName;

    [SerializeField]
    private TextMeshPro unitNameText;

    private void OnMouseEnter() {
        unitNameText.text = unitName;
    }
}