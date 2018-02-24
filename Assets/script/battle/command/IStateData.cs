using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundBattle.Command {

    internal class Fighter_ActionData {
        private int m_LastKeyFrame = -1;
        private FighterActionEnum m_LastAction = FighterActionEnum.None;

        internal int LastKeyFrame {
            get {
                return m_LastKeyFrame;
            }
            set {
                m_LastKeyFrame = value;
            }
        }

        internal FighterActionEnum LastAction {
            get {
                return m_LastAction;
            }
            set {
                m_LastAction = value;
            }
        }
    }

    /*---------------------------------------战斗中才有的数据--------------------------------------------------*/


    // 物理攻击移动数据
    internal class Fighter_PhsyicalAttackMoveData {
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

        public float MoveSpeed {
            get;
            private set;
        }

        public Fighter_PhsyicalAttackMoveData(SeatInfo origion, SeatInfo target, float moveSpeed) {
            this.Origion = origion;
            this.Target = target;
            this.MoveSpeed = moveSpeed;
        }
    }

    internal class Fighter_StartFighterData
    {
        public float MoveSpeed {
            get;
            private set;
        }

        public Fighter_StartFighterData(float moveSpeed)
        {
            this.MoveSpeed = moveSpeed;
        }
    }

    /*---------------------------------------------------------------------------------------------*/

    // 数据层
    internal interface IFighterStateData {

        // 战斗中才有
        Fighter_PhsyicalAttackMoveData PhysicalAttackMoveData {
            get;
            set;
        }

        Fighter_StartFighterData StartFighterData {
            get;
            set;
        }

        //----

        Fighter_ActionData ActionData {
            get;
            set;
        }
    }

    // 默认数据类型
    internal class DefaultFighterStateData: IFighterStateData
    {
        public Fighter_PhsyicalAttackMoveData PhysicalAttackMoveData {
            get;
            set;
        }

        public Fighter_StartFighterData StartFighterData {
            get;
            set;
        }

        public Fighter_ActionData ActionData {
            get;
            set;
        }

        public DefaultFighterStateData()
        {
            ActionData = new Fighter_ActionData ();
        }
    }
}
