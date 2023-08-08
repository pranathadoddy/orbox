/*! 
 * jQuery Bootgrid v1.3.1 - 09/11/2015
 * Copyright (c) 2014-2015 Rafael Staib (http://www.jquery-bootgrid.com)
 * Licensed under MIT http://www.opensource.org/licenses/MIT
 */
;(function ($, window, undefined)
{
    /*jshint validthis: true */
    "use strict";

    $.extend($.fn.bootgrid.Constructor.defaults.css, {
        icon: "icon fa",
        iconColumns: "fa-th-list",
        iconDown: "fa-sort-desc",
        iconRefresh: "fa-refresh",
        iconSearch: "fa-search",
        iconUp: "fa-sort-asc",
        paginationButton: "page-link"
    });

    $.extend($.fn.bootgrid.Constructor.defaults.templates, {
        actionButton: "<button class=\"btn btn-outline-secondary\" type=\"button\" title=\"{{ctx.text}}\">{{ctx.content}}</button>",
        actionDropDown: "<div class=\"{{css.dropDownMenu}}\"><button class=\"btn btn-outline-secondary dropdown-toggle\" type=\"button\" data-toggle=\"dropdown\"><span class=\"{{css.dropDownMenuText}}\">{{ctx.content}}</span> <span class=\"caret\"></span></button><ul class=\"{{css.dropDownMenuItems}}\" role=\"menu\"></ul></div>",
        paginationItem: "<li class=\"paginate_button page-item {{ctx.css}}\"><a data-page=\"{{ctx.page}}\" class=\"{{css.paginationButton}}\">{{ctx.text}}</a></li>",
        search: "<label class=\"align-middle mb-0 mr-1\"><nobr><div class=\"input-group\"><div class=\"input-group-prepend\"><div class=\"input-group-text\"><i class=\"fa fa-search\"></i></div></div><input type=\"text\" class=\"{{css.searchField}} form-control\" placeholder=\"{{lbl.search}}\" /></div></nobr></label>"
    });
})(jQuery, window);