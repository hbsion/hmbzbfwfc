var imgurl = "http://" + window.location.host + "/ServiceImage/";
var errorpage = "http://" + window.location.host + '/Wap/Sys/error.html?errcode=403';

var public_gps = '';
var public_area = '';
var public_address = ''
var getParam = function (name) {
    var search = document.location.search;
    var pattern = new RegExp("[?&]" + name + "\=([^&]+)", "g");
    var matcher = pattern.exec(search);
    var items = null;
    if (null != matcher) {
        try {
            items = decodeURIComponent(decodeURIComponent(matcher[1]));
        } catch (e) {
            try {
                items = decodeURIComponent(matcher[1]);
            } catch (e) {
                items = matcher[1];
            }
        }
    }
    return items;
};
function isNotNull(obj) {
    if (obj != null && obj != "" && obj != "undefined" && obj != "null") {
        return true;
    }
    return false;
}

function GetGSPAddress() {
    if (public_gps.length <= 0) {
        public_gps = '';
        public_area = '';
        public_address = '';
    }

    $.ajax({
        type: 'get',
        url: 'http://apis.map.qq.com/ws/geocoder/v1',
        dataType: 'jsonp',
        data: {
            key: "RWUBZ-POR3U-DMPVL-4AQCJ-Z26D2-3MF3Y",
            location: public_gps,
            get_poi: "0",
            coord_type: "1",
            output: "jsonp"
        },
        success: function (data, textStatus) {
            if (data.status == 0) {
                public_area = data.result.address_component.province + ',' + data.result.address_component.city;
                public_address = data.result.formatted_addresses.recommend;

            } else {
                public_area = '';
                public_address = '';
            }
        },
        error: function () {
            public_area = '';
            public_address = '';
        }
    });
}

function ToJson(str) {
    var o = eval("(" + str + ")");
    return o;
}





/**加载滚动图片**/
function LoadSliderImage(appid) {
    var getappid = localStorage.getItem("appid");
    var sliderjson = JSON.parse(localStorage.getItem("slider"));
    if (appid != getappid) {
        sliderjson = '';
    }
    if(!isNotNull(sliderjson) || !isNotNull(getappid) ||  getappid != appid){
        localStorage.setItem("appid", appid);
        $.ajax({
            type: "GET",
            async: false,
            cache: false,
            url: "../../../Ajax/WapData.ashx?type=LoadImage&appid=" + appid + "&image=Slider",
            success: function (data) {
                if (data.length <= 0) {
                    $('#pdm_slider_index').append(imgurl + 'Slider/Default.jpg');
                    return false;
                }
                sliderjson = data;
                localStorage.setItem("slider", JSON.stringify(data));
            }
        });
    }
    var $slider = $('#pdm_slider_index');
    $slider.flexslider({
        pauseOnAction: false
    });
    $slider.flexslider('removeSlide', 0);
    $.each(JSON.parse(sliderjson), function (idx, obj) {
        $slider.flexslider('addSlide', '<li><img src="'  +imgurl + "Slider/" + appid + "/" + obj.ImageName +'" /></li>');
    });
}
/**加载企业信息**/
function LoadCompanyInfo(appid) {
    $.ajax({
        type: "GET",
        async: false,
        cache: false,
        dataType: "json",
        url: "../../../Ajax/WapData.ashx?type=LoadCompanyInfo&appid=" + appid,
        success: function (data) {
            localStorage.setItem("about", data.AboutUs);
            localStorage.setItem("contact", data.ContactUs);
        },
        error: function () {
            localStorage.setItem("about", '');
            localStorage.setItem("contact", '');
            return false;
        }
    });
}

