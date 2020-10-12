angular.module('platformWebApp').directive('vaTabs', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            $('body').delegate('.tab-item', 'click', function () {
                var self = $(this);
                const index = self.index();

                self.addClass('__selected').siblings().removeClass('__selected');
                self.parents('.tabs').find('.tab-cnt').eq(index).addClass('__opened').siblings().removeClass('__opened');
            });
        }
    }
});
