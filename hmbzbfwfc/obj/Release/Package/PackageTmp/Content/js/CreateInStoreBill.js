//创建入库单
function addprod() {
    $("#submit").css("display", "none");
    $("#proinput").css("display", "");
}



function getprod() {
    var prodname = $("#prodparme").val();
    $.ajax({
        type: "post",
        url: "/Ajax/GetProd",
        data: { pname_no : prodname },
        dataType: "json",
        success: function (data) {
            var prod=""
            for (var i in data) {                
                prod += '<tr><td><label><input type="radio" name="xtcunor" id="xtcunor" value="' + data[i].p_no + '|' + data[i].pname + '|' + data[i].type + '|' + data[i].bm_no + '|' + data[i].price + '" />选择</label></td>';
                prod += '<td>' + data[i].p_no + '</td>';
                prod += '<td>' + data[i].pname + '</td>';
                prod += '<td>' + data[i].type + '</td>';
                prod += '<td>' + data[i].price + '</td></tr>';
            }
            $("#showprod").empty();
            $("#showprod").append(prod);
        }, error: function (data, status, e) {

        },
    });
}

function Setname(lt, pno, pname, linkman, linktype) {
    var data = $(lt);
    $("#sup").hide();
    $("#supname").val(data.text());
    $("#supno").val(pno);
    $("#suplinkman").val(linkman);
    $("#supphone").val(linktype);
  
}

$(function () {
    $("#supno").keyup(function () {

        $("#sup").hide();
        $("#sup").show();
        $("#sup").empty();
        $.post("/Ajax/GetSupplier", { parme: $("#supno").val() }, function (e) {
            var str = "";          
            if (e != "") {
                $.each(e, function (index, lt) {
                    str += "<tr><td><label><input name='uid' type='radio' id='uid' value='" + lt.SupNum + "'><span  onclick='Setname(this,\"" + lt.SupNum + "\",\"" + lt.SupName + "\",\"" + lt.ContactName + "\",\"" + lt.Phone + "\")'>" + lt.SupName + "</span></label></td></tr>";
                })
            } else {
                str += "<tr><td>未查询到供应商信息</td></tr>";
            }
            $("#sup").append(str);
        })
    })
})

function createbill() {    
    var submit = function (v) {
        var orderno = $("#orderno").val();
        var ordertime = $("#ordertime").val();
        var intype = $("#intype").val();
        var cuser = $("#cuser").val();
        var corder = $("#corder").val();
        var supno = $("#supno").val();
        var supname = $("#supname").val();
        var suplinkman = $("#suplinkman").val();
        var supphone = $("#supphone").val();
        var remark = $("#remark").val();
        var fid = $("#fid").val();
        if (v == 'ok') {
            $.ajax({
                type: "post",
                url: "/ERP/CreateInStoreBill",
                data: {
                    OrderNum: orderno, OrderTime: ordertime, InType: intype, CreateUser: cuser, ContractOrder: corder,
                    SupNum: supno,SupName:supname,ContactName:suplinkman,Phone:supphone,Remark:remark,fid:fid
                },
                dataType: "json",
                success: function (data) {
                    if (data["status"] == "success") {
                        $.jBox.tip("入库单编辑成功", "success");
                        setTimeout(function () { window.location.href = "/ERP/InStoreBillList"; }, 1000);
                    } else {
                        $.jBox.tip(data["data"], "error");
                    }
                }, error: function (data, status, e) {
                    $.jBox.tip("error:操作失败", "error");
                },
            });
        }
    };
    $.jBox.confirm("确定提交入库单吗？", "提示", submit);
}

function cancelprod() {
    $("#submit").css("display", "block");
    $("#proinput").css("display", "none");
}
//日期
$(function () {
    $('.date').each(function () {
        $(this).ionDatePicker({
            lang: 'zh-cn',
            format: 'YYYY-MM-DD hh:mm:ss'
        });
    });


    $.validator.setDefaults({
        submitHandler: function () {

        }
    });
    $().ready(function () {
        $("#fgiForm").validate();
    });

    $.validator.setDefaults({
        submitHandler: function () {
            var pno = $("#cpno").val();
            var pname = $("#prod").val();
            var bmno = $("#bmno").val();
            var type = $("#type").val();
            var aprice = $("#aprice").val();
            var sprice = $("#sprice").val();
            var num = $("#fginum").val();
            var pc = $("#pc").val();
            var orderno = $("#orderno").val();
            var store = $("#kb option:selected").val();
            var  kb= $("#kb option:selected").text();
            $.ajax({
                type: "post",
                url: "/ERP/SaveInStoreProd",
                data: {p_no:pno,pname:pname,barcode:bmno,ptype:type,inprice:aprice,sumprice:sprice,num:num,billno:orderno,batchnum:pc,storenum:store},
                dataType: "json",
                success: function (data) {
                    
                    var result = "";
                    if (data["status"] == "success") {                        
                        result += '<tr id="pro'+data["fid"]+'"><td>' + pname + '</td><td>' + bmno + '</td><td>' + type + '</td><td>' + pc + '</td><td>' + kb + '</td>';
                        result += '<td>' + aprice + '</td><td>' + num + '</td><td>' + sprice + '</td><td><a><i class="fa fa-close" title="删除" onclick="delpro(' + data["fid"] + ');"></i></a></td></tr>';
                        $("#prodata").prepend(result);
                        $("#submit").css("display", "block");
                        document.getElementById("AddProd").reset();
                        $("#proinput").css("display", "none");

                    } else {
                        $.jBox.tip("添加失败", "warn");
                    }
                }, error: function (data, status, e) {
                    
                },
            });
        }
    });
    $().ready(function () {
        $("#AddProd").validate();
    });

});



function delpro(fid)
{    
    var submit = function (v) {
        if (v == 'ok') {
            $.ajax({
                type: "post",
                url: "/ERP/DelInStoreProd",
                data: { fid: fid },
                dataType: "json",
                success: function (data) {
                    if (data["status"] == "success") {
                        $.jBox.tip("删除成功", "success");
                        $("tr[id=pro"+fid+"]").remove();
                    } else {
                        $.jBox.tip("删除失败", "error");
                    }
                }, error: function (data, status, e) {
                    $.jBox.tip("删除失败", "error");
                },
            });
        }
    };
    $.jBox.confirm("确定要删除吗？", "提示", submit);
}

function resetbill() {
    var submit = function (v) {
        if (v == 'ok') {
            $.ajax({
                type: "post",
                url: "/ERP/DelInStoreProd",
                data: { orderno: $("#orderno").val() },
                dataType: "json",
                success: function (data) {
                    $.jBox.tip("重置成功", "success");                                           
                    setTimeout(function () { window.location.href = "/ERP/EditInStoreBill"; }, 1000);
                }, error: function (data, status, e) {
                    $.jBox.tip("重置成功", "success");
                    setTimeout(function () { window.location.href = "/ERP/EditInStoreBill"; }, 1000);
                },
            });
        }
    };
    $.jBox.confirm("确定要重置吗？", "提示", submit);
}

$(function () {
    $("#fginum,#aprice").keyup(function () {
        var aprice = $("#aprice").val();
        var fginum = $("#fginum").val();
        var sprice = parseFloat(aprice) * parseInt(fginum);
        if (isNaN(sprice)) {
            $.jBox.tip("单价和数量必须为数字", "warn");
            $("#sprice").val(0);
        } else {
            $("#sprice").val(sprice);
        }

    })
})

function setcunor() {
    var xtcuno = $("input[name='xtcunor']:checked").val();
    var a = xtcuno.split('|');
    $("#cpno").val(a[0]);
    $("#prod").val(a[1]);
    $("#type").val(a[2]);
    $("#bmno").val(a[3]);
    $("#aprice").val(a[4]);

}

function submitfgidata() {

    $.ajax({
        type: "post",
        url: "/LogisticsManage/SalFgiScan",
        data: {},
        dataType: "json",
        success: function (data) {
            $("#result").prepend(data["data"]);
            if (data["status"] == true) {
                $("#js").val(data["cpsl"]);
                $("#cpsl").val(data["cpsl"]);
            }
        }, error: function (data, status, e) {

        },
    });
}

