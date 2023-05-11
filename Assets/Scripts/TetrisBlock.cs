using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    float prevTime;
    float fallTime = 1f;


    // Start is called before the first frame update
    void Start ()
    {
        ButtonInputs.instance.SetActiveBlock(gameObject, this);
        fallTime = GameManager.instance.ReadFallSpeed();
        if(!CheckValidMove())
        {
            GameManager.instance.SetGameIsOver();
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if(Time.time - prevTime > fallTime)
        {
            transform.position += Vector3.down;

            if(!CheckValidMove())
            {
                transform.position += Vector3.up;
                //delete layer if possible
                PlayField.instance.DeleteLayer();
                enabled = false;
                //create a new tetris block
                if(!GameManager.instance.ReadGameIsOver())
                {
                    PlayField.instance.SpawnNewBlock();
                }
            }
            else
            {
                //update the grid
                PlayField.instance.UpdateGrid(this);
            } 

            prevTime = Time.time;
        }

        //Player's Movement
        //key for left arrow 
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //SetInput(Vector3.left);
        }
        //key for right arrow 
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            //SetInput(Vector3.right);
        }
        //key for up arrow 
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            //SetInput(Vector3.forward);
        }
        //key for down arrow 
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            //SetInput(Vector3.down);
        }
    }
    //input for player's movement
    public void SetInput(Vector3 direction)
    {
        transform.position += direction;
        if(!CheckValidMove())
        {
            transform.position -= direction;
        }
        else
        {
            PlayField.instance.UpdateGrid(this);
        }
    }

    public void SetRotationInput(Vector3 rotation)
    {
        transform.Rotate(rotation, Space.World);
        if(!CheckValidMove())
        {
            transform.Rotate(-rotation, Space.World);
        }
        else
        {
            PlayField.instance.UpdateGrid(this);
        }
    }

    bool CheckValidMove()
    {
        foreach(Transform child in transform)
        {
            Vector3 pos = PlayField.instance.Round(child.position);

            if(!PlayField.instance.CheckInsideGrid(pos))
            {
                return false;
            }
        }

        foreach(Transform child in transform)
        {
            Vector3 pos = PlayField.instance.Round(child.position);
            Transform t = PlayField.instance.GetTransformOnGridPos(pos);
            if(t !=null && t.parent != transform)
            {
                return false;
            }
        }

        return true;
    }

    public void SetSpeed()
    {
        fallTime = 0.1f;
    }
}
