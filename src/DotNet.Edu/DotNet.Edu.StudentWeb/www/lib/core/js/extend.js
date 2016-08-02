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
