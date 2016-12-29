using System;

namespace Common.Caches
{
    /// <summary>
    /// 同步操作接口
    /// </summary>
    public interface ISynchrony
    {
        /// <summary>
        /// 同步数据
        /// </summary>
        /// <param name="parameter">同步时参数</param>
        void Synchronizing(object parameter);

        /// <summary>
        /// 开始同步
        /// </summary>
        /// <param name="delay">时间长度</param>
        void Start(long delay = 0);

        /// <summary>
        /// 结束同步
        /// </summary>
        void Stop();
    }
}
