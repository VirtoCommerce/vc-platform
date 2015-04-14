/// <reference path="jquery-1.7.2.min.js" />
/// <reference path="jQueryEnhancement.js" />
/// <reference path="Knockout-2.1.0.js" />
/// <reference path="Underscore.js" />
/* 
    Copyright 2012 Omar AL Zabir

    * The object models that are bound to the UI.eg. Section, Tile
    * The main ViewModel that initiaites and orchestrates the Dashboard experience.    
*/


/*
    The root Model class that holds all the sections and tiles inside the sections.

    Params:
        title - Title for the Dashboard eg "Start"
        sections - An array of section models.
        user - Currently logged in user details, or anonymous.
        ui - UI configuration, defaults.        
*/
var DashboardModel = function (title, sections, user, ui) {
    var self = this;

    this.appRunning = false;
    this.currentApp = "";
    this.user = ko.observable(user);
    this.title = ko.observable(title);
    this.sections = ko.observableArray(sections);
    this.trash = ko.observableArray([]);

    // Get a section model.
    this.getSection = function (uniqueId) {
        return ko.utils.arrayFirst(self.sections(), function (section) {
            return section.uniqueId == uniqueId;
        });
    }

    // Get a tile no matter where it is
    this.getTile = function (id) {
        var foundTile = null;
        ko.utils.arrayFirst(self.sections(), function (section) {
            foundTile = ko.utils.arrayFirst(section.tiles(), function (item) {
                return item.uniqueId == id;
            });
            return foundTile != null;
        });
        return foundTile;
    }

    // Remove a tile no matter where it is.
    this.removeTile = function (id) {
        ko.utils.arrayForEach(self.sections(), function (section) {
            var tile = ko.utils.arrayFirst(section.tiles(), function (tile) {
                return tile.uniqueId == id;
            });
            if (tile) {
                section.tiles.remove(tile);
                return;
            }
        });
    }

    // Subscribe to changes in each section's tile collection
    this.subscribeToChange = function (callback) {
        // Subscribe to the changes made in the sections collection. eg add/remove section.
        self.sections.subscribe(function (sections) {
            ko.utils.arrayForEach(sections(), function (section) {
                section.tiles.subscribe(function (tiles) {
                    callback(section, tiles);
                });
            });
        });
        // subscribe to changes made in the tiles collection of each section. eg add/remove tile.
        ko.utils.arrayForEach(self.sections(), function (section) {
            section.tiles.subscribe(function (tiles) {
                callback(section, tiles);
            });
        });
    }

    // Load sections and tiles from a serialized form. 
    this.loadSectionsFromString = function (tileSerialized, tileBuilder) {
        // Format: Section1~weather1,weather.youtube1,youtube|Section2~ie1,ie.

        var sections = ("" + tileSerialized).split("|");
        var sectionArray = [];

        _.each(sections, function (section) {
            var sectionName = _.string.strLeft(section, '~');

            var tiles = _.string.strRight(section, '~').split(".");

            var sectionTiles = [];

            var index = 0;
            _.each(tiles, function (tile) {
                if (tile.length > 0) {
                    var tileId = _.string.strLeft(tile, ",");
                    var tileName = _.string.strRight(tile, ",");

                    if (tileName.length > 0) {
                        var builder = tileBuilder[tileName];
                        if (builder == null) {
                            //console.log("No builder found for tile: " + tileName);
                        }
                        else {
                            var tileParams = builder(tileId);
                            var newTile = new Tile(tileParams, ui);
                            //newTile.index(index++);
                            sectionTiles.push(newTile);
                        }
                    }
                }
            });

            var newSection = new Section({
                name: sectionName,
                tiles: sectionTiles
            }, self);
            sectionArray.push(newSection);

        });


        self.sections(sectionArray);
    }

    // Load sections and tiles from an object model.
    this.loadSections = function (sections, tileBuilder) {
        var sectionArray = [];

        _.each(sections, function (section) {
            var sectionTiles = [];

            var index = 0;
            _.each(section.tiles, function (tile) {
                var builder = window.TileBuilders[tile.name];
                var tileParams = builder(tile.id, tile.name, tile.data);
                var newTile = new Tile(tileParams, ui);
                //newTile.index(index++);
                sectionTiles.push(newTile);
            });

            var newSection = new Section({
                name: section.title,
                tiles: sectionTiles
            }, self);
            sectionArray.push(newSection);

        });


        self.sections(sectionArray);
    }

    // Serialize sections and tiles in a string, handy to store in cookie.
    this.toSectionString = function () {
        // Format: Section1~weather1,weather.youtube1,youtube|Section2~ie1,ie.
        return ko.utils.arrayMap(self.sections(), function (section) {
            return section.name() + "~" +
                ko.utils.arrayMap(section.tiles(), function (tile) {
                    return tile.uniqueId + "," + tile.name;
                }).join(".");
        }).join("|");
    }

    
};

/*
    Represents a single Tile object model.
*/
var Tile = function (param, ui) {
    var self = this;

    this.uniqueId = param.uniqueId; // unique ID of a tile, Weather1, Weather2. Each instance must have unique ID.
    this.name = param.name; // unique name of a tile, eg Weather. 
    //this.index = ko.observable(param.index || 0); // order of tile on the screen. Calculated at run time.
    this.size = param.size || ""; // Size of the tile. eg tile-double, tile-double-vertical
    this.color = param.color || ui.tile_color;  // Color of tile. eg bg-color-blue
    this.additionalClass = param.additionalClass || ""; // Some additional class if you want to pass to further customize the tile
    this.tileImage = param.tileImage || ""; // Tile background image that fills the tile.

    this.cssSrc = param.cssSrc || [];   // CSS files to load at runtime.
    this.scriptSrc = param.scriptSrc || []; // Javascript files to load at runtime.
    this.initFunc = param.initFunc || ""; // After loading javascript, which function to call.
    this.initParams = param.initParams || {}; // Parameters to pass to the initial function.
    this.slidesFrom = param.slidesFrom || []; // HTML pages to load and inject as slides inside the tiles that rotate.

    this.appTitle = param.appTitle || ""; // Title of the application when launched by clicking on tile.
    this.appUrl = param.appUrl || "";   // URL of the application to launch.
    this.appInNewWindow = param.appInNewWindow || false; // To load the app on new browser window outside the Dashboard.

    this.iconStyle = param.iconStyle || ui.tile_icon_size; // Tile icon size.
    this.iconAdditionalClass = param.iconAdditionalClass || ""; // Additional class for the tile icon.
    this.iconSrc = param.iconSrc || ui.tile_icon_src; // Icon url
    this.appIcon = param.appIcon || this.iconSrc; // Icon to show when full screen app being launched.

    this.label = ko.observable(param.label || ""); // Bottom left label 
    this.counter = ko.observable(param.counter || ""); // Bottom right counter
    this.subContent = ko.observable(param.subContent || ""); // Content that comes up when mouse hover
    this.subContentColor = param.subContentColor || ui.tile_subContent_color; // Color for content

    this.slides = ko.observableArray(param.slides || []); // Tile content that rotates. Collection of html strings.

    this.tileClasses = ko.computed(function () {
        return [ui.tile,
            this.size,
            this.color,
            this.additionalClass,
            (this.slides().length > 0 ? ui.tile_multi_content : "")].join(" ");
        ;
    }, this);

    this.hasIcon = ko.computed(function () {
        return this.iconSrc.length > 0;
    }, this);

    this.iconClasses = ko.computed(function () {
        return [this.iconStyle, this.iconAdditionalClass].join(" ");
    }, this);

    this.hasLabel = ko.computed(function () {
        return this.label().length > 0;
    }, this);

    this.hasCounter = ko.computed(function () {
        return this.counter().length > 0;
    }, this);

    this.hasSubContent = ko.computed(function () {
        return this.subContent().length > 0;
    }, this);

    this.subContentClasses = ko.computed(function () {
        return [ui.tile_content_sub, this.subContentColor].join(" ");
    }, this);

    this.init = function (div) {
        if ($(div).data("tile_initialized") !== true)
            $(div).data("tile_initialized", true);
        else
            return;

        // If tile has css to load, then load all CSS.
        if (_.isArray(self.cssSrc)) {
            var head = $('head');

            // This needs to be exactly like this to work in IE 8.
            _.each(self.cssSrc, function (url) {
                $("<link>")
                  .appendTo(head)
                  .attr({ type: 'text/css', rel: 'stylesheet' })
                  .attr('href', url);
            });
        }

        // If tile has a collection of html pages as slides, then load them
        // and inject them inside tile so that they rotate.
        if (!_.isEmpty(self.slidesFrom)) {
            $.get((_.isArray(self.slidesFrom) ? self.slidesFrom : [self.slidesFrom]),
                function (slides) {
                    _.each(slides, function (slide) {
                        self.slides.push(slide);
                    });

                    // After loading the htmls, load the JS so that they
                    // can use the html elements.
                    self.loadScripts(div);
                });
        }
        else {
            self.loadScripts(div);
        }
    }

    // Loads the javascripts on a tile dynamically. Called from .attach()
    this.loadScripts = function (div) {
        if (!_.isEmpty(self.scriptSrc)) {
            $.getScript(self.scriptSrc, function () {
                if (!_.isEmpty(self.initFunc)) {
                    var func = eval(self.initFunc);
                    if (_.isFunction(func))
                        func(self, div, self.initParams);
                    else {
                        //console.log("Not a function: " + self.initFunc);
                    }
                }
            })
        }
    }

    this.click = function () {
        
    }
};

/*
    Section holds a collection of tiles. Each group of tiles you see
    huddled together on screen, are sections.
*/
var Section = function (section) {
    var self = this;

    this.name = ko.observable(section.name); // Name of a section. Can be used to show some title over section.
    this.uniqueId = _.uniqueId('section_'); // Unique ID generated at runtime and stored on the section Div.

    this.tiles = ko.observableArray(section.tiles);

    // Get a tile inside the section
    this.getTile = function(uniqueId) {
        return ko.utils.arrayFirst(self.tiles(), function(tile) {
            return tile.uniqueId == uniqueId;
        });
    }

    // Add a new tile at the end of the section
    this.addTile = function (tile) {
        self.tiles.push(tile);
        _.defer(function () {
            tile.attach($('#' + tile.uniqueId));
        });
    }

};



