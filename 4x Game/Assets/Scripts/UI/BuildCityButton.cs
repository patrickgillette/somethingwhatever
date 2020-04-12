using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildColonyButton : MonoBehaviour {

    public void BuildColony()
    {
        Colony Colony = new Colony();

        HexMap map = GameObject.FindObjectOfType<HexMap>();
        SelectionController sc = GameObject.FindObjectOfType<SelectionController>();

        //map.SpawnColonyAt(Colony, map.ColonyPrefab, sc.SelectedUnit.Hex.Q, sc.SelectedUnit.Hex.R);
    }
	
}
