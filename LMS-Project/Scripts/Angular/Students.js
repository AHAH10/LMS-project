(function () {
    var app = angular.module('student', []);

    app.controller('studentsController', ['$scope', '$http', function ($scope, $http) {
        $scope.students = [];

        $scope.getStudents = getStudents;
        $scope.orderBy = orderBy;

        function getStudents(usersname) {
            $http.get('/api/StudentsAPI/GetLastLesson?studentName=' + usersname)
                .then(function (response) {
                    $scope.students = response.data;
                });
        }

        $scope.propertyName = 'Student.LastName+Student.FirstName';
        $scope.reverse = false;

        function orderBy(propertyName) {
            $scope.reverse = $scope.propertyName === propertyName ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        }
    }]);
})();