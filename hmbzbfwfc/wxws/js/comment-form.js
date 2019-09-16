window.onload = function () {
    //初始化菜单栏
    //  var inner = "<div class=\"head_right\">";
    var inner = "";
    inner+="<ol>";
    inner+="<li><a href=\"index.aspx\"><img src=\"static/css/default/home/images/head_right_01.png\" /></a></li>";
 inner+="<li><a href=\"login.aspx\">";
 inner+="<img src=\"static/css/default/home/images/head_right_02.png\" /></a>";
 inner += "</li>";
 inner+="<li><a class=\"J_top_nav_block\" href=\"javascript:;\"><img src=\"static/css/default/home/images/head_right_03.png\" /></a>";
 inner+="</li>";
 inner+="<!-- 菜单 -->";
 inner+="<div class=\"who_right J_top_menu\" style=\" display:none;\">";
 inner+="<div class=\"who_right_top\">";
 inner+="<dl class=\"hover\">";
 inner+="<dt>";
 inner+="<span class=\"who_right_top_left\"><a href=\"index.aspx\"><img src=\"static/css/default/home/images/index_right1.png\" />首 页</a></span>";
 inner+="<span class=\"who_right_top_right\"><img src=\"static/css/default/home/images/sanjiao_1.png\" /></span>";
 inner+="</dt>";
 inner+="<dd></dd>";
 inner+="</dl>";
 inner+="<dl>";
 inner+="<dt>";
 inner+="<span class=\"who_right_top_left\"><a href=\"notice.aspx\"><img src=\"static/css/default/home/images/index_right2.png\" />官方活动</a></span>";
 inner+="<span class=\"who_right_top_right\"><img src=\"static/css/default/home/images/sanjiao_1.png\" /></span>";
 inner+="</dt>";
 inner+="<dd></dd>";
 inner+="</dl>";
 inner+="<dl>";
 inner+="<dt>";
 inner+="<span class=\"who_right_top_left\"><a href=\"javascript:;\"><img src=\"static/css/default/home/images/index_right3.png\" />代理商专区</a></span>";
 inner+="<span class=\"who_right_top_right\"><img src=\"static/css/default/home/images/sanjiao_3.png\" /></span>";
 inner+="</dt><dd><span><a href=\"register.aspx\">· 代理申请</a></span>";
 inner+="<span><a href=\"login.aspx\" >· 代理商登录</a></span>";
 inner+="<span><a href=\"query.aspx\">· 代理商查询</a></span>";
 inner += "<span><a href=\"policy.aspx\">· 代理商政策</a></span>";
 inner += "<span><a href=\"platform.aspx\">· 代理商工作台</a></span>";
 inner+="</dd>";
 inner+="</dl>";
 inner+="<dl>";
 inner+="<dt>";
 inner+="<span class=\"who_right_top_left\"><a href=\"javascript:;\"><img src=\"static/css/default/home/images/index_right5.png\" />关于-xxx</a></span>";
 inner+="<span class=\"who_right_top_right\"><img src=\"static/css/default/home/images/sanjiao_3.png\" /></span>";
 inner+="</dt>";
 inner+="<dd>";
 inner+="<span><a href=\"feedback.htm\" >· 意见反馈</a></span>";
 inner+="<span><a href=\"about.htm\">· 关于我们</a></span>";
 inner+="<span><a href=\"connection.htm\">· 联系我们 </a></span>";
 inner+="</dd>";
 inner+="<dt>";
 inner+="<span class=\"who_right_top_left\" style=\"padding-left:65px;\"><a href=\"javascript:;\" class=\"J_top_nav_none\" style=\"color:#66c300;\">关闭菜单</a></span>";
 inner+="</dt>";
 inner+="</dl>";
 inner+="</div>";
 inner+="</div>";
 inner+="</ol>";
// inner += "</div>";
 $("#head_right").html(inner);
    //弹出菜单*展开
    $('.J_top_nav_block').click(function () {
      
        $('.J_top_menu').fadeIn();
        $('.J_top_nav_block').attr('class', 'J_top_nav_none');
    });
    //弹出菜单*关闭
    $('.J_top_nav_none').click(function () {
        $('.J_top_menu').fadeOut();
        $('.J_top_nav_none').attr('class', 'J_top_nav_block');
    });
   
    var headH = $("#head").height();
    var fotH = $("#fot").height();
    var winH = $(window).height();
    if (headH > 0 && fotH > 0 && winH > 0) {
        var HH = winH - headH - fotH;
        if (HH > 0) { HH = HH + "px"; } else { HH = "100%" }
        var ee = "<iframe src=\"../shopapp/index.aspx\"  scrolling=\"no\" frameborder=\"0\" height=\"" + HH + "\" width=\"100%\" marginheight=\"0\" marginwidth=\"0\"></iframe>";
        document.getElementById("aa").innerHTML = ee;
    }
    else {
        window.location.href = "../shopapp/index.aspx";
    }
}