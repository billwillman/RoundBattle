using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RoundBattle;

namespace RoundBattle.Test {
    [RequireComponent(typeof(SpriteRenderer), typeof(SpriteA2D))]
    public class SpriteA2DBinder : MonoBehaviour {
        private SpriteA2D m_SpriteA2D = null;
        private SpriteRenderer m_Renderere = null;
        private Sprite[] m_Sprites = null;

        // 播放速度
        public float m_Speed = 1.0f;
        public string m_RootPath = string.Empty;
        public string m_ActionName = string.Empty;
        public SpriteFrameType m_FrameType = SpriteFrameType.frame5Dir;
        public int m_Dir = 0;
        public int m_FrameTick = 0;
        public bool m_IsLoop = true;
        public bool m_IsPlayer = false;
        public AniType m_AniType = AniType.AniNone;

        public void SetAction(FighterActionEnum action) {
            string actionName = FighterStringEnumHelper.GetActionName(action);
            SetAction(actionName);
        }

        public void SetAction(string actName) {
            if (string.IsNullOrEmpty(actName) || string.IsNullOrEmpty(m_RootPath) || 
                m_Renderere == null || m_SpriteA2D == null)
                return;
            
            string actRootPath;
            if (m_RootPath[m_RootPath.Length - 1] == '/') {
                if (m_IsPlayer)
                    actRootPath = string.Format("{0}body/@{1}", m_RootPath, actName);
                else
                    actRootPath = string.Format("{0}_{1}", m_RootPath, actName);
            } else {
                if (m_IsPlayer)
                    actRootPath = string.Format("{0}/body/@{1}", m_RootPath, actName);
                else
                    actRootPath = string.Format("{0}/_{1}", m_RootPath, actName);
            }
            string fileName = string.Format("{0}/{1}", actRootPath, actName);
            m_Sprites = Resources.LoadAll<Sprite>(fileName);
            if (m_Sprites == null) {
                Debug.LogError("SpriteA2DBinder: m_Sprites is null!");
                return;
            }

            AniInfo aniInfo = SpriteA2D.GeneratorDefaultAniInfo();
            aniInfo.type = m_IsLoop ? AniLoopType.aniLoop : AniLoopType.aniOnce;
            aniInfo.aniType = m_AniType;
            aniInfo.frameTick = m_FrameTick;
            m_SpriteA2D.Init(m_Sprites, m_FrameType, aniInfo, 0, -1, m_Dir);

            m_SpriteA2D.AttachSpriteRenderer(m_Renderere);
            string alphaFileName;
            if (m_IsPlayer)
                alphaFileName = string.Format("{0}/_a/{1}_a", actRootPath, actName);
            else
                alphaFileName = string.Format("{0}/a/{1}_a", actRootPath, actName);
            Texture alphaTex = Resources.Load<Texture>(alphaFileName);
            Material mat = m_Renderere.material;
            if (mat != null) {
                mat.SetTexture("_UVTex", alphaTex);
            }
        }

        private void InitSpriteA2D() {
            if (string.IsNullOrEmpty(m_RootPath)) {
                Debug.LogError("SpriteA2DBinder: m_SpriteFileName is null or empty!");
                return;
            }

            if (string.IsNullOrEmpty(m_ActionName))
                m_ActionName = "idle";
            SetAction(m_ActionName);
        }

        private void Start() {
            m_Renderere = GetComponent<SpriteRenderer>();
            m_SpriteA2D = GetComponent<SpriteA2D>();
            InitSpriteA2D();
        }

        private void Update() {
            if (m_SpriteA2D != null) {
                m_SpriteA2D.OnSpriteAnimateUpdate(m_Speed);
                m_SpriteA2D.AttachSpriteRenderer(m_Renderere);
            }
        }
    }
}