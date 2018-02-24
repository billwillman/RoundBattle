using UnityEngine;
using System.Collections;
using RoundBattle.Record;
using RoundBattle.Command;

namespace RoundBattle {
    // 战斗系统入口
    [RequireComponent(typeof(SeatManager))]
    public class BattleSystem : MonoBehaviour {
        // 战报系统
        private BattleRecordSystem m_RecordSystem = null;
        private SeatManager m_SeatMgr = null;
        private CommandManager m_CommandMgr = new CommandManager();
        private static BattleSystem m_Instance = null;

        public CommandManager CmdMgr
        {
            get {
                return m_CommandMgr;
            }
        }

        public static Transform FightersRoot {
            get;
            private set;
        }

        private void Awake() {
            var parentTrans = this.transform;
            var gameObj = new GameObject("Fighters");
            FightersRoot = gameObj.transform;
            FightersRoot.SetParent(parentTrans, false);

            // 地图特效节点
            gameObj = new GameObject("MapEffect", typeof(MapEffectManager));
            gameObj.transform.SetParent(parentTrans, false);
        }

        public static BattleSystem GetInstance() {
            if (m_Instance == null) {
                GameObject obj = new GameObject("BattleSystem", typeof(BattleSystem));
                m_Instance = obj.GetComponent<BattleSystem>();
            }
            return m_Instance;
        }

        public static BattleSystem Instance {
            get {
                return GetInstance();
            }
        }

        // 读取战报, 从头播放到尾部
        public bool LoadBattleRecord(string fileName) {
            // 保证干净清理
            Clear();

            if (m_RecordSystem == null)
                m_RecordSystem = gameObject.AddComponent<BattleRecordSystem>();
           
            return m_RecordSystem.LoadBattleRecord(fileName);
        }

        public bool LoadBattleRecord(BattleRecord record) {
            Clear();
            if (m_RecordSystem == null)
                m_RecordSystem = gameObject.AddComponent<BattleRecordSystem>();
            return m_RecordSystem.LoadBattleRecord(record);
        }

        internal SeatManager SeatMgr {
            get {
                if (m_SeatMgr == null)
                    m_SeatMgr = GetComponent<SeatManager>();
                return m_SeatMgr;
            }
        }

        // 全部清理入口
        public void Clear() {
            if (m_SeatMgr != null)
                m_SeatMgr.Clear();
            if (m_CommandMgr != null)
                m_CommandMgr.Clear();
        }

        private void OnDestroy() {
            FightersRoot = null;
            m_Instance = null;
        }
    }

}
