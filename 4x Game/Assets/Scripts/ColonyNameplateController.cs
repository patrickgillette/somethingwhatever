using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonyNameplateController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.FindObjectOfType<HexMap>().OnColonyCreated += CreateColonyNameplate;
	}

	// Update is called once per frame
	void Update () {
		
	}

    public GameObject ColonyNameplatePrefab;

    public void CreateColonyNameplate( Colony Colony, GameObject ColonyGO )
    {
        GameObject nameGO = (GameObject)Instantiate(ColonyNameplatePrefab, this.transform);
        nameGO.GetComponent<MapObjectNamePlate>().MyTarget = ColonyGO;
        nameGO.GetComponentInChildren<ColonyNameplate>().MyColony = Colony;

    }
}
