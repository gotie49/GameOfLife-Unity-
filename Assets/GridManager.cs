using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public int width;
    public int height;

    int[,] grid;
    int[,] buffer;

    public Tile tile;
    public Tile[,] tiles;

    public Transform cam;
    
    [Range(0,100)]
    public int randomFillpercent;
    public string seed;
    public bool useRandomSeed;


    void Start()
    {
        InitGrid();

        InvokeRepeating(nameof(GameOfLife), 2f, 0.05f);
    }
    void InitGrid()
    {
        grid = new int[width,height];
        buffer = new int[width,height];
        tiles = new Tile[width,height];
        // grid[10,10]=1;
        // grid[11,10]=1;
        // grid[12,10]=1;
        RandomFillGrid();
        InstantiateGrid();        
    }
    void InstantiateGrid()
    {
        for(int x=0; x < width ; x++)
        {
            for(int y=0; y < height ; y++)
            {
                tiles[x,y] = Instantiate(tile, new Vector3(x,y),Quaternion.identity);
                tiles[x,y].name = "Tile" + x + " , " + y;
                bool isAlive = grid[x,y] == 1;  
                tiles[x,y].Init(isAlive);
            }
        }
        cam.transform.position = new Vector3((float)width/2 -.5f,(float)height/2 -.5f,-10);
    }
    void RandomFillGrid()
    {
        if(useRandomSeed)
        {
            seed = Time.time.ToString();
        }
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for(int x=0; x < width ; x++)
        {
            for(int y=0; y < height ; y++)
            {
                grid[x,y] = (pseudoRandom.Next(0,100)<randomFillpercent)? 1:0;
            }
        }
    }
    void UpdateGrid()
    {
            for(int x=0; x < width ; x++)
            {
                for(int y=0; y < height ; y++)
                {
                    buffer[x,y] = isAlive(grid,x,y)?1:0;
                }
            }         
    }
    bool isAlive(int[,] grid,int x, int y)
    {
        int aliveIndex = 0;
        //testing left side
        if(x>0 && grid[x-1,y] == 1) aliveIndex += 1;
        //testing the right side
        if(x<width-1 && grid[x+1,y] == 1) aliveIndex += 1;
        //testing top
        if(y>0 && grid[x,y-1] == 1) aliveIndex += 1;
        //testing bottom
        if(y<height-1 && grid[x,y+1] == 1) aliveIndex += 1;
        
        //testing top left
        if(x>0 && y>0 &&grid[x-1,y-1] ==1) aliveIndex += 1;
        //testing top right
        if(x<width-1 && y>0 && grid[x+1,y-1] == 1) aliveIndex += 1;
        //testing bottom left
        if(x>0 && y<height-1 && grid[x-1,y+1] == 1) aliveIndex += 1;
        //testing bottom right
        if(x<width-1 && y<height-1 && grid[x+1,y+1] == 1) aliveIndex += 1;

        //Rules:
        //1. alive and fewer than 2 alive neighbours(aliveIndex < 2) => dies
        if(grid[x,y] == 1 && aliveIndex < 2) return false;
        //2. alive and more than 3 alive neighbours(aliveIndex > 3) => dies
        if(grid[x,y] == 1 && aliveIndex > 3) return false;
        //3. alive and 2 or 3 alive neighbours => alive
        if(grid[x,y] == 1 && (aliveIndex == 2 || aliveIndex == 3)) return true;
        //4. dead and exactly 3 alive neighbours(aliveIndex == 3) => alive
        if(grid[x,y] == 0 && aliveIndex == 3) return true;

        return false;
    }
    void DrawGrid()
    {   
        for(int x =0; x < width; x++)
        {
            for(int y =0; y < height; y++)
            {
                if(buffer[x,y] ==  1)tiles[x,y].Init(true);
                else tiles[x,y].Init(false);
            }
        }
    }
    void GameOfLife()
    {
        UpdateGrid();
        DrawGrid();
        for(int x =0; x < width; x++)
        {
            for(int y =0; y < height; y++)
            {
                grid[x,y] = buffer[x,y];
            }
        }
    }
}
