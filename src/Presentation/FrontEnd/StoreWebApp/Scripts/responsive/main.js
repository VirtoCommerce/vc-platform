(function ($){

	'use strict';

	$(function (){
		window.UI.headNav();
		window.UI.asideSlider();
		window.UI.scrollTop();
		window.UI.footerNav();
		window.Other.selectViewMode();
		window.Other.openTabs();
	});

	window.Other = {};
	window.UI = {};




	/* ----------------Other---------------- */
	window.Other.closestObject = function(visibleEl, hideEl, callBack)
	{
		$('body').click(function (e){

			if($(e.target).closest(visibleEl).length === 0)
			{
				$(hideEl).hide();

				if(typeof callBack === 'function')
				{
					callBack();
				}
			}
		});
	};

	window.Other.openTabs = function()
	{
		$('.tabs-content:not(:first)').hide();

		if(window.innerWidth < 768)
		{
			$('.tabs-content').hide();
		}
		
		$('.tabs > ul > li').on('click', function(){
			var tabs = $(this).parents('.tabs');

			$(this).addClass('selected').siblings().removeClass('selected');
			$('.tabs .head[data-index=' + $(this).index() + ']').addClass('selected').siblings().removeClass('selected');

			$('.tabs-content', tabs).hide();
			$('.tabs-content', tabs).eq($(this).index()).show();

			return false;
		});

		// Mobile navigation
		$('.tabs > .head').click(function (){
			var index = $(this).data('index');

			if($(this).next().is(':hidden'))
			{
				$(this).addClass('selected').siblings().removeClass('selected');
				$('.tabs ul li').eq(index).addClass('selected').siblings().removeClass('selected');

				$('.tabs .tabs-content').slideUp(500);
				$(this).next().slideDown(500);
			}
			else
			{
				$('.tabs ul li:first').addClass('selected').siblings().removeClass('selected');
				$(this).next().slideUp(500);
			}

			return false;
		});
	};

	window.Other.selectViewMode = function()
	{
		$('.view-mode a').click(function (){
			var name = $(this).attr('class').split(' ')[0];

			$('.view-mode a.' + name).addClass('current').siblings().removeClass('current');
			$('div.' + name).addClass('selected').siblings().removeClass('selected');
		});
	};


	/* ----------------UI---------------- */
	window.UI.headNav = function()
	{
		// For dropdowns menu
		$('.header .nav .menu li:not(.more)').each(function (){
			var self = $(this);

			if($('.sub-menu, ul', self).length)
			{
				self.addClass('dropdown');

				$('.level1 > a', self).append('<span>+</span>');
			}

			$('.arrow').remove();
			$('li.dropdown > a').append('<span class="arrow"></span>');
			$('.level1 .arrow, .level2 .arrow').remove();
		});

		// For mobile nav
		$('.header .nav .control').click(function (){
			$(this).parent().toggleClass('opened');

			window.Other.closestObject('.header .nav .control, .header .nav .menu li', '', function (){
				$('.header .nav').removeClass('opened');
			});
		});

		// Search for mobile
		$('.header .head-top-block .control').click(function (){
			$(this).parent().toggleClass('opened');
		});

		// Events for dropdowns
		$('.header .nav .menu li.dropdown > a .arrow').click(function (){
		    $(this).parent().parent().toggleClass('opened');
		    return false;
		});

		$('.header .nav .menu li.dropdown .fade').click(function (){
			$(this).parent().removeClass('opened');
		});

		// Events for dropdown sub menu
		$('.header .menu li .sub-menu ul.level0 li.level1.dropdown a span').on('click', function (){
			var self = $(this).parent().parent();

			if($('ul', self).is(':hidden'))
			{
				$('ul', self).show();
				$(this).text('-');
				self.addClass('opened');
			}
			else
			{
				$('ul', self).hide();
				$(this).text('+');
				self.removeClass('opened');
			}

			return false;
		});

		// Fixed header on scrolling
		$(window).scroll(function (){
			var scroll = $(this).scrollTop();

			if(scroll >= 35)
			{
				$('.header').addClass('fixed');
			}
			else
			{
				$('.header').removeClass('fixed');
			}
		});
	};

	window.UI.asideSlider = function()
	{
		var block = $('.article .home .block .slider ul'),
			slide = $('li', block),
			slide_width = slide.width(),
			counts = slide.length,
			block_height,
			block_width,
			slide_nav = $('<ol></ol>'),
			countdown = 5000,
			timer,
			index = 0;

		if(counts < 2)
		{
			return;
		}

		timer = setInterval(animateSlide, countdown);

		block_width = parseInt(counts * slide_width);

		slide.each(function (){
			block_height = $(this).height();
		});

		block.css({
			height: block_height,
			width: block_width
		});

		slide.eq(0).addClass('selected');

		// Navigation
		var li = '';

		for(var i = 1; i <= counts; i++)
		{
			li += '<li><a href="">' + i + '</a></li>';
		}

		slide_nav.append(li);
		$('li:eq(0)', slide_nav).addClass('selected');
		block.parent().after(slide_nav);

		$('.article .home .block .slider ol li').on('click', function (){
			var self = $(this);
			index = self.index();

			if(self.hasClass('selected'))
			{
				return false;
			}

			self.addClass('selected').siblings().removeClass('selected');
			slide.eq(index).addClass('selected').siblings().removeClass('selected');
			block.animate({'margin-left': '-' + index * slide_width + 'px'});

			clearInterval(timer);
			timer = setInterval(animateSlide, countdown);

			return false;
		});

		function animateSlide()
		{
			index++;

			if(index >= counts)
			{
				index = 0;
			}

			$('li', slide_nav).eq(index).addClass('selected').siblings().removeClass('selected');
			slide.eq(index).addClass('selected').siblings().removeClass('selected');
			block.stop(true, true).animate({left: '-' + index * slide_width + 'px'});
		}
	};

	window.UI.scrollTop = function()
	{
		$('body').append('<a class="top">Go top</a>');

		$(window).scroll(function (){
			var scroll = $(this).scrollTop();

			if(scroll > 50)
			{
				$('a.top').stop().animate({bottom: '100px', opacity: 1}, 500);
			}
			else
			{
				$('a.top').stop().animate({bottom: '-56px', opacity: 0}, 500);
			}
		});

		$('a.top').on('click', function (){
			$('html, body').animate({scrollTop: 0}, 500);
		});
	};

	window.UI.footerNav = function()
	{
		$('.footer-wrap .info .control').click(function (){
			$(this).toggleClass('opened');
		});
	};

})(jQuery);