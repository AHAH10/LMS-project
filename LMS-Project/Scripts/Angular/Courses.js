(function () {
    var sortSAsc = true;
    var sortTAsc = true;

    var app = angular.module("LMSApp");
    app.controller("CourseCtrl", ["$scope", "$http", function ($scope, $http) {

        $scope.selectedSubject = document.getElementById("subjects").value;

        $scope.Start = function(){
            update();
            //sortTheList("teachers");
            //sortTheList("subjects");
            alert("testin");
        }

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

        function update() {
            $scope.selectedSubject = document.getElementById("subjects").value;
            getAvaibleTeachers();
        }
        $scope.Update = function () {
            update();
        };
        function sortTheList(sID)
        {
            if (sID == "teachers") {
                var op = [];

                for (var i = 0; i < $scope.teachers.length; i++) {
                    var _option = { value: "", text: "" };
                    _option.value = $scope.teachers[i].Id;
                    _option.text = $scope.teachers[i].FirstName + " " + $scope.teachers[i].LastName;
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
            if (sID == "subjects") {
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