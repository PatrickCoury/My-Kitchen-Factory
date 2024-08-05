using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class SaveGame
{
    public int[] tileMap, buildMap, buildMapRotation, hotBarID, buildMapX, buildMapY;
    public string saveName, saveDate;
    public int mapSizeX, mapSizeY, money, playerPositionX, playerPositionY;
    public SaveGame(TileID[,] tileMap, string saveName, DateTime saveDate, int mapSizeX, int mapSizeY, TileID[,] buildMap, int money, int playerPositionX, int playerPositionY, int[] hotBarID, List<GameObject> harvesterList)
    {
        this.tileMap = new int[tileMap.GetLength(0) * tileMap.GetLength(1)];
        this.buildMap = new int[buildMap.GetLength(0) * buildMap.GetLength(1)];
        this.buildMapRotation = new int[buildMap.GetLength(0) * buildMap.GetLength(1)];
        this.buildMapX = new int[buildMap.GetLength(0) * buildMap.GetLength(1)];
        this.buildMapY = new int[buildMap.GetLength(0) * buildMap.GetLength(1)];
        int count = 0;
        for (int x = 0; x < tileMap.GetLength(0); x++)
        {
            for (int y = 0; y < tileMap.GetLength(1); y++)
            {
                this.tileMap[count] = tileMap[x, y].getID();
                if (buildMap[x, y] == null)
                {
                    this.buildMap[count] = -1;
                    this.buildMapRotation[count] = -1;
                    this.buildMapX[count] = -1;
                    this.buildMapY[count] = -1;
                }
                else
                {
                    this.buildMap[count] = buildMap[x, y].getID();
                    this.buildMapRotation[count] = (int)buildMap[x, y].getRotation();
                    this.buildMapX[count] = x;
                    this.buildMapY[count] = y;
                    
                }
                count++;
            }
        }
        this.saveName = saveName;
        this.saveDate = saveDate.ToString();
        this.mapSizeX = mapSizeX;
        this.mapSizeY = mapSizeY;
        this.playerPositionX = playerPositionX;
        this.playerPositionY = playerPositionY;
        this.hotBarID = hotBarID;
        this.money = money;
    }

}
