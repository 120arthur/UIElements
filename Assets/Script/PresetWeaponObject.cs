using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PresetWeaponObject : MonoBehaviour
{
    [SerializeField]
    public Sprite emptyWeapon;

    public int appliedPresetWeapon = 0;

    [Header("Preset List")]
    public List<PresetWeapon> presetsWeapon;

     private GameObject weapon;
     private Transform transform;
     private String weaponName;
     private Sprite weaponImage;
     public PresetWeapon CurrentlyEditing;

    public void ApplyPreset(PresetWeapon preset)
    {
        weapon = preset.weapon;
        transform = preset.weaponTansform.transform;
        weaponName = preset.weaponName;
        weaponImage = preset.weaponImage;
    }

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

    public void Update()
    {
        if (presetsWeapon.Count <= 0)
        {
            return;
        }

        if (presetsWeapon[appliedPresetWeapon] != null)
        {
            ApplyPreset(presetsWeapon[appliedPresetWeapon]);
        }

    }
}
