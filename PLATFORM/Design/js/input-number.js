'use strict';
     
$.fn.numberInputPlugin = function() {
     
    var clicked;
     
    clicked = function(e){
        e.preventDefault();
         
        var $self, sum, $block, $val, value;
         
        $self = $(this);
        $block = $self.parents('.js-number-input-plugin');
        $val = $block.find('.js-val');
        value = parseInt($val.val()) || 0;
         
        if(e.data.direction == 'up') {
            // Нажали кнопку вверх
            sum = value + e.data.step;
             
        }
        else if (e.data.direction == 'down') {
            // Нажали кнопку вниз
            sum = value - e.data.step;
             
            if(sum < 0) {
                sum = 0;
            }
        }
         
        $val.val(sum);
    };
     
    return $.each(this, function(){
     
        var $self, step;
         
        $self = $(this);
        step = parseInt($self.data('step')) || 1;
         
        $self.find('.js-up').on('click', {step: step, direction: 'up'}, clicked);
        $self.find('.js-down').on('click', {step: step, direction: 'down'}, clicked);
     
    });
};
     
$(function() {
    $('.js-number-input-plugin').numberInputPlugin();
});