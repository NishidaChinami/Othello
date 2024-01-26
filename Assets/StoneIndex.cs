using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneIndex 
{
    //オセロの配列を管理
    public int _row {  get; private set; }
    public int _col { get; private set;}
    public StoneIndex(int row, int col)
    {
        _row = row;
        _col = col;
    }
}
