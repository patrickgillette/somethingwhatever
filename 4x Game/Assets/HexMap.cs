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
        hexes = new Hex[numColumns, numRows];
        HexToGameObjectMap = new Dictionary<Hex, GameObject>();

        if (blankMap == true)
        {
            generateBlankMap();
        }



        if (testMap == true)
        {
            generateBlankMap();

            int starGen;
            for (int column = 0; column < numColumns; column++)
            {
                hexModvar = 0;
                for (int row = 0; row < numRows; row++)
                {
                    int hexMod = hexModvar;

                    starGen = Random.Range(1, 100);
                    if (starGen < 2 && column > 3 && column < numColumns - 3 && row > 3 && row < numRows - 3)
                    {
                        ElevateArea(row, column, 5);









                        //Offsets the edges
                        if (rowCounter % 2 == 0)
                        {
                            //Debug.Log(rowCounter);
                            hexModvar++;
                        }
                        rowCounter++;


                    }
                }

               




                UpdateHexVisuals();

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
                        if (rowCounter % 2 == 0)
                        {
                            //Debug.Log(rowCounter);
                            hexModvar++;
                        }
                        rowCounter++;

                    }
                }

                for (int column = 0; column < numColumns; column++)
                {
                    hexModvar = 0;
                    for (int row = 0; row < numRows; row++)
                    {
                        int hexMod = hexModvar;



                        







                        if (rowCounter % 2 == 0)
                        {
                            //Debug.Log(rowCounter);
                            hexModvar++;
                        }
                        rowCounter++;
                    }
                }









            }

        }
    }

    public void generateBlankMap()
    {
        int starGen;
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
                               if (rowCounter%2 == 0)
                               {
                                   //Debug.Log(rowCounter);
                                   hexModvar++;
                               }
                               rowCounter++;
                               
                    
            }
        }
    }

    void ElevateArea(int q, int r, int radius)
    {
        Hex centerHex = GetHexAt(q, r);
        Hex[] areaHexes = GetHexesWithinRadiusOf(centerHex, radius,q,r);
            
                foreach(Hex h in areaHexes)
                {
                h.Elevation = 1;
                }
        

        centerHex.Elevation = 2;
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
                if (h.Elevation >= 2)
                {
                    mr.material = MatStar;
                }
                else if (h.Elevation == 1)
                {
                    mr.material = MatPlanet;
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

    public Hex[] GetHexesWithinRadiusOf(Hex centerHex, int radius, int row, int column)
    {
    
        List<Hex> results = new List<Hex>();

        
      
              for (int dx = row -radius ; dx <= row + radius ; dx++)
               {
            
                   for (int dy = column-radius  ; dy <= column + radius ; dy++)
                   {

                    if (Mathf.Sqrt(((Mathf.Abs(row - dx  )* Mathf.Abs(row - dx  )) + Mathf.Abs(column - dy ) * Mathf.Abs(column - dy ) ) ) +1 <= radius)
                    {
                    results.Add(hexes[dx, dy]);
                    }
                
                       
                  
                 
                     

                   }

               }
        
        return results.ToArray();
        
    }





    //for (int dy = Mathf.Max(-radius, -dx - radius + 1); dy<Mathf.Min(radius, -dx + radius); dy++)


   













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
                if (rowCounter%2 == 0)
                               {
                                   //Debug.Log(rowCounter);
                                   hexModvar++;
                               }
                               rowCounter++;
            }
        }





    //star rng
     starGen = Random.Range(1, 100);
                if (starGen < 5)
                {
                    ElevateArea(21, 15, 4);

                }
*/
