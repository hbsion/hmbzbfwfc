//公共js
$(function(){
	
	//退出
	$('.J_logout').click(function(){
		var url = PINER.root + '/?m=user&a=logout';
		$.layer({
			shade: [0.2, '#000',true],
			area: ['auto','auto'],
			title: '提示信息',
			dialog: {
				msg: '您确定退出么？',
				btns: 2,                    
				type: 4,
				btn: ['确定','取消'],
				yes: function(){
					$.post(url,{},function(result){
						if(result.status == 1){
							layer.msg(result.msg, 2, 1);
							window.location.reload();
							return false;
						} else {
							layer.msg('退出失败',2,3);
							return false;
						}
					},'json');
				}
			}
		});
	});
	
	//选择省*弹出城市
	$('#province').change(function(){
		var url = PINER.root + '/?m=public&a=getcity';
		var cityId = $('#province').val();
		if(cityId==0){
			$('#city').html('<option value="0">城市</option>');
			$('#area').html('<option value="0">区县</option>');
			$('#area_code').val('');
			return false;
		}else{
			$.post(url, {id:cityId}, function(result){
				$('#city').html(result.data);
				return false;
			},'json')	
		}
	});
	
	//选择市*弹出区县*区号
	$('#city').change(function(){
		var url = PINER.root + '/?m=public&a=getarea';
		var cityId = $('#city').val();
		if(cityId==0){
			$('#area').html('<option value="0">区县</option>');
			$('#area_code').val('');
			return false;
		}else{
			$.post(url, {id:cityId}, function(result){
				$('#area').html(result.data);
				$('#area_code').val(result.msg);
				return false;
			},'json')	
		}
	});
	
	//注册
	$('.J_sub_register').live('click',function(){
		//基本信息
		var inviting_code  = $.trim($('#inviting_code').val());
		var username       = $.trim($('#username').val());
		var weixin         = $.trim($('#weixin').val());
		var money          = $.trim($('#money').val());
		var idcard         = $.trim($('#idcard').val());
		//收货地址
		//var province       = $.trim($('#province').val());
		//var city           = $.trim($('#city').val());
		var area           = $.trim($('#area').val());
		var area_code      = $.trim($('#area_code').val());
		var street_address = $.trim($('#street_address').val());
		var intro          = $.trim($('#intro').val());
		//上传图片
		var idcard_img     = $.trim($('#idcard_img').val());
		var pay_img        = $.trim($('#pay_img').val());
		//注册信息
		var mobile         = $.trim($('#mobile').val());
		var mobile_code    = $.trim($('#mobile_code').val());
		var password       = $.trim($('#password').val());
		
		var return_url  = unescape($.trim($('#url').val()));
		var url         = PINER.root + '/?g=home&m=user&a=register';
		var th          = $(this);
		if(return_url==''){
			layer.msg('参数出错',1,3);
			return false;
		}
	 	if(inviting_code==''){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请输入邀请码',type : 3}	
			});
			return false;
		}
		if(username==''){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请输入姓名',type : 3}	
			});
			return false;
		}
		if(!username.match(username_regex)){ 
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 2,
				dialog : {msg:'姓名只能输入汉字和字母',type : 3}	
			});
			return false; 
		 }
		if(weixin==''){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请输入微信号',type : 3}	
			});
			return false;
		}
		if(!weixin.match(weixin_regex)){ 
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 2,
				dialog : {msg:'微信号不能含有汉字和特殊字符',type : 3}	
			});
			return false; 
		}
		if(!money.match(num7)){ 
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'首付金额填写错误！',type : 3}	
			});
			return false; 
		}
		if(idcard==''){
			layer.msg('请输入身份证号码',1,3);
			return false;
		}
		if(idcard!=''){
			var iSum=0 ;
			var info="" ;
			var sId = idcard;
			if(!/^\d{17}(\d|x)$/i.test(sId)){
				layer.msg("你输入的身份证长度或格式错误",1,3);
				return true;
			}
			sId=sId.replace(/x$/i,"a"); 
			if(aCity[parseInt(sId.substr(0,2))]==null){
				layer.msg("你的身份证地区非法",1,3);
				return true;
			}
			sBirthday=sId.substr(6,4)+"-"+Number(sId.substr(10,2))+"-"+Number(sId.substr(12,2)); 
			var d=new Date(sBirthday.replace(/-/g,"/")) ;
			if(sBirthday!=(d.getFullYear()+"-"+ (d.getMonth()+1) + "-" + d.getDate())){
				layer.msg("身份证上的出生日期非法",1,3);
				return true;
			}
			for(var i = 17;i>=0;i --) iSum += (Math.pow(2,i) % 11) * parseInt(sId.charAt(17 - i),11) ;
			if(iSum%11!=1){
				layer.msg("你输入的身份证号非法",1,3);
				return true;
			}
		}
		if(area=='0'){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请选择省市区',type : 3}	
			});
			return false;
		}
		if(area_code==''){
			layer.msg('区号不能为空',1,3);
			return false;
		}
		if(street_address==''){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请输入街道地址',type : 3}	
			});
			return false;
		}
		if(idcard_img==''){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请上传证件合影',type : 3}	
			});
			return false;
		 }
		 if(mobile==''){
			layer.msg('请输入手机号码',1,3);
			return false;
		 }
		 if(!mobile.match(mobile_regex)){ 
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 2,
				dialog : {msg:'手机号码格式不正确！',type : 3}	
			});
			return false; 
		 }
		 if(mobile_code=='' || mobile_code.length!=6){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 2,
				dialog : {msg:'手机验证码输入错误',type : 3}	
			});
			return false;
		 }
		 if(password=='' || password.length<6){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请输入不能小于6位的密码',type : 3}	
			});
			return false;
		 }
		 
		 th.removeClass('J_sub_register');
		 th.text('正在提交...');
		 $.post(url,{inviting_code:inviting_code,username:username,weixin:weixin,money:money,idcard:idcard,area:area,street_address:street_address,intro:intro,idcard_img:idcard_img,pay_img:pay_img,mobile:mobile,mobile_code:mobile_code,password:password},function(result){
			if(result.status == 1){
				window.location.href=return_url;
				//alert('注册成功');
				return false;
            } else {
				layer.msg(result.msg,2,3);
				th.addClass('J_sub_register');
				th.text('提交注册');
				return false;
            }
		 },'json');
	});
	
	//登录
	$('.J_sub_login').click(function(){
		var username    = $.trim($('#username').val());
		var password    = $.trim($('#password').val());
		var captcha     = $.trim($('#J_captcha').val());
		var remember    = $.trim($('#remember').val());
		var return_url  = unescape($.trim($('#url').val()));
		var url         = PINER.root + '/?g=home&m=user&a=login';
		var th          = $(this);
		if(return_url==''){
			layer.msg('参数出错',2,3);
			return false;
		}
		if(username==''){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 2,
				dialog : {msg:'请输入手机号',type : 3}	
			});
			return false;
		}
		if(password==''){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 2,
				dialog : {msg:'请输入密码',type : 3}	
			});
			return false;
		}
		th.removeClass('J_sub_login');
		th.text('正在登录...');
		$.post(url,{username:username,password:password,remember:remember,captcha:captcha},function(result){
			if(result.status == 1){
				window.location.href=return_url; 
				return false;
			} else {
				th.addClass('J_sub_login');
				th.text('登录');
				layer.msg(result.msg,2,3);
				return false;
			}
		},'json');
	});
	
	//修改密码
	$('.J_submit_edit_password').click(function(){
		var oldpassword = $.trim($('#oldpassword').val());
		var password    = $.trim($('#J_password').val());
		var repassword  = $.trim($('#J_repassword').val());
		var url         = PINER.root + '/?g=home&m=user&a=password';
		var th = $(this);
		if(oldpassword==''){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请输入当前登录密码',type : 3}	
			});
			return false;
		 }
		 if(password=='' || password.length<6){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请输入不能小于6位的修改后密码',type : 3}	
			});
			return false;
		 }
		 if(password != repassword){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'修改密码和确认密码不一致',type : 3}	
			});
			return false;
		 }
		 th.removeClass('J_submit_edit_password');
		 th.text('正在提交...');
		 $.post(url,{oldpassword:oldpassword,password:password,repassword:repassword},function(result){
			if(result.status == 1){
				$('#oldpassword').val('');
				$('#J_password').val('');
				$('#J_repassword').val('');
				 th.addClass('J_submit_edit_password');
				 th.text('确认修改');
				layer.msg(result.msg,1,1);
				return false;
			} else {
				 layer.msg(result.msg,1,3);
				 th.addClass('J_submit_edit_password');
				 th.text('确认修改');
				return false;
			}
		},'json');
	});
	
	//意见反馈
	$('.J_submit_feedback').click(function(){
		var info = $.trim($('#info').val());
		var url         = PINER.root + '/?g=home&m=user&a=feedback';
		var th = $(this);
		if(info==''){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请输入留言内容',type : 3}	
			});
			return false;
		 }
		 th.removeClass('J_submit_feedback');
		 th.text('正在提交...');
		 $.post(url,{info:info},function(result){
			if(result.status == 1){
				$('#info').val('');
				th.addClass('J_submit_feedback');
				th.text('提交留言');
				layer.msg(result.msg,1,1);
				return false;
			} else {
				 layer.msg(result.msg,1,3);
				 th.addClass('J_submit_feedback');
				 th.text('提交留言');
				return false;
			}
		},'json');
	});
	
	//找回密码
	$('.J_submit_password').click(function(){
		var mobile       = $.trim($('#mobile').val());
		var password     = $.trim($('#password').val());
		var mobile_code  = $.trim($('#mobile_code').val());
		var url          = PINER.root + '/?g=home&m=password&a=index';
		var th           = $(this);
		if(mobile==''){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请输入申请手机号码',type : 3}	
			});
			return false;
		 }
		 if(password=='' || password.length<6){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请输入不能小于6位的新密码',type : 3}	
			});
			return false;
		 }
		 if(!mobile.match(mobile_regex)){ 
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 2,
				dialog : {msg:'手机号码格式不正确！',type : 3}	
			});
			return false; 
		 }
		 th.removeClass('J_submit_password');
		 th.text('正在提交...');
		 $.post(url,{mobile:mobile,password:password,mobile_code:mobile_code},function(result){
			if(result.status == 1){
				$('#mobile').val('');
				$('#password').val('');
				$('#mobile_code').val('');
				th.addClass('J_submit_password');
				th.text('找回修改');
				layer.msg(result.msg,1,1);
				return false;
			} else {
				 layer.msg(result.msg,1,3);
				 th.addClass('J_submit_password');
				 th.text('找回修改');
				return false;
			}
		},'json');
	});
	
	//弹出菜单*展开
	$('.J_top_nav_block').live('click',function(){
		$('.J_top_menu').fadeIn();
		$('.J_top_nav_block').attr('class','J_top_nav_none');
	});
	//弹出菜单*关闭
	$('.J_top_nav_none').live('click',function(){
		$('.J_top_menu').fadeOut();
		$('.J_top_nav_none').attr('class','J_top_nav_block');
	});
	
	//提交新的订单
	$('.J_order_submit').click(function(){
		var pid       = $.trim($('#pid').val());
		var crate     = $.trim($('#crate').val());
		var url       = PINER.root + '/?g=home&m=order&a=submit';
		var th        = $(this);
		if(pid==0){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请选择产品',type : 3}	
			});
			return false;
		 }
		 if(!crate.match(num6)){ 
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 2,
				dialog : {msg:'数量是不可为0的整数！',type : 3}
			});
			return false; 
		 }
		 
		 th.removeClass('J_order_submit');
		 th.text('正在提交...');
		 $.post(url,{pid:pid,crate:crate},function(result){
			if(result.status == 1){
				$('#pid').val(0);
				$('#crate').val('');
				th.addClass('J_order_submit');
				th.text('提交订单');
				layer.msg(result.msg,1,1);
				return false;
			} else {
				 layer.msg(result.msg,2,3);
				 th.addClass('J_order_submit');
				 th.text('提交订单');
				return false;
			}
		},'json');
	});
	
	//审核订单*单个
	$('.J_status_new_order').click(function(){
		var th        = $(this);
		var id        = th.attr('rel');
		var parent_id = th.attr('parent_id');
		var url       = PINER.root + '/?g=home&m=order&a=status_order';
		var th        = $(this);
		var msg       = "确定要审核通过该订单么？";
		if(id==''){
			layer.msg('非法提交',1,1);
			return false;
		}
		th.removeClass('J_status_new_order');
		layer.confirm(msg,function(index){
			$.post(url,{id:id},function(result){
				if(result.status == 1){
					if(parent_id!=0){
						$('.node-'+parent_id).remove();
						th.parents('tr').children('td').children('span').find('a').text('-');
						th.parent('td').next('td').next('td').children('a').removeClass('J_order_next');
					}else{
						th.parents('tr').remove();	
					}
					$.layer({
						shade : [0.4 , '#000' , false],
						area : ['auto','auto'],
						title : false,
						closeBtn:false,
						time : 1,
						dialog : {msg:result.msg,type : 1}	
					});
					return false;
				} else {
					th.addClass('J_status_new_order');
					layer.msg(result.msg,1,3);
					return false;
				}
			},'json');
		});
	});
	
	//审核订单*批量
	$('.J_batch_status_new_order').live('click',function(){
		var ids = '';
		var th  = $(this);
		$('.J_checkitem:checked').each(function(){
			ids += $(this).val() + ',';
		});
		var len=ids.length;
		var ids=ids.substring(0,len-1);
		
		var url = PINER.root + '/?g=home&m=order&a=status_order';
		var msg       = "确定要审核通过选择的订单么？";
		if(ids=='' || ids==','){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请选择要操作的选项',type : 3}
			});
			return false;
		}
		var i = $.layer({
			shade: [0],
			area: ['auto','auto'],
			dialog: {
				msg: msg,
				btns: 2,                    
				type: 4,
				btn: ['确定','取消'],
				yes: function(){
					th.removeClass('J_batch_status_new_order');
					th.text('正在提交...');
					$.post(url,{ids:ids},function(result){
						if(result.status == 1){
							$('.J_checkitem:checked').each(function(){
								$('input[uids="'+$(this).attr('uids')+'"]').attr('checked', false);
								$('input[uids="'+$(this).attr('uids')+'"]').val('');
							});
							th.addClass('J_batch_status_new_order');
							th.text('批 准');
							layer.msg(result.msg,1,1);
							window.location.reload();
							return false;
						} else {
							th.addClass('J_batch_status_new_order');
							th.text('批 准');
							layer.msg(result.msg,1,3);
							return false;
						}
					},'json');
				}, no: function(){
					th.addClass('J_batch_status_new_order');
					th.text('批 准');
					layer.close(i);
					return false;
				}
			}
		});
		
		
		
	});
	
	//订单拒绝*单个*弹出
	$('.J_deny_new_order').click(function(){
		var th = $(this);
		var id = th.attr('rel');
		var parent_id = th.attr('parent_id');
		if(id==''){
			layer.msg('非法提交',1,1);
			return false;
		}
		$('#id').val(id);
		$('#parent_id').val(parent_id);
		$('#deny_intro').val('');
		var i = $.layer({
			type: 1,
			title: false,
			closeBtn: false,
			border : [5, 0.5, '#666', false],
			offset: ['30px',''],
			move: ['.juanmove', true],
			area: ['80%',''],
			page : {dom : '.J_deny_order_tc'}
		});
		//取消
		$('.upload_btn2').click(function(){
			layer.close(i);
		})
	});
	
	//订单拒绝*批量*弹出
	$('.J_batch_deny_new_order').click(function(){
		var ids = '';
			$('.J_checkitem:checked').each(function(){
				ids += $(this).val() + ',';
			});
			var len=ids.length;
			var ids=ids.substring(0,len-1);
		if(ids==''){
			layer.msg('请选择要操作的选项',1,3);
			return false;
		}
		$('#deny_intro').val('');
		var i = $.layer({
			type: 1,
			title: false,
			closeBtn: false,
			border : [5, 0.5, '#666', false],
			offset: ['30px',''],
			move: ['.juanmove', true],
			area: ['80%',''],
			page : {dom : '.J_deny_order_tc'}
		});
		//取消
		$('.upload_btn2').click(function(){
			layer.close(i);
		})
	});
	
	//确定拒绝订单*弹出里的确定按钮
	$('.J_submit_deny').click(function(){
		var ids = '';
			$('.J_checkitem:checked').each(function(){
				ids += $(this).val() + ',';
			});
			var len=ids.length;
			var ids=ids.substring(0,len-1);
		var deny_intro = $('#deny_intro').val();
		var parent_id  = $('#parent_id').val();
		var url = PINER.root + '/?g=home&m=order&a=deny_order';

		if(ids==''){
			layer.msg('请选择要操作的选项',1,3);
			return false;
		}
		if(deny_intro==''){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请填写拒绝理由',type : 3}
			});
			return false;	
		}
		layer.closeAll()
		$.post(url,{ids:ids,deny_intro:deny_intro},function(result){
			if(result.status == 1){
				$('.J_checkitem:checked').each(function(){
					$('input[uids="'+$(this).attr('uids')+'"]').attr('checked', false);
					$('input[uids="'+$(this).attr('uids')+'"]').val('');
				});
				$.layer({
					shade : [0.4 , '#000' , false],
					area : ['auto','auto'],
					title : false,
					closeBtn:false,
					time : 1,
					dialog : {msg:result.msg,type : 1}	
				});
				window.location.reload();
				return false;
			} else {
				layer.msg(result.msg,1,3);
				return false;
			}
		},'json');
	});
	
	//全选反选
	$('.J_checkall').click(function(){
		$('.J_checkitem').attr('checked', this.checked);
		$('.J_checkall').attr('checked', this.checked);
	});
	
	//重新提交订单
	$('.J_again_submit_order').live('click',function(){
		var id = $(this).attr('rel');
		if(id==''){
			layer.msg('非法提交',1,3);
			return false;	
		}
		var url = PINER.root + '/?g=home&m=order&a=again_submit_order';
		var th  = $(this); 
		th.removeClass('J_again_submit_order');
		var i = $.layer({
			shade: [0.2, '#000',true],
			area: ['auto','auto'],
			title: '提示信息',
			dialog: {
				msg: '请与上级沟通好后再次申请',
				btns: 2,                    
				type: 4,
				btn: ['确定','取消'],
				yes: function(){
					$.post(url,{id:id},function(result){
						if(result.status == 1){
							layer.msg(result.msg, 2, 1);
							window.location.reload();
							return false;
						} else {
							th.addClass('J_again_submit_order');
							layer.msg(result.msg,2,3);
							return false;
						}
					},'json');
				}, no: function(){
					th.addClass('J_again_submit_order');
					layer.close(i)
				}
			}
		});
	});
	
	$('.J_search_user').click(function(){
		var keyword       = $.trim($('#keyword').val());
		var captcha       = $.trim($('#J_captcha').val());
		var url           = PINER.root + '/?g=home&m=search&a=index';
		var th            = $(this);
		if(keyword==''){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请输入手机号或微信号查询',type : 3}	
			});
			return false;	
		}
		if(captcha==''){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请输入验证码',type : 3}	
			});
			return false;	
		}
		th.removeClass('J_search_user');
		$('.J_user_content').html('<div style="text-align:center;">正在查询...</div>');
		$.post(url,{keyword:keyword,captcha:captcha},function(result){
			if(result.status == 1){
				$('.srarch_title').show();
				$('.J_user_content').html(result.data);
				$('.J_user_content_bg').show();
				$('#keyword').val('');
				$('#J_captcha').val('');
				th.addClass('J_search_user');
				return false;
			}else if(result.status == 2){
				$('.srarch_title').show();
				$('.J_user_content_bg').show();
				th.addClass('J_search_user');
				$('.J_user_content > div').text(result.msg);
				return false;
			}else{
				$('.J_user_content').html('');
				th.addClass('J_search_user');
				layer.msg(result.msg,1,3);
				return false;
			}
		},'json');
	});
	
	//用户展开下一级
	$('.J_user_next').live('click',function(){
		var th       = $(this);
		var id       = th.attr('rel');
		var url      = PINER.root + '/?g=home&m=user&a=user_next_ajax';
		var rel_left = parseInt(th.parent('td').attr('rel'));
		var left_num = rel_left+10;
		if(id==''){
			layer.msg('非法操作',1,3);
			return false;	
		}
		var tr = th.parent().parent().attr('class');
		if(!tr){
		    $('.zk_btn').attr('class','zk_btn lv J_user_next').text('展开');
		    $('.no').hide();
		}
		th.attr('class','zk_btn J_expanded').text('合并');
		th.parents('tr').children('td').find('.bdfh a').text('-');
		$.post(url,{id:id,left_num:left_num},function(result){
			if(result.status == 1){
				th.parents('tr').after(result.data);
				return false;
			}else{
				th.attr('class','zk_btn lv J_user_next').text('展开');
				layer.msg(result.msg,1,3);
				return false;
			}
		},'json');
	});
	
	//用户合并下一级
	$('.J_expanded').live('click',function(){
		var th = $(this);
		var id = th.attr('rel');
		var tr = th.parent().parent().attr('class');
		if(!tr){
		    $('.no').hide();
		}
		$('.node-'+id).hide();
		th.attr('class','zk_btn lv J_user_next').text('展开');
		th.parents('tr').children('td').find('.bdfh a').text('+');
		return false;
	});	
	
	
	//订单展开下一级
	$('.J_order_next').live('click',function(){
		var th       = $(this);
		var id       = th.attr('rel');
		if(id==''){
			layer.msg('非法操作',1,3);
			return false;	
		}
		$('.zk_btn').attr('class','zk_btn lv J_order_next').text('展开');
		$('.dl_2').hide();
		$('.node-'+id).show();
		var tr = th.parent().parent().attr('class');
		th.attr('class','zk_btn J_order_expanded').text('合并');
		th.parents('tr').children('td').find('.bdfh a').text('-');
		return false;
	});
	
	//订单合并下一级
	$('.J_order_expanded').live('click',function(){
		var th = $(this);
		var id = th.attr('rel');
		$('.node-'+id).hide();
		th.attr('class','zk_btn lv J_order_next').text('展开');
		th.parents('tr').children('td').find('.bdfh a').text('+');
		return false;
	});
	
	//未处理订单搜索按钮
	$('.J_search_order').click(function(){
		var time_start = $('#time_start').val();
		var time_end   = $('#time_end').val();
		var data1 = new Date(time_start.replace(/\-/g, "\/"));  
 		var data2 = new Date(time_end.replace(/\-/g, "\/"));  
		if(data1 >= data2){
			layer.msg('开始时间不能大于或等于结束时间',1,3);
			return false;
		}
		$('#from').submit();
		return false;
	});
	
	//流程中订单搜索按钮
	$('.J_search_process_order').click(function(){
		$('#from').submit();
		return false;
	});
	
	//拒绝之后重新提交注册信息
	$('.J_sub_again_info').live('click',function(){
		var url            = PINER.root + '/?m=user&a=sub_again_info';
		var return_url     = PINER.root + '/?m=user&a=index';
		
		var username       = $.trim($('#username').val());
		var weixin         = $.trim($('#weixin').val());
		var money          = $.trim($('#money').val());
		var idcard         = $.trim($('#idcard').val());
		//收货地址
		//var province       = $.trim($('#province').val());
		//var city           = $.trim($('#city').val());
		var area           = $.trim($('#area').val());
		var area_code      = $.trim($('#area_code').val());
		var street_address = $.trim($('#street_address').val());
		var intro          = $.trim($('#intro').val());
		//上传图片
		var idcard_img     = $.trim($('#idcard_img').val());
		var pay_img        = $.trim($('#pay_img').val());
		//注册信息
		var mobile         = $.trim($('#mobile').val());
		var mobile_code    = $.trim($('#mobile_code').val());
		var password       = $.trim($('#password').val());
		var th             = $(this);
		
		if(username==''){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请输入姓名',type : 3}	
			});
			return false;
		}
		if(!username.match(username_regex)){ 
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 2,
				dialog : {msg:'姓名只能输入汉字和字母',type : 3}	
			});
			return false; 
		 }
		if(weixin==''){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请输入微信号',type : 3}	
			});
			return false;
		}
		if(!weixin.match(weixin_regex)){ 
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 2,
				dialog : {msg:'微信号不能含有汉字和特殊字符',type : 3}	
			});
			return false; 
		}
		if(!money.match(num7)){ 
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'首付金额填写错误！',type : 3}	
			});
			return false; 
		}
		if(idcard==''){
			layer.msg('请输入身份证号码',1,3);
			return false;
		}
		if(idcard!=''){
			var iSum=0 ;
			var info="" ;
			var sId = idcard;
			if(!/^\d{17}(\d|x)$/i.test(sId)){
				layer.msg("你输入的身份证长度或格式错误",1,3);
				return true;
			}
			sId=sId.replace(/x$/i,"a"); 
			if(aCity[parseInt(sId.substr(0,2))]==null){
				layer.msg("你的身份证地区非法",1,3);
				return true;
			}
			sBirthday=sId.substr(6,4)+"-"+Number(sId.substr(10,2))+"-"+Number(sId.substr(12,2)); 
			var d=new Date(sBirthday.replace(/-/g,"/")) ;
			if(sBirthday!=(d.getFullYear()+"-"+ (d.getMonth()+1) + "-" + d.getDate())){
				layer.msg("身份证上的出生日期非法",1,3);
				return true;
			}
			for(var i = 17;i>=0;i --) iSum += (Math.pow(2,i) % 11) * parseInt(sId.charAt(17 - i),11) ;
			if(iSum%11!=1){
				layer.msg("你输入的身份证号非法",1,3);
				return true;
			}
		}
		if(area=='0'){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请选择省市区',type : 3}	
			});
			return false;
		}
		if(area_code==''){
			layer.msg('区号不能为空',1,3);
			return false;
		}
		if(street_address==''){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请输入街道地址',type : 3}	
			});
			return false;
		}
		if(idcard_img==''){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请上传证件合影',type : 3}	
			});
			return false;
		 }
		 if(mobile==''){
			layer.msg('请输入手机号码',1,3);
			return false;
		 }
		 if(!mobile.match(mobile_regex)){ 
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 2,
				dialog : {msg:'手机号码格式不正确！',type : 3}	
			});
			return false; 
		 }
		 if(mobile_code=='' || mobile_code.length!=6){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 2,
				dialog : {msg:'手机验证码输入错误',type : 3}	
			});
			return false;
		 }
		 if(password=='' || password.length<6){
			$.layer({
				shade : [0.4 , '#000' , false],
				area : ['auto','auto'],
				title : false,
				closeBtn:false,
				time : 1,
				dialog : {msg:'请输入不能小于6位的密码',type : 3}	
			});
			return false;
		 }
		
		var i = $.layer({
			shade: [0.2, '#000',true],
			area: ['auto','auto'],
			title: '提示信息',
			dialog: {
				msg: '请确定修改拒绝原因之后在进行提交',
				btns: 2,                    
				type: 4,
				btn: ['确定','取消'],
				yes: function(){
					th.removeClass('J_sub_again_info');
		 			th.text('正在提交...');
					$.post(url,{username:username,weixin:weixin,money:money,idcard:idcard,area:area,street_address:street_address,intro:intro,idcard_img:idcard_img,pay_img:pay_img,mobile:mobile,mobile_code:mobile_code,password:password},function(result){
						if(result.status == 1){
							layer.msg(result.msg, 2, 1);
							window.location.href=return_url;
							return false;
						} else {
							th.addClass('J_sub_again_info');
		 					th.text('确定修改');
							layer.msg(result.msg,2,3);
							return false;
						}
					},'json');
					
				}
			}
		});
	});
	
	

//初始化弹窗
lang.dialog_ok = "确定";
lang.dialog_cancel = "取消";
//end
})

/*
*用户注册上传证件照和付款截图
*/
function upload_img(J_upload_img,folder,input_name){
	var img_uploader = new qq.FileUploaderBasic({
        allowedExtensions: ['jpg','gif','jpeg','png','bmp','pdg'],
        button: document.getElementById(J_upload_img),
        multiple: false,
        action: PINER.root + '/?m=public&a=register_upload_img&folder='+folder+'',
        inputName: 'img',
        forceMultipart: true, //用$_FILES
        messages: {
            typeError: lang.upload_type_error,
            sizeError: lang.upload_size_error,
            minSizeError: lang.upload_minsize_error,
            emptyError: lang.upload_empty_error,
            noFilesError: lang.upload_nofile_error,
            onLeave: lang.upload_onLeave
        },
        showMessage: function(message){
            layer.msg(message);
        },
        onSubmit: function(id, fileName){
            $('#'+J_upload_img).addClass('btn_disabled').find('span').text(lang.uploading);
        },
        onComplete: function(id, fileName, result){
            $('#'+J_upload_img).removeClass('btn_disabled').find('span').text(lang.upload);
            if(result.status == '1'){
                $('#'+input_name).val(result.data);
				layer.msg('上传成功',1,1);
            } else {
                layer.msg(result.msg);
            }
        }
    });	
}


/*
*正则
*/
//验证手机
var mobile_regex = /^13[0-9]{1}[0-9]{8}$|15[012356789]{1}[0-9]{8}$|18[0123456789]{1}[0-9]{8}$|14[0-9]{1}[0-9]{8}$|17[0123456789]{1}[0-9]{8}$/;
var num1= /^(1[4-9]\d|230)$/;//身高>140 <230，不能有小数
var num1_1= /\b[3-9]\d|1[0-4]\d|150\b/;//身高>30 <150 ，可以有小数
var num2= /^([3-9]\d|1[0-4]\d|150)$/;//体重>30 <150 ，可以有小数
var num4= /^\+?[1-9][0-9]*$/;//正数（正整数 + 0）
var num5= /^([2-9]\d|50)$/;//胸围
var email_regex= /^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/;//邮箱
//var money_regex = /^[1-9]\d{0,5}(\.\d{1,2})?/;//金额，支持两位小数，不可以为0
var money_regex = /^[1-9]\d{0,5}(\.\d{1,2})?$/;//金额，支持两位小数，不可以为0
var num6 = /^[1-9-.]\d*$/; //不可为0的整数
var num7 = /^[1-9]\d*.\d*|0.\d*[1-9]\d*$/; //不可为0的整数
var username_regex = /^([\u4E00-\uFA29]|[\uE7C7-\uE7F3]|[a-zA-Z])*$/;
var weixin_regex = /^[A-Za-z0-9_-]+$/;

//验证身份证号码
var aCity={11:"北京",12:"天津",13:"河北",14:"山西",15:"内蒙古",21:"辽宁",22:"吉林",23:"黑龙江 ",31:"上海",32:"江苏",33:"浙江",34:"安徽",35:"福建",36:"江西",37:"山东",41:"河南",42:"湖北 ",43:"湖南",44:"广东",45:"广西",46:"海南",50:"重庆",51:"四川",52:"贵州",53:"云南",54:"西藏 ",61:"陕西",62:"甘肃",63:"青海",64:"宁夏",65:"新疆",71:"台湾",81:"香港",82:"澳门",91:"国外 "}
