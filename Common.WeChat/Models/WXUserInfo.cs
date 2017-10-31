using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.Models
{
    /// <summary>微信用户信息</summary>
    public class WXUserInfo
    {
        /// <summary>用户的唯一标识</summary>
        public string openid { get; set; }
        /// <summary>用户昵称</summary>
        public string nickname { get; set; }
        /// <summary>用户的性别，值为1时是男性，值为2时是女性，值为0时是未知</summary>
        public int sex { get; set; }
        /// <summary>用户个人资料填写的省份</summary>
        public string province { get; set; }
        /// <summary>普通用户个人资料填写的城市</summary>
        public string city { get; set; }
        /// <summary>国家，如中国为CN</summary>
        public string country { get; set; }
        /// <summary>用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。</summary>
        public string headimgurl { get; set; }
        /// <summary>用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）</summary>
        public object privilege { get; set; }
        /// <summary>只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。详见：获取用户个人信息（UnionID机制）</summary>
        public string unionid { get; set; }

        /// <summary>用户的语言，简体中文为zh_CN</summary>
        public string language { get; set; }
        /// <summary>用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。</summary>
        public int subscribe { get; set; }
        /// <summary>用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间</summary>
        public long subscribe_time { get; set; }
        /// <summary>公众号运营者对粉丝的备注</summary>
        public string remark { get; set; }
        /// <summary>用户所在的分组ID</summary>
        public int groupid { get; set; }
        /// <summary>用户被打上的标签ID列表</summary>
        public List<int> tagid_list { get; set; }

    }

    /// <summary>企业号成员信息</summary>
    public class WXMemberInfo
    {
        /// <summary>成员UserID</summary>
        public string userid { get; set; }
        /// <summary>成员姓名</summary>
        public string name { get; set; }
        /// <summary>成员所属部门</summary>
        public string department { get; set; }
        /// <summary>职位信息</summary>
        public string position { get; set; }
        /// <summary>成员手机号，仅在用户同意snsapi_privateinfo授权时返回</summary>
        public string mobile { get; set; }
        /// <summary>性别。0表示未定义，1表示男性，2表示女性</summary>
        public int gender { get; set; }
        /// <summary>成员邮箱，仅在用户同意snsapi_privateinfo授权时返回</summary>
        public string email { get; set; }
        /// <summary>头像url。注：如果要获取小图将url最后的”/0”改成”/100”即可</summary>
        public string avatar { get; set; }

    }

}