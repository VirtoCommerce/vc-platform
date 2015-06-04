$(function (){

    Navbar(window.scrollY);
    checkScroll(window.scrollY);

    $(window).scroll(function () {
        var scrollTo = $(this).scrollTop();
        Navbar(scrollTo);
        checkScroll(scrollTo);
    });

    $(window).resize(function () {
        $(window).scroll(function () {
            var scrollTo = $(this).scrollTop();
            Navbar(scrollTo);
            checkScroll(scrollTo);
        });
    });

    $('.form-input.__info .btn').on('click', function () {
        var self = $(this),
            posLeft = self.offset().left + 60,
            posTop = self.offset().top,
            toolText = self.find('.btn-ico').data('text');

        $('.tooltip').remove();

        $('body').prepend('<div class="tooltip" style="left: ' + posLeft + 'px; top: ' + posTop + 'px;"><div class="tooltip-cnt">' + toolText + '</div></div>');
    });

    $('.navbar .menu-link').on('click', function () {
        var self = $(this).parent();
            type = self.data('type');

        $('.navbar .menu-item').removeClass('__selected');

        if(self.data('type') == type) {
            self.addClass('__selected');
        }

        $('.__sub .menu-link').on('click', function () {
            var self = $(this).parent(),
                type = self.data('type');

            if(self.data('type') == type) {
                self.addClass('__selected');
                self.parent().parent().addClass('__selected');
            }
        });

        $('html, body').animate({scrollTop: $('#' + type).offset().top + 'px'}, 0);
    });

    $('.tables-modificators').on('click', function () {
        $('html, body').animate({scrollTop: $('#tblmodificators').offset().top + 'px'}, 0);
    });

    $('.icons').on('click', function () {
        $('html, body').animate({scrollTop: $('#icons').offset().top + 'px'}, 0);
    });

    $('.tree-horizontal').on('click', function () {
        $('html, body').animate({scrollTop: $('#tree_h').offset().top + 'px'}, 0);
    });

    $('section').each(function () {
        var self = $(this),
            example = self.find('.example:not(.__bg)');

        example.before('<a class="preview-blade"><i class="fa fa-eye"></i>Preview in blade</a>');
    });

    $('body').delegate('.reverse', 'click', function () {
        $(this).prev().find('.tree').toggleClass('__horizontal');
    });

    $('.preview-blade').on('click', function () {
        var htmlCnt,
            self = $(this).next();

        if(self.find('form').length) {
            htmlCnt = self.html();
        }
        else {
            if(self.find('.list.__items').length || self.find('.table-wrapper').length || self.find('.pagination').length) {
                htmlCnt = self.html();
            }
            else {
                htmlCnt = '<div class="form">' + self.html() + '</div>';
            }
        }
        
        var html = '<div class="blade">';
            html += '<header class="blade-head">';
            html += '<div class="blade-t">';
            html += '<i class="blade-t_ico fa fa-file"></i>';
            html += '<div class="blade-t_head">Title</div>';
            html += '</div>';
            html += '<div class="blade-toolbar">';
            html += '<ul class="menu __inline">';
            html += '<li class="menu-item">';
            html += '<a class="menu-btn"><i class="menu-ico fa fa-edit"></i>Manage</a>';
            html += '</li>';
            html += '</ul>';
            html += '</div>';
            html += '</header>';
            html += '<div class="blade-container">';
            html += '<div class="blade-content">';
            html += '<div class="blade-inner">';
            html += '<div class="inner-block">' + htmlCnt + '</div>';
            html += '</div>';
            html += '</div>';
            html += '</div>';
            html += '</div>';

        $('body').append('<div class="popup-overlay"><div class="overlay-cnt"><div class="popup"><div class="t">Blade example</div></div></div></div>');
        $('.popup').append(html);
    });

    $('button').attr('type', 'button');
    
    $(document).on("click", function (event) {
        if (!$('.tooltip, .btn').is(event.target) && !$('.tooltip, .btn').has(event.target).length) {
            $('.tooltip').remove();
        }

        if (!$('.popup, .preview-blade').is(event.target) && !$('.popup, .preview-blade').has(event.target).length) {
            $('.popup-overlay').remove();
        }
    });

    //****** F U N C T I O N S

    function Navbar(scrollTop) {
        if(scrollTop >= 230) {
            $('.navbar > .menu').addClass('__fixed');
        }
        else {
            $('.navbar > .menu').removeClass('__fixed');
        }
    }

    function checkScroll(scrollTop) {
        // List types
        var types, defaultMargin, typesMain;
 
        types = [
            'icons',
            'typography',
            'headings',
            'paragraphs',
            'links',
            'forms',
            'fbasic',
            'ftwo_columns',
            'fhelp_info',
            'finput',
            'ftextarea',
            'fcheckbox',
            'fradio',
            'fswitch',
            'buttons',
            'bbasic',
            'bcancel',
            'bdisable',
            'tables',
            'tblbasic',
            'tblimage',
            'tbltext',
            'tblmodificators',
            'breadcrumbs',
            'pagination',
            'lists',
            'list_def',
            'list_info',
            'list_number',
            'list_tags',
            'list_files',
            'list_items',
            'list_items_text',
            'list_choosen',
            'list_checkbox',
            'list_radio',
            'tree',
            'tree_intro',
            'tree_v',
            'tree_h',
            'widgets',
            'blade',
            'blade_constructor'
        ];
     
        defaultMargin = 10;
     
        typesMain = {
            icons: {
                margin: 40
            },
            typography: {
                margin: 40
            },
            forms: {
                margin: 40
            },
            buttons: {
                margin: 40
            },
            tables: {
                margin: 40
            },
            breadcrumbs: {
                margin: 40
            },
            pagination: {
                margin: 40
            },
            lists: {
                margin: 40
            },
            tree: {
                margin: 40
            },
            widgets: {
                margin: 40
            },
            blade: {
                margin: 40
            }
        };
     
        for(var i = 0; i < types.length; i++) {
     
            var $type, margin, typeOffset, name;
     
            $type = $('#' + types[i]);
     
            if(!$type.length) {
                break;
            }
     
            margin = defaultMargin;
     
            // Margin
            if(types[i] in typesMain) {
                margin = typesMain[types[i]]['margin'];
            }

            typeOffset = $type.offset().top - margin;
            name = $type.prop('id');

            if(scrollTop >= typeOffset) {
                $('.navbar .__sub .menu-item').removeClass('__selected');
                $('.menu-item[data-type=' + name + ']').addClass('__selected').siblings().removeClass('__selected');
            }
            else {
                $('.menu-item[data-type=' + name + ']').removeClass('__selected');
            }
        }
    }

});