using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utility.Redis
{
    /// <summary>缓存服务接口</summary>
    public interface ICacheService
    {
        /// <summary>删除指定键值</summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        long Del(params string[] key);

        /// <summary>获取或设置指定键缓存值</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object this[string key] { get; set; }

        /// <summary>获取指定条件内的键</summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        List<string> getKeys(string where);

        /// <summary>获取缓存值</summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        string Get(string key);

        /// <summary>获取指定键下的缓存值集合</summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <param name="keys">键集合</param>
        /// <returns></returns>
        List<T> GetValues<T>(List<string> keys);

        /// <summary>设置缓存值</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(string key, string value);

        /// <summary>设置缓存值</summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        void SetAndExpire(string key, string value, DateTime expire);

        /// <summary>获取缓存对象</summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>设置缓存对象</summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="tentity">对象值</param>
        void Set<T>(string key, T tentity);

        /// <summary>设置缓存对象</summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="tentity">对象值</param>
        /// <param name="expire">过期时间</param>
        void SetAndExpire<T>(string key, T tentity, DateTime expire);

        #region Queue Item List

        /// <summary>将对象值加入指定的消息队列中</summary>
        /// <param name="key">列表Id</param>
        /// <param name="value">对象值</param>
        void EnqueueItemOnList(string key, object value);

        /// <summary>将对象值加入指定的消息队列中</summary>
        /// <param name="key">列表Id</param>
        /// <param name="value">值</param>
        void EnqueueItemOnList(string key, string value);

        /// <summary>获取指定队列中的对象值</summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <param name="key">列表Id</param>
        /// <returns></returns>
        T DequeueItemFromList<T>(string key);

        /// <summary>获取指定队列中的对象值</summary>
        /// <param name="key">列表Id</param>
        /// <returns></returns>
        string DequeueItemFromList(string key);

        /// <summary>获取指定队列中的对象值总数</summary>
        /// <param name="key">列表Id</param>
        /// <returns></returns>
        int GetListCount(string key);

        #endregion

    }
}
