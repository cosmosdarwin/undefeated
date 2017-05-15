using UnityEngine;
using System.Collections;

public class PlayBounceSound : MonoBehaviour {

    public AudioClip Bounce;

    void OnCollisionEnter()
    {
        AudioSource Audio = this.GetComponent<AudioSource>();

        // Volume
        float MaxHeight = 10; // Same volume for all bounces above N meters
        float MaxVelocity = Mathf.Sqrt(2f * 9.8f * MaxHeight); // Vf^2 = Vi^2 + 2ad
        float Velocity = this.GetComponent<Rigidbody>().velocity.magnitude;
        Audio.volume = Velocity / MaxVelocity;

        // Play
        Audio.PlayOneShot(Bounce);
    }
}