using UnityEngine;
using System.Collections;
using System;

public class Fire : MonoBehaviour {

    public Rigidbody Projectile;
    public Transform BarrelPosition;
    public float ShotForce;
    public float CoolDownTime = 2f; // 2.0 seconds between shots

    private DateTime NextShotAllowed;

    void Update () {

        if (Input.GetButtonDown("Jump"))
        {
            DateTime Time = DateTime.Now;
            if (Time.CompareTo(NextShotAllowed) > 0)
            {
                // Instantiate
                Rigidbody shot = Instantiate(Projectile, BarrelPosition.position, BarrelPosition.rotation) as Rigidbody;

                // Start 1/2 unit ahead of barrel to avoid its collider
                shot.transform.Translate(Vector3.up / 2f);

                // Rename
                shot.gameObject.name = "Ball";

                // Fire!
                shot.AddForce(BarrelPosition.up * ShotForce);

                // Start cooldown timer
                NextShotAllowed = Time.AddSeconds(CoolDownTime);
            }
        }
	}
}
