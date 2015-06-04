var Blade = {

    settings: function() {
        var url = window.location.href.replace('index.html', '');
        $('#tplblade').load(url + '/templates/settings/blade.html');

        $('.settings .form-control input').on('click', function () {
            var type = $(this);
            
            if(type.prop('id') == 'headNav') {
                if (type.prop('checked')) {
                    // $('#tplblade .blade-nav').load(url + '/templates/settings/blade-nav.html');
                    $('#tplblade .blade-nav').append(123);
                    //$('.example.__construct .blade-nav').append(bladeNav);

                    Blade.saveCode();
                }
                else {
                    $('#tplblade .blade-nav .menu').remove();

                    Blade.saveCode();
                }
            }

            if(type.prop('id') == 'headIcon') {
                if (type.prop('checked')) {
                    $('.example.__construct .blade-t').append("<i class='blade-t_ico fa fa-folder'></i>");

                    Blade.saveCode();
                }
                else {
                    $('.example.__construct .blade-t .blade-t_ico').remove();

                    Blade.saveCode();
                }
            }

            if(type.prop('id') == 'headTitle') {
                if (type.prop('checked')) {
                    $('.example.__construct .blade-t').append("<div class='blade-t_head'>Title</div>");
                    $('#headDescr').prop('disabled', false);
                    $('#headDescr').parent().removeClass('__disabled');

                    Blade.saveCode();
                }
                else {
                    $('.example.__construct .blade-t .blade-t_head').remove();
                    $('#headDescr').prop('disabled', true);
                    $('#headDescr').parent().addClass('__disabled');

                    Blade.saveCode();
                }
            }

            if(type.prop('id') == 'headDescr') {
                if (type.prop('checked')) {
                    $('.example.__construct .blade-t').append("<div class='blade-t_subhead'>Description</div>");
                    $('.example.__construct .blade-t').addClass('__secondary');
                    $('#headTitle').prop('disabled', true);
                    $('#headTitle').parent().addClass('__disabled');

                    Blade.saveCode();
                }
                else {
                    $('.example.__construct .blade-t .blade-t_subhead').remove();
                    $('.example.__construct .blade-t').removeClass('__secondary');
                    $('#headTitle').prop('disabled', false);
                    $('#headTitle').parent().removeClass('__disabled');

                    Blade.saveCode();
                }
            }

            if(type.prop('id') == 'headToolbar') {
                if (type.prop('checked')) {
                    $('.example.__construct .blade-t').after(
                        "<div class='blade-toolbar'>" +
                            "<ul class='menu __inline'>" +
                                "<li class='menu-item'>" +
                                    "<a class='menu-btn'>" +
                                        "<i class='menu-ico fa fa-plus'></i> Add" +
                                    "</a>" +
                                "</li>" +
                            "</ul>" +
                        "</div>");

                    Blade.saveCode();
                }
                else {
                    $('.example.__construct .blade-toolbar').remove();

                    Blade.saveCode();
                }
            }

            if(type.prop('id') == 'staticTop') {
                if (type.prop('checked')) {
                    $('.example.__construct .blade-container').prepend("<div class='blade-static'></div>");
                    $('#collapsed, #expanded, #normal').prop('disabled', false);
                    $('#collapsed, #expanded, #normal').parent().removeClass('__disabled');
                                        
                    Blade.saveCode();
                }
                else {
                    $('.example.__construct .blade-static').remove();
                    $('#collapsed, #expanded, #normal').prop('disabled', true);
                    $('#collapsed, #expanded, #normal').parent().addClass('__disabled');

                    Blade.saveCode();
                }
            }

            if(type.prop('id') == 'normal') {
                if (type.prop('checked')) {
                    $('.example.__construct .blade-static').removeClass('__collapsed');
                    $('.example.__construct .blade-static').removeClass('__expanded');
                                        
                    Blade.saveCode();
                }
            }

            if(type.prop('id') == 'collapsed') {
                if (type.prop('checked')) {
                    $('.example.__construct .blade-static').addClass('__collapsed');
                    $('.example.__construct .blade-static').removeClass('__expanded');
                                        
                    Blade.saveCode();
                }
            }

            if(type.prop('id') == 'expanded') {
                if (type.prop('checked')) {
                    $('.example.__construct .blade-static').addClass('__expanded');
                    $('.example.__construct .blade-static').removeClass('__collapsed');
                                        
                    Blade.saveCode();
                }
            }

            if(type.prop('id') == 'wnormal') {
                if (type.prop('checked')) {
                    $('.example.__construct .blade-content').removeClass('__medium-wide');
                    $('.example.__construct .blade-content').removeClass('__large-wide');
                    $('.example.__construct .blade-content').removeClass('__xlarge-wide');
                                        
                    Blade.saveCode();
                }
            }

            if(type.prop('id') == 'wmedium') {
                if (type.prop('checked')) {
                    $('.example.__construct .blade-content').addClass('__medium-wide');
                    $('.example.__construct .blade-content').removeClass('__large-wide');
                    $('.example.__construct .blade-content').removeClass('__xlarge-wide');
                                        
                    Blade.saveCode();
                }
            }

            if(type.prop('id') == 'wlarge') {
                if (type.prop('checked')) {
                    $('.example.__construct .blade-content').addClass('__large-wide');
                    $('.example.__construct .blade-content').removeClass('__medium-wide');
                    $('.example.__construct .blade-content').removeClass('__xlarge-wide');
                                        
                    Blade.saveCode();
                }
            }

            if(type.prop('id') == 'wxlarge') {
                if (type.prop('checked')) {
                    $('.example.__construct .blade-content').addClass('__xlarge-wide');
                    $('.example.__construct .blade-content').removeClass('__medium-wide');
                    $('.example.__construct .blade-content').removeClass('__large-wide');
                                        
                    Blade.saveCode();
                }
            }
        });
    },

    saveCode: function() {
        var html = $('#tplblade').html();
        var lines = html.split('\n');

        console.log(html)
        
        var firstNotEmptyLine = lines[0];

        for (var i = 0; i < lines.length; i++) {
            if (lines[i].length > 0) {
                firstNotEmptyLine = lines[i];
                break;
            }
        }

        var exIndent = firstNotEmptyLine.substring(0, firstNotEmptyLine.indexOf('<'));
        
        html = '';

        for (var i = 0; i < lines.length; i++) {
            lines[i] = lines[i].substring(exIndent.length);
            html += lines[i] + '\n';
        }

        $('.code.__construct').html('<code class="language-markup">' + Blade.htmlSpecialChars(html) + '</code>');

        Prism.highlightAll();
    },

    htmlSpecialChars: function(text) {
        return text
          .replace(/&/g, "&amp;")
          .replace(/</g, "&lt;")
          .replace(/>/g, "&gt;")
          .replace(/"/g, "&quot;")
          .replace(/'/g, "&#039;");
    }

};

$(function () {

    $('.settings input[type=checkbox]').prop('checked', false);
    $('#wnormal, #normal').prop('checked', true);

    Blade.settings();
});