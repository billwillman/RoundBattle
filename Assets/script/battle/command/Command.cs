using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace RoundBattle.Command {
    public class Command {
    }

    public interface IFighterStateListener {
        void OnActionChanged(FighterActionEnum lastAction, Fighter fighter);
        void OnKeyFrameChanged(int lastFrame, Fighter fighter);
    }

    // 角色基础状态
    public class FighterBaseState: IState<FighterStates, Fighter> {
        public virtual bool CanEnter(Fighter target) {
            return true;
        }
        public virtual bool CanExit(Fighter target) {
            return true;
        }
        public virtual void Enter(Fighter target) { }
        public virtual void Exit(Fighter target) { }
        public virtual void Process(Fighter target) 
        {
            OnCheckActionFrame(target);
        }

        public IFighterStateListener Listener {
            get;
            set;
        }

        public FighterStates Id {
            get;
            set;
        }

        protected virtual void OnActionChanged(FighterActionEnum lastAction, Fighter fighter) {

        }

        protected virtual void OnKeyFrameChanged(int lastFrame, Fighter fighter) {

        }

        private void OnCheckActionFrame(Fighter target) {
            if (target == null)
                return;
            FighterActionEnum currentAction = target.CurrentAction;
            bool isVaildAction = currentAction != FighterActionEnum.None;
            if (isVaildAction) {
                if (currentAction != m_LastAction) {
                    m_LastKeyFrame = -1;
                    OnActionChanged(currentAction, target);
                    if (Listener != null)
                        Listener.OnActionChanged(m_LastAction, target);
                    m_LastAction = currentAction;
                }

                int currentFrame = target.CurrentFrameIndex;
                if (currentFrame >= 0 && m_LastKeyFrame != currentFrame) {
                    OnKeyFrameChanged(m_LastKeyFrame, target);
                    if (Listener != null)
                        Listener.OnKeyFrameChanged(m_LastKeyFrame, target);
                    m_LastKeyFrame = currentFrame;
                }
            }
        }

        private int m_LastKeyFrame = -1;
        private FighterActionEnum m_LastAction = FighterActionEnum.None;
    }
}
