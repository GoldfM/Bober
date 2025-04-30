using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Weapon meleeWeaponSlot;
    public Weapon rangedWeaponSlot;
    public Weapon currentWeapon;
    //private Vector3 FixScale = new Vector3((float)0.5,(float)0.5,0);
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

        if (Input.GetMouseButtonDown(0) && currentWeapon != null)
        {
            currentWeapon.Attack();
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
                weapon.transform.position = meleeWeaponSlot.transform.position;
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

    void EquipWeapon(Weapon weapon)
    {
        if (weapon != null)
        {
            if (currentWeapon != null)
            {
                currentWeapon.gameObject.SetActive(false);
                //currentWeapon.OnUnequip();
            }
            currentWeapon = weapon;
            currentWeapon.gameObject.SetActive(true);
           // currentWeapon.transform.localScale=FixScale;
            //currentWeapon.OnEquip();
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
            //weapon.OnUnequip();
            weapon.transform.SetParent(null);
            weapon.gameObject.SetActive(true);
            //добавляем скрипт подбора
            if (weapon.pickupPrefab != null)
            {
                GameObject droppedWeapon = Instantiate(weapon.pickupPrefab, transform.position, Quaternion.identity);
            }
            Destroy(weapon.gameObject);
        }
    }
}
