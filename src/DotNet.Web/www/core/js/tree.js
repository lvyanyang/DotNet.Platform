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
                        node.state = 'closed';
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