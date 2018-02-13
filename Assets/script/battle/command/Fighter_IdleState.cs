using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundBattle.Command {
    public class Fighter_IdleState: FighterBaseState {
        public override void Enter(Fighter target)
        {
            target.ChangeAction (FighterActionEnum.Idle);
        }
    }
}
