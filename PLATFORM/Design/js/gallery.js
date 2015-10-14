var Gallery = {

    sliderWidth: null,
    sliderCount: null,
    speed: 500,

    init: function () {
        this.sliderWidth = $('.gallery').width();
        this.sliderCount = $('.__slides .list-item').length;

        var navs = '<ul class="list __navs">';

        $('.__slides .list-item').each(function () {
            var li = $(this),
                src = li.find('img').prop('src');

            navs += '<li class="list-item"><img class="list-img" src="' + src +'" alt="" /></li>';
        });

        navs += '</ul>';

        $('.__slides').after(navs);
        $('.__navs .list-item:first').addClass('__selected');

        $('.__slides .list-item').width(this.sliderWidth);
        $('.__slides').width(this.sliderWidth * (this.sliderCount + 2));

        $('.__slides .list-item:first').addClass('__first');
        $('.__slides .list-item:last-child').addClass('__last');

        $('.__first').clone().appendTo('.__slides');
        $('.__last').clone().prependTo('.__slides');

        $('.__slides').css('left', -1 * this.sliderWidth + 'px');

        $('.cnt').addClass('__hidden');

        this.events();
    },

    events: function() {
        $('.ctrl-l').on('click', {pos: 'left'}, this.nextSlide);
        $('.ctrl-r').on('click', {pos: 'right'}, this.nextSlide);

        $(window).off('keypress').on('keypress', {}, this.nextSlide);

        $('.__navs .list-item').on('click', function () {
            var $index = $(this).index() + 1;

            $(this).addClass('__selected').siblings().removeClass('__selected');
            
            $('.__slides').animate({'left': '-' + ($index * Gallery.sliderWidth) + 'px'});
        });
    },

    nextSlide: function(e) {
        var pos = e.data.pos,
            keyCode = e.keyCode;

        $('.popup').select();

        if(pos == 'left' || keyCode == '37') {
            this.index--;

            $('.__slides').stop(true, true);
            $('.__navs .list-item.__selected').removeClass('__selected').prev().addClass('__selected');

            var $newLeft = $('.__slides').position().left + (1 * Gallery.sliderWidth);
            $('.__slides').animate({'left': $newLeft + 'px'}, Gallery.speed, function () {
                if(Math.abs($newLeft) == (0)) {
                    $('.__slides').css({'left': -(Gallery.sliderCount) * Gallery.sliderWidth + 'px'});

                    $('.__navs .list-item').removeClass('__selected').filter(':last-child').addClass('__selected');
                }
            });
        }
        
        if(pos == 'right' || keyCode == '39') {
            this.index++;

            $('.__slides').stop(true, true);

            var $newLeft = $('.__slides').position().left - (1 * Gallery.sliderWidth);
            $('.__navs .list-item.__selected').removeClass('__selected').next().addClass('__selected');

            $('.__slides').animate({'left': $newLeft + 'px'}, Gallery.speed, function () {
                if(Math.abs($newLeft) == ((Gallery.sliderCount + 1) * Gallery.sliderWidth)) {
                    $('.__slides').css({'left': -1 * Gallery.sliderWidth + 'px'});

                    $('.__navs .list-item').removeClass('__selected').filter(':first').addClass('__selected');
                }
            });
        }
    },

    closePopup: function() {
        $('.popup').css({
            'transform-origin': '0 100%',
            'transform': 'scale(0)'
        });

        setTimeout(function () {
            $('.overlay').removeClass('__loaded');
            
            setTimeout(function () {
                $('.overlay').remove();
            }, 500);
        }, 500);

        $('.cnt').removeClass('__hidden');
    }

};