jQuery.fn.mainSlider = function (options) {
    // default settings
    options = jQuery.extend({
        speed: 400,
        delay: 5000,
    }, options);

    var block = this;
    var counter = 0;
    var blockWidth;

    var init = function () {
        block.each(function () {
            var self = $(this);
            blockWidth = self.width();

            var el = $('.slider li', self),
                elCount = el.length,
                elWidth = blockWidth,
                timer,
                index = $('.nav li.active', self).index();

            $('.slider', self).width(parseInt(elWidth * elCount) + 'px');
            el.width(elWidth);

            $('.slider', self).removeAttr('style');
            $('.nav li:first', self).addClass('active').siblings().removeClass('active');

            clearInterval(timer);

            if (counter < 1) {
                timer = setInterval(animateSlider, options.delay);
            }

            $('.nav li', self).off('click').on('click', function () {
                index = $(this).index();

                $(this).addClass('active').siblings().removeClass('active');
                $('.slider', self).animate({ 'margin-left': '-' + parseInt(index * elWidth) + 'px' }, options.speed);

                clearInterval(timer);
                timer = setInterval(animateSlider, options.delay);
            });

            function animateSlider() {
                var eWidth = block.width();

                index++;

                if (index >= elCount) {
                    index = 0;
                }

                $('.nav li', self).eq(index).addClass('active').siblings().removeClass('active');
                $('.slider', self).stop().animate({ 'margin-left': '-' + parseInt(index * eWidth) + 'px' }, options.speed);
            }
        });

        counter++;
    };

    init();

    $(window).resize(function () {
        init();
    });
};