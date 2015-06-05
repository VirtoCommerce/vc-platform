var Blade = {

    settings: function() {
        $('.menu.__sub .form input[type=checkbox]').prop('checked', false);
        $('#bladeContainerStaticNormal').prop('checked', true);

        var url = window.location.href.replace('blade-constructor.html', '');
        
        $('#tplblade').load(url + '/templates/settings/blade.html', function () {
            Blade.saveCode();
        });

        var types,
            disabled,
            disableTypes,
            tblBladeError,
            tplBladeProgress,
            tplBladeProgressSolo,
            tblBladeNav,
            tplBladeIcon,
            tplBladeTitle,
            tplBladeToolbar,
            tplBladeContent,
            tplBladeStaticTop;

        types = [
            'bladeHeadError',
            'bladeHeadProgress',
            'bladeHeadNav',
            'bladeHeadIcon',
            'bladeHeadTitle',
            'bladeHeadDescr',
            'bladeHeadToolbar',
            'bladeContainerStaticTop',
            'bladeContainerStaticNormal',
            'bladeContainerStaticCollapsed',
            'bladeContainerStaticExpanded',
            'bladeContainerBreadcrumbs',
            'bladeContainerSearch',
            'bladeContainerSearchCount',
            'bladeContainerStaticBot',
            'bladeContainerStaticButton',
        ];

        disableTypes = [
            'bladeHeadDescr',
            'bladeHeadTitle',
            'bladeContainerStaticNormal',
            'bladeContainerStaticCollapsed',
            'bladeContainerStaticExpanded',
            'bladeContainerBreadcrumbs',
            'bladeContainerSearch',
            'bladeContainerSearchCount',
            'bladeContainerStaticBot',
            'bladeContainerStaticButton'
        ];

        for(var i = 0; i < disableTypes.length; i++) {
            $('#' + disableTypes[i]).prop('disabled', true);
            $('#' + disableTypes[i]).parent().addClass('__disabled');
        }

        $('#result').load(url + '/templates/settings/blade-error.html', function () {
            tplBladeError = $('#result').html();
        });

        $('#result').load(url + '/templates/settings/blade-progress.html', function () {
            tplBladeProgress = $('#result').html();
        });

        $('#result').load(url + '/templates/settings/blade-nav.html', function () {
            tblBladeNav = $('#result').html();
        });

        $('#result').load(url + '/templates/settings/blade-icon.html', function () {
            tplBladeIcon = $('#result').html();
        });

        $('#result').load(url + '/templates/settings/blade-title.html', function () {
            tplBladeTitle = $('#result').html();
        });

        $('#result').load(url + '/templates/settings/blade-descr.html', function () {
            tplBladeDescr = $('#result').html();
        });

        $('#result').load(url + '/templates/settings/blade-toolbar.html', function () {
            tplBladeToolbar = $('#result').html();
        });

        $('#result').load(url + '/templates/settings/blade-content.html', function () {
            tplBladeContent = $('#result').html();
        });

        $('#result').load(url + '/templates/settings/blade-static-top.html', function () {
            tplBladeStaticTop = $('#result').html();
        });

        $('#result').load(url + '/templates/settings/blade-breadcrumbs.html', function () {
            tplBladeBreadcrumbs = $('#result').html();
        });

        $('#result').load(url + '/templates/settings/blade-search.html', function () {
            tplBladeSearch = $('#result').html();
        });

        $('#result').load(url + '/templates/settings/blade-search-solo.html', function () {
            tplBladeSearchSolo = $('#result').html();
        });

        $('#result').load(url + '/templates/settings/blade-search-count.html', function () {
            tplBladeSearchCount = $('#result').html();
        });

        $('#result').load(url + '/templates/settings/blade-static-bottom.html', function () {
            tplBladeStaticBot = $('#result').html();
        });

        $('#result').load(url + '/templates/settings/blade-button.html', function () {
            tplBladeStaticButton = $('#result').html();
        });

        for(var i = 0; i < types.length; i++) {
            $('#' + types[i]).on('click', function () {
                var self = $(this),
                    type = self.prop('id'),
                    checked = self.is(':checked');

                /* Blade head error */
                if(type == 'bladeHeadError') {
                    if($('#bladeHeadError').is(':checked')) {
                        $('#tplblade .blade-status').append(tplBladeError);
                        $('#bladeHeadProgress').prop('disabled', true);
                        $('#bladeHeadProgress').parent().addClass('__disabled');

                        Blade.saveCode();
                    }
                    else {
                        $('#tplblade .blade-status').empty();
                        $('#bladeHeadProgress').prop('disabled', false);
                        $('#bladeHeadProgress').parent().removeClass('__disabled');

                        Blade.saveCode();
                    }
                }

                /* Blade head progress */
                if(type == 'bladeHeadProgress') {
                    if($('#bladeHeadProgress').is(':checked')) {
                        $('#tplblade .blade-status').append(tplBladeProgress);
                        $('#bladeHeadError').prop('disabled', true);
                        $('#bladeHeadError').parent().addClass('__disabled');

                        Blade.saveCode();
                    }
                    else {
                        $('#tplblade .blade-status').empty();
                        $('#bladeHeadError').prop('disabled', false);
                        $('#bladeHeadError').parent().removeClass('__disabled');

                        Blade.saveCode();
                    }
                }

                /* Blade head nav */
                if(type == 'bladeHeadNav') {
                    if($('#bladeHeadNav').is(':checked')) {
                        $('#tplblade .blade-nav').append(tblBladeNav);
                        Blade.saveCode();
                    }
                    else {
                        $('#tplblade .blade-nav').empty();
                        Blade.saveCode();
                    }
                }

                /* Blade head icon */
                if(type == 'bladeHeadIcon') {
                    if($('#bladeHeadIcon').is(':checked')) {
                        $('#tplblade .blade-t').prepend(tplBladeIcon);
                        $('#bladeHeadTitle').prop('disabled', false);
                        $('#bladeHeadTitle').parent().removeClass('__disabled');

                        Blade.saveCode();
                    }
                    else {
                        $('#tplblade .blade-t').empty();
                        $('#bladeHeadTitle').prop('disabled', true);
                        $('#bladeHeadTitle').parent().addClass('__disabled');
                        
                        Blade.saveCode();
                    }
                }

                /* Blade head title */
                if(type == 'bladeHeadTitle') {
                    if($('#bladeHeadTitle').is(':checked')) {
                        $('#tplblade .blade-t_ico').after(tplBladeTitle);

                        $('#bladeHeadIcon').prop('disabled', true);
                        $('#bladeHeadIcon').parent().addClass('__disabled');

                        $('#bladeHeadDescr').prop('disabled', false);
                        $('#bladeHeadDescr').parent().removeClass('__disabled');

                        Blade.saveCode();
                    }
                    else {
                        $('#tplblade .blade-t').empty();
                        $('#tplblade .blade-t').prepend(tplBladeIcon);
                        
                        $('#bladeHeadIcon').prop('disabled', false);
                        $('#bladeHeadIcon').parent().removeClass('__disabled');

                        $('#bladeHeadDescr').prop('disabled', true);
                        $('#bladeHeadDescr').parent().addClass('__disabled');
                        
                        Blade.saveCode();
                    }
                }

                /* Blade head descr */
                if(type == 'bladeHeadDescr') {
                    if($('#bladeHeadDescr').is(':checked')) {
                        $('#tplblade .blade-t_head').after(tplBladeDescr);

                        $('#bladeHeadTitle').prop('disabled', true);
                        $('#bladeHeadTitle').parent().addClass('__disabled');

                        $('#tplblade .blade-t').addClass('__secondary');

                        Blade.saveCode();
                    }
                    else {
                        $('#tplblade .blade-t').empty();
                        $('#tplblade .blade-t').prepend(tplBladeIcon);
                        $('#tplblade .blade-t_ico').after(tplBladeTitle);
                        
                        $('#bladeHeadTitle').prop('disabled', false);
                        $('#bladeHeadTitle').parent().removeClass('__disabled');

                        $('#tplblade .blade-t').removeClass('__secondary');
                        
                        Blade.saveCode();
                    }
                }

                /* Blade head toolbar */
                if(type == 'bladeHeadToolbar') {
                    if($('#bladeHeadToolbar').is(':checked')) {
                        $('#tplblade .blade-toolbar').append(tplBladeToolbar);

                        Blade.saveCode();
                    }
                    else {
                        $('#tplblade .blade-toolbar').empty();

                        Blade.saveCode();
                    }
                }

                /* Blade container static top */
                if(type == 'bladeContainerStaticTop') {
                    if($('#bladeContainerStaticTop').is(':checked')) {
                        $('#tplblade .blade-container').prepend(tplBladeStaticTop);
                        $('#bladeContainerStaticBot, #bladeContainerStaticNormal, #bladeContainerStaticCollapsed, #bladeContainerStaticExpanded').prop('disabled', false);
                        $('#bladeContainerStaticBot, #bladeContainerStaticNormal, #bladeContainerStaticCollapsed, #bladeContainerStaticExpanded').parent().removeClass('__disabled');
                        $('#bladeContainerSearch').prop('disabled', false);
                        $('#bladeContainerSearch').parent().removeClass('__disabled');

                        Blade.saveCode();
                    }
                    else {
                        $('#tplblade .blade-container').empty();
                        $('#tplblade .blade-container').append(tplBladeContent);
                        $('#bladeContainerStaticBot, #bladeContainerStaticNormal, #bladeContainerStaticCollapsed, #bladeContainerStaticExpanded').prop('disabled', true);
                        $('#bladeContainerStaticBot, #bladeContainerStaticNormal, #bladeContainerStaticCollapsed, #bladeContainerStaticExpanded').parent().addClass('__disabled');
                        $('#bladeContainerStaticNormal').prop('checked', true);
                        $('#bladeContainerSearch').prop('disabled', true);
                        $('#bladeContainerSearch').parent().addClass('__disabled');

                        Blade.saveCode();
                    }
                }

                if(type == 'bladeContainerStaticNormal') {
                    if($('#bladeContainerStaticNormal').is(':checked')) {
                        $('#tplblade .blade-static:not(.__bottom):not(.__bottom)').removeClass('__collapsed');
                        $('#tplblade .blade-static:not(.__bottom):not(.__bottom)').removeClass('__expanded');
                        $('#bladeContainerBreadcrumbs, #bladeContainerSearch, #bladeContainerSearchCount').prop('disabled', true);
                        $('#bladeContainerBreadcrumbs, #bladeContainerSearch, #bladeContainerSearchCount').parent().addClass('__disabled');
                        $('#tplblade .blade-static:not(.__bottom)').empty();
                        $('#bladeContainerBreadcrumbs').prop('checked', false);
                        $('#bladeContainerSearch').prop('disabled', false);
                        $('#bladeContainerSearch').parent().removeClass('__disabled');

                        if($('#bladeContainerSearch').is(':checked')) {
                            $('#tplblade .blade-static:not(.__bottom):not(.__bottom)').empty();
                            $('#tplblade .blade-static:not(.__bottom):not(.__bottom)').prepend(tplBladeSearchSolo);
                            $('#bladeContainerSearchCount').prop('disabled', false);
                            $('#bladeContainerSearchCount').parent().removeClass('__disabled');

                            Blade.saveCode();
                        }
                        else {
                            $('#tplblade .blade-static:not(.__bottom):not(.__bottom)').empty();

                            Blade.saveCode();
                        }
                    }
                }

                if(type == 'bladeContainerStaticCollapsed') {
                    if($('#bladeContainerStaticCollapsed').is(':checked')) {
                        $('#tplblade .blade-static:not(.__bottom):not(.__bottom)').addClass('__collapsed');
                        $('#tplblade .blade-static:not(.__bottom):not(.__bottom)').removeClass('__expanded');
                        $('#bladeContainerBreadcrumbs').prop('disabled', false);
                        $('#bladeContainerBreadcrumbs').parent().removeClass('__disabled');
                        $('#bladeContainerSearch, #bladeContainerSearchCount').prop('disabled', true);
                        $('#bladeContainerSearch, #bladeContainerSearchCount').parent().addClass('__disabled');
                        $('#bladeContainerSearch').prop('disabled', true);
                        $('#bladeContainerSearch').parent().addClass('__disabled');
                        $('#bladeContainerSearch').prop('checked', false);

                        if($('#tplblade .breadcrumbs').length) {
                            $('#tplblade .blade-static:not(.__bottom)').empty();
                            $('#tplblade .blade-static:not(.__bottom)').prepend(tplBladeBreadcrumbs);

                            Blade.saveCode();
                        }
                        else {
                            $('#tplblade .blade-static:not(.__bottom)').empty();

                            Blade.saveCode();
                        }
                    }
                }

                if(type == 'bladeContainerStaticExpanded') {
                    if($('#bladeContainerStaticExpanded').is(':checked')) {
                        $('#tplblade .blade-static:not(.__bottom)').addClass('__expanded');
                        $('#tplblade .blade-static:not(.__bottom)').removeClass('__collapsed');
                        $('#bladeContainerBreadcrumbs').prop('disabled', false);
                        $('#bladeContainerBreadcrumbs').parent().removeClass('__disabled');
                        $('#bladeContainerSearch').prop('disabled', false);
                        $('#bladeContainerSearch').parent().removeClass('__disabled');
                        $('#bladeContainerSearch').prop('disabled', false);
                        $('#bladeContainerSearch').parent().removeClass('__disabled');

                        Blade.saveCode();
                    }
                }

                if(type == 'bladeContainerBreadcrumbs') {
                    if($('#bladeContainerBreadcrumbs').is(':checked')) {
                        if($('#tplblade .form-group').length) {
                            $('#tplblade .blade-static:not(.__bottom)').empty();
                            $('#tplblade .blade-static:not(.__bottom)').prepend(tplBladeBreadcrumbs);
                            $('#tplblade .breadcrumbs').after(tplBladeSearch);
                            $('#tplblade .blade-static:not(.__bottom)').removeClass('__collapsed');
                            $('#tplblade .blade-static:not(.__bottom)').addClass('__expanded');
                            $('#bladeContainerStaticExpanded').prop('checked', true);
                        }
                        else {
                            $('#tplblade .blade-static:not(.__bottom)').empty();
                            $('#tplblade .blade-static:not(.__bottom)').prepend(tplBladeBreadcrumbs);
                        }

                        Blade.saveCode();
                    }
                    else {
                        if($('#tplblade .form-group').length) {
                            $('#tplblade .blade-static:not(.__bottom)').empty();
                            $('#tplblade .blade-static:not(.__bottom)').append(tplBladeSearchSolo);
                            $('#tplblade .blade-static:not(.__bottom)').removeClass('__collapsed');
                            $('#tplblade .blade-static:not(.__bottom)').removeClass('__expanded');
                            $('#bladeContainerStaticNormal').prop('checked', true);
                            $('#bladeContainerStaticExpanded').prop('disabled', false);
                            $('#bladeContainerStaticExpanded').parent().removeClass('__disabled');
                        }
                        else {
                            $('#tplblade .blade-static:not(.__bottom)').empty();
                            $('#bladeContainerStaticExpanded').prop('disabled', false);
                            $('#bladeContainerStaticExpanded').parent().removeClass('__disabled');
                        }

                        Blade.saveCode();
                    }
                }

                if(type == 'bladeContainerSearch') {
                    if($('#bladeContainerSearch').is(':checked')) {
                        if($('#tplblade .breadcrumbs').length) {
                            $('#tplblade .breadcrumbs').after(tplBladeSearch);
                            $('#tplblade .blade-static:not(.__bottom)').addClass('__expanded');
                            $('#tplblade .blade-static:not(.__bottom)').removeClass('__collapsed');
                            $('#bladeContainerStaticExpanded').prop('disabled', false);
                            $('#bladeContainerStaticExpanded').parent().removeClass('__disabled');
                            $('#bladeContainerStaticExpanded').prop('checked', true);
                        }
                        else {
                            $('#tplblade .blade-static:not(.__bottom)').empty();
                            $('#tplblade .blade-static:not(.__bottom)').append(tplBladeSearchSolo);
                            $('#tplblade .blade-static:not(.__bottom)').removeClass('__collapsed');
                            $('#tplblade .blade-static:not(.__bottom)').removeClass('__expanded');
                        }

                        $('#bladeContainerSearchCount').prop('disabled', false);
                        $('#bladeContainerSearchCount').parent().removeClass('__disabled');
                        $('#tplblade .form-input.__search').removeClass('__search');

                        Blade.saveCode();
                    }
                    else {
                        if($('#tplblade .breadcrumbs').length) {
                            $('#tplblade .blade-static:not(.__bottom)').empty();
                            $('#tplblade .blade-static:not(.__bottom)').prepend(tplBladeBreadcrumbs);
                            $('#tplblade .blade-static:not(.__bottom)').removeClass('__expanded');
                            $('#tplblade .blade-static:not(.__bottom)').addClass('__collapsed');
                            $('#bladeContainerStaticExpanded').prop('disabled', true);
                            $('#bladeContainerStaticExpanded').parent().addClass('__disabled');
                            $('#bladeContainerStaticCollapsed').prop('checked', true);
                        }
                        else {
                            $('#tplblade .blade-static:not(.__bottom)').empty();
                        }

                        $('#bladeContainerSearchCount').prop('disabled', true);
                        $('#bladeContainerSearchCount').parent().addClass('__disabled');

                        Blade.saveCode();
                    }
                }

                if(type == 'bladeContainerSearchCount') {
                    if($('#bladeContainerSearchCount').is(':checked')) {
                        $('#bladeContainerSearch').prop('disabled', true);
                        $('#bladeContainerSearch').parent().addClass('__disabled');

                        if($('#tplblade .breadcrumbs').length) {
                            $('#tplblade .blade-static:not(.__bottom)').empty();
                            $('#tplblade .blade-static:not(.__bottom)').prepend(tplBladeBreadcrumbs);
                            $('#tplblade .breadcrumbs').after(tplBladeSearch);
                            $('#tplblade .blade-static:not(.__bottom) .form-input').after(tplBladeSearchCount);
                            $('#tplblade .blade-static:not(.__bottom) .form-input').addClass('__search');

                            Blade.saveCode();
                        }
                        else {
                            $('#tplblade .blade-static:not(.__bottom)').empty();
                            $('#tplblade .blade-static:not(.__bottom)').prepend(tplBladeSearchSolo);
                            $('#tplblade .blade-static:not(.__bottom) .form-input').after(tplBladeSearchCount);
                            $('#tplblade .blade-static:not(.__bottom) .form-input').addClass('__search');

                            Blade.saveCode();
                        }

                        Blade.saveCode();
                    }
                    else {
                        $('#bladeContainerSearch').prop('disabled', false);
                        $('#bladeContainerSearch').parent().removeClass('__disabled');

                        if($('#tplblade .breadcrumbs').length) {
                            $('#tplblade .blade-static:not(.__bottom)').empty();
                            $('#tplblade .blade-static:not(.__bottom)').prepend(tplBladeBreadcrumbs);
                            $('#tplblade .breadcrumbs').after(tplBladeSearch);
                            $('#tplblade .blade-static:not(.__bottom) .form-input').removeClass('__search');

                            Blade.saveCode();
                        }
                        else {
                            $('#tplblade .blade-static:not(.__bottom)').empty();
                            $('#tplblade .blade-static:not(.__bottom)').prepend(tplBladeSearchSolo);
                            $('#tplblade .blade-static:not(.__bottom) .form-input').removeClass('__search');

                            Blade.saveCode();
                        }
                    }
                }

                /* Blade container static bottom */
                if(type == 'bladeContainerStaticBot') {
                    if($('#bladeContainerStaticBot').is(':checked')) {
                        if($('#tplblade .blade-static:not(.__bottom)').length) {
                            $('#tplblade .blade-container').empty();
                            $('#tplblade .blade-container').prepend(tplBladeStaticTop);
                            $('#tplblade .blade-static').after(tplBladeStaticBot);
                            $('#tplblade .blade-container').append(tplBladeContent);

                            Blade.saveCode();
                        }
                        else {
                            $('#tplblade .blade-container').empty();
                            $('#tplblade .blade-container').prepend(tplBladeStaticBot);
                            $('#tplblade .blade-container').append(tplBladeContent);

                            Blade.saveCode();
                        }

                        $('#bladeContainerStaticButton').prop('disabled', false);
                        $('#bladeContainerStaticButton').parent().removeClass('__disabled');
                    }
                    else {
                        if($('#tplblade .blade-static:not(.__bottom)').length) {
                            $('#tplblade .blade-container').empty();
                            $('#tplblade .blade-container').prepend(tplBladeStaticTop);
                            $('#tplblade .blade-container').append(tplBladeContent);

                            Blade.saveCode();
                        }
                        else {
                            $('#tplblade .blade-container').empty();
                            $('#tplblade .blade-container').append(tplBladeContent);

                            Blade.saveCode();
                        }

                        $('#bladeContainerStaticButton').prop('disabled', true);
                        $('#bladeContainerStaticButton').parent().addClass('__disabled');
                    }
                }

                if(type == 'bladeContainerStaticButton') {
                    if($('#bladeContainerStaticButton').is(':checked')) {
                        $('#tplblade .blade-static.__bottom').append(tplBladeStaticButton);

                        Blade.saveCode();
                    }
                    else {
                        $('#tplblade .blade-static.__bottom').empty();

                        Blade.saveCode();
                    }
                }
            });
        }

        // $('.settings .form-control input').on('click', function () {
        //     var type = $(this);
            
        //     if(type.prop('id') == 'headNav') {
        //         if (type.prop('checked')) {
        //             // $('#tplblade .blade-nav').load(url + '/templates/settings/blade-nav.html');
        //             $('#tplblade .blade-nav').append(123);
        //             //$('.example.__construct .blade-nav').append(bladeNav);

        //             Blade.saveCode();
        //         }
        //         else {
        //             $('#tplblade .blade-nav .menu').remove();

        //             Blade.saveCode();
        //         }
        //     }

        //     if(type.prop('id') == 'headIcon') {
        //         if (type.prop('checked')) {
        //             $('.example.__construct .blade-t').append("<i class='blade-t_ico fa fa-folder'></i>");

        //             Blade.saveCode();
        //         }
        //         else {
        //             $('.example.__construct .blade-t .blade-t_ico').remove();

        //             Blade.saveCode();
        //         }
        //     }

        //     if(type.prop('id') == 'headTitle') {
        //         if (type.prop('checked')) {
        //             $('.example.__construct .blade-t').append("<div class='blade-t_head'>Title</div>");
        //             $('#headDescr').prop('disabled', false);
        //             $('#headDescr').parent().removeClass('__disabled');

        //             Blade.saveCode();
        //         }
        //         else {
        //             $('.example.__construct .blade-t .blade-t_head').remove();
        //             $('#headDescr').prop('disabled', true);
        //             $('#headDescr').parent().addClass('__disabled');

        //             Blade.saveCode();
        //         }
        //     }

        //     if(type.prop('id') == 'headDescr') {
        //         if (type.prop('checked')) {
        //             $('.example.__construct .blade-t').append("<div class='blade-t_subhead'>Description</div>");
        //             $('.example.__construct .blade-t').addClass('__secondary');
        //             $('#headTitle').prop('disabled', true);
        //             $('#headTitle').parent().addClass('__disabled');

        //             Blade.saveCode();
        //         }
        //         else {
        //             $('.example.__construct .blade-t .blade-t_subhead').remove();
        //             $('.example.__construct .blade-t').removeClass('__secondary');
        //             $('#headTitle').prop('disabled', false);
        //             $('#headTitle').parent().removeClass('__disabled');

        //             Blade.saveCode();
        //         }
        //     }

        //     if(type.prop('id') == 'headToolbar') {
        //         if (type.prop('checked')) {
        //             $('.example.__construct .blade-t').after(
        //                 "<div class='blade-toolbar'>" +
        //                     "<ul class='menu __inline'>" +
        //                         "<li class='menu-item'>" +
        //                             "<a class='menu-btn'>" +
        //                                 "<i class='menu-ico fa fa-plus'></i> Add" +
        //                             "</a>" +
        //                         "</li>" +
        //                     "</ul>" +
        //                 "</div>");

        //             Blade.saveCode();
        //         }
        //         else {
        //             $('.example.__construct .blade-toolbar').remove();

        //             Blade.saveCode();
        //         }
        //     }

        //     if(type.prop('id') == 'staticTop') {
        //         if (type.prop('checked')) {
        //             $('.example.__construct .blade-container').prepend("<div class='blade-static'></div>");
        //             $('#collapsed, #expanded, #normal').prop('disabled', false);
        //             $('#collapsed, #expanded, #normal').parent().removeClass('__disabled');
                                        
        //             Blade.saveCode();
        //         }
        //         else {
        //             $('.example.__construct .blade-static:not(.__bottom)').remove();
        //             $('#collapsed, #expanded, #normal').prop('disabled', true);
        //             $('#collapsed, #expanded, #normal').parent().addClass('__disabled');

        //             Blade.saveCode();
        //         }
        //     }

        //     if(type.prop('id') == 'normal') {
        //         if (type.prop('checked')) {
        //             $('.example.__construct .blade-static:not(.__bottom)').removeClass('__collapsed');
        //             $('.example.__construct .blade-static:not(.__bottom)').removeClass('__expanded');
                                        
        //             Blade.saveCode();
        //         }
        //     }

        //     if(type.prop('id') == 'collapsed') {
        //         if (type.prop('checked')) {
        //             $('.example.__construct .blade-static:not(.__bottom)').addClass('__collapsed');
        //             $('.example.__construct .blade-static:not(.__bottom)').removeClass('__expanded');
                                        
        //             Blade.saveCode();
        //         }
        //     }

        //     if(type.prop('id') == 'expanded') {
        //         if (type.prop('checked')) {
        //             $('.example.__construct .blade-static:not(.__bottom)').addClass('__expanded');
        //             $('.example.__construct .blade-static:not(.__bottom)').removeClass('__collapsed');
                                        
        //             Blade.saveCode();
        //         }
        //     }

        //     if(type.prop('id') == 'wnormal') {
        //         if (type.prop('checked')) {
        //             $('.example.__construct .blade-content').removeClass('__medium-wide');
        //             $('.example.__construct .blade-content').removeClass('__large-wide');
        //             $('.example.__construct .blade-content').removeClass('__xlarge-wide');
                                        
        //             Blade.saveCode();
        //         }
        //     }

        //     if(type.prop('id') == 'wmedium') {
        //         if (type.prop('checked')) {
        //             $('.example.__construct .blade-content').addClass('__medium-wide');
        //             $('.example.__construct .blade-content').removeClass('__large-wide');
        //             $('.example.__construct .blade-content').removeClass('__xlarge-wide');
                                        
        //             Blade.saveCode();
        //         }
        //     }

        //     if(type.prop('id') == 'wlarge') {
        //         if (type.prop('checked')) {
        //             $('.example.__construct .blade-content').addClass('__large-wide');
        //             $('.example.__construct .blade-content').removeClass('__medium-wide');
        //             $('.example.__construct .blade-content').removeClass('__xlarge-wide');
                                        
        //             Blade.saveCode();
        //         }
        //     }

        //     if(type.prop('id') == 'wxlarge') {
        //         if (type.prop('checked')) {
        //             $('.example.__construct .blade-content').addClass('__xlarge-wide');
        //             $('.example.__construct .blade-content').removeClass('__medium-wide');
        //             $('.example.__construct .blade-content').removeClass('__large-wide');
                                        
        //             Blade.saveCode();
        //         }
        //     }
        // });
    },

    saveCode: function() {
        var html = $('#tplblade').html();
        var lines = html.split('\n');
        
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
    Blade.settings();
});