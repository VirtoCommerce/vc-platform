angular.module('platformWebApp').factory('platformWebApp.clipboardService', [function () {
    function fallbackCopy(text) {
        var copyElement = document.createElement("span");
        copyElement.appendChild(document.createTextNode(text));
        document.body.appendChild(copyElement);

        var range = document.createRange();
        range.selectNode(copyElement);
        window.getSelection().removeAllRanges();
        window.getSelection().addRange(range);

        document.execCommand('copy');
        window.getSelection().removeAllRanges();
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
