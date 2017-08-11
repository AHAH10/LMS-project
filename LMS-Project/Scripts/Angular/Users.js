(function () {
    var app = angular.module('user', ["waiting-load"]);

    app.controller('usersController', ['$scope', '$http', function ($scope, $http) {
        $scope.users = [];

        $scope.getUsers = getUsers;
        $scope.orderBy = orderBy;

        function getUsers() {
            $http.get('/api/UsersAPI/GetUsers')
                .then(function (response) {
                    $scope.users = response.data;
                });
        }

        $scope.propertyName = 'LastName';
        $scope.reverse = false;

        function orderBy(propertyName) {
            $scope.reverse = $scope.propertyName === propertyName ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        }
    }]);
})();