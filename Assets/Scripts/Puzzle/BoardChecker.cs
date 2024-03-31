using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class BoardChecker : MonoBehaviour
{
    
    int m_adjacentTiles;
    //TODO set these with an init function and not on every function call
    int m_boardWidth;
    int m_boardHeight;
    public bool AvailableMoves(Tile[,] currentBoard, int width, int height)
    {
        m_boardWidth = width;
        m_boardHeight = height;
        for (int i = 0; i < m_boardHeight; i++)
        {
            for (int j = 0; j < m_boardWidth; j++)
            {
                if (AvailableMoveFromTile(currentBoard[j, i], currentBoard, new Vector2(-1, -1)))
                {
                    return true;
                }
            }
        }
        //shuffle board:
        return false;
    }
    private bool AvailableMoveFromTile(Tile tile, Tile[,] board, Vector2 prevAvailable)
    {
        for (int i = (int)tile.m_coordinates.y - 1; i <= (int)tile.m_coordinates.y + 1; i++)
        {
            for (int j = (int)tile.m_coordinates.x - 1; j <= (int)tile.m_coordinates.x + 1; j++)
            {
                if (j == (int)tile.m_coordinates.x && i == (int)tile.m_coordinates.y)// compare x to j and y to i?, also in some other spot later in this functiuon...
                {
                    //this is the current tile, skip
                    continue;
                }
                if(j < 0 || j > m_boardWidth-1 || i < 0 || i > m_boardHeight-1)
                {
                    //checked spot is outside of board
                    continue;
                }
                if (prevAvailable.x != -1)
                {
                    //now checking for third tile, dont look for next matching tile from that
                    if (j == (int)prevAvailable.x && i == (int)prevAvailable.y)
                    {
                        continue;
                    }
                }
                if (tile.m_config.tileType == board[j, i].m_config.tileType)
                {
                    //tile at [j,i] is same type, add to adjacent counting
                    m_adjacentTiles++;
                    if (m_adjacentTiles == 2)
                    {
                        m_adjacentTiles = 0;
                        return true;
                    }
                    if (m_adjacentTiles == 1)
                    {
                        //if only one adjacent tile, need to check for one more
                        //if we cant find a new adjacent tile for that, need to go back
                        if (AvailableMoveFromTile(board[j, i], board, tile.m_coordinates))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        m_adjacentTiles = 0;
        return false;
    }
}
