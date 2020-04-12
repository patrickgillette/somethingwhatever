using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StarList : MonoBehaviour
{
    public string textDisplay;
    private string starName;
    private Star star;
    public Text display;
    public HexMap map;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        display.text = textDisplay;



        for(int i = 0; i < 5; i++)
        {
 
           // star = map.ArrayofStars[i];
           // textDisplay = star.Name + "/n";
            
           
        }
       
    }
    public string GetStarName(Star star)
    {

        starName = this.name;

        return starName;
    }
}
