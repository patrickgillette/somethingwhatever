using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    
    Vector3 newPosition;
    Vector3 currentVeloColony;
    float smoothTime = 0.5f;

    void Start()
    {
        newPosition = this.transform.position;  
    }

    public void OnUnitMoved(Hex oldHex, Hex newHex)
    {

        // Our correct position when we aren't moving, is to be at
        // 0,0 local position relative to our parent.


        Vector3 oldPosition = oldHex.PositionFromCamera();
        newPosition = newHex.PositionFromCamera();
        currentVeloColony = Vector3.zero;

        oldPosition.y += oldHex.HexMap.GetHexGO(oldHex).GetComponent<HexComponent>().VerticalOffset;
        newPosition.y += newHex.HexMap.GetHexGO(newHex).GetComponent<HexComponent>().VerticalOffset;
        this.transform.position = oldPosition;

        if (Vector3.Distance(this.transform.position, newPosition) > 0)
        {
            // This OnUnitMoved is considerably more than the expected move
            // between two adjacent tiles so just teleport
            this.transform.position = newPosition;
        }
        else
        {
            
           GameObject.FindObjectOfType<HexMap>().AnimationIsPlaying = true;
        }
    }

    void Update()
    {
        this.transform.position = Vector3.SmoothDamp(this.transform.position, newPosition, ref currentVeloColony, smoothTime);

    }
}
