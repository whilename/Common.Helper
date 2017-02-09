using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Common.Utility.Redis
{
    /// <summary>RedisService</summary>
    public class RedisService : ICacheService, IDisposable
    {
        // 当前对象实例 
        private static ICacheService instance = null;
        private static readonly object padlock = new object();
        // 使用Redis的客户端管理器（对象池）
        IRedisClientsManager prcm;

        /// <summary></summary>
        private RedisService()
        {
            // 获取设置缓存服务链接地址
            var connections = ConfigurationManager.AppSettings["dlm:redis"];
            // 如果是Redis集群则配置多个{IP地址:端口号}即可,例如: "10.0.0.1:6379","10.0.0.2:6379","pwd110@10.0.0.3:6379",
            var param = new string[] { connections };
            // 初始化连接池对象
            prcm = new PooledRedisClientManager(param, param, new RedisClientManagerConfig { MaxWritePoolSize = 5, MaxReadPoolSize = 5, AutoStart = true, });
        }

        /// <summary>获取缓存对象实例</summary>
        public static ICacheService Instance
        {
            get
            {
                if (instance == null)
                {
                    // 线程单列模式
                    lock (padlock) { if (instance == null) { instance = new RedisService(); } }
                }
                return instance;
            }
        }

        /// <summary>执行与释放或重置非托管资源相关的应用程序定义的任务</summary>
        public void Dispose()
        {
            prcm.Dispose();
        }

        /// <summary>删除指定键值</summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public long Del(params string[] key)
        {
            using (RedisClient redis = prcm.GetClient() as RedisClient)
            {
                return (key != null && key.Length > 0) ? redis.Del(key) : 0;
            }
        }

        /// <summary>获取或设置指定键缓存值</summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public object this[string key]
        {
            get { return Get(key); }
            set { Set(key, value); }
        }

        #region Get

        /// <summary>获取指定条件内的键</summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public List<string> getKeys(string where)
        {
            using (RedisClient redis = prcm.GetClient() as RedisClient)
            {
                List<string> list = redis.SearchKeys(where + "*");
                return list;
            }
        }

        /// <summary>获取缓存值</summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string Get(string key)
        {
            using (RedisClient redis = prcm.GetClient() as RedisClient)
            {
                return redis.Get<string>(key);
            }
        }

        /// <summary>获取缓存对象</summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            using (RedisClient redis = prcm.GetClient() as RedisClient)
            {
                return redis.Get<T>(key);
            }
        }

        /// <summary>获取指定键下的缓存值集合</summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <param name="keys">键集合</param>
        /// <returns></returns>
        public List<T> GetValues<T>(List<string> keys)
        {
            using (RedisClient redis = prcm.GetClient() as RedisClient)
            {
                return redis.GetValues<T>(keys);
            }
        }

        #endregion

        #region Set

        /// <summary>设置缓存值</summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Set(string key, string value)
        {
            using (RedisClient redis = prcm.GetClient() as RedisClient)
            {
                redis.Set<string>(key, value);
            }
        }

        /// <summary>设置缓存值</summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间</param>
        public void SetAndExpire(string key, string value, DateTime expire)
        {
            using (RedisClient redis = prcm.GetClient() as RedisClient)
            {
                redis.Set<string>(key, value, expire);
            }
        }

        /// <summary>设置缓存对象</summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="entity">对象值</param>
        public void Set<T>(string key, T entity)
        {
            using (RedisClient redis = prcm.GetClient() as RedisClient)
            {
                redis.Set<T>(key, entity);
            }
        }

        /// <summary>设置缓存对象</summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="entity">对象值</param>
        /// <param name="expire">过期时间</param>
        public void SetAndExpire<T>(string key, T entity, DateTime expire)
        {
            using (RedisClient redis = prcm.GetClient() as RedisClient)
            {
                redis.Set<T>(key, entity);
            }
        }

        #endregion

        #region Queue Item List

        /// <summary>将对象值加入指定的消息队列中</summary>
        /// <param name="listId">列表Id</param>
        /// <param name="value">值</param>
        public void EnqueueItemOnList(string listId, string value)
        {
            using (RedisClient redis = prcm.GetClient() as RedisClient)
            {
                redis.EnqueueItemOnList(listId, value);
            }
        }

        /// <summary>将对象值加入指定的消息队列中</summary>
        /// <param name="listId">列表Id</param>
        /// <param name="entity">对象值</param>
        public void EnqueueItemOnList(string listId, object entity)
        {
            using (RedisClient redis = prcm.GetClient() as RedisClient)
            {
                redis.EnqueueItemOnList(listId, JsonConvert.SerializeObject(entity));
            }
        }

        /// <summary>获取指定队列中的对象值</summary>
        /// <param name="listId">列表Id</param>
        /// <returns></returns>
        public string DequeueItemFromList(string listId)
        {
            using (RedisClient redis = prcm.GetClient() as RedisClient)
            {
                return redis.DequeueItemFromList(listId);
            }
        }

        /// <summary>获取指定队列中的对象值</summary>
        /// <typeparam name="T">指定对象类型</typeparam>
        /// <param name="listId">列表Id</param>
        /// <returns></returns>
        public T DequeueItemFromList<T>(string listId)
        {
            using (RedisClient redis = prcm.GetClient() as RedisClient)
            {
                string value = redis.DequeueItemFromList(listId);
                if (string.IsNullOrEmpty(value)) { return default(T); }
                return JsonConvert.DeserializeObject<T>(value);
            }
        }

        /// <summary>获取指定队列中的对象值总数</summary>
        /// <param name="listId">列表Id</param>
        /// <returns></returns>
        public int GetListCount(string listId)
        {
            using (RedisClient redis = prcm.GetClient() as RedisClient)
            {
                return redis.GetListCount(listId);
            }
        }

        #endregion
    }
}
