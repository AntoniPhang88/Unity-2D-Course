using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string ID;
    public ItemType itemType;
    public float damage;
    public float shootCooldown;
    public bool isAutomatic;

    [Header("Ammo")]
    public int currentAmmo;
    public int maxAmmo;
    public int storageAmmo;

    [Header("Reload")]
    public float reloadTime;
    public bool isReloading;

    [Header("Recoil")]
    public float recoilStrength;
    public float recoilTime; // its needs to be less than "shootCooldown"


    [Header("Reference")]
    public Transform shootingPoint;
    public Transform shellSpawnPoint;
    public GameObject shellPrefab;
    public GameObject effectPrefab;
    public Sprite weaponIconSprite;

    public float visibleLineTime;

    [SerializeField]
    private WeaponData weaponData = new WeaponData();
    private ShootEffect shootingEffect;

    public void PlayShootEffect()
    {
        if (effectPrefab == null || shootingPoint == null)
            return;

        if (shootingEffect == null)
        {
            GameObject effectInstance = Instantiate(effectPrefab, shootingPoint);
            effectInstance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            shootingEffect = effectInstance.GetComponent<ShootEffect>();
        }

        if (shootingEffect != null)
            shootingEffect.Play();
    }

    private void OnDisable()
    {
        if (shootingEffect != null)
            shootingEffect.DisableEffect();
    }

    public bool ReloadCheck()
    {
        int neededAmmo = maxAmmo - currentAmmo;
        if(neededAmmo <= 0 || storageAmmo <= 0)
            return false;
        return true;
    }
    public void Reload()
    {
        int neededAmmo = maxAmmo - currentAmmo;
        int ammoToReload = Mathf.Min(neededAmmo, storageAmmo);
        currentAmmo += ammoToReload;
        storageAmmo -= ammoToReload;
        isReloading = false;
    }
    public void SaveWeaponData()
    {
        weaponData.ID = ID;
        weaponData.currentAmmo = currentAmmo;
        weaponData.storageAmmo = storageAmmo;
        SaveLoadManager.instance.Save(weaponData, SaveLoadManager.instance.folderName, ID + ".json");
    }
    public void LoadWeaponData()
    {
        SaveLoadManager.instance.Load(weaponData, SaveLoadManager.instance.folderName, ID + ".json");
        if(weaponData.ID != "")
        {
            currentAmmo = weaponData.currentAmmo;
            storageAmmo = weaponData.storageAmmo;
        }
    }
}
