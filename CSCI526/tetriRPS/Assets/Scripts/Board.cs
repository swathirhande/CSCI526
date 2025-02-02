
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Text gameOverText;
    public Text scoreText;
    public GameObject panel;
    public GameObject scorePanel;
    public Text finalscore;
    private int finalScore = 0;


    private int score = 0;



    private bool isGameOver = false;


    public RectInt Bounds
    {
        get
        {

            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < this.tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize();
        }
    }

    public void Start()
    {
        SpawnPiece();
        UpdateScoreText();
        scorePanel.SetActive(true);
    }

    public void SpawnPiece()
    {

        
        if (IsGameOver())
        {
            isGameOver = true;
            
            Debug.Log("Game ended");
            return;
        }




        int random = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];

        this.activePiece.Initialize(this, this.spawnPosition, data);
        Set(this.activePiece);

    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogError("Score Text is not assigned in the Inspector!");
        }
    }

    private bool IsGameOver()
    {

        int topRowY = boardSize.y / 2 - 1;
        int secondTopRowY = topRowY - 1;


        for (int x = -boardSize.x / 2; x < boardSize.x / 2; x++)
        {
            Vector3Int topRowPos = new Vector3Int(x, topRowY, 0);
            Vector3Int secondTopRowPos = new Vector3Int(x, secondTopRowY, 0);

            if (tilemap.HasTile(secondTopRowPos))
            {
                Debug.Log("Second top row has a tile. Game over!");
                if (gameOverText != null)
                {
                    gameOverText.gameObject.SetActive(true);
                    panel.gameObject.SetActive(true);
                    scoreText.gameObject.SetActive(true);
                    gameOverText.text += $"\nFinal Score: {score}";
                    finalScore = score;
                    finalscore.text = "FINAL SCORE: " + finalScore;
                    finalscore.gameObject.SetActive(true);
                    gameOverText.text = "GAME OVER!"; 
                }
                else
                {
                    Debug.LogError("Game Over Text is not assigned in the Inspector!");
                }
                return true; 
            }
        }

        return false;
    }



    public void Set(Piece piece)
    {

        if (isGameOver) return;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {

        if (isGameOver) return;


        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }
    public bool IsValidPosition(Piece piece, Vector3Int position)
    {

        if (isGameOver) return false;

        RectInt bounds = this.Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            if (this.tilemap.HasTile(tilePosition))
            {
                return false;
            }

        }
        return true;
    }

    public void CheckAndClearTiles(Piece piece, TileBase[] hasTileArray)
    {
        List<Vector3Int> clearedTiles = new List<Vector3Int>();
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            if (hasTileArray[i] == null)
            {
                continue;
            }
            Vector3Int[] directions = new Vector3Int[]
            {
            new Vector3Int(1, 0, 0), // Right
            new Vector3Int(-1, 0, 0), // Left
            new Vector3Int(0, 1, 0), // Up
            new Vector3Int(0, -1, 0) // Down
            };

            foreach (Vector3Int direction in directions)
            {
                Vector3Int neighborPosition = tilePosition + direction;

                if (tilemap.HasTile(neighborPosition))
                {

                    Tetromino neighborTetromino = GetTetrominoAtPosition(neighborPosition);
                    Tetromino currentTetromino = piece.data.tetromino;

                    if (ShouldClearTile(currentTetromino, neighborTetromino))
                    {
                        tilemap.SetTile(tilePosition, null);
                        tilemap.SetTile(neighborPosition, null);
                        clearedTiles.Add(tilePosition);
                        clearedTiles.Add(neighborPosition);

                        score += 10;
                        Debug.Log($"Score: {score}");
                        UpdateScoreText();
                        break;
                    }
                }
            }
        }
        piece.updateArray(hasTileArray);
        //MoveTilesDown(clearedTiles);
    }

        private void MoveTilesDown(List<Vector3Int> clearedTiles)
    {

        clearedTiles.Sort((a, b) => b.y.CompareTo(a.y));


        HashSet<Vector3Int> processedTiles = new HashSet<Vector3Int>();

        foreach (Vector3Int tile in clearedTiles)
        {

            Vector3Int aboveTilePosition = tile + new Vector3Int(0, 1, 0);


            if (tilemap.HasTile(aboveTilePosition) && !processedTiles.Contains(aboveTilePosition))
            {

                TileBase tileToMove = tilemap.GetTile(aboveTilePosition);
                if (tileToMove != null)
                {
                    Vector3Int newPosition = tile + new Vector3Int(0, -1, 0);


                    if (newPosition.y >= Bounds.min.y)
                    {
                        tilemap.SetTile(newPosition, tileToMove);
                        tilemap.SetTile(aboveTilePosition, null);
                        processedTiles.Add(aboveTilePosition);
                    }
                    else
                    {

                        tilemap.SetTile(tile, null);
                    }
                }
            }
        }


        bool hasMoved;
        do
        {
            hasMoved = false;

            foreach (Vector3Int tile in clearedTiles)
            {
                Vector3Int aboveTilePosition = tile + new Vector3Int(0, 1, 0);


                if (tilemap.HasTile(aboveTilePosition) && !processedTiles.Contains(aboveTilePosition))
                {

                    TileBase tileToMove = tilemap.GetTile(aboveTilePosition);
                    if (tileToMove != null)
                    {
                        Vector3Int newPosition = tile + new Vector3Int(0, -1, 0);


                        if (newPosition.y >= Bounds.min.y)
                        {
                            tilemap.SetTile(newPosition, tileToMove);
                            tilemap.SetTile(aboveTilePosition, null);
                            processedTiles.Add(aboveTilePosition);
                            hasMoved = true;
                        }
                        else
                        {

                            tilemap.SetTile(tile, null);
                        }
                    }
                }
            }
        } while (hasMoved);
    }



    private bool ShouldClearTile(Tetromino current, Tetromino neighbor)
    {

        if ((current == Tetromino.I || current == Tetromino.J || current == Tetromino.S) &&
            (neighbor == Tetromino.T || neighbor == Tetromino.Z))
        {
            return true;
        }
        else if ((current == Tetromino.O || current == Tetromino.L) &&
                 (neighbor == Tetromino.I || neighbor == Tetromino.J || neighbor == Tetromino.S))
        {
            return true;
        }
        else if ((current == Tetromino.T || current == Tetromino.Z) &&
                 (neighbor == Tetromino.O || neighbor == Tetromino.L))
        {
            return true;
        }

        return false;
    }


    private Tetromino GetTetrominoAtPosition(Vector3Int position)
    {
        TileBase tile = tilemap.GetTile(position);

        foreach (TetrominoData data in tetrominoes)
        {
            if (data.tile == tile)
            {
                return data.tetromino;
            }
        }

        return Tetromino.I;
    }

}





