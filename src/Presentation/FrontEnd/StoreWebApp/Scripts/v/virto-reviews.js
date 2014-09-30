//Reviews class
Reviews =
{
    PageSize: 10,
    PageIndex: 0,
    TotalReviews: 0,
    CurrentSort: undefined,
    ReviewVotes: [],
    CommentVotes: [],
    FLAGS: [],
    container: '',
    currentItemId: '',

    init: function (options) {
        this.container = options.container;
        this.currentItemId = options.currentItemId;

        this.initGlobals();
        initSort();
        initPaging();
        $("#sortList").change();
        updatePagedItemNumbers();

        function initSort() {
            $("#sortList").change(function () {
                var sortBy = $(this).val();
                switch (sortBy) {
                    case "date_new":
                        Reviews.getReviews("CreatedDateTime desc");
                        break;
                    case "date_old":
                        Reviews.getReviews("CreatedDateTime asc");
                        break;
                    case "rating_high":
                        Reviews.getReviews("Rating desc");
                        break;
                    case "rating_low":
                        Reviews.getReviews("Rating asc");
                        break;
                    case "helpful_high":
                        Reviews.getReviews("PositiveFeedbackCount desc");
                        break;
                    case "helpful_low":
                        Reviews.getReviews("PositiveFeedbackCount asc");
                        break;
                }
            });
        }

        function initPaging() {
            $(".pr-page-prev").click(function () {
                if (Reviews.PageIndex > 0) {
                    Reviews.PageIndex = Reviews.PageIndex - 1;
                    Reviews.getReviews(Reviews.CurrentSort, Reviews.PageIndex);
                    updatePagedItemNumbers();
                }
            });
            $(".pr-page-next").click(function () {
                if (Math.ceil(Reviews.TotalReviews / Reviews.PageSize) > Reviews.PageIndex + 1) {
                    Reviews.PageIndex = Reviews.PageIndex + 1;
                    Reviews.getReviews(Reviews.CurrentSort, Reviews.PageIndex);
                    updatePagedItemNumbers();
                }
            });
        }

        function updatePagedItemNumbers() {
            var itemsFrom = Reviews.PageIndex * Reviews.PageSize + 1;
            var itemsTo = (Math.ceil(Reviews.TotalReviews / Reviews.PageSize) == Reviews.PageIndex + 1)
            ? Reviews.TotalReviews : (Reviews.PageIndex + 1) * Reviews.PageSize;
            $(".pr-page-count strong").text(itemsFrom + "-" + itemsTo);
        }
    },

    initGlobals: function () {
        $.ajax({

            type: "GET",
            url: VirtoCommerce.url("/api/review/GetReviewTotals/" + Reviews.currentItemId),
            dataType: "json",
            success: function (data)
            {
                if (data.length > 0) {
                    Reviews.TotalReviews = data[0].TotalReviews;
                    $(".pr-review-count-number").text(Reviews.TotalReviews);

                    if (data[0].TotalReviews > 0) {
                        $(".product-info .rating").show();
                        $(".product-info .rating").attr("title", data[0].AverageRating);
                        $(".product-info .rating").rateit({ resetable: false, readonly: true, value: data[0].AverageRating.toFixed(1), starwidth: 11, starheight: 11 });
                        $(".pr-review-average").text(data[0].AverageRating.toFixed(1));
                        $("#product_tabs_reviews_contents_empty").hide();
                        $("#product_tabs_reviews_contents_list").show();
                        $("#show_reviews_link").show();
                    }
                } else {
                    $("#product_tabs_reviews_contents_empty").show();
                    $("#product_tabs_reviews_contents_list").hide();
                }
            }
        });
    },

    getReviews: function (sort, pageIndex) {

        Reviews.CurrentSort = sort;
        Reviews.PageIndex = pageIndex == undefined ? this.PageIndex : pageIndex;

        var reviewsUrl = "/api/review/get/" + Reviews.currentItemId + "/?$top=" + this.PageSize;
        reviewsUrl = reviewsUrl + "&$skip=" + (this.PageSize * this.PageIndex);
        //reviewsUrl = reviewsUrl + "&$filter=ItemId eq '" + Reviews.currentItemId + "'";

        if (sort != undefined && sort != "") {
            reviewsUrl = reviewsUrl + "&$orderby=" + sort;
        }

        $.ajax({
            type: "GET",
            url: VirtoCommerce.url(reviewsUrl),
            dataType: "json",
            success: function (data) {
                $(Reviews.container).empty();
                $("#reviewTemplate").tmpl(data).appendTo(Reviews.container);

                //FIX: for unotrusive validation dynamic content
                //process each form retrieved by the ajax call
                $(Reviews.container + " form").each(function () {
                    $(this).removeData('validator');
                    $(this).removeData('unobtrusiveValidation');
                    $.validator.unobtrusive.parse("#" + this.id);
                });

                $(Reviews.container + " .rating-block").each(function ()
                {
                    var rating = $(this).find(".count .num").text();
                    $(this).find(".rating").rateit({ resetable: false, readonly: true, value: rating, starwidth: 11, starheight: 11  });
                });

                
            }
        });
    },
    comment: function (id) {

        var form = $("form#comment_" + id);
        var validator = form.validate();

        if (!validator.valid()) {
            return;
        }

        data = VirtoCommerce.deserializeForm(form);

        $.ajax({
            type: 'PUT',
            url: VirtoCommerce.url('/api/review/comment'),
            suppressErrors: true,
            data: data,
            dataType: 'JSON',
            success: function (comment) {
                form.parent().hide();
                form.formReset();
                form.resetValidation();

                //Just for testing add immediatly
                //$("#commentTemplate").tmpl(comment).appendTo("#comments_container_" + id);
                "Thank you for your feedback! We will review it and publish.".Localize(function (translation) {
                	$.showPopupMessage(translation);
                });
            },
            error: function (jqXhr) {
                VirtoCommerce.extractErrors(jqXhr, validator);
            }
        });
    },
    review: function (form) {

        //form.resetValidation();
        var validator = form.validate();
        //if (!validator.valid()) { return; }

        data = VirtoCommerce.deserializeForm(form);

        $.ajax({
            type: 'PUT',
            url: VirtoCommerce.url('/api/review/addreview'),
            data: data,
            suppressErrors: true,
            dataType: 'JSON',
            success: function (newReview) {
                //close dialog
                $('.form-popup, .fade-block').hide();

                "Thank you for your feedback! We will review it and publish.".Localize(function (translation) {
                    $.showPopupMessage(translation);
                });

                //update fields
                //Reviews.initGlobals();
                //Reviews.getReviews("CreatedDateTime desc", 0);

                //show reviews
                $("#show_reviews_link").trigger("click");
            },
            error: function (jqXhr)
            {
                VirtoCommerce.extractErrors(jqXhr, validator);
            }
        });
    },
    vote: function (id, bLike) {

        if ($.inArray(id, Reviews.ReviewVotes) >= 0) {
        	"You may only submit one vote per review.".Localize("#review_feedback_" + id);
            return;
        }

        $.ajax({
            type: 'POST',
            url: VirtoCommerce.url('/api/review/vote'),
            data: { Id: id, Like: bLike, IsReview: true },
            suppressErrors: true,
            dataType: 'JSON',
            success: function () {
        	"Thank you for your feedback.".Localize("#review_feedback_" + id);
            Reviews.ReviewVotes.push(id);

            $("#total_feedback_review_" + id).text(parseInt($("#total_feedback_review_" + id).text()) + 1);

                if (bLike) {
                    $("#positive_review_" + id).text(parseInt($("#positive_review_" + id).text()) + 1);
                }
            },
            error: function (data)
            {
                data.responseJSON.Message.Localize("#review_feedback_" + id);
            }
        });
    },
    votecomment: function (id, bLike) {
        if ($.inArray(id, Reviews.CommentVotes) >= 0) {
        	"You may only submit one vote per comment.".Localize("#comment_feedback_" + id);
            return;
        }

        $.ajax({
            type: 'POST',
            url: VirtoCommerce.url('/api/review/vote'),
            data: { Id: id, Like: bLike, IsReview: false },
            suppressErrors: true,
            dataType: 'JSON',
            success: function () {
        	"Thank you for your feedback.".Localize("#comment_feedback_" + id);
            Reviews.CommentVotes.push(id);

            $("#total_feedback_comment_" + id).text(parseInt($("#total_feedback_comment_" + id).text()) + 1);

            if (bLike) {
                $("#positive_comment_" + id).text(parseInt($("#positive_comment_" + id).text()) + 1);
            }
            },
            error: function (data) {
                data.responseJSON.Message.Localize("#comment_feedback_" + id);
            }
        });

    },
    flag: function (id) {

        var form = $("form#abuse_" + id);
        var validator = form.validate();

        if (!validator.valid()) {
            return;
        }

        data = VirtoCommerce.deserializeForm(form);

        $.ajax({
            type: 'PUT',
            url: VirtoCommerce.url('/api/review/reportabuse'),
            suppressErrors: true,
            data: data,
            dataType: 'JSON',
            success: function () {
                form.parent().hide();
                form.formReset();
                form.resetValidation();
                "Thank you for your feedback! We will review it and take necessary actions.".Localize(function (translation)
                {
                    $.showPopupMessage(translation);
                });
            },
            error: function (jqXhr)
            {
                VirtoCommerce.extractErrors(jqXhr, validator);
            }
        });
    },
    showAllComments: function (id)
    {
        $.ajax({
            type: "GET",
            url: VirtoCommerce.url("/api/review/getcomments/" + id + "?$orderby=CreatedDateTime desc"),
            dataType: "json",
            success: function (data) {

                //Hide show all comments
                $("#show_all_comment_for_" + id).hide();

                //Reload comments
                var commentsContainer = $("#comments_container_" + id);
                $(commentsContainer).empty();
                $("#commentTemplate").tmpl(data).appendTo(commentsContainer);

                //FIX: for unotrusive validation dynamic content
                //process each form retrieved by the ajax call
                commentsContainer.find("form").each(function () {
                    $(this).removeData('validator');
                    $(this).removeData('unobtrusiveValidation');
                    $.validator.unobtrusive.parse("#" + this.id);
                });
            }
        });
    },
    reviewPreview: function (form, previewContainer)
    {
        data = new Array();
        data.push((form).serializeObject());
        previewContainer.empty();
        previewContainer.hide();
        $("#reviewTemplate").tmpl(data).appendTo(previewContainer);

        previewContainer.find(".rating-block").each(function ()
        {
            var rating = $(this).find(".count .num").text();
            $(this).find(".rating").rateit({ resetable: false, readonly: true, value: rating, starwidth: 11, starheight: 11 });
        });

        //Hide comments
        previewContainer.find(".feedback").remove();

        //unbind clicks
        previewContainer.find("[onclick]").attr("onclick", "return false;");
        
        previewContainer.fadeIn(2000);
    },
    catalogReviews: function(filter) {
        $.ajax({
            type: "GET",
            url: VirtoCommerce.url("/api/review/GetReviewTotals?$filter=" + filter),
            dataType: "json",
            success: function (data)
            {
                for (var i = 0; i < data.length; i++)
                {
                    $("li#" + data[i].ItemId + " #rateit").rateit({ resetable: false, readonly: true, value: data[i].AverageRating.toFixed(1), starwidth: 11, starheight: 11 });
                    $("li#" + data[i].ItemId + " #totalreviews").text(data[i].TotalReviews);
                }
            }
        });
    }
};

//Public helpers

function GetYear(jsonDate) {
    var dateTime = new Date(jsonDate);
    return dateTime.getFullYear();
}

function GetDay(jsonDate) {
    var dateTime = new Date(jsonDate);
    return dateTime.getDate();
}

function GetMonth(jsonDate) {
    var d = new Date(jsonDate);
    var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    return monthNames[d.getMonth()];
}

function GetShortDate(jsonDate) {
    return GetMonth(jsonDate) + " " + GetDay(jsonDate) + ", " + GetYear(jsonDate);
}

function GetShortDateAndTime(jsonDate) {
    var dateTime = new Date(jsonDate);
    return GetShortDate(jsonDate) + " at " + dateTime.getHours() + ":" + dateTime.getMinutes();
}

