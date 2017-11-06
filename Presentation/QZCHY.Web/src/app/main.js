'use strict';

/* Controllers */

angular.module('app').controller('AppCtrl', ['$scope', '$localStorage', '$window', '$rootScope', '$state', 'authService', function ($scope, $localStorage, $window, $rootScope, $state, authService) {
    // add 'ie' classes to html
    var isIE = !!navigator.userAgent.match(/MSIE/i);
    isIE && angular.element($window.document.body).addClass('ie');
    isSmartDevice($window) && angular.element($window.document.body).addClass('smart');

    $rootScope.apiUrl = 'http://localhost:8087/api/'
    $rootScope.baseUrl = 'http://localhost:8087/';
    //config
    $scope.app = {
        name: '衢州市级行政事业单位和市直国有企业房产土地管理系统',
        version: '1.0.0',
        // for chart colors
        color: {
            primary: '#7266ba',
            info: '#23b7e5',
            success: '#27c24c',
            warning: '#fad733',
            danger: '#f05050',
            light: '#e8eff0',
            dark: '#3a3f51',
            black: '#1c2b36'
        },
        settings: {
            themeID: 1,
            navbarHeaderColor: 'bg-black',
            navbarCollapseColor: 'bg-white-only',
            asideColor: 'bg-black',
            headerFixed: true,
            asideFixed: true,
            asideFolded: true,
            asideDock: false,
            container: false
        },   
        dateRangeConfig: {
            local: {
                "format": "YYYY年MM月DD日",
                "separator": " - ",
                "applyLabel": "应用",
                "cancelLabel": "取消",
                "fromLabel": "开始",
                "toLabel": "结束",
                "customRangeLabel": "自定义",
                "daysOfWeek": [
                    "日",
                    "一",
                    "二",
                    "三",
                    "四",
                    "五",
                    "六"
                ],
                "monthNames": [
                    "一月",
                    "二月",
                    "三月",
                    "四月",
                    "五月",
                    "六月",
                    "七月",
                    "八月",
                    "九月",
                    "十月",
                    "十一月",
                    "十二月"
                ],
                "firstDay": 1
            },
            predefinedRnages: function () {
                var range = {
                    '今天': [moment(), moment()],
                    '昨天': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                    '上周': [moment().subtract(6, 'days'), moment()],
                    '上30天': [moment().subtract(29, 'days'), moment()],
                    '本月': [moment().startOf('month'), moment().endOf('month')],
                    '下月': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                };

                return range;
            }
        }
    };

    //登录成功后推送用户名
    $scope.account = authService.authentication.account;
    $scope.nickName = authService.authentication.nickName;
    $scope.isAdmin = authService.authentication.isAdmin;
    $scope.$on('to-parent', function (e, data) {
        $scope.account = data.account;  //登录名
        $scope.nickName = data.nickName; //昵称
        $scope.isAdmin = data.isAdmin;
    });

    //#region 界面布局
    var windowHeight = $window.innerHeight; //获取窗口高度
    var headerHeight = 50;
    var footerHeight = 51;
    var windowWidth = $window.innerWidth; //获取窗口宽度

    $rootScope.dialogHeight = { "height": windowHeight - headerHeight - footerHeight - 230 };
    //#endregion

    //退出登录
    $scope.logOut = function () {
        authService.logOut();
        $state.go("access.signin")
    };

    //#region 设置对话框高度
    var windowHeight = $window.innerHeight; //获取窗口高度
    var headerHeight = 50;
    var footerHeight = 51;
    var windowWidth = $window.innerWidth; //获取窗口宽度

    $rootScope.dialogHeight = { "height": windowHeight - headerHeight - footerHeight - 230 };
    //#endregion

    //保存设置到Local storage
    if(angular.isDefined($localStorage.settings))
    {
        $scope.app.settings = $localStorage.settings;
    }
    else
    {
        $localStorage.settings = $scope.app.settings;
    }

    $scope.$watch('app.settings', function () {
        //保存到local storage
        $localStorage.settings = $scope.app.settings;
    }, true);

    function isSmartDevice($window) {
        // Adapted from http://www.detectmobilebrowsers.com
        var ua = $window['navigator']['userAgent'] || $window['navigator']['vendor'] || $window['opera'];
        // Checks for iOs, Android, Blackberry, Opera Mini, and Windows mobile devices
        return (/iPhone|iPod|iPad|Silk|Android|BlackBerry|Opera Mini|IEMobile/).test(ua);
    }
}]);