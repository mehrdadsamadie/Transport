MyApp.controller('homeCtrl', ['$scope', '$http', '$state', 'path', 'PersonelInfo', 'gPSSaveLocalDb', 'localStorageService', 'authService', function ($scope, $http, $state, path, PersonelInfo, gPSSaveLocalDb, localStorageService, authService) {
    $scope.personel = PersonelInfo.data;
    $scope.path = path.pathName;
    $scope.user = authService.authentication;
    $scope.roles = $scope.user.role == null ? null : $scope.user.role.split(",");
    $scope.logOff = function () {
        $http({
            url: $scope.path + '/Account/LogOffAngular',
            method: 'Post',
        }).success(function (data) {
            var Authenticat = { token: null, passWord: null, userName: "", isAuth: false, role: null };
            localStorageService.set('Authenticat', Authenticat);
            authService.fillAuthData();
            $state.go('logIn');
            }).catch(function (data, status, headers, config) {
            // <--- catch instead error

            //  alert(data.statusText) //contains the error message

        });

    };
}]);
