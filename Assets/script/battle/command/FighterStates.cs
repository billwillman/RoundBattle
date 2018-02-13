using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundBattle.Command {
    public enum FighterStates {
        // 准备状态
        Fighter_Ready = 0,
        Fighter_Idle,
        // 从地上爬出来
        Fighter_Climb,
        // 物理攻击移动
        Fighter_PhysicalAttackMove,
        // 攻击状态
        Fighter_PhysicalAttack,
        // 被后仰状态
        Fighter_DamageStruct,
        // 击飞状态
        Fighter_DamageFly,
        // 物理攻击等待状态
        Fighter_PhysicalAttackWait,
    }
}
