angular.module('platformWebApp').factory('platformWebApp.clipboardService', [function () {
    function copyText(text) {
        if (navigator.clipboard && navigator.clipboard.writeText) {
            navigator.clipboard.writeText(text);
            return;
        }

        // Fallback for older browsers
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

    return {
        copyText: copyText
    };
}]);
