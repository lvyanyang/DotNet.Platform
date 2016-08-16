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



