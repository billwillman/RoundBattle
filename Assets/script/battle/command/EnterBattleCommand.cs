using System;

namespace RoundBattle.Command
{
    // 进入战场
    internal class EnterBattleCommand: Command
    {
        public override void StartRun()
        {
            var seatMgr = BattleSystem.GetInstance ().SeatMgr;
            SeatInfo seatInfo = new SeatInfo ();
            seatInfo.seat = SeatType.left;
            for (int i = 0; i < SeatManager._cSeatCount; ++i) {
                seatInfo.pos = i;
                seatMgr.ChangeState (seatInfo, FighterStates.Fighter_StartFight); 
            }

            seatInfo.seat = SeatType.right;
            for (int i = 0; i < SeatManager._cSeatCount; ++i) {
                seatInfo.pos = i;
                seatMgr.ChangeState (seatInfo, FighterStates.Fighter_StartFight);
            }
        }
    }
}

