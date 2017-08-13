(function () {
    var app = angular.module("LMSApp");
    //index
    app.controller("Subject_Index_Ctrl", ["$scope", "$http", function ($scope, $http) {
        $scope.reverse = false;
        $scope.myOrderBy = 'Name';

        $scope.orderByMe = function (type) {
            $scope.reverse = ($scope.myOrderBy === type) ? !$scope.reverse : false;
            $scope.myOrderBy = type;
        };
        $scope.getData = function () {
            $http.get('/api/SubjectsAPI/GetAllSubjects')
                .then(function (response) {
                    $scope.subjects = JSON.parse(JSON.stringify(response.data));
                });
        };

    }]);
}());