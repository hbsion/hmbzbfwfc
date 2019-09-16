
function genRandNumber(startNum, endNum) {
    var randomNumber;
    randomNumber = Math.round(Math.random() * (endNum - startNum)) + startNum;
    return randomNumber;
}

var UNPOST_KEY = '_J_P_';
var GET_KEY_PREFIX = '_J_';

/**
 * 缓存从服务端获取的数据
 * @param key
 * @param value
 */
var _save = function (key, value) {
    var data = {
        data: value,
        cacheTime: new Date()
    }
    window.localStorage.setItem(GET_KEY_PREFIX + key, JSON.stringify(data));
}
/**
 * 获取本地已缓存的数据
 */
var _get = function (key) {
    return JSON.parse(window.localStorage.getItem(GET_KEY_PREFIX + key));
}

/**
 * 删除本地已缓存的数据
 */
var _clear = function (key) {
    return  window.localStorage.removeItem(GET_KEY_PREFIX + key);
}

/**
 * 清空本地缓存
 */
var clear = function () {
    var storage = window.localStorage;
    for (var key in storage) {
        if (key.indexOf(GET_KEY_PREFIX) == 0) {
            storage.removeItem(key);
        }
    }
    storage.removeItem(UNPOST_KEY);
}

var TipLoad = {};

TipLoad.loading = function (text) {
    var tip = text ? text : '加载中...';

    $('#jingle_popup_mask').show();
    $('#jingle_popup').show();
    $('#jingle_popup p').text(tip);

}
TipLoad.close = function () {
    $('#jingle_popup_mask').hide();
    $('#jingle_popup').hide();
}
/* 

//取参数
*/ 
function GetQueryString(name, url) {

    if (url && url.indexOf('?') != -1) {
        var args = new Object(); //声明一个空对象 

        //获取URL中全部参数列表数据  
        var query = "&" + url.split("?")[1];

        if (query.indexOf('#') != -1) {
            query = query.split("#")[0];
        }

        var pairs = query.split("&"); // 以 & 符分开成数组 
        for (var i = 0; i < pairs.length; i++) {
            var pos = pairs[i].indexOf('='); // 查找 "name=value" 对 
            if (pos == -1) continue; // 若不成对，则跳出循环继续下一对 
            var argname = pairs[i].substring(0, pos); // 取参数名 
            var value = pairs[i].substring(pos + 1); // 取参数值 
            value = decodeURIComponent(value); // 若需要，则解码 
            args[argname] = value; // 存成对象的一个属性 
        }


        return args[name];

    } else {
        return null;
    }
}

/*  */
//截取字符串 包含中文处理 
//(串,长度,增加...) 
function subString(str, len, hasDot) {
    var newLength = 0;
    var newStr = "";
    var chineseRegex = /[^\x00-\xff]/g;
    var singleChar = "";
    var strLength = str.replace(chineseRegex, "**").length;
    for (var i = 0; i < strLength; i++) {
        singleChar = str.charAt(i).toString();
        if (singleChar.match(chineseRegex) != null) {
            newLength += 2;
        }
        else {
            newLength++;
        }
        if (newLength > len) {
            break;
        }
        newStr += singleChar;
    }

    if (hasDot && strLength > len) {
        newStr += "...";
    }
    return newStr;
}
 

////
//$(function () {
//    //自动设置高度
//    $('html').height($(window).height());
//    $('#index_section').height($(window).height());
//    $(window).resize(function () {
//        $('html').height($(window).height());
//        $('#index_section').height($(window).height());
//    });

    
//});
 

var  settings = {
   
    //page默认动画效果
    transitionType : 'slide',
    //自定义动画时的默认动画时间(非page转场动画时间)
    transitionTime : 250,
    //自定义动画时的默认动画函数(非page转场动画函数)
    transitionTimingFunc : 'ease-in',
    //toast 持续时间,默认为3s
    toastDuration : 3000  
}
/*
* alias func
* 简化一些常用方法的写法
** /
/**
* 完善zepto的动画函数,让参数变为可选
*/