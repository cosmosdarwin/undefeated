using UnityEngine;
using System.Collections;

public class CupFocus : FocusHandler
{
    private GameObject Cup;
    private GameObject CupPosition;

    void Start()
    {
        Cup = GameObject.Find("Cup");
        CupPosition = GameObject.Find("CupPosition");
    }

    public override void HandleFocus()
    {
        CupPlacement PlacementScript = CupPosition.GetComponent<CupPlacement>();

        if (Cup.GetComponent<CupCore>().IsKnockedOver)
        {
            // Restore
            PlacementScript.Restore();
        }
        else
        {
            // Randomize
            PlacementScript.Randomize();
        }

        gameObject.GetComponent<Handle>().Unfocus();
    }

    public override string Hint()
    {
        if (Cup.GetComponent<CupCore>().IsKnockedOver)
        {
            return "Say 'Restore'";
        }
        else
        {
            return "Say 'Randomize'";
        }
    }
}
