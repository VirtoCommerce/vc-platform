String.prototype.Localize = function (selector, category, culture)
{
	if (selector == null || selector == 'undefined')
		return;
	var url = "/Settings/Localize?text=" + this.toString();

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


VirtoCommerce = function ()
{
	this.Stores = [];
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
				$("div.quick-access-bar").html(data);
			}
		});
	},

	hideDemoNotice: function ()
	{
		$.post(VirtoCommerce.url('/settings/hidedemonotice'));
		$('.demo-notice-container').fadeOut(function () { $(this).remove(); });
	},

	///GLOBAL METHODS

	//Input: form object
	//Method serlizes form fields into javascript object
	deserializeForm: function (form)
	{
		data = {};
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
					errors[data[i].Key] = data[i].Value;
				}

				validator.showErrors(errors); // show the errors using the validator object
			}
			else
			{
				alert(data.Message);
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
		if (context.CartCount == 0 && context.DeleteSource == "LineItems")
		{
			location.reload();
			return;
		}
		
		// Update the page elements
		if (context.LineItemsView != null && context.LineItemsView.length > 0) {

		    if ($("#shopping-cart-table").length != 0) {
		        $("#shopping-cart-table tbody").html(context.LineItemsView);
		    }

		    if ($("#MiniCartContainer").length > 0) {
		        $("#MiniCartContainer").html(context.LineItemsView);
		    }
		} else {
		    $('#row-' + context.DeleteId).fadeOut('slow');
		}

		$('#cart-count').html(context.CartCount + ' items');
		$('#cart-subtotal').html(context.CartSubTotal);
		$('#cart-total').html(context.CartTotal);
		$('#messages').html("<li class=\"success-msg\"><ul><li><span>" + context.Message + "</span></li></ul></li>");
		//$('#update-message').parent().css({ display: "block" });
		VirtoCommerce.updateQuickLinks();
	},

	onCompareListUpdate: function (context)
	{
		if (context.CartCount == 0)
		{
			"You have no items in your compare list.".Localize(function (translation)
			{
				$('#CompareListContainer').html('<p class="empty">' + translation + '</p>');
			});
		} else
		{
			$('li#compare-' + context.DeleteId).fadeOut('slow');
			$('#messages').html("<li class=\"success-msg\"><ul><li><span>" + context.Message + "</span></li></ul></li>");
		}
	},

	add: function (name, title, itemId, parentItemId, quantity, relatedItems)
	{


		//Try to get variation
		try
		{
			var variation = this.getVariation();
			if (variation != undefined && variation != "")
			{
				itemId = variation;
			}
		}
		catch (e)
		{
			//Variation is invalid
			return false;
		}

		var url = VirtoCommerce.url('/Cart/Add') + '?height=120&width=500&name=' + name + '&itemId=' + itemId + '&quantity=' + quantity;
		
		if (parentItemId != undefined && parentItemId.length > 0)
		{
			url = url + '&parentItemId=' + parentItemId;
		}

		if (name == this.ShoppingCartName)
		{
			if (relatedItems != undefined && relatedItems.length > 0)
			{
				for (var i = 0; i < relatedItems.length; i++)
				{
					url = url + '&relatedItemId=' + relatedItems[i];
				}
			}
		}
		title.Localize(function (titleLoc)
		{
			tb_show(titleLoc, url, false);
		});


		$('#TB_window').bind('unload', function ()
		{
			VirtoCommerce.updateQuickLinks();
		});

		return false;
	},

	addToCart: function (title, itemId, parentItemId, quantity, relatedItems)
	{
		return this.add(this.ShoppingCartName, title, itemId, parentItemId, quantity, relatedItems);
	},
	
	addToCartOrdersClick: function ()
	{
		var relatedItems = this.collectRelatedItems('order_items[]');

		if (relatedItems.length == 0)
			return false;
		return VirtoCart.addToCart('Item added to cart', null, null, 1, relatedItems);
	},

	addToCartClick: function (selectedItemId, parentItemId)
	{
		if ($('#qty').length != 0 && !$('#qty').valid())
		{
			return false;
		} else
		{
			var relatedItems = this.collectRelatedItems('related_products[]');
			var qty = $('#qty').length != 0 ? $('#qty').val() : 1;

			return VirtoCart.addToCart('Item added to cart', selectedItemId, parentItemId, qty, relatedItems);
		}
	},

	addToWishList: function (title, itemId, parentId, quantity)
	{
		return this.add(this.WishListName, title, itemId, parentId, quantity);
	},

	addToCompareList: function (title, itemId, parentId, quantity)
	{
		return this.add(this.CompareListName, title, itemId, parentId, quantity);
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
	},

	updateMiniCart: function (name)
	{
		var placeholder = $("#MiniCartContainer");
		var urlForData = '/Cart/MiniView';

		if (name == this.CompareListName)
		{
			placeholder = $("#CompareListContainer");
			urlForData = '/Account/MiniCompareList';
		}

		$.get(VirtoCommerce.url(urlForData), {},
			function (data)
			{
				// if at least one item in cart exists
				if (placeholder)
				{
					placeholder.parent().html(data);
				} else
				{
					// if cart is empty
				}
			}, "html");
	},

	updateCoupon: function (form, onSuccess)
	{
		data = VirtoCommerce.deserializeForm(form);

		$.ajax({
			type: 'POST',
			url: VirtoCommerce.url('/cart/applyCoupon'),
			data: data,
			dataType: 'JSON',
			success: function (context)
			{
				couponUpdated(context, onSuccess);
			},
			error: function (jqXhr)
			{
				VirtoCommerce.extractErrors(jqXhr, validator);
			}
		});

		function couponUpdated(context, successCallback)
		{
			if (context.Message != undefined && context.Message.length > 0)
			{
				$('#messages').html("<li class=\"notice-msg\"><ul><li><span>" + context.Message + "</span></li></ul></li>");
			}
			
			successCallback(context);	
		}
	},

	submitEstimateShipping: function (form)
	{

		form.resetValidation();
		var validator = form.validate();
		if (form.valid())
		{
			//data = VirtoCommerce.deserializeForm(form);
			data = $(form).serializeObject().ShippingEstimateModel;

			$.ajax({
				type: 'POST',
				url: VirtoCommerce.url('/api/cart/estimatepost'),
				data: data,
				dataType: 'JSON',
				success: function (bestShipment)
				{
					$("#shippingRow").fadeIn(1000);
					$("#shippingRow span.price").html("(" + bestShipment.DisplayName + ") " + bestShipment.PriceFormatted);
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
	},

	updateRegions: function (countryId)
	{
		var dropdown, dropdown2, textbox;

		if (this.Id)
		{
			dropdown = '#' + this.Id + '_Address_CountryCode';
			dropdown2 = '#' + this.Id + '_StateProvinceId';
			textbox = '#' + this.Id + '_Address_StateProvince';
		}
		else
		{
			dropdown = '#Address_CountryCode';
			dropdown2 = '#Address_StateProvinceId';
			textbox = '#Address_StateProvince';
		}

		$.each(v_regions, function (index, country)
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

function initQtySpinner(id, min, max, isAvailable)
{
	return $(id).spinner({ min: min, max: max });
}