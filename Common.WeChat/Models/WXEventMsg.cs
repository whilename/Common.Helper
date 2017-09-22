using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WeChat.Models
{
    /// <summary>接收消息事件推送模型</summary>
    [XmlRootAttribute]
    public class WXEventMsg
    {
        /// <summary>开发者微信号</summary>
        [XmlElement]
        public string ToUserName { get; set; }
        /// <summary>发送方帐号（一个OpenID）</summary>
        [XmlElement]
        public string FromUserName { get; set; }
        /// <summary>消息创建时间</summary>
        [XmlElement]
        public long CreateTime { get; set; }
        /// <summary>消息类型</summary>
        [XmlElement]
        public string MsgType { get; set; }

        #region 接收普通消息

        /// <summary>文本消息内容</summary>
        [XmlElement]
        public string Content { get; set; }
        /// <summary>图片链接（由系统生成）</summary>
        [XmlElement]
        public string PicUrl { get; set; }
        /// <summary>图片、语音、视频消息媒体id，可以调用多媒体文件下载接口拉取数据</summary>
        [XmlElement]
        public string MediaId { get; set; }
        /// <summary>语音格式，如amr，speex等</summary>
        [XmlElement]
        public string Format { get; set; }
        /// <summary>语音识别结果，UTF8编码</summary>
        [XmlElement]
        public string Recognition { get; set; }
        /// <summary>视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据</summary>
        [XmlElement]
        public string ThumbMediaId { get; set; }
        /// <summary>地理位置维度</summary>
        [XmlElement]
        public string Location_X { get; set; }
        /// <summary>地理位置经度</summary>
        [XmlElement]
        public string Location_Y { get; set; }
        /// <summary>地图缩放大小</summary>
        [XmlElement]
        public string Scale { get; set; }
        /// <summary>地理位置信息</summary>
        [XmlElement]
        public string Label { get; set; }
        /// <summary>消息标题</summary>
        [XmlElement]
        public string Title { get; set; }
        /// <summary>消息描述</summary>
        [XmlElement]
        public string Description { get; set; }
        /// <summary>消息链接</summary>
        [XmlElement]
        public string Url { get; set; }

        /// <summary>消息id，64位整型</summary>
        [XmlElement]
        public string MsgId { get; set; }

        #endregion

        #region 接收事件推送

        /// <summary>事件类型，subscribe(订阅)、unsubscribe(取消订阅)、LOCATION(上报地理位置)、CLICK(点击菜单拉取消息时的事件)、VIEW(点击菜单跳转链接时的事件)</summary>
        [XmlElement]
        public string Event { get; set; }
        /// <summary>事件KEY值，qrscene_为前缀，后面为二维码的参数值</summary>
        [XmlElement]
        public string EventKey { get; set; }
        /// <summary>二维码的ticket，可用来换取二维码图片</summary>
        [XmlElement]
        public string Ticket { get; set; }
        /// <summary>地理位置纬度</summary>
        [XmlElement]
        public string Latitude { get; set; }
        /// <summary>地理位置经度</summary>
        [XmlElement]
        public string Longitude { get; set; }
        /// <summary>地理位置精度</summary>
        [XmlElement]
        public string Precision { get; set; }

        #endregion

    }
}
