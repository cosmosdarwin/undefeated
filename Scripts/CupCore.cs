using UnityEngine;
using System.Collections;

public class CupCore : MonoBehaviour
{

    public AudioClip Score;
    public AudioClip CupKnockedOver;

    public bool HasAlreadyHit = false;
    public bool IsKnockedOver = false;

    private GameObject Ball;
    private Vector3 BallVelocity;
    private Vector3 BallPosition;

    private GameObject Cup;
    private Vector3 CupVelocity;
    private Vector3 CupPosition;

    // Requirements to score
    private float RequiredDistanceToCupCenter = 0.04f; // 4 cm
    private float RequiredInboundAngle = -30;

    void Start()
    {
        // Find Cup
        Cup = GameObject.Find("Cup");
    }

    void FixedUpdate()
    {
        // Record velocity and position each physics update

        if (!HasAlreadyHit)
        {
            CupVelocity = Cup.GetComponent<Rigidbody>().velocity;
            CupPosition = Cup.transform.position;

            // TODO: What if there are multiple balls?
            Ball = GameObject.Find("Ball");

            if (Ball != null)
            {
                BallVelocity = Ball.GetComponent<Rigidbody>().velocity;
                BallPosition = Ball.transform.position;
            }
        }

        // Check if knocked over yet

        if (!IsKnockedOver)
        {
            Vector3 CupUpVector = transform.TransformDirection(Cup.transform.up);
            Vector3 WorldUpVector = Vector3.up;

            if (Mathf.Abs(Vector3.Dot(CupUpVector, WorldUpVector)) > 0.7) // Dot of 0.7 ~= 45'
            {
                // Still standing
            }
            else
            {
                IsKnockedOver = true;

                Debug.Log("IsKnockedOver!");

                AudioSource Audio = GameObject.Find("FPSController").GetComponent<AudioSource>();
                Audio.PlayOneShot(CupKnockedOver);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!HasAlreadyHit && collision.collider.gameObject == Ball)
        {
            // Only process the first hit
            HasAlreadyHit = true;

            // Check #1: DistanceToCupCenter, in World coordinates, ignore vertical component

            Vector2 LocationOfHit = new Vector2(collision.contacts[0].point.x, collision.contacts[0].point.z);
            Vector2 LocationOfCup = new Vector2(Cup.transform.position.x, Cup.transform.position.z);
            float DistanceToCupCenter = (LocationOfHit - LocationOfCup).magnitude;

            // Check #2: InboundAngle, in Degrees, where horizontal = 0'

            float v = BallVelocity.y;
            float h = Mathf.Max(new Vector2(BallVelocity.x, BallVelocity.z).magnitude, 0.001f); // Avoid divide-by-zero
            float InboundAngle = Mathf.Atan(v / h) * Mathf.Rad2Deg;

            Debug.Log("Calculation: DistanceToCupCenter = " + DistanceToCupCenter + ", Required: " + RequiredDistanceToCupCenter);
            Debug.Log("Calculation: InboundAngle = " + InboundAngle + ", Required: " + RequiredInboundAngle);

            if (DistanceToCupCenter < RequiredDistanceToCupCenter && InboundAngle < RequiredInboundAngle)
            {
                // Score!
                Debug.Log("Score!");
                AudioSource Audio = GameObject.Find("FPSController").GetComponent<AudioSource>();
                Audio.PlayOneShot(Score);

                // Make it possible for Ball to enter Cup
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.GetComponent<MeshCollider>().convex = false;

                // Undo effects of collision with Cup top surface
                Ball.GetComponent<Rigidbody>().velocity = BallVelocity;
                Ball.transform.position = BallPosition;

                Cup.GetComponent<Rigidbody>().velocity = CupVelocity;
                Cup.transform.position = CupPosition;
            }
            else
            {
                // Miss!
                Debug.Log("Miss!");

                // The physics play out on their own...
            }
        }
    }
}
