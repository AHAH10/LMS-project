(function () {
    var app = angular.module("LMSApp", []);

    app.controller('NotificationsController', ['$scope', '$http', function ($scope, $http) {
        $scope.reverse = false;

        $scope.OrderByType = function (type) {
            Sort(type);
        };
        function Sort(type) {
            $scope.OrderBy = type;
            $scope.reverse = ($scope.OrderBy === type) ? !$scope.reverse : false;
        }
        function getData() {
            $http.get('/api/NotificationsAPI/GetAllNotifications?userID='+$scope.user)
                .then(function (response) {
                    $scope.notifications = response.data;
                    setReadingDate();
                })
        };
        function setReadingDate() {
            $http.post('/api/NotificationsAPI/SetAllNotificationsAsReaded/'+$scope.user)
        };
        $scope.Initial = function (userID) {
            $scope.user = userID;
            getData();
            Sort("ReadingDate");
        }

    }]);
}());