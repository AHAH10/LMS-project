(function () {
    var app = angular.module('schedule', ["checklist-model"]);

    app.controller('schedulesController', ['$scope', '$http', '$window', function ($scope, $http, $window) {
        //ScopeVariable for Collection Data
        $scope.weekDays;
        $scope.courses = [];
        $scope.classrooms = [];
        $scope.students = [];
        $scope.schedules = [];
        //End Collection Data

        $scope.schedule = {
            'ID': undefined,
            'WeekDay': undefined,
            'BeginningTime': "08:00",
            'EndingTime': "19:00",
            'CourseID': undefined,
            'ClassroomID': undefined,
            'Students': []
        };

        $scope.init = function (schedulId) {
            getWeekDays();
            getCourses();
            getClassrooms();
            getStudents();

            if (schedulId) {
                // Let's get some information
                $http.get('/api/SchedulesAPI/Get?id=' + schedulId)
                    .then(function (response) {
                        $scope.schedule.ID = schedulId;
                        $scope.schedule.WeekDay = response.data.WeekDay;
                        $scope.schedule.BeginningTime = ExtractTime(response.data.BeginningTime);
                        $scope.schedule.EndingTime = ExtractTime(response.data.EndingTime);
                        $scope.schedule.CourseID = response.data.CourseID;
                        $scope.schedule.ClassroomID = response.data.ClassroomID;
                        $scope.schedule.Students = response.data.Students;
                    });
            }
        };

        function ExtractTime(dateString) {
            var date = new Date(dateString);
            var hours = date.getHours();
            if (hours < 10) {
                hours = '0' + hours;
            }
            var minutes = date.getMinutes();
            if (minutes < 10) {
                minutes = '0' + minutes;
            }

            return hours + ':' + minutes;
        }

        $scope.displayCourse = displayCourse;
        $scope.displayClassroom = displayClassroom;
        $scope.getSchedules = getSchedules;
        $scope.getWeekDays = getWeekDays;
        $scope.orderBy = orderBy;
        $scope.sendData = sendData;

        function getWeekDays() {
            $http.get('/api/SchedulesAPI/GetWeekDays')
                .then(function (response) {
                    $scope.weekDays = response.data;

                    // Init by the first available value
                    if (!$scope.schedule.WeekDay) {
                        $scope.schedule.WeekDay = $scope.weekDays[0].Key;
                    }
                });
        }

        function getCourses() {
            $http.get('/api/C_And_S_API/GetAllCourses')
                .then(function (response) {
                    $scope.courses = response.data;
                });
        }

        function displayCourse(course) {
            return course.Subject.Name + ' (' + course.Teacher.FirstName + ' ' + course.Teacher.LastName + ')';
        }

        function displayClassroom(classroom) {
            return classroom.Name + (classroom.Remarks === null ? '' : ' - ' + classroom.Remarks) + ' (' + classroom.AmountStudentsMax + ' st. max)';
        }

        function getClassrooms() {
            $http.get('/api/ClassroomsAPI/Get')
                .then(function (response) {
                    $scope.classrooms = response.data;
                });
        }

        function getStudents() {
            $http.get('/api/UsersAPI/GetStudents')
                .then(function (response) {
                    $scope.students = response.data;
                })
                .catch(function (errorMessage) {
                    var mybody = angular.element(document).find('body');
                    mybody.removeClass('waiting');
                });
        }

        function getSchedules() {
            $http.get('/api/SchedulesAPI/Get')
                .then(function (response) {
                    $scope.schedules = response.data;
                });
        }

        $scope.propertyName = 'LastName';
        $scope.reverse = false;

        function orderBy(propertyName) {
            $scope.reverse = $scope.propertyName === propertyName ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        }

        function sendData() {
            if ($scope.schedule.ID) {
                $http.put('/api/SchedulesAPI/Put/', $scope.schedule)
                    .then(function (response) {
                        $window.location.href = "/Schedules/Index";
                    }, function (error) {
                        var split = error.data['Message'].split("\\");
                        $scope.errorHead = '';
                        $scope.errorMessage = [];

                        // Filter on empty lines
                        for (var i = 0; i < split.length; i += 1) {
                            if (split[i]) {
                                if (!$scope.errorHead) {
                                    $scope.errorHead = split[i];
                                }
                                else {
                                    $scope.errorMessage.push(split[i]);
                                }
                            }
                        }

                        scrollToTopPage();
                    });
            }
            else {
                $http.post('/api/SchedulesAPI/Post/', $scope.schedule)
                    .then(function (response) {
                        $window.location.href = "/Schedules/Index";
                    }, function (error) {
                        var split = error.data['Message'].split("\\");
                        $scope.errorHead = '';
                        $scope.errorMessage = [];

                        // Filter on empty lines
                        for (var i = 0; i < split.length; i += 1) {
                            if (split[i]) {
                                if (!$scope.errorHead) {
                                    $scope.errorHead = split[i];
                                }
                                else {
                                    $scope.errorMessage.push(split[i]);
                                }
                            }
                        }

                        scrollToTopPage();
                    });
            }
        }

        var scrollToTopPage = function () {
            $window.location.href = "#";
        };

        // Code for time picker
        $scope.timeRange = [];
        $scope.initTimePicker = initTimePicker();

        function initTimePicker(hourMin, minuteMin, hourMax, minuteMax) {
            if (hourMax === undefined) {
                hourMax = 19;
            }
            // For hours dropdown (7 - 19)
            for (var h = 7; h < 20; h++) {
                // For minutes dropdown (0 - 55)
                for (var m = 0; m < 60; m += 5) {
                    var formatedTime = (h < 10 ? '0' + h : h) + ':' + (m < 10 ? '0' + m : m);

                    $scope.timeRange.push({
                        name: formatedTime,
                        value: formatedTime
                    });
                }
            }
        }
    }]);

    app.directive('ngStartWaiting', function () {
        return function (scope, element, attrs) {
            var mybody = angular.element(document).find('body');
            mybody.addClass('waiting');
        };
    });

    app.directive('ngTestEndWaiting', function () {
        return function (scope, element, attrs) {
            if (scope.$last) {
                var mybody = angular.element(document).find('body');
                mybody.removeClass('waiting');
            }
        };
    });
}());