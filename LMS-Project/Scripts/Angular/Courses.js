
//function Subjects() {
//    var id = document.getElementById("selectedValue").value;

//    $.getJSON("/api/CoursesAPI/GetAvaibleTeachers?subjectID=" + id, function (data) {
//        alert(data)
//    });
//};
//function Teachers() {
//    var id = document.getElementById("selectedValueT").value;

//    $.getJSON("/api/CoursesAPI/GetAvaibleSubjects?teacherID=" + id, function (data) {
//        alert(data);
//    });
//};

var app = angular.module("LMSApp");
app.controller("CourseCtrl", ["$scope", "$http", function ($scope, $http) {
    $scope.data = "Angular is Working!";
    //$scope.startGame = function () {
    //    var id = [Math.floor((Math.random() * 3))];
    //    $scope.id = id;
    //    $scope.animal = animals[id];
    //    $scope.image = "/Content/Images/" + $scope.animal + ".jpg";
    //}
}]);