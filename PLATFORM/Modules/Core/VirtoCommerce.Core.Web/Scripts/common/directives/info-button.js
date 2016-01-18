angular.module('virtoCommerce.coreModule.common')
.directive('vaInfoButton', ['$document', function ($document) {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            toolText: '@'
        },
        template: '<button class="btn"><i class="btn-ico fa fa-info"></i></button>',
        link: function (scope, element, attrs) {
            $(element).parents('.form-input').addClass('__info');

            function handleDocumentClickEvent(event) {
                {
                    if (!$('.tooltip, .btn').is(event.target) && !$('.tooltip, .btn').has(event.target).length) {
                        $('.tooltip').remove();
                        $document.unbind('click', handleDocumentClickEvent);
                    }
                }
            }

            element.bind('click', function () {
                var posLeft = $(this).offset().left + 42,
                    posTop = $(this).offset().top;

                $('.tooltip').remove();

                $('body').prepend('<div class="tooltip" style="left: ' + posLeft + 'px; top: ' + posTop + 'px;"><div class="tooltip-cnt">' + scope.toolText + '</div></div>');

                $document.on("click", handleDocumentClickEvent);
            });
        }
    };
}]);