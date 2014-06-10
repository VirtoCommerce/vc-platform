function initItemPage() {
    window.CloudZoom.quickStart();

    // initialize tabs
    window.setTimeout(function ()
    {
        if($(location.hash).length > 0)
            scrollToTab(location.hash);
    }, 100);


    $("#show_reviews_link").on('click', function () {
        return scrollToTab('#product_tabs_reviews');
    });
    
 
    function scrollToTab(id)
    {
        //Trigger tab click
        $(id).trigger("click");
        //Scroll down to reviews
        $("html, body").animate({ scrollTop: $(".tabs").offset().top });
        return false;
    }
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

function loadVariations(parentItemId, variation, dropdown)
{

    var url = VirtoCommerce.url("/catalog/itemvariations") + "?itemId=" + parentItemId + "&name=itemvariations";

    if (variation != undefined && variation.length > 0)
    {
        url += "&variation=" + variation;
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
        url = url + '&selections=' + encodeURIComponent(selectedValue);

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