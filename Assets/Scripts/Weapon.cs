using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public string weaponName;
    public Sprite icon;
    public abstract WeaponType Type { get; }
    public GameObject pickupPrefab;
    public abstract void Attack(); // Абстрактный метод для атаки

    //public virtual void OnEquip() { } // Виртуальный метод для экипировки (можно переопределить в подклассах)
    //public virtual void OnUnequip() { } // Виртуальный метод для снятия экипировки
}

public enum WeaponType { Melee, Ranged }
