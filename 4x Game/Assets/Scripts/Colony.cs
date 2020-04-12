using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Colony : MapObject
{
    public Colony()
    {
        Name = "Earth";

        
    }

    BuildingJob buildingJob;

    float productionPerTurn = 9001;

    override public void SetHex( Hex newHex )
    {
        if(Hex != null)
        {
            
            Hex.RemoveColony(this);
        }

        base.SetHex( newHex );

        Hex.AddColony( this );
    }

    public void DoTurn()
    {
        if(buildingJob != null)
        {
            float workLeft = buildingJob.DoWork( productionPerTurn );
            if(workLeft <= 0)
            {
                
                buildingJob = null;
                
            }
        }
    }

    public BuildingBlueprint[] GetPossibleBuildings()
    {
        return BuildingDatabase.GetListOfBuilding();
    }



}

