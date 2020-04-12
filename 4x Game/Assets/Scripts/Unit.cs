using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QPath;
using System.Linq;

public class Unit : MapObject, IQPathUnit
{
    
    public string Name;

    public Unit()
    {
        Name = "Bla";
    }

    public int Strenth = 8;
    public int Movement = 4;
    public int MovementRemaining = 4;
    public bool CanColonize = false;
    public bool SkipThisUnit = false;
    

    List<Hex> hexPath;

    
    

    public void DUMMY_PATHING_FUNCTION()
    {
        Debug.Log(Hex);
        Hex[] pathHexes = QPath.QPath.FindPath<Hex>(Hex.HexMap,this,Hex,
            Hex.HexMap.GetHexAt(Hex.Q + 6, Hex.R),Hex.CostEstimate);

        Debug.Log("Got pathfinding path of length: " + pathHexes.Length);

        SetHexPath(pathHexes);
    }

    public void ClearHexPath()
    {
        SkipThisUnit = false;
        this.hexPath = new List<Hex>();
    }

    public void SetHexPath( Hex[] hexArray )
    {
        SkipThisUnit = false;
        this.hexPath = new List<Hex>( hexArray );
    }

    public Hex[] GetHexPath()
    {
        //Debug.Log("GetHexPath");
        return (this.hexPath == null ) ? null : this.hexPath.ToArray();
    }

    public int GetHexPathLength()
    {
        Debug.Log("GethexLength");
        return this.hexPath.Count;
    }

    public bool UnitWaitingForOrders()
    {
        if(SkipThisUnit)
        {
            return false;
        }

        // Returns true if we have movement left but nothing queued
        if( 
            MovementRemaining > 0 && 
            (hexPath==null || hexPath.Count==0) 
            
        )
        {
            return true;
        }

        return false;
    }

    public void RefreshMovement()
    {
        SkipThisUnit = false;
        MovementRemaining = Movement;
    }

 
    public bool DoMove()
    {
        
        Debug.Log("DoMove");
        // Do queued move

        //Debug.Log(MovementRemaining);
        if(MovementRemaining <= 0)
        {
           // Debug.Log("returning");
            return false;

        }


        if(hexPath == null || hexPath.Count == 0)
        {
            Debug.Log("returning");
            return false;
        }
        Hex newhex = Hex;
   
        Hex oldhex = Hex;

        newhex = oldhex.HexMap.GetHexAt(oldhex.Q + 1, oldhex.R);
 
        // Move to the new Hex
        SetHex(newhex);
        

        // Grab the first hex from our queue
        Hex hexWeAreLeaving = hexPath[0];
        Hex newHex = hexPath[1];

        int costToEnter = 1;

        if( costToEnter > MovementRemaining && MovementRemaining < Movement )
        {
            // We can't enter the hex this turn
            Debug.Log("movement remaining too low");
            return false;
        }

        hexPath.RemoveAt(0);

        if( hexPath.Count == 1 )
        {
            // The only hex left in the list, is the one we are moving to now,
            // therefore we have no more path to follow, so just clear
            // the queue to avoid confusion.
            hexPath = null;
        }


        MovementRemaining = Mathf.Max(MovementRemaining-costToEnter, 0);

        return hexPath != null && MovementRemaining > 0;
    }



    public float AggregateTurnsToEnterHex( Hex hex, float turnsToDate )
    {
        // If you are trying to enter a tile
        // with a movement cost greater than your current remaining movement
        // points, this will either result in a cheaper-than expected
        // turn cost (Civ5) or a more-expensive-than expected turn cost (Civ6)

        float baseTurnsToEnterHex = 1 / Movement; // Example: Entering a forest is "1" turn

        if(baseTurnsToEnterHex < 0)
        {
            // Impassible terrain
            //Debug.Log("Impassible terrain at:" + hex.ToString());
            return -99999;
        }

        if(baseTurnsToEnterHex > 1)
        {
            // Even if something costs 3 to enter and we have a max move of 2, 
            // you can always enter it using a full turn of movement.
            baseTurnsToEnterHex = 1;
        }


        float turnsRemaining = MovementRemaining / Movement;    // Example, if we are at 1/2 move, then we have .5 turns left

        float turnsToDateWhole = Mathf.Floor(turnsToDate); // Example: 4.33 becomes 4
        float turnsToDateFraction = turnsToDate - turnsToDateWhole; // Example: 4.33 becomes 0.33

        if( (turnsToDateFraction > 0 && turnsToDateFraction < 0.01f) || turnsToDateFraction > 0.99f )
        {
            Debug.LogError("Looks like we've got floating-point drift: " + turnsToDate);

            if( turnsToDateFraction < 0.01f )
                turnsToDateFraction = 0;

            if( turnsToDateFraction > 0.99f )
            {
                turnsToDateWhole   += 1;
                turnsToDateFraction = 0;
            }
        }

        float turnsUsedAfterThismove = turnsToDateFraction + baseTurnsToEnterHex; // Example 0.33 + 1

        if(turnsUsedAfterThismove > 1)
        {
            //don't actually have enough movement to complete this move.
            // What do we do?
            // we can always enter a tile, even if we don't
            // have enough movement left.
            turnsUsedAfterThismove = 1;
            
        }




        // Do we return the number of turns THIS move is going to take?
        // No, this an an "aggregate" function, so return the total
        // turn cost of turnsToDate + turns for this move.

        return turnsToDateWhole + turnsUsedAfterThismove;

    }

    override public void SetHex( Hex newHex )
    {
        if(Hex != null)
        {
            Hex.RemoveUnit(this);
        }

        base.SetHex( newHex );

        Hex.AddUnit(this);
    }

    override public void Destroy(  )
    {
        base.Destroy(  );

        Hex.RemoveUnit(this);
    }


    public float CostToEnterHex( IQPathTile sourceTile, IQPathTile destinationTile )
    {
        return 1;
    }
}
