namespace Data.Structure {
  public abstract class Equipment { // 护甲类
    protected float Durability; // 耐久值
    protected float Remission; // 伤害减免

    protected string _name;

    public string GetName() {
      return _name;
    }
  }

  public class Helmet : Equipment { // 头盔类

  }

  public class Armor : Equipment { // 护甲类

  }

  public class Bag { // 背包类
    private string _name;
    private float _extraCapacity; // 提供额外容量

    public Bag(string name, float capacity) {
      _name = name;
      _extraCapacity = capacity;
    }

    public float GetCapacity() {
      return _extraCapacity;
    }

    public string GetName() {
      return _name;
    }
  }
}