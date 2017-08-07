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
            $http.get('/api/C_And_S_API/GetAllCourses')
                .then(function (response) {
                    $scope.courses = JSON.parse(JSON.stringify(response.data));
                })
        };
    }]);
    //Create
    app.controller("Course_Create_Ctrl", ["$scope", "$http", function ($scope, $http) {

        $scope.selectedSubject = document.getElementById("subjects").value;
        var start = true;
        var sortSAsc = true;
        var sortTAsc = true;

        $scope.Start = function () {
            sortTheList("subjects");
            update();
        }

        function getAvaibleTeachers() {
            $http.get('/api/C_And_S_API/GetAvailableTeachersWithLessInfo?subjectID=' + $scope.selectedSubject)
                .then(function (response) {
                    $scope.teachers = JSON.parse(JSON.stringify(response.data)); //Any references will be converted into objectsS

                    var op = [];

                    for (var i = 0; i < $scope.teachers.length; i++) {
                        var _option = { value: "", text: "" };
                        _option.value = $scope.teachers[i].Id;
                        _option.text = $scope.teachers[i].FirstName + " " + $scope.teachers[i].LastName;
                        op.push(_option);
                    }
                    updateDropDownList("teachers", op);
                    if (start) {
                        sortTheList("teachers");
                        start = false;
                    }
                });
        }
        function sort(options, desc) {
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

        function update() {
            $scope.selectedSubject = document.getElementById("subjects").value;
            getAvaibleTeachers();
        }
        $scope.Update = function () {
            update();
        };
        //an ng-repeat select list would had been easier, but if javascript is disabled on the client, the dropdown list should still work
        function sortTheList(sID) {
            if (sID == "teachers") {
                var op = [];

                for (var i = 0; i < document.getElementById("teachers").getElementsByTagName("option").length; i++) {
                    var _option = { value: "", text: "" };
                    _option.value = document.getElementById("teachers").getElementsByTagName("option")[i].value;
                    _option.text = document.getElementById("teachers").getElementsByTagName("option")[i].text;
                    op.push(_option);
                }
                if (sortTAsc == true) {
                    op = sort(op, true);
                    sortTAsc = false;
                }
                else {
                    op = sort(op, false);
                    sortTAsc = true;
                }
                updateDropDownList("teachers", op);
            }
            else {
                var opS = [];

                for (var i = 0; i < document.getElementById("subjects").getElementsByTagName("option").length; i++) {
                    var _option = { value: "", text: "" };
                    _option.value = document.getElementById("subjects").getElementsByTagName("option")[i].value;
                    _option.text = document.getElementById("subjects").getElementsByTagName("option")[i].text;
                    opS.push(_option);
                }
                if (sortSAsc == true) {
                    opS = sort(opS, true);
                    sortSAsc = false;
                }
                else {
                    opS = sort(opS, false);
                    sortSAsc = true;
                }
                updateDropDownList("subjects", opS);
            }
        }
        $scope.SortList = function (sID) {
            sortTheList(sID);
        };

        function updateDropDownList(select_id, optionsList) {
            var list = document.getElementById(select_id);
            //Remove all options from the dropdownlist
            if (list.getElementsByTagName("option") != null) {
                while (list.firstChild) {
                    list.removeChild(list.firstChild);
                }
            }
            //create and append new options
            for (var i = 0; i < optionsList.length; i++) {
                var element = document.createElement("option");
                element.textContent = optionsList[i].text;
                element.value = optionsList[i].value;
                list.appendChild(element);
            }
        }
    }]);

}());