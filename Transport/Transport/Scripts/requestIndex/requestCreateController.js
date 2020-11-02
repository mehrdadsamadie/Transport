var app = angular.module("transport", [
    'ui.grid',
    'ui.grid.selection',
    'ui.grid.resizeColumns',
    'ui.grid.autoResize',
    'ui.grid.pagination',
    'angucomplete-alt',
    'ui.grid.selection',
    'ui.grid.exporter'
]);
app.config(['$locationProvider', function ($locationProvider)
{ $locationProvider.html5Mode({ enabled: true, requireBase: false }); }])
app.controller("requestCtrl", function ($scope, $http, $location, uiGridConstants, $window) {
    $scope.showServices = false;
    $scope.requestId;
    $scope.TotalCount = 0;
    $scope.Requests = [];
    $scope.nowdate;
    $scope.pagingOptions = {
        pageSizes: [50, 75, 100],
        pageSize: 50,
        currentPage: 0
    };
    var urlsend;
    $scope.role =
        {
            Admin: false,
            FactorManager: false,
            Transport: false
        };
    $scope.setRole = function (admin, factorManager, transport) {
        $scope.role.Admin = admin == "true" ? true : false;
        $scope.role.FactorManager = factorManager == "true" ? true : false;
        $scope.role.Transport = transport == "true" ? true : false;
        if ($scope.role.Admin == true || $scope.role.Transport == true) {
            $scope.gridOptions.columnDefs[12].visible = true;
        }
        else {
            $scope.gridOptions.columnDefs[12].visible = false;
        };
    }
    $scope.filter = {
        order: "ServiceDate",
        isAsc: true,
        startDate: null,
        endDate: null,
        startTime: null,
        endTime: null,
        managerStatus: null,
        transportStatus: null,
        isDelete: null,
        isCancel: null,
        RequestPersonel: null
    }

    $scope.getRequests = function () {
        $scope.load = true;
        $http({
            url: '/Request/List',
            method: 'Post',
            data: {
                startDate: $scope.filter.startDate,
                endDate: $scope.filter.endDate,
                startTime: $scope.filter.startTime,
                endTime: $scope.filter.endTime,
                managerStatus: $scope.filter.managerStatus,
                transportStatus: $scope.filter.transportStatus,
                isDelete: $scope.filter.isDelete,
                isCancel: $scope.filter.isCancel,
                pageNumber: $scope.pagingOptions.currentPage,
                pageSize: $scope.pagingOptions.pageSize,
                RequestPersonel: $scope.filter.RequestPersonel,
                order: $scope.filter.order,
                isAsc: $scope.filter.isAsc
            }
        }).success(function (data) {
            if (data != "") {
                $scope.Requests = data.Request;
                $scope.TotalCount = data.TotalCount;
                $scope.gridOptions.totalItems = data.TotalCount;
            }
            else {
                $scope.Requests.length = 0;
                $scope.TotalCount = 0;
                $scope.gridOptions.totalItems = 0;
            }
            $scope.load = false;
        }).error(function (data, status, headers, config) {
            $scope.load = false;
        });
    };
    $scope.nowdate = null;
    $scope.isToday = null;
    $scope.workTime = null;
    $scope.startTime = null;
    $scope.endTime = null;
    $scope.nextdate = null;

    function getURL() {
        $scope.workTime = $location.search().worktime == undefined ? null : $location.search().worktime;
        $scope.isToday = $location.search().today == undefined ? null : $location.search().today;
        $scope.filter.startDate = $location.search().startDate == undefined ? null : $location.search().startDate;
        $scope.filter.endDate = $location.search().endDate == undefined ? null : $location.search().endDate;
        $scope.filter.startTime = $location.search().startTime == undefined ? null : $location.search().startTime;
        $scope.filter.endTime = $location.search().endTime == undefined ? null : $location.search().endTime;
    }
    getURL();

    $scope.setTime = function (time, nextdate, startTime, endTime) {
        $scope.nowdate = time;
        $scope.startTime = startTime;
        $scope.endTime = endTime;
        $scope.nextdate = nextdate;
    }

    $scope.exportToExcel = function () {
        var url = "/Request/ExportData?startDate=" + ($scope.filter.startDate == null ? "" : $scope.filter.startDate) + "&endDate=" + ($scope.filter.endDate == null ? "" : $scope.filter.endDate) + "&startTime=" + ($scope.filter.startTime == null ? "" : $scope.filter.startTime) + "&endTime=" + ($scope.filter.endTime == null ? "" : $scope.filter.endTime) + "&managerStatus=" + $scope.filter.managerStatus + "&transportStatus=" + $scope.filter.transportStatus + "&isDelete=" + $scope.filter.isDelete + "&isCancel=" + $scope.filter.isCancel + "&RequestPersonel=" + $scope.filter.RequestPersonel + "&order=" + $scope.filter.order + "&isAsc=" + $scope.filter.isAsc;
        $window.open(url);
    }

    $scope.changeURL = function () {
        var url =
            '?startDate=' + ($scope.filter.startDate == (undefined || null) ? '' : $scope.filter.startDate)
            + '&endDate=' + ($scope.filter.endDate == (undefined || null) ? '' : $scope.filter.endDate)
            + '&startTime=' + ($scope.filter.startTime == (undefined || null) ? '' : $scope.filter.startTime)
            + '&endTime=' + ($scope.filter.endTime == (undefined || null) ? '' : $scope.filter.endTime)
            + '&workTime=' + ($scope.workTime == (undefined || null) ? '' : $scope.workTime)
            + '&today=' + ($scope.isToday == (undefined || null) ? '' : $scope.isToday);
        urlsend = url;
        $location.url(url);
        $location.replace();
        $window.history.pushState(null, 'any', $location.absUrl());
    }

    $scope.gridOptions =
        {
            paginationPageSizes: $scope.pagingOptions.pageSizes,
            paginationPageSize: $scope.pagingOptions.pageSize,
            useExternalPagination: true,
            showGridFooter: true,
            showColumnFooter: true,
            enableFiltering: true,
            enableGridMenu: false,
            data: 'Requests',
            columnDefs: [{
                field: 'Id',
                displayName: 'Id',
                visible: false,

            }, {
                field: 'PersonelName',
                displayName: 'نام درخواست کننده',
                width: 165,
                enableFiltering: false,
                cellClass: 'center',
                cellTemplate: '<a class="ngCellText btn-link" ng-cell-text ng-class="col.colIndex()" ng-click="grid.appScope.editRequest(row.entity.Id)"  >{{row.entity.PersonelName}}</a>',


            },
            {
                name: 'RequestDate',
                displayName: 'تاریخ درخواست',
                width: 120,
                enableFiltering: false,
                cellClass: 'center',
            },
            {
                field: 'ServiceDate',
                displayName: 'تاریخ سرویس',
                width:85,
                cellClass: 'center',
                enableFiltering: false,
            },
            {
                field: 'StartTime',
                displayName: 'زمان شروع',
                width: 75,
                visible: true,
                cellClass: 'center',
                enableFiltering: false,
                enableSorting: false,

            }, {
                field: 'Biginning',
                displayName: 'مبدا',
                visible: true,
                width: 205,
                enableFiltering: false,
                enableSorting: false,

            }, {
                field: 'Destination',
                displayName: 'مقصد',
                width: 205,
                enableFiltering: false,
                enableSorting: false,
            }, {
                field: 'IsLocal',
                displayName: 'درون شهرک',
                width:50,
                cellClass: 'center',
                cellTemplate: '<div class="ngCellText" ng-cell-text ng-class="col.colIndex()"><i ng-show="COL_FIELD" class="fa fa-industry text-success"></i><i ng-show="!COL_FIELD" class="fa fa-globe text-warning"></i></div>',
                enableFiltering: false,
                enableSorting: false,
            }, {
                field: 'IsAcceptManager',
                displayName: 'مدیر',
                width: 50,
                cellClass: 'center',
                cellTemplate: '<a ng-if="(grid.appScope.role.Admin==true || grid.appScope.role.Transport==true || grid.appScope.role.FactorManager==true)" ng-click="grid.appScope.confirmrequest(row.entity.Id,row.entity.IsAcceptManager)"><span style="cursor:pointer" ng-if="row.entity.IsAcceptManager ==1"><i class="fa fa-times text-danger"></i></span><span style="cursor:pointer" ng-if="row.entity.IsAcceptManager ==0"><i class="fa fa-minus"></i></span><span style="cursor:pointer" ng-if="row.entity.IsAcceptManager ==2"><i class="fa fa-check text-success"></i></span></a><div ng-if="!(grid.appScope.role.Admin==true || grid.appScope.role.Transport==true)"><span ng-if="row.entity.IsAcceptManager ==1"><i class="fa fa-times text-danger"></i></span><span ng-if="row.entity.IsAcceptManager ==0"><i class="fa fa-minus"></i></span><span ng-if="row.entity.IsAcceptManager ==2"><i class="fa fa-check text-success"></i></span></div>',
                enableFiltering: true,
                filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div my-custom-modal></div></div>',
                enableSorting: false,
            }, {
                field: 'IsAcceptTranasport',
                displayName: 'نقلیه',
                width: 50,
                cellClass: 'center',
                cellTemplate: '<a ng-if="(grid.appScope.role.Admin==true || grid.appScope.role.Transport==true || grid.appScope.role.FactorManager==true)" ng-click="grid.appScope.confirmrequest(row.entity.Id,row.entity.IsAcceptTranasport)"><span ng-if="row.entity.IsAcceptTranasport ==1" style="cursor:pointer"><i class="fa fa-times text-danger"></i></span><span ng-if="row.entity.IsAcceptTranasport ==0" style="cursor:pointer"><i class="fa fa-minus"></i></span><div ng-if="row.entity.IsAcceptTranasport ==2" style="cursor:pointer"><i class="fa fa-check text-success"></i></span></a><div ng-if="!(grid.appScope.role.Admin==true || grid.appScope.role.Transport==true)" ><span ng-if="row.entity.IsAcceptTranasport ==1"><i class="fa fa-times text-danger"></i></span><span ng-if="row.entity.IsAcceptTranasport ==0"><i class="fa fa-minus"></i></span><div ng-if="row.entity.IsAcceptTranasport ==2"><i class="fa fa-check text-success"></i></span></a>',
                enableFiltering: true,
                filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div my-custom-modal></div></div>',
                enableSorting: false,

            },
            //{
            ////    field: 'IsEmergancy',
            ////    displayName: 'ضروری',
            ////    width: 50,
            ////    cellClass: 'center',
            ////    cellTemplate: '<div class="ngCellText" ng-cell-text ng-class="col.colIndex()"><i ng-show="COL_FIELD" class="fa fa-check text-success"></i><i ng-show="!COL_FIELD" class="fa fa-times text-danger"></i></div>',
            ////    enableSorting: false,
            ////    enableFiltering: false,
            ////},
            {
                field: 'IsCanceled',
                displayName: 'لغوشده',
                width: 50,
                cellClass: 'center',
                cellTemplate: '<a ng-if="row.entity.Cancelable==true" ng-click="grid.appScope.confirmCanceled(row.entity.Id)" class="ngCellText link" ng-cell-text ng-class="col.colIndex()"><i ng-show="!COL_FIELD" class="fa fa-times text-danger"></i><i ng-show="COL_FIELD" class="fa fa-check text-success"></i></a><div ng-if="row.entity.Cancelable==false" class="ngCellText" ng-cell-text ng-class="col.colIndex()"><i ng-show="!COL_FIELD" class="fa fa-times text-muted"></i><i ng-show="COL_FIELD" class="fa fa-check text-muted"></i></div>',
                enableSorting: false,
                filter: {
                    type: uiGridConstants.filter.SELECT,
                    selectOptions: [{ value: true, label: 'لغوشده ها' }, { value: false, label: 'لغونشده ها' }]
                },
            },
            {
                field: 'IsDelete',
                displayName: 'حذف',
                width: 50,
                cellClass: 'center',
                cellTemplate: '<a ng-if="row.entity.Deleteable==true"  ng-click="grid.appScope.deleteRequest(row.entity.Id)"  class="ngCellText"  ng-cell-text ng-class="col.colIndex()"><i ng-show="COL_FIELD" style="cursor:pointer" class="fa fa-check text-success"></i><i style="cursor:pointer" ng-show="!COL_FIELD" class="fa fa-times text-danger"></i></a><div ng-if="row.entity.Deleteable==false" class="ngCellText" ng-cell-text ng-class="col.colIndex()"><i ng-show="COL_FIELD" class="fa fa-check text-muted"></i><i ng-show="!COL_FIELD" class="fa fa-times text-muted"></i></div>',
                enableSorting: false,
                filter: {
                    type: uiGridConstants.filter.SELECT,
                    selectOptions: [{ value: true, label: 'حذف شده ها' }, { value: false, label: 'حذف نشده ها' }]
                },
            },
            {
                field: 'انتخاب سرویس',
                displayName: ' سرویس',
                width: 140,
                cellClass: 'center',
                cellTemplate: '<div ng-if="row.entity.ServiceId==null"><div class="btn btn-link" data-toggle="modal" data-target="#exampleModal" id="openPopUp" ng-click="grid.appScope.getActiveServices(row.entity.Id,row.entity.ServiceDate)">انتخاب</div>|<a  class="btn btn-link" ng-click="grid.appScope.convertRequestToService(row.entity.Id)">ایجاد</a></div><div ng-if="row.entity.ServiceId!=null"><a  class="btn btn-link " ng-click="grid.appScope.goToService(row.entity.ServiceId)">مشاهده</a>|<a class="btn btn-link " ng-click="grid.appScope.removeSerice(row.entity.Id)">حذف</a></div>',
                enableSorting: false,
                enableFiltering: false,
            },
            ],
            enableSelectAll: true,
            jqueryUITheme: true,
            showFooter: true,
            width: 1080,
            rowHeight: 30,
            enableColumnMenus: false,
            enableGridMenu: false,
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, $scope.sortChanged);
                $scope.gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    $scope.pagingOptions.currentPage = newPage - 1;
                    $scope.pagingOptions.pageSize = pageSize;
                    $scope.getRequests();

                });

                $scope.gridApi.core.on.filterChanged($scope, function () {
                    var grid = this.grid;
                    if (grid.columns[9].filters[0].term != undefined && grid.columns[9].filters[0].term != "") {
                        $scope.filter.managerStatus = grid.columns[9].filters[0].term.split(',').map(Number);
                    } else if (grid.columns[9].filters[0].term === undefined || grid.columns[9].filters[0].term == "") {
                        $scope.filter.managerStatus = null;
                    };
                    if (grid.columns[10].filters[0].term != undefined && grid.columns[10].filters[0].term != "") {
                        $scope.filter.transportStatus = grid.columns[10].filters[0].term.split(',').map(Number);
                    } else if (grid.columns[10].filters[0].term === undefined || grid.columns[10].filters[0].term === "") {
                        $scope.filter.transportStatus = null;

                    }
                    if (grid.columns[12].filters[0].term !== undefined) {
                        $scope.filter.isCancel = grid.columns[12].filters[0].term;
                    }
                    if (grid.columns[13].filters[0].term !== undefined) {
                        $scope.filter.isDelete = grid.columns[13].filters[0].term;
                    }
                    $scope.reSet();
                });

            }
        };
    $scope.getActiveServices = function (id,date) {
        $(window).scrollTop(0);
        $scope.showServices = true;
        $scope.requestId = id;
        $http({
            url: '/Request/GetActiveServices',
            method: 'Post',
            data: {
                date: date
            },

        }).then(function (success) {
            if (success.data != null) {
                $scope.services = success.data;

            }
        }, function (error) {
            $scope.showServices = false;
            $('.modal').css("display", "none")
        });
    };
    $scope.goToService = function (id)
    {
        $window.location = "/manage/service/Create/" + id; 
    }
    /******************if roles****************/

    $scope.confirmrequest = function (requestId, status) {

        var arr = [];
        if (requestId == null) {
            var temp = $scope.gridApi.selection.getSelectedRows();
            for (var i = 0; i < temp.length; i++) {
                arr.push(temp[i].Id);
            }
        }
        else {
            arr = [requestId];
        }
        if (arr.length == 0) {
            alert("ایتمی انتخاب نشده است");
        }
        if (status == 0) {
            status = true
        }
        else if (status == 1) {
            status = true;
        }
        else {
            status = false;
        }

        $http({
            url: '/Request/Confirm',
            method: 'Post',
            data: {
                requests: arr,
                isAccept: status
            }
        }).success(function (data) {
            if (data.result == true) {
                if (status) {
                    alert("تایید با موفقیت صورت گرفت");
                }
                else {
                    alert(" عدم تایید با موفقیت صورت گرفت");
                }
                $scope.getRequests();
            }
            else {
                alert(data.Message);
            }
        });
    }
    $scope.confirmCanceled = function (requestId) {
        var r = confirm("ایا مطمئن به لغو این درخواست می باشید؟");
        if (r == true) {
            $http({
                url: '/Request/Cancel',
                method: 'Post',
                data: {
                    id: requestId,
                }
            }).success(function (data) {
                if (data.success == true) {
                    alert("درخواست لغو شد");
                    $scope.getRequests();

                }
                else {
                    alert(data.message);
                }
            });
        }
    }
    $scope.deleteRequest = function (data) {


        var r = confirm("ایا مطمئن به حذف این آیتم از لیست می باشید؟");
        if (r == true) {
            $http({
                url: '/Request/Delete',
                method: 'Post',
                data: {
                    id: data,
                    isMobile: true
                }
            }).success(function (data) {
                if (data == true) {
                    alert("حذف با موفقیت صورت گرفت");
                    $scope.getRequests();
                }
                else {
                    alert("خطا در پاک کردن  !")
                }
            });
        };


    };
    $scope.removeSerice = function (data) {
        var r = confirm("ایا مطمئن به حذف این سرویس از این درخواست می باشید؟");
        if (r == true) {
            $http({
                url: '/Request/RemoveService',
                method: 'Post',
                data: {
                    requestId: data,
                }
            }).success(function (data) {
                if (data.Result == true) {
                    alert("حذف با موفقیت صورت گرفت");
                    $scope.getRequests();
                }
                else {
                    alert(data.Message);
                }
            });
        };


    };
    $scope.setSort = function (sortColumns, direction) {
        switch (sortColumns) {
            case "ServiceDate":
                {
                    $scope.filter.order = "ServiceDate";
                    $scope.filter.isAsc = direction
                }
                break;
            case "RequestDate":
                {
                    $scope.filter.order = "RequestDate";
                    $scope.filter.isAsc = direction
                }
                break;
            case "PersonelName":
                {
                    $scope.filter.order = "PersonelName";
                    $scope.filter.isAsc = direction
                }
                break;
            default:
                {
                    $scope.filter.order = "RequestDate";
                    $scope.filter.isAsc = direction
                };


        }
        $scope.getRequests();
    }

    $scope.reSet = function () {
        $scope.filter.order = 2;
        $scope.filter.isAsc = true;
        $scope.pagingOptions.currentPage = 0;
        $scope.Requests.length = 0;
        $scope.TotalCount = 0;
        $scope.gridOptions.totalItems = 0;
        $scope.getRequests();
        $scope.changeURL();
    }
    $scope.sortChanged = function (grid, sortColumns) {
        if (sortColumns.length === 0) {
            return;
        } else {
            switch (sortColumns[0].sort.direction) {
                case uiGridConstants.ASC:
                    $scope.setSort(sortColumns[0].name, true);
                    break;
                case uiGridConstants.DESC:
                    $scope.setSort(sortColumns[0].name, false);
                    break;
                case undefined:
                    $scope.setSort(sortColumns[0].name, true);
                    break;
            }
        }
    };
    $scope.beforeserviceId = null;
    $scope.selectedService = function (data) {
        $scope.beforeserviceId = data;
    }

    $scope.addServiceToRequest = function () {
        if ($scope.beforeserviceId == null) {
            alert("سرویسی انتخاب نشده");
            return;
        }
        $scope.showServices = false;
        $('.modal').css("display", "none")
        $http({
            url: '/Request/AddServiceToRequest',
            method: 'Post',
            data: {
                requestId: parseInt($scope.requestId),
                serviceId: parseInt($scope.beforeserviceId)
            },
        }).then(function (success) {
            if (success.data.Result == true) {
                window.location.reload();
            }
            else {
                alert(success.data.Message);
            }
            $scope.requestId = null;
        }, function (error) {
            $scope.requestId = null;
        });
    };
    $scope.$watch('filter.startDate', function (newVal, oldVal) {
        if (newVal === oldVal && oldVal === null) {
            return;
        }
        if (newVal !== undefined) {
            if (newVal == "") {
                $scope.filter.startDate = null;
            }
            $scope.reSet();

        }
    });
    $scope.$watch('filter.endDate', function (newVal, oldVal) {
        if (newVal == oldVal && oldVal === null) {
            return;
        }
        if (newVal !== undefined) {
            if (newVal == "") {
                $scope.filter.endDate = null;
            }
            $scope.reSet();
        }

    });


    $scope.$watch('nowdate', function (newVal, oldVal) {
        if ($scope.isToday==="true") {
            if ($scope.workTime==="true") {
                $scope.filter.startTime = $scope.startTime;
                $scope.filter.endTime = $scope.endTime;
            }
            else {

                $scope.filter.startTime = $scope.endTime;
                $scope.filter.endTime = "23:59";
            }
            $scope.filter.startDate = newVal;
            $scope.filter.endDate = newVal;
        }
        else if ($scope.isToday === "false") {
            $scope.filter.startDate = $scope.nextdate;
            $scope.filter.endDate = $scope.nextdate;
        }
        else
        {
            $scope.filter.startDate = $scope.filter.startDate == null ? newVal : $scope.filter.startDate;
            $scope.filter.endDate = $scope.filter.endDate == null ? newVal : $scope.filter.endDate;
        }
    });

    $scope.selectedPersonnel = function (selected) {
        if (selected != undefined) {
            if (selected.originalObject.Id != null) {
                $scope.filter.RequestPersonel = selected.originalObject.Id;
            }
            else {
                $scope.filter.RequestPersonel = null;
            }
        }
        else {
            $scope.filter.RequestPersonel = null;
        }
        $scope.reSet();
    };
    $scope.convertRequestToService = function(id)
    {
        urlsend = $window.location.hostname + "/Request/Index/" + urlsend
        $window.location = "/manage/service/ConvertRequestToService/" + id + "?returnUrl=" + $window.location.href; 
    }
    $scope.editRequest = function (id)
    {
        $window.location = "/Request/Create/" + id;
    }
});

app.controller('myCustomModalCtrl', function ($scope, $compile, $timeout) {
    var $elm;

    $scope.showAgeModal = function () {
        // $scope.listOfAges = [0, 1, 2];
        $scope.listOfAges = [{
            value: 0,
            label: "بررسی نشده"
        },
        {
            value: 1,
            label: "تایید نشده"
        },
        {
            value: 2,
            label: "تایید شده"
        },
        ];


        $scope.listOfAges.sort();

        $scope.gridOptions = {
            data: [],
            enableColumnMenus: false,
            columnDefs: [{
                field: 'age',
                displayName: 'وضعیت',
            }],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;

                if ($scope.colFilter && $scope.colFilter.listTerm) {
                    $timeout(function () {
                        $scope.colFilter.listTerm.forEach(function (age) {
                            var entities = $scope.gridOptions.data.filter(function (row) {
                                var status;
                                switch (age) {
                                    case "0":
                                        {
                                            status = "بررسی نشده";
                                        }
                                        break;
                                    case "1":
                                        {
                                            status = "تایید نشده";
                                        }
                                        break;
                                    case "2":
                                        {
                                            status = "تایید شده";
                                        }
                                        break;
                                }
                                return row.age === status;
                            });

                            if (entities.length > 0) {
                                $scope.gridApi.selection.selectRow(entities[0]);
                            }
                        });
                    });
                }
            }

        };

        $scope.listOfAges.forEach(function (age) {
            $scope.gridOptions.data.push({ age: age.label });
        });

        var html = '<div class="modal" ng-style="{display: \'block\'}"><div class="modal-dialog"><div class="modal-content"><div class="modal-header">وضعیت تایید</div><div class="modal-body"><div id="grid1" ui-grid="gridOptions" ui-grid-selection class="modalGrid"></div></div><div class="modal-footer"><button id="buttonClose" class="btn btn-primary" ng-click="close()">فیلتر</button></div></div></div></div>';
        $elm = angular.element(html);
        angular.element(document.body).prepend($elm);

        $compile($elm)($scope);

    };

    $scope.close = function () {
        var ages = $scope.gridApi.selection.getSelectedRows();
        $scope.colFilter.listTerm = [];

        ages.forEach(function (age) {
            var status;
            switch (age.age) {
                case "بررسی نشده":
                    {
                        status = "0";
                    }
                    break;
                case "تایید نشده":
                    {
                        status = "1";
                    }
                    break;
                case "تایید شده":
                    {
                        status = "2";
                    }
                    break;
            }
            $scope.colFilter.listTerm.push(status);
        });

        $scope.colFilter.term = $scope.colFilter.listTerm.join(', ');
        $scope.colFilter.condition = new RegExp($scope.colFilter.listTerm.join('|'));

        if ($elm) {
            $elm.remove();
        }
    };
});


app.directive('myCustomModal', function () {
    return {
        template: '<label>{{colFilter.term}}</label><button ng-click="showAgeModal()">...</button>',
        controller: 'myCustomModalCtrl'
    };
});
app.directive('persiandatepicker', function () {
    return {
        restrict: 'AE',
        require: '?ngModel',
        replace: true,
        link: function (scope, element, attrs, ngModel) {
            //if (!ngModel) return;

            element.bind("click", function () {
                PersianDatePicker.Show(this, scope.nowdate);
            });

            element.context.onchange = function () {
                scope.filter[this.name] = this.value;
                scope.$apply();
            };
            element.bind("change", function () {
                scope.filter[this.name] = this.value;
                scope.$apply();
            });
        }
    };
});
app.filter('sumByColumn', function () {
    return function (collection, column) {
        var total = 0;

        collection.forEach(function (item) {
            total += parseInt(item[column]);
        });

        return total;
    };
});