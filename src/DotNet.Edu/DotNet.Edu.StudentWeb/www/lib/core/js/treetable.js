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