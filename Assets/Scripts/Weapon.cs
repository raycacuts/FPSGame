using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float range = 35f;
  
  

    public GameObject muzzleFlare;
    public float flareDisplayTime = .001f;


    public bool canAutoFire;
    public float timeBetweenShots = .1f;


    public int currentAmmo = 100;
    public int clipSize = 15;

    public int remainingAmmo = 300;



    public int pickupAmount = 10;

    public float damageAmount = 15f;
}
