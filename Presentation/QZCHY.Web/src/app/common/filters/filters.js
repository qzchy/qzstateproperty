'use strict';

app.filter("highlight", function ($sce, $log) {

    var fn = function (text, search) {
        //$log.info("text: " + text);
        //$log.info("search: " + search);

        if (!search) {
            return $sce.trustAsHtml(text);
        }
        text = encodeURI(text);
        search = encodeURI(search);

        var regex = new RegExp(search, 'gi')
        var result = text.replace(regex, '<span class="highlightedText">$&</span>');
        result = decodeURI(result);
        //$log.info("result: " + result);
        return result;// $sce.trustAsHtml(result);
    };

    return fn;
}).filter("unit", function () {

    var fn = function (number, mode) {

        switch(mode)
        {
            case "carea":
                if (number >= 10000000000) return (Math.round((number / 10000000000) * 100) / 100) + " 万平方公里";
                    //else if (number >= 1000000) return (Math.round((number / 1000000) * 100) / 100) + " 平方公里";
                else if (number >= 10000) return (Math.round((number / 10000) * 100) / 100) + " 万平方米";
                else return (Math.round((number) * 100) / 100) + " 平方米";
                break;
            case "larea":
                //土地面积
                var t = number / 666.67;
                if (t > 10000) return (Math.round((t / 10000) * 100) / 100) + " 万亩";
                else return (Math.round(t * 100) / 100) + " 亩";
                break;
            case "price":
                if (number >= 10000) return (Math.round((number / 10000) * 100) / 100) + " 亿元";
                else if (number >= 1000) return (Math.round((number / 1000) * 100) / 100) + " 千万元";
                else return (Math.round(number * 100) / 100) + " 万元";
                break;
            case "count":
            default:
                return number + " 个";

                break;
        }
    };

    return fn;
});