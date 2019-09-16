function getprod(cuno) {
    var prodname = $("#prodparme").val();
    $.ajax({
        type: "post",
        url: "/Ajax/GetProd",
        data: { pname_no: prodname,cu_no:cuno },
        dataType: "json",
        success: function (data) {
            var prod = ""
            for (var i in data) {
                prod += '<tr><td><label><input type="radio" name="xtcunor" id="xtcunor" value="' + data[i].p_no + '|' + data[i].pname + '|' + data[i].type + '|' + data[i].bm_no + '|' + data[i].unit + '" />选择</label></td>';
                prod += '<td>' + data[i].p_no + '</td>';
                prod += '<td>' + data[i].pname + '</td>';
                prod += '<td>' + data[i].type + '</td>';
                prod += '<td>' + data[i].unit + '</td></tr>';
            }
            console.log(data);
            if (data.length<=0) {
                prod += "<tr><td colspan='5' style='text-align:center'>暂无审核通过的产品</td></tr>";
            }
            $("#showprod").empty();
            $("#showprod").append(prod);
        }, error: function (data, status, e) {

        },
    });
}

function getcus() {
    var cusparme = $("#cusparme").val();
    $.ajax({
        type: "post",
        url: "/Ajax/GetCustomer",
        data: { parme: cusparme , pagesize:500 },
        dataType: "json",
        success: function (data) {
            var prod = ""
            for (var i in data) {
                prod += '<tr><td><label><input type="radio" name="cus" id="cus" value="' + data[i].cu_no + '|' + data[i].cu_name + '" />选择</label></td>';
                prod += '<td>' + data[i].cu_no + '</td>';
                prod += '<td>' + data[i].cu_name + '</td>';
                prod += '<td>' + data[i].province + '</td>';
                prod += '<td>' + data[i].city + '</td></tr>';
            }
            $("#showcus").empty();
            $("#showcus").append(prod);
        }, error: function (data, status, e) {

        },
    });
}


function getmd(cu_no) {
    var mdparme = $("#mdparme").val();
    $.ajax({
        type: "post",
        url: "/Ajax/GetCusStore",
        data: { parme: mdparme, pagesize: 500 ,cuno: cu_no},
        dataType: "json",
        success: function (data) {
            var prod = ""
            for (var i in data) {
                prod += '<tr><td><label><input type="radio" name="md" id="md" value="' + data[i].dian_no + '|' + data[i].dian_name + '" />选择</label></td>';
                prod += '<td>' + data[i].dian_no + '</td>';
                prod += '<td>' + data[i].dian_name + '</td>';
                prod += '<td>' + data[i].province + '</td>';
                prod += '<td>' + data[i].city + '</td>';
                prod += '<td>' + data[i].addr + '</td></tr>';
            }
            $("#showmd").empty();
            $("#showmd").append(prod);
        }, error: function (data, status, e) {

        },
    });
}

//获取批次信息
function getinvlot(cuno) {
    var prodname = $("#invlotparme").val();
    $.ajax({
        type: "post",
        url: "/Ajax/GetInvLot",
        data: { pname_no: prodname, cu_no: cuno },
        dataType: "json",
        success: function (data) {
            var prod = ""
            for (var i in data) {
                prod += '<tr><td><label><input type="radio" name="invlot" id="invlot" value="' + data[i].lot_no + '" />选择</label></td>';
                prod += '<td>' + data[i].lot_no + '</td>';
                prod += '<td>' + data[i].p_no + '</td>';
                prod += '<td>' + data[i].pname + '</td>';
                prod += '<td>' + data[i].makeare + '</td></tr>';
            }
            
            if (data.length <= 0) {
                prod += "<tr><td colspan='5' style='text-align:center'>暂无批号信息</td></tr>";
            }
            $("#showinvlot").empty();
            $("#showinvlot").append(prod);
        }, error: function (data, status, e) {

        },
    });
}

//获取工序信息
function getprocess(cuno) {
    var prodname = $("#processparme").val();
    $.ajax({
        type: "post",
        url: "/Ajax/GetProcess",
        data: { pname_no: prodname, cu_no: cuno },
        dataType: "json",
        success: function (data) {
            var prod = ""
            for (var i in data) {
                prod += '<tr><td><label><input type="radio" name="process" id="process" value="' + data[i].pc_no + '|' + data[i].pc_name + '" />选择</label></td>';
                prod += '<td>' + data[i].pc_no + '</td>';
                prod += '<td>' + data[i].pc_name + '</td>';                
                prod += '<td>' + data[i].remark + '</td></tr>';
            }

            if (data.length <= 0) {
                prod += "<tr><td colspan='5' style='text-align:center'>暂无工序信息</td></tr>";
            }
            $("#showprocess").empty();
            $("#showprocess").append(prod);
        }, error: function (data, status, e) {

        },
    });
}

//获取原料信息
function getmpart(cuno) {
    var prodname = $("#mpartparme").val();
    $.ajax({
        type: "post",
        url: "/Ajax/GetMpart",
        data: { pname_no: prodname, cu_no: cuno },
        dataType: "json",
        success: function (data) {
            var prod = ""
            for (var i in data) {
                prod += '<tr><td><label><input type="radio" name="mpart" id="mpart" value="' + data[i].p_no + '|' + data[i].pname + '" />选择</label></td>';
                prod += '<td>' + data[i].p_no + '</td>';
                prod += '<td>' + data[i].pname + '</td>';
                prod += '<td>' + data[i].type + '</td>';
                prod += '<td>' + data[i].unit + '</td></tr>';
            }

            if (data.length <= 0) {
                prod += "<tr><td colspan='5' style='text-align:center'>暂无原料信息</td></tr>";
            }
            $("#showmpart").empty();
            $("#showmpart").append(prod);
        }, error: function (data, status, e) {

        },
    });
}