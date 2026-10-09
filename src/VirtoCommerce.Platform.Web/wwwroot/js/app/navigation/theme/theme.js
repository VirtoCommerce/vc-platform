angular.module('platformWebApp')
    .factory('platformWebApp.themeService', ['$rootScope', 'platformWebApp.themeAdapter', function ($rootScope, themeAdapter) {
        // classic: the original look; light/dark: the modern look; system: modern, following the OS color scheme
        var themes = ['light', 'dark', 'system', 'classic'];
        var defaultTheme = 'light';
        var storageKey = 'VirtoCommerce.Platform.UI.Theme';
        var fontsLinkId = 'vc-modern-fonts';
        var fontsHref = 'https://fonts.googleapis.com/css2?family=Geologica:wght@500;600;700&family=Inter:wght@400;500;600;700&display=swap';
        var darkQuery = window.matchMedia ? window.matchMedia('(prefers-color-scheme: dark)') : null;

        // The theme is a per-browser preference, so it is kept in localStorage rather than in the user profile.
        // Storage can be unavailable (private mode, blocked site data); fall back to the default theme then.
        function readTheme() {
            var value;
            try {
                value = window.localStorage.getItem(storageKey);
            } catch (e) {
                value = null;
            }
            // 'modern' was the value of the first preview build
            if (value === 'modern') {
                value = 'light';
            }
            return themes.indexOf(value) >= 0 ? value : defaultTheme;
        }

        function writeTheme(theme) {
            try {
                window.localStorage.setItem(storageKey, theme);
            } catch (e) {
                // ignore: the theme still applies for this page load
            }
        }

        function ensureFonts() {
            if (document.getElementById(fontsLinkId)) {
                return;
            }

            var link = document.createElement('link');
            link.id = fontsLinkId;
            link.rel = 'stylesheet';
            link.href = fontsHref;
            document.head.appendChild(link);
        }

        var service = {
            themes: themes,
            current: readTheme(),
            isModern: function () {
                return service.current !== 'classic';
            },
            isDark: function () {
                return service.current === 'dark' || (service.current === 'system' && !!darkQuery && darkQuery.matches);
            },
            // the modern look on a light surface (e.g. the header needs the dark-text logo)
            isLightModern: function () {
                return service.isModern() && !service.isDark();
            },
            apply: function () {
                var root = document.documentElement;
                var isModern = service.isModern();
                root.classList.toggle('vc-modern', isModern);
                root.classList.toggle('vc-dark', isModern && service.isDark());
                if (isModern) {
                    ensureFonts();
                    themeAdapter.apply();
                } else {
                    themeAdapter.remove();
                }
            },
            // Re-scan stylesheets, e.g. once late module stylesheets have loaded
            refreshAdapter: function () {
                themeAdapter.remove();
                if (service.isModern()) {
                    themeAdapter.apply();
                }
            },
            setTheme: function (theme) {
                service.current = themes.indexOf(theme) >= 0 ? theme : defaultTheme;
                writeTheme(service.current);
                service.apply();
                $rootScope.$broadcast('platformWebApp.themeChanged', service.current);
            }
        };

        if (darkQuery) {
            var onSchemeChange = function () {
                if (service.current === 'system') {
                    service.apply();
                    // bindings such as the header logo depend on the scheme
                    $rootScope.$applyAsync();
                }
            };
            if (darkQuery.addEventListener) {
                darkQuery.addEventListener('change', onSchemeChange);
            } else if (darkQuery.addListener) {
                darkQuery.addListener(onSchemeChange);
            }
        }

        return service;
    }])
    .run(['$rootScope', 'platformWebApp.themeService', 'platformWebApp.bladeNavigationService', function ($rootScope, themeService, bladeNavigationService) {
        // available to templates as $root.themeService
        $rootScope.themeService = themeService;
        themeService.apply();
        bladeNavigationService.setBookMode(themeService.isModern());
        if (document.readyState !== 'complete') {
            window.addEventListener('load', themeService.refreshAdapter);
        }
        $rootScope.$on('platformWebApp.themeChanged', function () {
            bladeNavigationService.setBookMode(themeService.isModern());
        });
    }]);
