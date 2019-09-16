//公共的操作

var Public = {
    SelectAll: function (item) {
        var flag = $("input[name=selall]").is(':checked')

        if (flag == true) {
            $("input[name='user_item']").prop("checked", true);
        }
        else {
            $("input[name='user_item']").prop("checked", false);
        }
    },
    Delete: function (ProductCode,requesturl,backurl) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["fid"] = ProductCode;
                $.gitAjax({
                    url:  requesturl, type: "post", data: param, success: function (result) {
                        $.jBox.tip(result, "success");
                        setTimeout(function () { window.location.href = backurl; }, 1000);
                    }, error: function (result) {
                        $.jBox.tip("删除失败", "error");
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);
    },
    BatchDel: function (requesturl,backurl) {
        var chklist = $("#tabInfo tbody tr").find("input:checked");
        var ids = "";
        $.each(chklist, function (index, item) {
            ids += $(item).attr("data") + ",";
        });
        if (ids.length > 0) {
            var submit = function (v, h, f) {
                if (v == 'ok') {
                    var param = {};
                    param["idlist"] = ids;
                    $.gitAjax({
                        url: requesturl , type: "post", data: param, success: function (result) {
                            $.jBox.tip(result, "success");
                            setTimeout(function () { window.location.href = backurl; }, 1000);
                        }, error: function (result) {
                            $.jBox.tip("删除失败", "error");
                        }
                    });
                }
            };
            $.jBox.confirm("确定要删除吗？", "提示", submit);
        }
        else {
            $.jBox.tip("请至少选择一条数据!", 'info');
        }
    },
    Add: function () {
        var backurl = $("#commentForm").attr("backurl");
        var targetUrl = $("#commentForm").attr("action");
        var data = $("#commentForm").serialize();
        
        $.ajax({
            url: targetUrl,
            data: data,
            type: 'post',
            dataType: 'json',
            success: function (data) {
                if (data["status"] == "success") {
                    $.jBox.tip(data["data"], "success");
                    setTimeout(function () { window.location.href = backurl; }, 1000);
                } else {
                    $.jBox.tip(data["data"], "error");
                    $("#error").empty();
                    $("#error").append(data["data"]);
                }
            },
            error: function () {
                $.jBox.tip("erroe:操作失败", "error");
            }
        })
    }

};

//库别管理
var Store = {
    SelectAll: function (item) {
        var flag = $("input[name=selall]").is(':checked')

        if (flag == true) {
            $("input[name='user_item']").prop("checked", true);
        }
        else {
            $("input[name='user_item']").prop("checked", false);
        }
    },
    Delete: function (ProductCode) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["fid"] = ProductCode;
                $.gitAjax({
                    url: "StoreDel", type: "post", data: param, success: function (result) {
                        $.jBox.tip(result, "success");
                        location.href = "StoreList"
                    }, error: function (result) {
                        $.jBox.tip("删除失败", "error");
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);
    },
    BatchDel: function () {
        var chklist = $("#tabInfo tbody tr").find("input:checked");
        var ids = "";
        $.each(chklist, function (index, item) {
            ids += $(item).attr("data") + ",";
        });
        if (ids.length > 0) {
            var submit = function (v, h, f) {
                if (v == 'ok') {
                    var param = {};
                    param["idlist"] = ids;
                    $.gitAjax({
                        url: "StoreDel", type: "post", data: param, success: function (result) {
                            $.jBox.tip(result, "success");
                            location.href = "StoreList"
                        }, error: function (result) {
                            $.jBox.tip("删除失败", "error");
                        }
                    });
                }
            };
            $.jBox.confirm("确定要删除吗？", "提示", submit);
        }
        else {
            $.jBox.tip("请至少选择一条数据!", 'info');
        }
    },
    Add: function () {
            var targetUrl = $("#commentForm").attr("action");
            var data = $("#commentForm").serialize();
            $.ajax({
                url: targetUrl,
                data: data,
                type: 'post',
                dataType: 'json',
                success: function (data) {
                    if (data["status"] == "success") {
                        $.jBox.tip(data["data"], "success");
                        setTimeout(function () { window.location.href = "/BasicInfo/Store/StoreList"; }, 1000);
                    } else {
                        $.jBox.tip(data["data"], "error");
                        $("#error").empty();
                        $("#error").append(data["data"]);
                    }
                },
                error: function () {
                    $.jBox.tip("erroe:操作失败", "error");
                }
            })
        }

};


//用户的删除 查找
var User = {
    SelectAll: function (item) {
        var flag = $("input[name=selall]").is(':checked')

        if (flag == true) {
            $("input[name='user_item']").prop("checked", true);
        }
        else {
            $("input[name='user_item']").prop("checked", false);
        }
    },
    Delete: function (userCode) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["fid"] = userCode;
                $.gitAjax({
                    url: "deleteuser", type: "post", data: param, success: function (result) {
                        $.jBox.tip(result, "success");
                        location.href = "UserList?pageno=1"
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);
    },
    BatchDel: function () {
        var chklist = $("#tabInfo tbody tr").find("input:checked");
        var ids = "";
        $.each(chklist, function (index, item) {
            ids += $(item).attr("data") + ",";
        });
        if (ids.length > 0) {
            var submit = function (v, h, f) {
                if (v == 'ok') {
                    var param = {};
                    param["idlist"] = ids;
                    $.gitAjax({
                        url: "deleteuser", type: "post", data: param, success: function (result) {
                            $.jBox.tip(result, "success");
                            location.href = "UserList?pageno=1"
                        }
                    });
                }
            };
            $.jBox.confirm("确定要删除吗？", "提示", submit);
        }
        else {
            $.jBox.tip("请至少选择一条数据!", 'info');
        }
    }, SearchEvent: function () {
        $("#btnHSearch").click(function () {
            var flag = $("#divHSearch").css("display");
            if (flag == "none") {
                $("#txtSearch").val("");
                $("#divHSearch").slideDown("slow");
            } else {
                $("#divHSearch").slideUp("slow");
            }
        });
    }
};


//删除产品
var Product = {
    SelectAll: function (item) {
        var flag = $("input[name=selall]").is(':checked')

        if (flag == true) {
            $("input[name='user_item']").prop("checked", true);
        }
        else {
            $("input[name='user_item']").prop("checked", false);
        }
    },
    Delete: function (ProductCode) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["fid"] = ProductCode;
                $.gitAjax({
                    url: "ProductDel", type: "post", data: param, success: function (result) {
                        $.jBox.tip(result, "success");
                        location.href = "ProductList"
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);
    },
    BatchDel: function () {
        var chklist = $("#tabInfo tbody tr").find("input:checked");
        var ids = "";
        $.each(chklist, function (index, item) {
            ids += $(item).attr("data") + ",";
        });
        if (ids.length > 0) {
            var submit = function (v, h, f) {
                if (v == 'ok') {
                    var param = {};
                    param["idlist"] = ids;
                    $.gitAjax({
                        url: "ProductDel", type: "post", data: param, success: function (result) {
                            $.jBox.tip(result, "success");
                            location.href = "ProductList"
                        }
                    });
                }
            };
            $.jBox.confirm("确定要删除吗？", "提示", submit);
        }
        else {
            $.jBox.tip("请至少选择一条数据!", 'info');
        }
    }
};


//经销商
var Customer = {
    SelectAll: function (item) {
        var flag = $("input[name=selall]").is(':checked')

        if (flag == true) {
            $("input[name='user_item']").prop("checked", true);
        }
        else {
            $("input[name='user_item']").prop("checked", false);
        }
    },
    Delete: function (ProductCode) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["fid"] = ProductCode;
                $.gitAjax({
                    url: "CustomerDel", type: "post", data: param, success: function (result) {
                        $.jBox.tip(result, "success");
                        location.href = "CustomerList"
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);
    },
    BatchDel: function () {
        var chklist = $("#tabInfo tbody tr").find("input:checked");
        var ids = "";
        $.each(chklist, function (index, item) {
            ids += $(item).attr("data") + ",";
        });
        if (ids.length > 0) {
            var submit = function (v, h, f) {
                if (v == 'ok') {
                    var param = {};
                    param["idlist"] = ids;
                    $.gitAjax({
                        url: "CustomerDel", type: "post", data: param, success: function (result) {
                            $.jBox.tip(result, "success");
                            location.href = "CustomerList"
                        }
                    });
                }
            };
            $.jBox.confirm("确定要删除吗？", "提示", submit);
        }
        else {
            $.jBox.tip("请至少选择一条数据!", 'info');
        }
    }, Add: function () {
        var targetUrl = $("#commentForm").attr("action");
        var data = $("#commentForm").serialize();
        $.ajax({
            url: targetUrl,
            data: data,
            type: 'post',
            dataType: 'json',
            success: function (data) {
                if (data["status"] == "success") {
                    $.jBox.tip(data["data"], "success");
                    setTimeout(function () { window.location.href = "/BasicInfo/Customer/CustomerList"; }, 1000);
                } else {
                    $.jBox.tip(data["data"], "error");
                    $("#error").empty();
                    $("#error").append(data["data"]);
                }
            },
            error: function () {
                $.jBox.tip("erroe:操作失败", "error");
            }
        })
    }
};


//菜单
var Menu = {
    SelectAll: function (item) {
        var flag = $("input[name=selall]").is(':checked')

        if (flag == true) {
            $("input[name='user_item']").prop("checked", true);
        }
        else {
            $("input[name='user_item']").prop("checked", false);
        }
    },
    Delete: function (ProductCode) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["fid"] = ProductCode;
                $.gitAjax({
                    url: "MenuDel", type: "post", data: param, success: function (result) {
                        $.jBox.tip(result, "success");
                        setTimeout(function () { window.location.href = "/SystemManage/MenuManage/MenuList"; }, 1000);
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？该操作将会删除当前和下级的菜单", "提示", submit);
    },
    BatchDel: function () {
        var chklist = $("#tabInfo tbody tr").find("input:checked");
        var ids = "";
        $.each(chklist, function (index, item) {
            ids += $(item).attr("data") + ",";
        });
        if (ids.length > 0) {
            var submit = function (v, h, f) {
                if (v == 'ok') {
                    var param = {};
                    param["idlist"] = ids;
                    $.gitAjax({
                        url: "MenuDel", type: "post", data: param, success: function (result) {
                            $.jBox.tip(result, "success");
                            setTimeout(function () { window.location.href = "/SystemManage/MenuManage/MenuList"; }, 1000);
                        }
                    });
                }
            };
            $.jBox.confirm("确定要删除吗？该操作将会删除当前和下级的菜单", "提示", submit);
        }
        else {
            $.jBox.tip("请至少选择一条数据!", 'info');
        }
    }, Add: function () {
        var targetUrl = $("#commentForm").attr("action");
        var data = $("#commentForm").serialize();
        $.ajax({
            url: targetUrl,
            data: data,
            type: 'post',
            dataType: 'json',
            success: function (data) {
                if (data["status"] == "success") {
                    $.jBox.tip(data["data"], "success");
                    setTimeout(function () { window.location.href = "/SystemManage/MenuManage/MenuList"; }, 1000);
                } else {
                    $.jBox.tip(data["data"], "error");
                    $("#error").empty();
                    $("#error").append(data["data"]);
                }
            },
            error: function () {
                $.jBox.tip("erroe:操作失败", "error");
            }
        })
    }
};

//入库单列表
var Bill = {
    SelectAll: function (item) {
        var flag = $("input[name=selall]").is(':checked')

        if (flag == true) {
            $("input[name='user_item']").prop("checked", true);
        }
        else {
            $("input[name='user_item']").prop("checked", false);
        }
    },
    Delete: function (fid) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["fid"] = fid;
                $.gitAjax({
                    url: "/ERP/InStoreBillDel", type: "post", data: param, success: function (result) {
                        $.jBox.tip(result, "success");
                        $.jBox.tip("删除成功", "error");
                        setTimeout(function () { window.location.href = "/ERP/CreateInStoreBill"; }, 1000);
                    }, error: function (result) {
                        $.jBox.tip("删除失败", "error");
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);
    },
    BatchDel: function () {
        var chklist = $("#tabInfo tbody tr").find("input:checked");
        var ids = "";
        $.each(chklist, function (index, item) {
            ids += $(item).attr("data") + ",";
        });
        if (ids.length > 0) {
            var submit = function (v, h, f) {
                if (v == 'ok') {
                    var param = {};
                    param["idlist"] = ids;
                    $.gitAjax({
                        url: "/ERP/InStoreBillDel", type: "post", data: param, success: function (result) {
                            $.jBox.tip(result, "success");
                            location.href = "InStoreBillList"
                        }, error: function (result) {
                            $.jBox.tip("删除失败", "error");
                        }
                    });
                }
            };
            $.jBox.confirm("确定要删除吗？", "提示", submit);
        }
        else {
            $.jBox.tip("请至少选择一条数据!", 'info');
        }
    }
};


var PackData = {
     Revoke: function (fid, type) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["fid"] = fid;
                param["type"] = type;
                $.gitAjax({
                    url: "/LogisticsManage/DataRevoke", type: "post", data: param, success: function (result) {
                        location.href = "FileDataList?filetype=" + type;
                    }, error: function (result) {
                        $.jBox.tip("撤回失败", "error");
                    }
                });
            }
        };
        $.jBox.confirm("确定撤回吗？撤回将会删除导入的数据！", "提示", submit);
    }
};