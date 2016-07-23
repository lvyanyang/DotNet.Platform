/**
 * jQuery EasyUI 1.4.5
 * 
 * Copyright (c) 2009-2016 www.jeasyui.com. All rights reserved.
 *
 * Licensed under the freeware license: http://www.jeasyui.com/license_freeware.php
 * To use it on other terms please contact us: info@jeasyui.com
 *
 */
/****************************jquery.resizable****************************/
(function($) {
    //	var isResizing = false;
    $.fn.resizable = function(options,param) {
        if(typeof options == 'string') {
            return $.fn.resizable.methods[options](this,param);
        }

        function resize(e) {
            var resizeData = e.data;
            var options = $.data(resizeData.target,'resizable').options;
            if(resizeData.dir.indexOf('e') != -1) {
                var width = resizeData.startWidth + e.pageX - resizeData.startX;
                width = Math.min(
							Math.max(width,options.minWidth),
							options.maxWidth
						);
                resizeData.width = width;
            }
            if(resizeData.dir.indexOf('s') != -1) {
                var height = resizeData.startHeight + e.pageY - resizeData.startY;
                height = Math.min(
						Math.max(height,options.minHeight),
						options.maxHeight
				);
                resizeData.height = height;
            }
            if(resizeData.dir.indexOf('w') != -1) {
                var width = resizeData.startWidth - e.pageX + resizeData.startX;
                width = Math.min(
							Math.max(width,options.minWidth),
							options.maxWidth
						);
                resizeData.width = width;
                resizeData.left = resizeData.startLeft + resizeData.startWidth - resizeData.width;

                //				resizeData.width = resizeData.startWidth - e.pageX + resizeData.startX;
                //				if (resizeData.width >= options.minWidth && resizeData.width <= options.maxWidth) {
                //					resizeData.left = resizeData.startLeft + e.pageX - resizeData.startX;
                //				}
            }
            if(resizeData.dir.indexOf('n') != -1) {
                var height = resizeData.startHeight - e.pageY + resizeData.startY;
                height = Math.min(
							Math.max(height,options.minHeight),
							options.maxHeight
						);
                resizeData.height = height;
                resizeData.top = resizeData.startTop + resizeData.startHeight - resizeData.height;

                //				resizeData.height = resizeData.startHeight - e.pageY + resizeData.startY;
                //				if (resizeData.height >= options.minHeight && resizeData.height <= options.maxHeight) {
                //					resizeData.top = resizeData.startTop + e.pageY - resizeData.startY;
                //				}
            }
        }

        function applySize(e) {
            var resizeData = e.data;
            var t = $(resizeData.target);
            t.css({
                left: resizeData.left,
                top: resizeData.top
            });
            if(t.outerWidth() != resizeData.width) { t._outerWidth(resizeData.width) }
            if(t.outerHeight() != resizeData.height) { t._outerHeight(resizeData.height) }
            //			t._outerWidth(resizeData.width)._outerHeight(resizeData.height);
        }

        function doDown(e) {
            //			isResizing = true;
            $.fn.resizable.isResizing = true;
            $.data(e.data.target,'resizable').options.onStartResize.call(e.data.target,e);
            return false;
        }

        function doMove(e) {
            resize(e);
            if($.data(e.data.target,'resizable').options.onResize.call(e.data.target,e) != false) {
                applySize(e)
            }
            return false;
        }

        function doUp(e) {
            //			isResizing = false;
            $.fn.resizable.isResizing = false;
            resize(e,true);
            applySize(e);
            $.data(e.data.target,'resizable').options.onStopResize.call(e.data.target,e);
            $(document).unbind('.resizable');
            $('body').css('cursor','');
            //			$('body').css('cursor','auto');
            return false;
        }

        return this.each(function() {
            var opts = null;
            var state = $.data(this,'resizable');
            if(state) {
                $(this).unbind('.resizable');
                opts = $.extend(state.options,options || {});
            } else {
                opts = $.extend({},$.fn.resizable.defaults,$.fn.resizable.parseOptions(this),options || {});
                $.data(this,'resizable',{
                    options: opts
                });
            }

            if(opts.disabled == true) {
                return;
            }

            // bind mouse event using namespace resizable
            $(this).bind('mousemove.resizable',{ target: this },function(e) {
                //				if (isResizing) return;
                if($.fn.resizable.isResizing) { return }
                var dir = getDirection(e);
                if(dir == '') {
                    $(e.data.target).css('cursor','');
                } else {
                    $(e.data.target).css('cursor',dir + '-resize');
                }
            }).bind('mouseleave.resizable',{ target: this },function(e) {
                $(e.data.target).css('cursor','');
            }).bind('mousedown.resizable',{ target: this },function(e) {
                var dir = getDirection(e);
                if(dir == '') return;

                function getCssValue(css) {
                    var val = parseInt($(e.data.target).css(css));
                    if(isNaN(val)) {
                        return 0;
                    } else {
                        return val;
                    }
                }

                var data = {
                    target: e.data.target,
                    dir: dir,
                    startLeft: getCssValue('left'),
                    startTop: getCssValue('top'),
                    left: getCssValue('left'),
                    top: getCssValue('top'),
                    startX: e.pageX,
                    startY: e.pageY,
                    startWidth: $(e.data.target).outerWidth(),
                    startHeight: $(e.data.target).outerHeight(),
                    width: $(e.data.target).outerWidth(),
                    height: $(e.data.target).outerHeight(),
                    deltaWidth: $(e.data.target).outerWidth() - $(e.data.target).width(),
                    deltaHeight: $(e.data.target).outerHeight() - $(e.data.target).height()
                };
                $(document).bind('mousedown.resizable',data,doDown);
                $(document).bind('mousemove.resizable',data,doMove);
                $(document).bind('mouseup.resizable',data,doUp);
                $('body').css('cursor',dir + '-resize');
            });

            // get the resize direction
            function getDirection(e) {
                var tt = $(e.data.target);
                var dir = '';
                var offset = tt.offset();
                var width = tt.outerWidth();
                var height = tt.outerHeight();
                var edge = opts.edge;
                if(e.pageY > offset.top && e.pageY < offset.top + edge) {
                    dir += 'n';
                } else if(e.pageY < offset.top + height && e.pageY > offset.top + height - edge) {
                    dir += 's';
                }
                if(e.pageX > offset.left && e.pageX < offset.left + edge) {
                    dir += 'w';
                } else if(e.pageX < offset.left + width && e.pageX > offset.left + width - edge) {
                    dir += 'e';
                }

                var handles = opts.handles.split(',');
                for(var i = 0;i < handles.length;i++) {
                    var handle = handles[i].replace(/(^\s*)|(\s*$)/g,'');
                    if(handle == 'all' || handle == dir) {
                        return dir;
                    }
                }
                return '';
            }


        });
    };

    $.fn.resizable.methods = {
        options: function(jq) {
            return $.data(jq[0],'resizable').options;
        },
        enable: function(jq) {
            return jq.each(function() {
                $(this).resizable({ disabled: false });
            });
        },
        disable: function(jq) {
            return jq.each(function() {
                $(this).resizable({ disabled: true });
            });
        }
    };

    $.fn.resizable.parseOptions = function(target) {
        var t = $(target);
        return $.extend({},
				$.parser.parseOptions(target,[
					'handles',{ minWidth: 'number',minHeight: 'number',maxWidth: 'number',maxHeight: 'number',edge: 'number' }
				]),{
				    disabled: (t.attr('disabled') ? true : undefined)
				})
    };

    $.fn.resizable.defaults = {
        disabled: false,
        handles: 'n, e, s, w, ne, se, sw, nw, all',
        minWidth: 10,
        minHeight: 10,
        maxWidth: 10000,//$(document).width(),
        maxHeight: 10000,//$(document).height(),
        edge: 5,
        onStartResize: function(e) { },
        onResize: function(e) { },
        onStopResize: function(e) { }
    };

    $.fn.resizable.isResizing = false;

})(jQuery);

/****************************jquery.panel****************************/
(function($) {
    $.fn._remove = function() {
        return this.each(function() {
            $(this).remove();
            try {
                this.outerHTML = "";
            }
            catch(err) {
            }
        });
    };
    function _1(_2) {
        _2._remove();
    };
    function _3(_4,_5) {
        var _6 = $.data(_4,"panel");
        var _7 = _6.options;
        var _8 = _6.panel;
        var _9 = _8.children(".panel-header");
        var _a = _8.children(".panelx-body");
        var _b = _8.children(".panel-footer");
        if(_5) {
            $.extend(_7,{ width: _5.width,height: _5.height,minWidth: _5.minWidth,maxWidth: _5.maxWidth,minHeight: _5.minHeight,maxHeight: _5.maxHeight,left: _5.left,top: _5.top });
        }
        _8._size(_7);
        _9.add(_a)._outerWidth(_8.width());
        if(!isNaN(parseInt(_7.height))) {
            _a._outerHeight(_8.height() - _9._outerHeight() - _b._outerHeight());
        } else {
            _a.css("height","");
            var _c = $.parser.parseValue("minHeight",_7.minHeight,_8.parent());
            var _d = $.parser.parseValue("maxHeight",_7.maxHeight,_8.parent());
            var _e = _9._outerHeight() + _b._outerHeight() + _8._outerHeight() - _8.height();
            _a._size("minHeight",_c ? (_c - _e) : "");
            _a._size("maxHeight",_d ? (_d - _e) : "");
        }
        _8.css({ height: "",minHeight: "",maxHeight: "",left: _7.left,top: _7.top });
        _7.onResize.apply(_4,[_7.width,_7.height]);
        $(_4).panel("doLayout");
    };
    function _f(_10,_11) {
        var _12 = $.data(_10,"panel").options;
        var _13 = $.data(_10,"panel").panel;
        if(_11) {
            if(_11.left != null) {
                _12.left = _11.left;
            }
            if(_11.top != null) {
                _12.top = _11.top;
            }
        }
        _13.css({ left: _12.left,top: _12.top });
        _12.onMove.apply(_10,[_12.left,_12.top]);
    };
    function _14(_15) {
        $(_15).addClass("panelx-body")._size("clear");
        var _16 = $("<div class=\"panelx\"></div>").insertBefore(_15);
        _16[0].appendChild(_15);
        _16.bind("_resize",function(e,_17) {
            if($(this).hasClass("easyui-fluid") || _17) {
                _3(_15);
            }
            return false;
        });
        return _16;
    };
    function _18(_19) {
        var _1a = $.data(_19,"panel");
        var _1b = _1a.options;
        var _1c = _1a.panel;
        _1c.css(_1b.style);
        _1c.addClass(_1b.cls);
        _1d();
        _1e();
        var _1f = $(_19).panel("header");
        var _20 = $(_19).panel("body");
        var _21 = $(_19).siblings(".panel-footer");
        if(_1b.border) {
            _1f.removeClass("panel-header-noborder");
            _20.removeClass("panel-body-noborder");
            _21.removeClass("panel-footer-noborder");
        } else {
            _1f.addClass("panel-header-noborder");
            _20.addClass("panel-body-noborder");
            _21.addClass("panel-footer-noborder");
        }
        _1f.addClass(_1b.headerCls);
        _20.addClass(_1b.bodyCls);
        $(_19).attr("id",_1b.id || "");
        if(_1b.content) {
            $(_19).panel("clear");
            $(_19).html(_1b.content);
            $.parser.parse($(_19));
        }
        function _1d() {
            if(_1b.noheader || (!_1b.title && !_1b.header)) {
                _1(_1c.children(".panel-header"));
                _1c.children(".panelx-body").addClass("panel-body-noheader");
            } else {
                if(_1b.header) {
                    $(_1b.header).addClass("panel-header").prependTo(_1c);
                } else {
                    var _22 = _1c.children(".panel-header");
                    if(!_22.length) {
                        _22 = $("<div class=\"panel-header\"></div>").prependTo(_1c);
                    }
                    if(!$.isArray(_1b.tools)) {
                        _22.find("div.panel-tool .panel-tool-a").appendTo(_1b.tools);
                    }
                    _22.empty();
                    var _23 = $("<div class=\"panel-title\"></div>").html(_1b.title).appendTo(_22);
                    if(_1b.iconCls) {
                        _23.addClass("panel-with-icon");
                        $("<div class=\"panel-icon\"></div>").addClass(_1b.iconCls).appendTo(_22);
                    }
                    var _24 = $("<div class=\"panel-tool\"></div>").appendTo(_22);
                    _24.bind("click",function(e) {
                        e.stopPropagation();
                    });
                    if(_1b.tools) {
                        if($.isArray(_1b.tools)) {
                            $.map(_1b.tools,function(t) {
                                _25(_24,t.iconCls,eval(t.handler));
                            });
                        } else {
                            $(_1b.tools).children().each(function() {
                                $(this).addClass($(this).attr("iconCls")).addClass("panel-tool-a").appendTo(_24);
                            });
                        }
                    }
                    if(_1b.collapsible) {
                        _25(_24,"panel-tool-collapse",function() {
                            if(_1b.collapsed == true) {
                                _4d(_19,true);
                            } else {
                                _3b(_19,true);
                            }
                        });
                    }
                    if(_1b.minimizable) {
                        _25(_24,"panel-tool-min",function() {
                            _58(_19);
                        });
                    }
                    if(_1b.maximizable) {
                        _25(_24,"panel-tool-max",function() {
                            if(_1b.maximized == true) {
                                _5c(_19);
                            } else {
                                _3a(_19);
                            }
                        });
                    }
                    if(_1b.closable) {
                        _25(_24,"panel-tool-close",function() {
                            _3c(_19);
                        });
                    }
                }
                _1c.children("div.panelx-body").removeClass("panel-body-noheader");
            }
        };
        function _25(c,_26,_27) {
            var a = $("<a href=\"javascript:void(0)\"></a>").addClass(_26).appendTo(c);
            a.bind("click",_27);
        };
        function _1e() {
            if(_1b.footer) {
                $(_1b.footer).addClass("panel-footer").appendTo(_1c);
                $(_19).addClass("panel-body-nobottom");
            } else {
                _1c.children(".panel-footer").remove();
                $(_19).removeClass("panel-body-nobottom");
            }
        };
    };
    function _28(_29,_2a) {
        var _2b = $.data(_29,"panel");
        var _2c = _2b.options;
        if(_2d) {
            _2c.queryParams = _2a;
        }
        if(!_2c.href) {
            return;
        }
        if(!_2b.isLoaded || !_2c.cache) {
            var _2d = $.extend({},_2c.queryParams);
            if(_2c.onBeforeLoad.call(_29,_2d) == false) {
                return;
            }
            _2b.isLoaded = false;
            $(_29).panel("clear");
            if(_2c.loadingMessage) {
                $(_29).mask('正在加载...',200);
                //$(_29).html($("<div class=\"panel-loading\"></div>").html(_2c.loadingMessage));
            }
            _2c.loader.call(_29,_2d,function(_2e) {
                var _2f = _2c.extractor.call(_29,_2e);
                $(_29).html(_2f);
                $.parser.parse($(_29));
                _2c.onLoad.apply(_29,arguments);
                _2b.isLoaded = true;
                $(_29).unmask();
            }, function () {
                $(_29).unmask();
                _2c.onLoadError.apply(_29,arguments);
            });
        }
    };
    function _30(_31) {
        var t = $(_31);
        t.find(".combo-f").each(function() {
            $(this).combo("destroy");
        });
        t.find(".m-btn").each(function() {
            $(this).menubutton("destroy");
        });
        t.find(".s-btn").each(function() {
            $(this).splitbutton("destroy");
        });
        t.find(".tooltip-f").each(function() {
            $(this).tooltip("destroy");
        });
        t.children("div").each(function() {
            $(this)._size("unfit");
        });
        t.empty();
    };
    function _32(_33) {
        $(_33).panel("doLayout",true);
    };
    function _34(_35,_36) {
        var _37 = $.data(_35,"panel").options;
        var _38 = $.data(_35,"panel").panel;
        if(_36 != true) {
            if(_37.onBeforeOpen.call(_35) == false) {
                return;
            }
        }
        _38.stop(true,true);
        if($.isFunction(_37.openAnimation)) {
            _37.openAnimation.call(_35,cb);
        } else {
            switch(_37.openAnimation) {
                case "slide":
                    _38.slideDown(_37.openDuration,cb);
                    break;
                case "fade":
                    _38.fadeIn(_37.openDuration,cb);
                    break;
                case "show":
                    _38.show(_37.openDuration,cb);
                    break;
                default:
                    _38.show();
                    cb();
            }
        }
        function cb() {
            _37.closed = false;
            _37.minimized = false;
            var _39 = _38.children(".panel-header").find("a.panel-tool-restore");
            if(_39.length) {
                _37.maximized = true;
            }
            _37.onOpen.call(_35);
            if(_37.maximized == true) {
                _37.maximized = false;
                _3a(_35);
            }
            if(_37.collapsed == true) {
                _37.collapsed = false;
                _3b(_35);
            }
            if(!_37.collapsed) {
                _28(_35);
                _32(_35);
            }
        };
    };
    function _3c(_3d,_3e) {
        var _3f = $.data(_3d,"panel").options;
        var _40 = $.data(_3d,"panel").panel;
        if(_3e != true) {
            if(_3f.onBeforeClose.call(_3d) == false) {
                return;
            }
        }
        _40.stop(true,true);
        _40._size("unfit");
        if($.isFunction(_3f.closeAnimation)) {
            _3f.closeAnimation.call(_3d,cb);
        } else {
            switch(_3f.closeAnimation) {
                case "slide":
                    _40.slideUp(_3f.closeDuration,cb);
                    break;
                case "fade":
                    _40.fadeOut(_3f.closeDuration,cb);
                    break;
                case "hide":
                    _40.hide(_3f.closeDuration,cb);
                    break;
                default:
                    _40.hide();
                    cb();
            }
        }
        function cb() {
            _3f.closed = true;
            _3f.onClose.call(_3d);
        };
    };
    function _41(_42,_43) {
        var _44 = $.data(_42,"panel");
        var _45 = _44.options;
        var _46 = _44.panel;
        if(_43 != true) {
            if(_45.onBeforeDestroy.call(_42) == false) {
                return;
            }
        }
        $(_42).panel("clear").panel("clear","footer");
        _1(_46);
        _45.onDestroy.call(_42);
    };
    function _3b(_47,_48) {
        var _49 = $.data(_47,"panel").options;
        var _4a = $.data(_47,"panel").panel;
        var _4b = _4a.children(".panelx-body");
        var _4c = _4a.children(".panel-header").find("a.panel-tool-collapse");
        if(_49.collapsed == true) {
            return;
        }
        _4b.stop(true,true);
        if(_49.onBeforeCollapse.call(_47) == false) {
            return;
        }
        _4c.addClass("panel-tool-expand");
        if(_48 == true) {
            _4b.slideUp("normal",function() {
                _49.collapsed = true;
                _49.onCollapse.call(_47);
            });
        } else {
            _4b.hide();
            _49.collapsed = true;
            _49.onCollapse.call(_47);
        }
    };
    function _4d(_4e,_4f) {
        var _50 = $.data(_4e,"panel").options;
        var _51 = $.data(_4e,"panel").panel;
        var _52 = _51.children(".panelx-body");
        var _53 = _51.children(".panel-header").find("a.panel-tool-collapse");
        if(_50.collapsed == false) {
            return;
        }
        _52.stop(true,true);
        if(_50.onBeforeExpand.call(_4e) == false) {
            return;
        }
        _53.removeClass("panel-tool-expand");
        if(_4f == true) {
            _52.slideDown("normal",function() {
                _50.collapsed = false;
                _50.onExpand.call(_4e);
                _28(_4e);
                _32(_4e);
            });
        } else {
            _52.show();
            _50.collapsed = false;
            _50.onExpand.call(_4e);
            _28(_4e);
            _32(_4e);
        }
    };
    function _3a(_54) {
        var _55 = $.data(_54,"panel").options;
        var _56 = $.data(_54,"panel").panel;
        var _57 = _56.children(".panel-header").find("a.panel-tool-max");
        if(_55.maximized == true) {
            return;
        }
        _57.addClass("panel-tool-restore");
        if(!$.data(_54,"panel").original) {
            $.data(_54,"panel").original = { width: _55.width,height: _55.height,left: _55.left,top: _55.top,fit: _55.fit };
        }
        _55.left = 0;
        _55.top = 0;
        _55.fit = true;
        _3(_54);
        _55.minimized = false;
        _55.maximized = true;
        _55.onMaximize.call(_54);
    };
    function _58(_59) {
        var _5a = $.data(_59,"panel").options;
        var _5b = $.data(_59,"panel").panel;
        _5b._size("unfit");
        _5b.hide();
        _5a.minimized = true;
        _5a.maximized = false;
        _5a.onMinimize.call(_59);
    };
    function _5c(_5d) {
        var _5e = $.data(_5d,"panel").options;
        var _5f = $.data(_5d,"panel").panel;
        var _60 = _5f.children(".panel-header").find("a.panel-tool-max");
        if(_5e.maximized == false) {
            return;
        }
        _5f.show();
        _60.removeClass("panel-tool-restore");
        $.extend(_5e,$.data(_5d,"panel").original);
        _3(_5d);
        _5e.minimized = false;
        _5e.maximized = false;
        $.data(_5d,"panel").original = null;
        _5e.onRestore.call(_5d);
    };
    function _61(_62,_63) {
        $.data(_62,"panel").options.title = _63;
        $(_62).panel("header").find("div.panel-title").html(_63);
    };
    var _64 = null;
    $(window).unbind(".panelx").bind("resize.panel",function() {
        if(_64) {
            clearTimeout(_64);
        }
        _64 = setTimeout(function() {
            var _65 = $("body.layout");
            if(_65.length) {
                _65.layout("resize");
                $("body").children(".easyui-fluid:visible").each(function() {
                    $(this).triggerHandler("_resize");
                });
            } else {
                $("body").panel("doLayout");
            }
            _64 = null;
        },100);
    });
    $.fn.panel = function(_66,_67) {
        if(typeof _66 == "string") {
            return $.fn.panel.methods[_66](this,_67);
        }
        _66 = _66 || {};
        return this.each(function() {
            var _68 = $.data(this,"panel");
            var _69;
            if(_68) {
                _69 = $.extend(_68.options,_66);
                _68.isLoaded = false;
            } else {
                _69 = $.extend({},$.fn.panel.defaults,$.fn.panel.parseOptions(this),_66);
                $(this).attr("title","");
                _68 = $.data(this,"panel",{ options: _69,panel: _14(this),isLoaded: false });
            }
            _18(this);
            if(_69.doSize == true) {
                _68.panel.css("display","block");
                _3(this);
            }
            if(_69.closed == true || _69.minimized == true) {
                _68.panel.hide();
            } else {
                _34(this);
            }
        });
    };
    $.fn.panel.methods = {
        options: function(jq) {
            return $.data(jq[0],"panel").options;
        },panel: function(jq) {
            return $.data(jq[0],"panel").panel;
        },header: function(jq) {
            return $.data(jq[0],"panel").panel.children(".panel-header");
        },footer: function(jq) {
            return jq.panel("panel").children(".panel-footer");
        },body: function(jq) {
            return $.data(jq[0],"panel").panel.children(".panelx-body");
        },setTitle: function(jq,_6a) {
            return jq.each(function() {
                _61(this,_6a);
            });
        },open: function(jq,_6b) {
            return jq.each(function() {
                _34(this,_6b);
            });
        },close: function(jq,_6c) {
            return jq.each(function() {
                _3c(this,_6c);
            });
        },destroy: function(jq,_6d) {
            return jq.each(function() {
                _41(this,_6d);
            });
        },clear: function(jq,_6e) {
            return jq.each(function() {
                _30(_6e == "footer" ? $(this).panel("footer") : this);
            });
        },refresh: function(jq,_6f) {
            return jq.each(function() {
                var _70 = $.data(this,"panel");
                _70.isLoaded = false;
                if(_6f) {
                    if(typeof _6f == "string") {
                        _70.options.href = _6f;
                    } else {
                        _70.options.queryParams = _6f;
                    }
                }
                _28(this);
            });
        },resize: function(jq,_71) {
            return jq.each(function() {
                _3(this,_71);
            });
        },doLayout: function(jq,all) {
            return jq.each(function() {
                _72(this,"body");
                _72($(this).siblings(".panel-footer")[0],"footer");
                function _72(_73,_74) {
                    if(!_73) {
                        return;
                    }
                    var _75 = _73 == $("body")[0];
                    var s = $(_73).find("div.panelx:visible,div.accordion:visible,div.tabs-container:visible,div.layout:visible,.easyui-fluid:visible").filter(function(_76,el) {
                        var p = $(el).parents(".panelx-" + _74 + ":first");
                        return _75 ? p.length == 0 : p[0] == _73;
                    });
                    s.each(function() {
                        $(this).triggerHandler("_resize",[all || false]);
                    });
                };
            });
        },move: function(jq,_77) {
            return jq.each(function() {
                _f(this,_77);
            });
        },maximize: function(jq) {
            return jq.each(function() {
                _3a(this);
            });
        },minimize: function(jq) {
            return jq.each(function() {
                _58(this);
            });
        },restore: function(jq) {
            return jq.each(function() {
                _5c(this);
            });
        },collapse: function(jq,_78) {
            return jq.each(function() {
                _3b(this,_78);
            });
        },expand: function(jq,_79) {
            return jq.each(function() {
                _4d(this,_79);
            });
        }
    };
    $.fn.panel.parseOptions = function(_7a) {
        var t = $(_7a);
        var hh = t.children(".panel-header,header");
        var ff = t.children(".panel-footer,footer");
        return $.extend({},$.parser.parseOptions(_7a,["id","width","height","left","top","title","iconCls","cls","headerCls","bodyCls","tools","href","method","header","footer",{ cache: "boolean",fit: "boolean",border: "boolean",noheader: "boolean" },{ collapsible: "boolean",minimizable: "boolean",maximizable: "boolean" },{ closable: "boolean",collapsed: "boolean",minimized: "boolean",maximized: "boolean",closed: "boolean" },"openAnimation","closeAnimation",{ openDuration: "number",closeDuration: "number" },]),{ loadingMessage: (t.attr("loadingMessage") != undefined ? t.attr("loadingMessage") : undefined),header: (hh.length ? hh.removeClass("panel-header") : undefined),footer: (ff.length ? ff.removeClass("panel-footer") : undefined) });
    };
    $.fn.panel.defaults = {
        id: null,title: null,iconCls: null,width: "auto",height: "auto",left: null,top: null,cls: null,headerCls: null,bodyCls: null,style: {},href: null,cache: true,fit: false,border: true,doSize: true,noheader: false,content: null,collapsible: false,minimizable: false,maximizable: false,closable: false,collapsed: false,minimized: false,maximized: false,closed: false,openAnimation: false,openDuration: 400,closeAnimation: false,closeDuration: 400,tools: null,footer: null,header: null,queryParams: {},method: "get",href: null,loadingMessage: "Loading...",loader: function(_7b,_7c,_7d) {
            var _7e = $(this).panel("options");
            if(!_7e.href) {
                return false;
            }
            $.ajax({
                type: _7e.method,url: _7e.href,cache: false,data: _7b,dataType: "html",success: function(_7f) {
                    _7c(_7f);
                },error: function() {
                    _7d.apply(this,arguments);
                }
            });
        },extractor: function(_80) {
            var _81 = /<body[^>]*>((.|[\n\r])*)<\/body>/im;
            var _82 = _81.exec(_80);
            if(_82) {
                return _82[1];
            } else {
                return _80;
            }
        },onBeforeLoad: function(_83) {
        },onLoad: function() {
        },onLoadError: function() {
        },onBeforeOpen: function() {
        },onOpen: function() {
        },onBeforeClose: function() {
        },onClose: function() {
        },onBeforeDestroy: function() {
        },onDestroy: function() {
        },onResize: function(_84,_85) {
        },onMove: function(_86,top) {
        },onMaximize: function() {
        },onRestore: function() {
        },onMinimize: function() {
        },onBeforeCollapse: function() {
        },onBeforeExpand: function() {
        },onCollapse: function() {
        },onExpand: function() {
        }
    };
})(jQuery);

/****************************jquery.accordion****************************/
(function($) {

    function setSize(container,param) {
        var state = $.data(container,'accordion');
        var opts = state.options;
        var panels = state.panels;
        var cc = $(container);

        if(param) {
            $.extend(opts,{
                width: param.width,
                height: param.height
            });
        }
        cc._size(opts);
        var headerHeight = 0;
        var bodyHeight = 'auto';
        var headers = cc.find('>.panel>.accordion-header');
        if(headers.length) {
            headerHeight = $(headers[0]).css('height','')._outerHeight();
        }
        if(!isNaN(parseInt(opts.height))) {
            bodyHeight = cc.height() - headerHeight * headers.length;
        }

        _resize(true,bodyHeight - _resize(false) + 1);

        function _resize(collapsible,height) {
            var totalHeight = 0;
            for(var i = 0;i < panels.length;i++) {
                var p = panels[i];
                var h = p.panel('header')._outerHeight(headerHeight);
                if(p.panel('options').collapsible == collapsible) {
                    var pheight = isNaN(height) ? undefined : (height + headerHeight * h.length);
                    p.panel('resize',{
                        width: cc.width(),
                        height: (collapsible ? pheight : undefined)
                    });
                    totalHeight += p.panel('panel').outerHeight() - headerHeight * h.length;
                }
            }
            return totalHeight;
        }
    }

    /**
	 * find a panel by specified property, return the panel object or panel index.
	 */
    function findBy(container,property,value,all) {
        var panels = $.data(container,'accordion').panels;
        var pp = [];
        for(var i = 0;i < panels.length;i++) {
            var p = panels[i];
            if(property) {
                if(p.panel('options')[property] == value) {
                    pp.push(p);
                }
            } else {
                if(p[0] == $(value)[0]) {
                    return i;
                }
            }
        }
        if(property) {
            return all ? pp : (pp.length ? pp[0] : null);
        } else {
            return -1;
        }
    }

    function getSelections(container) {
        return findBy(container,'collapsed',false,true);
    }

    function getSelected(container) {
        var pp = getSelections(container);
        return pp.length ? pp[0] : null;
    }

    /**
	 * get panel index, start with 0
	 */
    function getPanelIndex(container,panel) {
        return findBy(container,null,panel);
    }

    /**
	 * get the specified panel.
	 */
    function getPanel(container,which) {
        var panels = $.data(container,'accordion').panels;
        if(typeof which == 'number') {
            if(which < 0 || which >= panels.length) {
                return null;
            } else {
                return panels[which];
            }
        }
        return findBy(container,'title',which);
    }

    function setProperties(container) {
        var opts = $.data(container,'accordion').options;
        var cc = $(container);
        if(opts.border) {
            cc.removeClass('accordion-noborder');
        } else {
            cc.addClass('accordion-noborder');
        }
    }

    function init(container) {
        var state = $.data(container,'accordion');
        var cc = $(container);
        cc.addClass('accordion');

        state.panels = [];
        cc.children('div').each(function() {
            var opts = $.extend({},$.parser.parseOptions(this),{
                selected: ($(this).attr('selected') ? true : undefined)
            });
            var pp = $(this);
            state.panels.push(pp);
            createPanel(container,pp,opts);
        });

        cc.bind('_resize',function(e,force) {
            if($(this).hasClass('easyui-fluid') || force) {
                setSize(container);
            }
            return false;
        });
    }

    function createPanel(container,pp,options) {
        var opts = $.data(container,'accordion').options;
        pp.panel($.extend({},{
            collapsible: true,
            minimizable: false,
            maximizable: false,
            closable: false,
            doSize: false,
            collapsed: true,
            headerCls: 'accordion-header',
            bodyCls: 'accordion-body'
        },options,{
            onBeforeExpand: function() {
                if(options.onBeforeExpand) {
                    if(options.onBeforeExpand.call(this) == false) { return false }
                }
                if(!opts.multiple) {
                    // get all selected panel
                    var all = $.grep(getSelections(container),function(p) {
                        return p.panel('options').collapsible;
                    });
                    for(var i = 0;i < all.length;i++) {
                        unselect(container,getPanelIndex(container,all[i]));
                    }
                }
                var header = $(this).panel('header');
                header.addClass('accordion-header-selected');
                header.find('.accordion-collapse').removeClass('accordion-expand');
            },
            onExpand: function() {
                if(options.onExpand) { options.onExpand.call(this) }
                opts.onSelect.call(container,$(this).panel('options').title,getPanelIndex(container,this));
            },
            onBeforeCollapse: function() {
                if(options.onBeforeCollapse) {
                    if(options.onBeforeCollapse.call(this) == false) { return false }
                }
                var header = $(this).panel('header');
                header.removeClass('accordion-header-selected');
                header.find('.accordion-collapse').addClass('accordion-expand');
            },
            onCollapse: function() {
                if(options.onCollapse) { options.onCollapse.call(this) }
                opts.onUnselect.call(container,$(this).panel('options').title,getPanelIndex(container,this));
            }
        }));

        var header = pp.panel('header');
        var tool = header.children('div.panel-tool');
        tool.children('a.panel-tool-collapse').hide();	// hide the old collapse button
        var t = $('<a href="javascript:void(0)"></a>').addClass('accordion-collapse accordion-expand').appendTo(tool);
        t.bind('click',function() {
            togglePanel(pp);
            return false;
        });
        pp.panel('options').collapsible ? t.show() : t.hide();

        header.click(function() {
            togglePanel(pp);
            return false;
        });

        function togglePanel(p) {
            var popts = p.panel('options');
            if(popts.collapsible) {
                var index = getPanelIndex(container,p);
                if(popts.collapsed) {
                    select(container,index);
                } else {
                    unselect(container,index);
                }
            }
        }
    }

    /**
	 * select and set the specified panel active
	 */
    function select(container,which) {
        var p = getPanel(container,which);
        if(!p) { return }
        stopAnimate(container);
        var opts = $.data(container,'accordion').options;
        p.panel('expand',opts.animate);
    }

    function unselect(container,which) {
        var p = getPanel(container,which);
        if(!p) { return }
        stopAnimate(container);
        var opts = $.data(container,'accordion').options;
        p.panel('collapse',opts.animate);
    }

    function doFirstSelect(container) {
        var opts = $.data(container,'accordion').options;
        var p = findBy(container,'selected',true);
        if(p) {
            _select(getPanelIndex(container,p));
        } else {
            _select(opts.selected);
        }

        function _select(index) {
            var animate = opts.animate;
            opts.animate = false;
            select(container,index);
            opts.animate = animate;
        }
    }

    /**
	 * stop the animation of all panels
	 */
    function stopAnimate(container) {
        var panels = $.data(container,'accordion').panels;
        for(var i = 0;i < panels.length;i++) {
            panels[i].stop(true,true);
        }
    }

    function add(container,options) {
        var state = $.data(container,'accordion');
        var opts = state.options;
        var panels = state.panels;
        if(options.selected == undefined) options.selected = true;

        stopAnimate(container);

        var pp = $('<div></div>').appendTo(container);
        panels.push(pp);
        createPanel(container,pp,options);
        setSize(container);

        opts.onAdd.call(container,options.title,panels.length - 1);

        if(options.selected) {
            select(container,panels.length - 1);
        }
    }

    function remove(container,which) {
        var state = $.data(container,'accordion');
        var opts = state.options;
        var panels = state.panels;

        stopAnimate(container);

        var panel = getPanel(container,which);
        var title = panel.panel('options').title;
        var index = getPanelIndex(container,panel);

        if(!panel) { return }
        if(opts.onBeforeRemove.call(container,title,index) == false) { return }

        panels.splice(index,1);
        panel.panel('destroy');
        if(panels.length) {
            setSize(container);
            var curr = getSelected(container);
            if(!curr) {
                select(container,0);
            }
        }

        opts.onRemove.call(container,title,index);
    }

    $.fn.accordion = function(options,param) {
        if(typeof options == 'string') {
            return $.fn.accordion.methods[options](this,param);
        }

        options = options || {};
        return this.each(function() {
            var state = $.data(this,'accordion');
            if(state) {
                $.extend(state.options,options);
            } else {
                $.data(this,'accordion',{
                    options: $.extend({},$.fn.accordion.defaults,$.fn.accordion.parseOptions(this),options),
                    accordion: $(this).addClass('accordion'),
                    panels: []
                });
                init(this);
            }

            setProperties(this);
            setSize(this);
            doFirstSelect(this);
        });
    };

    $.fn.accordion.methods = {
        options: function(jq) {
            return $.data(jq[0],'accordion').options;
        },
        panels: function(jq) {
            return $.data(jq[0],'accordion').panels;
        },
        resize: function(jq,param) {
            return jq.each(function() {
                setSize(this,param);
            });
        },
        getSelections: function(jq) {
            return getSelections(jq[0]);
        },
        getSelected: function(jq) {
            return getSelected(jq[0]);
        },
        getPanel: function(jq,which) {
            return getPanel(jq[0],which);
        },
        getPanelIndex: function(jq,panel) {
            return getPanelIndex(jq[0],panel);
        },
        select: function(jq,which) {
            return jq.each(function() {
                select(this,which);
            });
        },
        unselect: function(jq,which) {
            return jq.each(function() {
                unselect(this,which);
            });
        },
        add: function(jq,options) {
            return jq.each(function() {
                add(this,options);
            });
        },
        remove: function(jq,which) {
            return jq.each(function() {
                remove(this,which);
            });
        }
    };

    $.fn.accordion.parseOptions = function(target) {
        var t = $(target);
        return $.extend({},$.parser.parseOptions(target,[
			'width','height',
			{ fit: 'boolean',border: 'boolean',animate: 'boolean',multiple: 'boolean',selected: 'number' }
        ]));
    };

    $.fn.accordion.defaults = {
        width: 'auto',
        height: 'auto',
        fit: false,
        border: true,
        animate: true,
        multiple: false,
        selected: 0,

        onSelect: function(title,index) { },
        onUnselect: function(title,index) { },
        onAdd: function(title,index) { },
        onBeforeRemove: function(title,index) { },
        onRemove: function(title,index) { }
    };
})(jQuery);

/****************************jquery.tabs****************************/
(function($) {
    function getContentWidth(c) {
        var w = 0;
        $(c).children().each(function() {
            w += $(this).outerWidth(true);
        });
        return w;
    }
    /**
	 * set the tabs scrollers to show or not,
	 * dependent on the tabs count and width
	 */
    function setScrollers(container) {
        var opts = $.data(container,'tabs').options;
        if(opts.tabPosition == 'left' || opts.tabPosition == 'right' || !opts.showHeader) { return }

        var header = $(container).children('div.tabs-header');
        var tool = header.children('div.tabs-tool:not(.tabs-tool-hidden)');
        var sLeft = header.children('div.tabs-scroller-left');
        var sRight = header.children('div.tabs-scroller-right');
        var wrap = header.children('div.tabs-wrap');

        // set the tool height
        var tHeight = header.outerHeight();
        if(opts.plain) {
            tHeight -= tHeight - header.height();
        }
        tool._outerHeight(tHeight);

        var tabsWidth = getContentWidth(header.find('ul.tabs'));
        var cWidth = header.width() - tool._outerWidth();

        if(tabsWidth > cWidth) {
            sLeft.add(sRight).show()._outerHeight(tHeight);
            if(opts.toolPosition == 'left') {
                tool.css({
                    left: sLeft.outerWidth(),
                    right: ''
                });
                wrap.css({
                    marginLeft: sLeft.outerWidth() + tool._outerWidth(),
                    marginRight: sRight._outerWidth(),
                    width: cWidth - sLeft.outerWidth() - sRight.outerWidth()
                });
            } else {
                tool.css({
                    left: '',
                    right: sRight.outerWidth()
                });
                wrap.css({
                    marginLeft: sLeft.outerWidth(),
                    marginRight: sRight.outerWidth() + tool._outerWidth(),
                    width: cWidth - sLeft.outerWidth() - sRight.outerWidth()
                });
            }
        } else {
            sLeft.add(sRight).hide();
            if(opts.toolPosition == 'left') {
                tool.css({
                    left: 0,
                    right: ''
                });
                wrap.css({
                    marginLeft: tool._outerWidth(),
                    marginRight: 0,
                    width: cWidth
                });
            } else {
                tool.css({
                    left: '',
                    right: 0
                });
                wrap.css({
                    marginLeft: 0,
                    marginRight: tool._outerWidth(),
                    width: cWidth
                });
            }
        }
    }

    function addTools(container) {
        var opts = $.data(container,'tabs').options;
        var header = $(container).children('div.tabs-header');
        if(opts.tools) {
            if(typeof opts.tools == 'string') {
                $(opts.tools).addClass('tabs-tool').appendTo(header);
                $(opts.tools).show();
            } else {
                header.children('div.tabs-tool').remove();
                var tools = $('<div class="tabs-tool"><table cellspacing="0" cellpadding="0" style="height:100%"><tr></tr></table></div>').appendTo(header);
                var tr = tools.find('tr');
                for(var i = 0;i < opts.tools.length;i++) {
                    var td = $('<td></td>').appendTo(tr);
                    var tool = $('<a href="javascript:void(0);"></a>').appendTo(td);
                    tool[0].onclick = eval(opts.tools[i].handler || function() { });
                    tool.linkbutton($.extend({},opts.tools[i],{
                        plain: true
                    }));
                }
            }
        } else {
            header.children('div.tabs-tool').remove();
        }
    }

    function setSize(container,param) {
        var state = $.data(container,'tabs');
        var opts = state.options;
        var cc = $(container);

        if(!opts.doSize) { return }
        if(param) {
            $.extend(opts,{
                width: param.width,
                height: param.height
            });
        }
        cc._size(opts);

        var header = cc.children('div.tabs-header');
        var panels = cc.children('div.tabs-panels');
        var wrap = header.find('div.tabs-wrap');
        var ul = wrap.find('.tabs');
        ul.children('li').removeClass('tabs-first tabs-last');
        ul.children('li:first').addClass('tabs-first');
        ul.children('li:last').addClass('tabs-last');

        if(opts.tabPosition == 'left' || opts.tabPosition == 'right') {
            header._outerWidth(opts.showHeader ? opts.headerWidth : 0);
            panels._outerWidth(cc.width() - header.outerWidth());
            header.add(panels)._size('height',isNaN(parseInt(opts.height)) ? '' : cc.height());
            wrap._outerWidth(header.width());
            ul._outerWidth(wrap.width()).css('height','');
        } else {
            header.children('div.tabs-scroller-left,div.tabs-scroller-right,div.tabs-tool:not(.tabs-tool-hidden)').css('display',opts.showHeader ? 'block' : 'none');
            header._outerWidth(cc.width()).css('height','');
            if(opts.showHeader) {
                header.css('background-color','');
                wrap.css('height','');
            } else {
                header.css('background-color','transparent');
                header._outerHeight(0);
                wrap._outerHeight(0);
            }
            ul._outerHeight(opts.tabHeight).css('width','');
            ul._outerHeight(ul.outerHeight() - ul.height() - 1 + opts.tabHeight).css('width','');

            panels._size('height',isNaN(parseInt(opts.height)) ? '' : (cc.height() - header.outerHeight()));
            panels._size('width',cc.width());
        }

        if(state.tabs.length) {
            var d1 = ul.outerWidth(true) - ul.width();
            var li = ul.children('li:first');
            var d2 = li.outerWidth(true) - li.width();
            var hwidth = header.width() - header.children('.tabs-tool:not(.tabs-tool-hidden)')._outerWidth();
            var justifiedWidth = Math.floor((hwidth - d1 - d2 * state.tabs.length) / state.tabs.length);

            $.map(state.tabs,function(p) {
                setTabSize(p,(opts.justified && $.inArray(opts.tabPosition,['top','bottom']) >= 0) ? justifiedWidth : undefined);
            });
            if(opts.justified && $.inArray(opts.tabPosition,['top','bottom']) >= 0) {
                var deltaWidth = hwidth - d1 - getContentWidth(ul);
                setTabSize(state.tabs[state.tabs.length - 1],justifiedWidth + deltaWidth);
            }
        }
        setScrollers(container);

        function setTabSize(p,width) {
            var p_opts = p.panel('options');
            var p_t = p_opts.tab.find('a.tabs-inner');
            var width = width ? width : (parseInt(p_opts.tabWidth || opts.tabWidth || undefined));
            if(width) {
                p_t._outerWidth(width);
            } else {
                p_t.css('width','');
            }
            p_t._outerHeight(opts.tabHeight);
            p_t.css('lineHeight',p_t.height() + 'px');
            p_t.find('.easyui-fluid:visible').triggerHandler('_resize');
        }
    }

    /**
	 * set selected tab panel size
	 */
    function setSelectedSize(container) {
        var opts = $.data(container,'tabs').options;
        var tab = getSelectedTab(container);
        if(tab) {
            var panels = $(container).children('div.tabs-panels');
            var width = opts.width == 'auto' ? 'auto' : panels.width();
            var height = opts.height == 'auto' ? 'auto' : panels.height();
            tab.panel('resize',{
                width: width,
                height: height
            });
        }
    }

    /**
	 * wrap the tabs header and body
	 */
    function wrapTabs(container) {
        var tabs = $.data(container,'tabs').tabs;
        var cc = $(container).addClass('tabs-container');
        var panels = $('<div class="tabs-panels"></div>').insertBefore(cc);
        cc.children('div').each(function() {
            panels[0].appendChild(this);
        });
        cc[0].appendChild(panels[0]);
        $('<div class="tabs-header">'
				+ '<div class="tabs-scroller-left"></div>'
				+ '<div class="tabs-scroller-right"></div>'
				+ '<div class="tabs-wrap">'
				+ '<ul class="tabs"></ul>'
				+ '</div>'
				+ '</div>').prependTo(container);

        cc.children('div.tabs-panels').children('div').each(function(i) {
            var opts = $.extend({},$.parser.parseOptions(this),{
                disabled: ($(this).attr('disabled') ? true : undefined),
                selected: ($(this).attr('selected') ? true : undefined)
            });
            createTab(container,opts,$(this));
        });

        cc.children('div.tabs-header').find('.tabs-scroller-left, .tabs-scroller-right').hover(
				function() { $(this).addClass('tabs-scroller-over'); },
				function() { $(this).removeClass('tabs-scroller-over'); }
		);
        cc.bind('_resize',function(e,force) {
            if($(this).hasClass('easyui-fluid') || force) {
                setSize(container);
                setSelectedSize(container);
            }
            return false;
        });
    }

    function bindEvents(container) {
        var state = $.data(container,'tabs')
        var opts = state.options;
        $(container).children('div.tabs-header').unbind().bind('click',function(e) {
            if($(e.target).hasClass('tabs-scroller-left')) {
                $(container).tabs('scrollBy',-opts.scrollIncrement);
            } else if($(e.target).hasClass('tabs-scroller-right')) {
                $(container).tabs('scrollBy',opts.scrollIncrement);
            } else {
                var li = $(e.target).closest('li');
                if(li.hasClass('tabs-disabled')) { return false; }
                var a = $(e.target).closest('a.tabs-close');
                if(a.length) {
                    closeTab(container,getLiIndex(li));
                } else if(li.length) {
                    //					selectTab(container, getLiIndex(li));
                    var index = getLiIndex(li);
                    var popts = state.tabs[index].panel('options');
                    if(popts.collapsible) {
                        popts.closed ? selectTab(container,index) : unselectTab(container,index);
                    } else {
                        selectTab(container,index);
                    }
                }
                return false;
            }
        }).bind('contextmenu',function(e) {
            var li = $(e.target).closest('li');
            if(li.hasClass('tabs-disabled')) { return; }
            if(li.length) {
                opts.onContextMenu.call(container,e,li.find('span.tabs-title').html(),getLiIndex(li));
            }
        });

        function getLiIndex(li) {
            var index = 0;
            li.parent().children('li').each(function(i) {
                if(li[0] == this) {
                    index = i;
                    return false;
                }
            });
            return index;
        }
    }

    function setProperties(container) {
        var opts = $.data(container,'tabs').options;
        var header = $(container).children('div.tabs-header');
        var panels = $(container).children('div.tabs-panels');

        header.removeClass('tabs-header-top tabs-header-bottom tabs-header-left tabs-header-right');
        panels.removeClass('tabs-panels-top tabs-panels-bottom tabs-panels-left tabs-panels-right');
        if(opts.tabPosition == 'top') {
            header.insertBefore(panels);
        } else if(opts.tabPosition == 'bottom') {
            header.insertAfter(panels);
            header.addClass('tabs-header-bottom');
            panels.addClass('tabs-panels-top');
        } else if(opts.tabPosition == 'left') {
            header.addClass('tabs-header-left');
            panels.addClass('tabs-panels-right');
        } else if(opts.tabPosition == 'right') {
            header.addClass('tabs-header-right');
            panels.addClass('tabs-panels-left');
        }

        if(opts.plain == true) {
            header.addClass('tabs-header-plain');
        } else {
            header.removeClass('tabs-header-plain');
        }
        header.removeClass('tabs-header-narrow').addClass(opts.narrow ? 'tabs-header-narrow' : '');
        var tabs = header.find('.tabs');
        tabs.removeClass('tabs-pill').addClass(opts.pill ? 'tabs-pill' : '');
        tabs.removeClass('tabs-narrow').addClass(opts.narrow ? 'tabs-narrow' : '');
        tabs.removeClass('tabs-justified').addClass(opts.justified ? 'tabs-justified' : '');
        if(opts.border == true) {
            header.removeClass('tabs-header-noborder');
            panels.removeClass('tabs-panels-noborder');
        } else {
            header.addClass('tabs-header-noborder');
            panels.addClass('tabs-panels-noborder');
        }
        opts.doSize = true;
    }

    function createTab(container,options,pp) {
        options = options || {};
        var state = $.data(container,'tabs');
        var tabs = state.tabs;
        if(options.index == undefined || options.index > tabs.length) { options.index = tabs.length }
        if(options.index < 0) { options.index = 0 }

        var ul = $(container).children('div.tabs-header').find('ul.tabs');
        var panels = $(container).children('div.tabs-panels');
        var tab = $(
				'<li>' +
				'<a href="javascript:void(0)" class="tabs-inner">' +
				'<span class="tabs-title"></span>' +
				'<span class="tabs-icon"></span>' +
				'</a>' +
				'</li>');
        if(!pp) { pp = $('<div></div>'); }
        if(options.index >= tabs.length) {
            tab.appendTo(ul);
            pp.appendTo(panels);
            tabs.push(pp);
        } else {
            tab.insertBefore(ul.children('li:eq(' + options.index + ')'));
            pp.insertBefore(panels.children('div.panel:eq(' + options.index + ')'));
            tabs.splice(options.index,0,pp);
        }

        // create panel
        pp.panel($.extend({},options,{
            tab: tab,
            border: false,
            noheader: true,
            closed: true,
            doSize: false,
            iconCls: (options.icon ? options.icon : undefined),
            onLoad: function() {
                if(options.onLoad) {
                    options.onLoad.call(this,arguments);
                }
                state.options.onLoad.call(container,$(this));
            },
            onBeforeOpen: function() {
                if(options.onBeforeOpen) {
                    if(options.onBeforeOpen.call(this) == false) { return false; }
                }
                var p = $(container).tabs('getSelected');
                if(p) {
                    if(p[0] != this) {
                        $(container).tabs('unselect',getTabIndex(container,p));
                        p = $(container).tabs('getSelected');
                        if(p) {
                            return false;
                        }
                    } else {
                        setSelectedSize(container);
                        return false;
                    }
                }

                var popts = $(this).panel('options');
                popts.tab.addClass('tabs-selected');
                // scroll the tab to center position if required.
                var wrap = $(container).find('>div.tabs-header>div.tabs-wrap');
                var left = popts.tab.position().left;
                var right = left + popts.tab.outerWidth();
                if(left < 0 || right > wrap.width()) {
                    var deltaX = left - (wrap.width() - popts.tab.width()) / 2;
                    $(container).tabs('scrollBy',deltaX);
                } else {
                    $(container).tabs('scrollBy',0);
                }

                var panel = $(this).panel('panel');
                panel.css('display','block');
                setSelectedSize(container);
                panel.css('display','none');
            },
            onOpen: function() {
                if(options.onOpen) {
                    options.onOpen.call(this);
                }
                var popts = $(this).panel('options');
                state.selectHis.push(popts.title);
                state.options.onSelect.call(container,popts.title,getTabIndex(container,this));
            },
            onBeforeClose: function() {
                if(options.onBeforeClose) {
                    if(options.onBeforeClose.call(this) == false) { return false; }
                }
                $(this).panel('options').tab.removeClass('tabs-selected');
            },
            onClose: function() {
                if(options.onClose) {
                    options.onClose.call(this);
                }
                var popts = $(this).panel('options');
                state.options.onUnselect.call(container,popts.title,getTabIndex(container,this));
            }
        }));

        // only update the tab header
        $(container).tabs('update',{
            tab: pp,
            options: pp.panel('options'),
            type: 'header'
        });
    }

    function addTab(container,options) {
        var state = $.data(container,'tabs');
        var opts = state.options;
        if(options.selected == undefined) options.selected = true;

        createTab(container,options);
        opts.onAdd.call(container,options.title,options.index);
        if(options.selected) {
            selectTab(container,options.index);	// select the added tab panel
        }
    }

    /**
	 * update tab panel, param has following properties:
	 * tab: the tab panel to be updated
	 * options: the tab panel options
	 * type: the update type, possible values are: 'header','body','all'
	 */
    function updateTab(container,param) {
        param.type = param.type || 'all';
        var selectHis = $.data(container,'tabs').selectHis;
        var pp = param.tab;	// the tab panel
        var opts = pp.panel('options');	// get the tab panel options
        var oldTitle = opts.title;
        $.extend(opts,param.options,{
            iconCls: (param.options.icon ? param.options.icon : undefined)
        });

        if(param.type == 'all' || param.type == 'body') {
            pp.panel();
        }
        if(param.type == 'all' || param.type == 'header') {
            var tab = opts.tab;

            if(opts.header) {
                tab.find('.tabs-inner').html($(opts.header));
            } else {
                var s_title = tab.find('span.tabs-title');
                var s_icon = tab.find('span.tabs-icon');
                s_title.html(opts.title);
                s_icon.attr('class','tabs-icon');

                tab.find('a.tabs-close').remove();
                if(opts.closable) {
                    s_title.addClass('tabs-closable');
                    $('<a href="javascript:void(0)" class="tabs-close"></a>').appendTo(tab);
                } else {
                    s_title.removeClass('tabs-closable');
                }
                if(opts.iconCls) {
                    s_title.addClass('tabs-with-icon');
                    s_icon.addClass(opts.iconCls);
                } else {
                    s_title.removeClass('tabs-with-icon');
                }
                if(opts.tools) {
                    var p_tool = tab.find('span.tabs-p-tool');
                    if(!p_tool.length) {
                        var p_tool = $('<span class="tabs-p-tool"></span>').insertAfter(tab.find('a.tabs-inner'));
                    }
                    if($.isArray(opts.tools)) {
                        p_tool.empty();
                        for(var i = 0;i < opts.tools.length;i++) {
                            var t = $('<a href="javascript:void(0)"></a>').appendTo(p_tool);
                            t.addClass(opts.tools[i].iconCls);
                            if(opts.tools[i].handler) {
                                t.bind('click',{ handler: opts.tools[i].handler },function(e) {
                                    if($(this).parents('li').hasClass('tabs-disabled')) { return; }
                                    e.data.handler.call(this);
                                });
                            }
                        }
                    } else {
                        $(opts.tools).children().appendTo(p_tool);
                    }
                    var pr = p_tool.children().length * 12;
                    if(opts.closable) {
                        pr += 8;
                    } else {
                        pr -= 3;
                        p_tool.css('right','5px');
                    }
                    s_title.css('padding-right',pr + 'px');
                } else {
                    tab.find('span.tabs-p-tool').remove();
                    s_title.css('padding-right','');
                }
            }
            if(oldTitle != opts.title) {
                for(var i = 0;i < selectHis.length;i++) {
                    if(selectHis[i] == oldTitle) {
                        selectHis[i] = opts.title;
                    }
                }
            }
        }
        if(opts.disabled) {
            opts.tab.addClass('tabs-disabled');
        } else {
            opts.tab.removeClass('tabs-disabled');
        }

        setSize(container);

        $.data(container,'tabs').options.onUpdate.call(container,opts.title,getTabIndex(container,pp));
    }

    /**
	 * close a tab with specified index or title
	 */
    function closeTab(container,which) {
        var opts = $.data(container,'tabs').options;
        var tabs = $.data(container,'tabs').tabs;
        var selectHis = $.data(container,'tabs').selectHis;

        if(!exists(container,which)) return;

        var tab = getTab(container,which);
        var title = tab.panel('options').title;
        var index = getTabIndex(container,tab);

        if(opts.onBeforeClose.call(container,title,index) == false) return;

        var tab = getTab(container,which,true);
        tab.panel('options').tab.remove();
        tab.panel('destroy');

        opts.onClose.call(container,title,index);

        //		setScrollers(container);
        setSize(container);

        // remove the select history item
        for(var i = 0;i < selectHis.length;i++) {
            if(selectHis[i] == title) {
                selectHis.splice(i,1);
                i--;
            }
        }

        // select the nearest tab panel
        var hisTitle = selectHis.pop();
        if(hisTitle) {
            selectTab(container,hisTitle);
        } else if(tabs.length) {
            selectTab(container,0);
        }
    }

    /**
	 * get the specified tab panel
	 */
    function getTab(container,which,removeit) {
        var tabs = $.data(container,'tabs').tabs;
        if(typeof which == 'number') {
            if(which < 0 || which >= tabs.length) {
                return null;
            } else {
                var tab = tabs[which];
                if(removeit) {
                    tabs.splice(which,1);
                }
                return tab;
            }
        }
        for(var i = 0;i < tabs.length;i++) {
            var tab = tabs[i];
            if(tab.panel('options').title == which) {
                if(removeit) {
                    tabs.splice(i,1);
                }
                return tab;
            }
        }
        return null;
    }

    function getTabIndex(container,tab) {
        var tabs = $.data(container,'tabs').tabs;
        for(var i = 0;i < tabs.length;i++) {
            if(tabs[i][0] == $(tab)[0]) {
                return i;
            }
        }
        return -1;
    }

    function getSelectedTab(container) {
        var tabs = $.data(container,'tabs').tabs;
        for(var i = 0;i < tabs.length;i++) {
            var tab = tabs[i];
            if(tab.panel('options').tab.hasClass('tabs-selected')) {
                return tab;
            }
        }
        return null;
    }

    /**
	 * do first select action, if no tab is setted the first tab will be selected.
	 */
    function doFirstSelect(container) {
        var state = $.data(container,'tabs')
        var tabs = state.tabs;
        for(var i = 0;i < tabs.length;i++) {
            var opts = tabs[i].panel('options');
            if(opts.selected && !opts.disabled) {
                selectTab(container,i);
                return;
            }
        }
        selectTab(container,state.options.selected);
    }

    function selectTab(container,which) {
        var p = getTab(container,which);
        if(p && !p.is(':visible')) {
            stopAnimate(container);
            if(!p.panel('options').disabled)
                p.panel('open');
        }
    }

    function unselectTab(container,which) {
        var p = getTab(container,which);
        if(p && p.is(':visible')) {
            stopAnimate(container);
            p.panel('close');
        }
    }

    function stopAnimate(container) {
        $(container).children('div.tabs-panels').each(function() {
            $(this).stop(true,true);
        });
    }

    function exists(container,which) {
        return getTab(container,which) != null;
    }

    function showHeader(container,visible) {
        var opts = $.data(container,'tabs').options;
        opts.showHeader = visible;
        $(container).tabs('resize');
    }

    function showTool(container,visible) {
        var tool = $(container).find('>.tabs-header>.tabs-tool');
        if(visible) {
            tool.removeClass('tabs-tool-hidden').show();
        } else {
            tool.addClass('tabs-tool-hidden').hide();
        }
        $(container).tabs('resize').tabs('scrollBy',0);
    }


    $.fn.tabs = function(options,param) {
        if(typeof options == 'string') {
            return $.fn.tabs.methods[options](this,param);
        }

        options = options || {};
        return this.each(function() {
            var state = $.data(this,'tabs');
            if(state) {
                $.extend(state.options,options);
            } else {
                $.data(this,'tabs',{
                    options: $.extend({},$.fn.tabs.defaults,$.fn.tabs.parseOptions(this),options),
                    tabs: [],
                    selectHis: []
                });
                wrapTabs(this);
            }

            addTools(this);
            setProperties(this);
            setSize(this);
            bindEvents(this);

            doFirstSelect(this);
        });
    };

    $.fn.tabs.methods = {
        options: function(jq) {
            var cc = jq[0];
            var opts = $.data(cc,'tabs').options;
            var s = getSelectedTab(cc);
            opts.selected = s ? getTabIndex(cc,s) : -1;
            return opts;
        },
        tabs: function(jq) {
            return $.data(jq[0],'tabs').tabs;
        },
        resize: function(jq,param) {
            return jq.each(function() {
                setSize(this,param);
                setSelectedSize(this);
            });
        },
        add: function(jq,options) {
            return jq.each(function() {
                addTab(this,options);
            });
        },
        close: function(jq,which) {
            return jq.each(function() {
                closeTab(this,which);
            });
        },
        getTab: function(jq,which) {
            return getTab(jq[0],which);
        },
        getTabIndex: function(jq,tab) {
            return getTabIndex(jq[0],tab);
        },
        getSelected: function(jq) {
            return getSelectedTab(jq[0]);
        },
        select: function(jq,which) {
            return jq.each(function() {
                selectTab(this,which);
            });
        },
        unselect: function(jq,which) {
            return jq.each(function() {
                unselectTab(this,which);
            });
        },
        exists: function(jq,which) {
            return exists(jq[0],which);
        },
        update: function(jq,options) {
            return jq.each(function() {
                updateTab(this,options);
            });
        },
        enableTab: function(jq,which) {
            return jq.each(function() {
                var opts = $(this).tabs('getTab',which).panel('options');
                opts.tab.removeClass('tabs-disabled');
                opts.disabled = false;
            });
        },
        disableTab: function(jq,which) {
            return jq.each(function() {
                var opts = $(this).tabs('getTab',which).panel('options');
                opts.tab.addClass('tabs-disabled');
                opts.disabled = true;
            });
        },
        showHeader: function(jq) {
            return jq.each(function() {
                showHeader(this,true);
            });
        },
        hideHeader: function(jq) {
            return jq.each(function() {
                showHeader(this,false);
            });
        },
        showTool: function(jq) {
            return jq.each(function() {
                showTool(this,true);
            });
        },
        hideTool: function(jq) {
            return jq.each(function() {
                showTool(this,false);
            });
        },
        scrollBy: function(jq,deltaX) {	// scroll the tab header by the specified amount of pixels
            return jq.each(function() {
                var opts = $(this).tabs('options');
                var wrap = $(this).find('>div.tabs-header>div.tabs-wrap');
                var pos = Math.min(wrap._scrollLeft() + deltaX,getMaxScrollWidth());
                wrap.animate({ scrollLeft: pos },opts.scrollDuration);

                function getMaxScrollWidth() {
                    var w = 0;
                    var ul = wrap.children('ul');
                    ul.children('li').each(function() {
                        w += $(this).outerWidth(true);
                    });
                    return w - wrap.width() + (ul.outerWidth() - ul.width());
                }
            });
        }
    };

    $.fn.tabs.parseOptions = function(target) {
        return $.extend({},$.parser.parseOptions(target,[
			'tools','toolPosition','tabPosition',
			{ fit: 'boolean',border: 'boolean',plain: 'boolean' },
			{ headerWidth: 'number',tabWidth: 'number',tabHeight: 'number',selected: 'number' },
			{ showHeader: 'boolean',justified: 'boolean',narrow: 'boolean',pill: 'boolean' }
        ]));
    };

    $.fn.tabs.defaults = {
        width: 'auto',
        height: 'auto',
        headerWidth: 150,	// the tab header width, it is valid only when tabPosition set to 'left' or 'right' 
        tabWidth: 'auto',	// the tab width
        tabHeight: 27,		// the tab height
        selected: 0,		// the initialized selected tab index
        showHeader: true,
        plain: false,
        fit: false,
        border: true,
        justified: false,
        narrow: false,
        pill: false,
        tools: null,
        toolPosition: 'right',	// left,right
        tabPosition: 'top',		// possible values: top,bottom
        scrollIncrement: 100,
        scrollDuration: 400,
        onLoad: function(panel) { },
        onSelect: function(title,index) { },
        onUnselect: function(title,index) { },
        onBeforeClose: function(title,index) { },
        onClose: function(title,index) { },
        onAdd: function(title,index) { },
        onUpdate: function(title,index) { },
        onContextMenu: function(e,title,index) { }
    };
})(jQuery);

/****************************jquery.layout****************************/
(function($) {
    var _1 = false;
    function _2(_3,_4) {
        var _5 = $.data(_3,"layout");
        var _6 = _5.options;
        var _7 = _5.panels;
        var cc = $(_3);
        if(_4) {
            $.extend(_6,{ width: _4.width,height: _4.height });
        }
        if(_3.tagName.toLowerCase() == "body") {
            cc._size("fit");
        } else {
            cc._size(_6);
        }
        var _8 = { top: 0,left: 0,width: cc.width(),height: cc.height() };
        _9(_a(_7.expandNorth) ? _7.expandNorth : _7.north,"n");
        _9(_a(_7.expandSouth) ? _7.expandSouth : _7.south,"s");
        _b(_a(_7.expandEast) ? _7.expandEast : _7.east,"e");
        _b(_a(_7.expandWest) ? _7.expandWest : _7.west,"w");
        _7.center.panel("resize",_8);
        function _9(pp,_c) {
            if(!pp.length || !_a(pp)) {
                return;
            }
            var _d = pp.panel("options");
            pp.panel("resize",{ width: cc.width(),height: _d.height });
            var _e = pp.panel("panel").outerHeight();
            pp.panel("move",{ left: 0,top: (_c == "n" ? 0 : cc.height() - _e) });
            _8.height -= _e;
            if(_c == "n") {
                _8.top += _e;
                if(!_d.split && _d.border) {
                    _8.top--;
                }
            }
            if(!_d.split && _d.border) {
                _8.height++;
            }
        };
        function _b(pp,_f) {
            if(!pp.length || !_a(pp)) {
                return;
            }
            var _10 = pp.panel("options");
            pp.panel("resize",{ width: _10.width,height: _8.height });
            var _11 = pp.panel("panel").outerWidth();
            pp.panel("move",{ left: (_f == "e" ? cc.width() - _11 : 0),top: _8.top });
            _8.width -= _11;
            if(_f == "w") {
                _8.left += _11;
                if(!_10.split && _10.border) {
                    _8.left--;
                }
            }
            if(!_10.split && _10.border) {
                _8.width++;
            }
        };
    };
    function _12(_13) {
        var cc = $(_13);
        cc.addClass("layout");
        function _14(cc) {
            var _15 = cc.layout("options");
            var _16 = _15.onAdd;
            _15.onAdd = function() {
            };
            cc.children("div").each(function() {
                var _17 = $.fn.layout.parsePanelOptions(this);
                if("north,south,east,west,center".indexOf(_17.region) >= 0) {
                    _19(_13,_17,this);
                }
            });
            _15.onAdd = _16;
        };
        cc.children("form").length ? _14(cc.children("form")) : _14(cc);
        cc.append("<div class=\"layout-split-proxy-h\"></div><div class=\"layout-split-proxy-v\"></div>");
        cc.bind("_resize",function(e,_18) {
            if($(this).hasClass("easyui-fluid") || _18) {
                _2(_13);
            }
            return false;
        });
    };
    function _19(_1a,_1b,el) {
        _1b.region = _1b.region || "center";
        var _1c = $.data(_1a,"layout").panels;
        var cc = $(_1a);
        var dir = _1b.region;
        if(_1c[dir].length) {
            return;
        }
        var pp = $(el);
        if(!pp.length) {
            pp = $("<div></div>").appendTo(cc);
        }
        var _1d = $.extend({},$.fn.layout.paneldefaults,{
            width: (pp.length ? parseInt(pp[0].style.width) || pp.outerWidth() : "auto"),height: (pp.length ? parseInt(pp[0].style.height) || pp.outerHeight() : "auto"),doSize: false,collapsible: true,onOpen: function() {
                var _1e = $(this).panel("header").children("div.panel-tool");
                _1e.children("a.panel-tool-collapse").hide();
                var _1f = { north: "up",south: "down",east: "right",west: "left" };
                if(!_1f[dir]) {
                    return;
                }
                var _20 = "layout-button-" + _1f[dir];
                var t = _1e.children("a." + _20);
                if(!t.length) {
                    t = $("<a href=\"javascript:void(0)\"></a>").addClass(_20).appendTo(_1e);
                    t.bind("click",{ dir: dir },function(e) {
                        _2d(_1a,e.data.dir);
                        return false;
                    });
                }
                $(this).panel("options").collapsible ? t.show() : t.hide();
            }
        },_1b,{ cls: ((_1b.cls || "") + " layout-panel layout-panel-" + dir),bodyCls: ((_1b.bodyCls || "") + " layout-body") });
        pp.panel(_1d);
        _1c[dir] = pp;
        var _21 = { north: "s",south: "n",east: "w",west: "e" };
        var _22 = pp.panel("panel");
        if(pp.panel("options").split) {
            _22.addClass("layout-split-" + dir);
        }
        _22.resizable($.extend({},{
            handles: (_21[dir] || ""),disabled: (!pp.panel("options").split),onStartResize: function(e) {
                _1 = true;
                if(dir == "north" || dir == "south") {
                    var _23 = $(">div.layout-split-proxy-v",_1a);
                } else {
                    var _23 = $(">div.layout-split-proxy-h",_1a);
                }
                var top = 0,_24 = 0,_25 = 0,_26 = 0;
                var pos = { display: "block" };
                if(dir == "north") {
                    pos.top = parseInt(_22.css("top")) + _22.outerHeight() - _23.height();
                    pos.left = parseInt(_22.css("left"));
                    pos.width = _22.outerWidth();
                    pos.height = _23.height();
                } else {
                    if(dir == "south") {
                        pos.top = parseInt(_22.css("top"));
                        pos.left = parseInt(_22.css("left"));
                        pos.width = _22.outerWidth();
                        pos.height = _23.height();
                    } else {
                        if(dir == "east") {
                            pos.top = parseInt(_22.css("top")) || 0;
                            pos.left = parseInt(_22.css("left")) || 0;
                            pos.width = _23.width();
                            pos.height = _22.outerHeight();
                        } else {
                            if(dir == "west") {
                                pos.top = parseInt(_22.css("top")) || 0;
                                pos.left = _22.outerWidth() - _23.width();
                                pos.width = _23.width();
                                pos.height = _22.outerHeight();
                            }
                        }
                    }
                }
                _23.css(pos);
                $("<div class=\"layout-mask\"></div>").css({ left: 0,top: 0,width: cc.width(),height: cc.height() }).appendTo(cc);
            },onResize: function(e) {
                if(dir == "north" || dir == "south") {
                    var _27 = $(">div.layout-split-proxy-v",_1a);
                    _27.css("top",e.pageY - $(_1a).offset().top - _27.height() / 2);
                } else {
                    var _27 = $(">div.layout-split-proxy-h",_1a);
                    _27.css("left",e.pageX - $(_1a).offset().left - _27.width() / 2);
                }
                return false;
            },onStopResize: function(e) {
                cc.children("div.layout-split-proxy-v,div.layout-split-proxy-h").hide();
                pp.panel("resize",e.data);
                _2(_1a);
                _1 = false;
                cc.find(">div.layout-mask").remove();
            }
        },_1b));
        cc.layout("options").onAdd.call(_1a,dir);
    };
    function _28(_29,_2a) {
        var _2b = $.data(_29,"layout").panels;
        if(_2b[_2a].length) {
            _2b[_2a].panel("destroy");
            _2b[_2a] = $();
            var _2c = "expand" + _2a.substring(0,1).toUpperCase() + _2a.substring(1);
            if(_2b[_2c]) {
                _2b[_2c].panel("destroy");
                _2b[_2c] = undefined;
            }
            $(_29).layout("options").onRemove.call(_29,_2a);
        }
    };
    function _2d(_2e,_2f,_30) {
        if(_30 == undefined) {
            _30 = "normal";
        }
        var _31 = $.data(_2e,"layout").panels;
        var p = _31[_2f];
        var _32 = p.panel("options");
        if(_32.onBeforeCollapse.call(p) == false) {
            return;
        }
        var _33 = "expand" + _2f.substring(0,1).toUpperCase() + _2f.substring(1);
        if(!_31[_33]) {
            _31[_33] = _34(_2f);
            var ep = _31[_33].panel("panel");
            if(!_32.expandMode) {
                ep.css("cursor","default");
            } else {
                ep.bind("click",function() {
                    if(_32.expandMode == "dock") {
                        _41(_2e,_2f);
                    } else {
                        p.panel("expand",false).panel("open");
                        var _35 = _36();
                        p.panel("resize",_35.collapse);
                        p.panel("panel").animate(_35.expand,function() {
                            $(this).unbind(".layout").bind("mouseleave.layout",{ region: _2f },function(e) {
                                if(_1 == true) {
                                    return;
                                }
                                if($("body>div.combo-p>div.combo-panel:visible").length) {
                                    return;
                                }
                                _2d(_2e,e.data.region);
                            });
                            $(_2e).layout("options").onExpand.call(_2e,_2f);
                        });
                    }
                    return false;
                });
            }
        }
        var _37 = _36();
        if(!_a(_31[_33])) {
            _31.center.panel("resize",_37.resizeC);
        }
        p.panel("panel").animate(_37.collapse,_30,function() {
            p.panel("collapse",false).panel("close");
            _31[_33].panel("open").panel("resize",_37.expandP);
            $(this).unbind(".layout");
            $(_2e).layout("options").onCollapse.call(_2e,_2f);
        });
        function _34(dir) {
            var _38 = { "east": "left","west": "right","north": "down","south": "up" };
            var _39 = (_32.region == "north" || _32.region == "south");
            var _3a = "layout-button-" + _38[dir];
            var p = $("<div></div>").appendTo(_2e);
            p.panel($.extend({},$.fn.layout.paneldefaults,{
                cls: ("layout-expand layout-expand-" + dir),title: "&nbsp;",iconCls: (_32.hideCollapsedContent ? null : _32.iconCls),closed: true,minWidth: 0,minHeight: 0,doSize: false,region: _32.region,collapsedSize: _32.collapsedSize,noheader: (!_39 && _32.hideExpandTool),tools: ((_39 && _32.hideExpandTool) ? null : [{
                    iconCls: _3a,handler: function() {
                        _41(_2e,_2f);
                        return false;
                    }
                }])
            }));
            if(!_32.hideCollapsedContent) {
                var _3b = typeof _32.collapsedContent == "function" ? _32.collapsedContent.call(p[0],_32.title) : _32.collapsedContent;
                _39 ? p.panel("setTitle",_3b) : p.html(_3b);
            }
            p.panel("panel").hover(function() {
                $(this).addClass("layout-expand-over");
            },function() {
                $(this).removeClass("layout-expand-over");
            });
            return p;
        };
        function _36() {
            var cc = $(_2e);
            var _3c = _31.center.panel("options");
            var _3d = _32.collapsedSize;
            if(_2f == "east") {
                var _3e = p.panel("panel")._outerWidth();
                var _3f = _3c.width + _3e - _3d;
                if(_32.split || !_32.border) {
                    _3f++;
                }
                return { resizeC: { width: _3f },expand: { left: cc.width() - _3e },expandP: { top: _3c.top,left: cc.width() - _3d,width: _3d,height: _3c.height },collapse: { left: cc.width(),top: _3c.top,height: _3c.height } };
            } else {
                if(_2f == "west") {
                    var _3e = p.panel("panel")._outerWidth();
                    var _3f = _3c.width + _3e - _3d;
                    if(_32.split || !_32.border) {
                        _3f++;
                    }
                    return { resizeC: { width: _3f,left: _3d - 1 },expand: { left: 0 },expandP: { left: 0,top: _3c.top,width: _3d,height: _3c.height },collapse: { left: -_3e,top: _3c.top,height: _3c.height } };
                } else {
                    if(_2f == "north") {
                        var _40 = p.panel("panel")._outerHeight();
                        var hh = _3c.height;
                        if(!_a(_31.expandNorth)) {
                            hh += _40 - _3d + ((_32.split || !_32.border) ? 1 : 0);
                        }
                        _31.east.add(_31.west).add(_31.expandEast).add(_31.expandWest).panel("resize",{ top: _3d - 1,height: hh });
                        return { resizeC: { top: _3d - 1,height: hh },expand: { top: 0 },expandP: { top: 0,left: 0,width: cc.width(),height: _3d },collapse: { top: -_40,width: cc.width() } };
                    } else {
                        if(_2f == "south") {
                            var _40 = p.panel("panel")._outerHeight();
                            var hh = _3c.height;
                            if(!_a(_31.expandSouth)) {
                                hh += _40 - _3d + ((_32.split || !_32.border) ? 1 : 0);
                            }
                            _31.east.add(_31.west).add(_31.expandEast).add(_31.expandWest).panel("resize",{ height: hh });
                            return { resizeC: { height: hh },expand: { top: cc.height() - _40 },expandP: { top: cc.height() - _3d,left: 0,width: cc.width(),height: _3d },collapse: { top: cc.height(),width: cc.width() } };
                        }
                    }
                }
            }
        };
    };
    function _41(_42,_43) {
        var _44 = $.data(_42,"layout").panels;
        var p = _44[_43];
        var _45 = p.panel("options");
        if(_45.onBeforeExpand.call(p) == false) {
            return;
        }
        var _46 = "expand" + _43.substring(0,1).toUpperCase() + _43.substring(1);
        if(_44[_46]) {
            _44[_46].panel("close");
            p.panel("panel").stop(true,true);
            p.panel("expand",false).panel("open");
            var _47 = _48();
            p.panel("resize",_47.collapse);
            p.panel("panel").animate(_47.expand,function() {
                _2(_42);
                $(_42).layout("options").onExpand.call(_42,_43);
            });
        }
        function _48() {
            var cc = $(_42);
            var _49 = _44.center.panel("options");
            if(_43 == "east" && _44.expandEast) {
                return { collapse: { left: cc.width(),top: _49.top,height: _49.height },expand: { left: cc.width() - p.panel("panel")._outerWidth() } };
            } else {
                if(_43 == "west" && _44.expandWest) {
                    return { collapse: { left: -p.panel("panel")._outerWidth(),top: _49.top,height: _49.height },expand: { left: 0 } };
                } else {
                    if(_43 == "north" && _44.expandNorth) {
                        return { collapse: { top: -p.panel("panel")._outerHeight(),width: cc.width() },expand: { top: 0 } };
                    } else {
                        if(_43 == "south" && _44.expandSouth) {
                            return { collapse: { top: cc.height(),width: cc.width() },expand: { top: cc.height() - p.panel("panel")._outerHeight() } };
                        }
                    }
                }
            }
        };
    };
    function _a(pp) {
        if(!pp) {
            return false;
        }
        if(pp.length) {
            return pp.panel("panel").is(":visible");
        } else {
            return false;
        }
    };
    function _4a(_4b) {
        var _4c = $.data(_4b,"layout");
        var _4d = _4c.options;
        var _4e = _4c.panels;
        var _4f = _4d.onCollapse;
        _4d.onCollapse = function() {
        };
        _50("east");
        _50("west");
        _50("north");
        _50("south");
        _4d.onCollapse = _4f;
        function _50(_51) {
            var p = _4e[_51];
            if(p.length && p.panel("options").collapsed) {
                _2d(_4b,_51,0);
            }
        };
    };
    function _52(_53,_54,_55) {
        var p = $(_53).layout("panel",_54);
        p.panel("options").split = _55;
        var cls = "layout-split-" + _54;
        var _56 = p.panel("panel").removeClass(cls);
        if(_55) {
            _56.addClass(cls);
        }
        _56.resizable({ disabled: (!_55) });
        _2(_53);
    };
    $.fn.layout = function(_57,_58) {
        if(typeof _57 == "string") {
            return $.fn.layout.methods[_57](this,_58);
        }
        _57 = _57 || {};
        return this.each(function() {
            var _59 = $.data(this,"layout");
            if(_59) {
                $.extend(_59.options,_57);
            } else {
                var _5a = $.extend({},$.fn.layout.defaults,$.fn.layout.parseOptions(this),_57);
                $.data(this,"layout",{ options: _5a,panels: { center: $(),north: $(),south: $(),east: $(),west: $() } });
                _12(this);
            }
            _2(this);
            _4a(this);
        });
    };
    $.fn.layout.methods = {
        options: function(jq) {
            return $.data(jq[0],"layout").options;
        },resize: function(jq,_5b) {
            return jq.each(function() {
                _2(this,_5b);
            });
        },panel: function(jq,_5c) {
            return $.data(jq[0],"layout").panels[_5c];
        },collapse: function(jq,_5d) {
            return jq.each(function() {
                _2d(this,_5d);
            });
        },expand: function(jq,_5e) {
            return jq.each(function() {
                _41(this,_5e);
            });
        },add: function(jq,_5f) {
            return jq.each(function() {
                _19(this,_5f);
                _2(this);
                if($(this).layout("panel",_5f.region).panel("options").collapsed) {
                    _2d(this,_5f.region,0);
                }
            });
        },remove: function(jq,_60) {
            return jq.each(function() {
                _28(this,_60);
                _2(this);
            });
        },split: function(jq,_61) {
            return jq.each(function() {
                _52(this,_61,true);
            });
        },unsplit: function(jq,_62) {
            return jq.each(function() {
                _52(this,_62,false);
            });
        }
    };
    $.fn.layout.parseOptions = function(_63) {
        return $.extend({},$.parser.parseOptions(_63,[{ fit: "boolean" }]));
    };
    $.fn.layout.defaults = {
        fit: false,onExpand: function(_64) {
        },onCollapse: function(_65) {
        },onAdd: function(_66) {
        },onRemove: function(_67) {
        }
    };
    $.fn.layout.parsePanelOptions = function(_68) {
        var t = $(_68);
        return $.extend({},$.fn.panel.parseOptions(_68),$.parser.parseOptions(_68,["region",{ split: "boolean",collpasedSize: "number",minWidth: "number",minHeight: "number",maxWidth: "number",maxHeight: "number" }]));
    };
    $.fn.layout.paneldefaults = $.extend({},$.fn.panel.defaults,{
        region: null,split: false,collapsedSize: 28,expandMode: "float",hideExpandTool: false,hideCollapsedContent: true,collapsedContent: function(_69) {
            var p = $(this);
            var _6a = p.panel("options");
            if(_6a.region == "north" || _6a.region == "south") {
                return _69;
            }
            var _6b = _6a.collapsedSize - 2;
            var _6c = (_6b - 16) / 2;
            _6c = _6b - _6c;
            var cc = [];
            if(_6a.iconCls) {
                cc.push("<div class=\"panel-icon " + _6a.iconCls + "\"></div>");
            }
            cc.push("<div class=\"panel-title layout-expand-title");
            cc.push(_6a.iconCls ? " layout-expand-with-icon" : "");
            cc.push("\" style=\"left:" + _6c + "px\">");
            cc.push(_69);
            cc.push("</div>");
            return cc.join("");
        },minWidth: 10,minHeight: 10,maxWidth: 10000,maxHeight: 10000
    });
})(jQuery);

