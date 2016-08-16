(function ($) {
    'use strict';

    //#region UITable类

    var UITable = (function () {

        function _loadData(instance) {

            var e = $.Event('loading');
            instance.$element.triggerHandler(e, [this]);
            if (e.result === false) {
                return;
            }

            if (instance.options.onBeforeLoad.call(instance.element,instance.options) == false) {
                return;
            }

            var ajaxParam = $.extend({}, {
                page: instance.options.page,
                order: instance.options.order,
                dir: instance.options.dir
            }, instance.options.params);

            $.ajax({
                url: instance.options.url,
                type: instance.options.type,
                data: ajaxParam,
                beforeSend: function () {
                    fx.loadMask.show(instance.options);
                },
                success: function (result) {
                    fx.loadMask.hide(instance.options);
                    _callback(instance, result);
                },
                error: function (result) {
                    fx.loadMask.hide(instance.options);
                    instance.$element.empty();
                    fx.ajaxFail(result, instance.$element);
                }
            });
        }

        function _callback(instance, result) {
            instance.$element.empty();
            instance.$element.append(result);
            instance.$table = instance.$element.find('table');
            instance.$paginate = instance.$element.find('.paginate-area');
            instance.$checkboxs = instance.$table.find('tbody .checkbox-check');
            instance.$element.find('[data-toggle="tooltip"]').tooltip();

            instance.$checkboxs.on('change', function () {
                var checked = $(this).prop('checked');
                if (checked) {
                    $(this).closest('tr').addClass('warning');
                } else {
                    $(this).closest('tr').removeClass('warning');
                }
            });

            instance.$table.find('thead .checkbox-check').on('change', function () {
                var checked = $(this).prop('checked');
                instance.$checkboxs.each(function () {
                    if (checked) {
                        $(this).prop('checked', true);
                        $(this).closest('tr').addClass('warning');
                    } else {
                        $(this).prop('checked', false);
                        $(this).closest('tr').removeClass('warning');
                    }
                });
            });

            //分页事件
            instance.$paginate.find('ul>li:not(.active,.disabled)>a').on('click', function () {
                instance.options.page = $(this).data('page');
                _loadData(instance);
            }).css('cursor', 'pointer');

            //排序事件
            instance.$table.find('thead>tr>th[data-order]').each(function () {
                var order = $(this).data('order');
                if (instance.options.order === order) {
                    if (instance.options.dir === 'asc') {
                        $(this).append(' <i class="fa fa-long-arrow-down" title="升序排序"></i>');
                    }
                    else if (instance.options.dir === 'desc') {
                        $(this).append(' <i class="fa fa-long-arrow-up" title="倒序排序"></i>');
                    }
                } else {
                    //$(this).append(' <i class="glyphicon glyphicon-sort" style="opacity: 0.2" title="点击排序"></i>');
                }
            }).on('click', function () {
                instance.options.order = $(this).data('order');
                if (instance.options.dir) {
                    if (instance.options.dir === 'asc') {
                        instance.options.dir = 'desc';
                    } else {
                        instance.options.dir = 'asc';
                    }
                } else {
                    instance.options.dir = 'asc';
                }
                _loadData(instance);
            }).css('cursor', 'pointer');

            //添加没有数据提示
            _noDataAlert(instance);

            instance.$element.triggerHandler('loaded', [this]);
            instance.options.onAfterLoad.call(instance.element,instance.options);
        }

        function _noDataAlert(instance) {
            if (instance.options.alert == true) {
                var hasData = instance.$table.find('tbody tr').length > 0;
                if (hasData == false) {
                    instance.$element.empty();
                    instance.$element.showWarning('没有查询到数据！');
                }
            }
        }

        //#endregion

        //#region 基础函数

        /**
         * 插件初始化构造函数
         * @param element 选择元素
         * @param options 配置选项
         * @constructor
         */
        function UITable(element, options) {
            this.options = options;
            this.element = element;
            this.$element = $(element);
            this.$table = null;
            this.$paginate = null;
            this.$checkboxs = null;
            this.init();
        }

        /**
         * 组件初始化
         */
        UITable.prototype.init = function () {
            if(!this.options.loadMaskTarget){
                this.options.loadMaskTarget = this.element;
            }
            if (!this.options.params) {
                this.options.params = {};
            }

            this.initSearchForm();

            if (this.options.autoLoad == true) {
                if (this.$element.find('table').length == 0) {
                    _loadData(this);
                } else {
                    _callback(this, this.$element.html());
                }
            }
        };

        UITable.prototype.initSearchForm = function () {
            //绑定表单查询事件
            var self = this;
            if (self.options.form) {
                var $queryForm = $(this.options.form);
                $queryForm.find(':submit:first').on('click', function () {
                    var ps = fx.serializeForm($queryForm);
                    self.search(ps);
                    return false;
                });
            }
        };

        /**
         * 设置控件配置属性
         * @param ops 配置属性
         */
        UITable.prototype.setOptions = function (ops) {
            if (typeof ops === 'object') {
                $.extend(this.options, ops);
            }
        };

        /**
         * 获取勾选的主键数组
         */
        UITable.prototype.getSelectedIds = function () {
            var sz = [];
            this.$checkboxs.each(function () {
                if (this.checked) {
                    sz.push(this.value);
                }
            });
            return sz;
        };

        /**
         * 数据排序
         * @param name 排序字段名称
         * @param dir 排序方式(asc,desc)
         */
        UITable.prototype.sort = function (name, dir) {
            if (!dir) {
                dir = 'asc';
            }
            this.options.order = name;
            this.options.dir = dir;
            _loadData(this);
        };

        /**
         * 查询数据(更改页面为第一页)
         * @param extraQueryParams 传递的参数
         */
        UITable.prototype.search = function (extraQueryParams) {
            this.options.page = 1;
            $.extend(this.options.params, extraQueryParams);
            _loadData(this);
        };

        /**
         * 重新加载数据
         * @param extraQueryParams 传递的参数
         */
        UITable.prototype.reload = function (extraQueryParams) {
            $.extend(this.options.params, extraQueryParams);
            _loadData(this);
        };

        /**
         * 清除当前表格数据行
         */
        UITable.prototype.clearRows = function () {
            this.$table.find('tbody').empty();
            this.$paginate.empty();
            _noDataAlert(this);
        };

        /**
         * 获取当前表格行数
         */
        UITable.prototype.getRowCount = function () {
            if (this.$table) {
                return this.$table.find('tbody tr').length;
            }
            return 0;
        };

        //#endregion

        return UITable;
    })();

    //#endregion

    //#region 插件

    /**
     * 插件初始化,并返回第一个实例对象
     * @param options 配置选项
     * @returns {UITable}
     */
    $.fn.uitable = function (options) {
        this.each(function () {
            var $element = $(this);
            var instance = $element.data('uitable');
            if (!instance) {
                var ops = $.extend({}, $.fn.uitable.defaults, $.fn.uitable.parseOptions($element), options);
                instance = new UITable(this, ops);
                $element.data('uitable', instance);
            } else {
                instance.setOptions(options);
            }
        });
        return $(this).data('uitable');
    };

    /**
     * 解析配置项
     * @param {jQuery} $element 目标对象
     * @returns {Object} 返回元素data配置选项
     */
    $.fn.uitable.parseOptions = function ($element) {
        fx.convertOptionToObj($element, ['params']);
        fx.convertOptionToFunc($element, ['onBeforeLoad', 'onAfterLoad']);
        return $element.data();
    };

    /**
     * 插件默认值
     */
    $.fn.uitable.defaults = $.extend({}, fx.loadMask.defaults,{

        container: fx.module,

        /**
         * 加载提示消息
         */
        loadMaskMessage: '正在加载数据,请稍等...',

        /**
         * 自动加载数据
         */
        autoLoad: true,

        /**
         * 没有数据时自动添加提示
         */
        alert: true,

        /**
         * 默认为 "Post",请求方式 ("POST" 或 "GET")
         */
        type: 'post',

        /**
         * 表格地址
         */
        url: null,

        /**
         * 页码
         */
        page: 1,

        /**
         * 排序字段名称
         */
        order: null,

        /**
         * 排序方式:asc,desc
         */
        dir: null,

        /**
         * 表单对象
         */
        form: '.search-table',

        /**
         * 数据加载前执行
         */
        onBeforeLoad: function (options) {
        },

        /**
         * 数据加载后执行
         */
        onAfterLoad: function (options) {
        }
    });

    //#endregion

    $(document).on(fx.initUIEventName, function (e) {
        $(e.target).find('.uitable').uitable();
    });

})(jQuery);