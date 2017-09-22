using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Models
{
    /// <summary>微信模板消息对象</summary>
    public class WXMsgTemplate
    {
        /// <summary>接收者openid</summary>
        public string touser { get; set; }
        /// <summary>模板ID</summary>
        public string template_id { get; set; }
        /// <summary>模板跳转链接</summary>
        public string url { get; set; }
        /// <summary>模板数据</summary>
        public object data { get; set; }

    }
    /// <summary>消息内容</summary>
    public class KeyValue
    {
        /// <summary>消息内容</summary>
        public string value { get; set; }
        /// <summary>颜色值(十六进制)</summary>
        public string color { get; set; }
    }

    /// <summary>模板1</summary>
    public class Template
    {
        /// <summary></summary>
        public KeyValue first { get; set; }
        /// <summary></summary>
        public KeyValue keyword1 { get; set; }
        /// <summary></summary>
        public KeyValue keyword2 { get; set; }
        /// <summary></summary>
        public KeyValue remark { get; set; }
    }

    /// <summary>微信客服消息</summary>
    public class WXMsgCustom
    {
        /// <summary>普通用户openid</summary>
        public string touser { get; set; }
        /// <summary>消息类型，文本为text，图片为image，语音为voice，视频消息为video，音乐消息为music，图文消息（点击跳转到外链）为news，图文消息（点击跳转到图文消息页面）为mpnews，卡券为wxcard，小程序为miniprogrampage</summary>
        public string msgtype { get; set; }

        /// <summary>发送的文本消息</summary>
        public MsgText text { get; set; }
        /// <summary>客服帐号</summary>
        public CustomService customservice { get; set; }
        /// <summary>发送的图片消息</summary>
        public MsgMedia image { get; set; }
        /// <summary>发送的语音消息</summary>
        public MsgMedia voice { get; set; }
        /// <summary>发送的视频消息</summary>
        public MsgVideo video { get; set; }
        /// <summary>发送的音乐消息</summary>
        public MsgMusic music { get; set; }
        /// <summary>发送图文消息（点击跳转到外链） 图文消息条数限制在8条以内，注意，如果图文数超过8，则将会无响应</summary>
        public MsgNews news { get; set; }
        /// <summary>发送图文消息（点击跳转到图文消息页面） 图文消息条数限制在8条以内，注意，如果图文数超过8，则将会无响应</summary>
        public MsgMedia mpnews { get; set; }
        /// <summary>发送卡券</summary>
        public MsgCard wxcard { get; set; }
        /// <summary>发送小程序卡片</summary>
        public MiniProgramPage miniprogrampage { get; set; }

    }

    /// <summary>文本消息类型</summary>
    public class MsgText
    {
        /// <summary>文本消息内容</summary>
        public string content { get; set; }
    }

    /// <summary>客服</summary>
    public class CustomService
    {
        /// <summary>客服帐号</summary>
        public string kf_account { get; set; }
    }

    /// <summary>图片/语音/视频/图文消息（点击跳转到图文消息页）的媒体ID</summary>
    public class MsgMedia
    {
        /// <summary>发送的图片/语音/视频/图文消息（点击跳转到图文消息页）的媒体ID</summary>
        public string media_id { get; set; }
    }

    /// <summary>卡券ID</summary>
    public class MsgCard
    {
        /// <summary>卡券ID</summary>
        public string card_id { get; set; }
    }

    /// <summary>视频消息</summary>
    public class MsgVideo : MsgMedia
    {
        /// <summary>缩略图</summary>
        public string thumb_media_id { get; set; }
        /// <summary>视频消息/音乐消息的标题</summary>
        public string title { get; set; }
        /// <summary>视频消息/音乐消息的描述</summary>
        public string description { get; set; }
    }

    /// <summary>音乐消息</summary>
    public class MsgMusic : MsgVideo
    {
        /// <summary>音乐链接</summary>
        public string musicurl { get; set; }
        /// <summary>高品质音乐链接，wifi环境优先使用该链接播放音乐</summary>
        public string hqmusicurl { get; set; }
    }

    /// <summary>图文消息</summary>
    public class MsgNews
    {
        /// <summary>图文消息列表</summary>
        public List<MsgArticles> articles { get; set; }
    }
    /// <summary>图文消息内容</summary>
    public class MsgArticles
    {
        /// <summary>图文消息的标题</summary>
        public string title { get; set; }
        /// <summary>图文消息的描述</summary>
        public string description { get; set; }
        /// <summary>图文消息被点击后跳转的链接</summary>
        public string url { get; set; }
        /// <summary>图文消息的图片链接，支持JPG、PNG格式，较好的效果为大图640*320，小图80*80</summary>
        public string picurl { get; set; }
    }

    /// <summary>小程序卡片</summary>
    public class MiniProgramPage
    {
        /// <summary>小程序卡片的标题</summary>
        public string title { get; set; }
        /// <summary>小程序的appid</summary>
        public string appid { get; set; }
        /// <summary>小程序的页面路径，跟app.json对齐，支持参数，比如pages/index/index?foo=bar</summary>
        public string pagepath { get; set; }
        /// <summary>缩略图/小程序卡片图片的媒体ID，小程序卡片图片建议大小为520*416</summary>
        public string thumb_media_id { get; set; }
    }

}