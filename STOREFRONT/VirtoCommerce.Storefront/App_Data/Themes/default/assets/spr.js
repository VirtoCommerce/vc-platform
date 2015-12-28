window.innerShiv = function () {
    function t(t, e, n) {
        return /^(?:area|br|col|embed|hr|img|input|link|meta|param)$/i.test(n) ? t : e + "></" + n + ">"
    }
    var e, n, i = document,
        r = "abbr article aside audio canvas datalist details figcaption figure footer header hgroup mark meter nav output progress section summary time video".split(" ");
    return function (a, o) {
        if (!e && (e = i.createElement("div"), e.innerHTML = "<nav></nav>", n = 1 !== e.childNodes.length)) {
            for (var s = i.createDocumentFragment(), l = r.length; l--;) s.createElement(r[l]);
            s.appendChild(e)
        }
        if (a = a.replace(/^\s\s*/, "").replace(/\s\s*$/, "").replace(/<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi, "").replace(/(<([\w:]+)[^>]*?)\/>/g, t), e.innerHTML = (s = a.match(/^<(tbody|tr|td|col|colgroup|thead|tfoot)/i)) ? "<table>" + a + "</table>" : a, s = s ? e.getElementsByTagName(s[1])[0].parentNode : e, o === !1) return s.childNodes;
        for (var l = i.createDocumentFragment(), c = s.childNodes.length; c--;) l.appendChild(s.firstChild);
        return l
    }
}(),
function () {
    window.SPR = function () {
        function t() { }
        return t.shop = Shopify.shop, t.host = "//productreviews.shopifycdn.com", t.version = "v4", t.api_url = "" + t.host + "/proxy/" + t.version, t.asset_url = "" + t.host + "/assets/" + t.version, t.badgeEls = [], t.reviewEls = [], t.elSettings = {}, t.$ = void 0, t.extraAjaxParams = {
            shop: t.shop
        }, t.registerCallbacks = function () {
            return this.$(document).bind("spr:badge:loaded", "undefined" != typeof SPRCallbacks && null !== SPRCallbacks ? SPRCallbacks.onBadgeLoad : void 0), this.$(document).bind("spr:product:loaded", "undefined" != typeof SPRCallbacks && null !== SPRCallbacks ? SPRCallbacks.onProductLoad : void 0), this.$(document).bind("spr:reviews:loaded", "undefined" != typeof SPRCallbacks && null !== SPRCallbacks ? SPRCallbacks.onReviewsLoad : void 0), this.$(document).bind("spr:form:loaded", "undefined" != typeof SPRCallbacks && null !== SPRCallbacks ? SPRCallbacks.onFormLoad : void 0), this.$(document).bind("spr:form:success", "undefined" != typeof SPRCallbacks && null !== SPRCallbacks ? SPRCallbacks.onFormSuccess : void 0), this.$(document).bind("spr:form:failure", "undefined" != typeof SPRCallbacks && null !== SPRCallbacks ? SPRCallbacks.onFormFailure : void 0)
        }, t.loadStylesheet = function () {
            var t, e;
            return e = document.createElement("link"), e.setAttribute("rel", "stylesheet"), e.setAttribute("type", "text/css"), e.setAttribute("href", "" + this.asset_url + "/spr.css"), e.setAttribute("media", "screen"), t = document.getElementsByTagName("head")[0], t.appendChild(e)
        }, t.initRatingHandler = function () {
            return t.$(document).on("mouseover mouseout", "form a.spr-icon-star", function (e) {
                var n, i, r;
                return n = e.currentTarget, r = t.$(n).attr("data-value"), i = t.$(n).parent(), "mouseover" === e.type ? (i.find("a.spr-icon:lt(" + r + ")").addClass("spr-icon-star-hover"), i.find("a.spr-icon:gt(" + (r - 1) + ")").removeClass("spr-icon-star-hover")) : i.find("a.spr-icon").removeClass("spr-icon-star-hover")
            })
        }, t.initDomEls = function () {
            return this.badgeEls = this.$(".shopify-product-reviews-badge[data-id]"), this.reviewEls = this.$("#shopify-product-reviews[data-id]"), this.$.each(this.reviewEls, function (t) {
                return function (e, n) {
                    var i;
                    return i = t.$(n).attr("data-id"), t.elSettings[i] = {}, t.elSettings[i].reviews_el = "#" + (t.$(n).attr("data-reviews-prefix") ? t.$(n).attr("data-reviews-prefix") : "reviews_"), t.elSettings[i].form_el = "#" + (t.$(n).attr("data-form-prefix") ? t.$(n).attr("data-form-prefix") : "form_")
                }
            }(this))
        }, t.loadProducts = function () {
            return this.$.each(this.reviewEls, function (t) {
                return function (e, n) {
                    var i, r, a;
                    return r = t.$(n).attr("data-id"), i = t.$(n).attr("data-autoload"), "false" !== i ? (a = t.$.extend({
                        product_id: r,
                        version: t.version
                    }, t.extraAjaxParams), t.$.get("" + t.api_url + "/reviews/product", a, t.productCallback, "jsonp")) : void 0
                }
            }(this))
        }, t.loadBadges = function () {
            var t, e, n, i, r;
            if (n = this.$.map(this.badgeEls, function (t) {
                return function (e) {
                    return t.$(e).attr("data-id")
            }
            }(this)), n.length > 0) {
                for (e = 7, r = [];
                    (t = n.splice(0, e)).length > 0;) i = this.$.extend(this.extraAjaxParams, {
                        product_ids: t
                    }), r.push(this.$.get("" + this.api_url + "/reviews/badges", i, this.badgesCallback, "jsonp"));
                return r
            }
        }, t.pageReviews = function (t) {
            var e, n, i;
            return i = this.$(t).data("product-id"), n = this.$(t).data("page"), e = this.$.extend({
                page: n,
                product_id: i
            }, this.extraAjaxParams), this.$.get("" + this.api_url + "/reviews", e, this.paginateCallback, "jsonp"), !1
        }, t.submitForm = function (t) {
            var e;
            return e = this.$(t).serializeObject(), e = this.$.extend(e, this.extraAjaxParams), e = this.$.param(e), e = e.replace(/%0D%0A/g, "%0A"), this.$.ajax({
                url: "" + this.api_url + "/reviews/create",
                type: "GET",
                dataType: "jsonp",
                data: e,
                success: this.formCallback,
                beforeSend: function (t) {
                    return function () {
                        return t.$(".spr-button-primary").attr("disabled", "disabled")
                    }
                }(this),
                complete: function (t) {
                    return function () {
                        return t.$(".spr-button-primary").removeAttr("disabled")
                    }
                }(this)
            }), !1
        }, t.reportReview = function (t) {
            var e;
            return confirm("Are you sure you want to report this review as inappropriate?") && (e = this.$.extend({
                id: t
            }, this.extraAjaxParams), this.$.get("" + this.api_url + "/reviews/report", e, this.reportCallback, "jsonp")), !1
        }, t.toggleReviews = function (t) {
            var e;
            return e = this.$("#shopify-product-reviews[data-id='" + t + "']"), e.find(".spr-reviews").toggle()
        }, t.toggleForm = function (t) {
            var e;
            return e = this.$("#shopify-product-reviews[data-id='" + t + "']"), e.find(".spr-form").toggle()
        }, t.setRating = function (t) {
            var e, n, i;
            return e = this.$(t).parents("form"), i = this.$(t).attr("data-value"), n = this.$(t).parent(), e.find("input[name='review[rating]']").val(i), this.setStarRating(i, n)
        }, t.setStarRating = function (t, e) {
            return e.find("a:lt(" + t + ")").removeClass("spr-icon-star-empty spr-icon-star-hover"), e.find("a:gt(" + (t - 1) + ")").removeClass("spr-icon-star-hover").addClass("spr-icon-star-empty")
        }, t.badgesCallback = function (e) {
            var n;
            return n = e.badges, t.$.map(t.badgeEls, function (e) {
                var i;
                return i = t.$(e).attr("data-id"), void 0 !== n[i] ? (t.$(e).replaceWith(n[i]), t.$(document).trigger("spr:badge:loaded", {
                    id: i
                })) : void 0
            })
        }, t.productCallback = function (e) {
            var n;
            return n = e.remote_id.toString(), t.renderProduct(n, e.product), t.renderForm(n, e.form), t.renderReviews(n, e.reviews)
        }, t.renderProduct = function (t, e) {
            return this.$.map(this.reviewEls, function (n) {
                return function (i) {
                    return t === n.$(i).attr("data-id") ? (n.$(i).html(innerShiv(e, !1)), n.$(document).trigger("spr:product:loaded", {
                        id: t
                    })) : void 0
                }
            }(this))
        }, t.renderForm = function (t, e) {
            var n;
            return n = this.$(this.elSettings[t].form_el + t), n.html(e), this.$(document).trigger("spr:form:loaded", {
                id: t
            })
        }, t.renderReviews = function (e, n) {
            var i;
            return i = t.$(t.elSettings[e].reviews_el + e), i.html(n), t.$(document).trigger("spr:reviews:loaded", {
                id: e
            })
        }, t.formCallback = function (e) {
            var n, i, r, a;
            return a = e.status, r = e.remote_id, i = e.form, n = t.$(t.elSettings[r].form_el + r), n.html(i), "failure" === a && t.initStarRating(n), "success" === a && t.$("#shopify-product-reviews[data-id='" + r + "'] .spr-summary-actions-newreview").hide(), t.$(document).trigger("spr:form:" + a, {
                id: r
            })
        }, t.initStarRating = function (t) {
            var e, n, i;
            return i = t.find("input[name='review[rating]']"), i && i.val() ? (n = i.val(), e = t.find(".spr-starrating"), this.setStarRating(n, e)) : void 0
        }, t.paginateCallback = function (e) {
            var n, i;
            return i = e.remote_id.toString(), n = e.reviews, t.renderReviews(i, n)
        }, t.reportCallback = function (e) {
            var n;
            return n = "#report_" + e.id, t.$(n).replaceWith("<span class='spr-review-reportreview'>" + t.$(n).attr("data-msg") + "</span>")
        }, t.loadjQuery = function (e) {
            return t.loadScript("//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js", function () {
                return t.$ = jQuery.noConflict(!0), e()
            })
        }, t.loadScript = function (t, e) {
            var n;
            return n = document.createElement("script"), n.type = "text/javascript", n.readyState ? n.onreadystatechange = function () {
                return "loaded" === n.readyState || "complete" === n.readyState ? (n.onreadystatechange = null, e()) : void 0
            } : n.onload = function () {
                return e()
            }, n.src = t, document.getElementsByTagName("head")[0].appendChild(n)
        }, t.loadjQueryExtentions = function (t) {
            return t.fn.serializeObject = function () {
                var e, n;
                return e = {}, n = this.serializeArray(), t.each(n, function () {
                    return e[this.name] ? (e[this.name].push || (e[this.name] = [e[this.name]]), e[this.name].push(this.value || "")) : e[this.name] = this.value || ""
                }), e
            }
        }, t
    }(),
    function () {
        return SPR.loadStylesheet(), SPR.loadjQuery(function () {
            return SPR.$.ajaxSetup({
                cache: !1
            }), SPR.loadjQueryExtentions(SPR.$), SPR.$(document).ready(function () {
                return SPR.registerCallbacks(), SPR.initRatingHandler(), SPR.initDomEls(), SPR.loadProducts(), SPR.loadBadges()
            })
        })
    }()
}.call(this);