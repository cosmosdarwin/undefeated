using UnityEngine;
using System.Collections;

public class ArrowMove : MonoBehaviour {

	void Start () {
        StartCoroutine("Animate");
    }

    IEnumerator Animate()
    {
        float MovePeriod = 2f;
        float SpinPeriod = 8f;

        while (true)
        {
            // Move

            // Linear easing from 0 --> 1 --> 0
            float e = (MovePeriod/2f - Mathf.Abs(MovePeriod/2f - Time.time % MovePeriod));

            int PeriodsSoFar = (int) Mathf.Floor(Time.time / MovePeriod);

            if (PeriodsSoFar % 2 == 0)
            {
                // Up
                gameObject.transform.Translate(new Vector3(0, Time.deltaTime * +1 * e, 0));
            }
            else
            {
                // Down
                gameObject.transform.Translate(new Vector3(0, Time.deltaTime * -1 * e, 0));
            }

            // Rotate

            gameObject.transform.Rotate(new Vector3(0, Time.deltaTime * 360/SpinPeriod, 0));
            yield return null;
        }
    }
}