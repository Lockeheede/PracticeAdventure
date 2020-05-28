using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public Player player;
    public Vector3 offset;
    public float focusSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime * focusSpeed);
            //offset coordinates for level1 (1, 2.7, -5.3) Rotation: x= 20

        if (player.JustTeleported)
            {
              transform.position = player.transform.position + offset;
            }
        }
    }
}
