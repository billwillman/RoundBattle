/*
 * 站位
 * 管理每个角色对象
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RoundBattle {

    // 坐位类型：左边，右边
    public enum SeatType {
        left, right
    }

    // 负责角色管理
    public class SeatManager: MonoBehaviour {

        private Fighter[] m_LeftSeats = new Fighter[_cSeatCount];
        private Fighter[] m_RightSeats = new Fighter[_cSeatCount];

        public SeatType MySelfSeatType {
            get;
            set;
        }

        private int GetClientPos(int serverId) {

        }

        public void AddFighter(SeatType seatType, Fighter fighter) {
            if (fighter == null)
                return;
            int serverId = fighter.ServerId;


            switch (seatType) {
                case SeatType.left:
                    break;
                case SeatType.right:
                    break;
            }
        }

        private static readonly int _cSeatCount = 10;
    }
}
