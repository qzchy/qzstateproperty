"use strict";

app.controller("DashboardCtrl", ['$scope', '$rootScope', 'StatisticsService', '$timeout', '$state', '$filter','authService',
function ($scope, $rootScope, statisticsService, $timeout, $state, $filter, authService) {
    $scope.isAdmin = authService.authentication.isAdmin;
    $scope.onlyShowSelf = false;
    $scope.overviewStatistics = null;
    $scope.groupStatisticsOption = {
        propertyType: 0,
        statisticsField: "ConstructArea"
    };
    $scope.groupStatistics = {
        data: [],
        dataTotal: function () {
            var data = [];
            if ($scope.groupStatistics.data.length == 0) return data;
            for (var j = 0; j < $scope.groupStatistics.data[0].length; j++) {
                data.push($scope.groupStatistics.data[0][j] + $scope.groupStatistics.data[1][j] + $scope.groupStatistics.data[2][j]);
            }

            return data;
        }
    };
    $scope.currentUsageStatistics = null;
    $scope.s = true;

    $scope.statistics = function (showSelf) {
        $scope.onlyShowSelf = showSelf;
        $scope.statisticsOverView(showSelf);
        $scope.statisticsGroup(showSelf);
        $scope.statisticsCurrentUasge(showSelf);

    };

    $scope.statisticsOverView = function (showSelf) {
        //加载汇总统计
        statisticsService.getOverviewStatistics(showSelf, true).then(function (data) {
            $scope.overviewStatistics = data;
            $scope.s = false;
        }, function (data) {
            console.log(data);
        });
    };
    $scope.statisticsGroup = function (showSelf) {
        //加载汇总统计
        statisticsService.getGroupStatistics(showSelf, true, $scope.groupStatisticsOption.propertyType, $scope.groupStatisticsOption.statisticsField).then(function (data) {

            $scope.groupStatistics.data = data;

            myChart0.setOption({
                series: [{ data: data[0] },
               {
                   data: data[1]
               },
               {
                   data: data[2]
               },
               {
                   data: $scope.groupStatistics.dataTotal(),
               }]
            });

        }, function (data) {
            console.log(data);
        });
    };
    $scope.statisticsCurrentUasge = function (showSelf) {
        //加载使用现状统计
        statisticsService.getCurrentUsageStatistics(showSelf, true).then(function (data) {

            myChart1.setOption({ series: data[0] });
            myChart2.setOption({ series: data[1] });

        }, function (data) {
            console.log(data);
        });
    };

    $scope.statistics($scope.onlyShowSelf);

    // 基于准备好的dom，初始化echarts实例


    //#region 按区域分组 单位性质统计

    var myChart0 = echarts.init(document.getElementById('chart0'));
    var option = {
        tooltip: {
            show: true,
            trigger: 'axis',
            axisPointer: {
                type: 'shadow',
                textStyle: {
                    color: '#fff',
                }

            },
            formatter: function (columns) {
                var html = "";

                if (columns.length >= 0) {
                    html += columns[0].name + '<br>';

                    angular.forEach(columns, function (i, v) {
                        html += '<span style="display:inline-block;margin-right:5px;border-radius:10px;width:9px;height:9px;background-color:' +
                            i.color + '"></span>' + i.seriesName + ' : ' + $filter('unit')(i.data, $scope.getStatisticsFieldUnit()) + '<br>';
                    });
                }


                return html;
            }
        },
        legend: {
            data: ['行政单位', '事业单位', '国有企业', '合计']
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        yAxis: {
            type: 'value'
        },
        xAxis: {
            type: 'category',
            data: ['老城区（含南区）', '西区', '集聚区', '柯城区', '衢江区', '其他区域']
        },
        series: [
              {
                  type: 'bar',
                  name: '行政单位',
                  stack: '总量',
                  data: $scope.groupStatistics.data[0],
                  label: {
                      normal: {
                          show: true,
                          formatter: function (column) {
                              if (column.data == 0) return "";
                              else return $filter('unit')(column.data, $scope.getStatisticsFieldUnit());
                          }
                      },
                      emphasis: {
                          textStyle: {
                              color: '#fff',
                          },
                      },
                  },
              },
            {
                type: 'bar',
                name: '事业单位',
                stack: '总量',
                data: $scope.groupStatistics.data[1],
                label: {
                    normal: {
                        show: true,
                        formatter: function (column) {
                            if (column.data == 0) return "";
                            else return $filter('unit')(column.data, $scope.getStatisticsFieldUnit());
                        }
                    },
                    emphasis: {
                        textStyle: {
                            color: '#fff',
                        },
                    },
                },
            },
            {
                type: 'bar',
                name: '国有企业',
                stack: '总量',
                data: $scope.groupStatistics.data[2],
                label: {
                    normal: {
                        show: true,
                        formatter: function (column) {
                            if (column.data == 0) return "";
                            else return $filter('unit')(column.data, $scope.getStatisticsFieldUnit());
                        }
                    },
                    emphasis: {
                        textStyle: {
                            color: '#fff',
                        },
                    },
                },
            },
            {
                type: 'line',
                name: '合计',
                data: $scope.groupStatistics.dataTotal(),
                label: {
                    normal: {
                        show: true,
                        formatter: function (column) {
                            return $filter('unit')(column.data, $scope.getStatisticsFieldUnit());
                        },
                        position: 'insideTop',
                        offset: [0, -30],
                        textStyle: {
                            color: '#7266ba',
                        },
                    },
                    emphasis: {
                        textStyle: {
                            color: '#23b7e5',
                        },
                    },
                },
                symbolSize: 8,
                itemStyle: {
                    normal: {
                        barBorderRadius: 0,
                        label: {
                            show: false,
                            position: "top",
                            formatter: function (p) {
                                return p.value > 0 ? (p.value) : '';
                            }
                        }
                    }
                },
                lineStyle: {
                    normal: {
                        color: '#7266ba',
                        width: 3,
                    },
                },
            },
        ]
    };

    // 使用刚指定的配置项和数据显示图表。
    myChart0.setOption(option);
    //#endregion

    var myChart1 = echarts.init(document.getElementById('chart1'));

    var option1 = {
        tooltip: {
            trigger: 'item',
            formatter: function (column) {
                var html = column.seriesName + '<br>';
                html += column.name + '：' + $filter('unit')(column.value, "carea") + ' (' + column.percent + '%)';
                return html;
            }
        },
        toolbox: {
            show: true,
            feature: {
                mark: { show: true },
                dataView: { show: true, readOnly: true },
                magicType: {
                    show: true,
                    type: ['pie', 'funnel']
                },
                //restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        legend: {
            orient: 'vertical',
            left: 'left',
            data: ['自用', '出租', '出借', '闲置']
        },
        series: [
            {
                name: '使用现状',
                type: 'pie',
                radius: '55%',
                center: ['50%', '60%'],
                data: [],
                itemStyle: {
                    emphasis: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }
            }
        ],
        color: ['#c23531', '#2f4554', '#61a0a8', '#749f83']
    };

    myChart1.setOption(option1);
    var myChart2 = echarts.init(document.getElementById('chart2'));
    var option1 = {
        tooltip: {
            trigger: 'item',
            formatter: function (column) {
                var html = column.seriesName + '<br>';
                html += column.name + '：' + $filter('unit')(column.value, "larea") + ' (' + column.percent + '%)';
                return html;
            }
        },
        toolbox: option1.toolbox,
        legend:option1.legend,
        series: option1.series,
        color: option1.color
    };
    myChart2.setOption(option1);

    $scope.groupStatisticsOptionChange = function (onlyShowSelf) {
        if ($scope.groupStatisticsOption.propertyType != "0") {
            if ($scope.groupStatisticsOption.statisticsField == "ConstructArea")
                $scope.groupStatisticsOption.statisticsField = "LandArea";
        }
        else {
            if ($scope.groupStatisticsOption.statisticsField != "ConstructArea")
                $scope.groupStatisticsOption.statisticsField = "ConstructArea";
        }

        $scope.statisticsGroup(onlyShowSelf);
    };

    $scope.getStatisticsFieldAlias = function () {
        switch ($scope.groupStatisticsOption.statisticsField) {
            case "ConstructArea":
                return "建筑面积";
            case "LandArea":
                return "土地面积";
            case "Price":
                return "账面价值";
            case "Count":
                return "数量";
        }
    };

    $scope.getStatisticsFieldUnit = function () {
        switch ($scope.groupStatisticsOption.statisticsField) {
            case "ConstructArea":
                return "carea";
            case "LandArea":
                return "larea";
            case "Price":
                return "price";
            case "Count":
                return "count";
        }
    };
    //        var mychart3 = echarts.init(document.getElementById('chart3'));
    //        var option3 = {
    //            title: {
    //                text: '堆叠区域图'
    //            },
    //            tooltip: {
    //                trigger: 'axis',
    //                axisPointer: {
    //                    type: 'cross',
    //                    label: {
    //                        backgroundColor: '#6a7985'
    //                    }
    //                }
    //            },
    //            legend: {
    //                data: ['房产数目', '房产建筑面积', '土地面积', '账面价值']
    //            },
    //            //toolbox: {
    //            //    feature: {
    //            //        saveAsImage: {}
    //            //    }
    //            //},
    //            grid: {
    //                left: '3%',
    //                right: '4%',
    //                bottom: '3%',
    //                containLabel: true
    //            },
    //            xAxis: [
    //                {
    //                    type: 'category',
    //                    boundaryGap: false,
    //                    data: ['60年代之前', '70年代', '80年代', '90年代', '00年代', '10年代']
    //                }
    //            ],
    //            yAxis: [
    //                {
    //                    type: 'value'
    //                }
    //            ],
    //            series: [
    //                {
    //                    name: '房产数目',
    //                    type: 'line',
    //                    stack: '总量',
    //                    areaStyle: { normal: {} },
    //                    data: [669,
    //721,
    //838,
    //1362,
    //3626,
    //6481,
    //                ]
    //                },
    //                {
    //                    name: '房产建筑面积',
    //                    type: 'line',
    //                    stack: '总量',
    //                    areaStyle: { normal: {} },
    //                    data: [804101.36,
    //816646.45,
    //864827.62,
    //1074592.41,
    //2412045.35,
    //3741226.39
    //                ]
    //                },
    //                {
    //                    name: '土地面积',
    //                    type: 'line',
    //                    stack: '总量',
    //                    areaStyle: { normal: {} },
    //                    data: [26556273.47,
    //26576222.93,
    //26638489.53,
    //27266363.84,
    //28964755.65,
    //31880010.73
    //                ]
    //                },
    //                {
    //                    name: '账面价值',
    //                    type: 'line',
    //                    stack: '总量',
    //                    areaStyle: { normal: {} },
    //                    data: [1254041.19,
    //1254414.69,
    //1257054.25,
    //1278098.38,
    //1538720.991,
    //2219360.321
    //                ]
    //                }
    //            ]
    //        };
    //        mychart3.setOption(option3);

}]);