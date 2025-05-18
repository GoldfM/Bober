using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Weapon meleeWeaponSlot;
    public Weapon rangedWeaponSlot;
    public Weapon currentWeapon;
    private Animator currentWeaponAnimator; // Ссылка на аниматор текущего оружия

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
                break;
            case WeaponType.Ranged:
                if (rangedWeaponSlot != null)
                {
                    DropWeapon(rangedWeaponSlot);
                }
                rangedWeaponSlot = weapon;
                break;
        }
        EquipWeapon(weapon);
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (weapon != null)
        {
            if (currentWeapon != null)
            {
                // Сбрасываем состояние текущего оружия перед переключением
                ResetWeaponState(currentWeapon);
                currentWeapon.gameObject.SetActive(false);
            }
            currentWeapon = weapon;
            currentWeapon.gameObject.SetActive(true);
            currentWeapon.transform.rotation = Quaternion.identity;

            currentWeaponAnimator = currentWeapon.GetComponent<Animator>();// Получаем ссылку на аниматор

            if (weapon.Type == WeaponType.Melee)
            {
                weapon.transform.GetComponent<Collider2D>().enabled = false;
            }
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

    // Функция для сброса состояния оружия (анимации и коллайдера)
    private void ResetWeaponState(Weapon weapon)
    {
        if (weapon != null)
        {
            Collider2D weaponCollider = weapon.GetComponent<Collider2D>();
            if (weaponCollider != null)
            {
                weaponCollider.enabled = false; // Выключаем коллайдер
            }

            Animator weaponAnimator = weapon.GetComponent<Animator>();
            if (weaponAnimator != null)
            {
                weaponAnimator.Rebind(); // Перезапускаем аниматор
                weaponAnimator.Update(0f); // Прокручиваем аниматор в начальное состояние
            }
        }
    }
}