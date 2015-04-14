/// <reference path="jquery-1.7.2.min.js" />
// Copyright 2012 Omar AL Zabir
// This is part of Droptiles open source project.


/* enhance $.getSctipt to handle mutiple scripts */
var getScript = jQuery.getScript;
var get = jQuery.get;

jQuery.getScript = function (resources, callback) {

    var // reference declaration & localization
    length = resources.length,
    handler = function () { counter++; },
    deferreds = [],
    counter = 0,
    idx = 0;

    for (; idx < length; idx++) {
        deferreds.push(
            getScript(resources[idx], handler)
        );
    }

    jQuery.when.apply(null, deferreds).then(function () {
        callback && callback();
    });
};

jQuery.get = function (resources, callback) {

    if (!jQuery.isArray(resources))
        return get.apply(this, arguments);

    var // reference declaration & localization
    length = resources.length,
    results = [],
    handler = function (result) { results.push(result); counter++; },
    deferreds = [],
    counter = 0,
    idx = 0;

    for (; idx < length; idx++) {
        deferreds.push(
            get(resources[idx], handler)
        );
    }

    jQuery.when.apply(null, deferreds).then(function () {
        callback && callback(results);
    });
};

/**
 * Call once at beginning to ensure your app can safely call console.log() and
 * console.dir(), even on browsers that don't support it.  You may not get useful
 * logging on those browers, but at least you won't generate errors.
 * 
 * @param  alertFallback - if 'true', all logs become alerts, if necessary. 
 *   (not usually suitable for production)
 */
function fixConsole(alertFallback) {
    if (typeof console === "undefined") {
        console = {}; // define it if it doesn't exist already
    }
    if (typeof console.log === "undefined") {
        if (alertFallback) { console.log = function (msg) { alert(msg); }; }
        else { console.log = function () { }; }
    }
    if (typeof console.dir === "undefined") {
        if (alertFallback) {
            // THIS COULD BE IMPROVED… maybe list all the object properties?
            console.dir = function (obj) { alert("DIR: " + obj); };
        }
        else { console.dir = function () { }; }
    }
}


fixConsole();

function fullscreen() {
    var el = document.documentElement
        , rfs = // for newer Webkit and Firefox
               el.requestFullScreen
            || el.webkitRequestFullScreen
            || el.mozRequestFullScreen
            || el.msRequestFullScreen
    ;
    if (typeof rfs != "undefined" && rfs) {
        rfs.call(el);
    } else if (typeof window.ActiveXObject != "undefined") {
            // for Internet Explorer
        var wscript = new ActiveXObject("WScript.Shell");
        if (wscript != null) {
            wscript.SendKeys("{F11}");
        }
    }
}

// jQuery.browser is depcrecated in 1.9. So, bring it back.
jQuery.uaMatch = function (ua) {
    ua = ua.toLowerCase();
    var match = /(chrome)[ \/]([\w.]+)/.exec(ua) ||
        /(webkit)[ \/]([\w.]+)/.exec(ua) ||
        /(opera)(?:.*version|)[ \/]([\w.]+)/.exec(ua) ||
        /(msie) ([\w.]+)/.exec(ua) ||
        ua.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([\w.]+)|)/.exec(ua) ||
        [];
    return {
        browser: match[1] || "",
        version: match[2] || "0"
    };
};
if (!jQuery.browser) {
    matched = jQuery.uaMatch(navigator.userAgent);
    browser = {};
    if (matched.browser) {
        browser[matched.browser] = true;
        browser.version = matched.version;
    }
    // Chrome is Webkit, but Webkit is also Safari.
    if (browser.chrome) {
        browser.webkit = true;
    } else if (browser.webkit) {
        browser.safari = true;
    }
    jQuery.browser = browser;
}