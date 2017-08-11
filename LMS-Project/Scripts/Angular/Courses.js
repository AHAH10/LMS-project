(function () {
    var app = angular.module("LMSApp");
    //index
    app.controller("Course_Index_Ctrl", ["$scope", "$http", function ($scope, $http) {
        $scope.reverse = true;

        $scope.orderByMe = function (type) {
            $scope.myOrderBy = type;
            $scope.reverse = ($scope.myOrderBy === type) ? !$scope.reverse : false;
        };
        $scope.getData = function () {
            $http.get('/api/CoursesAPI/GetAllCourses')
                .then(function (response) {
                    $scope.courses = JSON.parse(JSON.stringify(response.data));
                })
        };
    }]);
    //Edit
    app.controller("Course_Edit_Ctrl", ["$scope", "$http", function ($scope, $http) {
        $scope.reverse = true;

        $scope.orderByMe = function (type) {
            $scope.myOrderBy = type;
            $scope.reverse = ($scope.myOrderBy === type) ? !$scope.reverse : false;
        };

        $scope.Start = function (sID) {
            $scope.selectedSubject = sID;
            $http.get('/api/UsersAPI/GetAvailableTeachers?subjectID=' + $scope.selectedSubject)
                .then(function (response) {
                    $scope.teachers = JSON.parse(JSON.stringify(response.data));
                });            
        }

    }]);
    //Create
    app.controller("Course_Create_Ctrl", ["$scope", "$http", function ($scope, $http) {
        $scope.Treverse = true;
        $scope.Sreverse = true;

        $scope.orderBySubject = function (type) {
            orderBy(type,"Subject");
        };
        $scope.orderByTeacher = function (type) {
            orderBy(type, "Teacher");
        };
        function orderBy(t,o) {
            if (o == "Teacher") {
                $scope.TheOrderByTeacher = t;
                $scope.Treverse = ($scope.TheOrderByTeacher === t) ? !$scope.Treverse : false;
            }
            else{
                $scope.TheOrderBySubject = t;
                $scope.Sreverse = ($scope.TheOrderBySubject === t) ? !$scope.Sreverse : false;
            }
        }

        $scope.Start = function () {
            $http.get('/api/SubjectsAPI/GetAllSubjects')
                .then(function (response) {
                    $scope.subjects = JSON.parse(JSON.stringify(response.data));
                    $scope.subject = $scope.subjects[0].ID;
                    $http.get('/api/UsersAPI/GetAvailableTeachers?subjectID=' + $scope.subject)
                        .then(function (response) {
                            $scope.teachers = JSON.parse(JSON.stringify(response.data));
                            orderBy("Name", "Subject");
                            orderBy("LastName", "Teacher");
                        });
                });
        }

        function uData() {
            $http.get('/api/UsersAPI/GetAvailableTeachers?subjectID=' + $scope.subject)
                .then(function (response) {
                    $scope.teachers = JSON.parse(JSON.stringify(response.data));

                });
        }

        $scope.update = function () {
            uData();
        }

    }]);

}());