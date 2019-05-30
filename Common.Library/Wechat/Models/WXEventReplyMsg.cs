using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wechat.Models
{
    /// <summary>消息事件回复内容</summary>
    public class WXEventReplyMsg
    {
        /// <summary>接收方帐号（收到的OpenID）</summary>
        public string ToUserName { get; set; }
        /// <summary>开发者微信号</summary>
        public string FromUserName { get; set; }
        /// <summary>消息创建时间</summary>
        public long CreateTime { get; set; }
        /// <summary>消息类型 text、image、voice、video、music、news</summary>
        public string MsgType { get; set; }

        /// <summary>text 回复的消息内容（换行：在content中能够换行，微信客户端就支持换行显示）</summary>
        public string Content { get; set; }

        #region news 图文消息

        /// <summary>图文消息个数，限制为8条以内</summary>
        public int ArticleCount { get { return Articles.Count; } }
        /// <summary>多条图文消息信息，默认第一个item为大图,注意，如果图文数超过8，则将会无响应</summary>
        public List<MsgItem> Articles { get; set; }

        #endregion

        #region voice、video 语音/视频消息

        /// <summary>通过素材管理中的接口上传多媒体文件，得到的id </summary>
        public string MediaId { get; set; }
        /// <summary>视频/音乐消息的标题(可为空)</summary>
        public string Title { get; set; }
        /// <summary>视频/音乐消息的描述(可为空)</summary>
        public string Description { get; set; }

        #endregion

        #region voice、video 语音/视频消息
        /// <summary>音乐链接</summary>
        public string MusicUrl { get; set; }
        /// <summary>高质量音乐链接，WIFI环境优先使用该链接播放音乐</summary>
        public string HQMusicUrl { get; set; }
        /// <summary>缩略图的媒体id，通过素材管理中的接口上传多媒体文件，得到的id</summary>
        public string ThumbMediaId { get; set; }

        #endregion

        /// <summary>序列化自身为XML格式字符串</summary>
        /// <returns></returns>
        public string ToXmlSerialize()
        {
            StringBuilder str_item = new StringBuilder();// 消息内容
            switch (this.MsgType)
            {
                case "image":// 回复图片消息
                    str_item.AppendFormat("<Image>\r\n<MediaId><![CDATA[{0}]]></MediaId>\r\n</Image>", this.MediaId);
                    break;
                case "voice":// 回复语音消息
                    str_item.AppendFormat("<Voice>\r\n<MediaId><![CDATA[{0}]]></MediaId>\r\n</Voice>", this.MediaId);
                    break;
                case "video":// 回复视频消息
                    str_item.AppendFormat("<Video>\r\n<MediaId><![CDATA[{0}]]></MediaId>\r\n<Title><![CDATA[{1}]]></Title>\r\n<Description><![CDATA[{2}]]></Description>\r\n</Video>", this.MediaId, this.Title, this.Description);
                    break;
                case "music":// 回复音乐消息
                    str_item.AppendFormat("<Music>\r\n<Title><![CDATA[{0}]]></Title>\r\n<Description><![CDATA[{1}]]></Description>\r\n<MusicUrl><![CDATA[{2}]]></MusicUrl>\r\n<HQMusicUrl><![CDATA[{3}]]></HQMusicUrl>\r\n<ThumbMediaId><![CDATA[{4}]]></ThumbMediaId>\r\n</Music>", this.Title, this.Description, this.MusicUrl, this.HQMusicUrl, this.ThumbMediaId);
                    break;
                case "news":// 回复图文消息
                    str_item.AppendFormat("<ArticleCount>{0}</ArticleCount>\r\n<Articles>\r\n{1}\r\n</Articles>", this.ArticleCount, this.MsgNews());
                    break;
                case "text":// 回复文本消息
                default:
                    str_item.AppendFormat("<Content><![CDATA[{0}]]></Content>", this.Content);
                    break;
            }
            // 消息主体格式
            string xml = string.Format("<xml>\r\n<ToUserName><![CDATA[{0}]]></ToUserName>\r\n<FromUserName><![CDATA[{1}]]></FromUserName>\r\n<CreateTime>{2}</CreateTime>\r\n<MsgType><![CDATA[{3}]]></MsgType>\r\n{4}\r\n</xml>", this.ToUserName, this.FromUserName, this.CreateTime, this.MsgType, str_item);
            return xml;
        }

        /// <summary>图文消息</summary>
        /// <returns>图文消息集合</returns>
        private string MsgNews()
        {
            StringBuilder str_item = new StringBuilder();
            foreach (var item in Articles)
            {
                str_item.AppendFormat("<item>\r\n<Title><![CDATA[{0}]]></Title> \r\n<Description><![CDATA[{1}]]></Description>\r\n<PicUrl><![CDATA[{2}]]></PicUrl>\r\n<Url><![CDATA[{3}]]></Url>\r\n</item>", item.Title, item.Description, item.PicUrl, item.Url);
            }
            return str_item.ToString();
        }
    }


    /// <summary>图文消息明细项</summary>
    public class MsgItem
    {
        /// <summary>图文消息标题</summary>
        public string Title { get; set; }
        /// <summary>图文消息描述</summary>
        public string Description { get; set; }
        /// <summary>图片链接，支持JPG、PNG格式，较好的效果为大图360*200，小图200*200</summary>
        public string PicUrl { get; set; }
        /// <summary>点击图文消息跳转链接</summary>
        public string Url { get; set; }

    }
}
