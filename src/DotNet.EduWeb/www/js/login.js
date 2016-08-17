(function ($, window) {
    'use strict';

    // ============================================================ //

    var Login = function () {

        var $account, $password, $submit;

        /**
         * 获取随机数字
         * @param {number} max 随机数最大值
         * @returns {number} 返回从1到最大值的随机数
         */
        var getRandomNumber = function (max) {
            return Math.floor(Math.random() * max + 1);
        };

        /**
         * 设置页面背景图片
         */
        var setBackground = function () {
            var num = 1;//getRandomNumber(50);
            $('body').css('background', 'url(/www/img/bg/' + num + '.jpg)');
        };

        /**
         * 消息提示
         * @param {string} msg 消息
         */
        var msg = function (msg) {
            $submit.popover({
                animation: true,
                placement: 'bottom',
                content: '<span style="color:red;font-weight:bold;">' + msg + '</span>',
                html: true,
                trigger: 'manual',
                delay: { show: 10000, hide: 100 }
            });
            $submit.popover('show');
            setTimeout(function () {
                $submit.popover('destroy');
            }, 5000);
        };

        /**
         * 用户登录
         */
        var loginCore = function () {
            if ($account.val() == '') {
                msg('请输入账号');
                $account.focus();
                return;
            }
            $submit.val('登录中..').attr('disabled', 'disabled');
            $account.attr('disabled', 'disabled');
            $password.attr('disabled', 'disabled');
            $.ajax({
                url: $submit.data('url'),
                data: {
                    account: $account.val(),
                    password: $password.val(),
                    area: '',
                    ip: '',
                    browser: fx.getBrowser(),
                    device: fx.getDevice(),
                    os: fx.getOS()
                },
                type: 'post'
            }).done(function (result) {
                if (result.success) {
                    $submit.val('登陆成功,正在跳转...');
                    fx.setCurrentPanelId('e45c7a460f574d08baccea28f2ec49b9');
                    window.location.href = result.url;
                } else {
                    $submit.val('登录').removeAttr('disabled');
                    $account.removeAttr('disabled');
                    $password.removeAttr('disabled');
                    msg(result.message);
                }
            }).fail(function (request) {
                $submit.val('登录').removeAttr('disabled');
                $account.removeAttr('disabled');
                $password.removeAttr('disabled');
                if (request.responseJSON) {
                    msg(request.responseJSON.message);
                }
                else {
                    msg(request.statusText);
                }
            });
        };

        /**
         * 绑定控件事件
         */
        var bindEvent = function () {
            $submit.click(function () {
                loginCore();
            });

            $account.keydown(function (e) {
                if (e.keyCode == 13) {
                    if ($(this).val().length == 0) {
                        msg('请输入账号');
                    } else {
                        $password.focus();
                    }
                }
            });

            $password.keydown(function (e) {
                if (e.keyCode == 13) {
                    loginCore();
                } else if (e.keyCode == 27) {
                    $account.focus();
                }
            });
        };


        return {
            init: function () {
                $account = $('#account');
                $password = $('#password');
                $submit = $(':submit');
                setBackground();
                bindEvent();
            }
        };

    }();

    $(document).ready(function () {
        Login.init();
    });

})(jQuery, window);




