function loadjs(file) {
    var script = document.createElement("script");
    script.type = "application/javascript";
    script.src = file;
    script.id = "languagefile"
    document.head.appendChild(script);
    return script;
}

function getQueryStringParams(url, name) {
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}

function initialTranslation(obj) {
    obj.html(resource_globalization["SelectDefinition"]);
    $.initialize('section.models.is-open', function () {
        $(this).find('h4 > span').html(resource_globalization["Schemas"]);


    });
}

function allTranslation() {
    $.initialize('h4.opblock-title:not(.parameter__name)', function () {
        $(this).find('span').html(resource_globalization["Description"]);
    });

    $.initialize('h4.opblock-title.parameter__name', function () {
        $(this).html(resource_globalization["RequestBody"]);
    });

    $.initialize('table.parameters', function () {

        $(this).find('th.parameters-col_name').html(resource_globalization["Name"]);
        $(this).find('th.parameters-col_description').html(resource_globalization["Description"]);
    });

    $.initialize('button.try-out__btn', function () {
        $(this).html(resource_globalization["TryItOut"]);
    });

    $.initialize('button.try-out__btn.cancel', function () {
        $(this).html(resource_globalization["Cancel"]);
    });

    $.initialize('button.execute', function () {
        $(this).html(resource_globalization["Execute"]);
    });

    $.initialize('button.btn-clear', function () {
        $(this).html(resource_globalization["Clear"]);
    });

    $.initialize('button.authorize ', function () {
        $(this).find('span').html(resource_globalization["Authorize"]);
    });

    $.initialize('button.authorize.modal-btn.auth', function () {
        $(this).html(resource_globalization["Authorize"]);

        $(this).closest('.modal-ux-inner').find('.modal-ux-header > h3').html(resource_globalization["AvailableAuth"]);

    });

    $.initialize('.btn-done.modal-btn.auth', function () {
        $(this).html(resource_globalization["Close"]);
    });

    $.initialize('.parameter__enum ', function () {
        $(this).find('i').html(resource_globalization["AvailableValues"]);
    });

    $.initialize('a.tablinks[data-name="example"]', function () {
        $(this).html(resource_globalization["ExampleValue"]);
    });

    $.initialize('a.tablinks[data-name="model"]', function () {
        $(this).html(resource_globalization["Schema"]);
    });

    $.initialize('.responses-wrapper', function () {
        $(this).find('.opblock-section-header > h4').html(resource_globalization["Responses"]);
    });

    $.initialize('.request-url', function () {
        $(this).prev('h4').html(resource_globalization["RequestUrl"]);
    });

    $.initialize('table.responses-table.live-responses-table', function () {
        $(this).prev('h4').html(resource_globalization["ServerResponse"]);
        $(this).find('tr.responses-header > td.response-col_status').html(resource_globalization["Code"]);
        $(this).find('tr.responses-header > td.response-col_description').html(resource_globalization["Details"]);

        $(this).find('tr.response > td.response-col_description').find('h5:first').html(resource_globalization["ResponseBody"]);
        $(this).find('tr.response > td.response-col_description').find('h5').eq(1).html(resource_globalization["ResponseHeaders"]);

        $(this).parent('div').next('h4').html(resource_globalization["Responses"]);


    });

    $.initialize('table.responses-table:not(.live-responses-table)', function () {
        $(this).find('tr.responses-header > td.response-col_status').html(resource_globalization["Code"]);
        $(this).find('tr.responses-header > td.response-col_description').html(resource_globalization["Description"]);

    });

    $.initialize('.response-control-media-type__title', function () {
        $(this).html(resource_globalization["MediaType"]);
    });

    $.initialize('.download-contents', function () {
        $(this).html(resource_globalization["Download"]);
    });


}


$(document).ready(function () {

    if ($('script[id="languagefile"]').length == 0) {
        $.initialize('label.select-label', function () {
            $('script[id="languagefile"]').remove();

            var sel = $(this).find("#select");
            var lang = getQueryStringParams(sel.val(), 'lang');
            if (lang != null && lang != "") {

                var lbl = $(this);
                var script = loadjs('./script/translate/' + lang + '.js')
                script.onload = function () {

                    initialTranslation(lbl.find('span'));
                    allTranslation();
                   
                };
            }

            $(sel).change(function () {
                $('script[id="languagefile"]').remove();
                var lang = getQueryStringParams($(this).val(), 'lang');
                if (lang != null && lang != "") {
                    var script = loadjs('./script/translate/' + lang + '.js')
                    script.onload = function () {
                        initialTranslation($(sel).prev('span'));

                      
                    };
                }
            });

        });
    }
    else {
        $.initialize('label.select-label', function () {
                      
           var lbl = $(this);
               
            initialTranslation(lbl.find('span'));
            allTranslation();
    
        });
    }

   

    

});