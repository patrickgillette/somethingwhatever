using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        mouseController = GameObject.FindObjectOfType<MouseController>();
        hexMap = GameObject.FindObjectOfType<HexMap>();      
	}

    public GameObject UnitSelectionPanel;
    public GameObject ColonySelectionPanel;
    public static GameObject SelectionIndicator;
    HexMap hexMap;

    // Unit selection
    Unit __selectedUnit = null;
    public Unit SelectedUnit {
        get { return __selectedUnit; }   
        set {
            __selectedUnit = null;
            if(__selectedColony != null)
                SelectedColony = null;

            __selectedUnit = value;
            UnitSelectionPanel.SetActive( __selectedUnit != null );
            UpdateSelectionIndicator();
        }
    }

    Colony __selectedColony = null;
    public Colony SelectedColony {
        get { return __selectedColony; }   
        set {
            if(__selectedColony != null)
            {
                // We already have a Colony selected, make sure we cancel the old mouse mode
                mouseController.CancelUpdateFunc();

            }

            __selectedColony = null;
            if(__selectedUnit != null)
                SelectedUnit = null;

            __selectedColony = value;
            ColonySelectionPanel.SetActive( __selectedColony != null );
            if(__selectedColony != null)
            {
                mouseController.StartColonyView();
            }
        }
    }

    MouseController mouseController;

	
	// Update is called once per frame
	void Update () {

        // De-select things have have been destroyed
        if(SelectedUnit != null && SelectedUnit.IsDestroyed == true)
        {
            // We are pointing to a destroyed unit.
            SelectedUnit = null;
        }

        
        if(SelectedColony != null && SelectedColony.IsDestroyed == true)
        {
            // We are pointing to a destroyed unit.
            SelectedColony = null;
        }

        UpdateSelectionIndicator();
        //Debug.Log(__selectedUnit);
	}

    void UpdateSelectionIndicator()
    {
        if(SelectedUnit == null)
        {
            //SelectionIndicator.SetActive(false);
            return;
        }

        GameObject uGO = hexMap.GetUnitGO( SelectedUnit );
        if(uGO == null)
        {
            SelectedUnit = null;
            return;
        }

        //SelectionIndicator.SetActive(true);
        //SelectionIndicator.transform.position = uGO.transform.position;
    }

    public void SelectNextUnit( bool skipDoneUnits )
    {
        Player player = hexMap.CurrentPlayer;

        Unit[] units = player.Units;

        int currentIndex = 0;

        if(SelectedUnit != null)
        {
            for (int i = 0; i < units.Length; i++)
            {
                if(SelectedUnit == units[i])
                {
                    currentIndex = i;
                    break;
                }
            }
        }

        for (int i = 0; i < units.Length; i++)
        {
            int tryIndex = (currentIndex + i + 1) % units.Length;

            if ( skipDoneUnits == true && units[tryIndex].UnitWaitingForOrders() == false )
            {
                // Skip this unit
                continue;
            }

            // We only get here if we're on a valid pick
            SelectedUnit = units[ tryIndex ];
            return;
        }

        // if we got here, selection did not change
        // If the pre-existing unit is done, and we're suppposed to skip that,
        // then clear the selection.
        if(SelectedUnit.UnitWaitingForOrders() == false && skipDoneUnits == true)
        {
            SelectedUnit = null;
        }

    }
}
