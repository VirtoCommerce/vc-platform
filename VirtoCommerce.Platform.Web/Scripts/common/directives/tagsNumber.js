angular.module('ngTagsInput')
.config(['$provide', function ($provide) {
    $provide.decorator('tagsInputDirective', ['$delegate', 'platformWebApp.numberUtils', 'tiUtil',  function ($delegate, numberUtils, tiUtil) {
        var directive = $delegate[0];
        directive.compile = function () {
            return function (scope, element, attrs) {

                var options = scope.options;
                var events = scope.events;
                var onTagAdding = tiUtil.handleUndefinedResult(scope.onTagAdding, true);

                var getTagValue = function (tag) {
                    return tiUtil.safeToString(tag[options.displayProperty]);
                };
                var setTagValue = function (tag, text) {
                    tag[options.displayProperty] = text;
                };

                var tagIsValid = function (tag) {
                    var tagText = getTagValue(tag);

                    var value = (angular.isDefined(attrs.tagsNumber) && numberUtils.validate(tagText, attrs.numType, attrs.min, attrs.max, attrs.fraction));
                    if (angular.isDefined(value)) {
                        setTagValue(tag, value);
                    }
                    return tagText &&
                        tagText.length >= options.minLength &&
                        tagText.length <= options.maxLength &&
                        options.allowedTagsPattern.test(tagText) &&
                        !tiUtil.findInObjectArray(scope.tagList.items, tag, options.keyProperty || options.displayProperty) &&
                        angular.isDefined(value) &&
                        onTagAdding({ $tag: tag });
                };

                scope.tagList.add = function(tag) {
                    var tagText = getTagValue(tag);

                    if (options.replaceSpacesWithDashes) {
                        tagText = tiUtil.replaceSpacesWithDashes(tagText);
                    }

                    setTagValue(tag, tagText);

                    if (tagIsValid(tag)) {
                        scope.tagList.items.push(tag);
                        events.trigger('tag-added', { $tag: tag });
                    }
                    else if (tagText) {
                        events.trigger('invalid-tag', { $tag: tag });
                    }

                    return tag;
                };

                if (angular.isDefined(attrs.tagsNumber)) {
                    angular.extend(scope.options, { tagsNumber: attrs.tagsNumber, numType: attrs.numType, min: attrs.min, max: attrs.max, fraction: attrs.fraction });
                }

                directive.link.apply(this, arguments);
            };
        };
        return $delegate;
    }]);
    $provide.decorator('tiTagItemDirective', ['$delegate', 'platformWebApp.numberUtils', function ($delegate, numberUtils) {
        var directive = $delegate[0];
        var compile = directive.compile;
        directive.compile = function() {
            var link = compile.apply(this, arguments);
            return function (scope, element, attrs, tagsInputCtrl) {
                link.apply(this, arguments);

                // not real double-registration, this method just return object with needed methods
                var tagsInput = tagsInputCtrl.registerTagItem();
                var options = tagsInput.getOptions();
                if (angular.isDefined(options.tagsNumber)) {
                    scope.$getDisplayText = function() {
                        return numberUtils.format(scope.data[options.displayProperty], options.numType, options.min, options.max, options.fraction);
                    }
                }
            };
        };
        return $delegate;
    }]);
}]);