jQuery.fn.mainSlider = function(options)
{
	// default settings
	var options = jQuery.extend({
		speed: 400
	}, options);

	var block = this;
	var countdown = 5000;
	var counter = 0;
	var blockWidth;

    var init = function(this_) {
        block.each(function() {
            var self = $(this);
            blockWidth = self.width();

            var el = $('.slider li', self),
                elCount = el.length,
                elWidth = blockWidth,
                timer,
                index = $('.nav li.active', self).index();

            $('.slider', self).width(parseInt(elWidth * elCount) + 'px');
            el.width(elWidth - 30);

            $('.slider', self).removeAttr('style');
            $('.nav li:first', self).addClass('active').siblings().removeClass('active');

            clearInterval(timer);

            if (counter < 1) {
                timer = setInterval(animateSlider, countdown);
            }

            $('.nav li', self).off('click').on('click', function() {
                index = $(this).index();

                $(this).addClass('active').siblings().removeClass('active');
                $('.slider', self).animate({ 'margin-left': '-' + parseInt(index * elWidth) + 'px' }, options.speed);

                clearInterval(timer);
                timer = setInterval(animateSlider, countdown);
            });

            function animateSlider() {
                var elWidth = block.width();

                index++;

                if (index >= elCount) {
                    index = 0;
                }

                $('.nav li', self).eq(index).addClass('active').siblings().removeClass('active');
                $('.slider', self).stop(true, true).animate({ 'margin-left': '-' + parseInt(index * elWidth) + 'px' }, 500);
            }
        });

        counter++;
    };

	init();

	$(window).resize(function (){
		init();
	});
};