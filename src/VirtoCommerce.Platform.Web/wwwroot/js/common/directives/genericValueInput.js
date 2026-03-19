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
                    hiddenLanguages: "=",
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
                            //need add empty value for single value type
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
                            if (_.any(selectedValues, function (x) { return (x.value && x.value.id == dictValue.id) || x.valueId == dictValue.id })) {
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

                    // Add JSON validation function
                    function validateJson(cm, ngModel) {
                        if (!cm) return true;

                        try {
                            var content = cm.getValue();
                            // Allow empty values to pass validation
                            if (!content) return true;

                            // Try to parse the JSON
                            JSON.parse(content);
                            // If no exception, JSON is valid
                            ngModel.$setValidity('json', true);
                            return true;
                        } catch (e) {
                            // JSON is invalid, mark the form as invalid
                            ngModel.$setValidity('json', false);
                            return false;
                        }
                    }

                    function configureJsonEditorInterface(editorOptions) {
                        editorOptions.extraKeys = Object.assign(scope.editorOptions.extraKeys || {}, {
                            "Ctrl-Alt-F": function (cm) {
                                scope.formatJson(cm);
                            }
                        });

                        editorOptions.onLoad = function (_editor) {
                            scope.editor = _editor;

                            // Create container for buttons
                            var buttonContainer = angular.element('<div class="json-controls"></div>');
                            buttonContainer.css({
                                position: 'absolute',
                                top: '5px',
                                right: '5px',
                                zIndex: '10',
                                display: 'flex',
                                gap: '5px'
                            });

                            // Create formatting button
                            var formatButton = angular.element('<button class="json-btn format-btn" title="Format JSON (Ctrl+Alt+F)">Format JSON</button>');
                            formatButton.on('click', function () {
                                scope.formatJson();
                            });

                            // Create validation status indicator
                            var statusIndicator = angular.element('<div class="json-btn status-indicator"></div>');

                            // Apply common styles to both elements
                            var commonButtonStyle = {
                                padding: '2px 8px',
                                fontSize: '12px',
                                borderRadius: '3px',
                                height: '24px',
                                lineHeight: '20px',
                                boxSizing: 'border-box',
                                border: 'none'
                            };

                            // Style the format button
                            formatButton.css(Object.assign({}, commonButtonStyle, {
                                backgroundColor: '#43b0e6',
                                color: 'white',
                                cursor: 'pointer'
                            }));

                            // Style the validation indicator (initially hidden)
                            statusIndicator.css(Object.assign({}, commonButtonStyle, {
                                backgroundColor: '#4CAF50',
                                color: 'white',
                                display: 'none',
                                alignItems: 'center',
                                justifyContent: 'center'
                            }));

                            // Add both elements to the container
                            buttonContainer.append(statusIndicator);
                            buttonContainer.append(formatButton);

                            // Append container to editor
                            var wrapper = angular.element(_editor.getWrapperElement());
                            wrapper.prepend(buttonContainer);

                            // Validate JSON when editor loads and on change
                            _editor.on("change", function () {
                                var isValid = validateJson(_editor, ngModelController);
                                updateValidationIndicator(statusIndicator, isValid);
                            });

                            // Initial validation
                            setTimeout(function () {
                                var isValid = validateJson(_editor, ngModelController);
                                updateValidationIndicator(statusIndicator, isValid);
                            }, 100);
                        };
                    }

                    function updateValidationIndicator(statusIndicator, isValid) {
                        if (isValid) {
                            statusIndicator.css({
                                backgroundColor: '#4CAF50',
                                display: 'none'
                            });
                            statusIndicator.text('Valid JSON');
                        } else {
                            statusIndicator.css({
                                backgroundColor: '#F44336',
                                display: 'flex'
                            });
                            statusIndicator.text('Invalid JSON');
                        }
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

                            // Add JSON lint if needed (but don't rely on it for validation)
                            if (typeof window.jsonlint !== 'undefined') {
                                scope.editorOptions['lint'] = true;
                                scope.editorOptions.gutters = ["CodeMirror-linenumbers", "CodeMirror-foldgutter", "CodeMirror-lint-markers"];
                            }

                            configureJsonEditorInterface(scope.editorOptions);

                            // Initialize with validation
                            setTimeout(function () {
                                if (scope.editor) {
                                    validateJson(scope.editor, ngModelController);
                                }
                            }, 200);
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

                            // After compile, ensure validation runs for JSON fields
                            if (scope.currentEntity.valueType === 'Json') {
                                setTimeout(function () {
                                    if (scope.editor) {
                                        validateJson(scope.editor, ngModelController);
                                    }
                                }, 200);
                            }
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
                            controller: 'virtoCommerce.assetsModule.assetUploadController',
                            template: 'Modules/$(VirtoCommerce.Assets)/Scripts/blades/asset-upload.tpl.html',
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

                    scope.isLanguageVisible = function (language) {
                        if (scope.hiddenLanguages) {
                            if (_.contains(scope.hiddenLanguages, language)) {
                                return false;
                            }
                        }

                        return true;
                    }

                    scope.formatJson = function (cm) {
                        cm = cm || scope.editor;
                        if (!cm) return;

                        try {
                            var content = cm.getValue();
                            if (!content) return;

                            var formatted = JSON.stringify(JSON.parse(content), null, 2);
                            cm.setValue(formatted);

                            // Validate after formatting
                            validateJson(cm, ngModelController);
                        } catch (e) {
                            console.error('JSON formatting error:', e);
                        }
                    };
                }
            }
        }]);
