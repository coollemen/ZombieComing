namespace GameFramework
{
    /// <summary>
    /// 消息的类型  
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 启动
        /// </summary>
        START_UP = 1000,

        /// <summary>
        /// 解压
        /// </summary>
        UNPACK,

        /// <summary>
        /// 更新
        /// </summary>
        UPDATE,

        /// <summary>
        /// 更新完成
        /// </summary>
        UPDATE_COMPLETE,
    }


    /// <summary>
    /// 战斗的类型
    /// </summary>
    public enum BattleEvent
    {
        /// <summary>
        /// 攻击
        /// </summary>
        Attack = 10000,
    }

    /// <summary>
    /// 协议的类型
    /// </summary>
    public enum ProtocolEvent
    {
    }
}