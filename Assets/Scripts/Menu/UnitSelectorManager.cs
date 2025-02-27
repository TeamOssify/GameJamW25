using UnityEngine;

public class UnitSelectorManager : MonoBehaviour {
    [SerializeField]
    private UnitDetails selectorPrefab;

    [SerializeField]
    private UnitComponent[] units;

    [SerializeField]
    private MainMenu mainMenu;

    private void Awake() {
        foreach (var unit in units) {
            var obj = Instantiate(selectorPrefab, transform.position, Quaternion.identity, transform);
            obj.Init(unit, mainMenu);
        }
    }
}