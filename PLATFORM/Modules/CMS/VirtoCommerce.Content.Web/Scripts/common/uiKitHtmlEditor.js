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
            var codeMirror = htmlEditor.editor;
            htmlEditor.addButtons({
                headerFirst: { label: 'H1', title: 'First level header' },
                headerSecond: { label: 'H2', title: 'Second level header' }
            });
            htmlEditor.options.toolbar.unshift('headerFirst', 'headerSecond');
            htmlEditor._buildtoolbar();
            htmlEditor.off('action.image').on('action.image', function () {
                $('#fileUploader').trigger('click');
            });
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
                var imageClipboardItem = _.find(event.originalEvent.clipboardData.items, function (i) { return i.type.indexOf('image') !== -1 && i.kind === 'file' });
                if (imageClipboardItem) {
                    var blob = imageClipboardItem.getAsFile();
                    var filename = buildFileName() + '.png';
                    var file = new File([blob], filename, { type: blob.type });
                    scope.fileUploader.addToQueue([file]);
                }
            });

            addActions(htmlEditor, codeMirror.getMode().name);

            scope.$on('filesUploaded', function (event, arg) {
                var currentEditorLine = currentEditorLine || codeMirror.getCursor().line;
                var position = { line: currentEditorLine, ch: 0 };
                if (codeMirror.getLineTokens(currentEditorLine)) {
                    position.line++;
                }
                for (var i = 0; i < arg.items.length; i++) {
                    var file = arg.items[i];
                    position.line++;
                    var editorMode = codeMirror.getMode();
                    if (editorMode.name === 'gfm') {
                        codeMirror.replaceRange('![' + file.name + '](' + file.url + ')\n\n', position);
                    } else if (editorMode.name === 'htmlmixed') {
                        codeMirror.replaceRange('<img alt="' + file.name + '" src="' + file.url + '" />\n\n', position);
                    }
                }
                resetEditorLinesStyle(codeMirror);
            });

            scope.$on('resetContent', function (event, arg) {
                codeMirror.setValue(arg.body || '');
            });

            scope.$on('changeEditType', function (event, arg) {
                if (arg.editAsHtml) {
                    codeMirror.setOption('mode', 'htmlmixed');
                }
                else if (arg.editAsMarkdown) {
                    codeMirror.setOption('mode', 'gfm');
                }
                addActions(htmlEditor, codeMirror.getMode().name);
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
                htmlEditor.off('action.' + name).on('action.' + name, function () {
                    if (htmlEditor.getCursorMode() == 'html' || htmlEditor.getCursorMode() == 'markdown') {
                        htmlEditor[mode == 'replaceLine' || 'replaceSelection'](replace);
                    }
                });
            }

            function addActions(htmlEditor, editorMode) {
                if (editorMode === 'gfm') {
                    addAction(htmlEditor, 'headerFirst', '# $1');
                    addAction(htmlEditor, 'headerSecond', '## $1');
                } else if (editorMode === 'htmlmixed') {
                    addAction(htmlEditor, 'headerFirst', '<h1>$1</h1>');
                    addAction(htmlEditor, 'headerSecond', '<h2>$1</h2>');
                }
            }

            function buildFileName() {
                var date = new Date();
                return 'image_' + date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDay() + '_' + date.getHours() + '-' + date.getMinutes() + '-' + date.getSeconds();
            }
        }
    }
}]);