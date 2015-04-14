// Copyright 2012 Omar AL Zabir
// Part of Droptiles project.
// This file holds the definition of tiles and which tiles appear by default 
// to new visitors. 


// The default tile setup offered to new users.
window.DefaultTiles = [
    {
        name: 'Section 1',
        tiles: [
           { id: 'block1', name: 'testBlock1' },
           { id: 'block2', name: 'testBlock2' },
           { id: 'block3', name: 'testBlock3' },
           { id: 'block4', name: 'testBlock4' },
           { id: 'block5', name: 'testBlock5' },
           { id: 'block6', name: 'testBlock6' },
           { id: 'block7', name: 'testBlock7' },
        ]
    },
    {
        tiles: [
            { id: 'block8', name: 'testBlock8' },
            { id: 'block9', name: 'testBlock9' },
            { id: 'block10', name: 'testBlock10' },
        ]
    },
    {
        tiles: [
            { id: 'block11', name: 'testBlock11' },
            { id: 'block12', name: 'testBlock12' },
            { id: 'block13', name: 'testBlock13' },
        ]
    }
];


// Convert it to a serialized string
window.DefaultTiles = _.map(window.DefaultTiles, function (section) {
    return "" + section.name + "~" + (_.map(section.tiles, function (tile) {
        return "" + tile.id + "," + tile.name;
    })).join(".");
}).join("|");
        

// Definition of the tiles, their default values.
window.TileBuilders = {

    testBlock1: function (uniqueId) {
        return {
            label: 'Test Block 1',
            size: 'double',
            subContent: '<img src="http://fakeimg.pl/350x200/00CED1/FFF/?text=img+placeholder">',
            uniqueId: uniqueId
        };
    },

    testBlock2: function (uniqueId) {
        return {
            counter: 25,
            label: 'Test Block 2',
            size: 'double wide',
            subContent: '<span class="tile-count">15</span><span class="tile-descr">Test block 1</span>',
            uniqueId: uniqueId
        };
    },

    testBlock3: function (uniqueId) {
        return {
            label: 'Test Block 3',
            uniqueId: uniqueId
        };
    },

    testBlock4: function (uniqueId) {
        return {
            label: 'Test Block 4',
            uniqueId: uniqueId
        };
    },

    testBlock5: function (uniqueId) {
        return {
            label: 'Test Block 5',
            uniqueId: uniqueId
        };
    },

    testBlock6: function (uniqueId) {
        return {
            label: 'Test Block 6',
            size: 'double wide',
            uniqueId: uniqueId
        };
    },

    testBlock7: function (uniqueId) {
        return {
            label: 'Test Block 7',
            uniqueId: uniqueId
        };
    },

    testBlock8: function (uniqueId) {
        return {
            label: 'Test Block 8',
            size: 'double',
            uniqueId: uniqueId
        };
    },

    testBlock9: function (uniqueId) {
        return {
            label: 'Test Block 9',
            size: 'double',
            uniqueId: uniqueId
        };
    },

    testBlock10: function (uniqueId) {
        return {
            label: 'Test Block 10',
            size: 'double',
            uniqueId: uniqueId
        };
    },

    testBlock11: function (uniqueId) {
        return {
            label: 'Test Block 11',
            size: 'triple',
            uniqueId: uniqueId
        };
    },

    testBlock12: function (uniqueId) {
        return {
            label: 'Test Block 12',
            size: 'double',
            uniqueId: uniqueId
        };
    },

    testBlock13: function (uniqueId) {
        return {
            label: 'Test Block 13'
        };
    },
};