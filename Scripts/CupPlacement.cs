using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class CupPlacement : MonoBehaviour
{

    public float RaycastAltitude = 20f; // Altitude to raycast down from

    // Defaults
    public Vector3 PlacementCenter = new Vector3(0, 0, 0);
    public float MaxDistFromCenter = 1f;

    private GameObject Cup;
    private Vector3 InitialEulerAngles; // "Upright"

    private Vector3 Location;

    void Start()
    {
        // Find Cup
        Cup = GameObject.Find("Cup");
        InitialEulerAngles = Cup.transform.localEulerAngles;

        Randomize();
    }

    public void Randomize()
    {
        Debug.Log("Cup Placement: Randomize");

        GenerateNewLocation();

        // Set CupPosition to Location
        gameObject.transform.position = Location;
    }

    public void Restore()
    {
        Debug.Log("Cup Placement: Restore");

        // Enter isKinematic
        Rigidbody CupRigidBody = Cup.GetComponent<Rigidbody>();
        CupRigidBody.isKinematic = true; CupRigidBody.useGravity = false;

        // Calculate deltas to animate
        Vector3 deltaEulerAngles = InitialEulerAngles - Cup.transform.localEulerAngles;
        Vector3 DeltaLocation = Location - Cup.transform.position + new Vector3(0, 0.2f, 0); // Raise up slighly

        // Animate
        StartCoroutine(RestoreCorountine(deltaEulerAngles, DeltaLocation, 20));
    }

    IEnumerator RestoreCorountine(Vector3 deltaEulerAngles, Vector3 DeltaLocation, int Duration)
    {
        for (var i = 1; i <= Duration; i++)
        {
            Cup.transform.localEulerAngles += deltaEulerAngles / Duration;
            Cup.transform.position += DeltaLocation / Duration;
            if (i == Duration)
            {
                // Exit isKinematic
                Rigidbody CupRigidBody = Cup.GetComponent<Rigidbody>();
                CupRigidBody.isKinematic = false; CupRigidBody.useGravity = true;

                // Reset
                Cup.GetComponent<CupCore>().IsKnockedOver = false;
                Cup.GetComponent<CupCore>().HasAlreadyHit = false;

            }
            yield return null;
        }
    }

    void GenerateNewLocation()
    {
        float X;
        float Y;
        float Z;

        X = PlacementCenter.x + Random.Range(-MaxDistFromCenter, MaxDistFromCenter);
        Z = PlacementCenter.z + Random.Range(-MaxDistFromCenter, MaxDistFromCenter);

        Vector3 RaycastOrigin = new Vector3(X, RaycastAltitude, Z);

        // Cast ray to determine Y

        RaycastHit HitInfo;
        if (Physics.Raycast(RaycastOrigin, Vector3.down, out HitInfo, RaycastAltitude))
        {
            Y = RaycastAltitude - HitInfo.distance;
            Debug.DrawRay(RaycastOrigin, Vector3.down * RaycastAltitude, Color.blue, 15.0f);
        }
        else
        {
            throw new System.Exception("Cup placement raycast did not find solid ground!");
        }

        // Cast 4 more rays at +/- width, depth of cup to check if flat

        List<Vector3> Offsets = new List<Vector3>();
        Offsets.Add(new Vector3(+Cup.transform.localScale.x, 0, 0));
        Offsets.Add(new Vector3(-Cup.transform.localScale.x, 0, 0));
        Offsets.Add(new Vector3(0, 0, +Cup.transform.localScale.z));
        Offsets.Add(new Vector3(0, 0, -Cup.transform.localScale.z));

        bool IsFlat = true;

        foreach (Vector3 Offset in Offsets)
        {
            if (Physics.Raycast(RaycastOrigin + Offset, Vector3.down, out HitInfo, RaycastAltitude))
            {
                float Yprime = RaycastAltitude - HitInfo.distance;
                if (Yprime == Y)
                {
                    // Good
                    Debug.DrawRay(RaycastOrigin + Offset, Vector3.down * RaycastAltitude, Color.blue, 15.0f);
                }
                else
                {
                    Debug.DrawRay(RaycastOrigin + Offset, Vector3.down * RaycastAltitude, Color.red, 15.0f);
                    IsFlat = false;
                }
            }
        }

        if (IsFlat)
        {
            Location = new Vector3(X, Y, Z);
        }
        else
        {
            // Try again recursively
            GenerateNewLocation();
        }
    }
}
