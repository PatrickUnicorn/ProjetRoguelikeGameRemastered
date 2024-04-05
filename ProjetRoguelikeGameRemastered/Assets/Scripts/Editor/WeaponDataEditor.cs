using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

//https://docs.unity3d.com/Manual/editor-CustomEditors.html
[CustomEditor(typeof(WeaponData))]
public class WeaponDataEditor: Editor
{

    WeaponData weaponData;
    string[] weaponSubtypes;
    int selectedWeaponSubtype;

    void OnEnable()
    {
        // Met la valeur des données de l'arme dans la mémoire cache.
        weaponData = (WeaponData)target;
        // Récupère tous les sous-types d'arme et les met dans la mémoire cache.
        System.Type baseType = typeof(Weapon);
        List<System.Type> subTypes = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => baseType.IsAssignableFrom(p) && p != baseType)
            .ToList();

        List<string> subTypesString = subTypes.Select(t => t.Name).ToList();
        subTypesString.Insert(0, "None");
        weaponSubtypes = subTypesString.ToArray();

        selectedWeaponSubtype = Math.Max(0, Array.IndexOf(weaponSubtypes, weaponData.behaviour));
    }

    public override void OnInspectorGUI()
    {

        selectedWeaponSubtype = EditorGUILayout.Popup("Behaviour", Math.Max(0, selectedWeaponSubtype), weaponSubtypes);

        if (selectedWeaponSubtype > 0)
        {
            weaponData.behaviour = weaponSubtypes[selectedWeaponSubtype].ToString();
            //https://docs.unity3d.com/ScriptReference/EditorUtility.SetDirty.html
            EditorUtility.SetDirty(weaponData); 
            DrawDefaultInspector(); 
        }
    }
}
