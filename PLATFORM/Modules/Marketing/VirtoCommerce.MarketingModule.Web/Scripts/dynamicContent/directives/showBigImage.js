angular.module('platformWebApp').directive('ngShowbigimage', function () {
	//return function (scope, element, attrs) {
	//	var model = $parse(attrs.ngShowBigImage);
	//	console.log(model(scope));  // logs "test"
	//	//element.bind('click', function () {
	//	//	model.assign(scope, "Button 1");
	//	//	scope.$apply();
	//	//});
	//};

	return {
		scope: {
			image: '='
		},
		link: function (scope, element, attrs) {
			element.bind('mouseenter', function () {
				$('body').prepend('<div class="overlay"><div class="overlay-cnt"></div></div>');
				$('.overlay-cnt').html('<div class="popup"><div class="popup-cnt"><div class="image"><img src="' + scope.image + '"></div></div></div>');

				$('.overlay').off('click').on('click', function () {
					$('.overlay').remove();
				});
			});
		}
	}
});