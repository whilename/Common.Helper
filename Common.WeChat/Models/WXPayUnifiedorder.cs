using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChat.Models
{
    /// <summary>预支付交易单</summary>
    public class WXPayUnifiedorder : WXBaseModel
    {
        /// <summary></summary>
        public WXPayUnifiedorder() { properties = new Hashtable(); }

        /// <summary>签名类型(非必要),签名类型，默认为MD5，支持HMAC-SHA256和MD5</summary>
        public string sign_type { get { return GetVal("sign_type"); } set { SetVal("sign_type", value); } }
        /// <summary>商品描述,商品简单描述，该字段请按照规范传递，具体请见参数规定</summary>
        public string body { get { return GetVal("body"); } set { SetVal("body", value); } }
        /// <summary>商品详情(非必要),单品优惠字段(暂未上线)</summary>
        public string detail { get { return GetVal("detail"); } set { SetVal("detail", value); } }
        /// <summary>附加数据(非必要),附加数据，在查询API和支付通知中原样返回，可作为自定义参数使用</summary>
        public string attach { get { return GetVal("attach"); } set { SetVal("attach", value); } }
        /// <summary>商户订单号,商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|*@ ，且在同一个商户号下唯一。详见商户订单号</summary>
        public string out_trade_no { get { return GetVal("out_trade_no"); } set { SetVal("out_trade_no", value); } }
        /// <summary>标价币种(非必要),符合ISO 4217标准的三位字母代码，默认人民币：CNY，详细列表请参见货币类型</summary>
        public string fee_type { get { return GetVal("fee_type"); } set { SetVal("fee_type", value); } }
        /// <summary>标价金额,订单总金额，单位为分，详见支付金额</summary>
        public int total_fee { get { return properties["total_fee"] == null ? 0 : (int)properties["total_fee"]; } set { SetVal("total_fee", value); } }
        /// <summary>终端IP,APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP</summary>
        public string spbill_create_ip { get { return GetVal("spbill_create_ip"); } set { SetVal("spbill_create_ip", value); } }
        /// <summary>交易起始时间(非必要),订单生成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010</summary>
        public string time_start { get { return GetVal("time_start"); } set { SetVal("time_start", value); } }
        /// <summary>交易结束时间(非必要),订单失效时间，格式为yyyyMMddHHmmss，如2009年12月27日9点10分10秒表示为20091227091010;注意：最短失效时间间隔必须大于5分钟</summary>
        public string time_expire { get { return GetVal("time_expire"); } set { SetVal("time_expire", value); } }
        /// <summary>订单优惠标记(非必要),订单优惠标记，使用代金券或立减优惠功能时需要的参数，说明详见代金券或立减优惠</summary>
        public string goods_tag { get { return GetVal("goods_tag"); } set { SetVal("goods_tag", value); } }
        /// <summary>通知地址,异步接收微信支付结果通知的回调地址，通知url必须为外网可访问的url，不能携带参数</summary>
        public string notify_url { get { return GetVal("notify_url"); } set { SetVal("notify_url", value); } }
        /// <summary>交易类型,取值如下：JSAPI，NATIVE，APP等</summary>
        public string trade_type { get { return GetVal("trade_type"); } set { SetVal("trade_type", value); } }
        /// <summary>商品ID,trade_type=NATIVE时（即扫码支付），此参数必传。此参数为二维码中包含的商品ID，商户自行定义</summary>
        public string product_id { get { return GetVal("product_id"); } set { SetVal("product_id", value); } }
        /// <summary>指定支付方式(非必要),上传此参数no_credit--可限制用户不能使用信用卡支付</summary>
        public string limit_pay { get { return GetVal("limit_pay"); } set { SetVal("limit_pay", value); } }
        /// <summary>用户标识,trade_type=JSAPI时（即公众号支付），此参数必传，此参数为微信用户在商户对应appid下的唯一标识。</summary>
        public string openid { get { return GetVal("openid"); } set { SetVal("openid", value); } }

    }
}
