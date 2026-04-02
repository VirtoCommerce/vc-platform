angular.module('platformWebApp').factory('platformWebApp.clipboardService', [function () {
    function fallbackCopy(text) {
        var copyElement = document.createElement("textarea");
        copyElement.value = text;
        copyElement.style.position = "fixed";
        copyElement.style.opacity = "0";
        document.body.appendChild(copyElement);

        copyElement.select();
        document.execCommand('copy');
        copyElement.remove();
    }

    function copyText(text) {
        if (navigator.clipboard && navigator.clipboard.writeText) {
            navigator.clipboard.writeText(text).catch(function () {
                fallbackCopy(text);
            });
            return;
        }

        fallbackCopy(text);
    }

    return {
        copyText: copyText
    };
}]);
