angular.module('platformWebApp')
    .directive('vaPasswordToggle', ['$compile', function ($compile) {
        // Two design systems are live: the legacy `form-input` blades and the modern `vc-input`
        // auth screens. They differ only in class names and in which ancestor carries the
        // "--password" modifier that reveals the switcher, so describe both and pick by markup.
        var SKINS = [
            {
                container: 'form-input',
                modifier: 'form-input--password',
                modifierOnGrandparent: false,
                button: 'form-input__password-switcher',
                icon: 'form-input__password-icon'
            },
            {
                container: 'vc-input__container',
                modifier: 'vc-input--password',
                modifierOnGrandparent: true,
                button: 'vc-input__password-switcher',
                icon: 'vc-input__icon'
            }
        ];

        function buttonTemplate(skin) {
            return '<button type="button" class="' + skin.button + '" ng-click="togglePasswordVisibility()">' +
                '<svg class="' + skin.icon + '" ng-if="!passwordVisible"><use href="/images/eye.svg#icon"></use></svg>' +
                '<svg class="' + skin.icon + '" ng-if="passwordVisible"><use href="/images/eye-off.svg#icon"></use></svg>' +
                '</button>';
        }

        return {
            restrict: 'A',
            link: function (scope, element) {
                var parent = element.parent();
                var skin = _.find(SKINS, function (candidate) { return parent.hasClass(candidate.container); });
                if (!skin) {
                    return;
                }

                (skin.modifierOnGrandparent ? parent.parent() : parent).addClass(skin.modifier);

                var childScope = scope.$new();
                childScope.passwordVisible = false;
                childScope.togglePasswordVisibility = function () {
                    childScope.passwordVisible = !childScope.passwordVisible;
                    element.attr('type', childScope.passwordVisible ? 'text' : 'password');
                };

                var button = $compile(angular.element(buttonTemplate(skin)))(childScope);
                element.after(button);

                element.on('$destroy', function () {
                    childScope.$destroy();
                });
            }
        };
    }]);
