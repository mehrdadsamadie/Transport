var MyApp = angular.module('MyApp', ["ui.router", "angucomplete-alt", "ngAnimate", "LocalStorageModule", "ionic", "ionic-timepicker", "ion-autocomplete", "angular.filter","ion-floating-menu"]);
MyApp.config(['localStorageServiceProvider', function (localStorageServiceProvider) {
    localStorageServiceProvider.setPrefix('Transport');

}]);
MyApp.config(['$stateProvider', '$urlRouterProvider', '$urlMatcherFactoryProvider',
    function ($stateProvider, $urlRouterProvider, $urlMastcherFactoryProvider) {
        $urlRouterProvider
            .otherwise('/');
        $stateProvider
            .state('home', {
                url: "/home",
                templateUrl: "angularJs/views/home/home.html",
                controller: "homeCtrl",
                resolve: {
                    PersonelInfo: function (PersonelData) {
                        return PersonelData.getOfficeList();
                    }
                }
            })
            .state('logIn', {
                url: "/logIn",
                templateUrl: "angularJs/views/account/logIn.html",
                controller: "accountCtrl",
                resolve: {
                    MyServicehome: function (authService) {
                        return authService.isAuthenticat();
                    }
                }
            })
            .state('requestList', {
                url: "/requestList",
                templateUrl: "angularJs/views/request/list.html",
                controller: "requestListCtrl",
                resolve: {
                    dateNow: function (DateService) {
                        return DateService.getOfficeList();
                    }
                }
            })
            .state('requestCreate', {
                url: "/requestCreate",
                templateUrl: "angularJs/views/request/create.html",
                params: { Id: null },
                controller: "requestCreateCtrl",
                resolve: {
                    MyServicehome: function (requestCreateData) {
                        
                        return requestCreateData.promise;
                    }
                }
            })
            .state('serviceList', {
                url: "/serviceList",
                templateUrl: "angularJs/views/service/list.html",
                controller: "serviceListCtrl",
                resolve: {
                    dateNow: function (DateService) {
                        return DateService.getOfficeList();
                    }
                }
            })
            .state('serviceDetail', {
                url: "/serviceDetail",
                templateUrl: "angularJs/views/request/detail.html",
                params: { Id: null },
                controller: "serviceDetailCtrl",
                resolve: {
                    MyServicehome: function (requestCreateData) {

                        return requestCreateData.promise;
                    }
                }
            })
        $urlRouterProvider.otherwise('/logIn');
    }]);
MyApp.run(function ($rootScope, $location, $state, $window, gPSFactory) {
    var history = [];
    $rootScope.startload = true;
    $rootScope.loading = false;

    $rootScope.$on('$stateChangeSuccess', function () {
        gPSFactory.setEnd();
        if (history.length > 1) {
            for (var i = 0; i < history.length; i++) {
                var indexhis = history[i].indexOf("/", 1);
                var indexloc = $location.$$path.indexOf("/", 1);
                if (indexhis > 0) {
                    var subhis = history[i].substring(0, indexhis)
                }
                if (indexloc > 0) {
                    var subloc = $location.$$path.substring(0, indexloc)
                }
                if (subhis == subloc) {

                    history.splice(i, 1);
                }
            }
        }
        history.push($location.$$path);
        $rootScope.startload = false
        $rootScope.loading = false;
    });
    $rootScope.back = function () {
        var prevUrl = history.length > 1 ? history.splice(-2)[0] : "/";
        $location.path(prevUrl);
    };
});
MyApp.config(function (ionicTimePickerProvider) {
    var timePickerObj = {
        inputTime: (((new Date()).getHours() * 60 * 60) + ((new Date()).getMinutes() * 60)),
        format: 24,
        step: 1,
        setLabel: 'تایید',
        closeLabel: 'لغو'
    };
    ionicTimePickerProvider.configTimePicker(timePickerObj);
});
//enable zoom in div 
//MyApp.config(function ($ionicConfigProvider) {
//    $ionicConfigProvider.scrolling.jsScrolling(true);
//})

