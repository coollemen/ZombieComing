using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace GameFramework
{
    [System.Serializable]
    public class CommandManager
    {
        /// <summary>
        /// 需要执行的指令
        /// </summary>
        [ShowInInspector]
        private List<ICustomCommand> commands = new List<ICustomCommand>();

        /// <summary>
        /// 已完成的指令
        /// </summary>
        [ShowInInspector]
        private Stack<ICustomCommand> executedCommands = new Stack<ICustomCommand>();
        /// <summary>
        /// 待执行指令数量
        /// </summary>
        public int WaitCmdCount
        {
            get { return this.commands.Count; }
        }
        /// <summary>
        /// 已执行指令数量
        /// </summary>
        public int ExcutedCmdCount
        {
            get { return this.executedCommands.Count; }
        }
        public CommandManager()
        {
        }
        
        /// <summary>
        /// 添加指令
        /// </summary>
        /// <param name="cmd">指令</param>
        public void AddCommand(ICustomCommand cmd)
        {
            this.commands.Add(cmd);
        }

        /// <summary>
        /// 获取下一个待执行指令
        /// </summary>
        /// <returns></returns>
        public ICustomCommand GetNextCommand()
        {
            var cmd = this.commands[0];
            this.commands.RemoveAt(0);
            return cmd;
        }

        public void ExcuteNextCommand(GameObject go)
        {
            //获取下一个需要执行的指令
            var nextCmd = this.GetNextCommand();
            //执行指令
            nextCmd.Execute(go);
            //将已执行的指令放到已执行指令队列中
            this.executedCommands.Push(nextCmd);
        }

        /// <summary>
        /// 获取最新已执行的指令
        /// </summary>
        /// <returns></returns>
        public ICustomCommand GetLastCommand()
        {
            return this.executedCommands.Pop();
        }

        public void UndoLastCommand(GameObject go)
        {
            //获取上一次执行的指令
            var lastCmd = this.GetLastCommand();
            //执行撤销指令
            lastCmd.Undo(go);
            //将已撤销的指令放入到待执行指令队列中
            this.commands.Insert(0, lastCmd);
        }
    }
}