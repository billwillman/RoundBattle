using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

namespace RoundBattle.Command {
    // 命令管理器
    public class CommandManager {
        // 等待命令队列
        private LinkedList<Command> m_WaitCmdList = new LinkedList<Command>();
        // 运行队列
        private LinkedList<Command> m_RunCmdList = new LinkedList<Command>();

        // 清理
        public void Clear() {
            m_WaitCmdList.Clear();
            m_RunCmdList.Clear ();
        }
    }

    public class FighterStateMgr: StateMgr<FighterStates, Fighter> {

        public FighterStateMgr(Fighter target): base(target) {
            RegisterFighterStates();
        }

        private static void RegisterFighterStates() {
            Register(FighterStates.Fighter_PhysicalAttackMove, new Fighter_PhysicalAttackMoveState());
            Register(FighterStates.Fighter_Climb, new Fighter_ClimbState());
            Register (FighterStates.Fighter_Idle, new Fighter_IdleState ());
            Register (FighterStates.Fighter_StartFight, new Fighter_StartFightMove ());
            Register (FighterStates.Fighter_Ready, new Fighter_ReadyState ());
        }
    }
}
