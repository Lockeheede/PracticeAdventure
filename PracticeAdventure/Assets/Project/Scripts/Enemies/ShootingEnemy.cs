﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy
{
    public GameObject model;
    public float timeToRotate = 2f;
    public float rotationSpeed = 6f;
    
    public GameObject bulletPrefab;
    public float timeToShoot = 1f;

    private int targetAngle;
    private float rotationTimer;
    private float shootingTimer;

    // Start is called before the first frame update
    void Start()
    {
        rotationTimer = timeToRotate;
        shootingTimer = timeToShoot;
    }

    // Update is called once per frame
    void Update()
    {
        //Update the enemy's angle
        rotationTimer -= Time.deltaTime;
        if(rotationTimer <= 0f)
        {
            rotationTimer = timeToRotate;

            targetAngle += 90;
        }

        //Change the rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, targetAngle, 0), Time.deltaTime * rotationSpeed);

        //Shoot bullets
        shootingTimer -= Time.deltaTime;
        if(shootingTimer <= 0f)
        {
            shootingTimer = timeToShoot;

            GameObject bulletObject = Instantiate(bulletPrefab);
            bulletObject.transform.position = transform.position + model.transform.forward;
            bulletObject.transform.forward = model.transform.forward;
        }
    }
}
