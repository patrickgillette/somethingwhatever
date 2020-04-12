using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using QPath;

public class Hex : IQPathTile
{
    public Hex( int q, int r)

        {
            this.Q = q;
            this.R = r;
            this.S = -(q + r);
            units = new HashSet<Unit>();
        }

        public readonly int Q;  // Column

        public readonly int R;  // Row

        public readonly int S;



        public float Elevation;

        public readonly HexMap HexMap;

        static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

        float radius = 1f;


        public override string ToString()

        {
            return Q + ", " + R;
        }

        public Vector3 Position()

        {
            return new Vector3(
                HexHorizontalSpacing() * (this.Q + this.R / 2f),
                0,
                HexVerticalSpacing() * this.R

            );

        }

        public float HexHeight()
        {
            return radius * 2;
        }

        public float HexWidth()
        {
            return WIDTH_MULTIPLIER * HexHeight();
        }

        public float HexVerticalSpacing()
        {
            return HexHeight() * 0.75f;
        }

        public float HexHorizontalSpacing()
        {
            return HexWidth();
        }

        public Vector3 PositionFromCamera()

        {
            return HexMap.GetHexPosition(this);
        }

        public Vector3 PositionFromCamera(Vector3 cameraPosition, float numRows, float numColumns)
        {
            float mapHeight = numRows * HexVerticalSpacing();
            float mapWidth = numColumns * HexHorizontalSpacing();
            Vector3 position = Position();

            return position;

        }

        public static float CostEstimate(IQPathTile aa, IQPathTile bb)
        {
            return Distance((Hex)aa, (Hex)bb);
        }



    public static float Distance(Hex a, Hex b)
        { 
            int dQ = Mathf.Abs(a.Q - b.Q);
            int dR = Mathf.Abs(a.R - b.R);
            return
                Mathf.Max(dQ, dR, Mathf.Abs(a.S - b.S));
        }
/**********UNITS*************************
****************************************/
    HashSet<Unit> units;

    public Unit[] Units
    {
      get
        {
            return units.ToArray();
        }
    }

    //Add the unit to a list of units
    public void AddUnit(Unit unit)

        {

            if (units == null)

            {

                units = new HashSet<Unit>();

            }



            units.Add(unit);

        }


    //Remove the unit to a list of units
    public void RemoveUnit(Unit unit)

        {

            if (units != null)

            {

                units.Remove(unit);

            }

        }
/**********STARS*************************
 ***************************************/
        //Add the star to a list of stars
        HashSet<Star> stars;
    public void AddStar(Star star)
    {
        if (stars == null)
        {
            stars = new HashSet<Star>();
        }
        stars.Add(star);
    }

    //Remove the unit from a list of units
    public void RemoveStar(Star star)
    {

        stars.Remove(star);
    }

    public Star[] Stars()
    {
        return stars.ToArray();
    }

    public void AddColony(Colony Colony)
    {
        if (this.Colony != null)
        {
            throw new UnityException("Trying to add a Colony to a hex that already has one!");
        }

        this.Colony = Colony;
    }

    public void RemoveColony(Colony Colony)
    {
        if (this.Colony == null)
        {
            Debug.LogError("Trying to remove a Colony where there isn't one!");
            return;
        }

        if (this.Colony != Colony)
        {
            Debug.LogError("Trying to remove a Colony that isn't ours!");
            return;
        }

        this.Colony = null;
    }


    /// ///////////////////////////////////////Colonies///////////////////
    public Colony Colony { get; protected set; }



    Hex[] neighbours;

    #region IQPathTile implementation
    public IQPathTile[] GetNeighbours()
    {
        if (this.neighbours != null)
            return this.neighbours;

        List<Hex> neighbours = new List<Hex>();
        

        neighbours.Add(HexMap.GetHexAt(Q + 1, R + 0));
        neighbours.Add(HexMap.GetHexAt(Q + -1, R + 0));
        neighbours.Add(HexMap.GetHexAt(Q + 0, R + +1));
        neighbours.Add(HexMap.GetHexAt(Q + 0, R + -1));
        neighbours.Add(HexMap.GetHexAt(Q + +1, R + -1));
        neighbours.Add(HexMap.GetHexAt(Q + -1, R + +1));
        Debug.Log(neighbours);
        List<Hex> neighbours2 = new List<Hex>();

        foreach (Hex h in neighbours)
        {
            if (h != null)
            {
                neighbours2.Add(h);
            }
        }

        this.neighbours = neighbours2.ToArray();

        return this.neighbours;
    }

    public float AggregateCostToEnter(float costSoFar, IQPathTile sourceTile, IQPathUnit theUnit)
    {

        return ((Unit)theUnit).AggregateTurnsToEnterHex(this, costSoFar);
    }
    #endregion
}
