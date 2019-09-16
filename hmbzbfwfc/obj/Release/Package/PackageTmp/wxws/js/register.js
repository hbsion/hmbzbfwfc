

//验证上传
function doEdit() {


    var cu_no = document.getElementById("txtcu_no").value;
    if (cu_no.length == 0) { alert("请输入登录手机号码"); return false; }

    if (cu_no.length <6  ) { alert("请输入大于6位的手机号码"); return false; }

    
    if ($("#txtname").val() == "") { alert("请输入姓名"); return false; }
  //  if ($("#txtsfz").val() == "") { alert("请输入身份证号码"); return false; }

  //  if ($("#txtwx").val() == "") { alert("请输入微信号"); return false; }
  //  if ($("#txtphone").val() == "") { alert("请输入手机号"); return false; }

    if ($("#addr").val() == "") { alert("请输入收货地址"); return false; }

 

 register();

    return false;
}
function register() {
    var cu_no = $("#txtcu_no").val();
    var xtcu_no = $("#txtxtcu_no").val();
    
    var cu_name = $("#txtname").val();//姓名
    var wxid ="";//微信
    var mysfz ="";//身份证
    var addr = $("#addr").val();//地址
    var province = $("#province option:selected").text();

    var city = $("#City option:selected").text();
    var password = $("#txtpwd").val();
    var remark = "";
    var mcode = ""
    var phone = cu_no; //手机号
    
    var taoid = "";
    var shitidian = "";
    var salloca = "";
    //头像
    var icon="";
    var zfb = "";
    var khh = "";
    var yhzh = "";
    var qq = "";
 
     
    layer.msg("资料提交中...", { time: 2000, shade: 0.6 });
    $.ajax({
        type: "GET",
        url: "../App/AppNewcu.ashx",
        dataType: "text",
        async: false,
        data: { "type": "1", "icon": icon, "cu_no": cu_no, "xtcu_no": xtcu_no, "cu_name": cu_name, "wxid": wxid, "mysfz": mysfz, "addr": addr, "province": province, "city": city, "password": password, "remark": remark, "mcode": mcode, "phone": phone, "taoid": taoid, "shitidian": shitidian, "salloca": salloca, "zfb": zfb, "khh": khh, "yhzh": yhzh, "qq": qq },
        success: function(data) {
            layer.closeAll();
            var strsnn = new Array();
            strsnn = data.split("|");
            if (strsnn.length < 2) {
                layer.msg('网络不通或网络故障');
                return;
            }

            if (strsnn[1] == "OK") {


                layer.msg('网络不通或网络故障');
                 window.location.href = "wxCustomerBrandList.aspx";
            
  
 
            }
            else {
                layer.msg(strsnn[1]);

            }
        },
        error: function(xhr, error) {
        layer.closeAll();
        }
    });
}
