namespace Data.Structure {
  public abstract class Item { // 物品基类
    protected string _name;
    protected float _capacity;
    protected Type Type;

    public Item() {
      _name = "NULL";
      _capacity = 1;
    }

    public bool Equals(Item item) {
      return GetHashCode() == item.GetHashCode();
    }

    public override int GetHashCode() {
      return _name.GetHashCode();
    }

    public string GetName() {
      return _name;
    }

    public float GetCapacity() {
      return _capacity;
    }

    public new abstract Type GetType();
  }

  public class Ammo : Item { // 弹药类

    public Ammo(string name, float capacity) {
      _name = name;
      _capacity = capacity;
      Type = Type.AMMO;
    }

    public override Type GetType() {
      return Type;
    }
  }

  public abstract class Accessory : Item { // 武器配件类

  }

  public class AimingAccessory : Accessory { // 瞄具类
    public override Type GetType() {
      return Type;
    }
  }

  public class MuzzleAccessory : Accessory { // 枪口类
    public override Type GetType() {
      return Type;
    }
  }

  public class MagazineAccessory : Accessory { // 弹夹类
    public override Type GetType() {
      return Type;
    }
  }

  public class GunBackAccessory : Accessory { // 枪托类
    public override Type GetType() {
      return Type;
    }
  }
}