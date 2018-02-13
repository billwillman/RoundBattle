/*
 * 站位
 * 管理每个角色对象
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RoundBattle.Record;
using RoundBattle.Command;

namespace RoundBattle {

    // 坐位类型：左边，右边
    public enum SeatType {
        none,
        left,
        right
    }

    public struct SeatInfo {
        public SeatType seat {
            get;
            set;
        }

        public int pos {
            get;
            set;
        }

        public bool IsVaild {
            get {
                return (seat != SeatType.none) && (pos >= 0) && (pos < SeatManager._cSeatCount);
            }
        }
    }

    // 负责角色管理
    public class SeatManager : MonoBehaviour {

        private Fighter[] m_LeftSeats = new Fighter[_cSeatCount];
        private Fighter[] m_RightSeats = new Fighter[_cSeatCount];

        private static Dictionary<int, Vector3> m_dictPosition = null;
        private static void InitDictPosition() {
            if (m_dictPosition != null)
                return;

            m_dictPosition = new Dictionary<int, Vector3>();

            Vector3 vec;

            // 我方第一排
            for (int i = 0; i < 5; ++i) {
                vec = new Vector3(3 + i, -5 + i, 0);
                vec.z = vec.y * 0.1f;
                m_dictPosition.Add(10 + i, vec);
            }

            // 我方第二排
            for (int i = 0; i < 5; ++i) {
                vec = new Vector3(2 + i, -4 + i, 0);
                vec.z = vec.y * 0.1f;
                m_dictPosition.Add(15 + i, vec);
            }

            // 敌方第一排
            for (int i = 0; i < 5; ++i) {
                vec = new Vector3(-8 + i, -2 + i, 0);
                vec.z = vec.y * 0.1f;
                m_dictPosition.Add(0 + i, vec);
            }

            for (int i = 0; i < 5; ++i) {
                vec = new Vector3(-7 + i, -3 + i, 0);
                vec.z = vec.y * 0.1f;
                m_dictPosition.Add(5 + i, vec);
            }
        }

        public SeatInfo MySelfServerInfo {
            get;
            private set;
        }

        public int GetClientDir(RecordFighter fighter) {
            if (fighter.serverId < 0)
                return -1;
            if (fighter.isMySelf)
                return 7;
            SeatType clientSeat;
            int idx = GetClientPos(fighter.serverId, out clientSeat);
            if (idx < 0)
                return -1;
            return clientSeat == SeatType.left ? 3 : 7;
        }

        public SeatInfo GetClientSeatInfo(int serverId) {
            SeatInfo ret = new SeatInfo();
            SeatType clientSeat;
            ret.pos = GetClientPos(serverId, out clientSeat);
            ret.seat = clientSeat;
            return ret;
        }

        public int GetClientPos(int serverId, out SeatType clientSeatType) {
            clientSeatType = SeatType.none;
            if (!MySelfServerInfo.IsVaild)
                return -1;

            SeatType serverSeat = serverId < 10 ? SeatType.left : SeatType.right;
            if (MySelfServerInfo.seat == serverSeat) {
                // 友军
                clientSeatType = SeatType.right;
            } else {
                // 敌军
                clientSeatType = SeatType.left;
            }

            if (serverId >= 10)
                return serverId - 10;
            return serverId;
        }

        public bool AddMySelf(Fighter fighter) {
            if (fighter == null)
                return false;

            int serverId = fighter.ServerId;
            SeatInfo info = new SeatInfo();
            info.seat = serverId < 10 ? SeatType.left : SeatType.right;
            info.pos = serverId < 10 ? serverId : serverId - 10;
            this.MySelfServerInfo = info;

            return AddFighter(fighter);
        }

        // 加入其他角色
        public bool AddOtherFighter(Fighter fighter) {
            return AddFighter(fighter);
        }

        // 获得站立位置
        public Vector3 GetSeatStandWorldPosition(SeatInfo clientInfo) {
            if (!clientInfo.IsVaild)
                return _cInvaildPos;
            InitDictPosition();
            if (m_dictPosition == null)
                return _cInvaildPos;
            if (!MySelfServerInfo.IsVaild)
                return _cInvaildPos;
            int srvId = clientInfo.seat == MySelfServerInfo.seat ? _cSeatCount + clientInfo.pos : clientInfo.pos;
            Vector3 ret;
            if (!m_dictPosition.TryGetValue(srvId, out ret))
                return _cInvaildPos;
            return ret;
        }

        private static readonly Vector3 _cInvaildPos = new Vector3(-9999, -9999, -9999);

        private bool AddFighter(Fighter fighter) {
            if (fighter == null)
                return false;
            int serverId = fighter.ServerId;
            SeatType clientSeatType;
            int clientSeat = GetClientPos(serverId, out clientSeatType);
            if (clientSeat < 0 || clientSeat >= _cSeatCount || clientSeatType == SeatType.none)
                return false;
            switch (clientSeatType) {
                case SeatType.left:
                    Fighter oldFigher = m_LeftSeats[clientSeat];
                    DestroyFighter(oldFigher);
                    m_LeftSeats[clientSeat] = fighter;
                    break;
                case SeatType.right:
                    Fighter oldFigher1 = m_RightSeats[clientSeat];
                    DestroyFighter(oldFigher1);
                    m_RightSeats[clientSeat] = fighter;
                    break;
            }
            return true;
        }

        private Fighter GetFighter(SeatInfo info) {
            switch (info.seat) {
                case SeatType.left:
                    if (info.pos >= 0 && info.pos < _cSeatCount)
                        return m_LeftSeats[info.pos];
                    break;
                case SeatType.right:
                    if (info.pos >= 0 && info.pos < _cSeatCount)
                        return m_RightSeats[info.pos];
                    break;
            }
            return null;
        }

        public void DestroyFighter(SeatInfo info) {
            Fighter fighter = GetFighter(info);
            DestroyFighter(fighter);
        }

        public void Clear() {
            for (int i = 0; i < m_LeftSeats.Length; ++i) {
                Fighter fighter = m_LeftSeats[i];
                DestroyFighter(fighter);
            }

            for (int i = 0; i < m_RightSeats.Length; ++i) {
                Fighter fighter = m_RightSeats[i];
                DestroyFighter(fighter);
            }
        }

        public bool ChangeState(SeatInfo clientSeatInfo, FighterStates state)
        {
            if (!clientSeatInfo.IsVaild)
                return false;
            Fighter fighter = GetFighter(clientSeatInfo);
            if (fighter == null)
                return false;
            return fighter.StateMgr.ChangeState (state);
        }

        public bool ChangeAction(SeatInfo clientSeatInfo, FighterActionEnum action, int dir, bool isReset = false) {
            if (!clientSeatInfo.IsVaild)
                return false;
            Fighter fighter = GetFighter(clientSeatInfo);
            if (fighter == null)
                return false;
            fighter.ChangeAction(action, dir, isReset);
            return true;
        }

        public bool ChangeAction(SeatInfo clientSeatInfo, FighterActionEnum action, bool isReset = false) {
            if (!clientSeatInfo.IsVaild)
                return false;
            Fighter fighter = GetFighter(clientSeatInfo);
            if (fighter == null)
                return false;
            fighter.ChangeAction(action, isReset);
            return true;
        }

        private void DestroyFighter(Fighter fighter) {
            if (fighter == null)
                return;
            int serverId = fighter.ServerId;
            SeatType clientSeatType;
            int clientSeat = GetClientPos(serverId, out clientSeatType);
            
            switch (clientSeatType) {
                case SeatType.left:
                    Fighter old1 = m_LeftSeats[clientSeat];
                    if (old1 != null && old1 != fighter) {
                        Debug.LogError("SeatManager.DestroyFighter: delete seat is error!");
                    }
                    m_LeftSeats[clientSeat] = null;
                    break;
                case SeatType.right:
                    Fighter old2 = m_RightSeats[clientSeat];
                    if (old2 != null && old2 != fighter) {
                        Debug.LogError("SeatManager.DestroyFighter: delete seat is error!");
                    }
                    m_RightSeats[clientSeat] = null;
                    break;
            }
            fighter.Destroy();
        }

        internal static readonly int _cSeatCount = 10;
    }
}
