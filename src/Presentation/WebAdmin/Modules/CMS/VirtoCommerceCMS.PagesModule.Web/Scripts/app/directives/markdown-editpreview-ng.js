/* ===================================================
 * markdown-editpreview-ng.js v1.0.0
 * http://github.com/codemwnci/markdown-editpreview-ng.js
 * ===================================================
 * Copyright 2013-2014 Wayne Ellis
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * ========================================================== */

// directive 
(function() { 'use strict';
  angular.module('codemwnci.markdown-edit-preview', [])
  .directive('markdown', ['$window', '$sce', function($window, $sce) {
    var converter = $window.Markdown.getSanitizingConverter();

    return {
      template: "<div ng-bind-html='sanitisedHtml' />",
      restrict: 'E',
      replace: true,
      scope: {   
        markdown: '=bindFrom' ,
        class: '='
      },
      link: function(scope, element, attrs) {  
        scope.$watch('markdown', function(value) {
          if (value != undefined && value != '') {                        
            scope.html = converter.makeHtml(value); 
          	scope.sanitisedHtml = $sce.trustAsHtml(scope.html);
          }
        });
      }
    };
  }])

.directive('markdownedit', [ function() {
    return {
      restrict: 'A',
      replace: false,
      link: function(scope, element, attrs) {  
        var hiddenButtons = attrs.markdownHiddenButtons ? attrs.markdownHiddenButtons.split(",") : new Array();
        hiddenButtons.push('cmdPreview');
        element.markdown({hiddenButtons: hiddenButtons});        
      },
    };
  }])
  ;

}).call(this);
