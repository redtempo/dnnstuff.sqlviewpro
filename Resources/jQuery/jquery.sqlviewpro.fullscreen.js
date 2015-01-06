/*
* fullscreen - SQLView Pro Fullscreen
*/

(function(sqlviewpro, $, undefined) {
    //Private Properties
    //var isHot = true;

    //Public Properties
    sqlviewpro.fullScreenUrl = "";

    //Public Methods
    sqlviewpro.fullScreenUrlWithParameters = function(cmd) {
        return sqlviewpro.fullScreenUrl + parameterQueryString(cmd);
    };

    //Private Methods
    var parameterQueryString = function(cmd) {
        var parms = '';
        $(cmd).parent().find(":input").each(function() {
            if (this.id.length > 0) {
                var thisparm = getParameterName(this.id);
                var thisvalue = $(this).val();
                if (thisvalue != '') {
                    if (sqlviewpro.fullScreenUrl.indexOf('?') > -1) {
                        parms = parms + '&';
                    } else {
                        parms = parms + '?';
                    }
                    parms = parms + thisparm + '=' + thisvalue;
                }
            }
        });
        return parms;
    };

    var getParameterName = function(inputName) {
        var foundName = '';
        var matchStr = '(.+SQLViewPro_)(.+)(_.+)';
        var re = new RegExp(matchStr);
        var m = re.exec(inputName);
        if (m != null) {
            foundName = m[2];
        }
        return foundName;
    };

}(window.sqlviewpro = window.sqlviewpro || {}, jQuery));

jQuery(document).ready(function ($) {

    // fullscreen fancybox
    $("a.fullscreen").click(function (e) {
        this.href = sqlviewpro.fullScreenUrlWithParameters(this);
        return false;
    });

    $("a.fullscreen").fancybox({
        'width': '90%',
        'height': '90%',
        'autoScale': false,
        'transitionIn': 'none',
        'transitionOut': 'none',
        'type': 'iframe'
    });

    // new window
    $("a.newwindow").click(function (e) {
        window.open(sqlviewpro.fullScreenUrlWithParameters(this), "_blank");
        return false;
    });

});

