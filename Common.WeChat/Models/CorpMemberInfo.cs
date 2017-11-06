using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChat.Models
{
    /// <summary>企业号成员信息</summary>
    public class CorpMemberInfo
    {
        /// <summary>返回码</summary>
        public int errcode { get; set; }
        /// <summary>对返回码的文本描述内容</summary>
        public string errmsg { get; set; }

        /// <summary>如果userid由系统自动生成，则仅允许修改一次，新值可由new_userid字段指定</summary>
        public string new_userid { get; set; }
        /// <summary>成员UserID，对应管理端的帐号，企业内必须唯一。不区分大小写，长度为1~64个字节</summary>
        public string userid { get; set; }
        /// <summary>成员姓名，长度为1~64个字节</summary>
        public string name { get; set; }
        /// <summary>英文名，长度为1-64个字节</summary>
        public string english_name { get; set; }
        /// <summary>性别。0表示未定义，1表示男性，2表示女性</summary>
        public int gender { get; set; }
        /// <summary>头像url。注：如果要获取小图将url最后的”/0”改成”/100”即可</summary>
        public string avatar { get; set; }
        /// <summary>成员头像的mediaid，通过多媒体接口上传图片获得的mediaid</summary>
        public string avatar_mediaid { get; set; }
        /// <summary>成员所属部门</summary>
        public int[] department { get; set; }
        /// <summary>部门内的排序值，默认为0。数量必须和department一致，数值越大排序越前面。值范围是[0, 2^32)</summary>
        public int[] order { get; set; }
        /// <summary>职位信息</summary>
        public string position { get; set; }
        /// <summary>成员手机号，仅在用户同意snsapi_privateinfo授权时返回</summary>
        public string mobile { get; set; }
        /// <summary>座机。长度0-64个字节</summary>
        public string telephone { get; set; }
        /// <summary>成员邮箱，企业内必须唯一，mobile/email二者不能同时为空;仅在用户同意snsapi_privateinfo授权时返回</summary>
        public string email { get; set; }
        /// <summary>上级字段，标识是否为上级</summary>
        public int isleader { get; set; }
        /// <summary>启用/禁用成员。1表示启用成员，0表示禁用成员</summary>
        public int enable { get; set; }
        /// <summary>激活状态: 1=已激活，2=已禁用，4=未激活。已激活代表已激活企业微信或已关注微信插件。未激活代表既未激活企业微信又未关注微信插件</summary>
        public int status { get; set; }
        /// <summary>自定义字段。自定义字段需要先在WEB管理端“我的企业” — “通讯录管理”添加，否则忽略未知属性的赋值</summary>
        public string extattr { get; set; }

    }
}
