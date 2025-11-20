'use strict';

angular.module('platformWebApp')
    .directive('vcEnvironmentBanner', [function() {
        return {
            restrict: 'E',
            replace: true,
            scope: {
                environmentName: '@',
                theme: '@?'
            },
            templateUrl: '$(Platform)/Scripts/app/environmentBanner/environment-banner.tpl.html',
            controller: ['$scope', '$window', function($scope, $window) {
                function detectThemePreset(env, host) {
                    var s = ((env || '') + ' ' + (host || '')).toLowerCase();
                    if (s.indexOf('prod') !== -1) return 'prod';
                    if (s.indexOf('localhost') !== -1 || /\b\d{1,3}(\.\d{1,3}){3}\b/.test(s) || s.indexOf('dev') !== -1) return 'dev';
                    if (s.indexOf('qa') !== -1 || s.indexOf('test') !== -1) return 'test';
                    if (s.indexOf('stage') !== -1 || s.indexOf('staging') !== -1 || s.indexOf('uat') !== -1) return 'staging';
                    if (s.indexOf('demo') !== -1) return 'demo';
                    return 'default';
                }

                function createDefaultLabel(host, theme) {
                    switch (theme) {
                        case 'prod':
                            return 'Production';
                        case 'dev':
                            return 'Development';
                        case 'test':
                            return 'QA';
                        case 'staging':
                            return 'UAT';
                        case 'demo':
                            return 'Demo';
                    }

                    return host;
                }

                function createThemeStyle(theme) {
                    switch (theme) {
                        case 'prod':
                            return 'prod';
                        case 'dev':
                            return 'dev';
                        case 'test':
                            return 'test';
                        case 'staging':
                            return 'staging';
                        case 'demo':
                            return 'demo';
                    }

                    return 'default';
                }


                function createBannerModel(env, preset) {
                    var name = (env || '').toString();
                    var host = ($window && $window.location && $window.location.hostname) || '';
                    var theme = (preset && preset.toString().toLowerCase()) || detectThemePreset(env, host);
                    return {
                        label: name || createDefaultLabel(host, theme),
                        badgeClass: 'vc-env-badge--' + createThemeStyle(theme)
                    };
                }

                function update() {
                    var model = createBannerModel($scope.environmentName, $scope.theme);
                    $scope.label = model.label;
                    $scope.badgeClass = model.badgeClass;
                }

                $scope.$watchGroup(['environmentName', 'theme'], update);
            }]
        };
    }]);


