angular.module('platformWebApp')
    // Re-points literal colors of module and vendor stylesheets to the modern theme palette.
    // Module stylesheets are compiled separately with hard-coded colors (e.g. a white panel), which would
    // break dark mode. While the modern theme is on, this scans the same-origin stylesheets once and adds
    // `.vc-modern <selector> { <property>: var(--vc-<role>) }` overrides, so every module - current or
    // future - follows the theme without changes. The classic theme never sees these rules.
    .factory('platformWebApp.themeAdapter', [function () {
        var styleElementId = 'vc-theme-adapter';
        var colorProperties = {
            'color': 'fg',
            'background-color': 'bg',
            'border-top-color': 'bd',
            'border-right-color': 'bd',
            'border-bottom-color': 'bd',
            'border-left-color': 'bd',
            'outline-color': 'bd'
        };
        // classes the platform puts on <html> itself; selectors starting with them must not get a descendant prefix
        var rootClasses = ['.uk-notouch', '.uk-touch', '.vc-modern', '.vc-dark'];

        function parseColor(value) {
            var m;
            if (!value) {
                return null;
            }
            value = value.trim().toLowerCase();
            if (value === 'white') {
                return { r: 255, g: 255, b: 255, a: 1 };
            }
            if (value === 'black') {
                return { r: 0, g: 0, b: 0, a: 1 };
            }
            m = /^#([0-9a-f]{3}|[0-9a-f]{6})$/.exec(value);
            if (m) {
                var hex = m[1].length === 3 ? m[1].replace(/(.)/g, '$1$1') : m[1];
                return { r: parseInt(hex.substr(0, 2), 16), g: parseInt(hex.substr(2, 2), 16), b: parseInt(hex.substr(4, 2), 16), a: 1 };
            }
            m = /^rgba?\(\s*(\d+)[,\s]+(\d+)[,\s]+(\d+)(?:[,\s/]+([\d.]+))?\s*\)$/.exec(value);
            if (m) {
                return { r: +m[1], g: +m[2], b: +m[3], a: m[4] === undefined ? 1 : +m[4] };
            }
            return null;
        }

        function toHsl(c) {
            var r = c.r / 255, g = c.g / 255, b = c.b / 255;
            var max = Math.max(r, g, b), min = Math.min(r, g, b);
            var l = (max + min) / 2, h = 0, s = 0;
            if (max !== min) {
                var d = max - min;
                s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
                if (max === r) {
                    h = (g - b) / d + (g < b ? 6 : 0);
                } else if (max === g) {
                    h = (b - r) / d + 2;
                } else {
                    h = (r - g) / d + 4;
                }
                h *= 60;
            }
            return { h: h, s: s, l: l };
        }

        function family(hsl) {
            if (hsl.s < 0.18 || (hsl.l > 0.9 && hsl.s < 0.6 && hsl.h >= 180 && hsl.h <= 240) || (hsl.h >= 180 && hsl.h <= 230 && hsl.s < 0.35)) {
                return 'neutral';
            }
            if (hsl.h >= 185 && hsl.h <= 235) {
                return 'blue';
            }
            if (hsl.h < 18 || hsl.h >= 340) {
                return 'danger';
            }
            if (hsl.h < 55) {
                return 'warning';
            }
            if (hsl.h >= 75 && hsl.h < 170) {
                return 'success';
            }
            return 'other';
        }

        // Same mapping as the build-time token map of the platform styles (_modern-tokens.sass)
        function semantic(role, color) {
            var hsl = toHsl(color);
            var l = hsl.l, s = hsl.s;
            switch (family(hsl)) {
                case 'neutral':
                    if (role === 'bg') {
                        return l >= 0.985 ? 'plate' : l >= 0.95 ? 'panel' : l >= 0.9 ? 'sunken' : l >= 0.75 ? 'line' : l < 0.3 ? 'espresso' : 'faint';
                    }
                    if (role === 'fg') {
                        return l >= 0.97 ? null : l < 0.3 ? 'ink' : l < 0.5 ? 'ink-2' : l < 0.68 ? 'muted' : 'faint';
                    }
                    return l >= 0.97 ? 'plate' : l >= 0.9 ? 'line-soft' : l >= 0.6 ? 'line' : 'faint';
                case 'blue':
                    if (s < 0.5 && l > 0.6 && l < 0.85) {
                        return role === 'fg' ? 'faint' : 'line';
                    }
                    if (role === 'bg') {
                        return l >= 0.94 ? 'hover' : l >= 0.85 ? 'select' : 'primary';
                    }
                    if (role === 'fg') {
                        return 'link';
                    }
                    return s < 0.5 || l > 0.75 ? 'line' : 'focus';
                case 'danger':
                case 'warning':
                case 'success':
                    var fam = family(hsl);
                    if (role === 'bg') {
                        return fam + (l >= 0.85 ? '-soft' : '');
                    }
                    if (role === 'fg') {
                        return fam + (l >= 0.85 ? '-soft' : l < 0.45 ? '-ink' : '');
                    }
                    return l >= 0.75 ? fam + '-line' : fam;
                default:
                    return null;
            }
        }

        function prefixSelector(selector) {
            return selector.split(',').map(function (part) {
                var sel = part.trim();
                if (!sel) {
                    return sel;
                }
                if (/^(html|:root)\b/.test(sel)) {
                    return sel.replace(/^(html|:root)/, 'html.vc-modern');
                }
                for (var i = 0; i < rootClasses.length; i++) {
                    if (sel.indexOf(rootClasses[i]) === 0) {
                        return '.vc-modern' + sel;
                    }
                }
                return '.vc-modern ' + sel;
            }).join(', ');
        }

        function overridesFor(rule) {
            var declarations = [];
            angular.forEach(colorProperties, function (role, property) {
                var value = rule.style.getPropertyValue(property);
                var color = parseColor(value);
                if (!color || color.a < 0.9) {
                    return;
                }
                var target = semantic(role, color);
                if (target) {
                    var priority = rule.style.getPropertyPriority(property);
                    declarations.push(property + ': var(--vc-' + target + ')' + (priority ? ' !' + priority : ''));
                }
            });
            return declarations.length ? prefixSelector(rule.selectorText) + ' { ' + declarations.join('; ') + ' }' : null;
        }

        function collect(rules, out) {
            for (var i = 0; i < rules.length; i++) {
                var rule = rules[i];
                if (rule.type === 1) { // CSSStyleRule
                    if (rule.selectorText.indexOf('vc-modern') >= 0) {
                        continue; // rules of the modern theme itself
                    }
                    var css = overridesFor(rule);
                    if (css) {
                        out.push(css);
                    }
                } else if (rule.type === 4 && rule.cssRules) { // CSSMediaRule
                    var inner = [];
                    collect(rule.cssRules, inner);
                    if (inner.length) {
                        out.push('@media ' + rule.conditionText + ' { ' + inner.join('\n') + ' }');
                    }
                }
            }
        }

        return {
            apply: function () {
                if (document.getElementById(styleElementId)) {
                    return;
                }

                var out = [];
                angular.forEach(document.styleSheets, function (sheet) {
                    if (sheet.ownerNode && sheet.ownerNode.id === styleElementId) {
                        return;
                    }
                    var rules;
                    try {
                        rules = sheet.cssRules;
                    } catch (e) {
                        return; // cross-origin stylesheet (fonts, charts)
                    }
                    if (rules) {
                        collect(rules, out);
                    }
                });

                var style = document.createElement('style');
                style.id = styleElementId;
                style.textContent = out.join('\n');
                // Placed before every stylesheet: the prefixed selectors already outrank the module and vendor
                // rules they re-color, while the theme's own rules (same specificity, later) still win.
                document.head.insertBefore(style, document.head.firstChild);
            },
            remove: function () {
                var style = document.getElementById(styleElementId);
                if (style) {
                    style.parentNode.removeChild(style);
                }
            }
        };
    }]);
