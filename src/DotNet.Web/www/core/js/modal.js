(function ($) {
    'use strict';

    //#region 操作类

    var UIModal = (function () {

        //#region 构造函数

        /**
         * 界面初始化构造函数
         * @param element 控件元素
         * @param options 配置选项
         * @constructor
         */
        function UIModal(element, options) {
            this.element = element;
            this.$element = $(element);
            this.options = options;
            this.init();
        }

        //#endregion

        //#region 公共函数

        UIModal.prototype.init = function () {
            if (!this.options.loadMaskTarget) {
                this.options.loadMaskTarget = fx.module;
            }
        };

        /**
         * 设置控件配置属性
         * @param ops 配置属性
         */
        UIModal.prototype.setOptions = function (ops) {
            if (typeof ops === 'object') {
                $.extend(this.options, ops);
            }
        };

        /**
         * 显示模式窗口
         */
        UIModal.prototype.show = function () {
            var self = this;
            if (!self.options.data) {
                self.options.data = {};
            }
            if (self.options.onBeforeShow.call(self.element, self.options) == false) {
                self.$element.remove();
                return;
            }

            var e = $.Event('beforeshow');
            self.$element.triggerHandler(e, [self]);
            if (e.result == false) {
                self.$element.remove();
                return;
            }

            var animateDom = '', sizeDom = '';
            if (self.options.animate == true) {
                animateDom = 'fade ';
            }
            if (self.options.size == 'sm') {
                sizeDom = 'modal-sm';
            } else if (self.options.size == 'lg') {
                sizeDom = 'modal-lg';
            }else if (self.options.size == 'full') {
                sizeDom = 'modal-full';
            }else if (self.options.size == 'big') {
                sizeDom = 'modal-big';
            }

            if (self.options.class) {
                animateDom += self.options.class;
            }
            var html = '<div class="modal ' + animateDom + '"'
                + ' aria-hidden="true"  role="dialog" data-backdrop="' + self.options.backdrop + '">'
                + '    <div class="modal-dialog ' + sizeDom + ' ">'
                + '        <div class="modal-content">'
                + '        </div>'
                + '    </div>'
                + '</div>';
            if (self.options.autoHead==true) {
                self.$element.append(html);
            }

            
            fx.loadMask.show(self.options);
             
            var ajaxSuccess = function (result) {
                if (self.options.autoHead == true) {
                    self.$modal = self.$element.find('.modal');
                    self.$modal.find('.modal-content').empty().html(result);
                } else {
                    self.$element.empty().html(result);
                    self.$modal = self.$element.find('.modal');
                }

                self.$modal.onAfterInitUI(function () {
                    fx.loadMask.hide(self.options);
                    self.options.onAfterShow.call(self.element, self.options);
                    self.$element.triggerHandler($.Event('aftershow'),[self]);
                });

                self.$modal.initUI();
                self.$modal.on('hidden.bs.modal', function () {
                    self.options.onBeforeClose.call(self.element, self.options);
                    self.$element.find('.uidatetime').datetimepicker('remove');
                    self.$element.remove();
                    self.options.onAfterClose.call(self.element, self.options);
                });
                self.$modal.modal('show');
            };

            $.ajax({
                url: self.options.url,
                data: $.extend({}, self.options.data, self.options.onData.call(self.element, self.options)),
                type: 'get',
                cache: false,
                dataType: 'html'
            }).done(ajaxSuccess).fail(function (result) {
                fx.loadMask.hide(self.options);
                fx.ajaxFailAlert(result);
            });
        };

        /**
         * 关闭模式窗口
         */
        UIModal.prototype.close = function () {
            this.$modal.modal('hide');
        };

        //#endregion

        return UIModal;
    })();

    //#endregion

    //#region 插件

    /**
     * 控件初始化
     * @param options 配置选项
     * @returns {UIModal}
     */
    $.fn.uimodal = function (options) {
        this.each(function () {
            var $element = $(this);
            var instance = $element.data('uimodal');
            if (!instance) {
                var ops = $.extend({}, $.fn.uimodal.defaults, options);
                instance = new UIModal(this, ops);
                $element.data('uimodal', instance);
            } else {
                instance.setOptions(options);
            }
        });
        return $(this).data('uimodal');
    };

    /**
     * 解析配置项
     * @param $element 目标对象
     * @returns 返回元素data配置选项
     */
    $.fn.uimodal.parseOptions = function ($element) {
        fx.convertOptionToObj($element, ['data']);
        fx.convertOptionToFunc($element, ['onData', 'onBeforeShow', 'onAfterShow', 'onBeforeClose', 'onAfterClose']);
        return $element.data();
    };

    /**
     * 插件默认值
     */
    $.fn.uimodal.defaults = $.extend({}, fx.loadMask.defaults, {

        container: fx.module,

        autoHead:true,

        /**
         * 模式窗口Id
         */
        modalId:null,

        /**
         * 加载提示消息
         */
        loadMaskMessage: '正在加载,请稍等...',

        /**
         * 是否显示动画效果
         */
        animate: true,

        /**
         * 点击背景是否关闭,默认为不关闭(static)
         */
        backdrop: 'static',

        /**
         * 远程加载Url
         */
        url: null,

        /**
         * 尺寸大小:默认为正常大小,大 lg 小 sm 全屏 full big 比lg大
         */
        size: null,

        onData: function () {
            return {};
        },

        /**
         * 模式窗口显示之前事件,return false可以阻止窗口显示。
         */
        onBeforeShow: function () {
        },

        /**
         * 模式窗口显示之后事件,用于初始化窗口控件。
         */
        onAfterShow: function () {
        },

        /**
         * 模式窗口关闭之前事件。
         */
        onBeforeClose: function () {
        },

        /**
         * 模式窗口关闭之后事件。
         */
        onAfterClose: function () {
        }
    });

    //#endregion

    $(document).on('click.fx.modal', '.uimodal', function () {
        var $element = $(this);
        var el = $element.data('container') || fx.module;
        var modalId = $element.data('modalId');
        var idDom = '';
        if (modalId) {
            idDom = 'id="' + modalId + '"';
        }
        var instance = $('<div ' + idDom + ' class="uimodal-container"></div>')
            .appendTo($(el)).uimodal($.fn.uimodal.parseOptions($element));
        $element.data('uimodal', instance);
        instance.show();
        return false;
    });

})(jQuery);