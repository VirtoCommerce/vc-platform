angular.module('platformWebApp')
    // Ctrl+Alt+T (Cmd+Option+T on a Mac) opens the "Choose your look" dialog. The look is kept in this browser
    // (themeService); the classic look stays the default until a user picks another one here.
    .run(['$document', '$rootScope', 'platformWebApp.dialogService', function ($document, $rootScope, dialogService) {
        function isShortcut(event) {
            // event.code: the physical T key, whatever the keyboard layout types with Alt
            return (event.ctrlKey || event.metaKey) && event.altKey && !event.shiftKey && event.code === 'KeyT';
        }

        $document.on('keydown', function (event) {
            if (!isShortcut(event)) {
                return;
            }
            event.preventDefault();
            // the dialog service shows a dialog once per id, so pressing the keys again keeps the open one
            $rootScope.$apply(function () {
                dialogService.showDialog({ id: 'themeWelcome' }, '$(Platform)/Scripts/app/navigation/theme/themeWelcome.tpl.html',
                    'platformWebApp.themeWelcomeController', 'theme-welcome-window', 'static', true);
            });
        });
    }])
    // "Choose your look": every pick previews live behind the dialog; Apply keeps it,
    // Cancel (or Esc) restores the look the user had.
    .controller('platformWebApp.themeWelcomeController', ['$scope', 'platformWebApp.themeService', function ($scope, themeService) {
        var initial = { theme: themeService.current, preset: themeService.preset };

        $scope.themeService = themeService;
        $scope.modes = [
            { id: 'light', icon: 'fas fa-sun', title: 'platform.menu.theme-light', description: 'platform.theme.welcome.light-description' },
            { id: 'dark', icon: 'fas fa-moon', title: 'platform.menu.theme-dark', description: 'platform.theme.welcome.dark-description' },
            { id: 'system', icon: 'fas fa-desktop', title: 'platform.menu.theme-system', description: 'platform.theme.welcome.system-description' },
            { id: 'classic', icon: 'fas fa-history', title: 'platform.menu.theme-classic', description: 'platform.theme.welcome.classic-description' }
        ];
        $scope.presets = themeService.presets;

        $scope.pickMode = function (mode) {
            themeService.setTheme(mode.id);
        };
        $scope.pickPreset = function (preset) {
            if (themeService.isModern()) {
                themeService.setPreset(preset.id);
            }
        };
        // arrow keys move between the options of a radio group (WAI-ARIA radio group pattern)
        $scope.onGroupKey = function (event, list, currentId, pick) {
            var step = { ArrowRight: 1, ArrowDown: 1, ArrowLeft: -1, ArrowUp: -1 }[event.key];
            if (!step) {
                return;
            }
            event.preventDefault();
            var index = _.findIndex(list, function (x) { return x.id === currentId; });
            var next = list[(index + step + list.length) % list.length];
            pick(next);
            var buttons = event.currentTarget.querySelectorAll('[role="radio"]');
            if (buttons[list.indexOf(next)]) {
                buttons[list.indexOf(next)].focus();
            }
        };

        function restore() {
            themeService.setLook(initial.theme, initial.preset);
        }

        $scope.apply = function () {
            $scope.$close('apply');
        };
        $scope.cancel = function () {
            restore();
            $scope.$close('cancel');
        };
        // Esc closes the dialog as Cancel
        $scope.$on('modal.closing', function (event, reason, closed) {
            if (!closed) {
                restore();
            }
        });
    }]);
