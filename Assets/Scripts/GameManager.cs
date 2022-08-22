using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public struct Cord
{
    public int x, y;

    public Cord(int x, int y)
    {
            this.x = x;
            this.y = y;

            bool Equals(Cord cord)
            {
                return cord.x == x && cord.y == y;
            }
    }
}

public enum GameState
{
    gameplay, pause, gameOver, nextLevel
}

public class GameManager : MonoBehaviour
{
    public GameObject square;
    public Level[] levels = new Level[]{};
    private Transform boardParent;
    private Transform floor;
    

    private int levelInd = 0;
    Square[,] board;
    List<Cord> selected = new List<Cord>();
    bool isSelected = false;
    Color selectedColor;

    private GameState gameState = GameState.gameplay;
    float timeCounter;
    private int score = 0;
    GUI gui;
    int[] countToPoints = new int[] {0, 0, 0, 0, 1, 4, 8, 16, 32, 64, 128, 256, 512};
    
    void Start()
    {
        InitVars();
        LoadNextLevel();
    }

    void Update()
    {
        if(gameState == GameState.gameplay || gameState == GameState.pause)
        {
            if(gameState == GameState.gameplay)
                Move();
            UpdateTime();
            if(CheckGameOver())
            {
                gameState = GameState.nextLevel;
            }

            if(TimeOut())
            {
                gameState = GameState.gameOver;
                gui.gameOverPanel.SetActive(true);
                gui.timer.text = "00:00";
            }
        }

        if(gameState == GameState.nextLevel)
        {
            LoadNextLevel();
        }

    }

    private void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
    private void UpdateTime()
    {
        timeCounter -= Time.deltaTime;
        string minutes = Mathf.Floor(timeCounter / 60).ToString("00");
        string seconds = (timeCounter % 60).ToString("00");
     
        gui.timer.text = string.Format("{0}:{1}",minutes, seconds);
        
    }

    private void UpdateScore()
    {
        gui.score.text = score.ToString();
    }

    private void Move()
    {

        if(Input.GetMouseButtonDown(0) && !isSelected)
        {
            Cord cord;
            
            if(MouseToCord(out cord))
            {
                if(board[cord.x, cord.y] != null)
                {
                    selected.Add(cord);
                    board[cord.x, cord.y].SetColor(Color.black);
                    isSelected = true;     
                    selectedColor = board[cord.x, cord.y].color;
                }
            }
        }
       if(Input.GetMouseButton(0) && isSelected)
       {
           Cord cord;

           if(MouseToCord(out cord))
           {
                if(board[cord.x, cord.y] != null && board[cord.x, cord.y].color == selectedColor &&  !selected.Contains(cord) && Valid(cord, selected[selected.Count-1]) && selected.Count < 12)
                {
                    selected.Add(cord);
                }
                else if(selected.Count > 1 && cord.Equals(selected[selected.Count-2]))
                {
                    Cord lastCord = selected[selected.Count-1];
                    board[lastCord.x, lastCord.y].ResetColor();
                    selected.RemoveAt(selected.Count-1);
                }
           }


           if(selected.Count == 12)
           {
               for(int i = 0; i < 12; i++)
               {
                   board[selected[i].x, selected[i].y].SetColor(new Color(0,0,0,0.85f) );
               }
           }
           else
           {
                for(int i = 0; i < selected.Count; i++)
               {
                   board[selected[i].x, selected[i].y].SetColor(new Color(0,0,0, 1f));
               }
           }
       }

       if(Input.GetMouseButtonUp(0) && isSelected)
       {
            isSelected = false;
            score += countToPoints[selected.Count];
            UpdateScore();
            if(selected.Count >= 4 && selected.Count <= 12)
            {
                    for(int i = 0; i < selected.Count; i++)
                    { 
                        Destroy(board[selected[i].x, selected[i].y].gameObject);
                        board[selected[i].x, selected[i].y] = null;
                    }
            }
            else
            {
                    for(int i = 0; i < selected.Count; i++)
                    { 
                        board[selected[i].x, selected[i].y].ResetColor();
                    }
            }
            


            selected.Clear();
            UpdateBoard();
            Debug.Log(CheckGameOver());
       }
    }

    private void DestroyAllSquares()
    {
        foreach(Transform child in boardParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetBoard(ArrayLayout board)
    {
        DestroyAllSquares();
        
        floor = GameObject.Find("Floor").GetComponent<Transform>();

        int rows = board.rows.Length;
        int columns = board.rows[0].row.Length;
        this.board = new Square[columns, rows];
        floor.position = new Vector2(0f, -rows/2f);
        Camera.main.orthographicSize = (5f/8f) * Mathf.Max(columns, rows);
        for(int i = 0; i < columns; i++)
        {
            for(int j = 0; j < rows; j++)
            {
                Cord cord = new Cord(i, j);
                Square sq = Instantiate(square, CordToPos(cord), Quaternion.identity ,boardParent).GetComponent<Square>();
                sq.color = board.rows[j].GetColor(i);
                sq.ResetColor();
                this.board[i,j] = sq;
            }   
        }

    }

    private Vector2 CordToPos(Cord cord)
    {
        return new Vector2(cord.x - (float)(board.GetLength(0)-1)/2f, ((float)board.GetLength(1)-1)/2f - cord.y);
    }


    private bool MouseToCord(out Cord cord)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float a = board.GetLength(0) % 2 == 0 ? 0 : 0.5f;
        float b = board.GetLength(1) % 2 == 0 ? 0 : 0.5f;
        cord = new Cord( (int)(Mathf.FloorToInt(worldPosition.x + a) + (int)((board.GetLength(0))/2)), (int)( (int)((board.GetLength(1))/2) -  Mathf.CeilToInt(worldPosition.y-b)));
        return cord.x >= 0 && cord.x < board.GetLength(0) && cord.y >= 0 && cord.y < board.GetLength(1);
    }

    private bool Valid(Cord cord1, Cord cord2)
    {
        bool a = ((cord1.x + 1 == cord2.x || cord1.x - 1 == cord2.x)  && cord1.y == cord2.y) || ((cord1.y + 1 == cord2.y  || cord1.y - 1 == cord2.y) && cord1.x == cord2.x);
        
        return a;
    }

    private bool ContainsCord(Cord cord)
    {
        for(int i = 0; i < selected.Count; i++)
        {
            if(selected[i].x == cord.x && selected[i].y == cord.y)
                return true;
        }

        return false;
    }

    private void UpdateBoard()
    {
        for(int i = 0; i < board.GetLength(0); i++)
        {
            for(int j = 0; j < board.GetLength(1); j++)
            {
                if(board[i, j] == null)
                {
                    for(int k = j-1; k >= 0; k--)
                    {
                        board[i, k + 1] = board[i, k];
                        board[i,k] = null;
                    }
                }
            }
        }
    }

    private bool SquareIsFill(Cord cord, Color color)
    {
        return cord.x - 1 >= 0 && cord.x + 1 < board.GetLength(0) && cord.y - 1 >= 0 && cord.y + 1 < board.GetLength(1) &&
         board[cord.x - 1, cord.y]  &&  board[cord.x + 1, cord.y] != null  && board[cord.x, cord.y - 1] != null  &&  board[cord.x, cord.y + 1] != null &&
         board[cord.x-1, cord.y].color == color && board[cord.x+1, cord.y].color == color && board[cord.x, cord.y-1].color == color && board[cord.x, cord.y + 1].color == color;
    }

    private int CountMovesFromRight(Cord cord)
    {
        int ans = 1;
        if(cord.y + 1 < board.GetLength(1) && board[cord.x, cord.y + 1] != null &&  board[cord.x, cord.y + 1].color  == board[cord.x, cord.y].color)
        {
            ans = Mathf.Max(ans, ans + CountMovesFromRight(new Cord(cord.x, cord.y+1)));
        }
        if(cord.x + 1 < board.GetLength(0) && board[cord.x+1, cord.y] != null &&  board[cord.x+1, cord.y].color  == board[cord.x, cord.y].color)
        {
            ans = Mathf.Max(ans, ans + CountMovesFromRight(new Cord(cord.x + 1, cord.y)));
        }
        return ans;
    }

    private int CountMovesFromLeft(Cord cord)
    {
        int ans = 1;
        if(cord.y + 1 < board.GetLength(1) && board[cord.x, cord.y + 1] != null &&  board[cord.x, cord.y + 1].color  == board[cord.x, cord.y].color)
        {
            ans = Mathf.Max(ans, ans + CountMovesFromLeft(new Cord(cord.x, cord.y+1)));
        }
        if(cord.x - 1 >= 0 && board[cord.x-1, cord.y] != null &&  board[cord.x-1, cord.y].color  == board[cord.x, cord.y].color)
        {
            ans = Mathf.Max(ans, ans + CountMovesFromLeft(new Cord(cord.x - 1, cord.y)));
        }
        return ans;
    }
    private bool IsInSquare(Cord cord)
    {
        Color c = board[cord.x, cord.y].color;
        return cord.x+1 < board.GetLength(0) && cord.y + 1 < board.GetLength(1) && 
         null != board[cord.x+1, cord.y] && null != board[cord.x,cord.y+1] &&  null != board[cord.x+1,cord.y+1] && 
        c == board[cord.x+1, cord.y].color && c == board[cord.x,cord.y+1].color &&  c == board[cord.x+1,cord.y+1].color; 
    }

    private void InitVars()
    {
        gui = FindObjectOfType<GUI>();
        boardParent = GameObject.Find("Board").transform;
    }



    private void LoadNextLevel()
    {
        if(levelInd >= levels.Length)
        {
            SceneManager.LoadScene(2);
            return;
        }
        Level level = levels[levelInd];
        timeCounter = level.maxTime;
        SetBoard(level.board);
        gui.DisableAllPanels();
        gui.gameplayPanel.SetActive(true);
        levelInd++;
        gameState = GameState.gameplay;
        gui.levelName.text = "Poziom: " + level.levelName;
    }
    private bool TimeOut()
    {
        if(timeCounter < 0f)
            return true;
        return false;
    }
    private bool CheckGameOver()
    {
        int max = 0;
        bool[,] boardVisited = new bool[board.GetLength(0), board.GetLength(1)];

        for(int i = 0; i < board.GetLength(0); i++)
        {
            for(int j = 0; j < board.GetLength(1); j++)
            {
                boardVisited[i, j] = false;
            }
        }

        for(int i = 0; i < board.GetLength(0); i++)
        {
            for(int j = 0; j < board.GetLength(1); j++)
            {
                if(!boardVisited[i, j] && board[i, j] != null)
                {
                    Color color = board[i, j].color;
                    Queue<Cord> toCheck = new Queue<Cord>();
                    toCheck.Enqueue(new Cord(i, j));
                    int visitedSquaresCount = 0;
      
                    while(toCheck.Count > 0)
                    {
                        Cord cord = toCheck.Dequeue();
                        visitedSquaresCount++;
                        boardVisited[cord.x, cord.y] = true;


                        if(cord.x - 1 >= 0 && board[cord.x - 1, cord.y] != null && board[cord.x-1, cord.y].color == color && !boardVisited[cord.x-1, cord.y])
                        {
                            toCheck.Enqueue(new Cord(cord.x - 1, cord.y));
                            boardVisited[cord.x - 1, cord.y] = true;
                        }

                        if(cord.x + 1 < board.GetLength(0) &&  board[cord.x + 1, cord.y] != null && board[cord.x+1, cord.y].color == color && !boardVisited[cord.x+1, cord.y])
                        {
                            toCheck.Enqueue(new Cord(cord.x + 1, cord.y));
                            boardVisited[cord.x + 1, cord.y] = true;
                        }


                        if(cord.y - 1 >= 0 && board[cord.x, cord.y - 1] != null && board[cord.x, cord.y - 1].color == color && !boardVisited[cord.x, cord.y - 1])
                        {
                            toCheck.Enqueue(new Cord(cord.x, cord.y - 1));
                            boardVisited[cord.x, cord.y - 1] = true;
                        }

                        if(cord.y + 1 < board.GetLength(1) &&  board[cord.x, cord.y + 1] != null && board[cord.x, cord.y + 1].color == color && !boardVisited[cord.x, cord.y + 1])
                        {
                            toCheck.Enqueue(new Cord(cord.x, cord.y + 1));
                            boardVisited[cord.x, cord.y + 1] = true;
                        }
                    }

                    max = visitedSquaresCount > max ? visitedSquaresCount : max;


                }
            }
        }
        bool pos = true;
        
        for(int i = 0; i < board.GetLength(0); i++)
        {
            for(int j = 0; j < board.GetLength(1); j++)
            {
                if(board[i, j] == null)
                    continue;
                if(CountMovesFromLeft(new Cord(i, j)) > 3 || CountMovesFromRight(new Cord(i, j)) > 3 || IsInSquare(new Cord(i, j)))
                    pos = false;
            }
        }
        return max > 5 ? false : pos;
    }

    private void LoadGameOverScreen()
    {
        gui.gameOverPanel.SetActive(true);
    }
}