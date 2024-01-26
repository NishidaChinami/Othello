using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


namespace Reversi
{
    public enum StoneState
    {
        Empty,
        White,
        Black
    }
    public static class OthelloCaluculatar
    {
        /// <summary>
        /// ストーン状態を黒←→白で切り替えて返却
        /// </summary>
        public static StoneState GetReverseStoneState(StoneState stoneState)
        {
            return stoneState == StoneState.Black ? StoneState.White : StoneState.Black;
        }

        //探索８方向分のベクトル
        private static readonly Vector2Int[] _SearchPatern = new Vector2Int[]
        {
        new Vector2Int(1,0),
        new Vector2Int(1,1),
        new Vector2Int(1,-1),
        new Vector2Int(-1,0),
        new Vector2Int(-1,-1),
        new Vector2Int(-1,1),
        new Vector2Int(0,1),
        new Vector2Int(0,-1)
        };

        /// <summary>
        /// 石を置く処理
        /// </summary>
        /// <param name="stoneStates">配列の石情報</param>
        /// <param name="nowTurnState">今が何色のターンなのか</param>
        /// <param name="row">置く場所の列</param>
        /// <param name="col">置く場所の行</param>
        /// <param name="changeStonesIndex">その場所に置いたときひっくりかえせる石の配列情報</param>
        public static void PutStone(StoneState[,] stoneStates, StoneState nowTurnState, int row, int col)
        {
            List<StoneIndex> changeStonesIndex = null;
            //なにか入ってたら置けない
            if (stoneStates[row, col] != StoneState.Empty) return;
            //ひっくりかえせる石がなければ置けない
            changeStonesIndex ??= GetChangeAllIndex(stoneStates, nowTurnState, row, col);
            if (changeStonesIndex.Count == 0) return;

            //２次元配列で扱えるようにする
            stoneStates[row, col] = nowTurnState;

            //石を変換
            foreach (var changeStone in changeStonesIndex)
            {
                stoneStates[changeStone._row, changeStone._col] = nowTurnState;
            }
            GetReverseStoneState(nowTurnState);
        }

        /// <summary>
        /// 置くことが可能な場所を返す
        /// </summary>
        /// <param name="stoneStates"></param>　石のステートの全配列情報
        /// <param name="putState"></param>置いた石の状態
        /// <returns></returns>
        public static List<StoneIndex> GetCanputIndex(StoneState[,] stoneStates, StoneState putState)
        {
            var canputIndex = new List<StoneIndex>();
            for (var row = 0; row < stoneStates.GetLength(0); row++)
            {
                for (var col = 0; col < stoneStates.GetLength(1); col++)
                {
                    if (GetChangeAllIndex(stoneStates, putState, row, col).Count > 0)
                    {
                        canputIndex.Add(new StoneIndex(row, col));
                    }
                }
            }
            return canputIndex;
        }


        /// <summary>
        /// ここで8方向すべてにおいて色を変えられる石の配列情報を取得
        /// </summary>
        /// <param name="stoneStates">探索するマスの石のステート</param>
        /// <param name="putState">置いた石のステート</param>
        /// <param name="putrow">何列目か</param>
        /// <param name="putcol">何行目か</param>
        /// <returns></returns>
        public static List<StoneIndex> GetChangeAllIndex(StoneState[,] stoneStates, StoneState putState, int putrow, int putcol)
        {
            var changeIndex = new List<StoneIndex>();
            //石が置かれていたら
            if (stoneStates[putrow, putcol] != StoneState.Empty) return changeIndex;

            foreach (var search in _SearchPatern)
            {
                changeIndex.AddRange(GetChangeOneIndex(stoneStates, putState, putrow, putcol, search));
            }

            return changeIndex;
        }
        //探索１方向分のひっくりかえせる石を返す
        private static List<StoneIndex> GetChangeOneIndex(StoneState[,] stoneStates, StoneState putState, int putrow, int putcol, Vector2Int search)
        {
            //どっちの色を変えるか判定する
            var targetState = putState == StoneState.White ? StoneState.Black : StoneState.White;
            var changeIndex = new List<StoneIndex>();
            var row = putrow;
            var col = putcol;
            while (true)
            {
                row += search.x;
                col += search.y;


                // 外に出たら探索終了　Getlengthの中身どうする？
                if (row < 0 || stoneStates.GetLength(0) <= row ||
                    col < 0 || stoneStates.GetLength(1) <= col)
                {
                    break;
                }
                // Emptyであれば探索終了
                var stoneState = stoneStates[row, col];
                if (stoneStates[row, col] == StoneState.Empty)
                {
                    break;
                }
                //同じ色なら
                if (stoneState == putState)
                {
                    return changeIndex;
                }
                //違う色なら
                if (stoneState == targetState)
                {
                    changeIndex.Add(new StoneIndex(row, col));
                }
            }
            // 見つからなかったら空で返却
            return new List<StoneIndex>();
        }



    }
}
