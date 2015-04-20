angular.module('platformWebApp').directive('vaShowbigimage', function () {
	return {
		scope: {
			image: '='
		},
		link: function (scope, element, attrs) {
			element.bind('mouseenter', function () {
				var self = $(this);
				self.after('<div class="image-preview"><img src="' + scope.image + '"></div>');
			});
			element.bind('mouseleave', function () {
				var self = $(this);
				self.next().remove();
			});
		}
	}
});