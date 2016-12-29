using System;
using System.Collections;
using System.Threading;

namespace Common.Caches
{
    /// <summary></summary>
	public abstract class SynCache : Cache, ISynchrony
    {
        /// <summary></summary>
		protected Timer timer;

        /// <summary></summary>
		protected abstract Hashtable GetData();

        /// <summary></summary>
        public virtual void Synchronizing(object parameter) { this.collection = this.GetData(); }

        /// <summary></summary>
		public void Start(long delay = 0)
		{
			this.Stop();
            if (delay > 0) { this.timer = new Timer(this.Synchronizing, null, 0, delay); }
            else { this.Synchronizing(null); }
		}

        /// <summary></summary>
		public void Stop()
		{
			if (this.timer != null)
			{
				this.timer.Dispose();
				this.timer = null;
			}
		}
	}
}
