(function($) {

    'user strict';

    $('.live-preview').css('height', window.innerHeight - 47);

    $(window).resize(function() {
        $('.live-preview').css('height', window.innerHeight - 47);
    });

    $(window).load(function() {
        //$('.fade-block').remove();
    });

    $(function() {

        //$('body').prepend('<div class="fade-block" style="background: rgba(0, 0, 0, .8); height: 100%; left: 0; position: fixed; right: 0; z-index: 10000;"><img src="responsive/images/preloader.gif" alt="" style="left: 50%; margin: -12px 0 0 -80px; position: absolute; top: 50%;" /></div>');

        $('.top-nav li a').click(function() {
            var self = $(this),
                title = self.data('title');
            type = self.parent().prop('class');

            self.parent().addClass('active').siblings().removeClass('active');
            $('.live-preview').removeAttr('class').addClass('live-preview ' + type);
        });

        $('.sub-menu li a').click(function() {
            var self = $(this),
                size = self.data('size');

            $('.top-nav li.desktop').addClass('active').siblings().removeClass('active');
            self.parents('.sub-menu').slideUp('fast');

            self.parent().addClass('active').siblings().removeClass('active');
            $('.live-preview').removeAttr('class').addClass('live-preview ' + 'desktop-' + size);
        });

        $('.set-url').click(function() {
            var url = prompt('Введите адрес сайта:', '');

            if (url != null) {
                $('.live-preview iframe').prop('src', url);
            }
        });

        $('select[name="change-theme"]').change(function() {
            window.location.replace($(this).val());
        });
    });

})(jQuery);