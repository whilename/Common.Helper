/*!
 * 调用示例
 * $(function (e) {
 *      // 分享完成回调
 *      try {
 *          weixin.onsuccess(function (message) {
 *              $.post("URL", { name1:value1, name2:value2 },
 *                  function (data) {
 *                      layer.open({ content: JSON.parse(data).Msg, time: 2, style: "color:red;" });
 *                      console.log(data);
 *              });
 *          });
 *          // 注册JS
 *          weixin.ready(function () {
 *              weixin.init({
 *                  title: "分享标题",
 *                  desc: "分享描述",
 *                  link: "http://weixin.duileme.cn/WeiXin/Authorize",
 *                  imgUrl: "分享显示图标URL"
 *              });
 *          });
 *      } catch (e) { Common.log(e); }
 *  });
 *
 */
var weixin = (function () {
    $.post("/WeiXin/JSSignature", { url: window.location.href }, function (data) {
        wx.config({
            debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
            appId: data.appid, // 必填，公众号的唯一标识
            timestamp: data.timestamp, // 必填，生成签名的时间戳
            nonceStr: data.noncestr, // 必填，生成签名的随机串
            signature: data.signature, // 必填，签名，见附录1
            jsApiList: [ // 必填，需要使用的JS接口列表
				"checkJsApi",
				"onMenuShareTimeline",
				"onMenuShareAppMessage",
				"onMenuShareQQ",
				"onMenuShareWeibo",
				"hideMenuItems",
				"showMenuItems",
				"hideAllNonBaseMenuItem",
				"showAllNonBaseMenuItem",
				"getLocation"
            ]
        });
    }, "json");
    wx.ready(function () {
        // 页面加载时需要执行的事件
    });
    function init(data) {
        // 分享至朋友圈
        wx.onMenuShareTimeline({
            title: data.title, // 分享标题
            link: data.link,  // 分享链接，该链接域名或路径必须与当前页面对应的公众号JS安全域名一致
            imgUrl: data.imgUrl, // 分享图标
            success: function () { // 用户确认分享后执行的回调函数
                success("onMenuShareTimeline");
            },
            cancel: function () { // 用户取消分享后执行的回调函数
            }
        });
        // 分享至好友
        wx.onMenuShareAppMessage({
            title: data.title,
            desc: data.desc,
            link: data.link,
            imgUrl: data.imgUrl,
            type: data.type, // 分享类型,music、video或link，不填默认为link
            dataUrl: data.dataUrl,  // 如果type是music或video，则要提供数据链接，默认为空
            success: function () {
                success("onMenuShareAppMessage");
            },
            cancel: function () {
            }
        });
        // 分享至QQ
        wx.onMenuShareQQ({
            title: data.title,
            desc: data.desc,
            link: data.link,
            imgUrl: data.imgUrl,
            success: function () {
                success("onMenuShareQQ");
            },
            cancel: function () {
            }
        });
        // 分享至微博
        wx.onMenuShareWeibo({
            title: data.title,
            desc: data.desc,
            link: data.link,
            imgUrl: data.imgUrl,
            success: function () {
                success("onMenuShareWeibo");
            },
            cancel: function () {
            }
        });
    }
    function hideMenuItems(data) {
        wx.hideMenuItems({
            menuList: data,
            success: function (res) {
            },
            fail: function (res) {
            }
        });
    }
    function getLocation(callBack) {
        wx.getLocation({
            success: function (res) {
                callBack({
                    longitude: res.longitude,
                    latitude: res.latitude
                });
            },
            cancel: function (res) {
                callBack();
            }
        });
    }
    function WXPay(data) {
        wx.chooseWXPay({
            timestamp: data.timestamp, // 支付签名时间戳，注意微信jssdk中的所有使用timestamp字段均为小写。但最新版的支付后台生成签名使用的timeStamp字段名需大写其中的S字符
            nonceStr: data.nonceStr, // 支付签名随机串，不长于 32 位
            package: data.package, // 统一支付接口返回的prepay_id参数值，提交格式如：prepay_id=***）
            signType: data.signType, // 签名方式，默认为'SHA1'，使用新版支付需传入'MD5'
            paySign: data.paySign, // 支付签名
            success: function (res) { // 支付成功后的回调函数                
            }
        });
    }
    function success(message) {
    }
    return {
        ready: function (ready) {
            wx.ready(ready);
        },
        init: init,
        hideMenuItems: hideMenuItems,
        getLocation: getLocation,
        onsuccess: function (onsuccess) {
            success = onsuccess;
        }
    }
}(weixin || {}));