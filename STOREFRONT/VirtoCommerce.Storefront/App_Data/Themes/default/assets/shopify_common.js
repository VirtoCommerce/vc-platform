/* 
 * Shopify Common JS
 *
 */

if ((typeof Shopify) == 'undefined') {
  var Shopify = {};
}

Shopify.bind = function(fn, scope) {
  return function() {
    return fn.apply(scope, arguments);
  }
};

// set a given selector with value, if value is one of the options
Shopify.setSelectorByValue = function(selector, value) {
  for (var i = -1, count = selector.options.length; ++i < count; i) {
    if (value == selector.options[i].value) {
      selector.selectedIndex = i;
      return i;
    }
  }
};

// add a callback to an event
Shopify.addListener = function(target, eventName, callback) {
  target.addEventListener ? target.addEventListener(eventName, callback, false) : target.attachEvent('on'+eventName, callback);
};

// send request as a POST
Shopify.postLink = function(path, options) {
  options = options || {};
  var method = options['method'] || 'post';
  var params = options['parameters'] || {};
  
  var form = document.createElement("form");
  form.setAttribute("method", method);
  form.setAttribute("action", path);
  
  for(var key in params) {
    var hiddenField = document.createElement("input");
    hiddenField.setAttribute("type", "hidden");
    hiddenField.setAttribute("name", key);
    hiddenField.setAttribute("value", params[key]);
    form.appendChild(hiddenField);
  }
  document.body.appendChild(form);
  form.submit();
  document.body.removeChild(form);      
};

/* CountryProvinceSelector
 * js class that adds listener to country selector and on change updates
 * province selector with valid province values.
 * Selector should be in this format:
 *
 *   <select id="country_selector" name="country" data-default="Canada">
 *     <option data-provinces="['Alberta','Ontario','British Columbia',...] value="Canada">Canada</option>
 *     ...
 *   </select>
 *   <select id="province_selector" name="province" data-default="Ontario">
 *     <option value="Ontario">Ontario</option>
 *     ...
 *   </select> 
 */
Shopify.CountryProvinceSelector = function(country_domid, province_domid, options) {
  this.countryEl         = document.getElementById(country_domid);
  this.provinceEl        = document.getElementById(province_domid);
  this.provinceContainer = document.getElementById(options['hideElement'] || province_domid);

  Shopify.addListener(this.countryEl, 'change', Shopify.bind(this.countryHandler,this));
  
  this.initCountry();
  this.initProvince();
};

Shopify.CountryProvinceSelector.prototype = {
  initCountry: function() {
    var value = this.countryEl.getAttribute('data-default');    
    Shopify.setSelectorByValue(this.countryEl, value);    
    this.countryHandler();
  },
  
  initProvince: function() {
    var value = this.provinceEl.getAttribute('data-default');
    if (value && this.provinceEl.options.length > 0) {
      Shopify.setSelectorByValue(this.provinceEl, value);
    }
  },
  
  countryHandler: function(e) {
    var opt       = this.countryEl.options[this.countryEl.selectedIndex];
    var raw       = opt.getAttribute('data-provinces');
    var provinces = JSON.parse(raw);

    this.clearOptions(this.provinceEl);
    if (provinces && provinces.length == 0) {
      this.provinceContainer.style.display = 'none';
    } else {
      this.setOptions(this.provinceEl, provinces);
      this.provinceContainer.style.display = "";
    }
  },
  
  clearOptions: function(selector) {
    while (selector.firstChild) {
      selector.removeChild(selector.firstChild);
    }
  },
  
  setOptions: function(selector, values) {
    for (var i = 0, count = values.length; i < values.length; i++) {
      var opt = document.createElement('option');
      opt.value = values[i];
      opt.innerHTML = values[i];
      selector.appendChild(opt);        
    }    
  }
};