MyApp.controller('requestCreateCtrl', ['$scope', '$http', 'position', 'path', '$state', 'ionicTimePicker', '$stateParams', '$filter', 'authService', 'gPSFactory', 'localStorageService', 'requestCreateData', '$rootScope', '$ionicScrollDelegate',
    function ($scope, $http, position, path, $state, ionicTimePicker, $stateParams, $filter, authService, gPSFactory, localStorageService, requestCreateData, $rootScope, $ionicScrollDelegate) {
        $scope.years = [];

        $scope.errors = [];
        $scope.Months = [{
            name: "فروردین",
            number: 1
        }, {
            name: "اردیبهشت",
            number: 2
        }, {
            name: "خرداد",
            number: 3
        }, {
            name: "تیر",
            number: 4
        }, {
            name: "مرداد",
            number: 5
        }, {
            name: "شهریور",
            number: 6
        }, {
            name: "مهر",
            number: 7
        }, {
            name: "ابان",
            number: 8
        }, {
            name: "اذر",
            number: 9
        }, {
            name: "دی",
            number: 10
        }, {
            name: "بهمن",
            number: 11
        }, {
            name: "اسفند",
            number: 12
        }];

        $scope.setDays = function () {
            var days = [];
            if ($scope.selectedMonth == null) {
                for (var i = 1; i <= 31; i++) {
                    days.push(i);
                }
            }
            else {
                if ($scope.selectedMonth.number == 12 && $scope.selectedYear % 4 == 0) {
                    for (var i = 1; i <= 29; i++) {
                        days.push(i);
                    }
                }
                else if ($scope.selectedMonth.number >= 7) {
                    for (var i = 1; i <= 30; i++) {
                        days.push(i);
                    }
                }
                else
                    for (var i = 1; i <= 31; i++) {
                        days.push(i);
                    }
            }
            $scope.Days = days;
        };
        $scope.setYears = function () {
            var today = new Date();
            var year = today.getFullYear() - 621;
            for (var i = year - 100; i <= year - 5; i++) {
                $scope.years.push(i);
            }
            $scope.selectedYear = $scope.years[$scope.years.length - 1];
        };

        $scope.setYears();

        $scope.Days = [];
        $scope.$watch('selectedMonth.number', function (newval, oldval) {

            for (var i = 0; i < $scope.Months.length; i++) {
                if ($scope.Months[i].number == newval) {
                    $scope.selectedMonth.name = $scope.Months[i].name;
                }
            }
            $scope.setDays();
        }, true);

        $scope.selectedMonth = {
            name: "",
            number: null
        };
        $scope.selectedDay =
            {
                day: 1,
            };
        function MySpilit() {
            tmp = $scope.request.ServiceDate.split('/');
            $scope.selectedYear = parseInt(tmp[0]);
            $scope.selectedDay.day = parseInt(tmp[2]);
            $scope.selectedMonth = {
                number: parseInt(tmp[1])
            };


        }


        $scope.currentPosition = position.PositionServiceFactory.currentPosition;
        $scope.path = path.pathName;
        $scope.user = authService.authentication;
        $scope.roles = $scope.user.role == null ? null : $scope.user.role.split(",");
        $scope.requestId = $stateParams.Id;
        $scope.States = requestCreateData.DateFa.value.States;
        $scope.Eparchies = requestCreateData.DateFa.value.Eparchies;
        $scope.ServiceTypes = requestCreateData.DateFa.value.ServiceTypes;
        $scope.request =
            {
                id: null,
                PersonelId: null,
                RequestDate: null,
                RequestDateStr: null,
                ServiceDate: null,
                StartTime: null,
                StartTimeStr: null,
                EndTime: null,
                Description: null,
                IsEmergancy: false,
                FullName: null,
                IsLocal: true,
                ServiceId: false,
                GussetNumber: true,
                ServiceTypeId: null,
                IsMobile: true,
                Longitude: null,
                Latitude: null,
                BiginningAddress:
                {
                    Id: null,
                    Title: null,
                    CountryName: null,
                    EparchyId: null,
                    StateId: null,
                    CityName: null,
                    RegionName: null,
                    Address: null,
                    PostalCode: null
                },
                DestinationAddress:
                {
                    Id: null,
                    Title: null,
                    CountryName: null,
                    EparchyId: null,
                    StateId: null,
                    CityName: null,
                    RegionName: null,
                    Address: null,
                    PostalCode: null
                },
            };

        $scope.loadprocess = false;

        $scope.viewSelected = 0;
        $scope.locationshow = function (data) {
            $scope.viewSelected = data;
        }

        var toUTCDate = function (date) {
            var _utc = new Date(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds());
            return _utc;
        };
        var startrequest = {
            callback: function (val) {      //Mandatory
                if (typeof (val) === 'undefined') {
                } else {
                    $scope.request.StartTimeStr = toUTCDate(new Date(val * 1000));
                }
            },
            inputTime: $scope.request.StartTimeStr === (null) ? (((new Date()).getHours() * 60 * 60) + ((new Date()).getMinutes() * 60)) : $scope.request.StartTimeStr
        };
        $scope.opnestartRequest = function () {
            ionicTimePicker.openTimePicker(startrequest);
        };

        $scope.saveRequest = function () {
            $scope.request.StartTime = $filter('date')($scope.request.StartTimeStr, "HH:mm");
            $scope.request.RequestDate = $filter('date')($scope.request.RequestDate);
            $scope.request.ServiceDate = $scope.selectedYear + '/' + $scope.selectedMonth.number + '/' + $scope.selectedDay.day,
                $scope.loadprocess = true;
            $http({
                url: $scope.path + '/Request/Create',
                method: 'Post',
                data: {
                    model: $scope.request,
                }
            }).success(function (data) {
                $scope.loadprocess = false;
                if (data.success == true) {
                    alert("درخواست با موفقیت ثبت گردید");

                    $state.go("requestList");
                }
                if (data.success == false) {
                    $ionicScrollDelegate.scrollTop();

                    $scope.errors = data.errors;

                }
            }).error(function (data, status, headers, config) {

                $scope.loadprocess = false;
            });
        };
        $scope.getRequset = function () {
            $http({
                url: $scope.path + '/Request/CreateRequestMobile',
                method: 'Post',
                data: {
                    id: $scope.requestId,
                }
            }).success(function (data) {
                if (data != null) {
                    $scope.request = data;
                    startrequest.inputTime = (new Date(Number($scope.request.StartTimeStr)).getHours() * 60 * 60) + ((new Date(Number($scope.request.StartTimeStr))).getMinutes() * 60);
                    $scope.request.StartTime = $scope.request.StartTimeStr;
                    if ($scope.requestId != null) {
                        $scope.request.RequestDate = new Date($scope.request.RequestDateStr);
                    }

                    else {
                        changeEparchy();
                    }
                    MySpilit();

                }
            });
        };
        $scope.getRequset();
        $scope.changeEmergancy = function () {
            $scope.request.IsEmergancy = !$scope.request.IsEmergancy;
        }
        $scope.changeLocal = function () {
            $scope.request.IsLocal = !$scope.request.IsLocal;

        }
        $scope.$watch('request.IsLocal', function () {

            changeEparchy();
        });
        var changeEparchy = function () {
            if ($scope.request.IsLocal) {
                $scope.request.BiginningAddress.StateId = 5;
                $scope.request.BiginningAddress.EparchyId = 640;
                $scope.request.DestinationAddress.StateId = 5;
                $scope.request.DestinationAddress.EparchyId = 640;

            }
            else {
                $scope.request.BiginningAddress.StateId = 5;
                $scope.request.BiginningAddress.EparchyId = 480;
                $scope.request.DestinationAddress.StateId = 5;
                $scope.request.DestinationAddress.EparchyId = 480;
            }
        }

        $scope.selectedBiginningAddress = function (selected) {
            $scope.request.BiginningAddress.CountryName = selected.originalObject.CountryName;
            $scope.request.BiginningAddress.EparchyId = selected.originalObject.EparchyId;
            $scope.request.BiginningAddress.StateId = selected.originalObject.StateId;
            $scope.request.BiginningAddress.CityName = selected.originalObject.CityName;
            $scope.request.BiginningAddress.RegionName = selected.originalObject.RegionName;
            $scope.request.BiginningAddress.Address = selected.originalObject.Address;
            $scope.request.BiginningAddress.PostalCode = selected.originalObject.PostalCode;
        };

        $scope.selectedDestinationAddress = function (selected) {
            $scope.request.DestinationAddress.CountryName = selected.originalObject.CountryName;
            $scope.request.DestinationAddress.EparchyId = selected.originalObject.EparchyId;
            $scope.request.DestinationAddress.StateId = selected.originalObject.StateId;
            $scope.request.DestinationAddress.CityName = selected.originalObject.CityName;
            $scope.request.DestinationAddress.RegionName = selected.originalObject.RegionName;
            $scope.request.DestinationAddress.Address = selected.originalObject.Address;
            $scope.request.DestinationAddress.PostalCode = selected.originalObject.PostalCode;
        };
        $scope.backList = function () {
            $state.go("requestList");
        }
    }]);

////////////////////////////////////////Developed By Mehrdad Samadie: Ma hamonim ke mitonim///////////////////////////////////////////////////////
MyApp.filter('IsInRole', function () {
    return function (input, roles) {
        if (input == undefined || input == null || roles == null) {
            return false
        }
        var roleList = roles.split(",");
        for (var i = 0; i < input.length; i++) {
            for (var j = 0; j < roleList.length; j++) {
                if (input[i] == roleList[j]) {
                    return true;
                }
            }
        }
        return false;
    }
});
//MyApp.directive('bitaPresioncal', function () {
//    return {
//        restrict: 'E',
//        require: 'ngModel',
//        templateUrl: 'angularJs/directive/directive.html',
//        scope: {
//            modeldate: '=ngModel'
//        },
//        link: function ($scope, element, attr) {
//            $scope.calendar = [
//                []
//            ];
            //tmpdate = new Date();
            //tmppdate = toJalaali(tmpdate);
            //$scope.persian = pad(tmppdate.jy, 4) + '/' + pad(tmppdate.jm, 2) + '/' + pad(tmppdate.jd, 2);

            //$scope.pyear = $scope.$parent.$eval(attr.ngModel) == null ? parseInt($scope.persian.split('/')[0]) : parseInt($scope.$parent.$eval(attr.ngModel).split('/')[0]);
            //$scope.pmonth = $scope.$parent.$eval(attr.ngModel) == null ? parseInt($scope.persian.split('/')[1]) : parseInt($scope.$parent.$eval(attr.ngModel).split('/')[1]);
            //$scope.pday = $scope.$parent.$eval(attr.ngModel) == null ? parseInt($scope.persian.split('/')[2]) : parseInt($scope.$parent.$eval(attr.ngModel).split('/')[2]);

            //$scope.todayY = $scope.pyear;
            //$scope.todayM = $scope.pmonth;
            //$scope.todayD = parseInt($scope.persian.split('/')[2]);
            //$scope.daysofMonth = [];
            ////////set defult////////
            //$scope.persian = pad($scope.pyear, 4) + '/' + pad($scope.pmonth, 2) + '/' + pad($scope.pday, 2);
            //date = toGregorian($scope.pyear, $scope.pmonth, 1);
            //$scope.miladydate = new Date(date.gy, date.gm - 1, date.gd);

            //$(element).find("#persianCalendar1").val($scope.persian);
            //$(element).find("#persianCalendar1").attr("gdate", $scope.miladydate);




            ////////////////////////
            //$scope.regEx ='[0-9]{2}/[0-9]{2}/[0-9]{4}';

            //$scope.phoneNumberPattern = (function () {
            //    var regexp ='^\d{4}\-(0?[1-9]|1[012])\-(0?[1-9]|[12][0-9]|3[01])$';
            //    return {
            //        test: function (value) {
            //            if ($scope.requireTel === false) {
            //                return true;
            //            }
            //            return regexp.test(value);
            //        }
            //    };
            //})();






    //        $scope.fillCalendar = function () {
    //            date = toGregorian($scope.pyear, $scope.pmonth, 1);
    //            var gdate = new Date(date.gy, date.gm - 1, date.gd);
    //            var startday = (gdate.getDay() + 1) % 7;
    //            var maxday = jalaaliMonthLength($scope.pyear, $scope.pmonth);
    //            calWidth = 5;
    //            calHeight = 7;
    //            fillEmptyMap($scope.calendar, calWidth, calHeight);
    //            totalDaysMonth(maxday);
    //            day = 1;
    //            for (i = 0; i < 5; i++) {
    //                for (j = 0; j < 7; j++) {
    //                    if ((day <= maxday) && ((i == 0 && j >= startday) || i != 0)) {

    //                        $scope.calendar[i][j] = day;
    //                        day++;
    //                    }
    //                }
    //            }
    //        }
    //        $scope.fillCalendar();
    //        date = toGregorian($scope.pyear, $scope.pmonth, $scope.pday);
    //        $scope.miladydate = new Date(date.gy, date.gm - 1, date.gd);

    //        $scope.newDate = new Date();
    //        $scope.nextmonth = function () {
    //            if ($scope.pmonth < 12) {
    //                $scope.pmonth = $scope.pmonth + 1;
    //                $scope.persian = pad($scope.pyear, 4) + '/' + pad($scope.pmonth, 2) + '/' + pad($scope.pday, 2);
    //                $scope.fillCalendar();
    //                date = toGregorian($scope.pyear, $scope.pmonth, $scope.pday);
    //                $scope.miladydate = new Date(date.gy, date.gm - 1, date.gd);
    //            }
    //        };
    //        $scope.pervmonth = function () {
    //            if ($scope.pmonth > 1) {
    //                $scope.pmonth = $scope.pmonth - 1;
    //                $scope.persian = pad($scope.pyear, 4) + '/' + pad($scope.pmonth, 2) + '/' + pad($scope.pday, 2);
    //                $scope.fillCalendar();
    //                date = toGregorian($scope.pyear, $scope.pmonth, $scope.pday);
    //                $scope.miladydate = new Date(date.gy, date.gm - 1, date.gd);
    //            }
    //        }
    //        $scope.nextyear = function () {
    //            $scope.pyear = $scope.pyear + 1;
    //            $scope.persian = pad($scope.pyear, 4) + '/' + pad($scope.pmonth, 2) + '/' + pad($scope.pday, 2);
    //            $scope.fillCalendar();
    //            date = toGregorian($scope.pyear, $scope.pmonth, $scope.pday);
    //            $scope.miladydate = new Date(date.gy, date.gm - 1, date.gd);

    //        }
    //        $scope.pervyear = function () {
    //            $scope.pyear = $scope.pyear - 1;
    //            $scope.persian = pad($scope.pyear, 4) + '/' + pad($scope.pmonth, 2) + '/' + pad($scope.pday, 2);
    //            $scope.fillCalendar();
    //            date = toGregorian($scope.pyear, $scope.pmonth, $scope.pday);
    //            $scope.miladydate = new Date(date.gy, date.gm - 1, date.gd);
    //        }
    //        $scope.Today = function (persianCalendar, day) {
    //            $scope.pday = day;

    //            $scope.persian = pad($scope.todayY, 4) + '/' + pad($scope.todayM, 2) + '/' + pad($scope.todayD, 2);
    //            $scope.fillCalendar();
    //            date = toGregorian($scope.todayY, $scope.todayM, $scope.todayD);
    //            $scope.miladydate = new Date(date.gy, date.gm - 1, date.gd);
    //            $(element).find("#persianCalendar1").val($scope.persian);
    //            $(element).find("#persianCalendar1").attr("gdate", $scope.miladydate);
    //            $scope.ShowHide($("#persianCalendar"));
    //            $scope.modeldate = $scope.persian;
    //        }
    //        $scope.getday = function (persianCalendar, day) {
    //            if (day != '' && day != $scope.pday) {
    //                $scope.pday = day;

    //                $scope.persian = pad($scope.pyear, 4) + '/' + pad($scope.pmonth, 2) + '/' + pad($scope.pday, 2);
    //                $scope.fillCalendar();
    //                date = toGregorian($scope.pyear, $scope.pmonth, $scope.pday);
    //                $scope.miladydate = new Date(date.gy, date.gm - 1, date.gd);
    //             //   $(element).find("#persianCalendar1").val($scope.persian);
    //             //   $(element).find("#persianCalendar1").attr("gdate", $scope.miladydate);
    //                $scope.IsVisible = false;
    //                $scope.modeldate = $scope.persian;
    //                $scope.$apply();
    //            }
    //        }
    //        $scope.IsVisible = false;
    //        $scope.inputChanged = function () {
    //            tmp = $(element).find("#persianCalendar1").val().split('/');
    //            if (tmp.length == 3) {
    //                if (/[0-9]{4}(\/|-)(0[1-9]|1[0-2])(\/|-)(0[1-9]|[1-2][0-9]|3[0-1])$/.test($(element).find("#persianCalendar1").val())) {
    //                    $scope.persian = $(element).find("#persianCalendar1").val();
    //                    $scope.pyear = parseInt(tmp[0]);
    //                    $scope.pmonth = parseInt(tmp[1]);
    //                    $scope.getday($(element).find("#persianCalendar1"), parseInt(tmp[2]));
    //                }
    //            }

    //        }
    //        $scope.ShowHide = function (persianCalendar) {
    //            $scope.persian = $(element).find("#persianCalendar1").val();
    //            $scope.IsVisible = $scope.IsVisible ? false : true;
    //        }
    //        $scope.close = function () {

    //            $scope.IsVisible = false;
    //        }
    //        function fillEmptyMap(calendar, width, height) {
    //            for (var x = 0; x < width; x++) {
    //                calendar[x] = [];
    //                for (var y = 0; y < height; y++) {
    //                    calendar[x][y] = '';
    //                }
    //            }
    //        }
    //        function totalDaysMonth(maxday) {
    //            for (var x = 1; x <= maxday; x++) {
    //                $scope.daysofMonth.push(x);
    //            }
    //        }
    //        function pad(num, size) {
    //            var s = num + "";
    //            while (s.length < size) s = "0" + s;
    //            return s;
    //        }

    //    }
    //};

//});