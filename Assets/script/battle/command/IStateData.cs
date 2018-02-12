using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundBattle.Command {

    // 物理移动数据
    internal struct Fighter_PhsyicalAttackMoveData {
        // 原始位置
        public SeatInfo Origion {
            get;
            private set;
        }
        // 目标位置
        public SeatInfo Target {
            get;
            private set;
        }

        public void SetInfo(SeatInfo origion, SeatInfo target) {
            this.Origion = origion;
            this.Target = target;
        }
    }

    // 数据层
    internal interface IFighterStateData {
        Fighter_PhsyicalAttackMoveData PhysicalAttackMoveData {
            get;
            set;
        }
    }
}
