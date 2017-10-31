using WeChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;

namespace WeChat
{
    /// <summary>微信支付</summary>
    public class WXPayment : BaseCommon
    {
        /// <summary>预支付交易订单请求</summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WXPayUnifiedorderResponse Unifiedorder(WXPayUnifiedorder model)
        {
            string data = model.ToXml();
            // POST请求生成微信支付预订单
            string result = WebHttp.SendRequest(PAY_UNIFIEDORDER_URL, data, "post");
            // 记录请求与回传信息
            Log.Info("Request WeChat Unifiedorder：{0} \n Post：{1} \n Response：{2}", PAY_UNIFIEDORDER_URL, data, result);
            WXPayUnifiedorderResponse unifiedorder = new WXPayUnifiedorderResponse();
            unifiedorder.Reload(result);
            return unifiedorder;
        }

    }
}
