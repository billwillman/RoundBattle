using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RoundBattle {

    

    // 战斗角色
    class Fighter: MonoBehaviour {
        // 是否是玩家
        private bool m_IsPlayer = false;
        // 物件
        private Dictionary<int, SpriteA2D> m_PartMap = new Dictionary<int, SpriteA2D>();
        private string m_ActorResPath = string.Empty;
        private FighterActionEnum m_CurrentAction = FighterActionEnum.Idle;
        private string m_Name = string.Empty;
        private SpriteFrameType m_FrameType = SpriteFrameType.frame5Dir;
        private int m_Dir = 0;
        private AniInfo m_AniInfo;
        private int m_LoopDir = 0;
        private int m_LoopCount = -1;
        private float m_Speed = 1f;
        private int m_ServerId = -1;

        private void Awake() {
            m_AniInfo = SpriteA2D.GeneratorDefaultAniInfo();
        }

        public string Name {
            get {
                return m_Name;
            }
        }

        public SpriteFrameType FrameType {
            get {
                return m_FrameType;
            }
        }

        public FighterActionEnum CurrentAction {
            get {
                return m_CurrentAction;
            }
        }

        // 是否是玩家
        public bool IsPlayer {
            get {
                return m_IsPlayer;
            }
        }

        public SpriteA2D GetPartSpriteA2D(FigherPart part) {
            int key = (int)part;
            SpriteA2D ret;
            if (!m_PartMap.TryGetValue(key, out ret))
                ret = null;
            return ret;
        }

        public GameObject GetPartGameObject(FigherPart part) {
            SpriteA2D sprite = GetPartSpriteA2D(part);
            if (sprite == null)
                return null;
            return sprite.gameObject;
        }

        // 根结点
        protected virtual string GeneratorActorResPath() {
            string ret;
            if (m_IsPlayer)
                ret = string.Format("resources/actor/{0}", m_Name);
            else
                ret = string.Format("resources/actor/@{1}", m_Name);
            return ret;
        }

        // 动作路径
        protected virtual string GeneratorActionPath(FighterActionEnum action,
            bool isAlpha = false,
            FigherPart part = FigherPart.Body) {
            string ret = string.Empty;
            string actionName = FighterStringEnumHelper.GetActionName(action);
            if (string.IsNullOrEmpty(actionName))
                return ret;

            if (m_IsPlayer) {
                string partName = FighterStringEnumHelper.GetPartName(part);
                if (string.IsNullOrEmpty(partName))
                    return ret;
                if (!isAlpha)
                    ret = string.Format("{0}/{1}/@{2}/{2}", m_ActorResPath, partName, actionName);
                else
                    ret = string.Format("{0}/{1}/@{2}/_a/{2}_a", m_ActorResPath, partName, actionName);
            } else {
                if (part != FigherPart.Body)
                    return ret;
                if (!isAlpha)
                    ret = string.Format("{0}/_{1}/{1}", m_ActorResPath, actionName);
                else
                    ret = string.Format("{0}/_{1}/a/{1}_a", m_ActorResPath, actionName);
            }

            return ret;
        }

        // 暂时用同步，后续用异步
        protected virtual Sprite[] LoadPartSprites(FigherPart part, FighterActionEnum action) {
            string fileName = GeneratorActionPath(action, false, part);
            if (string.IsNullOrEmpty(fileName))
                return null;
            return Resources.LoadAll<Sprite>(fileName);
        }

        // 暂时用同步，后续用异步
        protected virtual Texture LoadPartAlphaTexture(FigherPart part, FighterActionEnum action) {
            string fileName = GeneratorActionPath(action, true, part);
            if (string.IsNullOrEmpty(fileName))
                return null;
            return Resources.Load<Texture>(fileName);
        }

        protected virtual Material LoadPartMaterial(FighterActionEnum action, FigherPart part) {
            return Resources.Load<Material>("resources/material/actormat");
        }

        private void Init(string name, bool isPlayer, FighterActionEnum action) {
            m_Name = name;
            m_IsPlayer = isPlayer;
            // 初始化路径
            m_ActorResPath = GeneratorActorResPath();
            // 优先创建身体
            CreatePart(action, FigherPart.Body);
        }

        private void CreatePart(FighterActionEnum action, FigherPart part) {

            SpriteA2D sprite = GetPartSpriteA2D(part);
            if (sprite == null) {
                GameObject gameObj = new GameObject(FighterStringEnumHelper.GetPartName(FigherPart.Body),
                    typeof(SpriteA2D));
                sprite = gameObj.GetComponent<SpriteA2D>();
                m_PartMap[(int)part] = sprite;
            }

            SpriteRenderer renderer = sprite.Renderer;
            if (renderer != null) {
                // 暂时先这样
                renderer.sharedMaterial = LoadPartMaterial(action, part);

                Texture alphaTex = LoadPartAlphaTexture(part, action);
                renderer.material.SetTexture("_UVTex", alphaTex);
            }

            // 后续考虑异步
            Sprite[] sps = LoadPartSprites(part, action);
            // 后面考虑读取配置
            sprite.Init(sps, m_FrameType, m_AniInfo, m_LoopDir, m_LoopCount, m_Dir);
        }

        // 创建NPC或者宠物
        public static Fighter CreatePetOrNpc(int serverId, string name, FighterActionEnum defaultAction = FighterActionEnum.Idle) {
            GameObject gameObj = new GameObject(name, typeof(Fighter));
            Fighter ret = gameObj.GetComponent<Fighter>();
            ret.m_ServerId = serverId;
            ret.Init(name, false, defaultAction);
            return ret;
        }

        // 服务器ID
        public int ServerId {
            get {
                return m_ServerId;
            }
        }

        private void Update() {
            var iter = m_PartMap.GetEnumerator();
            while (iter.MoveNext()) {
                SpriteA2D sprite = iter.Current.Value;
                if (sprite != null) {
                    sprite.OnSpriteAnimateUpdate(m_Speed);
                    sprite.AttachSpriteRenderer();
                }
            }
            iter.Dispose();
        }

    }
}
