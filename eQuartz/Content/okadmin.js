/^http(s*):\/\//.test(location.href) || alert('请先部署到 localhost 下再访问');
layui.use(['element', 'layer', 'okUtils', 'okTab'], function () {
    var element = layui.element, okUtils = layui.okUtils, $ = layui.jquery, layer = layui.layer,
        okTab = layui.okTab({ url: "./Content/data/navs.json", openTabNum: 30, /*允许同时选项卡的个数*/ });

    //左侧导航渲染完成之后的操作
    okTab.render(function () { });//渲染左侧导航

    // 添加新窗口
    $("body").on("click", "#navBar .layui-nav-item a,#userInfo a", function () {
        //如果不存在子级
        if ($(this).siblings().length == 0) { okTab.tabAdd($(this)); }
        $(this).parent("li").siblings().removeClass("layui-nav-itemed");//关闭其他的二级标签
    });

    /**
     * 左边菜单显隐功能
     * @type {boolean}
     */
    $(".ok-menu").click(function () {
        $(".layui-layout-admin").toggleClass("ok-left-hide");
        $(this).find('i').toggleClass("ok-menu-hide");
    });

    //移动端的处理事件Start
    $("body").on("click", ".layui-layout-admin .ok-left a[data-url],.ok-make", function () {
        if ($(".layui-layout-admin").hasClass("ok-left-hide")) {
            $(".layui-layout-admin").removeClass("ok-left-hide");
            $(".ok-menu").find('i').removeClass("ok-menu-hide");
        }
    });
    //移动端的处理事件End

    //tab左右移动
    $("body").on("click", ".okNavMove", function () {
        var moveId = $(this).attr("data-id");
        var that = this;
        okTab.navMove(moveId, that);
        // console.log(width);
    });

    //刷新当前tab页
    $("body").on("click", ".ok-refresh", function () { okTab.refresh(this); });

    //关闭tab页
    $("body").on("click", "#tabAction a", function () { var num = $(this).attr('data-num'); okTab.tabClose(num); });

    //全屏/退出全屏
    $("body").on("keydown", function (event) {
        event = event || window.event || arguments.callee.caller.arguments[0];
        if (event && event.keyCode == 27) { // 按 Esc
            $("#fullScreen").children("i").eq(0).removeClass("okicon-screen-restore");
        }
        if (event && event.keyCode == 122) { // 按 F11
            $("#fullScreen").children("i").eq(0).addClass("okicon-screen-restore");
        }
    });
    $("body").on("click", "#fullScreen", function () {
        if ($(this).children("i").hasClass("okicon-screen-restore")) {
            screenFun(2).then(() => { $(this).children("i").eq(0).removeClass("okicon-screen-restore"); });
        } else {
            screenFun(1).then(() => { $(this).children("i").eq(0).addClass("okicon-screen-restore"); });
        }
    });

    /**
     * 全屏和退出全屏的方法
     * @param num
     * num为1代表全屏
     * num为2代表退出全屏
     */
    function screenFun(num = 1) {
        num = num * 1;
        var docElm = document.documentElement;

        switch (num) {
            case 1:
                if (docElm.requestFullscreen) {
                    docElm.requestFullscreen();
                } else if (docElm.mozRequestFullScreen) {
                    docElm.mozRequestFullScreen();
                } else if (docElm.webkitRequestFullScreen) {
                    docElm.webkitRequestFullScreen();
                } else if (docElm.msRequestFullscreen) {
                    docElm.msRequestFullscreen();
                }
                break;
            case 2:
                if (document.exitFullscreen) {
                    document.exitFullscreen();
                } else if (document.mozCancelFullScreen) {
                    document.mozCancelFullScreen();
                } else if (document.webkitCancelFullScreen) {
                    document.webkitCancelFullScreen();
                } else if (document.msExitFullscreen) {
                    document.msExitFullscreen();
                }
                break;
        }

        return new Promise((res, rej) => { res("返回值"); });
    }

    /**
     * 系统公告
     
    $(document).on("click", "#notice", noticeFun);
    !function () {
        let notice = sessionStorage.getItem("notice");
        if (notice != "true") { noticeFun(); }
    }();

    function noticeFun() {
        var srcWidth = okUtils.getBodyWidth();
        layer.open({
            type: 0,
            title: "系统公告",
            btn: "我知道啦",
            btnAlign: 'c',
            content: "ok-admin v2.0上线啦(^し^)<br />" +
                "在此郑重承诺该模板<span style='color:#5cb85c'>永久免费</span>为大家提供" +
                "<br />若有更好的建议欢迎<span id='noticeQQ'>加入QQ群</span>一起聊",
            yes: (index) => {
                if (srcWidth > 800) {
                    layer.tips('公告跑到这里去啦', '#notice', {
                        tips: [1, '#000'],
                        time: 2000
                    });
                }
                sessionStorage.setItem("notice", "true");
                layer.close(index);
            },
            cancel: (index) => {
                if (srcWidth > 800) {
                    layer.tips('公告跑到这里去啦', '#notice', {
                        tips: [1, '#000'],
                        time: 2000
                    });
                }
            }
        });
    }*/
    
    /**
     * 退出操作
     */
    $("#logout").click(function () {
        layer.confirm("确定要退出吗？", { skin: 'layui-layer-lan', icon: 3, title: '提示', anim: 6 }, function () {
            window.location = "./pages/login.html";
        });
    });

    /**
     * 锁定账户
     */
    $("#lock").click(function () {
        layer.confirm("确定要锁定账户吗？", { skin: 'layui-layer-lan', icon: 4, title: '提示', anim: 1 }, function (index) {
            layer.close(index);
            $(".yy").show();
            layer.prompt({
                btn: ['确定'],
                title: '输入密码解锁(123456)',
                closeBtn: 0,
                formType: 1
            }, function (value, index, elem) {
                if (value == "123456") {
                    layer.close(index);
                    $(".yy").hide();
                } else {
                    layer.msg('密码错误', { anim: 6 });
                }
            });
        });
    });
    
});
