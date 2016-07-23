(function ($) {
    'use strict';

    //#region 操作类

    var UIForm = (function () {

        var _submit = function (self) {
            var data = {};
            self.$element.find('input[type=checkbox]').each(function () {
                if ($(this).prop('checked') !== true) {
                    var name = $(this).attr('name');
                    data[name] = $(this).prop('checked');
                    // var value = $(this).val();
                    // data[name] = checked;
                }
            });
            $.extend(data, self.options.onData.call(self.element, self.options));

            var ops = $.extend({}, self.options, {
                data: data,
                //beforeSend: function () {
                //},
                error: function (result) {
                    fx.loadMask.hide(self.options);
                    fx.ajaxFailAlert(result);
                },
                //complete: function () {
                //    fx.loadMask.hide(self.options);
                //},
                success: function (result) {
                    fx.loadMask.hide(self.options);
                    self.$element.triggerHandler('submitsuccess', [result]);
                    self.options.onSubmitSuccess.call(self.element, result, self.options);
                }
            });
            fx.loadMask.show(self.options);
            self.$element.ajaxSubmit(ops);
        };

        var _validate = function (self) {
            var $el = $(self.options.container);
            var valid = {
                messages: {},
                rules: {},
                invalidHandler: function () {
                    if (self.successEl) {
                        $el.find(self.successEl).hide();
                    }
                    if (self.errorEl) {
                        $el.find(self.errorEl).show();
                    }
                    if (self.options.modal == true) {
                        self.$element.closest('.modal').animate({ scrollTop: 0 }, 'slow');
                    } else {
                        $(fx.module).animate({ scrollTop: 0 }, 'slow');
                    }
                },
                submitHandler: function () {
                    if (self.successEl) {
                        $el.find(self.successEl).show();
                    }
                    if (self.errorEl) {
                        $el.find(self.errorEl).hide();
                    }

                    if (self.options.onBeforeSubmit.call(self.element, self.options) == false) {
                        return;
                    }

                    _submit(self);
                }
            };

            //转换验证规则
            self.$element.find('[data-validate]').each(function () {
                var name = $(this).attr('name');
                var rule = $(this).data('validate');
                var message = $(this).data('validateMessage');
                if (rule) {
                    valid.rules[name] = rule.toObject();
                }
                if (message) {
                    valid.messages[name] = message.toObject();
                }
            });

            self.$element.validate(valid);

            //#region 控件验证

            if ($().iCheck) {
                self.$element.find('.uicheck').on('ifToggled', function (e) {
                    self.$element.validate().element($(this));
                });
            }

            if ($().select2) {
                self.$element.find('.uiselect').on('change', function () {
                    self.$element.validate().element($(this));
                });
            }

            if ($().clockpicker) {
                self.$element.find('.uiclock').clockpicker().find('input').change(function () {
                    self.$element.validate().element($(this));
                });
            }

            if ($().timepicker) {
                self.$element.find('.uitime').timepicker().on('changeTime.timepicker', function (e) {
                    self.$element.validate().element($(this));
                });
            }

            if ($().datepicker) {
                self.$element.find('.uidate').datepicker().on('changeDate', function (e) {
                    $(this).datepicker('hide');
                    self.$element.validate().element($(this).find('.form-control'));
                });
            }

            if ($().datetimepicker) {
                self.$element.find('.uidatetime').datetimepicker().on('changeDate', function (e) {
                    self.$element.validate().element($(this));
                });
            }

            //#endregion
        };

        //#endregion

        //#region 基础函数

        /**
         * 界面初始化构造函数
         * @param element 选择元素
         * @param options 配置选项
         */
        function UIForm(element, options) {
            this.element = element;
            this.$element = $(element);
            this.options = options;
            this.init();
        }

        /**
         * 组件初始化
         * @returns {void}
         */
        UIForm.prototype.init = function () {
            if (!this.options.container) {
                this.options.container = this.element;
            }
            if (this.options.modal) {
                this.options.container = this.$element.getFormModal()[0];
            }
            if (!this.options.loadMaskTarget) {
                if (this.options.modal) {
                    this.options.loadMaskTarget = this.$element.closest('.modal-content');
                } else {
                    this.options.loadMaskTarget = this.options.container;
                }
            }
            _validate(this);
        };

        /**
         * 设置控件配置属性
         * @param ops 配置属性
         */
        UIForm.prototype.setOptions = function (ops) {
            if (typeof ops === 'object') {
                $.extend(this.options, ops);
            }
        };

        /**
         * 序列化表单值
         */
        UIForm.prototype.formSerialize = function () {
            return $element.formSerialize();
        };

        /**
         * 重置表单
         */
        UIForm.prototype.resetForm = function () {
            $element.resetForm();
        };

        /**
         * 清空表单
         */
        UIForm.prototype.clearForm = function () {
            $element.clearForm();
        };

        //#endregion

        return UIForm;
    })();

    //#endregion

    //#region 插件

    /**
     * 控件初始化
     * @param options 配置选项
     * @returns {UIForm}
     */
    $.fn.uiform = function (options) {
        this.each(function () {
            var $element = $(this);
            var instance = $element.data('uiform');
            if (!instance) {
                var ops = $.extend({}, $.fn.uiform.defaults, $.fn.uiform.parseOptions($element), options);
                instance = new UIForm(this, ops);
                $element.data('uiform', instance);
            } else {
                instance.setOptions(options);
            }
        });
        return $(this).data('uiform');
    };

    /**
     * 解析配置项
     * @param $element 目标对象
     * @returns 返回元素data配置选项
     */
    $.fn.uiform.parseOptions = function ($element) {
        fx.convertOptionToFunc($element, ['onData', 'onBeforeSubmit', 'onSubmitSuccess']);
        return $element.data();
    };

    /**
     * 插件默认值
     */
    $.fn.uiform.defaults = $.extend({}, fx.loadMask.defaults, {

        container: null,

        /**
         * 加载提示消息
         */
        loadMaskMessage: '正在提交,请稍等...',

        modal: false,

        errorEl: null,

        successEl: null,

        onData: function (options) {
            return {};
        },

        onSubmitSuccess: function (result, options) {
        },

        onBeforeSubmit: function (options) {
        }
    });

    //#endregion

    //#region validator
    $.validator.setDefaults({
        errorElement: 'span',
        errorClass: 'help-block help-block-error',
        focusInvalid: false,
        ignore: ":hidden",
        errorPlacement: function (error, element) {
            if (element.parent(".input-group").size() > 0) {
                error.insertAfter(element.parent(".input-group"));
            } else if (element.attr("data-error-container")) {
                error.appendTo(element.attr("data-error-container"));
            } else if (element.parents('.icheck-list').size() > 0) {
                error.appendTo(element.parents('.icheck-list'));
            } else if (element.parents('.icheck-inline').size() > 0) {
                error.appendTo(element.parents('.icheck-inline'));
            } else if (element.parents('.radio-list').size() > 0) {
                error.appendTo(element.parents('.radio-list'));
            } else if (element.parents('.iradio-inline').size() > 0) {
                error.appendTo(element.parents('.iradio-inline'));
            } else if (element.is('.uiselect')) {
                error.appendTo(element.parent());
            } else if (element.is('.uifileupload')) {
                error.appendTo(element.closest('.file-input'));
            } else {
                error.insertAfter(element);
            }
        },
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        success: function (label) {
            label.closest('.form-group').removeClass('has-error');
            label.remove();
        }
    });
    //#endregion

    //#region 初始化

    $(document).on(fx.initUIEventName, function (e) {
        $(e.target).find('.uiform').uiform();
    });

    //#endregion

})(jQuery);