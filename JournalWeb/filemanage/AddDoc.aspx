<%@ Page Language="C#" AutoEventWireup="true" Codebehind="AddDoc.aspx.cs" Inherits="JournalWeb.filemanage.AddDoc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>文档上传</title>
    <style type="text/css">
    body
    {
        margin:0px;
        padding:0px;
        font-size:14px;
    }
    .enter
    {
        text-align:center;
        
    }
    .btn{
        padding:2px 2px 0px 2px;
        border:#2C59AA 1px solid;         
        font-size: 12px; 
        filter:progid:DXImageTransform.Microsoft.Gradient(GradientType=0,StartColorStr=#ffffff, EndColorStr=#D7E7FA);         
        cursor: hand; 
        color: black;         
        text-align:center;
    }
    /*file容器样式*/
a.files {
	width:90px;
	height:30px;
	overflow:hidden;
	display:block;
	float:left;
	border:1px solid #BEBEBE;
	background:url(/Inc/images/fu_btn.gif) left top no-repeat;
	text-decoration:none;

}
a.files:hover {
	background-color:#FFFFEE;
	background-position:0 -30px;
}
/*file设为透明，并覆盖整个触发面*/
a.files input {
	margin-left:-350px;
	font-size:30px;
	cursor:pointer;
	filter:alpha(opacity=0);
	opacity:0;
}
/*取消点击时的虚线框*/
a.files, a.files input {
	outline:none;/*ff*/
	hide-focus:expression(this.hideFocus=true);/*ie*/
}
.fu_list {
	width:98%;
	background:#ebebeb;
	font-size:12px;
}
.fu_list td {
	padding:5px;
	line-height:20px;
	background-color:#fff;
}
.fu_list table {
	width:100%;
	border:1px solid #ebebeb;
}
.fu_list thead td {
	background-color:#f4f4f4;
}
.fu_list b {
	font-size:14px;
}

    #formwrapper { width:510px;margin:15px auto;padding:20px;text-align:left;border:1px #1E7ACE solid;}
    </style>
   <script type="text/javascript">
   
   function selectType()
   {
        var drop=document.getElementById("drop_DocType");
        var   index   =   drop.selectedIndex; 
        document.getElementById("typeId").value=drop.options[index].text;
   }
   </script>
</head>
<body onload="selectType();">
    <form id="uploadForm" action="/handler/File.ashx">
        <div id="formwrapper">
        <div>
        文档类型：<select name="drop_DocType" id="drop_DocType" onchange="selectType();">        
	<option value="1">交友在线</option>
	<option value="2">恋爱之家</option>
	<option value="3">快乐家庭</option>
	<option value="4">老乡会</option>
	<option value="5">校友沙龙</option>
	<option value="6">圈友同盟</option>
	<option value="7">同行方舟</option>
	<option value="8">中华沙龙</option>
	<option value="9">中华社区</option>
	<option value="10">中华互助</option>
	<option value="11">个人中心</option>
	<option value="12">个人事务</option>
	<option value="13">交流互动</option>
	<option value="14">原创地带</option>
	<option value="15">时代人生</option>
	<option value="16">生活助理</option>
	<option value="17">玩乐吧</option>
	<option value="18">策划文档</option>
	<option value="19">效果图</option>
	<option value="20">郑州文档</option>
	<option value="21">北京文档</option>
</select>        
        </div>
            <div>
                文档标题：<input type="text" name="txt_title" id="txt_title" size="25" maxlength="30" /><br />
            </div>            
            <div>
                    上传附件：<input type="hidden" name="txt_url" id="txt_url" size="25" readonly /></div>
            <div style="padding-left:70px;">
                <a href="javascript:void(0);" class="files" id="idFile"></a><img id="idProcess" style="display:none;" src="/Inc/images/loading.gif" /><br />
            </div>
            <div><br />
            <span style="font-size:14px;">
            温馨提示：最多可同时上传 <b id="idLimit"></b> 个文件，只允许上传 <b id="idExt"></b> 文件。 不得超过30MB</span></div>
            <div class="fu_list">
            <table border="0" cellspacing="0">
            <thead>
              <tr>
                <td>文件路径</td>
                <td width="100"></td>
              </tr>
            </thead>
            <tbody id="idFileList">
            </tbody>
          </table>
            </div>
            <div>
                <br />
                备&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 注：<textarea cols="45" rows="6" name="txt_Desc"
                    id="txt_Desc"></textarea>
                <input id="typeId" type="hidden" runat="server" /></div>
        </div>
        <div class="enter"  id="idMsg">
            <input name="btn_add" id="btn_add" type="submit" class="btn" value="确定" />
            <input name="idBtndel" id="idBtndel" type="reset" class="btn" value="重置" /></div>
            
            
    </form>
    <script type="text/javascript">

var isIE = (document.all) ? true : false;

var $ = function (id) {
    return "string" == typeof id ? document.getElementById(id) : id;
};

var Class = {
  create: function() {
    return function() {
      this.initialize.apply(this, arguments);
    }
  }
}

var Extend = function(destination, source) {
	for (var property in source) {
		destination[property] = source[property];
	}
}

var Bind = function(object, fun) {
	return function() {
		return fun.apply(object, arguments);
	}
}

var Each = function(list, fun){
	for (var i = 0, len = list.length; i < len; i++) { fun(list[i], i); }
};

//文件上传类
var FileUpload = Class.create();
FileUpload.prototype = {
  //表单对象，文件控件存放空间
  initialize: function(form, folder, options) {
	
	this.Form = $(form);//表单
	this.Folder = $(folder);//文件控件存放空间
	this.Files = [];//文件集合
	
	this.SetOptions(options);
	
	this.FileName = this.options.FileName;
	this._FrameName = this.options.FrameName;
	this.Limit = this.options.Limit;
	this.Distinct = !!this.options.Distinct;
	this.ExtIn = this.options.ExtIn;
	this.ExtOut = this.options.ExtOut;
	
	this.onIniFile = this.options.onIniFile;
	this.onEmpty = this.options.onEmpty;
	this.onNotExtIn = this.options.onNotExtIn;
	this.onExtOut = this.options.onExtOut;
	this.onLimite = this.options.onLimite;
	this.onSame = this.options.onSame;
	this.onFail = this.options.onFail;
	this.onIni = this.options.onIni;
	
	if(!this._FrameName){
		//为每个实例创建不同的iframe
		this._FrameName = "uploadFrame_" + Math.floor(Math.random() * 1000);
		//ie不能修改iframe的name
		var oFrame = isIE ? document.createElement("<iframe name=\"" + this._FrameName + "\">") : document.createElement("iframe");
		//为ff设置name
		oFrame.name = this._FrameName;
		oFrame.style.display = "none";
		//在ie文档未加载完用appendChild会报错
		document.body.insertBefore(oFrame, document.body.childNodes[0]);
	}
	
	//设置form属性，关键是target要指向iframe
	this.Form.target = this._FrameName;
	this.Form.method = "post";
	//注意ie的form没有enctype属性，要用encoding
	this.Form.encoding = "multipart/form-data";

	//整理一次
	this.Ini();
  },
  //设置默认属性
  SetOptions: function(options) {
    this.options = {//默认值
		FileName:	"",//文件上传控件的name，配合后台使用
		FrameName:	"",//iframe的name，要自定义iframe的话这里设置name
		onIniFile:	function(){},//整理文件时执行(其中参数是file对象)
		onEmpty:	function(){},//文件空值时执行
		Limit:		0,//文件数限制，0为不限制
		onLimite:	function(){},//超过文件数限制时执行
		Distinct:	true,//是否不允许相同文件
		onSame:		function(){},//有相同文件时执行
		ExtIn:		[],//允许后缀名
		onNotExtIn:	function(){},//不是允许后缀名时执行
		ExtOut:		[],//禁止后缀名，当设置了ExtIn则ExtOut无效
		onExtOut:	function(){},//是禁止后缀名时执行
		onFail:		function(){},//文件不通过检测时执行(其中参数是file对象)
		onIni:		function(){}//重置时执行
    };
    Extend(this.options, options || {});
  },
  //整理空间
  Ini: function() {
	//整理文件集合
	this.Files = [];
	//整理文件空间，把有值的file放入文件集合
	Each(this.Folder.getElementsByTagName("input"), Bind(this, function(o){
		if(o.type == "file"){ o.value && this.Files.push(o); this.onIniFile(o); }
	}))
	//插入一个新的file
	var file = document.createElement("input");
	file.name = this.FileName; file.type = "file"; file.onchange = Bind(this, function(){ this.Check(file); this.Ini(); });
	this.Folder.appendChild(file);
	//执行附加程序
	this.onIni();
  },
  //检测file对象
  Check: function(file) {
	//检测变量
	var bCheck = true;
	//空值、文件数限制、后缀名、相同文件检测
	if(!file.value){
		bCheck = false; this.onEmpty();
	} else if(this.Limit && this.Files.length >= this.Limit){
		bCheck = false; this.onLimite();
	} else if(!!this.ExtIn.length && !RegExp("\.(" + this.ExtIn.join("|") + ")$", "i").test(file.value)){
		//检测是否允许后缀名
		bCheck = false; this.onNotExtIn();
	} else if(!!this.ExtOut.length && RegExp("\.(" + this.ExtOut.join("|") + ")$", "i").test(file.value)) {
		//检测是否禁止后缀名
		bCheck = false; this.onExtOut();
	} else if(!!this.Distinct) {
		Each(this.Files, function(o){ if(o.value == file.value){ bCheck = false; } })
		if(!bCheck){ this.onSame(); }
	}
	//没有通过检测
	!bCheck && this.onFail(file);
  },
  //删除指定file
  Delete: function(file) {
	//移除指定file
	this.Folder.removeChild(file); this.Ini();
  },
  //删除全部file
  Clear: function() {
	//清空文件空间
	Each(this.Files, Bind(this, function(o){ this.Folder.removeChild(o); })); this.Ini();
  }
}

var fu = new FileUpload("uploadForm", "idFile", { Limit: 1, ExtIn: ["jpg", "gif","bmp","doc","rar"],
	onIniFile: function(file){ file.value ? file.style.display = "none" : this.Folder.removeChild(file); },
	onEmpty: function(){ alert("请选择一个文件"); },
	onLimite: function(){ alert("超过上传限制"); },
	onSame: function(){ alert("已经有相同文件"); },
	onNotExtIn:	function(){ alert("只允许上传" + this.ExtIn.join("，") + "文件"); },
	onFail: function(file){ this.Folder.removeChild(file); },
	onIni: function(){
		//显示文件列表
		var arrRows = [];
		if(this.Files.length){
			var oThis = this;
			Each(this.Files, function(o){
				var a = document.createElement("a"); a.innerHTML = "取消"; a.href = "javascript:void(0);";
				a.onclick = function(){ oThis.Delete(o); return false; };
				arrRows.push([o.value, a]);				
			});
		} else { arrRows.push(["<font color='gray'>没有添加文件</font>", "&nbsp;"]); }
		AddList(arrRows);
		//设置按钮
		$("btn_add").disabled = $("idBtndel").disabled = this.Files.length <= 0;
	}
});

$("btn_add").onclick = function(){
	//显示文件列表
	var arrRows = [];
	Each(fu.Files, function(o){ arrRows.push([o.value, "&nbsp;"]); });
	AddList(arrRows);
	
	fu.Folder.style.display = "none";
	$("idProcess").style.display = "";
	$("idMsg").innerHTML = "正在添加文件到服务器，请稍候……<br />有可能因为网络问题，出现程序长时间无响应，请点击“<a href='?'><font color='red'>取消</font></a>”重新上传文件";
	
	fu.Form.submit();
}

//用来添加文件列表的函数
function AddList(rows){
	//根据数组来添加列表
	var FileList = $("idFileList"), oFragment = document.createDocumentFragment();
	//用文档碎片保存列表
	Each(rows, function(cells){
		var row = document.createElement("tr");
		Each(cells, function(o){
			var cell = document.createElement("td");
			if(typeof o == "string"){ cell.innerHTML = o; }else{ cell.appendChild(o); }
			row.appendChild(cell);
		});
		oFragment.appendChild(row);
	})
	//ie的table不支持innerHTML所以这样清空table
	while(FileList.hasChildNodes()){ FileList.removeChild(FileList.firstChild); }
	FileList.appendChild(oFragment);
}


$("idLimit").innerHTML = fu.Limit;

$("idExt").innerHTML = fu.ExtIn.join("，");

$("idBtndel").onclick = function(){ fu.Clear(); }

//在后台通过window.parent来访问主页面的函数
function Finish(msg){ alert(msg); top.location.href = top.location.href; }
</script>
</body>
</html>
