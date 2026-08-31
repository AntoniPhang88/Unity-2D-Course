using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [Header("Reference")]
    public InputActionReference shootActionRef;
    public Weapon currentWeapon;
    private Player player;

    private ItemType currentWeaponType;
    private bool shootButtonHeld;
    private bool shootCooldownOver = true;

    [Header("Raycast Stuff")]
    [SerializeField] private LayerMask whatToHit;
    [SerializeField] private LineRenderer lineRender;
    private bool isShootLineActive = false;
    private Vector3 startPoint;
    private Vector3 endPoint;

    public static Action<Sprite, int, int, int> OnUpdateAllInfo;
    public static Action<int, int, int> OnUpdateAmmo;
    private void Awake()
    {
        player = GetComponent<Player>();
    }

    void Start()
    {
        currentWeapon = player.currentWeaponPrefab.GetComponent<Weapon>();
        OnUpdateAllInfo?.Invoke(currentWeapon.weaponIconSprite, currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);
    }

    private void OnEnable()
    {
        shootActionRef.action.performed += TryToShoot;
        shootActionRef.action.canceled += StopShooting;
    }

    private void OnDisable()
    {
        shootActionRef.action.performed -= TryToShoot;
        shootActionRef.action.canceled -= StopShooting;
    }

    private void TryToShoot(InputAction.CallbackContext value)
    {
        if (currentWeapon == null || player.stateMachine.currentState == PlayerStates.State.Ladders ||
            player.stateMachine.currentState == PlayerStates.State.Dash ||
            player.stateMachine.currentState == PlayerStates.State.WallSlide ||
            player.stateMachine.currentState == PlayerStates.State.KnockBack)
            return;
        if (shootButtonHeld || shootCooldownOver == false)
            return;

        if(currentWeapon.isAutomatic)
        {
            shootButtonHeld = true;
            return;
        }
        shootButtonHeld = true;
        Shoot();
    }

    private void StopShooting(InputAction.CallbackContext value)
    {
        shootButtonHeld = false;
    }

    private void Shoot()
    {
        if (currentWeapon.currentAmmo <= 0)
            return;
        lineRender.positionCount = 2;
        Vector3 direction = currentWeapon.shootingPoint.right;
        RaycastHit2D hitInfo = Physics2D.Raycast(currentWeapon.shootingPoint.position, direction, Mathf.Infinity, whatToHit);
        if(hitInfo)
        {
            startPoint = currentWeapon.shootingPoint.position;
            endPoint = hitInfo.point;
            //lineRender.SetPosition(0, startPoint);
            //lineRender.SetPosition(1, endPoint);
            IDamageable damageableObject = hitInfo.collider.GetComponent<IDamageable>();
            if(damageableObject != null)
            {
                damageableObject.TakeDamage(currentWeapon.damage);
            }

            Debug.Log("We Hit Something");
        }
        else
        {
            startPoint = currentWeapon.shootingPoint.position;
            endPoint = currentWeapon.shootingPoint.position + direction * 10;
            //lineRender.SetPosition(0, startPoint);
            //lineRender.SetPosition(1, endPoint);
            Debug.Log("We hit Nothing");
        }
        currentWeapon.currentAmmo -= 1;
        StartCoroutine(ShootDelay());
        StartCoroutine(ResetShootingLine());
        OnUpdateAmmo?.Invoke(currentWeapon.currentAmmo, currentWeapon.maxAmmo, currentWeapon.storageAmmo);

    }

    private IEnumerator ShootDelay()
    {
        shootCooldownOver = false;
        yield return new WaitForSeconds(currentWeapon.shootCooldown);
        shootCooldownOver = true;
    }
    private IEnumerator ResetShootingLine()
    {
        isShootLineActive = true;
        yield return new WaitForSeconds(currentWeapon.visibleLineTime);
        lineRender.positionCount = 0;
        isShootLineActive = false;
    }
    void Update()
    {
        if(shootButtonHeld && currentWeapon.isAutomatic && shootCooldownOver)
        {
            Shoot();
        }
        if(isShootLineActive)
        {
            lineRender.SetPosition(0, currentWeapon.shootingPoint.position);
            lineRender.SetPosition(1, endPoint);
        }
    }
}
