angular.module('platformWebApp')
.directive('vaContextMenu', [function () {
	return {
		restrict: 'A',
		link: function (scope, iElement, iAttrs) {

			var menuElement = $('#' + iAttrs.vaContextMenu),
				last = null;

			menuElement.css({ 'display': 'none' });
			
			$(iElement).mouseover(function (event) {
				menuElement.css({
					position: "fixed",
					display: "block",
					left: $(iElement).offset().left + $(iElement).width() + 'px',
					top: $(iElement).offset().top + 'px',
				});
			});

			$(iElement).mouseout(function (event)
			{
			    if (menuElement.has(event.relatedTarget).length == 0) {
			        menuElement.css({
			            'display': 'none'
			        });
			    }
			});

			//$(document).click(function (event) {
			//	var target = $(event.target);
			//	if (!target.is(".popover") && !target.parents().is(".popover")) {
			//		if (last === event.timeStamp)
			//			return;
			//		menuElement.css({
			//			'display': 'none'
			//		});
			//	}
			//});

		}
	};


}]);
