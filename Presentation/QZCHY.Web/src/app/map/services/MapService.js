"use strict";

//地图服务
app.service("MapService", ['$rootScope', function ($rootScope) {

    this.getLayer = function (id, type) {

        var providers = {
            Conutry: {
                Normal: {
                    Map: { url: "http://t{s}.tianditu.com/vec_c/wmts", options: { layer: "vec", style: "default", tilematrixSet: "c", format: "tiles", maxZoom: 14, minZoom: 1 } },
                    Annotion: { url: "http://t{s}.tianditu.com/cva_c/wmts", options: { layer: "cva", style: "default", tilematrixSet: "c", format: "tiles", maxZoom: 14, minZoom: 1 } },
                },
                Satellite: {
                    Map: { url: "http://t{s}.tianditu.com/img_c/wmts", options: { layer: "img", style: "default", tilematrixSet: "c", format: "tiles", maxZoom: 14, minZoom: 1 } },
                    Annotion: { url: "http://t{s}.tianditu.com/cia_c/wmts", options: { layer: "cia", style: "default", tilematrixSet: "c", format: "tiles", maxZoom: 14, minZoom: 1 } },
                },
                Subdomains: ['0', '1', '2', '3', '4', '5', '6', '7']
            },
            ZJ: {
                Normal: {
                    Map: { url: "http://srv{s}.zjditu.cn/ZJEMAP_2D/wmts", options: { layer: "TDT_ZJEMAP", style: "default", tilematrixSet: "default028mm", format: "image/jpgpng", maxZoom: 17, minZoom: 7 } },
                    Annotion: { url: "http://srv{s}.zjditu.cn/ZJEMAPANNO_2D/wmts", options: { layer: "TDT_ZJEMAPANNO", style: "default", tilematrixSet: "default028mm", format: "image/jpgpng", maxZoom: 17, minZoom: 7 } },
                },
                Satellite: {
                    Map: { url: "http://srv{s}.zjditu.cn/ZJDOM_2D/wmts", options: { layer: "imgmap", style: "default", tilematrixSet: "default028mm", format: "image/jpgpng", maxZoom: 17, minZoom: 7 } },
                    Annotion: { url: "http://srv{s}.zjditu.cn/ZJDOMANNO_2D/wmts", options: { layer: "TDT_ZJIMGANNO", style: "default", tilematrixSet: "default028mm", format: "image/jpgpng", maxZoom: 17, minZoom: 7 } },
                },
                Subdomains: ['0', '1', '2', '3', '4', '5', '6', '7']
            },
            QZ: {
                Normal: {
                    Map: { url: "http://www.qz-map.com/geoservices/qzemap/service.asmx/wmts", options: { layer: "QZEMAP", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                    Annotion: { url: "http://www.qz-map.com/geoservices/QZEMAPANNO/service/wmts", options: { layer: "QZEMAPANNO", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                },
                Satellite: {
                    Map: { url: "http://www.qz-map.com/geoservices/QZIMG/service/wmts", options: { layer: "QZIMG", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                    Annotion: { url: "http://www.qz-map.com/geoservices/QZIMGANNO/service/wmts", options: { layer: "QZIMGANNO", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                },
                Subdomains: []
            },
            HS: {
                Normal: {
                    Map: { url: "http://ahapp.ahmap.gov.cn/WMTS/kvp/services/ahhf_dom8Test/MapServer/TDTWMTSServer", options: { SERVICE: "WMTS", VERSION: "1.0.0", REQUEST: "GetCapabilities" } },
                    Annotion: { url: "http://hsapp.ahmap.gov.cn/WMTS/kvp/services/HSDLGAnno2014w/MapServer/TDTWMTSServer", options: { SERVICE: "WMTS", VERSION: "1.0.0", REQUEST: "GetCapabilities" } }
                },
                Satellite: {
                    Map: { url: "http://hsapp.ahmap.gov.cn/WMTS/kvp/services/HSDOM2014w/MapServer/TDTWMTSServer", options: { SERVICE: "WMTS", VERSION: "1.0.0", REQUEST: "GetCapabilities" } },
                    Annotion: { url: "http://hsapp.ahmap.gov.cn/WMTS/kvp/services/HSDLGAnno2014w/MapServer/TDTWMTSServer", options: { SERVICE: "WMTS", VERSION: "1.0.0", REQUEST: "GetCapabilities" } }
                },
                Subdomains: []
            },
            LY: {
                Normal: {
                    Map: { url: "http://www.qz-map.com/geoservices/lyemap/service/wmts", options: { layer: "LYEMAP", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                    Annotion: { url: "http://www.qz-map.com/geoservices/lyimganno/service/wmts", options: { layer: "LYIMGANNO", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                },
                Satellite: {
                    Map: { url: "http://www.qz-map.com/geoservices/lyimg/service/wmts", options: { layer: "LYIMG", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                    Annotion: { url: "http://www.qz-map.com/geoservices/lyimganno/service/wmts", options: { layer: "LYIMGANNO", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                },
                Subdomains: []
            },
            KH: {
                Normal: {
                    Map: { url: "http://www.qz-map.com/geoservices/KHEMAP/service/wmts", options: { layer: "KHEMAP", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                    Annotion: { url: "http://www.qz-map.com/geoservices/KHEMAPANNO/service/wmts", options: { layer: "KHEMAPANNO", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                },
                Satellite: {
                    Map: { url: "http://www.qz-map.com/geoservices/KHIMG/service/wmts", options: { layer: "KHIMG", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                    Annotion: { url: "http://www.qz-map.com/geoservices/KHIMGANNO/service/wmts", options: { layer: "KHIMGANNO", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                },
                Subdomains: []
            },
            CS: {
                Normal: {
                    Map: { url: "http://www.qz-map.com/geoservices/CSEMAP/service/wmts", options: { layer: "CSEMAP", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                    Annotion: { url: "http://www.qz-map.com/geoservices/CSEMAPANNO/service/wmts", options: { layer: "CSEMAPANNO", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                },
                Satellite: {
                    Map: { url: "http://www.qz-map.com/geoservices/CSIMG/service/wmts", options: { layer: "CSIMG", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                    Annotion: { url: "http://www.qz-map.com/geoservices/CSIMGANNO/service/wmts", options: { layer: "CSIMGANNO", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                },
                Subdomains: []
            },
            JS: {
                Normal: {
                    Map: { url: "http://www.qz-map.com/geoservices/JSEMAP/service/wmts", options: { layer: "JSEMAP", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                    Annotion: { url: "http://www.qz-map.com/geoservices/JSEMAPANNO/service/wmts", options: { layer: "JSEMAPANNO", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                },
                Satellite: {
                    Map: { url: "http://www.qz-map.com/geoservices/JSIMG/service/wmts", options: { layer: "JSIMG", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                    Annotion: { url: "http://www.qz-map.com/geoservices/JSIMGANNO/service/wmts", options: { layer: "JSIMGANNO", style: "default", tilematrixSet: "TileMatrixSet0", format: "image/png", maxZoom: 20, minZoom: 18 } },
                },
                Subdomains: []
            },
        };

        var url = providers["Conutry"][type]["Map"]["url"], options = providers["Conutry"][type]["Map"]["options"];
        options.subdomains = providers["Conutry"]["Subdomains"];
        options.tileSize = 256;
        options.id = "gj" + id + "_" + type;
        var gj = new L.TileLayer.WMTS(url, options);

        url = providers["Conutry"][type]["Annotion"]["url"], options = providers["Conutry"][type]["Annotion"]["options"];
        options.subdomains = providers["Conutry"]["Subdomains"];
        options.tileSize = 256;
        options.id = "gj" + id + "_" + type;
        var gjAnno = new L.TileLayer.WMTS(url, options);

        url = providers["ZJ"][type]["Map"]["url"], options = providers["ZJ"][type]["Map"]["options"];
        options.subdomains = providers["ZJ"]["Subdomains"];
        options.tileSize = 256;
        options.id = "zj" + id + "_" + type;
        var zj = new L.TileLayer.WMTS(url, options);

        url = providers["ZJ"][type]["Annotion"]["url"], options = providers["ZJ"][type]["Annotion"]["options"];
        options.subdomains = providers["ZJ"]["Subdomains"];
        options.tileSize = 256;
        options.id = "zj" + id + "_" + type;
        var zjAnno = new L.TileLayer.WMTS(url, options);

        url = providers["QZ"][type]["Map"]["url"], options = providers["QZ"][type]["Map"]["options"];
        options.subdomains = providers["QZ"]["Subdomains"];
        options.tileSize = 256;
        options.id = "qz" + id + "_" + type;
        var qz = new L.TileLayer.WMTS(url, options);

        url = providers["QZ"][type]["Annotion"]["url"], options = providers["QZ"][type]["Annotion"]["options"];
        options.subdomains = providers["QZ"]["Subdomains"];
        options.tileSize = 256;
        options.id = "qz" + id + "_" + type;
        var qzAnno = new L.TileLayer.WMTS(url, options);

        url = providers["LY"][type]["Map"]["url"], options = providers["LY"][type]["Map"]["options"];
        options.subdomains = providers["LY"]["Subdomains"];
        options.tileSize = 256;
        var ly = new L.TileLayer.WMTS(url, options);

        url = providers["LY"][type]["Annotion"]["url"], options = providers["LY"][type]["Annotion"]["options"];
        options.subdomains = providers["LY"]["Subdomains"];
        options.tileSize = 256;
        var lyAnno = new L.TileLayer.WMTS(url, options);

        url = providers["KH"][type]["Map"]["url"], options = providers["KH"][type]["Map"]["options"];
        options.subdomains = providers["KH"]["Subdomains"];
        options.tileSize = 256;
        var kh = new L.TileLayer.WMTS(url, options);

        url = providers["KH"][type]["Annotion"]["url"], options = providers["KH"][type]["Annotion"]["options"];
        options.subdomains = providers["KH"]["Subdomains"];
        options.tileSize = 256;
        var khAnno = new L.TileLayer.WMTS(url, options);

        url = providers["JS"][type]["Map"]["url"], options = providers["JS"][type]["Map"]["options"];
        options.subdomains = providers["JS"]["Subdomains"];
        options.tileSize = 256;
        var js = new L.TileLayer.WMTS(url, options);

        url = providers["JS"][type]["Annotion"]["url"], options = providers["JS"][type]["Annotion"]["options"];
        options.subdomains = providers["JS"]["Subdomains"];
        options.tileSize = 256;
        var jsAnno = new L.TileLayer.WMTS(url, options);

        url = providers["CS"][type]["Map"]["url"], options = providers["CS"][type]["Map"]["options"];
        options.subdomains = providers["CS"]["Subdomains"];
        options.tileSize = 256;
        var cs = new L.TileLayer.WMTS(url, options);

        url = providers["CS"][type]["Annotion"]["url"], options = providers["CS"][type]["Annotion"]["options"];
        options.subdomains = providers["CS"]["Subdomains"];
        options.tileSize = 256;
        var csAnno = new L.TileLayer.WMTS(url, options);


        return new L.layerGroup([gj, gjAnno, zj, zjAnno, qz, qzAnno, ly, lyAnno, kh, khAnno, js, jsAnno, cs, csAnno]);
    };

    this.setMapAttribute = function (map) {
        var that = this;
        var attributeControl = map.attributionControl;
        this.updateMapAttribute(attributeControl, 16);

        attributeControl.setPrefix("<a href='http://www.qz-map.com'>天地图衢州</a>");

        map.addEventListener('zoomend', function (e) { 
            var level = map.getZoom();
            that.updateMapAttribute(attributeControl, level);
        });

    }

    this.updateMapAttribute = function (attributeControl, level) {
        attributeControl.removeAttribution("数据来源：<a href='http://www.baidu.com'>国家测绘地理信息局</a>");
        attributeControl.removeAttribution("数据来源：浙江测绘与地理信息局");
        attributeControl.removeAttribution("数据来源：衢州测绘与地理信息局");

        if (level <= 7) {
            attributeControl.addAttribution("数据来源：<a href='http://www.baidu.com'>国家测绘地理信息局</a>");
        }
        else if (level <= 17) {
            attributeControl.addAttribution("数据来源：浙江测绘与地理信息局");
        }
        else {
            attributeControl.addAttribution("数据来源：衢州测绘与地理信息局");
        }
    };

    //定义一些参数
    L.Util.VincentyConstants = {
        a: 6378137,
        b: 6356752.3142,
        f: 1 / 298.257223563
    };
    
    //圆转多边形
    this.convertCircleToPolygon = function (layer) {

        var origin = layer.getLatLng(); //center of drawn circle
        var radius = layer.getRadius(); //radius of drawn circle
        var projection = L.CRS.EPSG4326;
        var polys = this.createGeodesicPolygon(origin, radius, 60, 0, projection); //these are the points that make up the circle
        var polygon = []; // store the geometry
        for (var i = 0; i < polys.length; i++) {
            var geometry = [polys[i].lat, polys[i].lng];
            polygon.push(geometry);
        }

        var polyCircle = L.polygon(polygon);
        return polyCircle;
    };


    //大地多边形
    this.createGeodesicPolygon=function(origin, radius, sides, rotation, projection) {

        var latlon = origin; //leaflet equivalent
        var angle;
        var new_lonlat, geom_point;
        var points = [];

        for (var i = 0; i < sides; i++) {
            angle = (i * 360 / sides) + rotation;
            new_lonlat = this.destinationVincenty(latlon, angle, radius);
            geom_point = L.latLng(new_lonlat.lng, new_lonlat.lat);

            points.push(geom_point);
        }

        return points;
    }

    this.destinationVincenty=function(lonlat, brng, dist) { //rewritten to work with leaflet

        var u = L.Util;
        var ct = u.VincentyConstants;
        var a = ct.a, b = ct.b, f = ct.f;
        var lon1 = lonlat.lng;
        var lat1 = lonlat.lat;
        var s = dist;
        var pi = Math.PI;
        var alpha1 = brng * pi / 180; //converts brng degrees to radius
        var sinAlpha1 = Math.sin(alpha1);
        var cosAlpha1 = Math.cos(alpha1);
        var tanU1 = (1 - f) * Math.tan(lat1 * pi / 180 /* converts lat1 degrees to radius */);
        var cosU1 = 1 / Math.sqrt((1 + tanU1 * tanU1)), sinU1 = tanU1 * cosU1;
        var sigma1 = Math.atan2(tanU1, cosAlpha1);
        var sinAlpha = cosU1 * sinAlpha1;
        var cosSqAlpha = 1 - sinAlpha * sinAlpha;
        var uSq = cosSqAlpha * (a * a - b * b) / (b * b);
        var A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
        var B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));
        var sigma = s / (b * A), sigmaP = 2 * Math.PI;
        while (Math.abs(sigma - sigmaP) > 1e-12) {
            var cos2SigmaM = Math.cos(2 * sigma1 + sigma);
            var sinSigma = Math.sin(sigma);
            var cosSigma = Math.cos(sigma);
            var deltaSigma = B * sinSigma * (cos2SigmaM + B / 4 * (cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM) -
                B / 6 * cos2SigmaM * (-3 + 4 * sinSigma * sinSigma) * (-3 + 4 * cos2SigmaM * cos2SigmaM)));
            sigmaP = sigma;
            sigma = s / (b * A) + deltaSigma;
        }
        var tmp = sinU1 * sinSigma - cosU1 * cosSigma * cosAlpha1;
        var lat2 = Math.atan2(sinU1 * cosSigma + cosU1 * sinSigma * cosAlpha1,
            (1 - f) * Math.sqrt(sinAlpha * sinAlpha + tmp * tmp));
        var lambda = Math.atan2(sinSigma * sinAlpha1, cosU1 * cosSigma - sinU1 * sinSigma * cosAlpha1);
        var C = f / 16 * cosSqAlpha * (4 + f * (4 - 3 * cosSqAlpha));
        var lam = lambda - (1 - C) * f * sinAlpha *
            (sigma + C * sinSigma * (cos2SigmaM + C * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)));
        var revAz = Math.atan2(sinAlpha, -tmp);  // final bearing
        var lamFunc = lon1 + (lam * 180 / pi); //converts lam radius to degrees
        var lat2a = lat2 * 180 / pi; //converts lat2a radius to degrees

        return L.latLng(lamFunc, lat2a);

    }
}]);
