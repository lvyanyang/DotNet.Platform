var fx = {};
(function ($, fx) {
    'use strict';

    var loaded = [], promise = false, deferred = $.Deferred();
    var REG_BODY = /<body[^>]*>([\s\S]*)<\/body>/;
    var pageRes = [];

    /**
     * 动态加载js脚本
     * @param src 脚本Url
     */
    var loadScript = function (src) {
        if (loaded[src]) return loaded[src].promise();

        var deferred = $.Deferred();
        var script = document.createElement('script');
        script.src = src;
        script.onload = function (e) {
            deferred.resolve(e);
        };
        script.onerror = function (e) {
            deferred.reject(e);
        };
        document.body.appendChild(script);
        loaded[src] = deferred;

        return deferred.promise();
    };

    /**
     * 动态加载css样式
     * @param href 样式Url
     */
    var loadCSS = function (href) {
        if (loaded[href]) return loaded[href].promise();

        var deferred = $.Deferred();
        var style = document.createElement('link');
        style.rel = 'stylesheet';
        style.type = 'text/css';
        style.href = href;
        style.onload = function (e) {
            deferred.resolve(e);
        };
        style.onerror = function (e) {
            deferred.reject(e);
        };
        document.head.appendChild(style);
        loaded[href] = deferred;

        return deferred.promise();
    };

    /**
     * 动态加载资源(css或者js)
     * @param srcs 资源文件路径数组
     * @returns {deferred} 延迟对象
     */
    var loadRes = function (srcs) {
        srcs = $.isArray(srcs) ? srcs : srcs.split(/\s+/);
        if (!promise) {
            promise = deferred.promise();
        }

        $.each(srcs, function (index, src) {
            promise = promise.then(function () {
                return src.indexOf('.css') >= 0 ? loadCSS(src) : loadScript(src);
            });
        });
        deferred.resolve();
        return promise;
    };

    $.extend(fx, {

        /**
         * 当前模块容器
         */
        module: '.home-main',
        initUIEventName: 'init.fx.ui',
        beforeInitUIEventName: 'before.fx.ui',
        afterInitUIEventName: 'after.fx.ui',
        destroyModuleEventName: 'destroy.fx.module',

        /**
         * 按键编码
         */
        keyCode: {
            enter: 13,
            esc: 27,
            end: 35,
            home: 36,
            shift: 16,
            ctrl: 17,
            tab: 9,
            left: 37,
            right: 39,
            up: 38,
            down: 40,
            delete: 46,
            backspace: 8
        },

        /**
         * 状态码
         */
        statusCode: {
            ok: 200,
            error: 300,
            timeout: 301
        },

        /**
         * 当前页面模块UI初始化完成事件
         * @param resArray 资源文件数组
         * @param callback 回调函数
         */
        onReady: function (resArray, callback) {
            var self = this;
            if (typeof resArray === 'function') {
                callback = resArray;
                resArray = null;
            } else {
                pageRes = resArray;
            }
            $(self.module).on(self.afterInitUIEventName, callback);
        },

        /**
         * 注册页面销毁事件
         * @param callback 回调函数
         */
        onModuleDestroy: function (callback) {
            $(this.module).on(this.destroyModuleEventName, callback);
        },

        /**
         * 加载面板
         * @param {object} node 节点对象
         */
        loadPage: function (node) {
            var id = node.id;
            var text = node.text;
            var url = node.url;
            var iconCls = node.iconCls;

            if (!url) return;
            this.setCurrentPanelId(id);
            $(fx.module).trigger(fx.destroyModuleEventName);
            $(fx.module).off(fx.destroyModuleEventName);
            $(fx.module).off(fx.afterInitUIEventName);
            $(fx.module).panel({
                id: id,
                title: text,
                iconCls: iconCls,
                href: url,
                onLoadError: function (result) {
                    var self = this;
                    try {
                        var r = $.parseJSON(result.responseText);
                        if (r) {
                            $(self).showDanger(r.message+ '  ' +result.statusText + '(' + result.status + ')');
                            //fx.alert(r.message, null, { title: result.statusText + '(' + result.status + ')' });
                        }
                    } catch (e) {
                        fx.loadResource(pageRes, function () {
                            $(self).append(result.responseText).initUI();
                        });
                    }
                },
                onLoad: function () {
                    var self = this;
                    fx.loadResource(pageRes, function () {
                        $(self).initUI();
                    });
                }
            });
        },

        /**
         * 设置当前面板Id,如果panelId为null则删除
         * @param {string} panelId 面板Id
         */
        setCurrentPanelId: function (panelId) {
            if (panelId === null) {
                $.sessionStorage.remove('currentPanelId');
            }
            else {
                $.sessionStorage.set('currentPanelId', panelId);
            }
        },

        /**
         * 获取当前面板Id
         */
        getCurrentPanelId: function () {
            return $.sessionStorage.get('currentPanelId');
        },


        /**
         * 获取当前屏幕分辨率
         */
        getScreenResolution: function () {
            return window.screen.width + '×' + window.screen.height;
        },

        /**
         * 获取当前操作系统
         */
        getOS: function () {
            var result = new userAgent(navigator.userAgent);
            return result.os.name + ' ' + result.os.version.original;
        },

        /**
         * 获取当前浏览器
         */
        getBrowser: function () {
            var result = new userAgent(navigator.userAgent);
            return result.browser.name + ' ' + (result.browser.version.major || result.browser.version.original);
        },
        /**
         * 获取设备信息
         */
        getDevice: function () {
            var result = new userAgent(navigator.userAgent);
            if (result.device.type.toLowerCase() == 'mobile') {
                return result.device.model.toLowerCase();
            }
            return 'pc';
        },
        /**
         * 序列化表单字段值为Json对象
         * @param {jQuery} $form 表单JObject
         * @returns {Object} 返回Json对象
         */
        serializeForm: function ($form) {
            var o = {}
            var a = $form.serializeArray();
            $.each(a, function () {
                if (o[this.name] !== undefined) {
                    if (!o[this.name].push) {
                        o[this.name] = [o[this.name]];
                    }
                    o[this.name].push(this.value || '');
                } else {
                    o[this.name] = this.value || '';
                }
            });

            $form.find('input[type=checkbox]').each(function () {
                var name = $(this).attr('name');
                var value = $(this).attr('value');
                //var checked = $(this).prop('checked');
                o[name] = value;
            });

            return o;
        },

        /**
         * 转换data属性(字符串)为object
         * @param {jQuery} $element 元素对象
         * @param {Array} names 属性名称数组
         */
        convertOptionToObj: function ($element, names) {
            for (var i = 0; i < names.length; i++) {
                var name = names[i];
                var value = $element.data(name);
                if (value !== undefined && typeof value == 'string') {
                    $element.data(name, value.toObject());
                }
            }
        },

        /**
         * 获取data属性(字符串)为object
         * @param {jQuery} $element 元素对象
         * @param {Array} names 属性名称数组
         */
        getOptionToObj: function ($element, name) {
            var value = $element.data(name);
            if (value !== undefined && typeof value == 'string') {
                return value.toObject();
            }
            return undefined;
        },

        /**
         * 转换data属性(字符串)为function
         * @param {jQuery} $element 元素对象
         * @param {Array} names 属性名称数组
         */
        convertOptionToFunc: function ($element, names) {
            for (var i = 0; i < names.length; i++) {
                var name = names[i];
                var value = $element.data(name);
                if (value !== undefined && typeof value == 'string') {
                    $element.data(name, value.toFunction());
                }
            }
        },

        /**
         * 获取data属性(字符串)为function
         * @param {jQuery} $element 元素对象
         * @param {Array} names 属性名称数组
         */
        getOptionToFunc: function ($element, name) {
            var value = $element.data(name);
            if (value !== undefined && typeof value == 'string') {
                return value.toFunction();
            }
            return undefined;
        },

        /**
         * 转换data属性(字符串)为事件function
         * @param {jQuery} $element 元素对象
         * @param {Object} defaults 默认对象
         */
        convertOptionToEvent: function ($element) {
            var datas = $element.data();
            $.each(datas, function (key) {
                if (key.indexOf('on') > -1) {
                    var value = $element.data(key);
                    if (value !== undefined && typeof value == 'string') {
                        $element.data(key, value.toFunction());
                    }
                }
            });
        },

        /**
         * 动态加载资源(css或者js)
         * @param {Array} resPaths 资源文件数组
         * @param {Function} callback 加载完成后的回调函数
         */
        loadResource: function (resPaths, callback) {
            loadRes(resPaths).then(callback);
        },

        /**
         * 创建UUID通用唯一识别码 (Universally Unique Identifier)
         */
        uuid: (function (uuidRegEx, uuidReplacer) {
            return function () {
                return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(uuidRegEx, uuidReplacer).toLowerCase();
            };
        })(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0,
                v = c == 'x' ? r : (r & 3 | 8);
            return v.toString(16);
        }),

        /**
         * 正则匹配获取body之间的字符串
         * @param content 待截取的字符串
         * @returns {*}
         */
        getBodyString: function (content) {
            var result = REG_BODY.exec(content);
            if (result && result.length === 2)
                return result[1];
            return content;
        },

        /**
         * 生成随机数(从1-max之间)
         * @param {Number} max 指定最大数
         * @returns {Number} 返回从1-max之间的随机数
         */
        getRandomNumber: function (max) {
            return Math.floor(Math.random() * max + 1);
        },

        /**
         * 关闭chrome浏览器
         */
        closeChrome: function () {
            var browserName = navigator.appName;
            if (browserName === 'Netscape') {
                window.open('', '_self', '');

                if (window.opener != undefined) {
                    //for chrome
                    window.opener.returnValue = '1';
                }
                else {
                    window.returnValue = '1';
                }
                window.close();
            }
            else {
                window.close();
            }
        },

        /**
         * 返回指定长度的字符串
         * @param {String} str 字符串
         * @param {Number} totalLength 字符串总长度
         * @param {String} defaultString 字符不够时的默认字符
         */
        fixLenString: function (str, totalLength, defaultString) {
            var result = str;
            var times = totalLength - (result.length);
            for (var i = 1; i <= times; i++) {
                result = defaultString + result;
            }
            return result;
        },

        /**
         * 显示加载提示
         */
        mask: function (options) {
            if (!options) {
                options = {};
            }
            if (options.masked == false) {
                return;
            }
            //if (!options.delay) {
            //    options.delay = 100;
            //}
            if (!options.mask) {
                options.mask = '正在提交请求,请稍等...';
            }
            if (options.maskTarget) {
                $(options.maskTarget).mask(options.mask, options.delay);
            } else {
                $('body').mask(options.mask, options.delay);
            }
        },

        /**
         * 隐藏加载提示
         */
        unmask: function (options) {
            if (!options) {
                options = {};
            }
            if (options.masked == false) {
                return;
            }
            if (options.maskTarget) {
                $(options.maskTarget).unmask();
            } else {
                $('body').unmask();
            }
        },

        /**
         * 发送ajax请求
         * @param {Object} ops 
         */
        ajax: function (ops) {
            if (!ops.url) {
                fx.alert('请指定提交Url');
                return;
            }

            var options = $.extend({
                url: ops.url,
                type: 'post',
                cache: false,
                //dataType: 'json',
                beforeSend: function () {
                    fx.mask(options);
                },
                complete: function () {
                    fx.unmask(options);
                },
                error: function (result) {
                    fx.ajaxFailAlert(result);
                },
                success: function (result) {
                    if (this.dataTypes[1] == 'json' && result.message) {
                        fx.alert(result.message);
                    } else if (this.dataTypes[1] == 'html') {
                        alert(result);
                    }
                }
            }, ops);

            if (ops.confirm) {
                fx.confirm(ops.confirm, function (index) {
                    layer.close(index);
                    $.ajax(options);
                });
            } else {
                $.ajax(options);
            }
        },

        /**
         * 显示模式窗口
         * @param {Object} options 配置选项
         */
        window: function (options) {
            if (!options.mask) {
                options.mask = '正在加载,请稍等...';
            }

            var autoTpl = function (content) {
                var sizeDom = '';
                if (options.size == 'sm') {
                    sizeDom = 'modal-sm';
                } else if (options.size == 'lg') {
                    sizeDom = 'modal-lg';
                } else if (options.size == 'full') {
                    sizeDom = 'modal-full';
                } else if (options.size == 'big') {
                    sizeDom = 'modal-big';
                }
                var tpl =
                    '<div class="modal fade" aria-hidden="true" role="dialog" data-backdrop="true">' +
                    '   <div class="modal-dialog ' + sizeDom + '">' +
                    '       <div class="modal-content">';
                if (options.title) {
                    tpl +=
                    '           <div class="modal-header">' +
                    '               <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
                    '               <h4 class="modal-title"> ' + options.title + ' </h4>' +
                    '           </div>';
                }
                tpl +=
                '           <div class="modal-body">' + content + '</div>' +
                '           <div class="modal-footer"><button type="button" class="btn btn-default" data-dismiss="modal"><i class="fa fa-sign-in"></i> 关闭</button></div>' +
                '       </div>' +
                '   </div>' +
                '</div>';
                return tpl;
            };

            var success = function (result) {
                var $element = $('<div class="uiwindow-container"></div>');
                if (!$(result).is('.modal')) {
                    result = autoTpl(result);
                }
                $element.html(result);
                var $modal = $element.find('.modal');
                $element.appendTo('body');

                //$modal.on('shown.bs.modal', function () {});
                $modal.on('hidden.bs.modal', function () {
                    $modal.triggerHandler('hide');
                    if (options.hide) {
                        options.hide.call($modal);
                    }
                    if ($().datetimepicker) {
                        $modal.find('.uidatetime').datetimepicker('remove');
                    }
                    $element.remove();
                });
                $modal.onAfterInitUI(function () {
                    $modal.triggerHandler('show');
                    fx.unmask(options);
                    if (options.show) {
                        options.show.call($modal);
                    }
                });

                $modal.initUI();
                $modal.modal('show');
            };

            if (!options.content) {
                if (!options.url) {
                    var msg = '请指定提交Url';
                    console.error(msg);
                    fx.alert(msg);
                    return;
                }
                $.ajax({
                    url: options.url,
                    data:options.data,
                    type: options.type || 'get',
                    cache: options.cache || false,
                    dataType: options.dataType || 'html',
                    beforeSend: function () {
                        fx.mask(options);
                    },
                    complete: function () {

                    },
                    error: function (result) {
                        fx.unmask(options);
                        fx.ajaxFailAlert(result);
                    },
                    success: success
                });
            } else {
                if (typeof options.content === 'object' && options.content.length > 0) {
                    success(options.content.html());
                } else {
                    success(options.content);
                }
            }
        },
        serverResult: function (items) {
            var tpl =
                    '<div class="uiwindow-container">' +
                    '<div class="modal fade" aria-hidden="true" role="dialog" data-backdrop="true">' +
                    '   <div class="modal-dialog modal-lg">' +
                    '       <div class="modal-content">' +
                    '           <div class="modal-header">' +
                    '               <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
                    '               <h4 class="modal-title"> 应用程序执行结果 </h4>' +
                    '           </div>' +
                    '           <div class="modal-body"></div>' +
                    '           <div class="modal-footer"><button type="button" class="btn btn-default" data-dismiss="modal"><i class="fa fa-sign-in"></i> 关闭</button></div>' +
                    '       </div>' +
                    '   </div>' +
                    '</div>' +
                    '</div>';
            var $element = $(tpl).appendTo('body');
            var $modal = $element.find('.modal');
            var $mbody = $modal.find('.modal-body');
            $modal.on('hidden.bs.modal', function () {
                $element.remove();
            });

            var successItems = [];
            var failureItems = [];
            $.each(items, function (i, v) {
                if (v.success == true) {
                    successItems.push(v);
                } else {
                    failureItems.push(v);
                }
            });
            $mbody.append('<h4>共操作 {0} 项，成功 {1} 项，失败 {2} 项</h4>'.format(items.length, successItems.length, failureItems.length));
            $mbody.append('<ul class="list-group">');
            $.each(successItems, function (i, v) {
                if (v.message) {
                    $mbody.append('<li class="list-group-item color-success">' + v.message + '</li>');
                }
            });
            $.each(failureItems, function (i, v) {
                if (v.message) {
                    $mbody.append('<li class="list-group-item color-danger">' + v.message + '</li>');
                }
            });
            $mbody.append('</ul>');
            $modal.modal('show');
        },

        /**
         * 信息警告框.
         * @param {String} message 消息内容
         * @param {Function(可选)} callback 回调函数
         * @param {Object(可选)} options 选项配置
         * @returns {void}
         */
        alert: function (message, callback, options) {
            var defaults = {
                title: '系统提示',
                //shift: zeniths.util.isChrome ? -1 : 0,
                icon: 0
            }
            var ops = $.extend({}, defaults, options);
            layer.alert(message, ops, callback);
        },

        /**
         * 询问框
         * @param {String} message 消息内容
         * @param {Function(可选)} yesCallback 确定回调函数
         * @param {Object(可选)} options 选项配置
         * @returns {void}
         */
        confirm: function (message, yesCallback, options) {
            var defaults = {
                title: '系统提示',
                //shift: zeniths.util.isChrome ? -1 : 0,
                icon: 3
            }
            var ops = $.extend({}, defaults, options);

            layer.confirm(message, ops, yesCallback);
        },

        /**
         * 提示框.
         * @param {String} message 消息内容
         * @param {Function(可选)} callback 回调函数
         * @returns {void}
         */
        msg: function (message, callback) {
            layer.msg(message, callback);
        },

        /**
         * 输入框 prompt参数: formType: 0文本,1密码,2多行文本 value: '' 初始时的值 icon:-1 图标序号
         * @param {String} message 输入标题
         * @param {Function(可选)} callback 回调函数
         * @param {Object(可选)} options 选项配置
         * @returns {void}
         */
        prompt: function (message, callback, options) {
            var defaults = {
                title: message
                //shift: zeniths.util.isChrome ? -1 : 0,
            }
            var ops = $.extend({}, defaults, options);
            layer.prompt(ops, callback);
        },

        /**
         * 监视记录布局面板宽度(自动恢复宽度)
         * @param {String} key 存储key名称
         * @param {jQuery} $layout 布局对象
         * @param {String} region 方位,可用值有：'north','south','east','west','center'。
         */
        monitorPanelWidth: function (key, $layout, region) {
            var statusKey = key + 'Status_' + region;
            var $panel = $layout.layout('panel', region);
            var lastWidth = 0;
            if ($.localStorage.isSet(key)) {
                $panel.panel('resize', {
                    width: $.localStorage.get(key)
                });
                $layout.layout('resize');
            }

            if ($.localStorage.isSet(statusKey)) {
                var isExpand = $.localStorage.get(statusKey);
                if (isExpand == true) {
                    setTimeout(function () {
                        $layout.layout('expand', region);
                    }, 1);
                }
                else if (isExpand == false) {
                    setTimeout(function () {
                        $layout.layout('collapse', region);
                    }, 1);
                }
            }

            $layout.layout({
                onCollapse: function () {
                    $.localStorage.set(statusKey, false);
                },
                onExpand: function () {
                    $.localStorage.set(statusKey, true);
                }
            });

            $panel.panel({
                onResize: function (w, h) {
                    if (lastWidth != w) {
                        $.localStorage.set(key, w);
                        lastWidth = w;
                    }
                }
            });
        },

        ajaxFail: function (result, $element) {
            if (!result.responseJSON) {
                $element.showDanger(fx.getBodyString(result.responseText));
            }
            else {
                $element.showDanger(result.responseJSON.message);
            }
        },

        ajaxFailAlert: function (result) {
            if (!result.responseJSON) {
                try {
                    var r = $.parseJSON(result.responseText);
                    if (r) {
                        fx.alert(r.message, null, { title: result.statusText + '(' + result.status + ')' });
                    }
                } catch (e) {
                    layer.open({
                        type: 1,
                        skin: 'layui-layer-rim',
                        area: [($(window).width() / 3 * 2) + 'px', ($(window).height() - 100) + 'px'],
                        title: false,
                        content: fx.getBodyString(result.responseText)
                    });
                }
            }
            else {
                fx.alert(result.responseJSON.message, null, { title: result.statusText + '(' + result.status + ')' });
            }
        },

        maSuccess: function (result) {
            var $modal = $(this).getFormModal();
            if (result.success) {
                $modal.uimodal().close();
                if (result.message) {
                    fx.alert(result.message);
                }
            }
            else {
                var errorEl = $(this).data('errorEl');
                if (errorEl) {
                    var $container = $($(this).uiform().options.container);
                    $container.find(errorEl).html(result.message).show();
                }
                else {
                    fx.alert(result.message);
                }
            }
        },

        mtfSuccess: function (result) {
            var table = $(this).data('table');
            if (!table) {
                fx.alert('请指定data-table属性');
                return;
            }
            var $modal = $(this).getFormModal();
            if (result.success) {
                $modal.uimodal().close();
                $(table).uitable().reload();
            }
            else {
                var errorEl = $(this).data('errorEl');
                if (errorEl) {
                    var $container = $($(this).uiform().options.container);
                    $container.find(errorEl).html(result.message).show();
                }
                else {
                    fx.alert(result.message);
                }
            }
        },
        mtftSuccess: function (result) {
            var tree = $(this).data('tree');
            if (!tree) {
                fx.alert('请指定data-tree属性');
                return;
            }

            var $modal = $(this).getFormModal();
            if (result.success) {
                $modal.uimodal().close();
                $(tree).uitree().reload();
            }
            else {
                var errorEl = $(this).data('errorEl');
                if (errorEl) {
                    var $container = $($(this).uiform().options.container);
                    $container.find(errorEl).html(result.message).show();
                }
                else {
                    fx.alert(result.message);
                }
            }
        },
        boolColumnFormatter: function (value) {
            var className = value ? "checkok" : "checkno";
            return '<div class="' + className + '"><div>';
        }

    });

    fx.loadMask = {
        defaults: {
            /**
             * 是否显示加载提示
             */
            loadMask: true,

            /**
             * 加载提示目标选择器
             */
            loadMaskTarget: null,

            /**
             * 加载提示消息
             */
            loadMaskMessage: '正在加载,请稍等...',

            /**
             * 加载提示显示延迟,默认为200ms
             */
            loadMaskDelay: 200
        },

        /**
         * 显示加载提示
         * @param options 插件配置
         */
        show: function (options) {
            if (options.loadMask == false) return;
            if (!options.loadMaskTarget) {
                options.loadMaskTarget = fx.module;
            }
            $(options.loadMaskTarget).mask(options.loadMaskMessage, options.loadMaskDelay);
        },

        /**
         * 隐藏加载提示
         * @param options 插件配置
         */
        hide: function (options) {
            if (options.loadMask == false) return;
            if (options.loadMaskTarget) {
                $(options.loadMaskTarget).unmask();
            }
        }
    };

    $.fn.extend({

        /**
         * 元素添加动画效果
         * @param aname 动画名称
         */
        animateCss: function (aname,callback) {
            var animationEnd = 'webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend';
            $(this).addClass('animated ' + aname).one(animationEnd, function () {
                $(this).removeClass('animated ' + aname);
                if (callback) {
                    callback.call(this);
                }
            });
        },

        /**
         * 是否是指定的标签
         * @param {String} tagName 标签名称
         * @returns {Boolean} 如果是,返回true
         */
        isTag: function (tagName) {
            if (!tagName) return false;
            if (!$(this).prop('tagName')) return false;
            return $(this)[0].tagName.toLowerCase() == tagName.toLowerCase() ? true : false;
        },

        /**
         * 判断当前元素是否已经绑定某个事件
         * @param {String} eventName
         */
        isBind: function (eventName) {
            var _events = $(this).data('events');
            return _events && eventName && _events[eventName];
        },

        /**
         * 初始化界面UI
         */
        initUI: function () {
            function _ui($element) {
                $.when($element.triggerHandler(fx.beforeInitUIEventName)).done(function () {
                    $element.trigger(fx.initUIEventName);
                });
            }

            function _init($element) {
                $.when(_ui($element)).done(function () {
                    $element.triggerHandler(fx.afterInitUIEventName);
                });
            }

            return this.each(function () {
                _init($(this));
            });
            //return this.each(function () {
            //    var $element = $(this);
            //    $.when($element.trigger(fx.initUIEventName)).done(function () {
            //        $element.triggerHandler(fx.afterInitUIEventName);
            //    });
            //});
        },

        getFormModal: function () {
            return $(this).closest('.uimodal-container');
        },

        /**
         * 获取模式窗口内容区域
         * @returns {} 
         */
        getModalContent: function () {
            return $(this).find('.modal-content');
        },

        onAfterInitUI: function (callback) {
            $(this).on(fx.afterInitUIEventName, callback);
        },

        onSelect2Select: function (callback) {
            $(this).on('select2:select', callback);
        },

        /**
         * 显示成功信息
         * @param {String} msg 消息内容
         */
        showSuccess: function (msg) {
            var html = '<div class="alert alert-success alert-dismissible">';
            html += '<strong><i class="fa-lg fa fa-check-circle"></i> ' + msg + '</strong>';
            html += '</div>';
            $(this).append(html);
        },

        /**
         * 显示提示信息
         * @param {String} msg 消息内容
         */
        showInfo: function (msg) {
            var html = '<div class="alert alert-info alert-dismissible">';
            html += '<strong><i class="fa-lg fa fa-info-circle"></i> ' + msg + '</strong>';
            html += '</div>';
            $(this).append(html);
        },

        /**
         * 显示警告信息
         * @param {String} msg 消息内容
         */
        showWarning: function (msg) {
            var html = '<div class="alert alert-warning alert-dismissible">';
            html += '<strong><i class="fa-lg fa fa-warning"></i> ' + msg + '</strong>';
            html += '</div>';
            $(this).append(html);
        },

        /**
         * 显示错误信息
         * @param {String} msg 消息内容
         */
        showDanger: function (msg) {
            var html = '<div class="alert alert-danger alert-dismissible">';
            html += '<strong>' + msg + '</strong>';
            html += '</div>';
            $(this).append(html);
        }
    });

})(jQuery, fx);


(function ($) {
    'use strict';

    $(document).on('click.fx.ajax', '.tsdelete', function () {
        var self = $(this);
        if (!self.data('url')) {
            fx.msg('请指定data-url属性');
            return false;
        }
        var $table;
        var table = self.data('table');
        if (table) {
            $table = $(table);
        } else {
            $table = self.closest('.uitable');
        }
        var ops = $.extend({
            confirm: '确定要删除当前记录吗?',
            loadMask: true,
            loadMaskDelay: 200,
            loadMaskTarget: $table[0],
            loadMaskMessage: '正在删除数据,请稍等...'
        },self.data());

        var _success = function (result) {
            fx.loadMask.hide(ops);
            if (result.success) {
                $table.uitable().reload();
                var callback = self.data('successCallback');
                if (callback) {
                    if ($.isFunction(callback)) {
                        callback.call(self[0]);
                    } else {
                        callback.toFunction().call(self[0]);
                    }
                }
            }
            else {
                fx.alert(result.message);
            }
        };

        var ajaxOptions = {
            url: ops.url,
            type: 'post',
            cache: false,
            data: self.data('data'),
            dataType: 'json',
            beforeSend: function () {
                fx.loadMask.show(ops);
            },
            error: function (result) {
                fx.loadMask.hide(ops);
                fx.ajaxFailAlert(result);
            },
            // complete: function () {
            //
            // },
            success: _success
        };

        fx.confirm(ops.confirm, function (index) {
            layer.close(index);
            $.ajax(ajaxOptions);
        });

        return false;
    });

    $(document).on('click.fx.ajax', '.tmdelete', function () {
        var self = $(this);
        var table = self.data('table');
        if (!self.data('url')) {
            fx.msg('请指定data-url属性');
            return false;
        }
        if (!table) {
            fx.msg('请指定data-table属性');
            return false;
        }
        var $table = $(table);
        var ids = $table.uitable().getSelectedIds();
        if (ids.length == 0) {
            fx.msg('请选择需要删除的记录');
            return false;
        }

        var ops = $.extend({
            loadMask: true,
            loadMaskDelay: 200,
            loadMaskTarget: $table[0],
            loadMaskMessage: '正在删除数据,请稍等...'
        }, self.data());
        if (!self.data('confirm')) {
            ops.confirm = '确定要删除选中的 <span class="color-danger"> ' + ids.length + '</span> 条记录吗?';
        }

        var data = $.extend({}, self.data('data'), {id: ids.join()});

        var _success = function (result) {
            fx.loadMask.hide(ops);
            if (result.success) {
                $table.uitable().reload();
                var callback = self.data('successCallback');
                if (callback) {
                    if ($.isFunction(callback)) {
                        callback.call(self[0]);
                    } else {
                        callback.toFunction().call(self[0]);
                    }
                }
            }
            else {
                fx.alert(result.message);
            }
        };

        var ajaxOptions = {
            url: ops.url,
            type: 'post',
            cache: false,
            data: data,
            dataType: 'json',
            beforeSend: function () {
                fx.loadMask.show(ops);
            },
            error: function (result) {
                fx.loadMask.hide(ops);
                fx.ajaxFailAlert(result);
            },
            // complete: function () {
            //
            // },
            success: _success
        };

        fx.confirm(ops.confirm, function (index) {
            layer.close(index);
            $.ajax(ajaxOptions);
        });

        return false;
    });

    $(document).on('click.fx.ajax', '.treedelete', function () {
        var self = $(this);
        if (!self.data('url')) {
            fx.msg('请指定data-url属性');
            return false;
        }
        var tree = self.data('tree');
        if (!tree) {
            fx.msg('请指定data-tree属性');
            return false;
        }
        var $tree = $(tree);
        var ops = $.extend({
            confirm: '确定要删除选中的节点以及子节点吗?',
            loadMask: true,
            loadMaskDelay: 200,
            loadMaskTarget: $tree[0],
            loadMaskMessage: '正在删除数据,请稍等...'
        }, self.data());

        var selectedNode = $tree.uitree().getSelected();
        var data = $.extend({}, self.data('data'), { id: selectedNode.id });

        var _success = function (result) {
            fx.loadMask.hide(ops);
            if (result.success) {
                $tree.uitree().remove(selectedNode.target);
                var table = self.data('table');
                if (table) {
                    $(table).uitable().clearRows();
                }
                var callback = self.data('successCallback');
                if (callback) {
                    if ($.isFunction(callback)) {
                        callback.call(self[0]);
                    } else {
                        callback.toFunction().call(self[0]);
                    }
                }
            }
            else {
                fx.alert(result.message);
            }
        };

        var ajaxOptions = {
            url: ops.url,
            type: 'post',
            cache: false,
            data: data,
            dataType: 'json',
            beforeSend: function () {
                fx.loadMask.show(ops);
            },
            error: function (result) {
                fx.loadMask.hide(ops);
                fx.ajaxFailAlert(result);
            },
            // complete: function () {
            //
            // },
            success: _success
        };

        fx.confirm(ops.confirm, function (index) {
            layer.close(index);
            $.ajax(ajaxOptions);
        });

        return false;
    });

})(jQuery);




/* =============================String扩展=============================== */
$.extend(String.prototype, {
    /**
     * 是否正整数
     */
    isPositiveInteger: function () {
        return (new RegExp(/^[1-9]\d*$/).test(this));
    },

    /**
     * 是否是整数
     */
    isInteger: function () {
        return (new RegExp(/^\d+$/).test(this));
    },

    /**
     * 是否数字
     */
    isNumber: function () {
        return (new RegExp(/^([-]{0,1}(\d+)[\.]+(\d+))|([-]{0,1}(\d+))$/).test(this));
    },

    /**
     * 是否包含汉字
     */
    includeChinese: function () {
        return (new RegExp(/[\u4E00-\u9FA5]/).test(this));
    },

    /**
     * 去掉前后空格
     */
    trim: function () {
        return this.replace(/(^\s*)|(\s*$)|\r|\n/g, '');
    },

    /**
     * 是否开头匹配
     * @param {} pattern
     */
    startsWith: function (pattern) {
        return this.indexOf(pattern) === 0;
    },

    /**
     * 是否结束匹配
     * @param {} pattern
     */
    endsWith: function (pattern) {
        var d = this.length - pattern.length;
        return d >= 0 && this.lastIndexOf(pattern) === d;
    },

    replaceSuffix: function (index) {
        return this.replace(/\[[0-9]+\]/, '[' + index + ']').replace('#index#', index);
    },

    replaceSuffix2: function (index) {
        return this.replace(/\-(i)([0-9]+)$/, '-i' + index).replace('#index#', index);
    },

    trans: function () {
        return this.replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&quot;/g, '"');
    },

    encodeTXT: function () {
        return (this).replaceAll('&', '&amp;').replaceAll('<', '&lt;').replaceAll('>', '&gt;').replaceAll(' ', '&nbsp;');
    },

    replaceAll: function (os, ns) {
        return this.replace(new RegExp(os, 'gm'), ns);
    },

    /**
     * 替换占位符为对应选择器的值  //{^(.|\#)[A-Za-z0-9_-\s]*}
     * @param {} $box
     */
    replacePlh: function ($box) {
        $box = $box || $(document);
        return this.replace(/{\/?[^}]*}/g, function ($1) {
            var $input = $box.find($1.replace(/[{}]+/g, ''));

            return $input ? $input.val() : $1;
        });
    },

    replaceMsg: function (holder) {
        return this.replace(new RegExp('({.*})', 'g'), holder);
    },

    replaceTm: function ($data) {
        if (!$data) return this;

        return this.replace(RegExp('({[A-Za-z_]+[A-Za-z0-9_-]*})', 'g'), function ($1) {
            return $data[$1.replace(/[{}]+/g, '')];
        });
    },

    replaceTmById: function (_box) {
        var $parent = _box || $(document);

        return this.replace(RegExp('({[A-Za-z_]+[A-Za-z0-9_-]*})', 'g'), function ($1) {
            var $input = $parent.find('#' + $1.replace(/[{}]+/g, ''));
            return $input.val() ? $input.val() : $1;
        });
    },

    isFinishedTm: function () {
        return !(new RegExp('{\/?[^}]*}').test(this));
    },

    skipChar: function (ch) {
        if (!this || this.length === 0) return '';
        if (this.charAt(0) === ch) return this.substring(1).skipChar(ch);
        return this;
    },

    isValidPwd: function () {
        return (new RegExp(/^([_]|[a-zA-Z0-9]){6,32}$/).test(this));
    },

    isValidMail: function () {
        return (new RegExp(/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/).test(this.trim()));
    },

    isSpaces: function () {
        for (var i = 0; i < this.length; i += 1) {
            var ch = this.charAt(i);

            if (ch != ' ' && ch != '\n' && ch != '\t' && ch != '\r') return false;
        }
        return true;
    },

    isPhone: function () {
        return (new RegExp(/(^([0-9]{3,4}[-])?\d{3,8}(-\d{1,6})?$)|(^\([0-9]{3,4}\)\d{3,8}(\(\d{1,6}\))?$)|(^\d{3,8}$)/).test(this));
    },

    isUrl: function () {
        return (new RegExp(/^[a-zA-z]+:\/\/([a-zA-Z0-9\-\.]+)([-\w .\/?%&=:]*)$/).test(this));
    },

    isExternalUrl: function () {
        return this.isUrl() && this.indexOf('://' + document.domain) == -1;
    },

    toBool: function () {
        return (this.toLowerCase() === 'true') ? true : false;
    },

    toJson: function () {
        var json = this;

        try {
            if (typeof json == 'object') json = json.toString();
            if (!json.trim().match('^\{(.+:.+,*){1,}\}$')) return this;
            else return JSON.parse(this);
        } catch (e) {
            return this;
        }
    },

    toObject: function () {
        var obj;

        try {
            var s = $.trim(this);
            if (s) {
                if (s.substring(0, 1) != '{') {
                    s = '{' + s + '}';
                }
                obj = (new Function('return ' + s))();
            }
        } catch (e) {
            obj = null;
            alert('String toObj：Parse "String" to "Object" error! Your str is: ' + this);
        }
        return obj;
    },

    /**
     * 参数(方法字符串或方法名)： 'function(){...}' 或 'getName' 或 'USER.getName' 均可
     */
    toFunction: function () {
        if (!this || this.length == 0) return undefined;
        //if ($.isFunction(this)) return this

        if (this.startsWith('function')) {
            return (new Function('return ' + this))();
        }

        var m_arr = this.split('.');
        var fn = window;

        for (var i = 0; i < m_arr.length; i++) {
            fn = fn[m_arr[i]];
        }

        if (typeof fn === 'function') {
            return fn;
        }

        return undefined;
    },

    setUrlParam: function (key, value) {
        var str, url = this;

        if (url.indexOf('?') != -1)
            str = url.substr(url.indexOf('?') + 1);
        else
            return url + '?' + key + '=' + value;

        var returnurl = '', setparam, arr, modify = '0';

        if (str.indexOf('&') != -1) {
            arr = str.split('&');

            for (var i in arr) {
                if (arr[i].split('=')[0] == key) {
                    setparam = value;
                    modify = '1';
                } else {
                    setparam = arr[i].split('=')[1];
                }
                returnurl = returnurl + arr[i].split('=')[0] + '=' + setparam + '&';
            }

            returnurl = returnurl.substr(0, returnurl.length - 1);
            if (modify == '0') {
                if (returnurl == str)
                    returnurl = returnurl + '&' + key + '=' + value;
            }
        } else {
            if (str.indexOf('=') != -1) {
                arr = str.split('=');
                if (arr[0] == key) {
                    setparam = value;
                    modify = '1';
                } else {
                    setparam = arr[1];
                }
                returnurl = arr[0] + '=' + setparam;
                if (modify == '0') {
                    if (returnurl == str)
                        returnurl = returnurl + '&' + key + '=' + value;
                }
            } else {
                returnurl = key + '=' + value;
            }
        }
        return url.substr(0, url.indexOf('?')) + '?' + returnurl;
    },

    /**
     * 参数化字符串
     */
    format: function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
                ? args[number] : match;
        });
    }
});

/* =============================Function扩展=============================== */
$.extend(Function.prototype, {
    /**
     * 转换为Function对象
     */
    toFunction: function () {
        return this;
    }
});

/* =============================Date扩展=============================== */
$.extend(Date.prototype, {
    /**
     * 格式化日期
     * @param {String} dates
     */
    format: function (dates) {
        var o = {
            "M+": this.getMonth() + 1, //month
            "d+": this.getDate(),    //day
            "h+": this.getHours(),   //hour
            "m+": this.getMinutes(), //minute
            "s+": this.getSeconds(), //second
            "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
            "S": this.getMilliseconds() //millisecond
        }
        if (/(y+)/.test(dates)) dates = dates.replace(RegExp.$1,
            (this.getFullYear() + '').substr(4 - RegExp.$1.length));
        for (var k in o) if (new RegExp('(' + k + ')').test(dates))
            dates = dates.replace(RegExp.$1,
                RegExp.$1.length == 1 ? o[k] :
                    ('00' + o[k]).substr(('' + o[k]).length));
        return dates;
    },

    /**
     * 格式化为标准日期
     * @param {String} dates
     */
    formatDate: function (dates) {
        return this.format('yyyy-MM-dd');
    },

    /**
     * 格式化为标准日期时间
     * @param {String} dates
     */
    formatDateTime: function (dates) {
        return this.format('yyyy-MM-dd hh:mm:ss');
    },

    /**
     * 格式化为标准时间
     * @param {String} dates
     */
    formatTime: function (dates) {
        return this.format('hh:mm:ss');
    }
});

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
    if ($().validator) {
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
    }
    //#endregion

    //#region 初始化

    $(document).on(fx.initUIEventName, function (e) {
        $(e.target).find('.uiform').uiform();
    });

    //#endregion

})(jQuery);

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
                    if ($().datetimepicker) {
                        self.$element.find('.uidatetime').datetimepicker('remove');
                    }
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
(function ($) {
    'use strict';

    //#region 操作类

    var UITree = (function () {

        //#region 私有变量/函数

        var _exists = function (rows, parentid) {
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].id == parentid) return true;
            }
            return false;
        }

        var _convert = function (rows) {

            var nodes = [];
            //获取root节点
            for (var i = 0; i < rows.length; i++) {
                var row = rows[i];
                if (!_exists(rows, row.parentid)) {
                    nodes.push(row);
                }
            }

            var toDo = [];
            for (var i = 0; i < nodes.length; i++) {
                toDo.push(nodes[i]);
            }
            while (toDo.length) {
                var node = toDo.shift();
                for (var i = 0; i < rows.length; i++) {
                    var row = rows[i];
                    if (row.parentid == node.id) {
                        var child = row;
                        if (node.children) {
                            node.children.push(child);
                        } else {
                            node.children = [child];
                        }
                        //node.state = 'closed';
                        toDo.push(child);
                    }
                }
            }
            return nodes;
        }

        var _dropHandle = function (self, target, source, point) {
            var $tree = self.$element;
            var targetId = $tree.tree('getNode', target).id;
            var sourceParentId = source.parentid;
            var sourceId = source.id;
            var sourceNode = $tree.tree('find', sourceId);
            var newParentNode = $tree.tree('getParent', sourceNode.target);
            var newParentId = newParentNode.id;

            if (sourceParentId != newParentId) {//改变父节点
                var _parentUrl = $tree.data('parentUrl');
                if (_parentUrl) {
                    var _postData = {
                        id: sourceId,
                        newParentId: newParentId
                    };
                    $.post(_parentUrl, _postData, function (result) {
                        if (result.message) {
                            alert(result.message);
                        }
                    });
                }
            }

            var _sortUrl = $tree.data('sortUrl');
            if (_sortUrl) {
                var node = $tree.tree('find', newParentId);
                if (!node) {
                    alert('更新节点顺序出错,无效的节点Id');
                    return;
                }
                var sortData = {};
                var childs = node.children || {};
                $.each(childs, function (index, value) {
                    sortData[value.id] = self.getNodeSortPath(value.id);
                });
                //更新节点序号
                $.post(_sortUrl, sortData, function (result) {
                    if (result.message) {
                        alert(result.message);
                    }
                });
            }
        }

        var _filter = function(q, node) {
            var qq = [];
            $.map($.isArray(q) ? q : [q],
                function(q) {
                    q = $.trim(q);
                    if (q) {
                        qq.push(q);
                    }
                });
            for (var i = 0; i < qq.length; i++) {
                var _textFilter = node.text.toLowerCase().indexOf(qq[i].toLowerCase())>=0;
                var _spellFilter = (node.spell) && (node.spell.toLowerCase().indexOf(qq[i].toLowerCase()) >= 0);
                if (_textFilter || _spellFilter) {
                    return true;
                }
            }
            return !qq.length;
        };

        var getChildNode = function (node) {
            if (!node) return null;
            if (node.children && node.children.length > 0) {
                return node.children[0];
            }
        };

        //#endregion

        //#region 基础函数

        /**
         * Tree控件操作类构造函数
         * @param element 选择元素
         * @param options 配置选项
         */
        function UITree(element, options) {
            this.element = element;
            this.$element = $(element);
            this.options = options;
            this.init();
        }

        //#region 公共函数

        /**
         * 组件初始化
         */
        UITree.prototype.init = function () {

            if (!this.options.loadMaskTarget) {
                this.options.loadMaskTarget = this.element;
            }

            //#region 初始化函数

            var self = this;

            this.options.filter = _filter;

            var _beforeLoad = this.options.onBeforeLoad;
            this.options.onBeforeLoad = function (node, param) {
                fx.loadMask.show(self.options);
                if (_beforeLoad) {
                    _beforeLoad.call(this, node, param);
                }
            };

            var _onLoadSuccess = this.options.onLoadSuccess;
            this.options.onLoadSuccess = function (node, data) {
                fx.loadMask.hide(self.options);
                if (_onLoadSuccess) {
                    _onLoadSuccess.call(this, node, data);
                }
            };

            var _loadError = this.options.onLoadError;
            this.options.onLoadError = function (result) {
                fx.loadMask.hide(self.options);
                fx.ajaxFail(result, self.$element);
                if (_loadError) {
                    _loadError.call(this, msg);
                }
            };

            var _loadFilter = this.options.loadFilter;
            this.options.loadFilter = function (rows) {
                if (_loadFilter) {
                    _loadFilter.call(this, rows);
                }
                if (self.options.convert == true) {
                    return _convert(rows);
                } else {
                    return rows;
                }
            };

            var _onClick = this.options.onClick;
            this.options.onClick = function (node) {
                if (self.options.clickToggle) {
                    self.toggle(node.target);
                }
                if (_onClick) {
                    _onClick.call(this, node);
                }
            };

            var _onDrop = this.options.onDrop;
            this.options.onDrop = function (target, source, point) {
                if (_onDrop) {
                    _onDrop.call(this, target, source, point);
                }

                _dropHandle(self, target, source, point);
            };

            //#endregion

            this.$element.tree(this.options);
        };

        /**
         * 设置控件配置属性
         * @param ops 配置属性
         */
        UITree.prototype.setOptions = function (ops) {
            if (typeof ops === 'object') {
                $.extend(this.options, ops);
                this.init();
            }
        };

        UITree.prototype.getLevelNodeId = function (level) {
            var i = 1;
            var node = this.getRoot();
            while (i < level) {
                i++;
                node = getChildNode(node);
                if (!node) {
                    break;
                }
            }
            if (node) {
                return node.id;
            }
            return 0;
        };

        /**
         * 获取数节点的排序路径
         * @param $tree JQueryTree对象
         * @param id 节点主键
         */
        UITree.prototype.getNodeSortPath = function (nodeId) {
            if (!nodeId) {
                return '';
            }
            var node = this.find(nodeId);
            var parentNode = this.getParent(node.target);
            if (parentNode) {
                var pstring = '';
                if (parentNode.id != '0') {
                    pstring = this.getNodeSortPath(parentNode.id);
                }
                return pstring + fx.fixLenString(this.getNodeIndex(nodeId, parentNode.children), 4, '0');
            } else {
                return fx.fixLenString(this.getNodeIndex(nodeId, this.getRoots()), 4, '0');
            }
        }

        /**
         * 获取节点序号
         * @param id 节点主键
         * @param nodes 待查找的节点集合
         * @param pkName 主键字段名
         */
        UITree.prototype.getNodeIndex = function (nodeId, nodes) {
            var nodeIndex = -1;
            $.each(nodes, function (index, value) {
                if (value.id == nodeId) {
                    nodeIndex = index + 1;
                    return false;
                }
            });
            return nodeIndex.toString();
        },

        /**
         * 展开根节点
         */
        UITree.prototype.expandRoot = function () {
            var root = this.$element.tree('getRoot');
            if (root) {
                this.$element.tree('expand', root.target);
            }
        };

        /**
         * 读取树控件数据
         * @param data 待加载的数据
         */
        UITree.prototype.loadData = function (data) {
            this.$element.tree('loadData', data);
        };

        /**
         * 获取指定节点对象。
         * @param target 节点的DOM对象
         * @returns 返回指定的节点对象
         */
        UITree.prototype.getNode = function (target) {
            return this.$element.tree('getNode', target);
        };

        /**
         * 获取指定节点数据，包含它的子节点。
         * @param target 节点的DOM对象
         */
        UITree.prototype.getData = function (target) {
            return this.$element.tree('getData', target);
        };

        /**
         * 重新载入树控件数据。
         * @param target 节点的DOM对象
         */
        UITree.prototype.reload = function (target) {
            this.$element.tree('reload', target);
        };

        /**
         * 获取通过“nodeEl”参数指定的节点的顶部父节点元素。
         * @param nodeEl 节点选择器字符串
         */
        UITree.prototype.getRoot = function (nodeEl) {
            return this.$element.tree('getRoot', nodeEl);
        };

        /**
         * 获取所有根节点，返回节点数组。
         */
        UITree.prototype.getRoots = function () {
            return this.$element.tree('getRoots');
        };

        /**
         * 获取父节点，'target'参数代表节点的DOM对象。
         * @param target 节点的DOM对象
         */
        UITree.prototype.getParent = function (target) {
            return this.$element.tree('getParent', target);
        };

        /**
         * 获取所有子节点，'target'参数代表节点的DOM对象。
         * @param target 节点的DOM对象
         * @returns
         */
        UITree.prototype.getChildren = function (target) {
            return this.$element.tree('getChildren', target);
        };

        /**
         * 获取所有选中的节点。'state'可用值有：'checked','unchecked','indeterminate'。
         * 如果'state'未指定，将返回'checked'节点。
         * @param state 可用值有：'checked','unchecked','indeterminate'。
         * @returns
         */
        UITree.prototype.getChecked = function (state) {
            return this.$element.tree('getChecked', state);
        };

        /**
         * 获取选中节点,如果未选择则返回null.
         * @returns 如果未选择则返回null.
         */
        UITree.prototype.getSelected = function () {
            return this.$element.tree('getSelected');
        };

        /**
         * 判断指定的节点是否是叶子节点，target参数是一个节点DOM对象。
         * @param target 节点的DOM对象
         */
        UITree.prototype.isLeaf = function (target) {
            return this.$element.tree('isLeaf', target);
        };

        /**
         * 查找指定节点并返回节点对象。
         * @param id 节点Id
         */
        UITree.prototype.find = function (id) {
            return this.$element.tree('find', id);
        };

        /**
         * 选择一个节点，'target'参数表示节点的DOM对象。
         * @param target 节点的DOM对象
         */
        UITree.prototype.select = function (target) {
            this.$element.tree('select', target);
        };

        /**
         * 选中指定节点。
         * @param target 节点的DOM对象
         */
        UITree.prototype.check = function (target) {
            this.$element.tree('check', target);
        };

        /**
         * 取消选中指定节点。
         * @param target 节点的DOM对象
         */
        UITree.prototype.uncheck = function (target) {
            this.$element.tree('uncheck', target);
        };

        /**
         * 折叠一个节点，'target'参数表示节点的DOM对象。
         * @param target 节点的DOM对象
         */
        UITree.prototype.collapse = function (target) {
            this.$element.tree('collapse', target);
        };

        /**
         * 展开一个节点，'target'参数表示节点的DOM对象。
         * 在节点关闭或没有子节点的时候，节点ID的值(名为'id'的参数)将会发送给服务器请求子节点的数据。
         * @param target 节点的DOM对象
         */
        UITree.prototype.expand = function (target) {
            this.$element.tree('expand', target);
        };

        /**
         * 折叠所有节点。
         * @param target 节点的DOM对象
         */
        UITree.prototype.collapseAll = function (target) {
            this.$element.tree('collapseAll', target);
        };

        /**
         * 展开所有节点。
         * @param target 节点的DOM对象
         */
        UITree.prototype.expandAll = function (target) {
            this.$element.tree('expandAll', target);
        };

        /**
         * 打开从根节点到指定节点之间的所有节点。
         * @param target 节点的DOM对象
         */
        UITree.prototype.expandTo = function (target) {
            this.$element.tree('expandTo', target);
        };

        /**
         * 滚动到指定节点。
         * @param target 节点的DOM对象
         */
        UITree.prototype.scrollTo = function (target) {
            this.$element.tree('scrollTo', target);
        };

        /**
         * 打开或关闭节点的触发器
         * @param target 节点DOM对象
         */
        UITree.prototype.toggle = function (target) {
            this.$element.tree('toggle', target);
        };

        /**
         * 追加若干子节点到一个父节点，
         * param参数有2个属性：
         * parent：DOM对象，将要被追加子节点的父节点，如果未指定，子节点将被追加至根节点。
         * data：数组，节点数据。
         * @param param
         */
        UITree.prototype.append = function (param) {
            this.$element.tree('append', param);
        };

        /**
         * 在一个指定节点之前或之后插入节点，
         * 'param'参数包含如下属性：
         * before：DOM对象，在某个节点之前插入。
         * after：DOM对象，在某个节点之后插入。
         * data：对象，节点数据。
         * @param param
         */
        UITree.prototype.insert = function (param) {
            this.$element.tree('insert', param);
        };

        /**
         * 移除一个节点和它的子节点，'target'参数是该节点的DOM对象。
         * @param target 节点的DOM对象
         */
        UITree.prototype.remove = function (target) {
            this.$element.tree('remove', target);
        };

        /**
         * 移除一个节点和它的子节点，该方法跟remove方法一样，不同的是它将返回被移除的节点数据。
         * @param target 节点的DOM对象
         */
        UITree.prototype.pop = function (target) {
            this.$element.tree('pop', target);
        };

        /**
         * 更新指定节点。'param'参数包含以下属性：
         * target(DOM对象，将被更新的目标节点)，id，text，iconCls，checked等。
         * @param param
         */
        UITree.prototype.update = function (param) {
            this.$element.tree('update', param);
        };

        /**
         * 启用拖拽功能。
         */
        UITree.prototype.enableDnd = function () {
            this.$element.tree('enableDnd');
        };

        /**
         * 禁用拖拽功能。
         */
        UITree.prototype.disableDnd = function () {
            this.$element.tree('disableDnd');
        };

        /**
         * 开始编辑一个节点。
         * @param target 节点的DOM对象
         */
        UITree.prototype.beginEdit = function (target) {
            this.$element.tree('beginEdit', target);
        };

        /**
         * 结束编辑一个节点。
         * @param target 节点的DOM对象
         */
        UITree.prototype.endEdit = function (target) {
            this.$element.tree('endEdit', target);
        };

        /**
         * 取消编辑一个节点。
         * @param target 节点的DOM对象
         */
        UITree.prototype.cancelEdit = function (target) {
            this.$element.tree('cancelEdit', target);
        };

        /**
         * 过滤操作，和filter属性功能类似
         * $('#tt').tree('doFilter', '');    // 清除过滤器
         * @param text 查询关键字
         */
        UITree.prototype.doFilter = function (text) {
            this.$element.tree('doFilter', text);
        }

        /**
         * 过滤操作，和filter属性功能类似
         * $('#tt').tree('doFilter', '');    // 清除过滤器
         * @param text 查询关键字
         */
        UITree.prototype.filter = function (text) {
            this.$element.tree('doFilter', text);
        }

        //#endregion

        //#endregion

        return UITree;
    }());

    //#endregion

    //#region 插件

    /**
     * 控件初始化
     * @param options 配置选项
     * @returns {UITree}
     */
    $.fn.uitree = function (options) {
        this.each(function () {
            var $element = $(this);
            var instance = $element.data('uitree');
            if (!instance) {
                var ops = $.extend({}, $.fn.uitree.defaults, $.fn.uitree.parseOptions($element), options);
                instance = new UITree(this, ops);
                $element.data('uitree', instance);
            } else {
                instance.setOptions(options);
            }
        });
        return $(this).data('uitree');
    };

    /**
     * 解析配置项
     * @param $element 目标对象
     * @returns 返回元素data配置选项
     */
    $.fn.uitree.parseOptions = function ($element) {
        fx.convertOptionToEvent($element);
        return $element.data();
    };

    /**
     * 插件默认值
     */
    $.fn.uitree.defaults = $.extend({}, fx.loadMask.defaults, {

        container: fx.module,

        convert:true,

        clickToggle: true,

        /**
         * 加载提示消息
         */
        loadMaskMessage: '正在加载数据,请稍等...'

    });

    //#endregion

    //#region 初始化

    $(document).on(fx.initUIEventName, function (e) {
        $(e.target).find('.uitree').uitree();
    });

    //#endregion

})(jQuery);
(function ($) {
    'use strict';

    //#region 操作类

    var UITreeGrid = (function () {
        var dropParentId;
        var _exists = function (rows, id) {
            for (var i = 0; i < rows.length; i++) {
                if (rows[i]._parentId == id) return true;
            }
            return false;
        }

        var getSelectedTreeNode = function (self) {
            var node = self.$treegridEl.treegrid('getSelected');
            if (!node) {
                fx.msg('请先选择节点');
                return null;
            }
            return node;
        };

        //#region 构造函数

        /**
         * 界面初始化构造函数
         * @param element 控件元素
         * @param options 配置选项
         * @constructor
         */
        function UITreeGrid(element, options) {
            this.element = element;
            this.$element = $(element);
            this.options = options;

            this.$treegridEl = this.$element.find('.treegrid');

            this.$toolbarEl = this.$element.find('.treegridtoolbar');

            this.$tcreaterootEl = this.$toolbarEl.find('.tcreateroot');
            this.$tcreateEl = this.$toolbarEl.find('.tcreate');
            this.$teditEl = this.$toolbarEl.find('.tedit');
            this.$tdeleteEl = this.$toolbarEl.find('.tdelete');
            this.$trefreshEl = this.$toolbarEl.find('.trefresh');

            this.$menuEl = this.$element.find('.ttmenu');
            this.$menuCreateEl = this.$element.find('.ttmcreate');
            this.$menuEditEl = this.$element.find('.ttmedit');
            this.$menuDeleteEl = this.$element.find('.ttmdelete');
            this.$menuRefreshEl = this.$element.find('.ttmrefresh');

            this.$tfilterEl = this.$element.find('.tfilter');

            //var self = this;
            //this.$tfilterEl.keypress(function (e) {
            //    var name = $(this).val().trim();
            //    if (e.keyCode == 13) {
            //        self.$treegridEl.treegrid('load', {name: name});
            //    }
            //});

            this.initToolbar();
            this.initTreeGrid();
            this.initMenu();
        }

        //#endregion

        //#region 公共函数

        UITreeGrid.prototype.initTreeGrid = function () {
            var self = this;
            self.$treegridEl.treegrid({
                url: self.$treegridEl.data('url'),
                fit: true,
                border: false,
                striped: true,
                rownumbers: true,
                loadMsg: '正在加载数据...',
                animate: true,
                fitColumns: true,
                collapsible: true,
                method: 'get',
                toolbar: self.$toolbarEl[0],
                idField: 'Id',
                treeField: 'Name',
                loadFilter: function (data) {
                    $.each(data.rows, function (i, v) {
                        if (v._parentId && v._parentId == '0') {
                            v._parentId = null;
                        }
                        if (v.IsExpand && v.IsExpand == true) {
                            v.state = 'open';
                        }
                        else if (_exists(data.rows, v.Id)) {
                            v.state = 'closed';
                        }
                        if (v.iconCls) {
                            v.iconCls = 'font-icon-treegrid ' + v.iconCls;
                        }
                    });
                    return data;
                },
                onSelect: function (node) {
                    self.lastSelectedId = node.Id;
                },
                onContextMenu: function (e, node) {
                    e.preventDefault();
                    if (node) {
                        $(this).treegrid('expand', node.Id);
                        $(this).treegrid('select', node.Id);
                        self.$menuEl.menu('show', {
                            left: e.pageX,
                            top: e.pageY
                        });
                    }
                },
                onDblClickRow: function (row) {
                    var url = $(this).data("detailsUrl");
                    var data = { id: row.Id };
                    fx.window({ url: url, data: data });
                },
                onDragOver: function (targetRow, sourceRow) {
                    if (sourceRow._parentId == targetRow.Id) {
                        return false;
                    }
                    return true;
                },
                onBeforeDrop: function (targetRow, sourceRow) {
                    dropParentId = sourceRow._parentId;
                },
                onDrop: function (targetRow, sourceRow, point) {
                    var saveParentUrl = $(this).data('parentUrl');
                    var updateSortPathUrl = $(this).data('sortUrl');
                    var id = sourceRow.Id;
                    var newParentId;
                    if (point == 'append') {
                        newParentId = targetRow.Id;
                    }
                    else {
                        newParentId = targetRow._parentId;
                    }
                    if (!newParentId || newParentId == '') {
                        newParentId = '0';
                    }
                    $.post(saveParentUrl, { id: id, oldParentId: dropParentId, newParentId: newParentId }, function (result) {
                        if (!result.success) {
                            fx.alert(result.message);
                        }
                    });

                    var ids = [];
                    ids.push(newParentId);

                    var data = {};
                    $.each(ids, function (i, v) {
                        var row = self.$treegridEl.treegrid('find', v);
                        if (row == null) {
                            var sz = self.$treegridEl.treegrid('getRoots');
                            $.each(sz, function (index, value) {
                                data[value.Id] = fx.fixLenString(index.toString(), 4, '0');
                            });
                        } else if (row.children) {
                            $.each(row.children, function (index, value) {
                                data[value.Id] = self.getNodeSortPath(value.Id);
                            });
                        }
                    });
                    $.post(updateSortPathUrl, data, function (result) {
                        if (!result.success) {
                            fx.alert(result.message);
                        }
                    });
                },
                onLoadSuccess: function () {
                    var $tree = $(this);
                    var node = $tree.treegrid('find', self.lastSelectedId);
                    if (node) {
                        $tree.treegrid('expandTo', node.Id);
                        $tree.treegrid('select', node.Id);
                    }

                    $(this).treegrid('enableDnd', null);
                }
            });
        };

        /**
         * 获取数节点的排序路径
         * @param id 节点Id
         */
        UITreeGrid.prototype.getNodeSortPath = function (id) {
            if (!id) {
                return '';
            }
            var parentNode = this.$treegridEl.treegrid('getParent', id);
            if (parentNode != null) {
                var pstring = this.getNodeSortPath(parentNode.Id);
                return pstring + fx.fixLenString(this.getNodeIndex(id, parentNode.children), 4, '0');
            } else {
                return fx.fixLenString(this.getNodeIndex(id, this.$treegridEl.treegrid('getRoots')), 4, '0');
            }
        };

        UITreeGrid.prototype.getNodeIndex = function (id, nodes) {
            var nodeIndex = -1;
            $.each(nodes, function (index, value) {
                if (value.Id == id) {
                    nodeIndex = index;
                    return false;
                }
            });
            return nodeIndex.toString();
        }

        UITreeGrid.prototype.initMenu = function () {
            var self = this;
            if (self.$menuEl.length > 0) {
                self.$menuEl.menu({
                    onShow: function () {
                        var node = getSelectedTreeNode(self);
                        if (node.id == '0') {
                            if (self.$menuEditEl.length > 0) {
                                $(this).menu('disableItem', self.$menuEditEl[0]);
                            }
                            if (self.$menuDeleteEl.length > 0) {
                                $(this).menu('disableItem', self.$menuDeleteEl[0]);
                            }
                        } else {
                            if (self.$menuEditEl.length > 0) {
                                $(this).menu('enableItem', self.$menuEditEl[0]);
                            }
                            if (self.$menuDeleteEl.length > 0) {
                                $(this).menu('enableItem', self.$menuDeleteEl[0]);
                            }
                        }
                    }
                });

                fx.onModuleDestroy(function () { self.$menuEl.menu('destroy'); });
            }

            if (self.$menuCreateEl.length > 0) {
                self.$menuCreateEl.click(function () {
                    var url = $(this).data('url');
                    self.create(url);
                });
            }

            if (self.$menuEditEl.length > 0) {
                self.$menuEditEl.click(function () {
                    var url = $(this).data('url');
                    self.edit(url);
                });
            }

            if (self.$menuDeleteEl.length > 0) {
                self.$menuDeleteEl.click(function () {
                    var url = $(this).data('url');
                    self.delete(url);
                });
            }

            if (self.$menuRefreshEl.length > 0) {
                self.$menuRefreshEl.click(function () {
                    self.refresh();
                });
            }
        };

        UITreeGrid.prototype.initToolbar = function () {
            var self = this;

            if (self.$tcreaterootEl.length > 0) {
                self.$tcreaterootEl.click(function () {
                    var url = $(this).data('url');
                    self.createRoot(url);
                });
            }

            if (self.$tcreateEl.length > 0) {
                self.$tcreateEl.click(function () {
                    var url = $(this).data('url');
                    self.create(url);
                });
            }

            if (self.$teditEl.length > 0) {
                self.$teditEl.click(function () {
                    var url = $(this).data('url');
                    self.edit(url);
                });
            }

            if (self.$tdeleteEl.length > 0) {
                self.$tdeleteEl.click(function () {
                    var url = $(this).data('url');
                    self.delete(url);
                });
            }

            if (self.$trefreshEl.length > 0) {
                self.$trefreshEl.click(function () {
                    self.refresh();
                });
            }
        }

        UITreeGrid.prototype.createRoot = function (url) {
            fx.window({ url: url, data: { parentid: 0 } });
        };
        UITreeGrid.prototype.create = function (url) {
            var self = this;
            var node = getSelectedTreeNode(self);
            if (node) {
                fx.window({ url: url, data: { parentid: node.Id } });
            }
        };
        UITreeGrid.prototype.edit = function (url) {
            var self = this;
            var node = getSelectedTreeNode(self);
            if (node) {
                fx.window({ url: url, data: { id: node.Id } });
            }
        };
        UITreeGrid.prototype.delete = function (url) {
            var self = this;
            var node = getSelectedTreeNode(self);
            if (node) {
                fx.ajax({
                    url: url,
                    data: { id: node.Id },
                    confirm: '确定要删除选中的节点以及子节点吗?',
                    success: function (result) {
                        if (result.success) {
                            self.refresh();
                        } else {
                            fx.alert(result.message);
                        }
                    }
                });
            }
        };
        UITreeGrid.prototype.refresh = function () {
            this.$treegridEl.treegrid('reload');
        };

        /**
         * 设置控件配置属性
         * @param ops 配置属性
         */
        UITreeGrid.prototype.setOptions = function (ops) {
            if (typeof ops === 'object') {
                $.extend(this.options, ops);
            }
        };

        //#endregion

        return UITreeGrid;
    })();

    //#endregion

    //#region 插件

    /**
     * 控件初始化
     * @param options 配置选项
     * @returns {UITreeGrid}
     */
    $.fn.uitreegrid = function (options) {
        this.each(function () {
            var $element = $(this);
            var instance = $element.data('uitreegrid');
            if (!instance) {
                instance = new UITreeGrid(this, {});
                $element.data('uitreegrid', instance);
            } else {
                instance.setOptions(options);
            }
        });
        return $(this).data('uitreegrid');
    };

    //#endregion

    $(document).on(fx.initUIEventName, function (e) {
        $(e.target).find('.uitreegrid').uitreegrid();
    });

})(jQuery);

(function ($) {
    'use strict';

    //#region 操作类

    var UITreeTable = (function () {

        var getSelectedTreeNode = function (self) {
            var tree = self.$treeEl.uitree();
            var node = tree.getSelected();
            if (!node) {
                fx.alert(self.$treeEl.data('selectedMessage') || '请先选择树节点');
                return null;
            }
            return node;
        };

        var searchTable = function (self) {
            var node = getSelectedTreeNode(self);
            if (self.lastLoadId != node.id) {
                self.$tableEl.uitable().search();
            }
        };

        var treeSubmitSuccess = function (self, $modal, isCreate) {
            var callback = function (result) {
                var $form = $(this);
                if (result.success) {
                    $modal.uimodal().close();

                    var tree = self.$treeEl.uitree();
                    if (isCreate == true) {
                        tree.append({
                            parent: tree.getSelected().target,
                            data: {
                                id: result.data.id,
                                parentid: result.data.parentId,
                                iconCls: result.data.iconCls,
                                text: result.data.text
                            }
                        });
                        if (self.$menuCreateEl.data('refreshTable') == true) {
                            self.$tableEl.uitable().reload();
                        }
                    } else {
                        tree.update({
                            target: tree.getSelected().target,
                            iconCls: result.data.iconCls,
                            text: result.data.text
                        });
                        if (self.$menuEditEl.data('refreshTable') == true) {
                            self.$tableEl.uitable().reload();
                        }
                    }


                } else {
                    var errorEl = $form.data('errorEl');
                    if (errorEl) {
                        var $container = $($(this).uiform().options.container);
                        $container.find(errorEl).html(result.message).show();
                    }
                    else {
                        fx.alert(result.message);
                    }
                }
            };
            $modal.find('form').uiform({ onSubmitSuccess: callback });
        };

        //#region 构造函数

        /**
         * 界面初始化构造函数
         * @param element 控件元素
         * @param options 配置选项
         * @constructor
         */
        function UITreeTable(element, options) {
            var self = this;

            this.element = element;
            this.$element = $(element);
            this.options = options;

            this.$treeEl = this.$element.find('.tttree');
            this.$tableEl = this.$element.find('.tttable');
            this.$tableCreateEl = this.$element.find('.tttcreate');

            this.$menuEl = this.$element.find('.ttmenu');
            this.$menuCreateEl = this.$element.find('.ttmcreate');
            this.$menuEditEl = this.$element.find('.ttmedit');
            this.$menuDeleteEl = this.$element.find('.ttmdelete');
            this.$menuRefreshEl = this.$element.find('.ttmrefresh');

            this.$tttree_filterEl = this.$element.find('.tttree_filter');

            this.$tttree_filterEl.keypress(function (e) {
                if (e.keyCode == 13) {
                    self.$treeEl.tree('doFilter', $(this).val().trim());
                }
            });

            this.initMenu();
            this.initTable();
            this.initTree();

            
            var westWidthName = self.$element.data('westWidthName');
            if (westWidthName) {
                fx.monitorPanelWidth(westWidthName, self.$element, 'west');
            }
        }

        //#endregion

        //#region 公共函数

        UITreeTable.prototype.initTree = function () {
            var self = this;
            if (self.$treeEl.length == 0) return;
            self.$treeEl.uitree({
                onLoadSuccess: function () {
                    var tree = $(this).uitree();
                    var node = tree.find(self.lastSelectedId);
                    if (!node) {
                        var level = self.$treeEl.data('expandLevel') || 1;
                        var nodeId = tree.getLevelNodeId(level);
                        node = tree.find(nodeId);
                    }

                    tree.expandTo(node.target);
                    tree.expand(node.target);
                    tree.select(node.target);
                    tree.scrollTo(node.target);
                },

                onSelect: function (node) {
                    self.lastSelectedId = node.id;
                    searchTable(self);
                },

                onContextMenu: function (e, node) {
                    e.preventDefault();
                    $(this).tree('expand', node.target);
                    $(this).tree('select', node.target);
                    self.$menuEl.menu('show', {
                        left: e.pageX,
                        top: e.pageY
                    });
                },

                onClick: function (node) {
                    $(this).tree('expand', node.target);
                }
            });
        };

        UITreeTable.prototype.initMenu = function () {
            var self = this;
            if (self.$menuEl.length > 0) {
                self.$menuEl.menu({
                    onShow: function () {
                        var tree = self.$treeEl.uitree();
                        var node = tree.getSelected();
                        if (node.id == '0') {
                            if (self.$menuEditEl.length > 0) {
                                $(this).menu('disableItem', self.$menuEditEl[0]);
                            }
                            if (self.$menuDeleteEl.length > 0) {
                                $(this).menu('disableItem', self.$menuDeleteEl[0]);
                            }
                        } else {
                            if (self.$menuEditEl.length > 0) {
                                $(this).menu('enableItem', self.$menuEditEl[0]);
                            }
                            if (self.$menuDeleteEl.length > 0) {
                                $(this).menu('enableItem', self.$menuDeleteEl[0]);
                            }
                        }
                    }
                });

                fx.onModuleDestroy(function () { self.$menuEl.menu('destroy'); });
            }

            if (self.$menuCreateEl.length > 0) {
                self.$menuCreateEl.data('onBeforeShow', function (options) {
                    options.data.parentid = self.$treeEl.uitree().getSelected().id;
                });
                self.$menuCreateEl.data('onAfterShow', function () {
                    treeSubmitSuccess(self, $(this), true);
                });
            }

            if (self.$menuEditEl.length > 0) {
                self.$menuEditEl.data('onBeforeShow', function (options) {
                    options.data.id = self.$treeEl.uitree().getSelected().id;
                });
                self.$menuEditEl.data('onAfterShow', function () {
                    treeSubmitSuccess(self, $(this), false);
                });
            }

            if (self.$menuRefreshEl.length > 0) {
                self.$menuRefreshEl.click(function () {
                    self.$treeEl.uitree().reload();
                });
            }
        };

        UITreeTable.prototype.initTable = function () {
            var self = this;
            if (self.$tableEl.length > 0) {
                self.$tableEl.uitable({
                    onBeforeLoad: function (options) {
                        var node = getSelectedTreeNode(self);
                        if (!node) return false;
                        options.params.parentid = node.id;
                        self.lastLoadId = node.id;
                    },
                    onAfterLoad: function () {
                     
                        self.$element.find('.tsdelete').each(function () {
                            if ($(this).data('refreshTree') == true) {
                                $(this).data('successCallback', function () {
                                    self.$treeEl.uitree().reload();
                                });
                            }
                        });
                    }
                });
            }

            if (self.$tableCreateEl.length > 0) {
                self.$tableCreateEl.data('onBeforeShow', function (options) {
                    var node = getSelectedTreeNode(self);
                    if (!node) return false;
                    options.data.parentid = node.id;
                });
            }

        };

        /**
         * 设置控件配置属性
         * @param ops 配置属性
         */
        UITreeTable.prototype.setOptions = function (ops) {
            if (typeof ops === 'object') {
                $.extend(this.options, ops);
            }
        };

        //#endregion

        return UITreeTable;
    })();

    //#endregion

    //#region 插件

    /**
     * 控件初始化
     * @param options 配置选项
     * @returns {UITreeTable}
     */
    $.fn.uitreetable = function (options) {
        this.each(function () {
            var $element = $(this);
            var instance = $element.data('uitreetable');
            if (!instance) {
                instance = new UITreeTable(this, {});
                $element.data('uitreetable', instance);
            } else {
                instance.setOptions(options);
            }
        });
        return $(this).data('uitreetable');
    };

    //#endregion

    $(document).on(fx.initUIEventName, function (e) {
        $(e.target).find('.uitreetable').uitreetable();
    });

})(jQuery);
(function ($) {
    'use strict';

    function _select2SetText(select, text_name) {
        var option = $(select).find('option:checked');
        $(select).siblings('[name=' + text_name + ']').val(option.text());
    }

    function _initPlugins($box) {

        /*-------select2-------*/
        if ($().select2) {

            //#region uiselect

            $.fn.select2.amd.require(['select2/compat/matcher'], function (oldMatcher) {
                $box.find('.uiselect').select2({
                    width: 'auto',
                    allowClear: false,
                    placeholder: '',
                    language: 'zh-CN',
                    theme: 'bootstrap',
                    matcher: oldMatcher(function (term, text, obj) {
                        var spell = $(obj.element).data('spell');
                        if ((text.toUpperCase().indexOf(term.toUpperCase()) > -1)
                            || (spell && spell.toUpperCase().indexOf(term.toUpperCase()) > -1)) {
                            return true;
                        }
                        return false;
                    })
                }).each(function () {
                    var current = this;

                    $(current).on('select2:open', function (v) { //修复宽度问题
                        var width = $(v.target).next().width();
                        $(v.target).next().width(width);
                        $('.select2-container--open > .select2-dropdown').width(width);
                    });

                    var hasTextField = $(current).data('hasTextField');
                    if (hasTextField !== false) {
                        var text_name = $(current).data('textField');
                        if (!text_name && current.name) {
                            text_name = current.name + '_text';
                        }
                        if (text_name) {
                            var hidden = '<input type="hidden" name="' + text_name + '"/>';
                            $(current).parent().prepend(hidden);
                            _select2SetText(current, text_name);

                            $(current).on('select2:select', function () {
                                _select2SetText(current, text_name);
                            });
                        }
                    }
                });
            });

            //#endregion
        }

        /*-------minicolors-------*/
        if ($().minicolors) {
            $box.find('.uicolor').minicolors({
                control: $(this).attr('data-control') || 'hue',
                defaultValue: $(this).attr('data-defaultValue') || '',
                inline: $(this).attr('data-inline') === 'true',
                letterCase: $(this).attr('data-letterCase') || 'lowercase',
                opacity: $(this).attr('data-opacity'),
                position: $(this).attr('data-position') || 'bottom left',
                change: function (hex, opacity) {
                    if (!hex) return '';
                    if (opacity) hex += ', ' + opacity;
                    return hex;
                    //                if(typeof console === 'object') {
                    //                    console.log(hex);
                    //                }
                },
                theme: 'bootstrap'
            });
        }

        /*-------bootstrapSwitch-------*/
        if ($().bootstrapSwitch) {
            $box.find('.uiswitch').bootstrapSwitch();
        }

        /*-------bootstrap-maxlength-------*/
        if ($().maxlength) {
            $box.find('.uilength').maxlength({
                limitReachedClass: 'label label-danger'
            });
        }

        /*-------icheck-------*/
        if ($().iCheck) {
            $box.find('.uicheck').each(function () {
                var checkboxClass = $(this).attr('data-checkbox') ? $(this).attr('data-checkbox') : 'icheckbox_square-green';
                var radioClass = $(this).attr('data-radio') ? $(this).attr('data-radio') : 'iradio_square-green';

                if (checkboxClass.indexOf('_line') > -1 || radioClass.indexOf('_line') > -1) {
                    $(this).iCheck({
                        checkboxClass: checkboxClass,
                        radioClass: radioClass,
                        insert: '<div class="icheck_line-icon"></div>' + $(this).attr('data-label')
                    });
                } else {
                    $(this).iCheck({
                        checkboxClass: checkboxClass,
                        radioClass: radioClass
                    });
                }
            });
        }

        /*-------TouchSpin-------*/
        if ($().TouchSpin) {
            $box.find('.uispin').TouchSpin();
        }

        /*-------datepicker-------*/
        if ($().datepicker) {
            $box.find('.uidate').datepicker({
                todayBtn: 'linked',
                language: 'zh-CN',
                format: 'yyyy-mm-dd',
                autoclose: true,
                todayHighlight: true
            });
        }

        /*-------timepicker-------*/
        if ($().timepicker) {
            $box.find('.uitime').timepicker({
                defaultTime: false
            });
        }

        /*-------datetimepicker-------*/
        if ($().datetimepicker) {
            $box.find('.uidatetime').datetimepicker({
                autoclose: true,
                todayBtn: 'linked',
                todayHighlight: true,
                language: 'zh-CN',
                fontAwesome: true
            });
        }

        /*-------clockpicker-------*/
        if ($().clockpicker) {
            $box.find('.uiclock').clockpicker({
                default: 'now',
                placement: 'bottom',
                align: 'left',
                afterShow: function () {
                    $('.clockpicker-popover').css('z-index', '10061');
                }
            });
        }

        /*-------menu();-------*/
        if ($().menu) {
            var es = ['onShow', 'onHide', 'onClick'];
            $box.find('.easyui-menu').each(function () {
                var ops = {};
                var $self = $(this);
                $.each(es, function (i, v) {
                    var dv = $self.data(v);
                    if (dv) {
                        ops[v] = dv.toFunction();
                    }
                });
                $self.menu(ops);
            });
        }

        /*-------layout();-------*/
        if ($().layout) {
            $box.find('.easyui-layout').layout();
        }

        /*-------treegrid();-------*/
        if ($().treegrid) {
            $box.find('.easyui-treegrid').treegrid();
        }

        /*-------linkbutton();-------*/
        if ($().linkbutton) {
            $box.find('.easyui-linkbutton').linkbutton();
        }

        /*-------scroller-------*/
        if ($().slimScroll) {
            $box.find('.scroller').each(function () {
                if ($(this).attr('data-initialized')) {
                    return; // exit
                }

                var height;

                if ($(this).attr('data-height')) {
                    height = $(this).attr('data-height');
                } else {
                    height = $(this).css('height');
                }

                $(this).slimScroll({
                    allowPageScroll: true, // allow page scroll when the element scroll is ended
                    size: '7px',
                    color: ($(this).attr('data-handle-color') ? $(this).attr('data-handle-color') : '#bbb'),
                    wrapperClass: ($(this).attr('data-wrapper-class') ? $(this).attr('data-wrapper-class') : 'slimScrollDiv'),
                    railColor: ($(this).attr('data-rail-color') ? $(this).attr('data-rail-color') : '#eaeaea'),
                    position: 'right',
                    height: height,
                    alwaysVisible: ($(this).attr('data-always-visible') == '1' ? true : false),
                    railVisible: ($(this).attr('data-rail-visible') == '1' ? true : false),
                    disableFadeOut: true
                });

                $(this).attr('data-initialized', '1');
            });
        }

        /*-------tooltip-------*/
        if ($().tooltip) {
            $box.find('[data-toggle="tooltip"]').tooltip({
                placement: 'bottom'
            });
        }

        /*-------Select2SelectSubmit();-------*/
        $box.find('.uiselectsubmit').each(function () {
            $(this).onSelect2Select(function () {
                $(this).closest('form').find(':submit').click();
            });
        });

        /*-------fileupload-------*/
        if ($().fileinput) {
            $box.find('.uifileupload').each(function () {
                var ops = {
                    language: 'zh',
                    showCaption: true,
                    showRemove: false,
                    showUpload: false,
                    showPreview: false,
                    showClose: false,
                    uploadAsync: false
                };
                $(this).fileinput(ops);
            });
        }

        ///*-------KindEditor-------*/
        //if (KindEditor) {
            
        //}
    }

    $(document).on(fx.initUIEventName, function (e) {
        _initPlugins($(e.target));
    });

    $(document).on('click.fx', '.uiwindow', function () {
        fx.window($(this).data());
        return false;
    });

    $('body').on('hidden.bs.modal', function () {
        if ($('.modal:visible').size() > 0 && $('body').hasClass('modal-open') === false) {
            $('body').addClass('modal-open');
        }
    });

})(jQuery);