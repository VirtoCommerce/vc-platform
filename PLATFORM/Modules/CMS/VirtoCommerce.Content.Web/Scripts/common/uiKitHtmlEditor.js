angular.module('virtoCommerce.contentModule').directive('uiKitHtmlEditor', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        scope: {
            ngModel: '='
        },
        link: function (scope, element, attrs, ngModel) {
            $(element).html(scope.ngModel);

            UIkit.htmleditor($('textarea[ui-kit-html-editor]'), { mode: 'tab', toolbar: ['bold', 'italic', 'strike', 'link', 'image', 'blockquote', 'listUl', 'listOl'] });

            var editor = $('.CodeMirror')[0].CodeMirror;

            editor.on('change', function () {
                ngModel = editor.getValue();
            });

            editor.setOption("mode", "gfm");

            scope.$on('resetContent', function (event, arg) {
                var editor = $('.CodeMirror')[0].CodeMirror;

                editor.setValue(arg.body);
            });

            scope.$on('changeEditType', function (event, arg) {
                if (arg.editAsHtml) {
                    $('.CodeMirror')[0].CodeMirror.setOption("mode", "htmlmixed");
                }
                else if (arg.editAsMarkdown) {
                    $('.CodeMirror')[0].CodeMirror.setOption("mode", "gfm");
                }
            });
        }
    }
});