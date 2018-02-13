using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using RoundBattle.Record;

namespace RoundBattle {
    // 角色动画枚举类型对应字符串

    public enum FighterActionEnum {
        None = 0,
        // 攻击
        Attack,
        // 从地上爬出来
        Climb,
        // 受伤
        Damage,
        // 死亡
        Death,
        // 防御
        Defence,
        // 待机
        Idle,
        // 跑步
        Run,
        // 释放魔法
        SkillMagic,

        Max
    }

    // 部件枚举
    public enum FigherPart {
        // 身体
        Body = 0,
        // 武器
        Weapon,
        // 部件
        Avatar,
    }

      struct ActionNameDirKey: IEquatable<ActionNameDirKey> {

        public RecordOtherPartType weapon {
            get;
            set;
        }

        public FighterActionEnum action {
            get;
            set;
        }

        public string modelName {
            get;
            set;
        }

        public static bool operator !=(ActionNameDirKey a1, ActionNameDirKey a2) {
            return (!(a1 == a2));
        }
        public static bool operator ==(ActionNameDirKey a1, ActionNameDirKey a2) {
            return a1.weapon == a2.weapon && a1.action == a2.action && string.Compare(a1.modelName, a2.modelName) == 0;
        }
        public override int GetHashCode() {
            int ret = FilePathMgr.InitHashValue();
            FilePathMgr.HashCode(ref ret, weapon);
            FilePathMgr.HashCode(ref ret, (int)action);
            FilePathMgr.HashCode(ref ret, modelName);
            return ret;
        }

        public bool Equals(ActionNameDirKey other) {
            return this == other;
        }

        public override bool Equals(object obj) {
            if (obj == null)
                return false;

            if (GetType() != obj.GetType())
                return false;

            if (obj is ActionNameDirKey) {
                ActionNameDirKey other = (ActionNameDirKey)obj;
                return Equals(other);
            } else
                return false;

        }
    }

    class ActionNameDirKeyComparser : StructComparser<ActionNameDirKey> { }

    public static class FighterStringEnumHelper {
        // 不用枚举声明减少GC
        private static Dictionary<int, string> m_ActionNames = null;
        private static Dictionary<int, string> m_PartNames = null;
        private static Dictionary<int, string> m_RecordOtherPartNames = null;
        // 不需要包含默认的转换
        private static Dictionary<ActionNameDirKey, string> m_RoleBodyActionNames = new Dictionary<ActionNameDirKey, string>(ActionNameDirKeyComparser.Default);
        private static void InitRecordOtherPartNames() {
            if (m_RecordOtherPartNames != null)
                return;
            m_RecordOtherPartNames = new Dictionary<int, string>();
            m_RecordOtherPartNames.Add((int)RecordOtherPartType.avatar_decoration, "decoration");
            m_RecordOtherPartNames.Add((int)RecordOtherPartType.weapon_1, "1");
            m_RecordOtherPartNames.Add((int)RecordOtherPartType.weapon_2, "2");
            m_RecordOtherPartNames.Add((int)RecordOtherPartType.weapon_3, "3");
            m_RecordOtherPartNames.Add((int)RecordOtherPartType.weapon_4, "4");
            m_RecordOtherPartNames.Add((int)RecordOtherPartType.weapon_5, "5");
            m_RecordOtherPartNames.Add((int)RecordOtherPartType.weapon_6, "6");
        }

        // 暂时先这样，后续更改为按需读取配置表
        private static void InitRoleBoydActionNames() {
            if (m_RoleBodyActionNames.Count > 0)
                return;
            ActionNameDirKey key = new ActionNameDirKey();
            key.modelName = "role_lingnv4";
            key.action = FighterActionEnum.Attack;
            key.weapon = RecordOtherPartType.weapon_1;
            m_RoleBodyActionNames.Add(key, "attack_1");

            key = new ActionNameDirKey();
            key.modelName = "role_lingnv4";
            key.action = FighterActionEnum.Attack;
            key.weapon = RecordOtherPartType.weapon_2;
            m_RoleBodyActionNames.Add(key, "attack_2");

            key = new ActionNameDirKey();
            key.modelName = "role_lingnv4";
            key.action = FighterActionEnum.Attack;
            key.weapon = RecordOtherPartType.weapon_3;
            m_RoleBodyActionNames.Add(key, "attack_3");

            key = new ActionNameDirKey();
            key.modelName = "role_lingnv4";
            key.action = FighterActionEnum.Attack;
            key.weapon = RecordOtherPartType.weapon_4;
            m_RoleBodyActionNames.Add(key, "attack_4");

            key = new ActionNameDirKey();
            key.modelName = "role_lingnv4";
            key.action = FighterActionEnum.Attack;
            key.weapon = RecordOtherPartType.weapon_5;
            m_RoleBodyActionNames.Add(key, "attack_5");

            key = new ActionNameDirKey();
            key.modelName = "role_lingnv4";
            key.action = FighterActionEnum.Attack;
            key.weapon = RecordOtherPartType.weapon_6;
            m_RoleBodyActionNames.Add(key, "attack_6");

            // 跑步
            key = new ActionNameDirKey();
            key.modelName = "role_lingnv4";
            key.action = FighterActionEnum.Run;
            key.weapon = RecordOtherPartType.weapon_1;
            m_RoleBodyActionNames.Add(key, "run_1_2_3_4_5");

            key = new ActionNameDirKey();
            key.modelName = "role_lingnv4";
            key.action = FighterActionEnum.Run;
            key.weapon = RecordOtherPartType.weapon_2;
            m_RoleBodyActionNames.Add(key, "run_1_2_3_4_5");

            key = new ActionNameDirKey();
            key.modelName = "role_lingnv4";
            key.action = FighterActionEnum.Run;
            key.weapon = RecordOtherPartType.weapon_3;
            m_RoleBodyActionNames.Add(key, "run_1_2_3_4_5");

            key = new ActionNameDirKey();
            key.modelName = "role_lingnv4";
            key.action = FighterActionEnum.Run;
            key.weapon = RecordOtherPartType.weapon_4;
            m_RoleBodyActionNames.Add(key, "run_1_2_3_4_5");

            key = new ActionNameDirKey();
            key.modelName = "role_lingnv4";
            key.action = FighterActionEnum.Run;
            key.weapon = RecordOtherPartType.weapon_5;
            m_RoleBodyActionNames.Add(key, "run_1_2_3_4_5");

            key = new ActionNameDirKey();
            key.modelName = "role_lingnv4";
            key.action = FighterActionEnum.Run;
            key.weapon = RecordOtherPartType.weapon_6;
            m_RoleBodyActionNames.Add(key, "run_6");
        }

        public static string GetRoleBodyActionNames(string modelName, FighterActionEnum action, RecordOtherPartType weapon) {

            // 暂时先这样，后续按需读取
            InitRoleBoydActionNames();

            if (string.IsNullOrEmpty(modelName))
                return GetActionName(action);
            string ret;
            ActionNameDirKey key = new ActionNameDirKey();
            key.modelName = modelName;
            key.action = action;
            key.weapon = weapon;
            if (m_RoleBodyActionNames.TryGetValue(key, out ret))
                return ret;
            return GetActionName(action);
        }
        
        private static void InitActorNames() {
            if (m_ActionNames != null)
                return;
            m_ActionNames = new Dictionary<int, string>();
            m_ActionNames.Add((int)FighterActionEnum.Attack, "attack");
            m_ActionNames.Add((int)FighterActionEnum.Climb, "climb");
            m_ActionNames.Add((int)FighterActionEnum.Damage, "damage");
            m_ActionNames.Add((int)FighterActionEnum.Death, "death");
            m_ActionNames.Add((int)FighterActionEnum.Defence, "defence");
            m_ActionNames.Add((int)FighterActionEnum.Idle, "idle");
            m_ActionNames.Add((int)FighterActionEnum.Run, "run");
            m_ActionNames.Add((int)FighterActionEnum.SkillMagic, "skill_magic");
        }

        private static void InitPartNames() {
            if (m_PartNames != null)
                return;
            m_PartNames = new Dictionary<int, string>();
            m_PartNames.Add((int)FigherPart.Body, "body");
            m_PartNames.Add((int)FigherPart.Weapon, "weapon");
            m_PartNames.Add((int)FigherPart.Avatar, "avatar");
        }

        public static string GetPartName(FigherPart part) {
            InitPartNames();
            if (m_PartNames == null)
                return string.Empty;
            string ret;
            if (!m_PartNames.TryGetValue((int)part, out ret))
                ret = string.Empty;
            return ret;
        }

        public static string GetOtherPartName(RecordOtherPartType part) {
            InitRecordOtherPartNames();
            if (m_RecordOtherPartNames == null)
                return string.Empty;
            string ret;
            if (!m_RecordOtherPartNames.TryGetValue((int)part, out ret))
                return string.Empty;
            return ret;
        }

        // 常规动作是否循环
        public static bool IsLoopNormalAction(FighterActionEnum action) {
            return action == FighterActionEnum.Idle || action == FighterActionEnum.Run;
        }

        public static string GetActionName(FighterActionEnum action) {
            InitActorNames();
            if (m_ActionNames == null)
                return string.Empty;
            string ret;
            if (!m_ActionNames.TryGetValue((int)action, out ret))
                return string.Empty;
            return ret;
        }
    }
}
