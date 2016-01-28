angular.module('platformWebApp')
 .factory('platformWebApp.validators', ['$translate', function ($translate) {
     function webSafeFileNameValidator(value) {
         var pattern = /^[\w.-]+$/;
         return pattern.test(value);
     };

     return {
         webSafeFileNameValidator: webSafeFileNameValidator
     };
 }]);
