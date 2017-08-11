angular.module('waiting-load', [])
    .directive('myStartWaiting', function () {
        return function (scope, element, attrs) {
            var mybody = angular.element(document).find('body');
            mybody.addClass('waiting');
        };
    })

    .directive('myTestEndWaiting', function () {
        return function (scope, element, attrs) {
            if (scope.$last) {
                var mybody = angular.element(document).find('body');
                mybody.removeClass('waiting');
            }
        };
    });