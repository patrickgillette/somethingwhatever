using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Player
{
    public Player( string name )
    {
        PlayerName = name; 

        units = new HashSet<Unit>();
        colonies = new HashSet<Colony>();
    }

    public string PlayerName;

    public enum PlayerType { LOCAL, AI, REMOTE };
    public PlayerType Type = PlayerType.LOCAL;

    private HashSet<Unit> units;
    private HashSet<Colony> colonies;

    public Unit[] Units {
        get { return units.ToArray(); }
    }
    public Colony[] Colonies {
        get { return colonies.ToArray(); }
    }

    public void AddUnit( Unit u )
    {
        units.Add(u);
        u.OnObjectDestroyed += OnUnitDestroyed;
    }

    public void OnUnitDestroyed( MapObject mo )
    {
        units.Remove( (Unit)mo );
    }

    public void AddColony ( Colony c )
    {
        colonies.Add(c);
        c.OnObjectDestroyed += OnColonyDestroyed;
    }

    public void OnColonyDestroyed( MapObject mo )
    {
        colonies.Remove( (Colony)mo );
    }


}

