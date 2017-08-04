(function () {
    var sortSAsc = true;
    var sortTAsc = true;

    var app = angular.module("LMSApp");
    app.controller("CourseCtrl", ["$scope", "$http", function ($scope, $http) {

        $scope.selectedSubject = document.getElementById("subjects").value;

        function getAvaibleTeachers() {
            $http.get('/api/CoursesAPI/GetAvaibleTeachers?subject=' + $scope.selectedSubject)
                .then(function (response) {
                    $scope.teachers = JSON.parse(JSON.stringify(response.data));

                    var op = [];

                    for (var i = 0; i < $scope.teachers.length; i++) {
                        var _option = { value: "", text: "" };
                        _option.value = $scope.teachers[i].Id;
                        _option.text = $scope.teachers[i].FirstName + " " + $scope.teachers[i].LastName;
                        op.push(_option);
                    }

                    updateDropDownList("teachers", op);
                });
        }
        function sort(options,desc){
            if (desc == true) {
                options.sort(function (a, b) {
                    var textA = a.text.toLowerCase(), textB = b.text.toLowerCase()
                    if (textA < textB)
                        return -1
                    if (textA > textB)
                        return 1
                    return 0
                });
            }
            else {
                options.sort(function (a, b) {
                    var textA = a.text.toLowerCase(), textB = b.text.toLowerCase()
                    if (textA > textB)
                        return -1
                    if (textA < textB)
                        return 1
                    return 0
                });
            }
            return options;
        }

        $scope.Update = function () {
            $scope.selectedSubject = document.getElementById("subjects").value;
            getAvaibleTeachers();
        };
        $scope.SortList = function (sID) {
            alert(sID);
        };
    }]);

    function updateDropDownList(select_id,optionsList) {
        var list = document.getElementById(select_id);
        
        if (list.getElementsByTagName("option") != null) {
            while (list.firstChild) {
                list.removeChild(list.firstChild);
            }
        }
        for (var i = 0; i < optionsList.length; i++) {
            var element = document.createElement("option");
            element.textContent = optionsList[i].text;
            element.value = optionsList[i].value;
            list.appendChild(element);
        }
    }
 
}());