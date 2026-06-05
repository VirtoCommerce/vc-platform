// va-reset-confirm-popover
// The reset confirmation popover lives inside the settings property list, which is a
// scroll container (overflow-y: auto). A position:absolute popover would be clipped by
// that container near the bottom edge. This directive re-anchors the popover with
// position:fixed (relative to the viewport) so it is never clipped, and keeps it pinned
// to its reset icon while the list is scrolled or the window is resized.
angular.module('platformWebApp')
    .directive('vaResetConfirmPopover', ['$window', function ($window) {
        return {
            restrict: 'A',
            link: function (scope, element) {
                var el = element[0];

                function reposition() {
                    var container = el.parentElement; // .settings-reset (wraps icon + popover)
                    var icon = container && container.querySelector('.settings-reset__ico');
                    if (!icon) { return; }

                    var rect = icon.getBoundingClientRect();
                    var width = el.offsetWidth || 230;
                    var height = el.offsetHeight || 90;
                    var margin = 8;
                    var gap = 6;
                    var viewportW = $window.innerWidth;
                    var viewportH = $window.innerHeight;

                    // Right-align the popover's right edge with the icon, then clamp to viewport.
                    var left = rect.right - width;
                    if (left < margin) { left = margin; }
                    if (left + width > viewportW - margin) { left = viewportW - margin - width; }

                    // Open below the icon by default; flip above if there isn't room below.
                    var openUp = (rect.bottom + gap + height > viewportH - margin) && (rect.top - gap - height > margin);
                    var top = openUp ? (rect.top - gap - height) : (rect.bottom + gap);

                    el.classList.toggle('settings-reset__popover--up', openUp);
                    el.style.position = 'fixed';
                    el.style.top = Math.round(top) + 'px';
                    el.style.left = Math.round(left) + 'px';
                    el.style.right = 'auto';

                    // Keep the little arrow pointing at the icon regardless of clamping.
                    var iconCenter = rect.left + rect.width / 2;
                    var arrowRight = (left + width) - iconCenter - 5;
                    arrowRight = Math.max(8, Math.min(width - 18, arrowRight));
                    el.style.setProperty('--arrow-right', Math.round(arrowRight) + 'px');
                }

                reposition();
                // Re-measure on the next frames: at link time the popover content and the
                // surrounding layout (scrollbar, blade sizing) may not be settled yet, which
                // could make the flip-up decision use stale geometry. rAF guarantees a final,
                // correct placement after paint.
                var raf1 = $window.requestAnimationFrame(function () {
                    reposition();
                    raf2 = $window.requestAnimationFrame(reposition);
                });
                var raf2 = null;

                // Capture phase so scrolls inside the (non-bubbling) inner panel are caught too.
                var onChange = function () { reposition(); };
                $window.addEventListener('scroll', onChange, true);
                $window.addEventListener('resize', onChange);

                scope.$on('$destroy', function () {
                    $window.cancelAnimationFrame(raf1);
                    if (raf2) { $window.cancelAnimationFrame(raf2); }
                    $window.removeEventListener('scroll', onChange, true);
                    $window.removeEventListener('resize', onChange);
                });
            }
        };
    }]);
