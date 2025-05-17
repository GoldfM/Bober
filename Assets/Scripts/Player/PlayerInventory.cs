using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Weapon meleeWeaponSlot;
    public Weapon rangedWeaponSlot;
    public Weapon currentWeapon;

    void Start()
    {
        EquipWeapon(meleeWeaponSlot);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchWeapon();
        }
    }

    public void AddWeapon(Weapon weaponPrefab)
    {
        Weapon weapon = Instantiate(weaponPrefab);
        weapon.transform.SetParent(transform);
        weapon.transform.localPosition = Vector3.zero;
        weapon.gameObject.SetActive(false);

        switch (weapon.Type)
        {
            case WeaponType.Melee:
                if (meleeWeaponSlot != null)
                {
                    DropWeapon(meleeWeaponSlot);
                }
                meleeWeaponSlot = weapon;
                EquipWeapon(meleeWeaponSlot);
                break;
            case WeaponType.Ranged:
                 weapon.transform.position = rangedWeaponSlot.transform.position;
                if (rangedWeaponSlot != null)
                {
                    DropWeapon(rangedWeaponSlot);
                }
                rangedWeaponSlot = weapon;
                EquipWeapon(rangedWeaponSlot);
                break;
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (weapon != null)
        {
            if (currentWeapon != null)
            {
                currentWeapon.gameObject.SetActive(false);
            }
            currentWeapon = weapon;
            currentWeapon.gameObject.SetActive(true);
            currentWeapon.transform.rotation = Quaternion.identity;
        }
    }

    void SwitchWeapon()
    {
        if (currentWeapon != null)
        {
            if (currentWeapon.Type == WeaponType.Melee && rangedWeaponSlot != null)
            {
                EquipWeapon(rangedWeaponSlot);
            }
            else if (currentWeapon.Type == WeaponType.Ranged && meleeWeaponSlot != null)
            {
                EquipWeapon(meleeWeaponSlot);
            }
        }
    }

    void DropWeapon(Weapon weapon)
    {
        if (weapon != null)
        {
            weapon.transform.SetParent(null);
            weapon.gameObject.SetActive(true);
            Destroy(weapon.gameObject);
        }
    }
}