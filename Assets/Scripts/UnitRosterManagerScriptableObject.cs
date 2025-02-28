using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(UnitRosterManagerScriptableObject), menuName = "ScriptableObjects/Unit Roster Manager")]
public class UnitRosterManagerScriptableObject : ScriptableObject {
    public HashSet<UnitComponent> UnitRoster { get; set; }
}