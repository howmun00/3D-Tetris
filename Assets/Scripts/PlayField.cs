using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayField : MonoBehaviour
{   
    public static PlayField instance;
    public int gridSizeX, gridSizeY, gridSizeZ;
    [Header("Blocks")]
    public GameObject[] blockList;
    public GameObject[] ghostList;

    [Header("PlayField Grid")]
    public GameObject bottomPlane;
    public GameObject N, S, W, E;

    int randomIndex;
    
    public Transform[,,] theGrid;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        theGrid = new Transform[gridSizeX, gridSizeY, gridSizeZ];
        CalculatePreview();
        SpawnNewBlock();
    }

    public Vector3 Round(Vector3 vec)
    {
        return new Vector3(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y), Mathf.RoundToInt(vec.z));
    }

    public bool CheckInsideGrid(Vector3 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridSizeX &&
                (int)pos.z >= 0 && (int)pos.z < gridSizeZ &&
                (int)pos.y >= 0 );
    }

    public void UpdateGrid(TetrisBlock block)
    {
        //delete possible parent objects
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int z = 0; z < gridSizeZ; z++)
            {
                for(int y = 0; y < gridSizeY; y++)
                {
                    if(theGrid[x, y, z] != null)
                    {
                        if(theGrid[x, y, z].parent == block.transform)
                        {
                            theGrid[x, y, z] = null;
                        }
                    }
                }
            }
        }

        //fill in all child objects
        foreach(Transform child in block.transform)
        {
            Vector3 pos = Round(child.position);

            Debug.Log(pos);

            if(pos.y < gridSizeY)
            {
                theGrid[(int)pos.x, (int)pos.y, (int)pos.z] = child;
            }
        }
    }

    public Transform GetTransformOnGridPos(Vector3 pos)
    {
        if(pos.y >gridSizeY-1)
        {
            return null;
        }
        else
        {
            return theGrid[(int)pos.x, (int)pos.y, (int)pos.z];
        }
    }

    public void SpawnNewBlock()
    {   //spawning location
        Vector3 spawnPoint = new Vector3((int)transform.position.x + (float)gridSizeX/2, 
                                         (int)transform.position.y + gridSizeY,
                                         (int)transform.position.z + (float)gridSizeZ/2);

        //spawning random blocks
        GameObject newBlock = Instantiate(blockList[randomIndex], spawnPoint, Quaternion.identity) as GameObject;

        //Ghost
        GameObject newGhost = Instantiate(ghostList[randomIndex], spawnPoint, Quaternion.identity) as GameObject;
        newGhost.GetComponent<GhostBlock>().SetParent(newBlock);

        CalculatePreview();
        Previewer.instance.ShowPreview(randomIndex); 
    }

    public void CalculatePreview()
    {
        randomIndex = Random.Range(0, blockList.Length);
    }

    public void DeleteLayer()
    {
        int layersCleared = 0;
        for (int y=gridSizeY-1; y>=0; y--)
        {
            //check full layer
            if(CheckFullLayer(y))
            {
                layersCleared++;
                //delete all blocks
                DeleteLayerAt(y);

                //move all down by one
                MoveAllLayerDown(y);
            }
        }
        if(layersCleared>0)
        {
            GameManager.instance.LayersCleared(layersCleared);
        } 
    }

    bool CheckFullLayer(int y)
    {
        for (int x = 0; x< gridSizeX; x++)
        {
            for(int z = 0; z < gridSizeZ; z++)
            {
                if(theGrid[x,y,z] == null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void DeleteLayerAt(int y)
    {
        for (int x = 0; x< gridSizeZ; x++)
        {
            for (int z = 0; z< gridSizeZ; z++ )
            {
                Destroy(theGrid[x,y,z].gameObject);
                theGrid[x,y,z] = null;
            }
        }
    }

    void MoveAllLayerDown(int y)
    {             //y
        for(int i = y; i< gridSizeY; i++)
        {
            MoveOneLayerDown(i);
        }
    }

    void MoveOneLayerDown(int y)
    {
        //check for collision
        for(int x = 0; x< gridSizeX; x++)
        {
            for(int z = 0; z< gridSizeZ; z++)
            {
                Debug.Log(theGrid[x, y ,z]);
                if(theGrid[x, y, z] !=null)
                {
                    theGrid[x, y-1, z] = theGrid[x, y, z];
                    theGrid[x, y, z] = null;
                    theGrid[x, y-1, z].position += Vector3.down;
                }
            }
        } 
    }

    void OnDrawGizmos()
    {
        if(bottomPlane != null)
        {
                //resizing bottom plane
                Vector3 scaler = new Vector3((float)gridSizeX/10, 0, (float)gridSizeZ/10);
                bottomPlane.transform.localScale = scaler;

                //reposition
                bottomPlane.transform.position = new Vector3(transform.position.x + (float)gridSizeX/2,
                                                             transform.position.y ,
                                                             transform.position.z + (float)gridSizeZ/2);

                //retile material
                bottomPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeZ);
        }

        if(N != null)
        {
                //resizing north plane
                Vector3 scaler = new Vector3((float)gridSizeX/10, 1, (float)gridSizeY/10);
                N.transform.localScale = scaler;

                //reposition
                N.transform.position = new Vector3(transform.position.x + (float)gridSizeX/2,
                                                             transform.position.y + (float)gridSizeY/2,
                                                             transform.position.z + gridSizeZ);

                //retile material
                N.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);
        }

        if(S != null)
        {
                //resizing south plane
                Vector3 scaler = new Vector3((float)gridSizeX/10, 1, (float)gridSizeY/10);
                S.transform.localScale = scaler;

                //reposition
                S.transform.position = new Vector3(transform.position.x + (float)gridSizeX/2, transform.position.y +(float)gridSizeY/2, transform.position.z);

                //retile material
                //S.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);
        }

        if(E != null)
        {
                //resizing east plane
                Vector3 scaler = new Vector3((float)gridSizeZ/10, 1, (float)gridSizeY/10);
                E.transform.localScale = scaler;

                //reposition
                E.transform.position = new Vector3(transform.position.x + gridSizeX, transform.position.y +(float)gridSizeY/2, transform.position.z + (float)gridSizeZ/2);

                //retile material
                E.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeZ, gridSizeY);
        }

        if(W != null)
        {
                //resizing west plane
                Vector3 scaler = new Vector3((float)gridSizeZ/10, 1, (float)gridSizeY/10);
                W.transform.localScale = scaler;

                //reposition
                W.transform.position = new Vector3(transform.position.x, transform.position.y +(float)gridSizeY/2, transform.position.z + (float)gridSizeZ/2);

                //retile material
                W.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeZ, gridSizeY);
        }
    }

}
