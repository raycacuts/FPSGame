using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    public float range = 35f;
    public Transform cam;
    public LayerMask validLayers;

    public GameObject impactEffect;
    public GameObject damageEffect;

    public GameObject muzzleFlare;
    public float flareDisplayTime = .001f;
    private float flareCounter;

    public bool canAutoFire;
    public float timeBetweenShots = .1f;
    private float shotCounter;

    public int currentAmmo = 100;
    public int clipSize = 15;

    public int remainingAmmo = 300;

    private UIController UIcon;

    public int pickupAmount = 10;

    public float damageAmount = 15f;

    public Weapon[] weapons;
    private int currentWeapon = 0;
    private int previousWeapon = 0;

    private int sfxIndex;

    // Start is called before the first frame update
    void Start()
    {
        UIcon = FindFirstObjectByType<UIController>();

        SetWeapon(currentWeapon);

        Reload();
    }

    // Update is called once per frame
    void Update()
    {
        if(flareCounter > 0)
        {
            flareCounter -= Time.deltaTime;
            if(flareCounter <= 0)
            {
                muzzleFlare.SetActive(false);
            }
        }
    }

    public void Shoot()
    {
        if(currentAmmo > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, range, validLayers))
            {

                if (hit.transform.tag == "Enemy")
                {
                    Instantiate(damageEffect, hit.point, Quaternion.identity);

                    hit.transform.GetComponent<EnemyController>().TakeDamage(damageAmount);
                    AudioManager.instance.PlaySFX(0);
                }
                else
                {
                    Instantiate(impactEffect, hit.point, Quaternion.identity);
                    AudioManager.instance.PlaySFX(1);
                }

            }

            muzzleFlare.SetActive(true);
            flareCounter = flareDisplayTime;

            shotCounter = timeBetweenShots;
            currentAmmo--;

            UIcon.UpdateAmmoText(currentAmmo, remainingAmmo);
            AudioManager.instance.PlaySFX(sfxIndex);
        }
        
    }
    public void ShootHeld()
    {
        if(canAutoFire = true)
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                Shoot();
            }
        }
        
    }
    public void Reload()
    {
        remainingAmmo += currentAmmo;

        if(remainingAmmo >= clipSize)
        {
            currentAmmo = clipSize;
            remainingAmmo -= clipSize;
        }
        else
        {
            currentAmmo = remainingAmmo;
            remainingAmmo = 0;
        }
        UIcon.UpdateAmmoText(currentAmmo, remainingAmmo);
    }
    public void GetAmmo()
    {
        remainingAmmo += pickupAmount;
        UIcon.UpdateAmmoText(currentAmmo, remainingAmmo);
    }
    public void SetWeapon(int weaponToSet)
    {
        if(previousWeapon != currentWeapon)
        {
            weapons[previousWeapon].currentAmmo = currentAmmo;
            weapons[previousWeapon].remainingAmmo = remainingAmmo;
        }
        

        range = weapons[weaponToSet].range;
        flareDisplayTime = weapons[weaponToSet].flareDisplayTime;
        canAutoFire = weapons[weaponToSet].canAutoFire;
        timeBetweenShots = weapons[weaponToSet].timeBetweenShots;
        currentAmmo = weapons[weaponToSet].currentAmmo;
        clipSize = weapons[weaponToSet].clipSize;
        remainingAmmo = weapons[weaponToSet].remainingAmmo;
        pickupAmount = weapons[weaponToSet].pickupAmount;
        damageAmount = weapons[weaponToSet].damageAmount;
        muzzleFlare = weapons[weaponToSet].muzzleFlare;

        sfxIndex = weapons[weaponToSet].sfxIndex;

        foreach(Weapon w in weapons)
        {
            w.gameObject.SetActive(false);
        }
        weapons[weaponToSet].gameObject.SetActive(true);

        UIcon.UpdateAmmoText(currentAmmo, remainingAmmo);

        previousWeapon = currentWeapon;
    }
    public void NextWeapon()
    {
        currentWeapon = (currentWeapon + 1) % weapons.Length;
        SetWeapon(currentWeapon);
    }
    public void PreviousWeapon()
    {
        currentWeapon = (currentWeapon - 1) % weapons.Length;
        SetWeapon(currentWeapon);
    }
}
