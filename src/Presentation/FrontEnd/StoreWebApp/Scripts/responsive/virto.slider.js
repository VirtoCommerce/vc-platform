jQuery.fn.virtoSlider = function(options)
{
	// default settings
	var options = jQuery.extend({
		elementVisible: '',
		speed: 500
	}, options);

	var this_ = this;

	var init = function(this_)
	{
		var slider = jQuery('.slider-list', this_),
			element = jQuery(' > li', slider),
			elementWidth = element.width() + parseInt(element.css('margin-left')) * 2,
			elementCounts = element.length,
			elementVisible = options.elementVisible ? options.elementVisible : parseInt($(this_).width() / elementWidth),
			index = 0;

		slider.css('width', parseInt(elementWidth * elementCounts));

		// Append navigation
		$('.nav', this_).remove();

		if (elementCounts > elementVisible)
		{
		$(this_).append(
			'<ul class="nav">' +
				'<li class="left"><a>Left</a></li>' +
				'<li class="right"><a>Right</a></li>' +
			'</ul>'
		);
	    }

		// Animation slider
		var animateSlider = function(e)
		{
			var self = $(this),
				pos = e.data.pos;

			if(pos == 'left')
			{
				if(index > 0)
				{
					index--;
				}

				self.off('click');

				slider.animate({'left': '-' + (index * elementWidth) + 'px'}, options.speed, function (){
					$('.nav .left', this_).on('click', {pos: 'left'}, animateSlider);
				});
			}
			else
			{
				if(index < elementCounts - elementVisible)
				{
					index++;
				}

				self.off('click');

				slider.animate({'left': '-' + (index * elementWidth) + 'px'}, options.speed, function (){
					$('.nav .right', this_).on('click', {pos: 'right'}, animateSlider);
				});
			}
		}

		// Events for navigation
		$('.nav .left', this_).on('click', {pos:'left'}, animateSlider);
		$('.nav .right', this_).on('click', {pos:'right'}, animateSlider);
	};

	return this.each(function (){
		init(this);
	});
};