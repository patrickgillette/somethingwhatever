using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QPath;
using System.Linq;

public class HexMap : MonoBehaviour, IQPathWorld
{

    
    public bool blankMap;
    public bool testMap;
    public GameObject HexPrefab;
    public GameObject UnitShip;
    public GameObject StarPrefab;
    public GameObject PlanetPrefab;
    public GameObject AsteroidPrefab;


    public Material MatSpace;
    public Material MatPlanet;

    public Material MatAsteroid;
    private int starsNum;
    private int hexModvar;
    int rowCounter;
    public Mesh MeshPlanet;
    public Mesh MeshStar;
    public Mesh MeshAsteroid;
    public int starsMax;
    public int TurnNumber = 0;


    private Hex[,] hexes;
    private Star[,] starArray;

    private Dictionary<Hex, GameObject> HexToGameObjectMap;
    private Dictionary<GameObject, Hex> gameObjectToHexMap;


    public Star[] ArrayOfStars;

    private HashSet<Unit> units;
    private HashSet<Star> stars;

    private Dictionary<Star, GameObject> starToGameObjectDic;
    private Dictionary<Unit, GameObject> unitToGameObjectMap;





    public Material[] HexMaterials;
    public int numRows = 100;
    public int numColumns = 100;


    /////////////////Start Code////////////////////////////////////////////////////////////////////////

    void Start()
    {
        GenerateMap();
        GeneratePlayers(1);
        
    }

    //WIP exectute qued commands
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            if (units != null)
            {

                foreach (Unit u in units)
                {

                    u.DoMove();

                    
                }

            }
        
    }


    public bool AnimationIsPlaying = false;

    public delegate void ColonyCreatedDelegate(Colony Colony, GameObject ColonyGO);
    public event ColonyCreatedDelegate OnColonyCreated;
    /********************************************************************8
     * ********************************************************************/
    public Player[] Players;
    int currentPlayerIndex = 0;
    public Player CurrentPlayer
    {
        get { return Players[currentPlayerIndex]; }
    }
    void GeneratePlayers(int numPlayers)
    {
        Players = new Player[numPlayers];
        for (int i = 0; i < numPlayers; i++)
        {
            Players[i] = new Player("Player " + (i + 1));
            Players[i].Type = Player.PlayerType.AI;
        }
        
        Players[0].Type = Player.PlayerType.LOCAL;
        currentPlayerIndex = 0;
    }

    public void AdvanceToNextPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % Players.Length;

        if (currentPlayerIndex == 0)
        {
            // New turn has begun!
            TurnNumber++;
            Debug.Log("STARTING TURN: " + TurnNumber);
        }

        Debug.Log("Starting turn for player index: " + currentPlayerIndex + " -- " + CurrentPlayer.PlayerName + " -- " + CurrentPlayer.Type.ToString());
    }

    /// /////////////Get Functions/////////////////////////////////////////////////

    public Hex GetHexAt(int x, int y)
    {
        if (hexes == null)
        {
            Debug.LogError("Hexes not yet instantiated");
            return null;
        }

        Debug.Log("hex instantiated");
        return hexes[x, y];

    }

    public GameObject GetUnitGO(Unit c)
    {
        if( unitToGameObjectMap.ContainsKey(c) )
        {
            return unitToGameObjectMap[c];
        }

        return null;
    }

    public GameObject GetStarGO(Star c)
    {
        if (starToGameObjectDic.ContainsKey(c))
        {
            return starToGameObjectDic[c];
        }

        return null;
    }

    public GameObject GetHexGO(Hex h)
    {
        if (HexToGameObjectMap.ContainsKey(h))
        {
            return HexToGameObjectMap[h];
        }

        return null;
    }

    public Hex GetHexFromGameObject(GameObject hexGO)
    {
        if (gameObjectToHexMap.ContainsKey(hexGO))
        {
            //Debug.Log("contains key");
            //Debug.Log(gameObjectToHexMap[hexGO]);
            return gameObjectToHexMap[hexGO];
        }

        //Debug.Log("contains no key");
        return null;
    }

    public Star GetStarAt(int x, int y)
    {
        if (starArray == null)
        {
            Debug.LogError("Star not yet instantiated");
            return null;
        }
        return starArray[x, y];

    }

    public Vector3 GetHexPosition(int q, int r)
    {
        Hex hex = GetHexAt(q, r);
        return GetHexPosition(hex);

    }

    public Vector3 GetHexPosition(Hex hex)
    {
        
        return hex.PositionFromCamera(Camera.main.transform.position, numRows, numColumns);

    }

    public Hex[] GetHexesWithinRadiusOf(Hex centerHex, int radius, int row, int column)
    {

        List<Hex> results = new List<Hex>();



        for (int dx = row - radius; dx <= row + radius; dx++)
        {
            for (int dy = column - radius; dy <= column + radius; dy++)
            {
                if (Mathf.Sqrt(((Mathf.Abs(row - dx) * Mathf.Abs(row - dx)) + Mathf.Abs(column - dy) * Mathf.Abs(column - dy))) + 1 <= radius)
                {
                    results.Add(hexes[dx, dy]);
                }
            }
        }
        return results.ToArray();
    }


    /************** MAPGEN *******************
    *****************************************/

    //Generate the Map
    public void GenerateMap()
    {
        hexes = new Hex[numColumns, numRows];
        HexToGameObjectMap = new Dictionary<Hex, GameObject>();
        gameObjectToHexMap = new Dictionary<GameObject, Hex>();

        generateBlankMap();
             

            while (starsNum < starsMax)
            {


                for (int column = 0; column < numColumns; column++)
                {
                    hexModvar = 0;
                    for (int row = 0; row < numRows; row++)
                    {
                        int hexMod = hexModvar;
                        if (starsNum < starsMax)
                        {
                            GenerateStar(column, row, hexMod);
                            //Offsets the edges
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
         UpdateHexVisuals();
         Unit unit = new Unit();
         SpawnUnitAt(unit, UnitShip, 10, 10);
        



    }
 
 /**********************************************************************************  
 **********************************************************************************/
    //makes baseline grid
    public void generateBlankMap()
    {
        for (int column = 0; column < numColumns; column++)
        {
            hexModvar = 0;
            for (int row = 0; row < numRows; row++)
            {
                int hexMod = hexModvar;
                Hex h = new Hex(column - hexMod, row);
                h.Elevation = 0;

                hexes[column, row] = h;
                GameObject hexGO = Instantiate(HexPrefab, h.Position(), Quaternion.identity, this.transform);
                hexGO.transform.Rotate(-90, 0, 0);
                HexToGameObjectMap[h] = hexGO;
                gameObjectToHexMap[hexGO] = h;


                hexGO.name = string.Format("HEX: {0},{1}", column, row);


                MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
                mr.material = MatSpace;
                //Offsets the edges
                if (rowCounter % 2 == 0)
                {                  
                    hexModvar++;
                }
                rowCounter++;
            }
        }
    }

/**********************************************************************************  
**********************************************************************************/
    //loops through grid and changes the hex visuals
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
                if (h.Elevation == 2)
                {
                    



                }
                else if (h.Elevation == 1)
                {
                    hexGO = Instantiate(PlanetPrefab, h.Position(), Quaternion.identity, this.transform);
                    
                }
                else if (h.Elevation == 3)
                {
                    hexGO = Instantiate(AsteroidPrefab, h.Position(), Quaternion.identity, this.transform);
                    
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

/**********************************************************************************  
**********************************************************************************/
    //Randomly Generates Star Locations
    public void GenerateStar(int column, int row, int hexMod)
    {
        int starGen;
        starGen = Random.Range(1, 1000);
        if (starGen < 2 && column > 4 && column < numColumns - 4 && row > 4 && row < numRows - 4)
        {
            Hex h = new Hex(column - hexMod, row);
            if (h.Elevation != 2 && h.Elevation != 1)
                
                ElevateArea(column, row, 5);
            starArray = new Star[column, row];
            starsNum++;
        }
    }
/**********************************************************************************  
**********************************************************************************/
    //"Elevates" the area within the radius given
    void ElevateArea(int q, int r, int radius)
    {
        Hex centerHex = GetHexAt(q, r);
        //Debug.Log(centerHex);
        Hex[] areaHexes = GetHexesWithinRadiusOf(centerHex, radius, q, r);
        foreach (Hex h in areaHexes)
        {
            int planetGen;
            planetGen = Random.Range(1, 18);
            if (planetGen < 2)
            {
                if (h.Elevation != 2)
                    h.Elevation = 1;
            }
        }
        Star newstar = new Star("star" + starsNum, q,r);
        SpawnStarAt(newstar, StarPrefab, q, r);
        centerHex.Elevation = 2;

    }
/**********************************************************************************  
**********************************************************************************/

    //Spawns a Star at a location
    public void SpawnStarAt(Star star, GameObject starprefab, int q, int r)
    {
        starToGameObjectDic = new Dictionary<Star, GameObject>();
        if (stars == null)
        {
            stars = new HashSet<Star>();
        }

        GameObject starHexGO = HexToGameObjectMap[GetHexAt(q, r)];
        star.SetHex(GetHexAt(q, r));
        star.Name = "star" + (starsNum+1);
        star.Name = string.Format("STAR: {0},{1}", q, r);


        GameObject starGO = Instantiate(starprefab, starHexGO.transform.position, Quaternion.identity, starHexGO.transform);
        stars.Add(star);
        starToGameObjectDic.Add(star, starGO);

    }





/**********************************************************************************  
**********************************************************************************/
    //Spawns a Unit at a location 
    public void SpawnUnitAt(Unit unit, GameObject prefab, int q, int r)
    {
        
        if (unitToGameObjectMap == null)
        {
            unitToGameObjectMap = new Dictionary<Unit, GameObject>();
        }
        
        if (units == null)
        {
            units = new HashSet<Unit>();
           
        }

        Hex myHex = GetHexAt(q, r);
        GameObject myHexGO = HexToGameObjectMap[myHex];
        unit.SetHex(myHex);

        GameObject unitGO = (GameObject)Instantiate(prefab, myHexGO.transform.position, Quaternion.identity, myHexGO.transform);
        units.Add(unit);
        unitToGameObjectMap.Add(unit, unitGO);
        gameObjectToHexMap.Add(unitGO, GetHexAt (q, r));

        unit.OnObjectMoved += unitGO.GetComponent<UnitView>().OnUnitMoved;

        CurrentPlayer.AddUnit(unit);
        unit.OnObjectDestroyed += OnUnitDestroyed;

        






    }

    public void OnUnitDestroyed(MapObject mo)
    {
        GameObject go = unitToGameObjectMap[(Unit)mo];
        unitToGameObjectMap.Remove((Unit)mo);
        Destroy(go);
    }

    IEnumerator DoAllUnitMoves()
    {
        foreach (Unit u in CurrentPlayer.Units)
        {
            yield return DoUnitMoves(u);
        }
    }

    public IEnumerator DoUnitMoves(Unit u)
    {

        while (u.DoMove())
        {
            Debug.Log("DoMove returned true -- will be called again.");
            // TODO: Check to see if an animation is playing, if so
            // wait for it to finish. 
            while (AnimationIsPlaying)
            {
                yield return null; 
            }

        }

    }







}









