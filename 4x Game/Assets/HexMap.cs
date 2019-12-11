using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    public bool randomMap;
    public bool blankMap;
    public bool testMap;
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }
    public GameObject HexPrefab;
    public GameObject HexStar;
    public Material MatSpace;
    public Material MatPlanet;
    public Material MatStar;
    public Material MatAsteroid;
    private int hexModvar;
    int rowCounter;
    public Mesh MeshPlanet;
    public Mesh MeshStar;
    public Mesh MeshAsteroid;
    private Hex[,] hexes;
    private Dictionary<Hex, GameObject> HexToGameObjectMap;
    

    
    public Material[] HexMaterials;
    int numRows = 100;
    int numColumns = 100;

    public Hex GetHexAt(int x, int y)
    {
        if(hexes == null)
        { Debug.LogError("Hexes not yet instantiated");
            return null; }
        return hexes[x, y];

    }


    public void GenerateMap()
    {
        hexes = new Hex[ numColumns , numRows];
        HexToGameObjectMap = new Dictionary<Hex, GameObject>();

        if (testMap == true)
        {
            generateBlankMap();

            ElevateArea(21, 15, 1);

            UpdateHexVisuals();

        }











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
                       // Debug.Log(rowCounter);
                        hexModvar++;
                    }
                    rowCounter++;
                    if (rowCounter == 10)
                    { rowCounter = rowCounter / 10 - 1; }

                }
            }

            for (int column = 0; column < numColumns; column++)
            {
                hexModvar = 0;
                for (int row = 0; row < numRows; row++)
                {
                    int hexMod = hexModvar;

                   
                    
                        int starGen = Random.Range(1, 100);
                        if (starGen < 10)
                        {
                        
                       
                    }

                  //  }





                    if (rowCounter == 0 || rowCounter == 2 || rowCounter == 4 || rowCounter == 6 || rowCounter == 8)
                    {
                     
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
                int hexMod = hexModvar;
                Hex h = new Hex(column - hexMod, row);
                h.Elevation = -1;
                
                hexes[column, row] = h;

                

                GameObject hexGO = Instantiate(HexPrefab, h.Position(), Quaternion.identity, this.transform);
                HexToGameObjectMap[h] = hexGO;
                MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
                mr.material = MatSpace;

                //Offsets the edges
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

    void ElevateArea(int q, int r, int radius)
    {
        for (int x = -radius; x < radius; x++)
        {
            for (int y = -radius; y < radius; y++)
            {
                Hex h = GetHexAt(q + x, r + y);
                h.Elevation = 0.5f;
            }
        }

    }

    public void UpdateHexVisuals()
    {
  
            for (int column = 0; column < numColumns; column++)
            {
                hexModvar = 0;
                for (int row = 0; row < numRows; row++)
                {
                Hex h = hexes[column, row];
                GameObject hexGO = HexToGameObjectMap[h];
                MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
                if(h.Elevation >= 0)
                {
                    mr.material = MatStar;
                }
                else
                mr.material = MatSpace;

                






                //Offsets the edges
                if (rowCounter == 0 || rowCounter == 2 || rowCounter == 4 || rowCounter == 6 || rowCounter == 8)
                    {
                       
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

/*
 * for (int column = 0; column < numColumns; column++)
        {
            hexModvar = 0;
            for (int row = 0; row < numRows; row++)
            {

                //Offsets the edges
                if (rowCounter == 0 || rowCounter == 2 || rowCounter == 4 || rowCounter == 6 || rowCounter == 8)
                {
                    hexModvar++;
                }
                rowCounter++;
                if (rowCounter == 10)
                { rowCounter = rowCounter / 10 - 1; }
            }
        }
*/