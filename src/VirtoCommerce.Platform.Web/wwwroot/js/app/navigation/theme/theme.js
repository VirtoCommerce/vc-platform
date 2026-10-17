angular.module('platformWebApp')
    .factory('platformWebApp.themeService', ['$rootScope', 'platformWebApp.themeAdapter', function ($rootScope, themeAdapter) {
        // classic: the original look; light/dark: the modern look; system: modern, following the OS color scheme
        var themes = ['light', 'dark', 'system', 'classic'];
        // the classic look until the user picks another one in the look dialog (Ctrl+Alt+T, themeWelcome.js)
        var defaultTheme = 'classic';
        // Color presets of the modern look (light and dark variants in _modern-presets.sass); warm is the built-in palette
        // in _modern.sass, the others override it.
        // swatch: the preset's primary color and ground, shown in the theme menu and the welcome dialog
        var presets = [
            { id: 'warm', title: 'platform.theme.presets.warm', swatch: ['#e5451c', '#f5f1ea'] },
            { id: 'blue', title: 'platform.theme.presets.blue', swatch: ['#237aa5', '#dbe4ec'] },
            { id: 'gray', title: 'platform.theme.presets.gray', swatch: ['#465769', '#d9effa'] },
            { id: 'green', title: 'platform.theme.presets.green', swatch: ['#0a7a2f', '#eef3ec'] },
            { id: 'indigo', title: 'platform.theme.presets.indigo', swatch: ['#4a63e0', '#eaeaeb'] }
        ];
        var presetIds = presets.map(function (p) { return p.id; });
        var defaultPreset = 'gray';
        var basePreset = 'warm';
        var storageKey = 'VirtoCommerce.Platform.UI.Theme';
        var presetStorageKey = 'VirtoCommerce.Platform.UI.ThemePreset';
        var darkQuery = window.matchMedia ? window.matchMedia('(prefers-color-scheme: dark)') : null;

        // The theme and preset are kept in this browser's localStorage.
        // Storage can be unavailable (private mode, blocked site data); fall back to the defaults then.
        function read(key) {
            try {
                return window.localStorage.getItem(key);
            } catch (e) {
                return null;
            }
        }

        function write(key, value) {
            try {
                window.localStorage.setItem(key, value);
            } catch (e) {
                // ignore: the choice still applies for this page load
            }
        }

        function readTheme() {
            var value = read(storageKey);
            // 'modern' was the value of the first preview build
            if (value === 'modern') {
                value = 'light';
            }
            return themes.indexOf(value) >= 0 ? value : defaultTheme;
        }

        function readPreset() {
            var value = read(presetStorageKey);
            return presetIds.indexOf(value) >= 0 ? value : defaultPreset;
        }

        function changed() {
            $rootScope.$broadcast('platformWebApp.themeChanged', service.current, {
                theme: service.current,
                preset: service.preset
            });
        }

        var service = {
            themes: themes,
            presets: presets,
            defaultTheme: defaultTheme,
            defaultPreset: defaultPreset,
            current: readTheme(),
            preset: readPreset(),
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
            isValidTheme: function (theme) {
                return themes.indexOf(theme) >= 0;
            },
            isValidPreset: function (preset) {
                return presetIds.indexOf(preset) >= 0;
            },
            apply: function () {
                var root = document.documentElement;
                var isModern = service.isModern();
                root.classList.toggle('vc-modern', isModern);
                root.classList.toggle('vc-dark', isModern && service.isDark());
                // the warm palette is the modern look's own; other presets override its colors
                if (isModern && service.preset !== basePreset) {
                    root.setAttribute('data-vc-preset', service.preset);
                } else {
                    root.removeAttribute('data-vc-preset');
                }
                if (isModern) {
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
            // Small screens: the main menu is a drawer
            toggleMobileMenu: function (open) {
                var root = document.documentElement;
                root.classList.toggle('vc-menu-open', open === undefined ? !root.classList.contains('vc-menu-open') : !!open);
            },
            setTheme: function (theme) {
                service.current = service.isValidTheme(theme) ? theme : defaultTheme;
                write(storageKey, service.current);
                service.apply();
                changed();
            },
            setPreset: function (preset) {
                service.preset = service.isValidPreset(preset) ? preset : defaultPreset;
                write(presetStorageKey, service.preset);
                service.apply();
                changed();
            },
            // both at once (one change notification)
            setLook: function (theme, preset) {
                service.current = service.isValidTheme(theme) ? theme : defaultTheme;
                service.preset = service.isValidPreset(preset) ? preset : defaultPreset;
                write(storageKey, service.current);
                write(presetStorageKey, service.preset);
                service.apply();
                changed();
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
        // Book mode (blades fold to spines): always in the modern look; the classic look uses it on small screens
        // only, where it shows one blade at a time. Keep the breakpoint in sync with $mobileBreakpoint in _modern-mobile.sass
        var smallScreenQuery = window.matchMedia ? window.matchMedia('(max-width: 767px)') : null;
        function updateBookMode() {
            bladeNavigationService.setBookMode(themeService.isModern() || !!(smallScreenQuery && smallScreenQuery.matches));
        }
        updateBookMode();
        if (smallScreenQuery) {
            var onSmallScreenChange = function () {
                $rootScope.$applyAsync(updateBookMode);
            };
            if (smallScreenQuery.addEventListener) {
                smallScreenQuery.addEventListener('change', onSmallScreenChange);
            } else if (smallScreenQuery.addListener) {
                smallScreenQuery.addListener(onSmallScreenChange);
            }
        }
        if (document.readyState !== 'complete') {
            window.addEventListener('load', themeService.refreshAdapter);
        }

        // Close the menu drawer after navigating, picking a menu entry, or tapping outside of it
        $rootScope.$on('$locationChangeSuccess', function () {
            themeService.toggleMobileMenu(false);
        });
        document.addEventListener('click', function (event) {
            if (!document.documentElement.classList.contains('vc-menu-open') || event.target.closest('.header__menu-toggle')) {
                return;
            }
            var menuItem = event.target.closest('.nav-bar .list-item');
            if (menuItem) {
                // entries that open a sub-menu (e.g. "More") keep the drawer open
                if (!menuItem.classList.contains('__has-dropdown') && !event.target.closest('.list-fav')) {
                    themeService.toggleMobileMenu(false);
                }
                return;
            }
            if (!event.target.closest('.nav-bar')) {
                themeService.toggleMobileMenu(false);
            }
        }, true);
        $rootScope.$on('platformWebApp.themeChanged', updateBookMode);
    }]);
