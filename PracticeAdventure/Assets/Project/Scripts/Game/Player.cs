﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
public class Player : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject model;
    public float rotatingSpeed = 2f;

    [Header("Movement")]
    public float movingVelocity;
    public float jumpVelocity;
    public float knockbackForce;

    [Header("Equipment")]
    public int health = 12;
    public Sword sword;
    public Bow bow;
    public GameObject bombPrefab;
    public float throwingSpeed;
    public int bombAmount = 5;
    public int arrowAmount = 15;

    private Rigidbody playerRigidbody;
    private bool canJump = false;
    private Quaternion targetModelRotation;
    private float knockBackTimer;

    void Start()
    {
        bow.gameObject.SetActive(false);
        playerRigidbody = GetComponent<Rigidbody>();
        targetModelRotation = Quaternion.Euler(0, 0, 0);
    }
    void Update()
    {
        //Raycast to identify if the player can jump
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.01f))
        {
            canJump = true;
        }
        model.transform.rotation = Quaternion.Lerp(model.transform.rotation, targetModelRotation, Time.deltaTime * rotatingSpeed);

        if (knockBackTimer > 0)
        {
            knockBackTimer -= Time.deltaTime;
        }
        else
        {
            ProcessInput();
        }
    }

    void ProcessInput()
    {
        //Move in the XZ Axes
        
        //Netural Movement, followed by right, left, up and down
        playerRigidbody.velocity = new Vector3(
           0,
           playerRigidbody.velocity.y,
           0
           );

        if (Input.GetKey("right"))
        {
           playerRigidbody.velocity = new Vector3(
           movingVelocity,
           playerRigidbody.velocity.y,
           playerRigidbody.velocity.z
           );
            targetModelRotation = Quaternion.Euler(0, 90, 0);
        }

        if (Input.GetKey("left"))
        {
           playerRigidbody.velocity = new Vector3(
           -movingVelocity,
           playerRigidbody.velocity.y,
           playerRigidbody.velocity.z
           );
            targetModelRotation = Quaternion.Euler(0, 270, 0);
        }

        if (Input.GetKey("up"))
        {
           playerRigidbody.velocity = new Vector3(
           playerRigidbody.velocity.x,
           playerRigidbody.velocity.y,
           movingVelocity
           );
            targetModelRotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey("down"))
        {
            playerRigidbody.velocity = new Vector3(
            playerRigidbody.velocity.x,
            playerRigidbody.velocity.y,
            -movingVelocity
            );
            targetModelRotation = Quaternion.Euler(0, 180, 0);
        }

        //Check for jumps
        if (canJump && Input.GetKeyDown("space"))
        {
            canJump = false;
            playerRigidbody.velocity = new Vector3(
                playerRigidbody.velocity.x,
                jumpVelocity,
                playerRigidbody.velocity.z
                );
        }

        //Check equipment interaction
        if (Input.GetKeyDown("z"))
        {
            sword.gameObject.SetActive(true);
            bow.gameObject.SetActive(false);
            sword.Attack();
        }

        if(Input.GetKeyDown("c"))
        {
            ThrowBomb();
        }

        if(Input.GetKeyDown("x"))
        {
            sword.gameObject.SetActive(false);
            bow.gameObject.SetActive(true);
            if (arrowAmount > 0)
            {
                bow.Attack();
                arrowAmount--;
            }
          
        }
    }

    private void ThrowBomb()
    {
        if(bombAmount <= 0)
        {
            return;
        }

        GameObject bombObject = Instantiate(bombPrefab);
        bombObject.transform.position = transform.position + model.transform.forward;

        Vector3 throwingDirection = (model.transform.forward + Vector3.up).normalized;

        bombObject.GetComponent<Rigidbody>().AddForce(throwingDirection * throwingSpeed);

        bombAmount--;
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        if(otherCollider.GetComponent<EnemyBullet> () != null)
        {
            Hit((transform.position - otherCollider.transform.position).normalized);
            Destroy(otherCollider.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            Hit((transform.position - collision.transform.position).normalized);
        }
    }

    private void Hit(Vector3 direction)
    {
        Vector3 knockbackDirection = (direction + Vector3.up).normalized;
        playerRigidbody.AddForce(knockbackDirection * knockbackForce);
        knockBackTimer = 1f;
       
        health--;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}