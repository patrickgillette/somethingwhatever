using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star
{
    public string Name;
    
    public Star(string name,int x, int y)
    {
        Name = name;
        x = x;
        y = y;

    }


    public Hex StarHex { get; protected set; }

    public void SetHex(Hex hex)
    {
        if (StarHex != null)
        {
            StarHex.RemoveStar(this);
        }
        StarHex = hex;
        StarHex.AddStar(this);

    }


}
