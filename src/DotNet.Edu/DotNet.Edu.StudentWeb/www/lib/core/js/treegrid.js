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

})(jQuery)