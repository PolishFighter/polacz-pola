using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public struct Coord
{
    public int x, y;

    public Coord(int x, int y)
    {
            this.x = x;
            this.y = y;

            bool Equals(Coord coord)
            {
                return coord.x == x && coord.y == y;
            }
    }
}

public enum GameState
{
    gameplay, gameOver, nextLevel
}

public class GameManager : MonoBehaviour
{
    public GameObject square;
    public AudioSource pop;
    public AudioSource nextLevelSound;
    public Level[] levels = new Level[]{};
    public int timeBonusX = 100;
    
    private Transform boardParent;
    private Transform floor;

    private Coord[] direction = new Coord[]{
        new Coord(-1, 0),
        new Coord(1, 0),
        new Coord(0, 1),
        new Coord(0, -1),
    };
    

    private int levelInd = 0;
    Square[,] board;
    List<Coord> selected = new List<Coord>();
    bool isSelected = false;
    Color selectedColor;

    private GameState gameState = GameState.gameplay;
    float timeCounter;
    public static int score = 0;
    GUI gui;
    int[] countToPoints = new int[] {0, 0, 0, 0, 1, 4, 8, 16, 32, 64, 128, 256, 512};
    
    void Start()
    {
        InitVars();
        LoadNextLevel();
    }

    void Update()
    {
        if(gameState == GameState.gameplay)
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
                InitGameOver();
                gui.gameOverPanel.SetActive(true);
                gui.timer.text = "00:00";
            }
        }

        if(gameState == GameState.nextLevel)
        {
            if(levelInd < levels.Length)
                nextLevelSound.Play();
            pop.Stop();
            AddBonusForTime();
            LoadNextLevel();
        }

    }
    private void InitGameOver()
    {
        gui.scoreGameOver.text = "Wynik\n" + score.ToString();

        if(PlayerPrefs.HasKey("HighScore"))
        {
            if( PlayerPrefs.GetInt("HighScore") < score)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
            else
            {
               gui.newHighScore.SetActive(false); 
            }
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", score);
            gui.newHighScore.SetActive(true);
        }

        gui.highScoreGameOver.text = "Najwyższy wynik\n" +  PlayerPrefs.GetInt("HighScore").ToString();
    }

    private void AddBonusForTime()
    {
        if(levelInd >= levels.Length)
            return;
        score +=  (int) ((1 - timeCounter/levels[levelInd].maxTime) * timeBonusX);
        UpdateScore();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
    private void UpdateTime()
    {
        timeCounter -= Time.deltaTime;
        int minutes = (int)Mathf.Floor(timeCounter / 60);
        int seconds = ((int) timeCounter % 60);
        if(minutes < 1 && seconds < 11)
        {
            gui.timer.color = Color.red;
        }
        else
        {
           gui.timer.color = Color.white; 
        }
        gui.timer.text = string.Format("{0}:{1}",minutes.ToString("00"), seconds.ToString("00"));
        
    }

    private void UpdateScore()
    {
        gui.score.text = score.ToString();
    }

    private void Move()
    {

        if(Input.GetMouseButtonDown(0) && !isSelected)
        {
            Coord coord;
            
            if(MouseTocoord(out coord))
            {
                if(board[coord.x, coord.y] != null)
                {
                    selected.Add(coord);
                    board[coord.x, coord.y].SetSelectedColor();
                    isSelected = true;     
                    selectedColor = board[coord.x, coord.y].color;
                }
            }
        }
       if(Input.GetMouseButton(0) && isSelected)
       {
           Coord coord;

           if(MouseTocoord(out coord))
           {
                if(board[coord.x, coord.y] != null && board[coord.x, coord.y].color == selectedColor &&  !selected.Contains(coord) && Valid(coord, selected[selected.Count-1]) && selected.Count < 12)
                {
                    selected.Add(coord);
                }
                else if(selected.Count > 1 && coord.Equals(selected[selected.Count-2]))
                {
                    Coord lastcoord = selected[selected.Count-1];
                    board[lastcoord.x, lastcoord.y].SetDefaultColor();
                    selected.RemoveAt(selected.Count-1);
                }
           }


           if(selected.Count == 12)
           {
               for(int i = 0; i < 12; i++)
               {
                   board[selected[i].x, selected[i].y].SetLimitColor();
               }
           }
           else
           {
                for(int i = 0; i < selected.Count; i++)
               {
                   board[selected[i].x, selected[i].y].SetSelectedColor();
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
                    pop.Play();
                    for(int i = 0; i < selected.Count; i++)
                    { 
                        board[selected[i].x, selected[i].y].Destroy();
                        board[selected[i].x, selected[i].y] = null;
                    }
            }
            else
            {
                    for(int i = 0; i < selected.Count; i++)
                    { 
                        board[selected[i].x, selected[i].y].SetDefaultColor();
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
                Coord coord = new Coord(i, j);
                Square sq = Instantiate(square, coordToPos(coord), Quaternion.identity ,boardParent).GetComponent<Square>();
                sq.color = board.rows[j].GetColor(i);
                sq.SetDefaultColor();
                this.board[i,j] = sq;
            }   
        }

    }

    private Vector2 coordToPos(Coord coord)
    {
        return new Vector2(coord.x - (float)(board.GetLength(0)-1)/2f, ((float)board.GetLength(1)-1)/2f - coord.y);
    }


    private bool MouseTocoord(out Coord coord)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float a = board.GetLength(0) % 2 == 0 ? 0 : 0.5f;
        float b = board.GetLength(1) % 2 == 0 ? 0 : 0.5f;
        coord = new Coord( (int)(Mathf.FloorToInt(worldPosition.x + a) + (int)((board.GetLength(0))/2)), (int)( (int)((board.GetLength(1))/2) -  Mathf.CeilToInt(worldPosition.y-b)));
        return coord.x >= 0 && coord.x < board.GetLength(0) && coord.y >= 0 && coord.y < board.GetLength(1);
    }

    private bool Valid(Coord coord1, Coord coord2)
    {
        bool a = ((coord1.x + 1 == coord2.x || coord1.x - 1 == coord2.x)  && coord1.y == coord2.y) || ((coord1.y + 1 == coord2.y  || coord1.y - 1 == coord2.y) && coord1.x == coord2.x);
        
        return a;
    }

    private bool Containscoord(Coord coord)
    {
        for(int i = 0; i < selected.Count; i++)
        {
            if(selected[i].x == coord.x && selected[i].y == coord.y)
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

    private void InitVars()
    {
        score = 0;
        gui = FindObjectOfType<GUI>();
        boardParent = GameObject.Find("Board").transform;
    }



    private void LoadNextLevel()
    {
        if(levelInd >= levels.Length)
        {
            InitGameOver();
            gui.gameOverPanel.SetActive(false);
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

    private void ClearBoardVisited(ref bool[,] boardVisited)
    {
        for(int i = 0; i < board.GetLength(0); i++)
        {
                for(int j = 0; j < board.GetLength(1); j++)
                {
                    boardVisited[i, j] = false;
                }
        }
    }
    private int FindMaxPath(Color color,Coord coord, ref bool[,] boardVisited)
    {
        boardVisited[coord.x, coord.y] = true;
        bool minOne = false;
        int ans = 0;
        for(int i = 0; i < 4; i++)
        {
            
            if(
                coord.x + direction[i].x >= 0 && coord.x + direction[i].x < board.GetLength(0) &&
                coord.y + direction[i].y >= 0 && coord.y + direction[i].y < board.GetLength(1) &&
                board[coord.x + direction[i].x, coord.y + direction[i].y] != null &&
                board[coord.x + direction[i].x, coord.y + direction[i].y].color == color &&
                !boardVisited[coord.x + direction[i].x, coord.y + direction[i].y]
            )
            {
                minOne = true;
                ans = Mathf.Max(ans, 
                    FindMaxPath(color, new Coord(coord.x + direction[i].x, coord.y + direction[i].y),ref boardVisited)
                );
            } 
        }
        boardVisited[coord.x, coord.y] = false;
        if(!minOne)
            return 1;
        return ans + 1;
    }
    private bool CheckGameOver()
    {
        int max = 1;
        bool[,] boardVisited = new bool[board.GetLength(0),board.GetLength(1)];
        for(int i = 0; i < board.GetLength(0); i++)
        {
            for(int j = 0; j < board.GetLength(1); j++)
            {
                if(board[i, j] == null)
                    continue;

                ClearBoardVisited(ref boardVisited);
                int mP = FindMaxPath(board[i, j].color, new Coord(i, j), ref boardVisited);
                
                max = Mathf.Max(max, mP);
            }
        }
        return max < 4;
    }

    private void LoadGameOverScreen()
    {
        gui.gameOverPanel.SetActive(true);
    }
}