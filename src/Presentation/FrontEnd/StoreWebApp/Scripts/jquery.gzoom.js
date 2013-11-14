/*!
* jQuery gzoom 0.1
* 2009 Giovanni Battista Lenoci <gianiaz@gianiaz.net>
* 
* Based on minizoomPan plugin of Gian Carlo Mingati Version: 1.0 (18-JUNE-2009) http://www.gcmingati.net/wordpress/wp-content/lab/jquery/minizoompan/
* Inspiration from jquery lightbox plugin http://leandrovieira.com/projects/jquery/lightbox/
* Dual licensed under the MIT and GPL licenses:
* http://www.opensource.org/licenses/mit-license.php
* http://www.gnu.org/licenses/gpl.html
*
* Requires:
* jQuery v1.3.2 or later
* jQuery ui.core.js v.1.7.1
* jQuery ui.slider.js v.1.7.1
* 
*/
var settingsInstanse = null;
var sliderInstanse = null;

jQuery.fn.gzoom = function (settings) {
	settings = jQuery.extend({
		sW: 10, // small image width
		sH: 10, // small image height
		lW: 20, // large image width
		lH: 20, // large image height
		step: 5,
		frameColor: "#cecece",
		frameWidth: 1,
		re: /thumbs\//,
		replace: '',
		debug: false,
		overlayBgColor: '#000',
		overlayOpacity: 0.8,
		containerBorderSize: 10,
		containerResizeSpeed: 400,
		loaderContent: "loading...",  // plain text or an image tag eg.: "<img src='yoursite.com/spinner.gif' />"
		lightbox: false,
		zoomIcon: "" // icon url, if not empty shown on the right-top corner of the image
	}, settings);

	settingsInstanse = settings;

	return this.each(function () {
		var swapped = false;

		var curX = null;
		var curY = null;

		var $div = jQuery(this).css({ width: settingsInstanse.sW, height: settingsInstanse.sH, border: settingsInstanse.frameWidth + "px solid " + settingsInstanse.frameColor, overflow: 'hidden' }).addClass("minizoompan");
		$div.wrap('<div class="gzoomwrap"></div>').css({ width: settingsInstanse.sW });
		var ig = $div.children().css({ position: "relative" });

		var activeEvent = false;

		jQuery(window).bind("load", function () {
			ig.width(settingsInstanse.sW);
			ig.height(settingsInstanse.sH);
		});


		//jQuery("<span class='loader'>" + settingsInstanse.loaderContent + "<\/span>").insertBefore(ig);
		if (settingsInstanse.zoomIcon != '' && settingsInstanse.lightbox) {
			var $zoom = jQuery('<img class="zoomIcon" src="' + settingsInstanse.zoomIcon + '" alt="view larger" />').insertBefore(ig);
			$zoom.load(function () {
				$zoom.css({ 'left': (settingsInstanse.sW - $zoom.width()) + 'px' }).show();
			});
			$zoom.dblclick(function () {
				drawIface();
			});
		} else if (settingsInstanse.lightbox) {
			$div.dblclick(function () {
				drawIface();
			});
			$div.css({ 'cursor': 'pointer' });
		}

		var $plus = jQuery('<div class="ui-icon ui-icon-circle-plus gzoombutton">&nbsp;</div>').insertAfter($div);
		var $slider = jQuery('<div class="gzoomSlider"></div>').insertAfter($div).css({ width: (settingsInstanse.sW - 42) + 'px' });
		sliderInstanse = $slider;
		var $minus = jQuery('<div class="ui-icon ui-icon-circle-minus gzoombutton">&nbsp;</div>').insertAfter($div);

		$plus.click(function () {
			valore = parseInt($slider.slider('value')) + settingsInstanse.step;
			$slider.slider('value', valore);
		});

		$minus.click(function () {
			valore = parseInt($slider.slider('value')) - settingsInstanse.step;
			$slider.slider('value', valore);
		});

		$slider.slider({ value: 0,
			min: 0,
			max: 100,
			step: settingsInstanse.step,
			change: function (event, ui) {

				if (!swapped) {
					var hiSrc = ig.attr("src").replace(settingsInstanse.re, settingsInstanse.replace);
					swapImage(ig, hiSrc);
					$div.children("span.loader").fadeIn(250);
					swapped = true;
				}

				val = ui.value;

				newWidth = settingsInstanse.sW + ((settingsInstanse.lW - settingsInstanse.sW) * val) / 100;
				newHeight = settingsInstanse.sH + ((settingsInstanse.lH - settingsInstanse.sH) * val) / 100;
				ig_pos = ig.position();

				if (settingsInstanse.debug && typeof (console) != 'undefined') {
					console.log('new dimensions:' + newWidth + 'x' + newHeight);
				}

				deltaWidth = ig.width() - settingsInstanse.sW;
				leftPos = Math.abs(ig_pos.left);
				leftFactor = leftPos / deltaWidth;
				newDeltaWidth = newWidth - settingsInstanse.sW;
				newLeft = (leftFactor * newDeltaWidth) * -1;

				deltaHeight = ig.height() - settingsInstanse.sH;
				topPos = Math.abs(ig_pos.top);
				topFactor = topPos / deltaHeight;
				newDeltaHeight = newHeight - settingsInstanse.sH;
				newTop = (topFactor * newDeltaHeight) * -1;
				ig.css({ width: newWidth, height: newHeight, left: newLeft, top: newTop });
			}
		});
		$('<br style="clear:both" />').insertAfter($plus);

		$slider.slider('value', 0);

		function swapImage(param, uri) {
			param.load(function () {
				$div.children("span.loader").fadeOut(250);
			}).error(function () {
				alert("Image does not exist or its SRC is not correct.");
			}).attr('src', uri);
		}

		$div.mousedown(function (e) {
			activeEvent = true;

			if (document.all) {
				window.event.cancelBubble = true;
				e.originalEvent.cancelBubble = true;
				return false;
			}

			curX = e.clientX;
			curY = e.clientY;
			e.stopPropagation();
			return false;
		});

		$div.mouseup(function (e) {
			activeEvent = false;

			if (document.all) {
				window.event.cancelBubble = true;
				e.originalEvent.cancelBubble = true;
				return false;
			}

			e.stopPropagation();
			return false;
		});

		$div.mousemove(function (e) {
			if (activeEvent) {

				var dY = curY - e.clientY;
				var dX = curX - e.clientX;

				var newLeft = ($('#imageContainer img.primaryimage')[0].style.left == '' ? 0 : parseInt($('#imageContainer img.primaryimage')[0].style.left)) - dX;
				var newTop = ($('#imageContainer img.primaryimage')[0].style.top == '' ? 0 : parseInt($('#imageContainer img.primaryimage')[0].style.top)) - dY;

				if (parseInt($('#imageContainer img.primaryimage')[0].style.width) < settingsInstanse.sW + Math.abs(newLeft) || newLeft > 0)
					newLeft = $('#imageContainer img.primaryimage')[0].style.left;

				if (parseInt($('#imageContainer img.primaryimage')[0].style.height) < settingsInstanse.sH + Math.abs(newTop) || newTop > 0)
					newTop = $('#imageContainer img.primaryimage')[0].style.top;

				ig.css({ left: newLeft, top: newTop });
				//console.log('dx=' + dX + ' dy=' + dY, ' newLeft=' + newLeft + ' newTop=' + newTop);

				curX = e.clientX;
				curY = e.clientY;

				if (document.all) {
					window.event.cancelBubble = true;
					e.originalEvent.cancelBubble = true;
					return false;
				}

			}
		});

		if (typeof ($.event.special.mousewheel) != 'undefined') {
			ig.mousewheel(function (event, delta) {
				if (delta > 0) {
					valore = parseInt($slider.slider('value')) + settingsInstanse.step;
					$slider.slider('value', valore);
				} else if (delta < 0) {
					valore = parseInt($slider.slider('value')) - settingsInstanse.step;
					$slider.slider('value', valore);
				}
				return false; // prevent default
			});
		}

		function resize_fx(intImageWidth, intImageHeight) {

			if (settingsInstanse.debug && typeof (console) != 'undefined') {
				console.log('resize_fx(' + intImageWidth + ',' + intImageHeight + ')');
			}

			if (intImageWidth > ($(window).width() * 80 / 100)) {
				imgWidth = $(window).width() * 80 / 100;
				intImageHeight = (imgWidth / intImageWidth) * intImageHeight;
				$('#zoomedimage').css({ 'width': imgWidth + 'px', 'height': intImageHeight });
				if (settingsInstanse.debug && typeof (console) != 'undefined') {
					console.log('Img dimensions 80% horizontal:' + imgWidth + 'x' + intImageHeight);
				}
			} else {
				imgWidth = intImageWidth;
			}

			if (intImageHeight > $(window).height() * 80 / 100) {
				imgHeight = $(window).height() * 80 / 100;
				imgWidth = (imgHeight / intImageHeight) * imgWidth;
				$('#zoomedimage').css({ 'width': imgWidth + 'px', 'height': imgHeight });
				if (settingsInstanse.debug && typeof (console) != 'undefined') {
					console.log('Img dimensions 80% vertical:' + imgWidth + 'x' + imgHeight);
				}
			} else {
				imgHeight = intImageHeight;
			}

			if (settingsInstanse.debug && typeof (console) != 'undefined') {
				console.log('Img dimensions:' + imgWidth + 'x' + imgHeight);
			}

			// Get current width and height
			var intCurrentWidth = $('#imagebox').width();
			var intCurrentHeight = $('#imagebox').height();
			// Get the width and height of the selected image plus the padding
			var intWidth = (imgWidth + (settingsInstanse.containerBorderSize * 2)); // Plus the imageЕЅs width and the left and right padding value
			var intHeight = (imgHeight + (settingsInstanse.containerBorderSize * 2)); // Plus the imageЕЅs height and the left and right padding value



			// Diferences
			var intDiffW = intCurrentWidth - intWidth;
			var intDiffH = intCurrentHeight - intHeight;
			// Perfomance the effect
			$('#imagebox').animate({ width: intWidth, height: intHeight }, settingsInstanse.containerResizeSpeed, function () { _show_image(); });
			if ((intDiffW == 0) && (intDiffH == 0)) {
				if ($.browser.msie) {
					___pause(250);
				} else {
					___pause(100);
				}
			}
			$('#lboximgdatacontainer').css({ width: imgWidth });
		};

		function drawIface() {

			$('body').append('<div id="gzoomoverlay"></div><div id="gzoomlbox"><div id="imagebox"><div id="gzoom-cont-img"><img id="zoomedimage"><div id="gzoomloading"><a href="#" id="gzoomloadinglink"><img src="images/loading.gif"></a></div></div></div><div id="gzoomlbox-con-imgdata"><div id="lboximgdatacontainer"><div id="gzoomlbox-image-details"><span id="gzoom-image-caption"></span></div></div></div></div>');

			$('#gzoomoverlay').css({
				backgroundColor: settingsInstanse.overlayBgColor,
				opacity: settingsInstanse.overlayOpacity,
				width: $(window).width(),
				height: $(document).height()
			}).fadeIn();

			// Calculate top and left offset for the gzoomlbox div object and show it
			$('#gzoomlbox').css({
				top: $(window).scrollTop() + ($(window).height() / 10),
				left: $(window).scrollLeft()
			}).show();

			$('#gzoomoverlay,#gzoomlbox').click(function () {
				close();
			});
			// If window was resized, calculate the new overlay dimensions
			$(window).resize(function () {
				$('#gzoomoverlay').css({
					width: $(window).width(),
					height: $(window).height()
				});
				// Calculate top and left offset for the jquery-lightbox div object and show it
				$('#gzoomlbox').css({
					top: $(window).scrollTop() + ($(window).height() / 10),
					left: $(window).scrollLeft()
				});
			});

			_set_image_to_view();
		}

		function _set_image_to_view() { // show the loading
			// Show the loading
			$('#gzoomlbox-con-imgdata').hide();
			$('#zoomedimage').hide();
			$('#gzoomloading').show();
			// Image preload process
			var objImagePreloader = new Image();
			if (!swapped) {
				var hiSrc = ig.attr("src").replace(settingsInstanse.re, settingsInstanse.replace);
			} else {
				var hiSrc = ig.attr("src").replace(settingsInstanse.re, settingsInstanse.replace);
			}
			objImagePreloader.onload = function () {
				$('#zoomedimage').attr('src', hiSrc);
				resize_fx(objImagePreloader.width, objImagePreloader.height);
			};
			objImagePreloader.src = hiSrc;
		};

		function _show_image() {
			$('#gzoomloading').hide();
			$('#zoomedimage').fadeIn(function () {
				_show_image_data();
			});
		};

		/**
		* Show the image information
		*
		*/
		function _show_image_data() {
			$('#lightbox-loading').hide();
			$('#gzoom-image-caption').html(ig.attr('title'));
			$('#gzoomlbox-con-imgdata').slideDown('fast');
		}

		function close() {
			$('#gzoomlbox').remove();
			$('#gzoomoverlay').fadeOut(function () { $('#gzoomoverlay').remove(); });
			$('embed, object, select').css({ 'visibility': 'visible' });
		}

		function ___pause(ms) {
			var date = new Date();
			curDate = null;
			do { var curDate = new Date(); }
			while (curDate - date < ms);
		};

	});

};

jQuery.fn.gzoom.setSettings = function (settings) {
	settingsInstanse.sH = settings.sH;
	settingsInstanse.lW = settings.lW;
	settingsInstanse.lH = settings.lH;
}