using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameWeapon))]
class WeaponInitializeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameWeapon weapon = (GameWeapon)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Reinit Weapon"))
        {
            weapon.InitializeWeapon();
        }
    }
}