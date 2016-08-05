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