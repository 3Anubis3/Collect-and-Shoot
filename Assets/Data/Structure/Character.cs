using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data.Structure {

  public class Character { // 角色类
    private class ItemInventory { // 道具栏类
      private float _baseCapacity; // 基础容量
      private float _extraCapacity; // 附加容量
      private float _remainCapacity; // 当前剩余容量

      private Dictionary<Item, int> _items; // 内容物及对应数量

      public ItemInventory(float capacity) {
        _items = new Dictionary<Item, int>();
        _baseCapacity = capacity;
        _extraCapacity = 0;

        _remainCapacity = _baseCapacity;
      }

      public void SetExtraCapacity(float extraCapacity) {
        _remainCapacity -= _extraCapacity;
        _remainCapacity += extraCapacity;
        _extraCapacity = extraCapacity;
      }

      public void Clear() {
        _items = null;
      }

      public bool AddItem(Item item, int amount) {
        if (amount == 0 || item == null || item.GetCapacity() * amount > _remainCapacity) {
          return false;
        }
        KeyValuePair<Item, int> targetEntry = new KeyValuePair<Item, int>(null, 0);
        foreach (var pair in _items) {
          if (pair.Key.Equals(item)) {
            targetEntry = pair;
            break;
          }
        }

        if (targetEntry.Key == null) {
          if (amount > 0) {
            _items.Add(item, amount);
          }
          else {
            return false;
          }
        }
        else {
          if (targetEntry.Value + amount < 0) {
            return false;
          }

          _items.Remove(targetEntry.Key);
          if (targetEntry.Value + amount != 0) {
            _items.Add(targetEntry.Key, targetEntry.Value + amount);
          }
        }

        _remainCapacity -= item.GetCapacity() * amount;
        return true;
      }

      public Item GetItemByName(string name) {
        foreach (var pair in _items) {
          if (pair.Key.GetName() == name) {
            return pair.Key;
          }
        }
        return null;
      }

      public int GetNumber(string entryName) {
        foreach (var pair in _items) {
          if (pair.Key.GetName() == entryName) {
            return pair.Value;
          }
        }

        return -1;
      }

      public string[] GetNamesAndAmount() {
        string[] entries = new string[_items.Count];
        int index = 0;
        foreach (var item in _items) {
          entries[index++] = item.Key.GetName() + "_" + item.Value;
        }

        return entries;
      }

      public int GetAmountByName(string name) {
        foreach (var item in _items) {
          if (item.Key.GetName() == name) {
            return item.Value;
          }
        }

        return -1;
      }

      public Dictionary<Item, int> GetItems() {
        return _items;
      }
    }

    private class BodyInventory { // 装备栏类
      private PrimaryGun _primaryGunSlot1, _primaryGunSlot2; // 主武器槽位1、2
      private SubGun _subGunSlot; // 副武器槽
      private ThrowableWeapon _throwableWeaponSlot; // 投掷武器槽
      private CloseWeapon _closeWeaponSlot; // 近战武器槽
      private Helmet _helmetSlot; // 头盔槽
      private Armor _armorSlot; // 全身甲槽
      private Bag _bagSlot; // 背包槽

      public void Clear() {
        _primaryGunSlot1 = null;
        _primaryGunSlot2 = null;
        _subGunSlot = null;
        _throwableWeaponSlot = null;
        _closeWeaponSlot = null;
        _helmetSlot = null;
        _armorSlot = null;
        _bagSlot = null;
      }

      public PrimaryGun GetPrimaryGun(int slot) {
        return slot == 1 ? _primaryGunSlot1 : _primaryGunSlot2;
      }

      public void SetPrimaryGun(int slot, PrimaryGun primaryGun) {
        if (slot == 1) {
          _primaryGunSlot1 = primaryGun;
        }
        else {
          _primaryGunSlot2 = primaryGun;
        }
      }

      public SubGun GetSubGun() {
        return _subGunSlot;
      }

      public bool SetSubGun(SubGun subGun) {
        _subGunSlot = subGun;
        return true;
      }

      public bool SetThrowable(ThrowableWeapon weapon) {
        _throwableWeaponSlot = weapon;
        return true;
      }

      public float AddBag(Bag bag) {
        if (_bagSlot == null) {
          _bagSlot = bag;
          return bag.GetCapacity();
        }

        return -1;

      }

      public string[] GetStrings() {
        string[] names = new string[8];

        names[0] = _primaryGunSlot1 == null ? "Empty" : _primaryGunSlot1.GetName();
        names[1] = _primaryGunSlot2 == null ? "Empty" : _primaryGunSlot2.GetName();
        names[2] = _subGunSlot == null ? "Empty" : _subGunSlot.GetName();
        names[3] = _throwableWeaponSlot == null ? "Empty" : _throwableWeaponSlot.GetName();
        names[4] = _closeWeaponSlot == null ? "Empty" : _closeWeaponSlot.GetName();
        names[5] = _helmetSlot == null ? "Empty" : _helmetSlot.GetName();
        names[6] = _armorSlot == null ? "Empty" : _armorSlot.GetName();
        names[7] = _bagSlot == null ? "Empty" : _bagSlot.GetName();

        return names;
      }

      public bool SetClose(CloseWeapon weapon) {
        _closeWeaponSlot = weapon;
        return true;
      }

      public bool SetHelmet(Helmet helmet) {
        _helmetSlot = helmet;
        return true;
      }

      public bool SetArmor(Armor armor) {
        _armorSlot = armor;
        return true;
      }

      public bool SetBag(Bag bag) {
        _bagSlot = bag;
        return true;
      }
    }

    private bool _isAlive; // 存活标记 
    private readonly float _maxHP; // 最大血量
    private float _HP; // 当前血量
    private ItemInventory _itemInventory; // 物品栏
    private BodyInventory _bodyInventory; // 装备栏
    private int _activeSlot; // 当前武器栏

    public Character(float bagCapacity, float maxHP) {
      _isAlive = true;
      _maxHP = maxHP;
      _HP = _maxHP;
      _itemInventory = new ItemInventory(bagCapacity);
      _bodyInventory = new BodyInventory();
      _activeSlot = 1;
    }

    ~Character() {
      _isAlive = false;
      _HP = 0;
      _itemInventory.Clear();
      _bodyInventory.Clear();
    }

    public bool Shot(float damage) {
      if (damage < 0) {
        return false;
      }
      
      _HP -= damage;
      if (_HP <= 0) {
        _isAlive = false;
      }

      return true;
    }

    public bool IsAlive() {
      return _isAlive;
    }

    public void SetActiveSlot(int slot) {
      _activeSlot = slot;
    }

    public int GetActiveSlot() {
      return _activeSlot;
    }

    public bool AddAmmo(Ammo ammo, int number) {
      return _itemInventory.AddItem(ammo, number);
    }

    public void SetPrimaryGun(int index, PrimaryGun primaryGun) {
      if (index == 1) {
        _bodyInventory.SetPrimaryGun(1, primaryGun);
        return;
      }

      _bodyInventory.SetPrimaryGun(2, primaryGun);
    }

    public PrimaryGun GetPrimaryGun(int index) {
      return _bodyInventory.GetPrimaryGun(index);
    }

    public SubGun GetSubGun() {
      return _bodyInventory.GetSubGun();
    }

    public bool SetSubGun(SubGun subGun) {
      return _bodyInventory.SetSubGun(subGun);
    }

    public bool SetThrowable(ThrowableWeapon weapon) {
      return _bodyInventory.SetThrowable(weapon);
    }

    public Vector2Int[] GetEquippedAmmo() {
      Vector2Int[] ammos = new Vector2Int[3];

      if (_bodyInventory.GetPrimaryGun(1) == null) {
        ammos[0] = new Vector2Int(0, 0);
      }
      else {
        ammos[0] = new Vector2Int(_bodyInventory.GetPrimaryGun(1).GetRemainCapacity(), _itemInventory.GetNumber(_bodyInventory.GetPrimaryGun(1).GetSuitableAmmoName()));
      }

      if (_bodyInventory.GetPrimaryGun(2) == null) {
        ammos[1] = new Vector2Int(0, 0);
      }
      else {
        ammos[1] = new Vector2Int(_bodyInventory.GetPrimaryGun(2).GetRemainCapacity(), _itemInventory.GetNumber(_bodyInventory.GetPrimaryGun(2).GetSuitableAmmoName()));
      }

      if (_bodyInventory.GetSubGun() == null) {
        ammos[2] = new Vector2Int(0, 0);
      }
      else {
        ammos[2] = new Vector2Int(_bodyInventory.GetSubGun().GetRemainCapacity(), _itemInventory.GetNumber(_bodyInventory.GetSubGun().GetSuitableAmmoName()));
      }

      return ammos;
    }

    public Gun GetActiveGun() {
      switch (_activeSlot) {
        case 1:
          return GetPrimaryGun(1);
        case 2:
          return GetPrimaryGun(2);
        case 3:
          return GetSubGun();
        default:
          return null;
      }
    }

    public void RemoveAmmoByName(string name, int removedNumber) {
      _itemInventory.AddItem(_itemInventory.GetItemByName(name), -removedNumber);
    }

    public bool AddBag(Bag bag) {
      float addedCapacity = _bodyInventory.AddBag(bag);
      if (addedCapacity != -1) {
        _itemInventory.SetExtraCapacity(addedCapacity);
        return true;
      }

      return false;
    }

    public string[] GetBodySlotStrings() {
      return _bodyInventory.GetStrings();
    }

    public string[] GetItemNamesAndAmount() {
      return _itemInventory.GetNamesAndAmount();
    }

    public int GetItemAmountByName(string name) {
      return _itemInventory.GetAmountByName(name);
    }

    public Dictionary<Item, int> GetItems() {
      return _itemInventory.GetItems();
    }

    public bool SetClose(CloseWeapon weapon) {
      return _bodyInventory.SetClose(weapon);
    }

    public bool SetHelmet(Helmet helmet) {
      return _bodyInventory.SetHelmet(helmet);
    }

    public bool SetArmor(Armor armor) {
      return _bodyInventory.SetArmor(armor);
    }

    public bool SetBag(Bag bag) {
      _bodyInventory.SetBag(bag);
      _itemInventory.SetExtraCapacity(bag.GetCapacity());

      return true;
    }
  }
}