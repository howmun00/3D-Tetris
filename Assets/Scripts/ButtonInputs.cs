using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInputs : MonoBehaviour
{
    public static ButtonInputs instance;

    public GameObject[] rotateCanvases;
    public GameObject rotateCanvas;
    public GameObject moveCanvas;

    GameObject activeBlock;
    TetrisBlock activeTetris;

    public bool moveIsOn = true;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetInputs();
    }

    void RepositionToActiveBlock()
    {
        if(activeBlock != null)
        {
            transform.position = activeBlock.transform.position;
        }
    }

    public void SetActiveBlock(GameObject block, TetrisBlock tetris)
    {
        activeBlock = block;
        activeTetris = tetris;
    }

    void Update()
    {
        RepositionToActiveBlock();
    }

    public void MoveBlock(string direction)
    {
        if(activeBlock != null)
        {
            if(direction == "left")
            {
                activeTetris.SetInput(Vector3.left);
            }
            if(direction == "right")
            {
                activeTetris.SetInput(Vector3.right);
            }
            if(direction == "forward")
            {
                activeTetris.SetInput(Vector3.forward);
            }
            if(direction == "back")
            {
                activeTetris.SetInput(Vector3.back);
            }
        }
    }

    public void RotateBlock(string rotation)
    {
        if(activeBlock != null)
        {
            //X rotation
            if(rotation == "posX")
            {
                activeTetris.SetRotationInput(new Vector3(90, 0, 0));
            }
            if(rotation == "negX")
            {
                activeTetris.SetRotationInput(new Vector3(-90, 0, 0));
            }
            //Y rotation
            if(rotation == "posY")
            {
                activeTetris.SetRotationInput(new Vector3(0, 90, 0));
            }
            if(rotation == "negY")
            {
                activeTetris.SetRotationInput(new Vector3(0, -90, 0));
            }
            //Z rotation
            if(rotation == "posZ")
            {
                activeTetris.SetRotationInput(new Vector3(0, 0, 90));
            }
            if(rotation == "negZ")
            {
                activeTetris.SetRotationInput(new Vector3(0, 0, -90));
            }
        }
    }

    public void SwitchInputs()
    {
        moveIsOn = !moveIsOn;
        SetInputs();
    }

    void SetInputs()
    {
        moveCanvas.SetActive(moveIsOn);
        rotateCanvas.SetActive(!moveIsOn); 
    }

    public void SetHighSpeed()
    {
        activeTetris.SetSpeed();
    }

}
