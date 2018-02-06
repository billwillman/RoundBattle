using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RoundBattle.Command {
    // 命令管理器
    public class CommandManager {

        private LinkedList<Command> m_CmdList = new LinkedList<Command>();

        // 清理
        public void Clear() {
            m_CmdList.Clear();
        }
    }
}
