angular.module('platformWebApp')
    // Keeps the look (theme mode and color preset) in the signed-in user's profile through the Settings v2 "me" API,
    // so a choice made in one browser or environment is restored in every other one. localStorage (themeService)
    // stays a cache for the sign-in page and the first paint.
    .factory('platformWebApp.themeProfile', ['$q', '$timeout', 'platformWebApp.settingsV2', 'platformWebApp.themeService',
        function ($q, $timeout, settingsV2, themeService) {
            var names = {
                theme: 'VirtoCommerce.Platform.UI.Theme',
                preset: 'VirtoCommerce.Platform.UI.ThemePreset',
                selected: 'VirtoCommerce.Platform.UI.ThemeSelected'
            };
            var saveTimer = null;

            var service = {
                names: names,
                // true once the profile has been read for the signed-in user (saves wait for it)
                loaded: false,
                // the user has chosen (or skipped choosing) a look; until then the welcome dialog is offered
                selected: false,
                // the user picked a look in the theme menu while the profile was still loading
                changedWhileLoading: false,

                // Reads the user's look and applies it; resolves to whether the user has chosen one.
                // The profile always wins over the browser cache: for a user who has not chosen yet its values are the
                // defaults, so a new user does not inherit the look of the previous user of this browser.
                // A pick made in the theme menu while loading wins over the profile and is saved.
                load: function () {
                    // only the platform's own user scope settings (as modules do with their moduleId)
                    return settingsV2.getMeValues({ moduleId: 'Platform', modifiedOnly: false }).$promise.then(function (values) {
                        service.loaded = true;
                        service.selected = values[names.selected] === true || values[names.selected] === 'true';
                        if (service.changedWhileLoading) {
                            service.changedWhileLoading = false;
                            service.save();
                            return true;
                        }
                        themeService.setLook(values[names.theme], values[names.preset], { persist: false });
                        return service.selected;
                    }, function () {
                        // the settings are not known yet (older server) or the call failed: keep the browser's look
                        service.loaded = false;
                        return $q.reject();
                    });
                },

                // Stores the current look (debounced: picking presets in a row saves once)
                save: function () {
                    if (!service.loaded) {
                        // the profile is still loading: remember the pick, load() saves it
                        service.changedWhileLoading = true;
                        return;
                    }
                    if (saveTimer) {
                        $timeout.cancel(saveTimer);
                    }
                    saveTimer = $timeout(function () {
                        saveTimer = null;
                        var values = {};
                        values[names.theme] = themeService.current;
                        values[names.preset] = themeService.preset;
                        values[names.selected] = true;
                        service.selected = true;
                        settingsV2.saveMeValues(values);
                    }, 400);
                },

                reset: function () {
                    service.loaded = false;
                    service.selected = false;
                    service.changedWhileLoading = false;
                    if (saveTimer) {
                        $timeout.cancel(saveTimer);
                        saveTimer = null;
                    }
                }
            };
            return service;
        }])
    .run(['$rootScope', '$state', '$timeout', 'platformWebApp.themeService', 'platformWebApp.themeProfile', 'platformWebApp.dialogService',
        function ($rootScope, $state, $timeout, themeService, themeProfile, dialogService) {
            var welcomeTimer = null;

            function cancelWelcome() {
                if (welcomeTimer) {
                    $timeout.cancel(welcomeTimer);
                    welcomeTimer = null;
                }
            }

            function showWelcome() {
                dialogService.showDialog({
                    id: 'themeWelcome',
                    // Apply and "Not now" both remember the answer, so the question is asked once
                    callback: function () { themeProfile.save(); },
                    callbackOnDismiss: function () { themeProfile.save(); }
                }, '$(Platform)/Scripts/app/navigation/theme/themeWelcome.tpl.html', 'platformWebApp.themeWelcomeController', 'theme-welcome-window', 'static', true);
            }

            // Offer the choice once the user is in the workspace: not over the sign-in page, a forced password change,
            // the setup wizard or another dialog
            function offerWelcome(attempt) {
                cancelWelcome();
                welcomeTimer = $timeout(function () {
                    welcomeTimer = null;
                    if (themeProfile.selected || !themeProfile.loaded) {
                        return;
                    }
                    var ready = $state.includes('workspace') && !document.querySelector('.modal');
                    if (ready) {
                        showWelcome();
                    } else if (attempt < 240) {
                        offerWelcome(attempt + 1);
                    }
                }, attempt ? 500 : 1200);
            }

            $rootScope.$on('loginStatusChanged', function (event, authContext) {
                cancelWelcome();
                themeProfile.reset();
                if (!authContext.isAuthenticated) {
                    return;
                }
                themeProfile.load().then(function (selected) {
                    if (!selected && !$rootScope.isEmbeddedMode) {
                        offerWelcome(0);
                    }
                });
            });

            // a choice made in the theme menu (or the welcome dialog) is stored in the profile
            $rootScope.$on('platformWebApp.themeChanged', function (event, theme, change) {
                if (change && change.persist) {
                    themeProfile.save();
                }
            });
        }])
    // "Choose your look": shown once after the first sign-in. Every pick previews live behind the dialog;
    // "Not now" (or Esc) restores the look the user had and does not ask again; the theme menu changes it any time.
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
            themeService.setTheme(mode.id, { persist: false });
        };
        $scope.pickPreset = function (preset) {
            if (themeService.isModern()) {
                themeService.setPreset(preset.id, { persist: false });
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
            themeService.setLook(initial.theme, initial.preset, { persist: false });
        }

        $scope.apply = function () {
            $scope.$close('apply');
        };
        $scope.notNow = function () {
            restore();
            $scope.$close('skip');
        };
        // Esc closes the dialog as "Not now"
        $scope.$on('modal.closing', function (event, reason, closed) {
            if (!closed) {
                restore();
            }
        });
    }]);
