$(function () {

    /* Navbar */
    WebAdmin.NavbarItemHome.on('click', function (event) {
        event.preventDefault();

        WebAdmin.NavbarActiveMenuItem($(this));
    });

    WebAdmin.NavbarDropdownClose.on('click', function(){
        WebAdmin.NavbarItem.removeClass('__active');
        WebAdmin.NavbarDropdown.removeClass('__opened');
    });

    /* Dashboard user panel */
    WebAdmin.DashboardAccount.on('click', function (event) {
        WebAdmin.DashboardOpenUser($(this));
    });

    /* Horizontal scroll for blades  */
    WebAdmin.HorizontalScrollForBlades('off');

    $(window).on('resize', function () {
        WebAdmin.HorizontalScrollForBlades('off');
    });

    $('.table .table-item').on('click', function () {
        $(this).addClass('__selected').siblings().removeClass('__selected');
    });

    $('.list-group').on('click', function () {
        var self = $(this);

        if(self.next().is(':hidden')) {
            self.addClass('__opened');
        }
        else {
            self.removeClass('__opened');
        }
    });

    $('.__images-list .tile').on('click', function () {
        $(this).toggleClass('__selected');
    });

    $('.list.__items, .dynamic-expression_b').on('contextmenu', function () {
        return false;
    });

    $('.list.__items').on('mousedown', function (event) {
        var leftPos = event.pageX,
            topPos = event.pageY;

        $('.menu.__context').remove();

        if (event.button == 2) {
            $(this).find('.list-item').prepend('<ul class="menu __context" style="display: block; left: ' + leftPos + 'px; top: ' + topPos + 'px;"><li class="menu-item"><i class="menu-ico fa fa-edit"></i> Manage</li><li class="menu-item"><i class="menu-ico fa fa-trash-o"></i> Delete</li></ul>');
        }
    });

    $('.dynamic-expression_b .add').on('mousedown', function (event) {
        var leftPos = event.pageX,
            topPos = event.pageY,
            self = $(this);

        $('.menu.__context').remove();

        if (event.button == 2) {
            self.after('<ul class="menu __context" style="display: block; left: ' + leftPos + 'px; top: ' + topPos + 'px;"><li class="menu-item __right"><i class="menu-ico fa fa-caret-right"></i> Cart item discount<ul class="menu __sub"><li class="menu-item">Get []% of [] items</li><li class="menu-item">Get $[] of [] items</li></ul></li></ul>');
        }
    });

    $('body').delegate('.gridster .list:not(.__editing) .gridster-item', 'mouseenter', function () {
        var self = $(this);

        self.prepend('<a class="customize">Customize</a>');

        $('.customize').on('click', function () {
            $('.dashboard .gridster ul').addClass('__editing');

            $('.nav-bar .bar').prepend('<li class="menu-item __done"><a class="menu-link"><span class="menu-ico fa fa-check"></span>Done</a></li>');

            $('.__done .menu-link').on('click', function () {
                $(this).remove();
                $('.dashboard .gridster ul').removeClass('__editing');
            });
        });

    }).delegate('.gridster-loaded .gridster-item', 'mouseleave', function () {
        $('.customize').remove();
    });

    $('.form-input.__number .down, .form-input.__number .up').on('click', function () {
        var step = $(this).parents('.form-input.__number').data('step'),
            value = $(this).parents('.form-input.__number').find('input').val();

            console.log(value)

        if($(this).hasClass('down')) {
            console.log(Math.floor(value + step))
            //$(this).parents('.form-input.__number').find('input').val(value-step);
        }
        else {
            //$(this).parents('.form-input.__number').find('input').val(value+step)
        }
    });

    $('*').on('mouseenter', function (event) {
        var blade = $(this).parents('.blade'),
            bladeI = blade.find('.blade-inner'),
            bladeH = bladeI.height(),
            bladeIh = blade.find('.inner-block').height(),
            dashboard = $(this).parents('.dashboard'),
            dashboardA = dashboard.find('.dashboard-area'),
            dashboardH = dashboardA.height(),
            dashboardIh = dashboard.find('.dashboard-inner').height();

        if (blade.length) {
            if (bladeH <= bladeIh) {
                WebAdmin.HorizontalScrollForBlades('off');
            }
            else {
                WebAdmin.HorizontalScrollForBlades('on');
            }
        }

        if (dashboard.length) {
            if (dashboardH <= dashboardIh) {
                WebAdmin.HorizontalScrollForBlades('off');
            }
            else {
                WebAdmin.HorizontalScrollForBlades('on');
            }
        }
    });

    $('.dashboard-head, .dashboard-head *, .blade-head, .blade-head *, .static, .static *').on('mouseenter', function (event) {
        WebAdmin.HorizontalScrollForBlades('on');
    });

    /* Tree view */
    WebAdmin.GetTreeItems();

    $('.tree-block').on('click', function () {
        WebAdmin.TreeItemSelected($(this));
    });

    $('.__files .list-img').on('mouseenter', function () {
        var self = $(this);

        self.after('<div class="image-preview"><img src="http://fakeimg.pl/350x350/00CED1/FFF"></div>');
    }).on('mouseleave', function () {
        var self = $(this);

        self.next().remove();
    });

    var localData = JSON.parse(localStorage.getItem('positions'));
        
    if(localData != null)
    {
        $.each(localData, function(i, value) {
            var id_name;

            id_name = "#";
            id_name = id_name + value.id;

            $(id_name).attr({ "data-col": value.col, "data-row": value.row, "data-sizex": value.size_x, "data-sizey": value.size_y });
        });
    }

    $('.form-input.__info .btn').on('click', function () {
        var self = $(this),
            posLeft = self.offset().left + 60,
            posTop = self.offset().top,
            toolText = self.find('.btn-ico').data('text');

        $('.tooltip').remove();

        $('body').prepend('<div class="tooltip" style="left: ' + posLeft + 'px; top: ' + posTop + 'px;"><div class="tooltip-cnt">' + toolText + '</div></div>');
    });

    /* Close dropdown if click in other area window */
    $(document).on("click", function (event) {
        if (!$('.nav-bar').is(event.target) && !$('.nav-bar').has(event.target).length) {
            if(WebAdmin.NavbarFlag) {
                WebAdmin.NavbarDropdown.removeClass('__opened');
                WebAdmin.NavbarItem.removeClass('__active');
            }

            WebAdmin.NavbarFlag = false;
        }

        if (!$('.dashboard .dashboard-account').is(event.target) && !$('.dashboard .dashboard-account').has(event.target).length) {
            if(WebAdmin.DashboardFlag) {
                WebAdmin.DashboardAccount.removeClass('__opened');
            }

            WebAdmin.DashboardFlag = false;
        }

        if (!$('.menu.__context').is(event.target) && !$('.menu.__context').has(event.target).length) {
            $('.menu.__context').remove();
        }

        if (!$('.tooltip, .btn').is(event.target) && !$('.tooltip, .btn').has(event.target).length) {
            $('.tooltip').remove();
        }
    });

});

var WebAdmin = {

    NavbarFlag: false,
    DashboardFlag: false,
    Speed: null,

    /* Elements for Navbar */
    NavbarItem: $('.nav-bar .menu-item'),
    NavbarItemHome: $('.nav-bar .menu-item:not(.__home)'),
    NavbarDropdown: $('.nav-bar .dropdown'),
    NavbarDropdownClose: $('.dropdown .dropdown-close'),

    /* Dashboard user */
    DashboardAccount: $('.dashboard .dashboard-account'),

    NavbarActiveMenuItem: function(self) {
        var name = $('.name', self);

        if (self.hasClass('__active')) {
            self.removeClass('__active');
            this.NavbarDropdown.removeClass('__opened');
        }
        else {
            this.NavbarItem.removeClass('__active');
            self.addClass('__active');
            this.NavbarDropdown.addClass('__opened');
        }

        this.NavbarFlag = true;
    },

    DashboardOpenUser: function(self) {
        if (self.hasClass('__opened')) {
            self.removeClass('__opened');
        }
        else {
            self.addClass('__opened');
        }

        this.DashboardFlag = true;
    },

    HorizontalScrollForBlades: function(flag) {
        this.Speed = (window.navigator.platform == 'MacIntel' ? .5 : 40);
        
        if (flag != 'off') {
            $('.cnt').off('mousewheel').on('mousewheel', function (event, delta) {
                this.scrollLeft -= (delta * WebAdmin.Speed);
                event.preventDefault();
            });
        }
        else {
            $('.cnt').unmousewheel();
        }
    },

    GetTreeItems: function(el) {
        el == el || '';
        $('.tree-node').each(function () {
        var node = $(this);
        if($('.tree-item', node).length <= 1) {
        $(this).find('.tree-item').addClass('last');
        }
        });
        var inW = $('.blade-inner .inner-block').width(),
        trW = $('.tree').width();
        if(inW < trW) {
        $('.tree-scroll').width(trW + 40);
        }
        else {
        $('.tree-scroll').width('auto');
        }
    },

    TreeItemSelected: function(self) {
        $('.tree-block').removeClass('__selected');
        self.addClass('__selected');
    },

};