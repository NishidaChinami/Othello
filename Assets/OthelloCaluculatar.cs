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
        /// �X�g�[����Ԃ����������Ő؂�ւ��ĕԋp
        /// </summary>
        public static StoneState GetReverseStoneState(StoneState stoneState)
        {
            return stoneState == StoneState.Black ? StoneState.White : StoneState.Black;
        }

        //�T���W�������̃x�N�g��
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
        /// �΂�u������
        /// </summary>
        /// <param name="stoneStates">�z��̐Ώ��</param>
        /// <param name="nowTurnState">�������F�̃^�[���Ȃ̂�</param>
        /// <param name="row">�u���ꏊ�̗�</param>
        /// <param name="col">�u���ꏊ�̍s</param>
        /// <param name="changeStonesIndex">���̏ꏊ�ɒu�����Ƃ��Ђ����肩������΂̔z����</param>
        public static void PutStone(StoneState[,] stoneStates, StoneState nowTurnState, int row, int col)
        {
            List<StoneIndex> changeStonesIndex = null;
            //�Ȃɂ������Ă���u���Ȃ�
            if (stoneStates[row, col] != StoneState.Empty) return;
            //�Ђ����肩������΂��Ȃ���Βu���Ȃ�
            changeStonesIndex ??= GetChangeAllIndex(stoneStates, nowTurnState, row, col);
            if (changeStonesIndex.Count == 0) return;

            //�Q�����z��ň�����悤�ɂ���
            stoneStates[row, col] = nowTurnState;

            //�΂�ϊ�
            foreach (var changeStone in changeStonesIndex)
            {
                stoneStates[changeStone._row, changeStone._col] = nowTurnState;
            }
            GetReverseStoneState(nowTurnState);
        }

        /// <summary>
        /// �u�����Ƃ��\�ȏꏊ��Ԃ�
        /// </summary>
        /// <param name="stoneStates"></param>�@�΂̃X�e�[�g�̑S�z����
        /// <param name="putState"></param>�u�����΂̏��
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
        /// ������8�������ׂĂɂ����ĐF��ς�����΂̔z������擾
        /// </summary>
        /// <param name="stoneStates">�T������}�X�̐΂̃X�e�[�g</param>
        /// <param name="putState">�u�����΂̃X�e�[�g</param>
        /// <param name="putrow">����ڂ�</param>
        /// <param name="putcol">���s�ڂ�</param>
        /// <returns></returns>
        public static List<StoneIndex> GetChangeAllIndex(StoneState[,] stoneStates, StoneState putState, int putrow, int putcol)
        {
            var changeIndex = new List<StoneIndex>();
            //�΂��u����Ă�����
            if (stoneStates[putrow, putcol] != StoneState.Empty) return changeIndex;

            foreach (var search in _SearchPatern)
            {
                changeIndex.AddRange(GetChangeOneIndex(stoneStates, putState, putrow, putcol, search));
            }

            return changeIndex;
        }
        //�T���P�������̂Ђ����肩������΂�Ԃ�
        private static List<StoneIndex> GetChangeOneIndex(StoneState[,] stoneStates, StoneState putState, int putrow, int putcol, Vector2Int search)
        {
            //�ǂ����̐F��ς��邩���肷��
            var targetState = putState == StoneState.White ? StoneState.Black : StoneState.White;
            var changeIndex = new List<StoneIndex>();
            var row = putrow;
            var col = putcol;
            while (true)
            {
                row += search.x;
                col += search.y;


                // �O�ɏo����T���I���@Getlength�̒��g�ǂ�����H
                if (row < 0 || stoneStates.GetLength(0) <= row ||
                    col < 0 || stoneStates.GetLength(1) <= col)
                {
                    break;
                }
                // Empty�ł���ΒT���I��
                var stoneState = stoneStates[row, col];
                if (stoneStates[row, col] == StoneState.Empty)
                {
                    break;
                }
                //�����F�Ȃ�
                if (stoneState == putState)
                {
                    return changeIndex;
                }
                //�Ⴄ�F�Ȃ�
                if (stoneState == targetState)
                {
                    changeIndex.Add(new StoneIndex(row, col));
                }
            }
            // ������Ȃ��������ŕԋp
            return new List<StoneIndex>();
        }



    }
}
