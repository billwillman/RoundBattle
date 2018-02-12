using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

namespace RoundBattle.Command {
    // 命令管理器
    public class CommandManager {

        private LinkedList<Command> m_CmdList = new LinkedList<Command>();

        // 清理
        public void Clear() {
            m_CmdList.Clear();
        }
    }

    public class FighterStateMgr: StateMgr<FighterStates, Fighter> {

        public FighterStateMgr(Fighter target): base(target) {
            RegisterFighterStates();
        }

        private static void RegisterFighterStates() {
            Register(FighterStates.Fighter_PhysicalAttackMove, new Fighter_PhysicalAttackMoveState());
        }
    }
}
