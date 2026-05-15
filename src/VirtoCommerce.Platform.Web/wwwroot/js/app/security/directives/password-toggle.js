angular.module('platformWebApp')
    .directive('vaPasswordToggle', ['$compile', function ($compile) {
        var BUTTON_TEMPLATE =
            '<button type="button" class="form-input__password-switcher" ng-click="togglePasswordVisibility()">' +
                '<svg class="form-input__password-icon" ng-if="!passwordVisible"><use href="/images/eye-off.svg#icon"></use></svg>' +
                '<svg class="form-input__password-icon" ng-if="passwordVisible"><use href="/images/eye.svg#icon"></use></svg>' +
            '</button>';

        return {
            restrict: 'A',
            link: function (scope, element) {
                var childScope = scope.$new();
                childScope.passwordVisible = false;
                childScope.togglePasswordVisibility = function () {
                    childScope.passwordVisible = !childScope.passwordVisible;
                    element.attr('type', childScope.passwordVisible ? 'text' : 'password');
                };

                var parent = element.parent();
                if (parent.hasClass('form-input')) {
                    parent.addClass('form-input--password');
                }

                var button = $compile(angular.element(BUTTON_TEMPLATE))(childScope);
                element.after(button);

                element.on('$destroy', function () {
                    childScope.$destroy();
                });
            }
        };
    }]);
