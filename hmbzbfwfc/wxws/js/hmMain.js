function pullUpAction() {
    setTimeout(function() {
        load(parseInt($("#PageIndex").val()) + 1);
    }, 500);
}
function pullDownAction() {
    setTimeout(function() {
        load('1');
    }, 500);
}
function pullWinH() {
    var headH = $("#heads").height();
    var fotH = "1";
    var winH = $(window).height();
    if (headH > 0 && fotH > 0 && winH > 0) {
        var HH = winH - headH - fotH;
        if (HH > 0) { HH = HH + "px"; } else { HH = "500px" }

        document.getElementById("wrapper").style.height = HH;
        document.getElementById("resText").style.minHeight = HH;
    }
    else {
        document.getElementById("wrapper").style.height = "500px";
        document.getElementById("resText").style.minHeight = "500px";
    }
}
function onSearch() {
    $("#mybg").show();
    $("#query").show();
}
function hide() {
    $("#mybg").hide();
    $("#query").hide();
}
//初始化时间
function getNowFormatDateF() {
    var mydate = new Date();
    mydate.setMonth(mydate.getMonth() - 1);
    var y = mydate.getFullYear();
    var M = mydate.getMonth() + 1;
    if (M < 10) M = '0' + M;
    var D = mydate.getDate();
    if (D < 10) D = '0' + D;
    $("#dayF").val(y + '-' + M + '-' + D);
}
function getNowFormatDateT() {
    var mydate = new Date();
    var y = mydate.getFullYear();
    var M = mydate.getMonth() + 1;
    if (M < 10) M = '0' + M;
    var D = mydate.getDate();
    if (D < 10) D = '0' + D;
    $("#dayT").val(y + '-' + M + '-' + D);
}