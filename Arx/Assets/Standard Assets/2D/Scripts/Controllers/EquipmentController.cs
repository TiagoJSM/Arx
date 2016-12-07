using ArxGame.Components.Weapons;
using CommonInterfaces.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum WeaponSocket
{
    ClosedCombarWeapon1,
    ClosedCombatWeapon2,
}

public class EquipmentController : MonoBehaviour
{
    private GameObject _equippedCloseCombatWeaponVisual;
    private GameObject _equippedShooterWeaponVisual;
    private GameObject _equippedThrowWeaponVisual;

    private ICloseCombatWeapon _equippedCloseCombatWeapon;
    private IShooterWeapon _equippedShooterWeapon;
    private ChainThrow _equippedChainThrowWeapon;

    [SerializeField]
    private BaseCloseCombatWeapon _closeCombatWeapon1;
    [SerializeField]
    private BaseCloseCombatWeapon _closeCombatWeapon2;
    [SerializeField]
    private Shooter _shooter;
    [SerializeField]
    private ChainThrow _chainThrow;
    [SerializeField]
    private WeaponSocket _equippedWeaponIndex;
    [SerializeField]
    private GameObject _weaponSocket;

    public WeaponSocket ActiveCloseCombatSocket
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
            EquipCloseCombatWeapon();
        }
    }

    public ICloseCombatWeapon EquippedCloseCombatWeapon
    {
        get
        {
            return _equippedCloseCombatWeapon;
        }
    }

    public IShooterWeapon EquippedShooterWeapon
    {
        get
        {
            return _equippedShooterWeapon;
        }
    }

    public ChainThrow EquippedChainThrowWeapon
    {
        get
        {
            return _equippedChainThrowWeapon;
        }
    }

    void Awake()
    {
        EquipCloseCombatWeapon();
        EquipShooterWeapon(_shooter);
        EquipChainThrowWeapon(_chainThrow);
    }

    private void EquipCloseCombatWeapon(ICloseCombatWeapon weaponObject)
    {
        _equippedCloseCombatWeapon = EquipWeapon(weaponObject, _equippedCloseCombatWeapon, ref _equippedCloseCombatWeaponVisual);
    }

    private void EquipShooterWeapon(IShooterWeapon weaponObject)
    {
        _equippedShooterWeapon = EquipWeapon(weaponObject, _equippedShooterWeapon, ref _equippedShooterWeaponVisual);
    }

    private void EquipChainThrowWeapon(ChainThrow weaponObject)
    {
        _equippedChainThrowWeapon = EquipWeapon(weaponObject, _equippedChainThrowWeapon, ref _equippedThrowWeaponVisual);
    }

    private TWeapon EquipWeapon<TWeapon>(TWeapon weaponObject, TWeapon equipedWeapon, ref GameObject weaponVisual) 
        where TWeapon : class, IWeapon
    {
        if (weaponVisual != null)
        {
            Destroy(weaponVisual);
            equipedWeapon.Unequipped();
        }

        equipedWeapon = UnityEngine.Object.Instantiate(weaponObject as UnityEngine.Object) as TWeapon;
        equipedWeapon.RightHandSocket = _weaponSocket;
        weaponVisual = Instantiate(equipedWeapon.RightHandWeapon);
        weaponVisual.transform.SetParent(_weaponSocket.transform, false);
        equipedWeapon.Equipped();

        return equipedWeapon;
    }

    private ICloseCombatWeapon GetWeaponAt(WeaponSocket socket)
    {
        switch (socket)
        {
            case WeaponSocket.ClosedCombarWeapon1: return _closeCombatWeapon1;
            case WeaponSocket.ClosedCombatWeapon2: return _closeCombatWeapon2;
        }
        return null;
    }

    private void EquipCloseCombatWeapon()
    {
        var weapon = GetWeaponAt(_equippedWeaponIndex);
        if (weapon != null)
        {
            EquipCloseCombatWeapon(weapon);
        }
    }
}