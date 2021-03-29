angular.module('platformWebApp')
    .directive('vaGenericValueInput', ['$compile', '$templateCache', '$http', 'platformWebApp.objCompareService', 'platformWebApp.bladeNavigationService',
        function ($compile, $templateCache, $http, objComparer, bladeNavigationService) {
            return {
                restrict: 'E',
                require: 'ngModel',
                replace: true,
                transclude: true,
                scope: {
                    languages: "=",
                    getDictionaryValues: "&"
                },
                link: function (scope, element, attr, ngModelController, linker) {
                    scope.currentEntity = ngModelController.$modelValue;

                    scope.context = {};
                    scope.context.currentPropValues = [];
                    scope.context.allDictionaryValues = [];
                    var theEmptyValue = { value: null };

                    scope.$watch('context.currentPropValues', function (newValue, oldValue) {
                        //reflect only real changes
                        if (newValue !== oldValue &&
                            !objComparer.equal(newValue, [theEmptyValue]) &&
                            (newValue.length != scope.currentEntity.values.length || !objComparer.equal(newValue, scope.currentEntity.values))) {
                            if (scope.currentEntity.isDictionary) {
                                scope.currentEntity.values = _.map(newValue, function (x) { return { value: x } });
                            }
                            else if (scope.currentEntity.isArray && scope.currentEntity.isMultilingual && !scope.currentEntity.isDictionary) {
                                //ungrouping values for multilingual array properties
                                scope.currentEntity.values = [];
                                angular.forEach(newValue, function (x) {
                                    var values = _.map(x.values, function (y) { return { locale: x.locale, value: y.value }; });
                                    scope.currentEntity.values = scope.currentEntity.values.concat(values);
                                });
                            }
                            else {
                                scope.currentEntity.values = newValue;
                            }

                            ngModelController.$setViewValue(scope.currentEntity);
                        }
                    }, true);

                    scope.$watch('languages', function (languages) {
                        if (scope.currentEntity.isMultilingual && !scope.currentEntity.isDictionary) {
                            initLanguagesValuesMap(scope.currentEntity, languages);
                        }
                    });

                    ngModelController.$render = function () {
                        scope.currentEntity = ngModelController.$modelValue;

                        if (!scope.currentEntity.ngBindingModel) {
                            scope.context.currentPropValues = angular.copy(scope.currentEntity.values);
                            if (needAddEmptyValue(scope.currentEntity, scope.context.currentPropValues)) {
                                scope.context.currentPropValues.push(angular.copy(theEmptyValue));
                            }
                        }

                        if (scope.currentEntity.isDictionary) {
                            scope.getDictionaryValues()(scope.currentEntity, setDictionaryValues);
                        }

                        //Group values for multilingual array properties
                        if (scope.currentEntity.isMultilingual && scope.currentEntity.isArray && !scope.currentEntity.isDictionary) {
                            var groupByLocaleObj = _.groupBy(scope.context.currentPropValues, 'locale');
                            scope.context.currentPropValues = [];
                            for (var key in groupByLocaleObj) {
                                if (groupByLocaleObj.hasOwnProperty(key)) {
                                    scope.context.currentPropValues.push({ locale: key, values: groupByLocaleObj[key] });
                                }
                            }
                        }
                        changeValueTemplate();
                    };

                    function needAddEmptyValue(property, values) {
                        return !property.isArray && !property.isDictionary && !property.isMultilingual && values.length == 0;
                    }

                    function initLanguagesValuesMap(property, languages) {
                        //Group values by language
                        angular.forEach(languages, function (language) {
                            //Currently select values
                            var currentPropValues = _.filter(scope.context.currentPropValues, function (x) { return x.locale == language; });
                            //need add empty value for single  value type
                            if (currentPropValues.length == 0) {
                                scope.context.currentPropValues.push({ value: null, locale: language });
                            }
                        });
                    }

                    function setDictionaryValues(allDictionaryValues) {
                        var selectedValues = scope.currentEntity.values;
                        scope.context.allDictionaryValues = [];
                        scope.context.currentPropValues = [];

                        angular.forEach(allDictionaryValues, function (dictValue) {
                            scope.context.allDictionaryValues.push(dictValue);
                            //Need to select already selected values. Dictionary values have same type as standard values.
                            if (_.any(selectedValues, function (x) { return x.value.id == dictValue.id || x.valueId == dictValue.id })) {
                                //add selected value
                                scope.context.currentPropValues.push(dictValue);
                            }
                        });
                    }

                    function getTemplateName(property) {
                        var result;
                        switch (property.valueType) {
                            case 'Html':
                            case 'Json':
                                result = 'dCode';
                                break;
                            case 'PositiveInteger':
                                return 'dPositiveInteger.html';
                            default:
                                result = 'd' + property.valueType;
                        }

                        if (property.isDictionary) {
                            result += '-dictionary';
                        }
                        if (property.isArray) {
                            result += '-multivalue';
                        }
                        if (property.isMultilingual) {
                            result += '-multilang';
                        }
                        result += '.html';
                        return result;
                    }

                    function changeValueTemplate() {
                        if (scope.currentEntity.valueType === 'Html' || scope.currentEntity.valueType === 'Json') {
                            // Codemirror configuration
                            scope.editorOptions = {
                                lineWrapping: true,
                                lineNumbers: true,
                                extraKeys: { "Ctrl-Q": function (cm) { cm.foldCode(cm.getCursor()); } },
                                foldGutter: true,
                                gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
                                onLoad: function (_editor) { }
                            };
                        }

                        if (scope.currentEntity.valueType === 'Html') {
                            scope.editorOptions['parserfile'] = 'liquid.js';
                            scope.editorOptions['mode'] = 'htmlmixed';
                        }

                        if (scope.currentEntity.valueType === 'Json') {
                            scope.editorOptions['parserfile'] = 'javascript.js';
                            scope.editorOptions['mode'] = { name: 'javascript', json: true };
                        }

                        var templateName = getTemplateName(scope.currentEntity);

                        //load input template and display
                        $http.get(templateName, { cache: $templateCache }).then(function (results) {
                            //Need to add ngForm to isolate form validation into sub form
                            //var innerContainer = "<div id='innerContainer' />";

                            //We must destroy scope of elements we are removing from DOM to avoid angular firing events
                            var el = element.find('#valuePlaceHolder #innerContainer');
                            if (el.length > 0) {
                                el.scope().$destroy();
                            }
                            var container = angular.element("<div><div id='valuePlaceHolder'></div></div>");
                            element.append(container);

                            container = element.find('#valuePlaceHolder');
                            var result = container.html(results.data.trim());

                            if (scope.currentEntity.ngBindingModel) {
                                $(result).find('[ng-model]').attr("ng-model", 'currentEntity.blade.currentEntity.' + scope.currentEntity.ngBindingModel);
                            }

                            //Create new scope, otherwise we would destroy our directive scope
                            var newScope = scope.$new();
                            $compile(result)(newScope);
                        });
                    }

                    /* Datepicker */
                    scope.datepickers = {
                        DateTime: false
                    }

                    scope.open = function ($event, which) {
                        $event.preventDefault();
                        $event.stopPropagation();

                        scope.datepickers[which] = true;
                    };

                    linker(function (clone) {
                        element.append(clone);
                    });

                    /* Image */
                    var originalBlade;
                    scope.uploadImage = function () {
                        var newBlade = {
                            id: "imageUpload",
                            currentEntityId: 'images',
                            title: 'platform.blades.asset-upload.title',
                            subtitle: scope.currentEntity.name,
                            controller: 'platformWebApp.assets.assetUploadController',
                            template: '$(Platform)/Scripts/app/assets/blades/asset-upload.tpl.html',
                            fileUploadOptions: {
                                singleFileMode: true,
                                accept: "image/*",
                                suppressParentRefresh: true
                            }
                        };
                        newBlade.onUploadComplete = function (data) {
                            if (data && data.length) {
                                scope.context.currentPropValues[0].value = data[0].url;
                                bladeNavigationService.closeBlade(newBlade);
                            }
                        }

                        //saving orig blade reference (that is not imageUpload blade) for subsequent showBlade calls
                        if (!originalBlade) {
                            originalBlade = bladeNavigationService.currentBlade.id !== "imageUpload" ? bladeNavigationService.currentBlade : bladeNavigationService.currentBlade.parentBlade;
                        }
                        bladeNavigationService.showBlade(newBlade, originalBlade);
                    }

                    scope.clearImage = function () {
                        scope.context.currentPropValues[0].value = undefined;
                    }

                    scope.openUrl = function (url) {
                        window.open(url, '_blank');
                    }
                }
            }
        }]);
