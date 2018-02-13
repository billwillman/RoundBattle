using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoundBattle;
using UnityEngine;

namespace RoundBattle.Command {
    public class Fighter_PhysicalAttackMoveState : FighterBaseState {

        private bool IsVaild(Fighter target) {
            return target != null && target.FighterStateData != null && 
                target.FighterStateData.PhysicalAttackMoveData != null;
        }

        public override bool CanEnter(Fighter target) {
            return IsVaild(target);
        }
        public override void Enter(Fighter target) {
            if (!IsVaild(target))
                return;
            // 切换动作
            var data = target.FighterStateData.PhysicalAttackMoveData;
            SeatInfo targetSeat = data.Target;
            var seatMgr = BattleSystem.GetInstance().SeatMgr;
            Vector3 targetVec = seatMgr.GetSeatStandWorldPosition(targetSeat);
            int targetDir = target.GetDestWorldPosDirect(targetVec);
            target.ChangeAction(FighterActionEnum.Run, targetDir);
        }

        public override void Update(Fighter target) {
            if (!IsVaild(target))
                return;
            var data = target.FighterStateData.PhysicalAttackMoveData;
            // 移动速度
            float moveSpeed = data.MoveSpeed;
            var trans = target.transform;
            Vector3 vec = trans.position;
            SeatInfo targetSeat = data.Target;
            var seatMgr = BattleSystem.GetInstance().SeatMgr;
            Vector3 targetVec = seatMgr.GetSeatStandWorldPosition(targetSeat);
            Vector3 destOrgDir = (targetVec - vec).normalized;
            Vector3 dir = destOrgDir * target.TickDetla * moveSpeed;
            vec += dir;
            bool isPosEnd = Fighter.IsPosEnd(destOrgDir, targetVec, vec);
            if (isPosEnd) {
                trans.position = targetVec;
                // 更换状态
                DoNextState(target);
            } else
                trans.position = vec;
        }

        private void DoNextState(Fighter target) {
            // 物理攻击等待状态
            target.StateMgr.ChangeState(FighterStates.Fighter_PhysicalAttackWait);
        }
    }
}
