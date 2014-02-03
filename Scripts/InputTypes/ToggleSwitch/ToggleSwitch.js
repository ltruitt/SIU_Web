/*
 * @namespace window.AcidJs
 * @class ToggleSwitch
 * @javascript
 * @author Martin Ivanov
 * @web http://wemakesites.net
 **/

(function() {
    "use strict";
    
    /*
     * @namespace window.AcidJs
     **/
    if(undefined === window.AcidJs) {
        window.AcidJs = {};
    }
    
    /*
     * @class ToggleSwitch
     * @constructor
     * @config (Object)
     * {
     *  appRoot: (String) [optional] path to the AcidJs.ToggleSwitch/ folder; default: ""
     * }
     * @return Object
     **/
    function ToggleSwitch(config) {
        
        config = config || {};
        
        var
            appRoot = config.appRoot || "";
        
        this._stylesheet(appRoot + this.URLS.stylesheet);
    }
    
    ToggleSwitch.prototype = {
        /*
         * @member MANIFEST
         * @public
         * @desc developer information and credits
         **/
        MANIFEST: {
            version: "1.0",
            name: "ToggleSwitch",
            developer: "Martin Ivanov",
            websites: {
                page: "http://experiments.wemakesites.net/javascript-iphone-toggle-buttons.html",
                personal: "http://wemakesites.net",
                portfolio: "http://acidjs.wemakesites.net",
                blog: "http://acidmartin.wordpress.com/",
                rss: "http://feeds.feedburner.com/acidmartin",
                twitter: "https://twitter.com/#!/wemakesitesnet"
            },
            email: "acid_martin@yahoo.com"
        },
        
        /*
         * @member URLS
         * @public
         * @desc URLS used in the class
         **/
        URLS: {
            //stylesheet: "AcidJs.ToggleSwitch/styles/ToggleSwitch.css"
            stylesheet: "ToggleSwitch.css"
        },
        
        /*
         * @member EVENTS
         * @public
         * @desc custom events
         **/
        EVENTS: [
            "acidjs-toggle-switch"
        ],
        
        /*
         * @member TEMPLATES
         * @public
         * @desc HTML templates used in the class
         **/
        TEMPLATES: {
            item: ['<li class="{{itemCssClasses}}">',
                     '<label>',
                       '<input type="{{type}}" value="{{value}}" name="{{name}}" {{enabled}} />',
                       '<span data-on=\'{{on}}\' data-off=\'{{off}}\'>',
                         '<b><!-- / --></b>',
                       '</span><strong>{{label}}</strong>',
                     '</label>',
                   '</li>'],
            wrapper: ['<form>',
                        '<ul>{{itemsHtml}}</ul>',
                      '</form>']
        },
        
        /*
         * @member CSS_CLASSES
         * @public
         * @desc CSS classes used in the app
         **/
        CSS_CLASSES: {
            base: "acidjs-toggle-switch"
        },
        
        /*
         * @method render
         * @public
         * @param config (Object) [required]
         * {
         *  type: (String) "checkbox"|"radio" [optional] default: "checkbox"
         *  name: (String) [required]
         *  defaultCheckedNode: (Number) [optional] default: undefined
         *  cssClasses: (Array) [optional]
         *  boundingBox: (JQueryDomNode) [required]
         *  on: (String) [optional] default: "on"
         *  off: (String) [optional] default: "off"
         *  items: (Array)
         *  [{
         *      value: (String) [optional] default: ""
         *      cssClasses: (Array) optional default: ""
         *      label: (String) optional default: ""
         *      enabled: (Boolean) [optional] default: true
         *   }]
         *  }
         **/
        render: function(config) {
            if(!config.boundingBox || !config.boundingBox.length || !config.name) {
                return;
            }
            
            var
                that = this,
                boundingBox = config.boundingBox,
                cssClasses = [this.CSS_CLASSES.base],
                items = config.items,
                defaultCheckedNode = config.defaultCheckedNode,
                html,
                itemsHtml = [],
                type = config.type || "checkbox",
                on = config.on || "on",
                off = config.off || "off",
                _setInputChecked = function(defaultCheckedNode) {
                    boundingBox.find('li:eq(' + defaultCheckedNode + ') input').prop("checked", true);
                };
            
            if(config.cssClasses) {
                cssClasses = cssClasses.concat(config.cssClasses);
            }
            
            if(items) {
                $.each(items, function(i) {
                    var
                        item = items[i],
                        itemCssClasses = item.cssClasses || [],
                        enabled = item.enabled === false ? false : true,
                        templateData = {
                            itemCssClasses: itemCssClasses.join(" "),
                            enabled: enabled ? "" : 'disabled="disabled"',
                            value: item.value || "",
                            type: type,
                            label: item.label || "",
                            on: on,
                            off: off,
                            name: type === "radio" ? config.name : config.name + "-" + i
                        };
                    
                    itemsHtml.push(that._template("item", templateData));
                });
            }
            
            cssClasses = cssClasses.join(" ");
            itemsHtml = itemsHtml.join("");
            html = this._template("wrapper", {
                itemsHtml: itemsHtml
            });
            boundingBox.addClass(cssClasses)
                       .html(html);
            
            if("number" === typeof(defaultCheckedNode)) {
                _setInputChecked.call(this, defaultCheckedNode);
            } else if (defaultCheckedNode instanceof Array) {
                $.each(defaultCheckedNode, function(i) {
                    _setInputChecked.call(this, defaultCheckedNode[i]);
                });
            }
            
            this._bind(boundingBox);
            
            boundingBox.delegate("input", "change", function() {
                var
                    input = $(this),
                    data = {
                        value: input.val(),
                        checked: input.prop("checked"),
                        name: input.attr("name"),
                        type: input.attr("type"),
                        boundingBox: input.parents("." + that.CSS_CLASSES.base)
                    };
                
                boundingBox.trigger(that.EVENTS[0], data);
            });
        },
        
        /*
         * @method destroy
         * @public
         * @param boundingBox (jQueryDomNode)
         * @return void
         * @desc remove widget by specified bounding box
         **/
        destroy: function(boundingBox) {
            if(boundingBox) {
                boundingBox.empty() 
                           .removeClass(this.CSS_CLASSES.base);
            }
        },
        
        /*
         * @method _bind
         * @private
         * @param name (String)
         * @param data (Object)
         * @return (DOMNode)
         * @desc handle the "acidjs-toggle-switch" custom event of the class
         * @return (Object)
         * {
         *  boundingBox: (jQueryDomNode)
         *  checked: (Boolean) the "checked" state of the selected node
	 *  name: (String) the "name" attribute of the selected node
         *  value: (String) the "value" attribute of the selected node
         *  type: (String) "checkbox"|"radio"
         * }
         **/
        _bind: function(boundingBox) {
            boundingBox.on(this.EVENTS[0], function(data) {
                return data;
            });
        },
        
        /*
         * @method _template
         * @public
         * @param name (String)
         * @param data (Object)
         * @return (DOMNode)
         * @desc compile a template from the TEMPLATES object
         **/
        _template: function(name, data) {
            var
                html = this.TEMPLATES[name] || [];

            data = data || {};
            html = html.join("");
            
            for(var key in data) {
                if(data.hasOwnProperty(key)) {
                    var
                        value = data[key],
                        regexp = new RegExp("{{" + key + "}}", "g");

                    html = html.replace(regexp, value);
                }
            }

            return html;
        },
        
        /*
         * @method _stylesheet
         * @param url (String)
         * @public
         * @return void
         * @desc asynchronously load a CSS file from the server
         **/
        _stylesheet: function(url) {
            var
                css = document.createElement("link"),
                id = this.CSS_CLASSES.base + "-stylesheet";
            
            css.setAttribute("rel", "stylesheet");
            css.setAttribute("href", url);
            css.setAttribute("id", id);
            
            if($("#" + id).length <= 0) {
                document.getElementsByTagName("head")[0].appendChild(css);
            }
        }
    };
    
    window.AcidJs.ToggleSwitch = ToggleSwitch;
})();