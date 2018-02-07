using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RoundBattle {

    // 几方向，5方向还是2方向
    public enum SpriteFrameType {
        // 两方向
        frame2Dir = 2,
        // 五方向
        frame5Dir = 5,
        // 八方向
        frame8Dir = 8
    }

    public enum AniLoopType {
        aniOnce = 0,
        aniLoop
    }

    public enum AniType {
        AniNone = 0,
        AniPingPang
    }

    // 动画类型
    public struct AniInfo {
        public AniLoopType type {
            get;
            set;
        }

        public AniType aniType {
            get;
            set;
        }

        // 帧间隔时间(单位: 毫秒)
        public int frameTick {
            get;
            set;
        }
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteA2D: MonoBehaviour {
        private SpriteFrameType m_FrameType = SpriteFrameType.frame5Dir;
        // 所有帧
        private Sprite[] m_Frames = null;
        // 当前方向
        private int m_CurrentDir = -1;
        private int m_CurrentFrameIndex = -1;
        private int m_FrameCount = 0;
        private bool m_IsFlip = false;
        /* 动画特性 */
        private AniInfo m_AniInfo;
        // 不限制
        private int m_LoopCount = -1;
        private int m_LoopDir = 0; // 0: INDEX从低到高 1:从高到低
        private int m_LoopIndex = -1;
        private int m_AniTickCount = 0;
        private int m_LastFrameTick = 0;
        /*----------------*/
        private SpriteRenderer m_Renderer = null;

        private void OnEnable() {
            
        }

        private void ResetData() {
            m_FrameType = SpriteFrameType.frame5Dir;
            m_Frames = null;
            m_CurrentDir = -1;
            m_CurrentFrameIndex = -1;
            m_FrameCount = 0;
            m_IsFlip = false;
            m_AniInfo = GeneratorDefaultAniInfo();
            m_LoopCount = -1;
            m_LoopDir = 0;
            m_LoopIndex = -1;
            m_AniTickCount = 0;
            m_LastFrameTick = 0;
        }

        private void CalcFrameCount() {
            if (!IsVaildData) {
                m_FrameCount = 0;
                return;
            }
            m_FrameCount = m_Frames.Length / (int)m_FrameType;
        }

        public SpriteRenderer Renderer {
            get {
                if (m_Renderer == null)
                    m_Renderer = GetComponent<SpriteRenderer>();
                return m_Renderer;
            }
        }

        public static AniInfo GeneratorDefaultAniInfo() {
            AniInfo ret = new AniInfo();
            ret.frameTick = 200;
            return ret;
        }

        public void Init(Sprite[] sps, SpriteFrameType frameType, int dir = 0) {
            Init(sps, frameType, GeneratorDefaultAniInfo(), 0, -1, dir);
        }

        public void Init(Sprite[] sps, SpriteFrameType frameType,
            AniInfo info, int loopDir = 0, int loopCount = -1,
            int dir = 0) {
            m_Frames = sps;
            m_FrameType = frameType;
            m_CurrentDir = dir;
            m_CurrentFrameIndex = -1;
            CalcFrameCount();
            SetAniData(info, loopDir, loopCount);
            TurnDir(dir, true);
        }

        private void SetAniData(AniInfo info, int loopDir = 0, int loopCount = -1) {
            m_AniInfo = info;
            m_LoopCount = loopCount;
            m_LoopDir = loopDir;
            m_LoopIndex = 0;
            m_LastFrameTick = info.frameTick;
        }

        public AniInfo AniData {
            get {
                return m_AniInfo;
            }
        }

        public int CurrentDir {
            get {
                return m_CurrentDir;
            }
        }

        public int CurrentFrameIndex {
            get {
                return m_CurrentFrameIndex;
            }
        }

        public bool IsVaildData {
            get {
                return m_Frames != null && m_Frames.Length > 0;
            }
        }

        protected bool IsVaidFrame {
            get {
                return IsVaildData && m_FrameCount > 0;
            }
        }

        public int FrameCount {
            get {
                return m_FrameCount;
            }
        }

        // 当前Sprite
        public Sprite CurrentSprite {
            get {
                int frameIndex = RealFrameIndex;
                if (frameIndex < 0 || frameIndex >= m_Frames.Length)
                    return null;
                return m_Frames[frameIndex];
            }
        }

        public int RealDir {
            get {
                int dir = m_CurrentDir;
                switch (m_FrameType) {
                    case SpriteFrameType.frame2Dir:
                        // 1, 3, 5, 7
                        if (dir != 1 && dir != 3 && dir !=5 && dir != 7)
                            return -1;
                        if (dir == 7) {
                            m_IsFlip = true;
                            dir = 1;
                        } else if (dir == 5) {
                            m_IsFlip = true;
                            dir = 3;
                        }
                        break;
                    case SpriteFrameType.frame5Dir:
                        if (dir > 7)
                            return -1;
                        if (dir == 7) {
                            m_IsFlip = true;
                            dir = 1;
                        } else if (dir == 5) {
                            m_IsFlip = true;
                            dir = 3;
                        }
                        break;
                    case SpriteFrameType.frame8Dir:
                        if (dir > 7)
                            return -1;
                        break;
                }
                return dir;
            }
        }

        public int RealFrameIndex {
            get {
                if (!IsVaidFrame)
                    return -1;

                int dir = RealDir;
                if (dir < 0)
                    return -1;
                int frameIndex = dir * (int)m_FrameCount + m_CurrentFrameIndex;

                return frameIndex;
            }
        }

        public int CurrentBeginFrame {
            get {
                if (!IsVaidFrame)
                    return -1;
                switch (m_LoopDir) {
                    case 0:
                        return 0;
                    case 1:
                        return m_FrameCount - 1;
                    default:
                        return -1;
                }
            }
        }

        public int CurrentEndFrame {
            get {
                if (!IsVaidFrame)
                    return -1;
                switch (m_LoopDir) {
                    case 0:
                        return m_FrameCount - 1;
                    case 1:
                        return 0;
                    default:
                        return -1;
                }
            }
        }

        public void Destroy() {
            ResetData();
            GameObject.Destroy(gameObject);
        }

        // 转向
        public bool TurnDir(int dir, bool resetFirstFrame = false) {
            if (!IsVaildData) {
                m_CurrentDir = -1;
                m_CurrentFrameIndex = - 1;
                return false;
            }
            resetFirstFrame = resetFirstFrame || m_CurrentDir < 0 || m_CurrentDir >= (int)m_FrameType ||
                              m_CurrentFrameIndex < 0 || m_CurrentFrameIndex >= m_FrameCount;
            if (resetFirstFrame) {
                m_CurrentFrameIndex = CurrentBeginFrame;
                m_LastFrameTick = m_AniInfo.frameTick;
            }
            m_CurrentDir = dir;

            return true;
        }

        // 手动调用下一帧
        public bool NextFrame() {
            if (!IsVaidFrame)
                return false;

            int nextFrame;
            switch (m_LoopDir) {

                case 0:
                    nextFrame = m_CurrentFrameIndex + 1;
                    if (nextFrame >= m_FrameCount) {
                        if (m_AniInfo.type == AniLoopType.aniLoop) {
                            if (m_LoopCount <= 0 ||
                                (m_LoopIndex + 1 < m_LoopCount)) {
                                ++m_LoopIndex;
                                nextFrame = CurrentBeginFrame;
                            } else
                                nextFrame = m_FrameCount - 1;
                        } else
                            nextFrame = m_FrameCount - 1;
                    } else if (nextFrame == m_FrameCount - 1) {
                        if (m_AniInfo.aniType == AniType.AniPingPang) {
                            m_LoopDir = 1;
                        }
                    }
                    break;

                case 1:
                    nextFrame = m_CurrentFrameIndex - 1;
                    if (nextFrame < 0) {
                        if (m_AniInfo.type == AniLoopType.aniLoop) {
                            if (m_LoopCount <= 0 || 
                                (m_LoopIndex + 1 < m_LoopCount)) {
                                ++m_LoopIndex;
                                nextFrame = CurrentBeginFrame;
                            } else
                                nextFrame = 0;
                        } else
                            nextFrame = 0;
                    } else if (nextFrame == 0) {
                        if (m_AniInfo.aniType == AniType.AniPingPang && 
                            m_AniInfo.type == AniLoopType.aniLoop) {
                            if (m_LoopCount <= 0 ||
                                (m_LoopIndex + 1 < m_LoopCount)) {
                                ++m_LoopIndex;
                                m_LoopDir = 0;
                            }
                        }
                    }
                    break;
                default:
                    return false;
            }

            m_CurrentFrameIndex = nextFrame;

            return true;
        }

        // 动画更新调用
        public void OnSpriteAnimateUpdate(int tickCount) {
            if (m_AniTickCount == 0) {
                m_AniTickCount = tickCount;
            }

            int delta = tickCount - m_AniTickCount;
            if (delta <= 0)
                return;

            m_LastFrameTick -= delta;
            if (m_LastFrameTick <= 0) {
                NextFrame();
                m_LastFrameTick = m_AniInfo.frameTick;
            }
        }

        private static readonly int _cSpriteAnimateDeltaTick = (int)(1f/30f * 1000f);
        // 采用默认帧频率调用
        public void OnSpriteAnimateUpdate(float speed = 1.0f) {
            int tickCount = (int)((float)m_AniTickCount + _cSpriteAnimateDeltaTick * speed);
            OnSpriteAnimateUpdate(tickCount);
        }

        public void AttachSpriteRenderer(SpriteRenderer renderer) {
            if (renderer == null)
                return;
            // 重置状态
            m_IsFlip = false;
            renderer.sprite = CurrentSprite;
            renderer.flipX = m_IsFlip;
        }

        public void AttachSpriteRenderer() {
            SpriteRenderer renderer = this.Renderer;
            if (renderer == null)
                return;
            AttachSpriteRenderer(renderer);
        }
    }
}
