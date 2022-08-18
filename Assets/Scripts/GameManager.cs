using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject square;

    int level = 0;
    Square[,] board;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetBoard(Color[,] board)
    {
        this.board = new Square[board.GetLength(0), board.GetLength(1)];
        
        for(int i = 0; i < board.GetLength(0); i++)
        {
            for(int j = 0; j < board.GetLength(1); j++)
            {
                Square sq = Instantiate(square).GetComponent<Square>();
                sq.color = board[i, j];
                this.board[i,j] = sq;
            }   
        }

    }
}
