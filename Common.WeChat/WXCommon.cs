using Common.Utility;
using Common.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WeChat.Models;
using System.Threading;

namespace WeChat
{
    /// <summary>微信公共处理类</summary>
    public class WXCommon : BaseCommon
    {
        /// <summary></summary>
        public static WXCommon Instance { get; private set; }

        /// <summary></summary>
        private WXCommon()
        {
            var e = Utils.GetSettingValue("Environment");
            switch (e)
            {
                case "dev":
                    Initialize("wx165579c7fca21c14", "", "", "dcd8424687ebdf63e116aca973ddc645", "1000003");
                    break;
                case "test":
                    Initialize("wx35342ac41e22b328", "", "", "d4624c36b6795d1d99dcf0547af5443d", "1000003");
                    break;
                case "pro":
                    Initialize("", "", "", "", "");
                    break;
                default:
                    Initialize("wx35342ac41e22b328", "", "", "d4624c36b6795d1d99dcf0547af5443d", "1000003");
                    break;
            }
        }
        /// <summary></summary>
        static WXCommon()
        {
            Instance = new WXCommon();
            // 启动缓存微信鉴权信息，7000秒刷新一次
            Instance.Start();
        }

        /// <summary>初始化</summary>
        /// <param name="appid"></param>
        /// <param name="mchid"></param>
        /// <param name="key"></param>
        /// <param name="secret"></param>
        /// <param name="agentid"></param>
        private void Initialize(string appid, string mchid, string key, string secret,string agentid)
        {
            AppIdCorpId = appid;
            MCHID = mchid;
            KEY = key;
            CorpSecret = secret;
            AgentId = agentid;
        }

    }
}