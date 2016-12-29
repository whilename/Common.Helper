using System;
using System.Threading;

namespace Common.Utility
{
    /// <summary>异步处理接口</summary>
    public interface IAsynchronous
    {
        /// <summary>异步执行</summary>
        void Asynchronous(object parameter);
        /// <summary>开始异步执行</summary>
        void Start(int index = 0);
        /// <summary>结束异步执行</summary>
        void Stop();
        /// <summary>继续异步执行</summary>
        void Continue();
    }

    /// <summary>异步定时任务</summary>
    public abstract class AsynTimer : IAsynchronous
    {
        /// <summary>定时器</summary>
        protected Timer timer;
        /// <summary>延时执行时间数组</summary>
        protected long[] delays = { };
        /// <summary>延时执行时间索引</summary>
        protected int index = 0;
        /// <summary>间隔时间，毫秒</summary>
        protected long intervals = 0;

        /// <summary>开始异步执行</summary>
        public void Start(int index = 0)
        {
            this.Stop();
            this.index = index;
            this.FirstExecute();
            this.Continue();
        }
        /// <summary>结束异步执行</summary>
        public void Stop()
        {
            if (this.timer != null)
            {
                this.timer.Dispose();
                this.timer = null;
            }
        }
        /// <summary>执行方法</summary>
        public abstract bool Execute();
        /// <summary>首次执行</summary>
        public virtual void FirstExecute() { }
        /// <summary>异步执行</summary>
        public void Asynchronous(object parameter)
        {
            this.Stop();
            if (this.Execute()) { this.Continue(); }
        }
        /// <summary>继续异步执行</summary>
        public void Continue()
        {
            long delay = 0;
            if (this.index < this.delays.Length)
            {
                delay = this.delays[this.index++];
            }
            else { delay = this.intervals; }
            if (delay > 0)
            {
                this.timer = new Timer(this.Asynchronous, null, delay, 0);
            }
        }
    }
}
