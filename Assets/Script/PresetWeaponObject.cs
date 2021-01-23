using System.Collections.Generic;
using UnityEngine;

public class PresetWeaponObject : MonoBehaviour
{
    [SerializeField]
    //Defalut image that will appear in the container
    public Sprite emptyWeapon;

    [Header("Preset List")]
    public List<PresetWeapon> presetsWeapon;
    public PresetWeapon CurrentlyEditing;

    public void InstantiateWeapon()
    {
        if (presetsWeapon.Count <= 0)
        {
            return;
        }
        else
        {
            Instantiate(CurrentlyEditing.weapon, CurrentlyEditing.weaponTansform.transform);

        }
    }


}
