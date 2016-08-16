$(document).ready(function () {
    //验证码随机
    $('#getcode').on('click', function () {
        $(this).attr('src', '/default/CaptchaImage?t=' + Math.random());
    });
    // 背景图片
    var aDiv = $('.banner-list').find('div'),
        index = 0;
    setInterval(function () {
        startFocus();
    }, 10000);
    function startFocus() {
        index++;
        index = index > aDiv.size() - 1 ? 0 : index;
        aDiv.eq(index)
             .stop()
             .animate({ 'opacity': 1 }, 300)
             .css({ 'z-index': 10 })
             .siblings()
             .stop()
             .animate({ 'opacity': 0 }, 300)
             .css({ 'z-index': 0 });
    }
    //function stopFoucs() {
    //    clearInterval(timer);
    //};
    //// 选择登录方式
    //var wxrun = '';
    $('.login-type')
        .toggle(function() {
            $('#pcLogin').hide();
            $('#codeLogin').show();
            $('#login-box h1').html('微信登录');
            $(this).attr('title', '用户名登录');
            $(this).removeClass('wx').addClass('pc');
        },
            function() {
                $('#pcLogin').show();
                $('#codeLogin').hide();
                $('#login-box h1').html('会员登录');
                $(this).attr('title', '微信登录');
                $(this).removeClass('pc').addClass('wx');
            });
    //表单验证
    $('form[id=pcLogin]').submit(function (e) {
        e.preventDefault();
        if ($('#username').val() == '' || $('#username').val() == '身份证号码/手机号码') {
            $('.login_err').html('请填写：身份证号码 / 手机号码');
            $('.login_err').show();
        }
        else if ($('#password').val() == '') {
            $('.login_err').html('请填写密码！');
            $('.login_err').show();
        }
        else if ($('#postcaptcha').val() == '') {

            $('.login_err').html('请填写验证码！');
            $('.login_err').show();
        }
        else {
            $('#login').hide();
            $('#waiting').show();
            //if ($("#expire").attr("checked") == true) {
            //    var expire = $("#expire").val();
            //}
            //else {
            //    var expire = "";
            //}
            var ipObj = $('#ip').text().trim().toObject();
            var ip = '';
            var address = '';
            if(ipObj){
                ip = ipObj.ip;
                address = ipObj.address;
            }
            $.post('/default/login',
                {
                    account: $('#username').val(),
                    password: $('#password').val(),
                    area: address,
                    ip: ip,
                    vcode: $('#postcaptcha').val(),
                    //expire: expire,
                    browser: fx.getBrowser(),
                    device: fx.getDevice(),
                    os: fx.getOS()
                },
                function(result) {
                    if (result.success) {
                        window.location.href = result.url;
                    } else {
                        $('#login').show();
                        $('#waiting').hide();
                        $('#password').attr('value', '');
                        $('.login_err').show();
                        $('.login_err').html(result.message);
                        $('#getcode').click();
                    }
                });
        }
        return false;
    });
});