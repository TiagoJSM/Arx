using ArxGame.Components.Weapons;
using CommonInterfaces.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum WeaponSocket
{
    Weapon1,
    Weapon2,
    Weapon3,
    Weapon4,
}

public class EquipmentController : MonoBehaviour
{
    private GameObject _equippedWeaponVisual;
    private IWeapon _equippedWeapon;

    [SerializeField]
    private BaseWeapon _weapon1;
    [SerializeField]
    private BaseWeapon _weapon2;
    [SerializeField]
    private BaseWeapon _weapon3;
    [SerializeField]
    private BaseWeapon _weapon4;
    [SerializeField]
    private WeaponSocket _equippedWeaponIndex;
    [SerializeField]
    private GameObject _weaponSocket;

    public WeaponSocket ActiveWeaponSocket
    {
        get
        {
            return _equippedWeaponIndex;
        }
        set
        {
            if (_equippedWeaponIndex == value)
            {
                return;
            }
            _equippedWeaponIndex = value;
            EquipWeaponFromActiveSocket();
        }
    }

    public IWeapon EquippedWeapon
    {
        get
        {
            return _equippedWeapon;
        }
    }

    void Awake()
    {
        EquipWeaponFromActiveSocket();
    }

    private void EquipWeapon(IWeapon weaponObject)
    {
        if (_equippedWeaponVisual != null)
        {
            Destroy(_equippedWeaponVisual);
            _equippedWeapon.Unequipped();
        }

        _equippedWeapon = UnityEngine.Object.Instantiate(weaponObject as UnityEngine.Object) as IWeapon;
        _equippedWeapon.RightHandSocket = _weaponSocket;
        _equippedWeaponVisual = Instantiate(_equippedWeapon.RightHandWeapon);
        _equippedWeaponVisual.transform.SetParent(_weaponSocket.transform, false);
        _equippedWeapon.Equipped();
    }

    private IWeapon GetWeaponAt(WeaponSocket socket)
    {
        switch (socket)
        {
            case WeaponSocket.Weapon1: return _weapon1;
            case WeaponSocket.Weapon2: return _weapon2;
            case WeaponSocket.Weapon3: return _weapon3;
            case WeaponSocket.Weapon4: return _weapon4;
        }
        return null;
    }

    private void EquipWeaponFromActiveSocket()
    {
        var weapon = GetWeaponAt(_equippedWeaponIndex);
        if (weapon != null)
        {
            EquipWeapon(weapon);
        }
    }
}