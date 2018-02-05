using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoundBattle {
    // 角色动画枚举类型对应字符串

    public enum FighterActionEnum {
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
        SkillMagic
    }

    // 部件枚举
    public enum FigherPart {
        // 身体
        Body = 0,
        // 武器
        Weapon,
        // 头发
        Decoration,
    }

    public static class FighterStringEnumHelper {
        // 不用枚举声明减少GC
        private static Dictionary<int, string> m_ActionNames = null;
        private static Dictionary<int, string> m_PartNames = null;
        
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
            m_PartNames.Add((int)FigherPart.Decoration, "decoration");
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
