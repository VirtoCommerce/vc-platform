angular.module('platformWebApp').directive('vaShowbigimage', function () {
    return {
        scope: {
            image: '='
        },
        link: function (scope, element, attrs) {
            element.bind('mouseenter', function () {
                var el = $(this),
                    elLeft = parseInt(el.offset().left) + 56,
                    elTop  = parseInt(el.offset().top) - 110;

                $('body').prepend('<div class="check-image-size"><img src="' + scope.image + '" alt="" /></div>');
                
                var imgH = $('.check-image-size img').height(),
                    imgW = $('.check-image-size img').width();

                if(imgH >= 300 || imgW >= 300) {
                    $('body').append('<div class="image-preview" style="left: ' + elLeft + 'px; top: ' + elTop + 'px;"><img src="' + scope.image + '"></div>');
                }

                $('.check-image-size').remove();
            });
            element.bind('mouseleave', function () {
                $('.image-preview').remove();
            });
        }
    }
});