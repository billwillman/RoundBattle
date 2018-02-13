/*
 * todo: 加载改为异步
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RoundBattle.Record;
using RoundBattle.Command;

namespace RoundBattle {

    

    // 战斗角色
    public class Fighter: MonoBehaviour {
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
        // 非战斗中无此数据
        private IFighterStateData m_FighterStateData;

        private void Awake() {
            m_AniInfo = SpriteA2D.GeneratorDefaultAniInfo();
        }

        public string Name {
            get {
                return m_Name;
            }
        }

        public int CurrentFrameIndex {
            get {
                SpriteA2D body = GetPartSpriteA2D(FigherPart.Body);
                if (body == null)
                    return -1;
                return body.CurrentFrameIndex;
            }
        }

        internal IFighterStateData FighterStateData {
            get {
                return m_FighterStateData;
            }
            set {
                m_FighterStateData = value;
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
                ret = string.Format("actor/{0}", m_Name);
            else
                ret = string.Format("actor/@{0}", m_Name);
            return ret;
        }

        // 动作路径
        protected virtual string GeneratorActionPath(FighterActionEnum action,
            bool isAlpha = false,
            FigherPart part = FigherPart.Body, string name = "") {
            string ret = string.Empty;
            string actionName = FighterStringEnumHelper.GetActionName(action);
            if (string.IsNullOrEmpty(actionName))
                return ret;

            if (m_IsPlayer) {
                string partName = FighterStringEnumHelper.GetPartName(part);
                if (string.IsNullOrEmpty(partName))
                    return ret;
                if (!isAlpha) {
                    if (string.IsNullOrEmpty(name))
                        ret = string.Format("{0}/{1}/@{2}/{2}", m_ActorResPath, partName, actionName);
                    else
                        ret = string.Format("{0}/{1}/@{2}/_{3}/{3}", m_ActorResPath, partName, name, actionName);
                } else {
                    if (string.IsNullOrEmpty(name))
                        ret = string.Format("{0}/{1}/@{2}/_a/{2}_a", m_ActorResPath, partName, actionName);
                    else
                        ret = string.Format("{0}/{1}/@{2}/_{3}/a/{3}_a", m_ActorResPath, partName, name, actionName);
                }
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
        protected virtual Sprite[] LoadPartSprites(FigherPart part, FighterActionEnum action, string name = "") {
            string fileName = GeneratorActionPath(action, false, part, name);
            if (string.IsNullOrEmpty(fileName))
                return null;
            return Resources.LoadAll<Sprite>(fileName);
        }

        // 暂时用同步，后续用异步
        protected virtual Texture LoadPartAlphaTexture(FigherPart part, FighterActionEnum action, string name = "") {
            string fileName = GeneratorActionPath(action, true, part, name);
            if (string.IsNullOrEmpty(fileName))
                return null;
            return Resources.Load<Texture>(fileName);
        }

        protected virtual Material LoadPartMaterial(FighterActionEnum action, FigherPart part, string name) {
            return Resources.Load<Material>("material/actormat");
        }

        private void InitLoopData() {
            if (FighterStringEnumHelper.IsLoopNormalAction(m_CurrentAction)) {
                m_LoopDir = 0;
                m_LoopCount = -1;
                m_AniInfo.type = AniLoopType.aniLoop;
            } else {
                m_LoopDir = 0;
                m_LoopCount = -1;
                m_AniInfo.type = AniLoopType.aniOnce;
            }
        }

        private void Init(string name, bool isPlayer, FighterActionEnum action, int dir = 0) {
            m_Name = name;
            m_IsPlayer = isPlayer;
            // 初始化路径
            m_ActorResPath = GeneratorActorResPath();
            m_Dir = dir;
            m_CurrentAction = action;
            InitLoopData();
            // 优先创建身体
            CreatePart(action, FigherPart.Body);
        }

        public bool AddOtherPart(RecordOtherPartType otherPart) {
            
            FigherPart part;
            float z = 0f;
            switch (otherPart) {
                case RecordOtherPartType.avatar_decoration:
                    part = FigherPart.Avatar;
                    z = -0.001f;
                    break;
                case RecordOtherPartType.weapon_1:
                case RecordOtherPartType.weapon_2:
                case RecordOtherPartType.weapon_3:
                case RecordOtherPartType.weapon_4:
                case RecordOtherPartType.weapon_5:
                case RecordOtherPartType.weapon_6:
                    part = FigherPart.Weapon;
                    z = -0.002f;
                    break;
                default:
                    return false;
            }
            string name = FighterStringEnumHelper.GetOtherPartName(otherPart);

            SpriteA2D sprite = CreatePart(m_CurrentAction, part, name);
            sprite.Tag = (int)otherPart;
            if (sprite != null) {
                var trans = sprite.transform;
                Vector3 vec = trans.localPosition;
                vec.z = z;
                trans.localPosition = vec;
            }

            return true;
        }

        protected void AttachSpriteA2D(SpriteA2D sprite, FighterActionEnum action, FigherPart part, string name) {
            if (sprite == null)
                return;
            SpriteRenderer renderer = sprite.Renderer;
            if (renderer != null) {
                // 暂时先这样
                renderer.sharedMaterial = LoadPartMaterial(action, part, name);

                Texture alphaTex = LoadPartAlphaTexture(part, action, name);
                renderer.material.SetTexture("_UVTex", alphaTex);
            }

            // 后续考虑异步
            Sprite[] sps = LoadPartSprites(part, action, name);
            sprite.Init(sps, m_FrameType, m_AniInfo, m_LoopDir, m_LoopCount, m_Dir);
        }

        private SpriteA2D CreatePart(FighterActionEnum action, FigherPart part, string name = "") {

            SpriteA2D sprite = GetPartSpriteA2D(part);
            if (sprite == null) {
                string gameObjName = name;
                if (string.IsNullOrEmpty(gameObjName))
                    gameObjName = FighterStringEnumHelper.GetPartName(part);
                GameObject gameObj = new GameObject(gameObjName, typeof(SpriteA2D));
                gameObj.transform.SetParent(this.transform, false);
                sprite = gameObj.GetComponent<SpriteA2D>();
                m_PartMap[(int)part] = sprite;
            }

            AttachSpriteA2D(sprite, action, part, name);
            return sprite;
        }

        public void InitServerData(int serverId) {
            m_ServerId = serverId;

        }

        // 创建NPC或者宠物
        public static Fighter CreateFighter(int serverId, string name, bool isPlayer,
            int dir = 0,
            FighterActionEnum defaultAction = FighterActionEnum.Idle) {
            GameObject gameObj = new GameObject(name, typeof(Fighter));
            gameObj.transform.localScale = new Vector3(1.33f, 1.33f, 1f);
            Fighter ret = gameObj.GetComponent<Fighter>();
            ret.InitServerData(serverId);
            ret.Init(name, isPlayer, defaultAction, dir);
            return ret;
        }

        private void ResetData() {
            m_IsPlayer = false;
            m_PartMap.Clear();
            m_ActorResPath = string.Empty;
            m_CurrentAction = FighterActionEnum.Idle;
            m_Name = string.Empty;
            m_FrameType = SpriteFrameType.frame5Dir;
            m_AniInfo = SpriteA2D.GeneratorDefaultAniInfo();
            m_Dir = 0;
            m_LoopDir = 0;
            m_LoopCount = -1;
            m_Speed = 1f;
            m_ServerId = -1;
        }

        private void DestroyAllPart() {
            var iter = m_PartMap.GetEnumerator();
            while (iter.MoveNext()) {
                SpriteA2D sprite = iter.Current.Value;
                if (sprite != null) {
                    sprite.Destroy();
                }
            }
            iter.Dispose();
            m_PartMap.Clear();
        }

        // 删除
        public void Destroy() {
            DestroyAllPart();
            ResetData();

            // 释放GameObject
            GameObject.Destroy(gameObject);
        }

        // 服务器ID
        public int ServerId {
            get {
                return m_ServerId;
            }
        }

        public void ChangeAction(FighterActionEnum action, int dir) {
            if (m_CurrentAction == action && m_Dir == dir)
                return;
            m_CurrentAction = action;
            m_Dir = dir;
            InitLoopData();
            var iter = m_PartMap.GetEnumerator();
            while (iter.MoveNext()) {
                SpriteA2D sprite = iter.Current.Value;
                FigherPart part = (FigherPart)iter.Current.Key;
                string name = string.Empty;
                if (sprite.Tag != 0) {
                    RecordOtherPartType otherPartType = (RecordOtherPartType)sprite.Tag;
                    name = FighterStringEnumHelper.GetOtherPartName(otherPartType);
                }
                AttachSpriteA2D(sprite, m_CurrentAction, part, name);
            }
            iter.Dispose();
        }

        public static bool IsPosEnd(Vector3 destOrgDir, Vector3 dest, Vector3 current) {
            Vector3 curretDir = dest - current;
            float v = destOrgDir.x * current.x + destOrgDir.y * current.y;
            if (v <= float.Epsilon)
                return true;
            return false;
        }

        // 获得当前方向的target
        public int GetDestWorldPosDirect(Vector3 worldPos) {
            var orgVec = this.transform.position;
            float deltaX = worldPos.x - orgVec.x;
            float deltaY = worldPos.y - orgVec.y;
            if (deltaX < -float.Epsilon && deltaY >= 0) {
                if (deltaY <= 0.001f)
                    return 6;
                if (deltaX >= -0.001f)
                    return 0;
                return 7;
            } else if (deltaX > float.Epsilon && deltaY >= 0) {
                if (deltaY <= 0.001f)
                    return 2;
                if (deltaX <= 0.001f)
                    return 0;
                return 1;
            } else if (deltaX < -float.Epsilon && deltaY < 0) {
                if (deltaY >= -0.001f)
                    return 6;
                if (deltaX >= -0.001f)
                    return 4;
                return 5;
            } else if (deltaX > float.Epsilon && deltaY < 0) {
                if (deltaY >= -0.001f)
                    return 2;
                if (deltaX <= 0.001f)
                    return 4;
                return 3;
            }

            return m_Dir;
        }

        public void ChangeAction(FighterActionEnum action) {
            ChangeAction(action, m_Dir);
        }

        public float TickDetla {
            get {
                return ((float)SpriteA2D._cSpriteAnimateDeltaTick) * m_Speed;
            }
        }

        private void Update() {
            var iter = m_PartMap.GetEnumerator();
            while (iter.MoveNext()) {
                SpriteA2D sprite = iter.Current.Value;
                if (sprite != null && sprite.enabled) {
                    sprite.OnSpriteAnimateUpdate(m_Speed);
                    sprite.AttachSpriteRenderer();
                }
            }
            iter.Dispose();
        }

    }
}
