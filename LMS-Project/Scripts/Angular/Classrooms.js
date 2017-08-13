(function () {
    var app = angular.module('classroom', ["waiting-load"]);

    app.controller('classroomsController', ['$scope', '$http', function ($scope, $http) {
        $scope.classrooms = [];

        $scope.getClassrooms = getClassrooms;
        $scope.orderBy = orderBy;

        function getClassrooms() {
            $http.get('/api/ClassroomsAPI/Get')
                .then(function (response) {
                    $scope.classrooms = response.data;
                });
        }

        $scope.propertyName = 'Name';
        $scope.reverse = false;

        function orderBy(propertyName) {
            $scope.reverse = $scope.propertyName === propertyName ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        }
    }]);
})();