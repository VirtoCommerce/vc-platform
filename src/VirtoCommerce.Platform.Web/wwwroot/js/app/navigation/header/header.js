angular.module('platformWebApp').directive('vaHeader', ["$state", '$document', 'platformWebApp.themeService',
    function ($state, $document, themeService) {
        return {
            restrict: 'E',
            replace: true,
            transclude: true,
            templateUrl: '$(Platform)/Scripts/app/navigation/header/header.tpl.html',
            link: function (scope) {
                //tooltip popup delay for all header widgets
                scope.tooltipPopupDelay = 2000;

                scope.manageSettings = function () {
                    $state.go('workspace.settings');
                }

                // Theme picker
                scope.themeService = themeService;
                scope.themeOptions = [
                    { id: 'light', icon: 'fas fa-sun', title: 'platform.menu.theme-light' },
                    { id: 'dark', icon: 'fas fa-moon', title: 'platform.menu.theme-dark' },
                    { id: 'system', icon: 'fas fa-desktop', title: 'platform.menu.theme-system' },
                    { id: 'classic', icon: 'fas fa-history', title: 'platform.menu.theme-classic' }
                ];
                // the modern modes first, then the color presets, then the classic look at the bottom
                scope.modernThemes = scope.themeOptions.filter(function (theme) { return theme.id !== 'classic'; });
                scope.classicTheme = _.findWhere(scope.themeOptions, { id: 'classic' });
                scope.themeMenuOpen = false;
                // hidden from users for now: the look dialog (Ctrl+Alt+T) changes the look; true shows the menu again
                scope.showThemeMenu = false;

                function closeThemeMenu() {
                    scope.$apply(function () {
                        scope.themeMenuOpen = false;
                    });
                    $document.off('click', closeThemeMenu);
                }

                scope.toggleThemeMenu = function (event) {
                    event.stopPropagation();
                    scope.themeMenuOpen = !scope.themeMenuOpen;
                    if (scope.themeMenuOpen) {
                        $document.on('click', closeThemeMenu);
                    } else {
                        $document.off('click', closeThemeMenu);
                    }
                };

                scope.selectTheme = function (theme) {
                    themeService.setTheme(theme.id);
                };

                scope.selectPreset = function (preset) {
                    themeService.setPreset(preset.id);
                };

                scope.toggleMobileMenu = function (event) {
                    event.stopPropagation();
                    themeService.toggleMobileMenu();
                };
                scope.$on('$destroy', function () {
                    $document.off('click', closeThemeMenu);
                });
            }
        }
    }]);
