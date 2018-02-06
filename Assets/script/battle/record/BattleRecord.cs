using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundBattle.Record {

    public struct RecordFighter {
        // 模型名
        public string model {
            get;
            set;
        }

        // 其他部件，除了body
        public List<string> otherPart {
            get;
            set;
        }

        public int serverId {
            get;
            set;
        }

        public bool isMySelf {
            get;
            set;
        }

        public bool isPlayer {
            get;
            set;
        }
    }

    // 战斗数据
    public struct BattleRecord {
        // 初始出场角色(第一个必须是自己)
        public List<RecordFighter> fighters {
            get;
            set;
        }

        public bool IsVaild {
            get {
                if (fighters == null || fighters.Count <= 0)
                    return false;
                return true;
            }
        }
    }
}
