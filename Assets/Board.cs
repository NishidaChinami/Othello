using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;
using Reversi;
using Unity.VisualScripting;

public class Board : MonoBehaviour,IPointerClickHandler
{
    //8×8のオセロ
    private int _Size = 8;

    private StoneState[,] _State;
    private StoneState _PlayerState = StoneState.White;

   // public OthelloCaluculatar _Caluculatar;



    [SerializeField]
    private Cell _cellPrefab = null;

    private Cell[,] _cells;

    [SerializeField]
    private Stone _stonePrefab = null;

    private Stone[,] _stoneindex;

   

    //白と黒の石の枚数のカウンター変数
    public int _whitecount = 0;
    public int _blackcount = 0;

    //石の状態を白なら黒へ黒なら白に返す
    public void GetChangeState()
    {
        _PlayerState = _PlayerState == StoneState.White ? StoneState.Black : StoneState.White;
    }


    //石が置けたかを判定する
    public bool HavePutStone(int row, int col)
    {
        // 置けなかった場合、falseを返却
        var turnStonesIndex = OthelloCaluculatar.GetChangeAllIndex(_State, _PlayerState, row, col);
        if (turnStonesIndex == null || turnStonesIndex.Count == 0) return false;
        return true;
    }

    
    // ストーンが置けるかどうか？
    public bool IsCanPutStone()
    {
        return OthelloCaluculatar.GetCanputIndex(_State, _PlayerState).Count > 0;
    }
    

    void GetCanPutPotiton()
    {
        for (var r = 0; r < _cells.GetLength(0); r++)
        {
            for (var c = 0; c < _cells.GetLength(1); c++)
            {
                _cells[r, c].ChangeColorG();
            }
        }
        foreach (var getcell in OthelloCaluculatar.GetCanputIndex(_State, _PlayerState))
        {
            _cells[getcell._row, getcell._col].ChangeColorY();
        }
    }

    //行番号と列番号の取得方法
    private bool TryGetIndex(Cell cell, out int row, out int column)
    {
        for (var r = 0; r < _cells.GetLength(0); r++)
        {
            for (var c = 0; c < _cells.GetLength(1); c++)
            {
                if (cell == _cells[r, c])
                {
                    row = r;
                    column = c;
                    return true;
                }
            }
        }
        row = column = -1;
        return false;
    }

    //置いて情報を石とリンクさせる
    public void StoneStateLink()
    {
        for (var r = 0; r < _State.GetLength(0); r++)
        {
            for (var c = 0; c < _State.GetLength(1); c++)
            {

                _stoneindex[r, c].StoneState = _State[r, c];
                Debug.Log(_State[r, c]);
            }
        }

    }

    //白と黒のカウント
    public void ColorCount(StoneState[,] stoneStates)
    {
        var whitecount = 0;
        var blackcount = 0;
        for (var row = 0; row < stoneStates.GetLength(0); row++)
        {
            for (var col = 0; col < stoneStates.GetLength(1); col++)
            {
                if (stoneStates[row, col] == StoneState.Empty) return;
                if (stoneStates[row, col] == StoneState.White) whitecount++;
                if (stoneStates[row, col] == StoneState.Black) blackcount++;
            }
        }
        //数えなおしたものを代入
        _whitecount = whitecount;
        _blackcount = blackcount;

    }
    //すべてのマスに置かれたか
    private bool IsFull(StoneState[,] stoneStates)
    {
        foreach (var state in stoneStates)
        {
            if (state == StoneState.Empty) { return false; }
        }
        return true;
    }
    //どちらもおけるないか？
    public bool NoPlace()
    {
        return OthelloCaluculatar.GetCanputIndex(_State, StoneState.White).Count == 0
            && OthelloCaluculatar.GetCanputIndex(_State, StoneState.Black).Count == 0;
    }
    //勝敗
    public void Result()
    {
        //もし、IsFUllが真だったら
        if (_whitecount == _blackcount) Debug.Log("引き分け");
        if (_whitecount > _blackcount) Debug.Log("白の勝ち");
        if (_whitecount < _blackcount) Debug.Log("黒の勝ち");

    }




    void Start()
    {
       //マス目を置く
        _cells = new Cell[_Size, _Size];
        _stoneindex = new Stone[_Size, _Size];
        _State = new StoneState[_Size, _Size];
     

        for (var r = 0; r < _Size; r++)
        {
            for (var c = 0; c < _Size; c++)
            {
                var cell = Instantiate(_cellPrefab);
                cell.name = $"Cell({r}, {c})";
                var stone = Instantiate(_stonePrefab);
                cell.transform.position = new Vector3(r*15-52, c*15-52, -1);
                stone.transform.position= new Vector3(r*15-52, c*15-52, -1);
                _cells[r, c] = cell;
                _cells[r, c].transform.SetParent(transform);
                _stoneindex[r, c] = stone;
                _stoneindex[r, c].transform.SetParent(transform);
                _State[r, c] = StoneState.Empty;
            }
        }
        var centerIndex1 = _Size / 2;
        var centerIndex2 = centerIndex1 - 1;
        _State[centerIndex1, centerIndex1] = StoneState.White;
        _State[centerIndex2, centerIndex1] = StoneState.Black;
        _State[centerIndex1, centerIndex2] = StoneState.Black;
        _State[centerIndex2, centerIndex2] = StoneState.White;
        StoneStateLink();
    }

    //クリックして行を取得
    public void OnPointerClick(PointerEventData eventData)
    {
        var target = eventData.pointerCurrentRaycast.gameObject;
        var getcell = target.transform.GetComponentInChildren<Cell>();
      
        Debug.Log(getcell);
        Debug.Log("今は"+_PlayerState+"のターン");
       
        if (TryGetIndex(getcell, out int row, out int col))
        {

            Debug.Log($"{row}{col}");
            if (HavePutStone(row, col))
            {
                OthelloCaluculatar.PutStone(_State, _PlayerState, row, col);
                GetChangeState();
            }
            StoneStateLink();
        }
            
    }

   

    public void Update()
    {
        if (IsCanPutStone() == false)
        {
            Debug.Log("パス");
            GetChangeState();
        }
        GetCanPutPotiton();
        ColorCount(_State);        
            if (IsFull(_State))
            {
                Result();
            }
            else if (NoPlace())
            {
                Result();
            }
    }

   

}
