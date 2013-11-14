function initItemPage() {
    initJqZoom();

    // initialize tabs
    $("#product_tabs_description").on('click', function () {
        $('.product-tabs-content').hide();
        $('.product-tabs li').removeClass('active');
        $(this).addClass('active');
        $('#product_tabs_description_contents').show();
    });
    $("#product_tabs_upsell_products").on('click', function () {
        $('.product-tabs-content').hide();
        $('.product-tabs li').removeClass('active');
        $(this).addClass('active');
        $('#product_tabs_upsell_products_contents').show();
    });
    $("#product_tabs_additional").on('click', function () {
        $('.product-tabs-content').hide();
        $('.product-tabs li').removeClass('active');
        $(this).addClass('active');
        $('#product_tabs_additional_contents').show();
    });
    $("#product_tabs_product_tags").on('click', function () {
        $('.product-tabs-content').hide();
        $('.product-tabs li').removeClass('active');
        $(this).addClass('active');
        $('#product_tabs_product_tags_contents').show();
    });
    $("#product_tabs_reviews").on('click', function () {
        $('.product-tabs-content').hide();
        $('.product-tabs li').removeClass('active');
        $(this).addClass('active');
        $('#product_tabs_reviews_contents').show();
    });

    $("#show_reviews_link").on('click', function () {
        //Trigger Review tab click
        $("#product_tabs_reviews").trigger("click");
        //Scroll down to reviews
        $("html, body").animate({ scrollTop: $(".product-collateral").offset().top });
        return false;
    });
}

function initJqZoom() {
    window.setTimeout(function () {
        var nh = $('#imageContainer img.primaryimage')[0].offsetHeight;
        $('#imageContainer').css("height", nh);

        $('.jqzoom').jqzoom({
            zoomType: 'standard',
            lens: true,
            preloadImages: false,
            alwaysOn: false,
            zoomWidth: nh,
            zoomHeight: nh
            //showEffect: 'fadein',
            //hideEffect: 'fadeout'
        });
    }, 1000);
}

function imageChanged() {

    window.setTimeout(function () {
        var nh = $('#imageContainer img.primaryimage')[0].offsetHeight;
        $('#imageContainer').css("height", nh);

    }, 10);
}


//this is the catch-all for error messages
function errorMsg(xhr, ajaxOptions, thrownError) {
    alert(xhr.statusText);
    alert(thrownError);

    'Error! Something went wrong. Please try again.'.Localize(function (translation)
    {
    	alert(translation);
    });
}

function loadVariations(parentItemId, variationId, dropdown) {

    var url = VirtoCommerce.url("/Catalog/ItemVariations") + "?itemId=" + parentItemId + "&name=itemvariations";

    if (variationId != undefined && variationId.length > 0) {
        url += "&variationId=" + variationId;
    }

    $.each($("div.variations select"), function () {
        var selectedValue = $(this).val();
        selectedValue = this.id + ":" + selectedValue;

        //mark as trigger
        if (this.id == dropdown.id) {
            selectedValue = selectedValue + ":t";
        }

        var keyValue = selectedValue.split(":");

        //If dropdown explicitly set to see all items
        if (keyValue[1].length == 0 && this.id == dropdown.id) {
            //null means show all
            selectedValue = keyValue[0] + ":null";
        }
        url = url + '&selections=' + selectedValue;

    });

    $.ajax({
        type: 'GET',
        url: url,
        dataType: "html",
        success: function (data) {
            if (data != undefined && data.length > 0) {
                $("#item-variations").html(data);
            }
        }
    });
}