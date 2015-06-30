angular.module('platformWebApp').directive('vaShowbigimage', function () {
    return {
        scope: {
        	vaShowbigimage: '='
        },
        link: function (scope, element, attrs) {
            element.bind('mouseenter', function () {
                var el = $(this),
                    elLeft = parseInt(el.offset().left) + 56,
                    elTop  = parseInt(el.offset().top) - 110;

                $('body').prepend('<div class="check-image-size"><img src="' + scope.vaShowbigimage + '" alt="" /></div>');
                
                var imgH = $('.check-image-size img').height(),
                    imgW = $('.check-image-size img').width();

                if(imgH >= 300 || imgW >= 300) {
                	$('body').append('<div class="image-preview" style="left: ' + elLeft + 'px; top: ' + elTop + 'px;"><img src="' + scope.vaShowbigimage + '"></div>');
                }

                $('.check-image-size').remove();
            });
            element.bind('mouseleave', function () {
                $('.image-preview').remove();
            });
        }
    }
});