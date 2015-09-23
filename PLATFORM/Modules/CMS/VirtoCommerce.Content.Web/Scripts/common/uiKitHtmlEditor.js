angular.module('virtoCommerce.contentModule').directive('uiKitHtmlEditor', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        scope: {
            ngModel: '='
        },
        link: function (scope, element, attrs, ngModel) {
            $(element).html(scope.ngModel);

            UIkit.htmleditor($('textarea[ui-kit-html-editor]'), { mode: 'tab', toolbar: ['bold', 'italic', 'strike', 'link', 'image', 'blockquote', 'listUl', 'listOl'], markdown: false });

            var editor = $('.CodeMirror')[0].CodeMirror;

            editor.on('change', function () {
                ngModel = editor.getValue();
            });

            scope.$on('resetContent', function (event, arg) {
                var editor = $('.CodeMirror')[0].CodeMirror;

                editor.setValue(arg.body);
            });
        }
    }
});