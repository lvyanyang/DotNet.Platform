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

