using UnityEngine;
using System.Collections;

public class Gaze : MonoBehaviour {

    public GameObject PreviousTarget;

	void Update () {
               
        Vector3 Forward = Vector3.forward; // Forward vector in object ("local") space
        Vector3 Gaze = transform.TransformDirection(Forward); // In world space

        int HandleLayer = 1 << 8; // Only gaze upon objects on the Handle (8th) layer

        RaycastHit HitInfo;
        if (Physics.Raycast(transform.position, Gaze, out HitInfo, 20, HandleLayer)) {
            GameObject Target = HitInfo.collider.gameObject;
            if (Target != PreviousTarget)
            {
                if (PreviousTarget != null)
                {
                    PreviousTarget.GetComponent<Handle>().Unhover();
                }
                Target.GetComponent<Handle>().Hover();
                try
                {
                    Target.GetComponent<Handle>().Hover();
                    PreviousTarget = Target;
                }
                catch
                {
                    throw new System.Exception("The ray hit an object on the Handle layer missing the Handle.cs script!");
                }
            }
        }
        else
        {
            if (PreviousTarget != null) {
                PreviousTarget.GetComponent<Handle>().Unhover();
                PreviousTarget = null;
            }
        }
        
    }
}
