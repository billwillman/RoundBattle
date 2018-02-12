/*
 * 战报系统
 */

using UnityEngine;
using System.Collections;

namespace RoundBattle.Record {
    // 战报系统
    public class BattleRecordSystem: MonoBehaviour {
        private BattleSystem m_Sys = null;

        protected BattleSystem Sys {
            get {
                if (m_Sys == null)
                    m_Sys = GetComponent<BattleSystem>();
                return m_Sys;
            }
        }

        public bool LoadBattleRecord(string fileName) {
            return false;
        }

        // 暂时这样
        public bool LoadBattleRecord(BattleRecord record) {
            if (!record.IsVaild)
                return false;
            SeatManager seatMgr = Sys.SeatMgr;
            for (int i = 0; i < record.fighters.Count; ++i) {
                var info = record.fighters[i];
                int dir = seatMgr.GetClientDir(info);

                Fighter fighter = Fighter.CreateFighter(info.serverId, info.model, info.isPlayer, dir);
                fighter.transform.SetParent(BattleSystem.FightersRoot, false);
                if (info.isMySelf) {
                    if (!Sys.SeatMgr.AddMySelf(fighter))
                        return false;
                } else {
                    if (!Sys.SeatMgr.AddOtherFighter(fighter))
                        return false;
                }

                SeatInfo seatInfo = seatMgr.GetClientSeatInfo(info.serverId);
                // 这部分是角色的位置
                fighter.transform.localPosition = seatMgr.GetSeatStandWorldPosition(seatInfo);
                // 创建部件
                if (info.isPlayer) {
                    // 玩家模式才会创建部件
                    if (info.otherPart != null && info.otherPart.Count > 0) {
                        for (int j = 0; j < info.otherPart.Count; ++j) {
                            var otherPart = info.otherPart[j];
                            fighter.AddOtherPart(otherPart);
                        }
                    }
                }
            }
            return true;
        }
    }
}
