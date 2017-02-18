$(document).ready(function () {
    var cookie = $.cookie('des');
    //alert(cookie);
    var ua = window.navigator.userAgent.toLowerCase();
    if (cookie == null || cookie == "" || cookie == "undefined") {
        if (ua.match(/MicroMessenger/i) == 'micromessenger') {
            window.location.href = "WeChatLand.aspx";
        } else {
            window.location.href = "/Login";
        }
    } else {
        var url = '/CheckAuthorization/Get?t=' + new Date;
        //var data = { cookie: cookie };
        var data = {};

        $.ajax({
            type: 'get',
            url: url,
            data: data,
            async: false,
            success: function (obj) {
                //var obj = jQuery.parseJSON(data);
                var errorcode = obj.ErrorCode;
                
                if (errorcode == "1" || errorcode == "2") {
                    if (ua.match(/MicroMessenger/i) == 'micromessenger') {
                        window.location.href = "WeChatLand.aspx";
                    } else {
                        window.location.href = "Signin.html";
                    }
                } else if (errorcode == "3") {
                    alert("您的Vip权限已过期，请充值");
                    window.location.href = "/view/index.html";
                } else if (errorcode == "4") {
                    alert("网络异常，请稍后再试");
                }
            },
            error: function () {
                alert("网络异常，请稍后再试");
            }
        });
    }
});

function GetCookie() {
    var res;
    var cookie = $.cookie('des');

    if (cookie == null || cookie == "" || cookie == "undefined") {
        res = "";
    } else {
        res = cookie;
    }

    return res;
}