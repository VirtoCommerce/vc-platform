String.prototype.Localize = function (selector, category, culture)
{
    if (selector == null || selector == 'undefined')
        return;
    var url = "/settings/localize?text=" + this.toString();

    if (category != null && category != 'undefined')
    {
        url = url + "&category=" + category;
    }

    if (culture != null && culture != 'undefined')
    {
        url = url + "&culture=" + culture;
    }

    $.ajax({
        type: "GET",
        url: VirtoCommerce.url(url),
        dataType: "json",
        success: function (result)
        {
            if (typeof selector == 'function')
            {
                selector(result.translation);
            } else
            {
                $(selector).html(result.translation);
            }
        }
    });

};

if (typeof String.prototype.trim !== 'function')
{
    String.prototype.trim = function ()
    {
        return this.replace(/^\s+|\s+$/g, '');
    };
}


VirtoCommerce = function ()
{
    this.Stores = [];
    this.DynamicContent = {};
};
VirtoCommerce.prototype.constructor = VirtoCommerce.prototype;

VirtoCommerce.prototype = {
    initialize: function ()
    {
    },

    changeStore: function (id)
    {
        $.each(this.Stores, function (index, obj)
        {
            if (obj.Id == id)
            {
                window.location.href = obj.Url;
            }
        });
    },

    changeCurrency: function (id)
    {
        $.redirect(location.href, { currency: id });
    },

    updateQuickLinks: function ()
    {
        $.ajax({
            type: "GET",
            dataType: "html",
            url: VirtoCommerce.url('/store/quickaccess', true),
            success: function (data)
            {
                $("ul.head-links").parent().html(data);
            }
        });
    },


    initCameraSliders: function (selector, options)
    {

        if (selector == undefined)
        {
            selector = "";
        }

        selector = (selector + " .camera-slides").trim();

        options = jQuery.extend({
            loader: 'bar',
            playPause: false,
            barPosition: 'top',
            loaderColor: '#322C29',
            loaderBgColor: 'rgba(0, 0, 0, 0)',
            height: '25%'
        }, options);

        if ($(selector).length > 0)
        {
            $(selector).camera(options);
        }
    },

    initHtmlSliders: function (selector, options)
    {

        if (selector == undefined)
        {
            selector = "";
        }

        selector = (selector + " .html-slides").trim();

        if ($(selector).length > 0)
        {
            $(selector).mainSlider(options);
        }
    },

    initSliders: function ()
    {
        this.initCameraSliders();
        this.initHtmlSliders();
    },

    //Register dynamic content
    //banner: place name
    //selector: jquery selector or callback function
    registerDynamicContent: function (banner, selector)
    {
        this.DynamicContent[banner] = selector;
    },

    //This method must be called after all registrations are done. Preferably at the end of base layout
    renderDynamicContent: function ()
    {
        var url = VirtoCommerce.url('/banner/showdynamiccontents');
        var i = 0;
        for (var placeName in this.DynamicContent)
        {
            if (i == 0)
            {
                url = url + '?';
            } else
            {
                url = url + '&';
            }
            url = url + 'placeName=' + placeName;
            i = i + 1;
        }
        if (i > 0)
        {
            $.ajax({
                type: "GET",
                dataType: "html",
                url: url,
                success: function (data)
                {
                    var htmlData = $('<div/>').html(data);

                    for (var key in VirtoCommerce.DynamicContent)
                    {
                        var selector = VirtoCommerce.DynamicContent[key];
                        var bannerContent = htmlData.find('#' + key).html();
                        if (typeof selector == 'function')
                        {
                            selector(bannerContent);
                        } else
                        {
                            $(selector).html(bannerContent);
                        }
                    }
                }
            });
        }
    },

    updatePrices: function (items)
    {

        if (items == undefined)
            return;

        var url = VirtoCommerce.url('/search/prices');

        var postData = {};
        var i = 0;
        for (var key in items)
        {
            postData['itemAndOutine[' + i + '].Key'] = key;
            postData['itemAndOutine[' + i + '].Value'] = items[key];
            i = i + 1;
        }

        $.ajax({
            type: "POST",
            dataType: "html",
            url: url,
            data: postData,
            success: function (data)
            {
                var htmlData = $('<div/>').html(data);

                for (var itemId in items)
                {
                    var selector = "[id='" + itemId + "'] div.price";
                    var priceContent = htmlData.find(selector).html();
                    $(selector).html(priceContent);
                    priceContent = $('<div/>').html(priceContent);
                    var oldPrice = $(priceContent).find("span.old");
                    if (oldPrice.length != 0)
                    {
                        $('li#' + itemId).addClass('sale');
                    } else
                    {
                        $('li#' + itemId).removeClass('sale');
                    }
                }
            }
        });
    },

    ///GLOBAL METHODS

    //Input: form object
    //Method serlizes form fields into javascript object
    deserializeForm: function (form)
    {
        var data = {};
        var serialized = form.serializeArray();
        // turn the array of form properties into a regular JavaScript object
        for (var i = 0; i < serialized.length; i++)
        {
            data[serialized[i].name] = serialized[i].value;
        }
        return data;
    },

    //Method tries to create errors object from dictionary [{Key: fieldName, Value: errorMessage}] and shows errors using given validator
    extractErrors: function (jqXhr, validator)
    {

        try
        {
            var data = JSON.parse(jqXhr.responseText); // parse the response into a JavaScript object

            if (data.length != undefined && data.length > 0)
            {

                var errors = {};

                for (var i = 0; i < data.length; i++)
                { // add each error to the errors object
                    errors[data[i].key] = data[i].errors[0];
                }

                validator.showErrors(errors); // show the errors using the validator object
            }
            else if (data.Message != undefined)
            {
                $.showPopupMessage(data.Message);
            }
        }
        catch (ex)
        {
            alert(jqXhr.responseText);
        }
    },

    disableAll: function (selector)
    {
        selector.find('input').attr('disabled', 'disabled');
        selector.find('select').attr('disabled', 'disabled');
        selector.find('textarea').attr('disabled', 'disabled');
    },

    enableAll: function (selector)
    {
        selector.find('input').removeAttr('disabled');
        selector.find('select').removeAttr('disabled');
        selector.find('textarea').removeAttr('disabled');
    },

    url: function (url, force)
    {
        var baseHref = $('base').attr('href');
        if (url.indexOf(baseHref) == 0 && (force == undefined || force == false))
        {
            return url;
        }
        else
        {
            return baseHref + url.substr(1);
        }
    },
    processMessages: function (context)
    {
        //Show success message
        if (context != null && context.Messages != undefined && context.Messages.length > 0)
        {
            var joiner = [];
            for (var x = 0; x < context.Messages.length; x++)
            {
                joiner.push(context.Messages[x].Text);
            }
            $.showPopupMessage(joiner.join("<br/>"));
        }
    },
    setCookie: function (cname, cvalue, exdays)
    {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toGMTString();
        var path = "path=/";
        document.cookie = cname + "=" + cvalue + "; " + expires + "; " + path;
    },

    getCookie: function (cname)
    {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++)
        {
            var c = ca[i].trim();
            if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
        }
        return "";
    }
};
var VirtoCommerce = new VirtoCommerce();

VirtoCart = function ()
{
    this.ShoppingCartName = 'ShoppingCart';
    this.WishListName = 'WishList';
    this.CompareListName = 'CompareList';
};
VirtoCart.prototype.constructor = VirtoCart.prototype;

VirtoCart.prototype = {
    initialize: function ()
    {
    },

    onUpdate: function (context)
    {
        if (context.CartCount == 0 && context.Source == "LineItems")
        {
            location.reload();
            return;
        }

        // Update the page elements
        if (context.LineItemsView != null && context.LineItemsView.length > 0)
        {
            if (context.Source.toLowerCase() == "MiniCart".toLowerCase())
            {
                if ($(".cart .popup").length > 0)
                {
                    $(".cart .popup").html(context.LineItemsView);
                }
                $("[id=cart-count]").html(context.CartCount);
                $("[id=cart-subtotal]").html(context.CartSubTotal);
                $("[id=cart-total]").html(context.CartTotal);
            }
            else if (context.Source.toLowerCase() == "MiniCompareList".toLowerCase())
            {
                $('.compare .popup').html(context.LineItemsView);
            }
            else if (context.Source.toLowerCase() == "LineItems".toLowerCase())
            {
                $('#shopping-cart-table tbody').html(context.LineItemsView);
                $("[id=cart-count]").html(context.CartCount);
                $("[id=cart-subtotal]").html(context.CartSubTotal);
                $("[id=cart-total]").html(context.CartTotal);
            }
        } else
        {
            $('#row-' + context.DeleteId).fadeOut('slow');
        }
        VirtoCommerce.updateQuickLinks();
        VirtoCommerce.processMessages(context);
    },

    //name, title, itemId, parentItemId, quantity, relatedItems
    add: function (options)
    {

        if (options.quantity == undefined)
        {
            options.quantity = 1;
        }

        //Try to get variation
        try
        {
            var variation = this.getVariation();
            if (variation != undefined && variation != "")
            {
                options.itemId = variation;
            }
        }
        catch (e)
        {
            //Variation is invalid
            return false;
        }

        var url = VirtoCommerce.url('/Cart/Add') + '?name=' + options.name + '&itemId=' + options.itemId + '&quantity=' + options.quantity;

        if (options.parentItemId != undefined && options.parentItemId.length > 0)
        {
            url = url + '&parentItemId=' + options.parentItemId;
        }

        if (options.name == this.ShoppingCartName)
        {
            if (options.relatedItems != undefined && options.relatedItems.length > 0)
            {
                for (var i = 0; i < options.relatedItems.length; i++)
                {
                    url = url + '&relatedItemId=' + options.relatedItems[i];
                }
            }
        }

        $.ajaxq("addtocart", {
            type: 'POST',
            url: url,
            cache: false,
            success: function (context)
            {
                if (typeof options.callback == 'function')
                {
                    options.callback(context);
                }

                VirtoCart.updateMiniCart(context.CartName);
                if (context.CartName == VirtoCart.ShoppingCartName)
                {
                    $('#cart-count').html(context.CartCount);
                }
                if (context.CartName != VirtoCart.CompareListName)
                {
                    VirtoCommerce.updateQuickLinks();
                }
                //Show success message
                VirtoCommerce.processMessages(context);

            }
        });

        return false;
    },

    addToCart: function (options)
    {
        options.name = this.ShoppingCartName;
        return this.add(options);
    },

    addToCartOrdersClick: function ()
    {
        var relatedItems = this.collectRelatedItems('order_items[]');

        if (relatedItems.length == 0)
            return false;
        return VirtoCart.addToCart({ relatedItems: relatedItems });
    },

    addToCartClick: function (sender, options)
    {
        if (options.quantitySelector == undefined)
        {
            options.quantitySelector = '#qty';
        }
        var qty = $(sender).closest("form").find(options.quantitySelector);

        if (qty.length != 0 && !qty.valid())
        {
            return false;
        } else
        {
            options.relatedItems = this.collectRelatedItems('related_products[]');
            options.quantity = qty.length != 0 ? qty.val() : 1;

            return VirtoCart.addToCart(options);
        }
    },

    addToWishList: function (options)
    {
        options.name = this.WishListName;
        return this.add(options);
    },

    addToCompareList: function (options)
    {
        options.name = this.CompareListName;
        return this.add(options);
    },

    checkout: function ()
    {
        window.location = VirtoCommerce.url('/checkout');
    },

    getVariation: function ()
    {

        if ($("#SelectedVariationId").length != 0)
        {

            var isValid = true;
            $.each($("div.variations select"), function ()
            {
                isValid = $(this).valid() && isValid;
            });

            if (!isValid)
                throw "You must select product first! Select property for each category";

            if ($("#SelectedVariationId").val().length != 0)
            {
                return $("#SelectedVariationId").val();
            }
            else
            {
                throw "You must select product first! Select property for each category";
            }
        }
        return null;
    },

    updateMiniCart: function (name)
    {
        var placeholder = $(".cart .popup");
        var urlForData = '/Cart/MiniView';

        if (name == this.CompareListName)
        {
            placeholder = $(".compare .popup");
            urlForData = '/Account/MiniCompareList';
        }

        $.get(VirtoCommerce.url(urlForData), {},
			function (data)
			{
			    // if at least one item in cart exists
			    if (placeholder)
			    {
			        placeholder.html(data);
			    } else
			    {
			        // if cart is empty
			    }
			}, "html");
    },

    updateCartOptions: function ()
    {
        $.ajax({
            type: "GET",
            dataType: "html",
            url: VirtoCommerce.url('/store/cartoptions', true),
            success: function (data)
            {
                $("div.cart-options").html(data);
            }
        });
    },

    updateCoupon: function (code, renderItems, onSuccess)
    {

        $.ajax({
            type: 'POST',
            url: VirtoCommerce.url('/cart/applyCoupon'),
            data: { couponCode: code, renderItems: renderItems },
            dataType: 'JSON',
            success: function (context)
            {
                onSuccess(context);
            },
            error: function (jqXhr)
            {
                VirtoCommerce.extractErrors(jqXhr, validator);
            }
        });
    },

    submitEstimateShipping: function (form)
    {

        form.resetValidation();
        var validator = form.validate();
        if (form.valid())
        {
            //data = VirtoCommerce.deserializeForm(form);
            var data = $(form).serializeObject().ShippingEstimateModel;
            $.ajax({
                type: 'POST',
                url: VirtoCommerce.url('/api/cart/estimatepost'),
                data: data,
                dataType: 'JSON',
                headers: {'forcedwebapi':true},
                success: function (bestShipment)
                {
                    $("#shippingRow").fadeIn(1000);
                    $("#cart-shipping").html("(" + bestShipment.DisplayName + ") " + bestShipment.PriceFormatted);
                    $("#cart-total").html(bestShipment.TotalCartPriceFormatted);
                },
                error: function (jqXhr)
                {
                    VirtoCommerce.extractErrors(jqXhr, validator);
                }
            });
        }
    },
    collectRelatedItems: function (checkBoxName)
    {
        var relatedItems = new Array();
        $.each($("input[name='" + checkBoxName + "']:checked"), function ()
        {
            relatedItems.push($(this).val());
        });
        return relatedItems;
    },
};
var VirtoCart = new VirtoCart();

////////////////////// ADDRESS CLASS //////////////////////

VirtoAddress = function (id)
{
    this.initialize(id);
};
VirtoAddress.prototype.constructor = VirtoAddress.prototype;

VirtoAddress.prototype = {
    initialize: function (id)
    {
        this.Id = id;
        var address = this;
        var dropdown, dropdown2, textbox, countryName;
        if (this.Id)
        {
            dropdown = '#' + this.Id + '_Address_CountryCode';
            dropdown2 = '#' + this.Id + '_StateProvinceId';
            textbox = '#' + this.Id + '_Address_StateProvince';
            countryName = '#' + this.Id + '_Address_CountryName';
        }
        else
        {
            dropdown = '#Address_CountryCode';
            dropdown2 = '#Address_StateProvinceId';
            textbox = '#Address_StateProvince';
            countryName = '#Address_CountryName';
        }

        updateSelections();

        $(dropdown).change(function ()
        {
            updateSelections();
        });

        $(dropdown2).change(function ()
        {
            var stateId = $(dropdown2).val();
            $(textbox).val(stateId);
        });

        function updateSelections()
        {
            var countryId = $(dropdown).val();

            if ($(countryName).length > 0)
            {
                $(countryName).val($(dropdown + " option:selected").text());
            }

            address.updateRegions(countryId);
        }

        if ($('#geocomplete-' + this.Id).length != 0)
        {

            $('#geocomplete-' + this.Id).geocomplete({
                details: "#geo-details-" + this.Id,
                detailsAttribute: "data-geo"
            }).bind("geocode:result", function (event, result)
            {
                var addressArray = result.address_components;
                var street = "";
                var countryCode = "";
                var stateId = "";
                for (var j = 0; j < addressArray.length; j++)
                {
                    var typeName = addressArray[j].types[0];
                    if (typeName == 'street_number')
                    {
                        street += addressArray[j].short_name + ' ';
                    }
                    if (typeName == 'route')
                    {
                        street += addressArray[j].short_name;
                    }
                    if (typeName == 'country')
                    {
                        countryCode += addressArray[j].short_name;
                    }
                    if (typeName == 'administrative_area_level_1')
                    {
                        stateId += addressArray[j].short_name;
                    }
                }
                $('#geo-details-' + address.Id + ' [data-geo="route"]').val(street);

                countryCode = window.v_codes[countryCode];
                if (countryCode.length > 0)
                {
                    $(dropdown).val(countryCode);
                    address.updateRegions(countryCode);
                }
                if (stateId.length > 0)
                {
                    $(dropdown2).val(stateId);
                    $(textbox).val(stateId);
                }
            });
        }
    },

    updateRegions: function (countryId)
    {
        var dropdown2, textbox;

        if (this.Id)
        {
            dropdown2 = '#' + this.Id + '_StateProvinceId';
            textbox = '#' + this.Id + '_Address_StateProvince';
        }
        else
        {
            dropdown2 = '#Address_StateProvinceId';
            textbox = '#Address_StateProvince';
        }

        $.each(window.v_regions, function (index, country)
        {
            if (country.CountryId == countryId)
            {
                if (country.Regions.length > 0)
                {
                    $(textbox).hide();
                    $(dropdown2).show();
                    $(textbox).val("");
                    $(dropdown2).html("");
                    "select state/province".Localize(function (translation)
                    {
                        $(dropdown2).prepend($("<option></option>").val("").html(translation));
                    });

                    $.each(country.Regions, function (index2, region)
                    {
                        $(dropdown2).append($("<option></option>").val(region.RegionId).html(region.Name));
                    });
                }
                else
                {
                    $(dropdown2).hide();
                    $(textbox).show();
                }
                return;
            }
        }
        );
    }
};
////////////////////Global Functions/////////////////////////////


function bindDropDownList(e, targetDropDownList)
{
    var key = this.value;
    var allOptions = targetDropDownList.allOptions;
    var option;
    var newOption;
    targetDropDownList.options.length = 0;

    for (var i = 0; i < allOptions.length; i++)
    {
        option = allOptions[i];
        if (option.key == key)
        {
            newOption = new Option(option.text, option.value);
            targetDropDownList.options.add(newOption);
        }
    }
}

function initQtySpinner(id, min, max)
{
    return $(id).spinner({ min: min, max: max, numberFormat: 'n0' });
}