(function ($) {
    'use strict';
    $('body').fadeIn(1000);

    //window.onbeforeunload = function (event) {
    //    var event = event || window.event;
    //    // 兼容IE8和Firefox 4之前的版本
    //    if (event) {
    //        event.returnValue = "确定要关闭窗口吗？";
    //    }
    //    // Chrome, Safari, Firefox 4+, Opera 12+ , IE 9+
    //    return '确定要关闭窗口吗>现代浏览器？';
    //}

    fx.home = function () {

        /**
         * 初始化系统状态信息
         */
        var initStatusMessage = function () {
            var $status = $('.home-south .message');
            $status.find('.status-system').html(fx.getOS());
            $status.find('.status-browser').html(fx.getBrowser());
            $status.find('.status-screen').html(fx.getScreenResolution());
        };

        /**
         * 初始化时间
         */
        var initTime = function () {
            var _getTime = function () {
                $('.home-south .time').html(new Date().formatDateTime());
            };
            setInterval(_getTime, 1000);
        };


        /**
         * 初始化导航菜单
         */
        var initTree = function () {
            $('.home-tree').tree({
                method: 'get',
                onClick: function (node) {
                    //if (fx.getCurrentPanelId() == node.id) return;
                    $(this).tree('expand', node.target);
                    fx.loadPage(node);
                },
                onLoadSuccess: function () {
                    var panelId = fx.getCurrentPanelId();
                    if (!panelId) return;
                    var node = $(this).tree('find', panelId);
                    if (!node) return;
                    $(this).tree('expandTo', node.target);
                    $(this).tree('select', node.target);
                    fx.loadPage(node);
                }
            });
        };

        var initLogout = function () {
            $('.logout-action').click(function (e) {
                e.preventDefault();
                e.stopPropagation();
                var url = $(this).data('url');
                fx.confirm('确定要退出系统吗?', function () {
                    fx.setCurrentPanelId(null);
                    window.location.href = url;
                });
            });
        };

        return {
            /**
             * 初始化主界面
             */
            init: function () {
                initTree();
                initTime();
                initStatusMessage();
                initLogout();
                fx.monitorPanelWidth('homeWestWidth', $('body'), 'west');
            }
        };

    }();

    $(document).ready(function () {
        fx.home.init();
    });

})(jQuery);