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
    public class WXCommon : BaseWechat
    {
        /// <summary></summary>
        private static readonly object padlock = new object();

        /// <summary></summary>
        private WXCommon()
        {
            var e = Utils.GetSettingValue("Environment");
            switch (e)
            {
                case "dev":
                    Initialize("wx165579c7fca21c14", "", "", "dcd8424687ebdf63e116aca973ddc645", "");
                    break;
                case "test":
                    Initialize("wx35342ac41e22b328", "", "", "d4624c36b6795d1d99dcf0547af5443d", "");
                    break;
                case "pro":
                    Initialize("", "", "", "", "");
                    break;
                default:
                    Initialize("wx35342ac41e22b328", "", "", "d4624c36b6795d1d99dcf0547af5443d", "");
                    break;
            }
        }

        private static WXCommon instance = null;
        /// <summary>获取缓存对象实例</summary>
        public static WXCommon Instance
        {
            get
            {
                if (instance == null)
                {
                    // 线程单列模式
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new WXCommon();
                            // 启动缓存微信鉴权信息，7000秒刷新一次
                            instance.Start();
                        }
                    }
                }
                return instance;
            }
        }

    }

    /// <summary>企业号</summary>
    public class CorpCommon : BaseWechat
    {
        private static readonly object padlock = new object();

        /// <summary></summary>
        private CorpCommon()
        {
            Initialize("ww2906385266c2cc7c", "", "", "cp7yHx5XX0n1gBBvNmV3xUZ0C76k91QFeyoZ9CeDePY", "1000003");
        }

        private static CorpCommon instance = null;
        /// <summary>获取缓存对象实例</summary>
        public static CorpCommon Instance
        {
            get
            {
                if (instance == null)
                {
                    // 线程单列模式
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new CorpCommon();
                            // 启动缓存微信鉴权信息，7000秒刷新一次
                            instance.Start();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary></summary>
        public void StartMenu()
        {
            WXMenus menu = new WXMenus();
            menu.button = new List<SubButton>();
            menu.button.Add(new SubButton
            {
                name = "扫码",
                sub_button = new List<object> {
                    new SubButton { type = "scancode_waitmsg", name = "扫码带提示", key = "rselfmenu_0_0" },
                    new SubButton { type = "scancode_push", name = "扫码推事件", key = "rselfmenu_0_1" }
                }
            });
            menu.button.Add(new SubButton
            {
                name = "发图",
                sub_button = new List<object> {
                    new SubButton { type = "pic_sysphoto", name = "系统拍照发图", key = "rselfmenu_1_0" },
                    new SubButton { type = "pic_photo_or_album", name = "拍照或者相册发图", key = "rselfmenu_1_1" },
                    new SubButton { type = "pic_weixin", name = "微信相册发图", key = "rselfmenu_1_2" }
                }
            });
            menu.button.Add(new SubButton
            {
                name = "菜单",
                sub_button = new List<object> {
                    new SubButton { type = "view", name = "搜索", url = "http://www.soso.com/" },
                    new SubButton { type = "click", name = "赞一下我们", key = "V1001_GOOD" },
                    new SubButton { type = "location_select", name = "发送位置", key = "rselfmenu_2_0" }
                }
            });
            this.MenuCreate(menu);
        }
    }
}