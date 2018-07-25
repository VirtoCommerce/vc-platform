///https://github.com/dcohenb/angular-img-fallback
///Angular directives that handles image loading, it has fallback-src to handle errors in image loading and loading-src for placeholder while the image is being loaded.
angular.module('platformWebApp')
    .directive('fallbackSrc', ['imageService', imageService => {
        return {
            restrict: 'A',
            link: (scope, element, attr) => {

                // Update the service with the correct missing src if present, otherwise use the default image
                let newSrc = attr.fallbackSrc ? imageService.setMissing(attr.fallbackSrc) : imageService.getMissing();

                // Listen for errors on the element and if there are any replace the source with the fallback source
                let errorHandler = () => {

                    // fallbackSrc may have changed since the link function ran, so try to grab it again.
                    let newSrc = attr.fallbackSrc ? imageService.setMissing(attr.fallbackSrc) : imageService.getMissing();

                    if (element[0].src !== newSrc) {
                        element[0].src = newSrc;
                    }
                };

                // Replace the loading image with missing image if `ng-src` link was broken
                if (element[0].src === imageService.getLoading()) {
                    element[0].src = newSrc;
                }

                element.on('error', errorHandler);

                scope.$on('$destroy', () => {
                    element.off('error', errorHandler);
                });

            }
        };
    }])
    .directive('loadingSrc', ['$interpolate', 'imageService', ($interpolate, imageService) => {
        // Load the image source in the background and replace the element source once it's ready
        let linkFunction = (scope, element, attr) => {
            // Update the service with the correct loading src if present, otherwise use the default image
            element[0].src = attr.loadingSrc ? imageService.setLoading(attr.loadingSrc) : imageService.getLoading();

            let img = new Image();
            img.src = $interpolate(attr.imgSrc)(scope);

            img.onload = () => {
                img.onload = null;
                if (element[0].src !== img.src) {
                    element[0].src = img.src;
                }
            };
        };

        return {
            restrict: 'A',
            compile: (el, attr) => {
                // Take over the ng-src attribute to stop it from loading the image
                attr.imgSrc = attr.ngSrc;
                delete attr.ngSrc;

                return linkFunction;
            }
        };
    }])
    .factory('imageService', () => {
        // Both images have the same prefix we can save some space on that
        let base64prefix = 'data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAxMDAwIDEwMDAiPjxkZWZzPjxyYWRpYWxHcmFkaWVudCBpZD0icmFkaWFsLWdyYWRpZW50IiBjeD0iNTAwIiBjeT0iNTAwIiByPSI1MDAiIGdyYWRpZW50VW5pdHM9InVzZXJTcGFjZU9uVXNlIj48c3RvcCBvZmZzZXQ9IjAiIHN0b3AtY29sb3I9IiNkZmRmZGYiLz48c3RvcCBvZmZzZXQ9IjEiIHN0b3AtY29sb3I9IiM5OTkiLz48L3JhZGlhbEdyYWRpZW50PjwvZGVmcz48cmVjdCBmaWxsPSJ1cmwoI3JhZGlhbC1ncmFkaWVudCkiIHdpZHRoPSIxMDAwIiBoZWlnaHQ9IjEwMDAiLz48cGF0aCBmaWxsPSIjZmZmIiBkPSJNNjAxIDQxNGwwIDBWNTg2bDAgMEgzOTlsMCAwVjQxNGwwIDBINjAxWm0wLTE0SDM5OUExNCAxNCAwIDAgMCAzODUgNDE0djE3M2ExNCAxNCAwIDAgMCAxNCAxNEg2MDFBMTQgMTQgMCAwIDAgNjE1IDU4NlY0MTRhMTQgMTQgMCAwIDAtMTQtMTRoMFpNNTc';

        let loadingDefault = `${base64prefix}1IDUwMmE3NyA3NyAwIDAgMC0yNC01NCA3NiA3NiAwIDAgMC0yNS0xNiA3NSA3NSAwIDAgMC01NyAxQTc0IDc0IDAgMCAwIDQzMCA0NzQgNzMgNzMgMCAwIDAgNDMxIDUzMGE3MiA3MiAwIDAgMCAzOSAzOCA3MCA3MCAwIDAgMCA1NC0xIDY5IDY5IDAgMCAwIDM3LTM4IDY4IDY4IDAgMCAwIDQtMTZsMSAwYTEwIDEwIDAgMCAwIDEwLTEwYzAgMCAwLTEgMC0xaDBabS0xNSAyNmE2NyA2NyAwIDAgMS0zNyAzNSA2NiA2NiAwIDAgMS01MC0xIDY0IDY0IDAgMCAxLTM0LTM1QTYzIDYzIDAgMCAxIDQ0MCA0NzkgNjIgNjIgMCAwIDEgNDU0IDQ1OSA2MiA2MiAwIDAgMSA0NzQgNDQ2YTYxIDYxIDAgMCAxIDIzLTQgNjAgNjAgMCAwIDEgNDIgMTlBNTkgNTkgMCAwIDEgNTUyIDQ4MGE1OCA1OCAwIDAgMSA0IDIyaDBjMCAwIDAgMSAwIDFhMTAgMTAgMCAwIDAgOSAxMCA2NyA2NyAwIDAgMS01IDE1aDBaIi8+PC9zdmc+`;
        let missingDefault = `${base64prefix}yIDQ1MGEyMiAyMiAwIDEgMS0yMi0yMkEyMiAyMiAwIDAgMSA1NzIgNDUwWk01ODYgNTcySDQxNFY1NDNsNTAtODYgNTggNzJoMTRsNTAtNDN2ODZaIi8+PC9zdmc+`;

        return {
            getLoading: () => {
                return loadingDefault;
            },
            getMissing: () => {
                return missingDefault;
            },
            setLoading: (newSrc) => {
                return loadingDefault = newSrc;
            },
            setMissing: (newSrc) => {
                return missingDefault = newSrc;
            }
        };
    });
