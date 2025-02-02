
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }


    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float lockTime;


    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;

        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length]; //can use even 4
        }
        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }
    private void Update()
    {

        this.board.Clear(this);
        this.lockTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector2Int.right);
        }

        // Soft drop
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector2Int.down);
        }

        //hard drop
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }
        if (Time.time >= this.stepTime)
        {
            Step();
        }


        this.board.Set(this);
    }
    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay;

        Move(Vector2Int.down);

        if (this.lockTime >= this.lockDelay)
        {
            Lock();
        }
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }
        Lock();
    }


    private void Lock()
    {
        this.board.Set(this);
        var hasTileArray = new TileBase[] { null, null, null, null };
        for (int i = 0; i < this.cells.Length; i++)
        {
            hasTileArray[i] = board.tilemap.GetTile(this.cells[i] + this.position);
        }
        board.CheckAndClearTiles(this, hasTileArray);
        bool newHasTile, hasUnder = false;
        var lowestY = new int[] { 10, 10, 10, 10 };
        while (hasUnder == false)
        {
            newHasTile = false;
            for (int i = 0; i < this.cells.Length; i++)
            {
                if (hasTileArray[i] != null)
                {
                    newHasTile = true;
                    if (this.cells[i].y < lowestY[this.cells[i].x + 1])
                        lowestY[this.cells[i].x + 1] = this.cells[i].y;
                }
            }
            if (newHasTile == false) break;
            for (int i = 0; i < this.cells.Length; i++)
            {
                Vector3Int temp = this.cells[i] + this.position;
                if (hasTileArray[i] != null &&
                    (temp.y == -10 || (lowestY[this.cells[i].x + 1] == this.cells[i].y &&
                    board.tilemap.HasTile(temp + new Vector3Int(0, -1, 0)) == true)))
                {
                    hasUnder = true;
                    break;
                }
            }
            if (hasUnder == false)
            {
                for (int i = 0; i < this.cells.Length; i++)
                {
                    if (hasTileArray[i] != null)
                    {
                        board.tilemap.SetTile(this.cells[i] + this.position, null);
                    }
                }
                for (int i = 0; i < this.cells.Length; i++)
                {
                    Vector3Int temp = this.cells[i] + this.position;
                    if (hasTileArray[i] != null)
                    {
                        board.tilemap.SetTile(temp + new Vector3Int(0, -1, 0), hasTileArray[i]);
                    }
                }
                this.position += new Vector3Int(0, -1, 0);
                board.CheckAndClearTiles(this, hasTileArray);
            }
        }
        /*
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3Int temp = this.cells[i] + this.position;
            if (temp.y > -10 && hasTileArray[i] != null &&
                board.tilemap.HasTile(temp + new Vector3Int(0, -1, 0)) == false &&
                board.tilemap.HasTile(temp + new Vector3Int(1, 0, 0)) == false &&
                board.tilemap.HasTile(temp + new Vector3Int(-1, 0, 0)) == false)
            {
                while (temp.y > -10 && hasTileArray[i] != null &&
                board.tilemap.HasTile(temp + new Vector3Int(0, -1, 0)) == false)
                {
                    board.tilemap.SetTile(temp + new Vector3Int(0, -1, 0), board.tilemap.GetTile(temp));
                    board.tilemap.SetTile(temp, null);
                    this.cells[i].y--;
                    temp.y--;
                    board.CheckAndClearTiles(this, hasTileArray);
                }
            }
        }
        */
        for(int i = -9;i < 8;i++)
        {
            for(int j = -5;j < 5;j++)
            {
                var temp = new Vector3Int(j, i, 0);
                if (board.tilemap.HasTile(temp) == true &&
                    board.tilemap.HasTile(temp + new Vector3Int(0, -1, 0)) == false &&
                    board.tilemap.HasTile(temp + new Vector3Int(1, 0, 0)) == false &&
                    board.tilemap.HasTile(temp + new Vector3Int(-1, 0, 0)) == false &&
                    board.tilemap.HasTile(temp + new Vector3Int(0, 1, 0)) == false)
                {
                    while (temp.y > -10 && board.tilemap.HasTile(temp) == true &&
                    board.tilemap.HasTile(temp + new Vector3Int(0, -1, 0)) == false)
                    {
                        board.tilemap.SetTile(temp + new Vector3Int(0, -1, 0), board.tilemap.GetTile(temp));
                        board.tilemap.SetTile(temp, null);
                        temp.y--;
                        board.CheckAndClearTiles(this, hasTileArray);
                    }
                }
            }
        }
        this.board.SpawnPiece();
    }

    public void updateArray(TileBase[] hasTileArray)
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            if (board.tilemap.GetTile(this.cells[i] + this.position) == null)
            {
                hasTileArray[i] = null;
            }
        }
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this, newPosition);
        if (valid)
        {
            this.position = newPosition;
            this.lockTime = 0f;
        }
        return valid;
    }



}




