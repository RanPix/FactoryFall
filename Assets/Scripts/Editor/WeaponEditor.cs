using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
    private Weapon m_Weapon;

    private void OnEnable()
    {
        m_Weapon = target as Weapon;
    }

    public override void OnInspectorGUI()
    {
        m_Weapon.shootType = (Weapon.ShootType)EditorGUILayout.EnumPopup("Weapon shoot type", m_Weapon.shootType);
        base.OnInspectorGUI();
        switch (m_Weapon.shootType)
        {
            case Weapon.ShootType.Physics:
                Debug.Log("lol");
                m_Weapon.weaponScriptableObject.bulletPrefab = EditorGUILayout.ObjectField("Element", m_Weapon.weaponScriptableObject.bulletPrefab, typeof(GameObject), true) as GameObject;
                break;

            case Weapon.ShootType.Ray:

                break;

            case Weapon.ShootType.Trigger:

                break;
        }
    }

}
