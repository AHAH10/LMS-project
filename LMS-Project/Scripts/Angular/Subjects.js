(function () {
    var app = angular.module("LMSApp");
    //index
    app.controller("Subject_Index_Ctrl", ["$scope", "$http", function ($scope, $http) {
        $scope.reverse = true;

        $scope.orderByMe = function (type) {
            $scope.myOrderBy = type;
            $scope.reverse = ($scope.myOrderBy === type) ? !$scope.reverse : false;
        };
        $scope.getData = function () {
            $http.get('/api/SubjectsAPI/GetAllSubjects')
                .then(function (response) {
                    $scope.subjects = JSON.parse(JSON.stringify(response.data));
                });
        };

    }]);
}());