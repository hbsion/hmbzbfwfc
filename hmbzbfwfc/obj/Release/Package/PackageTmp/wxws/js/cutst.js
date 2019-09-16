//判断是否这空
function isEmpty(elArr) {
    if (elArr.length > 0) {
        for (var i = 0; i < elArr.length; i++) {
            var val = $(elArr[i]).val();
            if (val == "" || val == null) {
                layer.tips("带*号的是必填项", elArr[i], { tips: 1, icon: 5 });
                $(elArr[i]).focus();
            }
        }
    }
}
function isInput(el, val) {
    if (val == null || val.length == 0 || val == "") {
        layer.tips("带*号的是必填项", el, { tips: 1, icon: 5 });
        $(el).focus();
        return;
    }
}

function wxError() {
    wx.error(function(res) {
        alert(res);
        // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，也可以在返回的res参数中查看，对于SPA可以在这里更新签名。

    });
}
function getCookieVal(str, key) {
    var userData = [];
    userData = str.split('&'); //
    if (userData.length <= 0)
        return null;
    for (var i = 0; i < userData.length; i++) {
        var cookies = userData[i].split('=');
        if (cookies[0] == key)
            return cookies[1];
    }
    return null;
}

//自动生成订单
function autoBillno() {
    //日期部分
    var date = new Date();
    var yyyy = date.getFullYear();
    var mm = date.getMonth() + 1; //特殊,从0开始
    var dd = date.getDate();
    //时间部分
    var HH = date.getHours();
    var MM = date.getMinutes();
    var ss = date.getSeconds();
    return "T" + yyyy + isTen(mm) + isTen(dd) + isTen(HH) + isTen(MM) + isTen(ss);
}
//时间补足两个字符判断
function isTen(num) {
    if (num < 10)
        return '0' + num;
    return num;
}


function onSelect(title, content, w, h) {//打开弹出层
    layer.open({
        skin: 'layui-layer-lan',
        type: 2,
        title: title,
        shadeClose: true,
        shade: 0.8,
        area: [w, h],
        content: content
    });
}