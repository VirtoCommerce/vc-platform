function initItemPage() {
    CloudZoom.quickStart();

    // initialize tabs

    $("#show_reviews_link").on('click', function () {
        //Trigger Review tab click
        $("#product_tabs_reviews").trigger("click");
        //Scroll down to reviews
        $("html, body").animate({ scrollTop: $(".tabs").offset().top });
        return false;
    });
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
                $(".variations").html(data);
            }
        }
    });
}