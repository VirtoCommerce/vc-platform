angular.module('virtoCommerce.contentModule').directive('uiKitHtmlEditor', ['platformWebApp.dialogService', function (dialogService) {
    return {
        restrict: 'A',
        require: 'ngModel',
        scope: {
            ngModel: '=',
            fileUploader: '='
        },
        link: function (scope, element, attributes) {
            var htmlEditor = UIkit.htmleditor(element, { mode: 'split', maxsplitsize: 900, markdown: true });
            htmlEditor.addButtons({
                headerFirst: { label: 'H1', title: 'First level header' },
                headerSecond: { label: 'H2', title: 'Second level header' }
            });
            htmlEditor.options.toolbar.unshift('headerFirst', 'headerSecond');
            htmlEditor._buildtoolbar();
            addAction(htmlEditor, 'headerFirst', '# $1');
            addAction(htmlEditor, 'headerSecond', '## $1');
            htmlEditor.off('action.image').on('action.image', function () {
                dialogService.showUploadDialog({ uploader: scope.fileUploader });
            });
            var codeMirror = htmlEditor.editor;
            var codeMirrorElement = $('.CodeMirror');
            var currentEditorLine;
            codeMirrorElement.on('dragenter', function (event) {
                event.preventDefault();
                event.stopPropagation();
                currentEditorLine = getCurrentEditorLine(codeMirror, event);
                highlightCurrentEditorLine(codeMirror, currentEditorLine);
            });
            codeMirrorElement.on('dragover', function (event) {
                event.preventDefault();
                event.stopPropagation();
            });
            codeMirrorElement.on('drop', function (event) {
                event.preventDefault();
                event.stopPropagation();
                scope.fileUploader.addToQueue(event.originalEvent.dataTransfer.files);
            });
            codeMirrorElement.on('paste', function (event) {
                var blob = event.originalEvent.clipboardData.items[0].getAsFile();
                if (blob) {
                    var filename = (new Date().getTime()).toString() + '.png';
                    var file = new File([blob], filename, { type: blob.type });
                    scope.fileUploader.addToQueue([ file ]);
                }
            });

            scope.$on('filesUploaded', function (event, arg) {
                var currentEditorLine = currentEditorLine || codeMirror.getCursor().line;
                var position = { line: currentEditorLine, ch: 0 };
                if (codeMirror.getLineTokens(currentEditorLine)) {
                    position.line++;
                }
                for (var i = 0; i < arg.items.length; i++) {
                    var file = arg.items[i];
                    position.line++;
                    codeMirror.replaceRange('![' + file.name + '](' + file.url + ')\n\n', position);
                }
                resetEditorLinesStyle(codeMirror);
            });

            scope.$on('resetContent', function (event, arg) {
                codeMirror.setValue(arg.body || '');
            });

            function getCurrentEditorLine(editor, event) {
                return editor.lineAtHeight(event.originalEvent.pageY);
            }

            function highlightCurrentEditorLine(editor, line) {
                resetEditorLinesStyle(editor);
                editor.addLineClass(line, null, 'CodeMirror-activeline-background');
            }

            function resetEditorLinesStyle(editor) {
                for (var i = 0; i < editor.lineCount() ; i++) {
                    editor.removeLineClass(i, null, 'CodeMirror-activeline-background');
                }
            }

            function addAction(htmlEditor, name, replace, mode) {
                htmlEditor.on('action.' + name, function () {
                    if (htmlEditor.getCursorMode() == 'html' || htmlEditor.getCursorMode() == 'markdown') {
                        htmlEditor[mode == 'replaceLine' || 'replaceSelection'](replace);
                    }
                });
            }
        }
    }
}]);


//// ===============================================================================================================



//            //var addActions = function (editor, isHtml) {
//            //    function addAction(name, replace, mode) {
//            //        editor.on('action.' + name, function () {
//            //            if (editor.getCursorMode() == 'html' || editor.getCursorMode() == 'markdown') {
//            //                editor[mode == 'replaceLine' ? 'replaceLine' : 'replaceSelection'](replace);
//            //            }
//            //        });
//            //    }

//            //    if (isHtml) {
//            //        addAction('headerFirst', '<h1>$1</h1>');
//            //        addAction('headerSecond', '<h2>$1</h2>');
//            //        addAction('headerThird', '<h3>$1</h3>');
//            //        addAction('headerFourth', '<h4>$1</h4>');
//            //        addAction('headerFifth', '<h5>$1</h5>');
//            //        addAction('image', '<img src="$1" />');
//            //    }
//            //    else {
//            //        addAction('headerFirst', '# $1');
//            //        addAction('headerSecond', '## $1');
//            //        addAction('headerThird', '### $1');
//            //        addAction('headerFourth', '#### $1');
//            //        addAction('headerFifth', '##### $1');
//            //        addAction('image', '');
//            //    }
//            //};

//            //$(element).html(scope.ngModel);

//            //UIkit.htmleditor($('textarea[ui-kit-html-editor]'), { mode: 'split', maxsplitsize: 900, toolbar: ['bold', 'italic', 'strike', 'link', 'blockquote', 'listUl', 'listOl'], markdown: true });

//            //var editor = $('.CodeMirror')[0].CodeMirror;
//            //var htmlEditor = editor.htmleditor;

//            //editor.on('change', function () {
//            //    ngModel = editor.getValue();
//            //});

//            //editor.setOption('mode', 'gfm');

//            //htmlEditor.addButtons({
//            //    headerFirst: {
//            //        title: 'First level header',
//            //        label: 'H1'
//            //    },
//            //    headerSecond: {
//            //        title: 'Second level header',
//            //        label: 'H2'
//            //    },
//            //    headerThird: {
//            //        title: 'Third level header',
//            //        label: 'H3'
//            //    },
//            //    headerFourth: {
//            //        title: 'Fourth level header',
//            //        label: 'H4'
//            //    },
//            //    headerFifth: {
//            //        title: 'Fifth level header',
//            //        label: 'H5'
//            //    }
//            //});

//            //htmlEditor.options.toolbar.unshift('headerFirst', 'headerSecond', 'headerThird', 'headerFourth', 'headerFifth');
//            //htmlEditor._buildtoolbar();

//            //addActions(htmlEditor, false);


//            //scope.$on('resetContent', function (event, arg) {
//            //    var editor = $('.CodeMirror')[0].CodeMirror;
//            //    editor.setValue(arg.body || '');
//            //});

//            //scope.$on('changeEditType', function (event, arg) {
//            //    var editor = $('.CodeMirror')[0].CodeMirror;
//            //    var htmlEditor = editor.htmleditor;
//            //    if (arg.editAsHtml) {
//            //        editor.setOption('mode', 'htmlmixed');
//            //        addActions(htmlEditor, true);
//            //    }
//            //    else if (arg.editAsMarkdown) {
//            //        editor.setOption('mode', 'gfm');
//            //        addActions(htmlEditor, false);
//            //    }
//            //});
//        },
//    }
//}]);