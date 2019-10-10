angular.module('platformWebApp')
.directive('multipleAttr', function () {
    return {
        'restrict': 'A',
        'compile': function () {
            return {
                'pre': function ($s, $el, attrs) {
                    if (attrs.multipleAttr === 'true' || attrs.multipleAttr === true) {
                        $el.attr('multiple', '');
                    }
                }
            }
        }
    }
})