using System;

namespace RoundBattle.Command
{
    public enum CommandType
    {
        // 开始战斗
        StartFight = 0,
        // 开始回合
        StartRound,
        // 使用技能
        UseSkill,
        // 等待指令输入
        WaitCmdInput,
        // 回合结束
        EndRound,
        // 战斗结束
        EndFight
    }
}

