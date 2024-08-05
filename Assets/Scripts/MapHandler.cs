using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Contains logic for the tileMap, which handles the base tile IDs and logic.
 * Contains logic for the buildMap, which handles all player built items.
 * Contains logic for the instanceGrid, which is the tileMap instantiated into gameObjects
 * Contains the payload for the saveGame, as it has by far the most data relevant.
 * Contains logic for generating a new map.
 */
public class MapHandler : MonoBehaviour
{
    public int waterSeeds=3, grassSeeds=7,cowSeeds=2,chickenSeeds=2,pigSeeds=2,fishSeeds=2, cowAmount=3, chickenAmount=3, pigAmount=3,fishAmount=3, smoothness=2;//waterSeeds+grassSeeds=10, other seeds 1-4 amounts go 1-5, smoothness 1-3
    public int mapSizeX=64,mapSizeY=36;
    public TileID[,] tileMap, buildMap;
    public GameObject[,] instanceGrid;
    public GameObject tilePrefab;
    public bool newGame=false,loadGame=false;
    public SaveGame loadingSave;
    public AudioSource noBuild;
    private MainSceneHandler mainSceneHandler;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        tileMap = new TileID[mapSizeX, mapSizeY];
        instanceGrid = new GameObject[mapSizeX, mapSizeY];
        buildMap = new TileID[mapSizeX, mapSizeY];
        //generateMap();
        //setSelectedTile(instanceGrid[0, 0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void loadMap()
    {
        mapSizeX = loadingSave.mapSizeX;
        mapSizeY = loadingSave.mapSizeY;
        tileMap = new TileID[mapSizeX, mapSizeY];
        buildMap = new TileID[mapSizeX, mapSizeY];
        int count = 0;
        for(int x = 0; x < mapSizeX; x++)
        {
            for(int y = 0; y<mapSizeY; y++)
            {
                tileMap[x, y] = new TileID(loadingSave.tileMap[count], x, y, 0);
                instanceGrid[x, y] = Instantiate(tilePrefab);
                tileMap[x, y].matchTile(instanceGrid[x, y], 0);
                count++;
            }
        }
    }
    public void generateMap()
    {
        
        int randomX, randomY;

        for (int i = 0; i < waterSeeds; i++)//generate # of water seeds
        {
            randomX = getRandomCoord(0);
            randomY = getRandomCoord(1);
            tileMap[randomX, randomY] = new TileID(4, randomX, randomY, 0);
            instanceGrid[randomX,randomY] = Instantiate(tilePrefab);
            tileMap[randomX, randomY].matchTile(instanceGrid[randomX, randomY], 0);
        }

        for (int i = 0; i < grassSeeds; i++)//generate # of grass seeds
        {
            randomX = getRandomCoord(0);
            randomY = getRandomCoord(1);
            tileMap[randomX, randomY] = new TileID(0, randomX, randomY, 0);
            instanceGrid[randomX, randomY] = Instantiate(tilePrefab);
            tileMap[randomX, randomY].matchTile(instanceGrid[randomX, randomY], 0);

        }
        bool mapContainsEmpty = true;
        while (mapContainsEmpty)
        {
            TileID[,] tempMap = new TileID[tileMap.GetLength(0), tileMap.GetLength(1)];
            for (int x = 0; x < tempMap.GetLength(0); x++)//establish tempMap
            {
                for (int y = 0; y < tempMap.GetLength(1); y++)
                {
                    if (tileMap[x, y] != null)//if not empty
                    {
                        if (x + 1 < tileMap.GetLength(0))//directly right isn't world limit
                        {
                            if (tileMap[x + 1, y] == null && tempMap[x + 1, y] == null)//if is empty
                            {
                                tempMap[x + 1, y] = new TileID(tileMap[x, y].getID(), x + 1, y, 0);
                                instanceGrid[x+1, y] = Instantiate(tilePrefab);
                                tempMap[x + 1, y].matchTile(instanceGrid[x + 1, y], 0);
                            }
                        }
                        if (x - 1 >= 0)//directly left isn't world limit
                        {
                            if (tileMap[x - 1, y] == null && tempMap[x - 1, y] == null)//if is empty
                            {
                                tempMap[x - 1, y] = new TileID(tileMap[x, y].getID(), x - 1, y, 0);
                                instanceGrid[x - 1, y] = Instantiate(tilePrefab);
                                tempMap[x - 1, y].matchTile(instanceGrid[x - 1, y], 0);
                            }
                        }
                        if (y + 1 < tileMap.GetLength(1))//directly up isn't world limit
                        {
                            if (tileMap[x, y + 1] == null && tempMap[x, y + 1] == null)//if is empty
                            {
                                tempMap[x, y + 1] = new TileID(tileMap[x, y].getID(), x, y + 1, 0);
                                instanceGrid[x, y + 1] = Instantiate(tilePrefab);
                                tempMap[x, y+1].matchTile(instanceGrid[x, y+1], 0);
                            }
                        }
                        if (y - 1 >= 0)//directly down isn't world limit
                        {
                            if (tileMap[x, y - 1] == null && tempMap[x, y - 1] == null)//if is empty
                            {
                                tempMap[x, y - 1] = new TileID(tileMap[x, y].getID(), x, y - 1, 0);
                                instanceGrid[x, y - 1] = Instantiate(tilePrefab);
                                tempMap[x, y - 1].matchTile(instanceGrid[x, y - 1], 0);
                            }
                        }
                    }
                }
            }
            for (int x = 0; x < tileMap.GetLength(0); x++)//transfer tempMap to tileMap
            {
                for (int y = 0; y < tileMap.GetLength(1); y++)
                {
                    if (tileMap[x, y] == null && tempMap[x, y] != null)//element of tileMap is empty and element of tempMap isn't
                    {
                        tileMap[x, y] = tempMap[x, y];

                    }
                }
            }
            mapContainsEmpty = false;
            for (int x = 0; x < tileMap.GetLength(0); x++)//check for empty elements
            {
                for (int y = 0; y < tileMap.GetLength(1); y++)
                {
                    if (tileMap[x, y] == null) mapContainsEmpty = true;
                }
            }
            
        }
        for(int k = 0; k<cowSeeds;k++)
            generateNode(1);
        for (int k = 0; k < chickenSeeds; k++)
            generateNode(2);
        for (int k = 0; k < pigSeeds; k++)
            generateNode(3);
        for (int k = 0; k < fishSeeds; k++)
            generateNode(5);
        ensmoothen();
        centerMiddles();
        centerMiddles();
        centerMiddles();
    }
    private void generateNode(int ID)
    {
        int X = 0;
        int Y = 0;
        bool nodeGenerated = false;
        while (!nodeGenerated)
        {
            X = getRandomCoord(0);
            Y = getRandomCoord(1);
            int bottomID = tileMap[X, Y].getID();
            TileID tile = tileMap[X, Y];
            tile.setID((ID+10));
            if (bottomID == 0 && tile.goesOnGrass)
            {
                changeTile(tile.getID(), X, Y);
                nodeGenerated = true;
            }
            else if (bottomID == 4 && tile.goesOnWater)
            {
                changeTile(tile.getID(), X, Y);
                nodeGenerated = true;
            }
            else
                tile.setID(bottomID);
        }
        Queue q = new Queue();
        int t = 5;
        {
            for (int x = 0; x < mapSizeX; x++)
            {
                for (int y = 0; y < mapSizeY; y++)
                {
                    if (tileSquareDistance(X, Y, x, y) <= t && tileGoesOn(X, Y, x, y))//distance <=t && tiles goes on grass/water
                    {
                        q.Enqueue(tileMap[x, y]);
                    }
                }
            }
        } 
        if(q.Count < (t * 2) * (t + 1))
        {
            for (int x = 0; x < mapSizeX; x++)
            {
                for (int y = 0; y < mapSizeY; y++)
                {
                    if (tileSquareDistance(X,Y,x,y) == t+1 && tileGoesOn(X, Y, x, y))//distance <=t && tiles goes on grass/water
                    {
                        q.Enqueue(tileMap[x, y]);
                    }
                }
            }
        }
        float amount = 0;
        float count = 0;
        float max = Mathf.Pow((1 + amount), 2) + (3 * amount);
        amount = getAmount(ID);
        while (count <= max)
        {
            foreach (TileID tile in q)
            {
                int x = tile.getCoords().Key;
                int y = tile.getCoords().Value;
                KeyValuePair<int, int> closestMiddle = getClosest(ID + 10, x, y);
                int distance = tileDistance(closestMiddle.Key, closestMiddle.Value, x, y);
                if (distance <= amount)
                {
                    changeTile(ID, x, y);
                    count++;
                }
                else if (distance <= amount + 1 && Random.Range(0f, 1f) < 0.75f)
                {
                    changeTile(ID, x, y);
                    count++;
                }
                else if (distance<=amount + 2 && Random.Range(0f, 1f) < 0.45f&& tileDistance(getClosest(ID, x, y).Key, getClosest(ID, x, y).Value, x, y)<=1)
                {
                    changeTile(ID, x, y);
                    count++;
                }
            }
        }
        }
    private void ensmoothen()
    {
        for (int j = 0; j < smoothness; j++)
        {
            for (int x = 0; x < mapSizeX; x++)
            {
                for (int y = 0; y < mapSizeY; y++)
                {
                    Queue q = new Queue();
                    if (x + 1 < mapSizeX)
                    {
                        q.Enqueue(tileMap[x + 1, y].getID());
                    }
                    if (x - 1 >= 0)
                    {
                        q.Enqueue(tileMap[x - 1, y].getID());
                    }
                    if (y + 1 < mapSizeY)
                    {
                        q.Enqueue(tileMap[x, y + 1].getID());
                    }
                    if (y - 1 >= 0)
                    {
                        q.Enqueue(tileMap[x, y - 1].getID());
                    }
                    int matchCount = 0;
                    Dictionary<int, int> dic = new Dictionary<int, int>();
                    foreach (int ID in q)
                    {
                        if (ID == tileMap[x, y].getID())
                            matchCount++;
                        else
                        {
                            try
                            {
                                dic.Add(ID, 1);
                            }
                            catch
                            {
                                dic[ID] = dic[ID] + 1;
                            }
                        }
                    }
                    if (matchCount > 1 || tileMap[x, y].getID() >= 10)
                        continue;
                    else
                    {
                        KeyValuePair<int, int> changeTo = new KeyValuePair<int, int>(tileMap[x, y].getID(), 0);
                        foreach (KeyValuePair<int, int> ID in dic)
                        {
                            if (ID.Value > changeTo.Value)
                            {
                                changeTo = ID;
                            }
                        }
                        changeTile(changeTo.Key, x, y);
                    }
                }
            }
        }
    }
    private void centerMiddles()
    {
        Queue q = new Queue();
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (tileMap[x, y].getName().Contains("middle"))
                    q.Enqueue(tileMap[x, y]);
            }
        }
        int right, left, up, down;

        foreach (TileID middle in q)
        {
            right = left = up = down = 1;
            while (middle.getCoords().Key + right < mapSizeX && tileMap[middle.getCoords().Key + right, middle.getCoords().Value].getID() == middle.getID() - 10)//probe right
                right++;
            while (middle.getCoords().Key - left >= 0 && tileMap[middle.getCoords().Key - left, middle.getCoords().Value].getID() == middle.getID() - 10)//probe left
                left++;
            while (middle.getCoords().Value + up < mapSizeY && tileMap[middle.getCoords().Key, middle.getCoords().Value + up].getID() == middle.getID() - 10)//probe up
                up++;
            while (middle.getCoords().Value - down >= 0 && tileMap[middle.getCoords().Key, middle.getCoords().Value - down].getID() == middle.getID() - 10)//probe down
                down++;
            int newX = middle.getCoords().Key + ((right - left) / 2);
            int newY = middle.getCoords().Value + ((up - down) / 2);
            int ID = middle.getID();
            changeTile(ID - 10, middle.getCoords().Key, middle.getCoords().Value);
            changeTile(ID, newX, newY);
            KeyValuePair<int, int> nextClosestMiddle = getNextClosest(ID, newX, newY);
            if (tileDistance(nextClosestMiddle.Key, nextClosestMiddle.Value, newX, newY) <= getAmount(ID - 10))
                changeTile(ID - 10, newX, newY);
            
        }
        Queue v = new Queue();
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (tileMap[x, y].getName().Contains("middle"))
                    v.Enqueue(tileMap[x, y]);
            }
        }
        foreach (TileID middle in v)
        {
            for (int x = 0; x < mapSizeX; x++)
            {
                for (int y = 0; y < mapSizeY; y++)
                {
                    if (tileSquareDistance(middle.getCoords().Key, middle.getCoords().Value, x, y) == 1 && tileMap[x, y].getID() != middle.getID() - 10)
                        changeTile(middle.getID() - 10, x, y);
                }
            }
        }
    }
    private int getRandomCoord(int xy)
    {
        return Random.Range(0, tileMap.GetLength(xy));
    }
    private void changeTile(int ID, int x, int y)
    {
        Destroy(instanceGrid[x, y]);
        tileMap[x, y].setID(ID);
        instanceGrid[x, y] = Instantiate(tilePrefab);
        tileMap[x, y].matchTile(instanceGrid[x, y], 0);
    }

    //only a minor affront to God
    public KeyValuePair<int, int> getClosest(int ID, int X, int Y)
    {
        if (tileMap[X, Y].getID() == ID)//if selected tile is correct ID
            return new KeyValuePair<int, int>(X, Y);

        //queues all tiles with relevant ID
        Queue q = new Queue();
        for (int x = 0; x < tileMap.GetLength(0); x++)
        {
            for (int y = 0; y < tileMap.GetLength(1); y++)
            {
                if (tileMap[x, y].getID() == ID)
                {
                    q.Enqueue(tileMap[x, y]);//stack contains all valid tiles on map
                }
            }
        }

        //this is the affront part, finds the minimum distance of all items in the queue
        KeyValuePair<int, int> coords = new KeyValuePair<int, int>(0, 0);
        foreach(TileID newTile in q)
        {
            
            if ((Mathf.Abs(X - coords.Key) + Mathf.Abs(Y - coords.Value)) > (Mathf.Abs(X - (int)newTile.getCoords().Key) + Mathf.Abs(Y - (int)newTile.getCoords().Value)))
                coords = newTile.getCoords();
    }
        return coords;
    }
    public KeyValuePair<int, int> getNextClosest(int ID, int X, int Y)
    {

        //queues all tiles with relevant ID
        Queue q = new Queue();
        for (int x = 0; x < tileMap.GetLength(0); x++)
        {
            for (int y = 0; y < tileMap.GetLength(1); y++)
            {
                if (tileMap[x, y].getID() == ID)
                {
                    q.Enqueue(tileMap[x, y]);//stack contains all valid tiles on map
                }
            }
        }

        //this is the affront part, finds the minimum distance of all items in the queue
        KeyValuePair<int, int> coords = new KeyValuePair<int, int>(0, 0);
        foreach (TileID newTile in q)
        {
            if (newTile.getCoords().Key == X && newTile.getCoords().Value == Y)
                continue;
            else if ((Mathf.Abs(X - coords.Key) + Mathf.Abs(Y - coords.Value)) > (Mathf.Abs(X - (int)newTile.getCoords().Key) + Mathf.Abs(Y - (int)newTile.getCoords().Value)))
                coords = newTile.getCoords();
        }
        return coords;
    }
    public bool tileGoesOn(int x, int y, int dx, int dy)
    {
        return ((tileMap[x, y].goesOnGrass && tileMap[dx, dy].getID() == 0) || (tileMap[x, y].goesOnWater && tileMap[dx, dy].getID() == 4));
    }
    public int tileSquareDistance(int X, int Y, int x, int y)
    {
        if (Mathf.Abs(X - x) > Mathf.Abs(Y - y))
            return (Mathf.Abs(X - x));
        else
            return Mathf.Abs(Y - y);
    }
    public int tileDistance(int X, int Y, int x, int y)
    {
        return (Mathf.Abs(X - x) + Mathf.Abs(Y - y));
    }
    public float getAmount(int ID) {
        switch (ID)
        {
            case 1:
                return cowAmount;
            case 2:
                return chickenAmount;
            case 3:
                return pigAmount;
            case 5:
                return fishAmount;
            default:
                return 0;
        }
    }
    public void setMainSceneHandler(MainSceneHandler mainSceneHandler)
    {
        this.mainSceneHandler = mainSceneHandler;
    }
    public bool goesHere(int X, int Y, TileID tile)
    {
        return ((tileMap[X, Y].isWater && tile.goesOnWater) || (tileMap[X, Y].isGrass && tile.goesOnGrass));
    }
    public GameObject buildTile(int ID, float r, int x, int y)
    {
        if (ID > 0)
        {
            TileID tile = new TileID(ID, x, y, r);
            if (buildMap[x, y] != null || !goesHere(x, y, tile) || !mainSceneHandler.spend(tile))
            {
                GameObject.Find("No Build").GetComponent<AudioSource>().Play();
                return null;
            }
            else
            {
                //Destroy(instanceGrid[x, y]);
                buildMap[x, y] = tile;
                instanceGrid[x, y] = Instantiate(tilePrefab);
                buildMap[x, y].matchTile(instanceGrid[x, y], -1);

            }
        }
        return instanceGrid[x, y];
    }
    public void sellTile(int x, int y)
    {
        TileID tile = new TileID(tileMap[x,y].getID(), x, y, 0);
        if (buildMap[x, y] != null)
        {
            mainSceneHandler.sell(buildMap[x, y]);
            Destroy(instanceGrid[x, y]);
            buildMap[x, y] = null;
            instanceGrid[x, y] = Instantiate(tilePrefab);
            tileMap[x,y].matchTile(instanceGrid[x, y], 0);
        }
    }
}

