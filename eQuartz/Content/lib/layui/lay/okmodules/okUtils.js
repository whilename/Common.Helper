layui.define(["jquery"], function (exprots) {
    var $ = layui.jquery;
    var okUtils = {
        getBodyWidth: function () {
            return document.body.scrollWidth;//body的总宽度
        }
    };
    exprots("okUtils", okUtils);
});

