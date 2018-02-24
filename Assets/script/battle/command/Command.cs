using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace RoundBattle.Command {

    // 命令状态
    internal enum CommandStatus
    {
        // 等待执行
        Wait = 0,
        // 执行中
        Run,
        // 执行完毕
        Done
    }

    // 命令基类
    internal abstract class Command {
        private CommandStatus m_Status = CommandStatus.Wait;

        // 是否可运行
        public virtual bool CanRun()
        {
            return true;
        }

        public virtual void StartRun()
        {}

        public virtual void Runing()
        {}

        public virtual void EndRun()
        {}

        public CommandStatus Status
        {
            get
            {
                return m_Status;
            }
        }
    }

    public interface IFighterStateListener {
        void OnActionChanged(FighterActionEnum lastAction, Fighter fighter);
        void OnKeyFrameChanged(int lastFrame, Fighter fighter);
        // 最后一帧播放完毕
        void OnEndKeyFrameEnd(Fighter fighter);
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
        public virtual void Update(Fighter target) { }
        public void Process(Fighter target) 
        {
            OnCheckActionFrame(target);
            Update(target);
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

        // 最后一帧播放完毕
        protected virtual void OnEndKeyFrameEnd(Fighter fighter) {

        }

        private void OnCheckActionFrame(Fighter target) {
            if (target == null)
                return;

            if (target.FighterStateData == null)
                target.FighterStateData = new DefaultFighterStateData ();

            if (target.FighterStateData.ActionData == null)
                return;

            FighterActionEnum currentAction = target.CurrentAction;
            bool isVaildAction = currentAction != FighterActionEnum.None;
            if (isVaildAction) {
                var lastAction = target.FighterStateData.ActionData.LastAction;
                var lastKeyFrame = target.FighterStateData.ActionData.LastKeyFrame;
                if (currentAction != lastAction) {
                    lastKeyFrame = -1;
                    OnActionChanged(currentAction, target);
                    if (Listener != null)
                        Listener.OnActionChanged(lastAction, target);
                    lastAction = currentAction;
                }

                int currentFrame = target.CurrentFrameIndex;
                int currentFrameCount = target.CurrentFrameCount;
                if (currentFrame >= 0 && currentFrameCount > 0 && lastKeyFrame != currentFrame) {
                    OnKeyFrameChanged(lastKeyFrame, target);
                    if (Listener != null)
                        Listener.OnKeyFrameChanged(lastKeyFrame, target);
                    lastKeyFrame = currentFrame;
                } else if (currentFrame >= 0 && currentFrame == currentFrameCount - 1 && lastKeyFrame == currentFrame) {
                    OnEndKeyFrameEnd(target);
                    if (Listener != null)
                        Listener.OnEndKeyFrameEnd(target);
                }

                target.FighterStateData.ActionData.LastAction = lastAction;
                target.FighterStateData.ActionData.LastKeyFrame = lastKeyFrame;
            }
        }
    }
}
