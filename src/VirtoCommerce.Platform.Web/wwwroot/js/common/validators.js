angular.module('platformWebApp')
 .factory('platformWebApp.validators', [function () {
     function webSafeFileNameValidator(value) {
         var pattern = /^[\w.-]+$/;
         return pattern.test(value);
     }

     return {
         webSafeFileNameValidator: webSafeFileNameValidator
     };
 }]);
