using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    public bool randomMap;
    public bool blankMap;
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }
    public GameObject HexPrefab;
    
    public Material MatSpace;
    public Material MatPlanet;
    public Material MatStar;
    public Material MatAsteroid;
    private int hexModvar;
    int rowCounter;
    public Mesh MeshPlanet;
    public Mesh MeshStar;
    public Mesh MeshAsteroid;


    public Material[] HexMaterials;
    int numRows = 100;
    int numColumns = 100;

    public void GenerateMap()
    {
        if (blankMap == true)
        {
            generateBlankMap();   
            }

        //generate random map
        if (randomMap == true)
        {
           // generateBlankMap();

            for (int column = 0; column < numColumns; column++)
            {

                hexModvar = 0;



                for (int row = 0; row < numRows; row++)
                {
                    int hexMod = hexModvar;



                    Hex h = new Hex(column - hexMod, row);
                    GameObject hexGO = Instantiate(HexPrefab, h.Position(), Quaternion.identity, this.transform);
                    MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
                    //mr.material = MatSpace;
                     mr.material = HexMaterials[Random.Range(0, HexMaterials.Length)];
               
                    if (rowCounter == 0 || rowCounter == 2 || rowCounter == 4 || rowCounter == 6 || rowCounter == 8)
                    {
                        Debug.Log(rowCounter);
                        hexModvar++;
                    }
                    rowCounter++;
                    if (rowCounter == 10)

                    { rowCounter = rowCounter / 10 - 1; }


                }


            }











        }
        
    }

    public void generateBlankMap()
    {
        for (int column = 0; column < numColumns; column++)
        {
            hexModvar = 0;
            for (int row = 0; row < numRows; row++)
            {
                //generate blank map
                int hexMod = hexModvar;
                Hex h = new Hex(column - hexMod, row);
                GameObject hexGO = Instantiate(HexPrefab, h.Position(), Quaternion.identity, this.transform);
                MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
                mr.material = MatSpace;
                if (rowCounter == 0 || rowCounter == 2 || rowCounter == 4 || rowCounter == 6 || rowCounter == 8)
                {
                    Debug.Log(rowCounter);
                    hexModvar++;
                }
                rowCounter++;
                if (rowCounter == 10)

                { rowCounter = rowCounter / 10 - 1; }
            }
        }
    }

}








//For random materials
//mr.material = HexMaterials[Random.Range(0, HexMaterials.Length)];



//if (row == 0 || row == 2 || row == 4 || row == 6 || row == 8)
//                   { hexModvar++; }