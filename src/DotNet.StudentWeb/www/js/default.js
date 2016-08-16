$(function () {
            $(".leftmenu .meunbox li:not([class='h'])")
                .hover(
                    function () {
                        $(this).css("background-color", "#F5F5F5");
                    },
                    function () {
                        $(this).css("background-color", "#FFFFFF");
                    }
                );

            //所有提交按钮效果
            $("input[type='submit'],input[type='button']")
                .hover(
                    function () {
                        $(this).addClass("hover");
                    },
                    function () {
                        $(this).removeClass("hover");
                    }
                );
            $(".foot_list ul:odd li").css("width", 62);
            $(".weixin_img:last").css("margin-right", 0);

            //回到顶部组件出现设置
            $(window)
                .scroll(function () {
                    if ($(window).scrollTop() > 200) {
                        $(".back").fadeIn(400);
                    } else {
                        $(".back").fadeOut(400);
                    }
                });

            //回到顶部hover效果

            //设置滚回顶部方法
            $(".back").click(function () {
                $("body,html").animate({ scrollTop: 0 }, 500);
                return false;
            });
            $('.uitable').uitable();
        });