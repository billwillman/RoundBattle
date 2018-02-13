using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundBattle.Command {
    // 从地上爬出来
    public class Fighter_ClimbState: FighterBaseState {

        public override void Enter(Fighter target) {
            target.ChangeAction(FighterActionEnum.Climb);
        }

        protected override void OnEndKeyFrameEnd(Fighter fighter) {
            DoNextState(fighter);
        }

        private void DoNextState(Fighter fighter) {
            fighter.StateMgr.ChangeState (FighterStates.Fighter_Idle);
        }
    }
}
