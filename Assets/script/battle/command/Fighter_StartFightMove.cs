using System;
using UnityEngine;

namespace RoundBattle.Command
{
    public class Fighter_StartFightMove: FighterBaseState
    {
        private bool IsVaild(Fighter target)
        {
            return target != null && target.FighterStateData != null && 
                target.FighterStateData.StartFighterData != null;
        }

        public override bool CanEnter(Fighter target)
        {
            return IsVaild (target);
        }

        public override void Enter(Fighter target)
        {
            target.ChangeAction (FighterActionEnum.Run);
            //  重置位置
            var seatMgr = BattleSystem.GetInstance().SeatMgr;
            Vector3 pos = seatMgr.GetSeatStartFightWorldPosition (target.ServerId);
            target.transform.position = pos;
        }

        public override void Update(Fighter target)
        {
            var seatMgr = BattleSystem.GetInstance().SeatMgr;
            Vector3 targetPos = seatMgr.GetSeatStartFightWorldPosition (target.ServerId);
            var trans = target.transform;
            Vector3 currentPos = trans.position;
            Vector3 dir = (targetPos - currentPos).normalized * 
                target.FighterStateData.StartFighterData.MoveSpeed * target.TickDetla;
            currentPos += dir;
            bool isPosEnd = Fighter.IsPosEnd(dir, currentPos, currentPos);
            if (isPosEnd) {
                trans.position = targetPos;
                // 更换状态
                DoNextState(target);
            } else
                trans.position = currentPos;

        }

        private void DoNextState(Fighter target)
        {
            target.StateMgr.ChangeState (FighterStates.Fighter_Ready);
        }
    }
        
}

