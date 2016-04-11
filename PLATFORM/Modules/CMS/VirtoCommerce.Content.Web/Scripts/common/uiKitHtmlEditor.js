angular.module('virtoCommerce.contentModule').directive('uiKitHtmlEditor', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        scope: {
            ngModel: '='
        },
        link: function (scope, element, attrs, ngModel) {
            var addActions = function (editor, isHtml) {
                function addAction(name, replace, mode) {
                    editor.on('action.' + name, function () {
                        if (editor.getCursorMode() == 'html' || editor.getCursorMode() == 'markdown') {
                            editor[mode == 'replaceLine' ? 'replaceLine' : 'replaceSelection'](replace);
                        }
                    });
                }

                if (isHtml) {
                    addAction('headerFirst', '<h1>$1</h1>');
                    addAction('headerSecond', '<h2>$1</h2>');
                    addAction('headerThird', '<h3>$1</h3>');
                    addAction('headerFourth', '<h4>$1</h4>');
                    addAction('headerFifth', '<h5>$1</h5>');
                }
                else {
                    addAction('headerFirst', '# $1');
                    addAction('headerSecond', '## $1');
                    addAction('headerThird', '### $1');
                    addAction('headerFourth', '#### $1');
                    addAction('headerFifth', '##### $1');
                }
            };

            $(element).html(scope.ngModel);

            UIkit.htmleditor($('textarea[ui-kit-html-editor]'), { mode: 'split', maxsplitsize: 900, toolbar: ['bold', 'italic', 'strike', 'link', 'image', 'blockquote', 'listUl', 'listOl'], markdown: true });

            var editor = $('.CodeMirror')[0].CodeMirror;
            var htmlEditor = editor.htmleditor;

            editor.on('change', function () {
                ngModel = editor.getValue();
            });

            editor.setOption('mode', 'gfm');

            htmlEditor.addButtons({
                headerFirst: {
                    title: 'First level header',
                    label: 'H1'
                },
                headerSecond: {
                    title: 'Second level header',
                    label: 'H2'
                },
                headerThird: {
                    title: 'Third level header',
                    label: 'H3'
                },
                headerFourth: {
                    title: 'Fourth level header',
                    label: 'H4'
                },
                headerFifth: {
                    title: 'Fifth level header',
                    label: 'H5'
                }
            });

            htmlEditor.options.toolbar.unshift('headerFirst', 'headerSecond', 'headerThird', 'headerFourth', 'headerFifth');
            htmlEditor._buildtoolbar();

            addActions(htmlEditor, false);


            scope.$on('resetContent', function (event, arg) {
                var editor = $('.CodeMirror')[0].CodeMirror;
                editor.setValue(arg.body || '');
            });

            scope.$on('changeEditType', function (event, arg) {
                var editor = $('.CodeMirror')[0].CodeMirror;
                var htmlEditor = editor.htmleditor;
                if (arg.editAsHtml) {
                    editor.setOption('mode', 'htmlmixed');
                    addActions(htmlEditor, true);
                }
                else if (arg.editAsMarkdown) {
                    editor.setOption('mode', 'gfm');
                    addActions(htmlEditor, false);
                }
            });
        },
    }
});