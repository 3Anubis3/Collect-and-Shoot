namespace Data.Structure {
  public abstract class Gun { // 枪械基类
    protected string _name;
    protected class AccessoryInventory { // 配件槽类
      private AimingAccessory _aimingAccessory; // 瞄具槽
      private MuzzleAccessory _muzzleAccessory; // 枪口槽
      private MagazineAccessory _magazineAccessory; // 弹夹槽
      private GunBackAccessory _gunBackAccessory; // 枪托槽
    }
    
    protected int _baseCapacity, _remainCapacity; // 基础装弹量、剩余装弹量
    protected float _baseReloadTime; // 基础重装时间
    protected string _suitableAmmoName; // 可使用弹种
    protected float _baseDamage, _baseDiffusion, _baseRecoil, _baseSpeed; //基础伤害、基础扩散、基础后坐力、基础射速
    protected AccessoryInventory _accessoryInventory; // 配件槽

    public string GetSuitableAmmoName() {
      return _suitableAmmoName;
    }

    public int GetRemainCapacity() {
      return _remainCapacity;
    }

    public string GetName() {
      return _name;
    }

    public float GetBaseReloadTime() {
      return _baseReloadTime;
    }

    public float GetBaseRecoilTime() {
      return 60 / _baseSpeed;
    }

    public float GetBaseRecoil() {
      return _baseRecoil;
    }

    public int GetBaseCapacity() {
      return _baseCapacity;
    }

    public float GetBaseDamage() {
      return _baseDamage;
    }

    public bool Shot() {
      if (_remainCapacity > 0) {
        _remainCapacity--;
        return true;
      }
      return false;
    }

    public void SetRemainCapacity(int remainCapacity) {
      _remainCapacity = remainCapacity;
    }
  }

  public class PrimaryGun : Gun { // 主武器类

    public PrimaryGun(string name, int baseCapacity, float baseReloadTime, string suitableAmmoName, float baseDamage, float baseDiffusion
      , float baseRecoil, float baseSpeed) {
      _accessoryInventory = new AccessoryInventory();

      _name = name;
      _baseCapacity = baseCapacity;
      _baseReloadTime = baseReloadTime;
      _suitableAmmoName = suitableAmmoName;
      _baseDamage = baseDamage;
      _baseDiffusion = baseDiffusion;
      _baseRecoil = baseRecoil;
      _baseSpeed = baseSpeed;
    }
  }

  public class SubGun : Gun { // 副武器类
    public SubGun(string name, int baseCapacity, float baseReloadTime, string suitableAmmoName, float baseDamage, float baseDiffusion
                      , float baseRecoil, float baseSpeed) {
      _accessoryInventory = new AccessoryInventory();

      _name = name;
      _baseCapacity = baseCapacity;
      _baseReloadTime = baseReloadTime;
      _suitableAmmoName = suitableAmmoName;
      _baseDamage = baseDamage;
      _baseDiffusion = baseDiffusion;
      _baseRecoil = baseRecoil;
      _baseSpeed = baseSpeed;
    }
  }

  public class CloseWeapon { // 近身武器
    private float _distance; // 作用距离
    private float _damage; // 伤害

    private string _name;

    public string GetName() {
      return _name;
    }
  }

  public class ThrowableWeapon { // 投掷武器
    private float _distance; // 作用半径
    private float _damage; // 伤害
    private string _name;

    public string GetName() {
      return _name;
    }
  }
}