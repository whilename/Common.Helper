using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Common.Caches
{
    /// <summary>缓存接口</summary>
    public interface ICache
    {
        /// <summary>清空缓存</summary>
        void Clear();

        /// <summary>根据KEY取值</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(object key);

        /// <summary>根据KEY取值</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(object key);

        /// <summary>将对象加入缓存</summary>
        /// <param name="key">对象KEY</param>
        /// <param name="obj">对象</param>
        bool Put(object key, object obj);

        /// <summary>根据key设置已存在缓存对象新值</summary>
        /// <param name="key">对象KEY</param>
        /// <param name="obj">对象</param>
        void Set(object key, object obj);

        /// <summary>根据KEY删除缓存</summary>
        /// <param name="key"></param>
        void Remove(object key);

        /// <summary>获取全部</summary>
        List<object> GetAll();
    }

    /// <summary>缓存</summary>
    public class Cache : ICache
    {
        /// <summary></summary>
        protected Hashtable collection = new Hashtable();

        /// <summary>根据KEY取值</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(object key) { return key == null ? null : this.collection[key]; }

        /// <summary>根据KEY取值</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(object key) { return key == null ? default(T) : (T)this.collection[key]; }

        /// <summary>根据key设置已存在缓存对象新值</summary>
        /// <param name="key">对象KEY</param>
        /// <param name="obj">对象</param>
        public void Set(object key, object obj) { this.collection[key] = obj; }

        /// <summary>将对象加入缓存</summary>
        /// <param name="key">对象KEY</param>
        /// <param name="obj">对象</param>
        public bool Put(object key, object obj) { try { this.collection.Add(key, obj); return true; } catch { return false; } }

        /// <summary>获取或设置指定KEY的值</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[object key] { get { return this.collection[key]; } set { this.collection[key] = value; } }

        /// <summary>清空缓存</summary>
        public void Clear() { this.collection = new Hashtable(); }

        /// <summary>根据KEY删除缓存</summary>
        /// <param name="key"></param>
        public void Remove(object key) { this.collection.Remove(key); }

        /// <summary>获取全部</summary>
        public List<object> GetAll()
        {
            List<object> list = new List<object>();
            foreach (DictionaryEntry entry in this.collection) { list.Add(entry.Value); }
            return list;
        }
    }
}
