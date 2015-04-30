angular.module('app', ['gridster']);

angular.module('app')
    .controller('gridsterController', ['$scope', function ($scope) {
         $scope.standardItems = [
                  { sizeX: 2, sizeY: 1, row: 0, col: 0, template: 'tmp1.html', data: {name:'naME1'} },
                  { sizeX: 2, sizeY: 2, row: 0, col: 2, template: 'tmp2.html', data: {name:'naME2'} },
                  { sizeX: 1, sizeY: 1, row: 0, col: 4, template: 'tmp1.html', data: {name:'naME3'} },
                  { sizeX: 1, sizeY: 1, row: 0, col: 5, template: 'tmp2.html', data: {name:'naME4'} },
                  { sizeX: 2, sizeY: 1, row: 1, col: 0, template: 'tmp1.html', data: {name:'naME5'} },
                  { sizeX: 1, sizeY: 1, row: 1, col: 4, template: 'tmp2.html', data: {name:'naME6'} },
                  { sizeX: 1, sizeY: 2, row: 1, col: 5, template: 'tmp1.html', data: {name:'naME7'} },
                  { sizeX: 1, sizeY: 1, row: 2, col: 0, template: 'tmp2.html', data: {name:'naME8'} },
                  { sizeX: 2, sizeY: 1, row: 2, col: 1, template: 'tmp1.html', data: {name:'naME9'} },
                  { sizeX: 1, sizeY: 1, row: 2, col: 3, template: 'tmp2.html', data: {name:'naME0'} },
                  { sizeX: 1, sizeY: 1, row: 2, col: 4, template: 'tmp1.html', data: {name:'naME-'} }
         ];
         $scope.gridsterOpts = {
             minRows: 2, // the minimum height of the grid, in rows
             maxRows: 100,
             columns: 6, // the width of the grid, in columns
             colWidth: 'auto', // can be an integer or 'auto'.  'auto' uses the pixel width of the element divided by 'columns'
             rowHeight: 'match', // can be an integer or 'match'.  Match uses the colWidth, giving you square widgets.
             margins: [10, 10], // the pixel distance between each widget
             defaultSizeX: 2, // the default width of a gridster item, if not specifed
             defaultSizeY: 1, // the default height of a gridster item, if not specified
             mobileBreakPoint: 600, // if the screen is not wider that this, remove the grid layout and stack the items
             resizable: {
                 enabled: false
             },
             draggable: {
                 enabled: true, // whether dragging items is supported
                 handle: '.my-class', // optional selector for resize handle
                 start: function (event, uiWidget, $element) { }, // optional callback fired when drag is started,
                 drag: function (event, uiWidget, $element) { }, // optional callback fired when item is moved,
                 stop: function (event, uiWidget, $element) { } // optional callback fired when item is finished dragging
             }
         };
    }])
    .controller('GridController', ['$scope', function ($scope) {
        // var localData = JSON.parse(localStorage.getItem('positions'));
    
        // if(localData != null)
        // {
        //     $.each(localData, function(i, value) {
        //         var id_name;

        //         id_name = "#";
        //         id_name = id_name + value.id;

        //         $(id_name).attr({ "data-col": value.col, "data-row": value.row, "data-sizex": value.size_x, "data-sizey": value.size_y });
            //     });
            // }

        $scope.options = {
            widget_margins: [5, 5],
            widget_base_dimensions: [120, 120],
            helper: 'clone',
            // serialize_params: function($w, wgd) 
            // {
            //     return {
            //         id: $($w).attr('id'),
            //         col: wgd.col, 
            //         row: wgd.row,
            //         size_x: wgd.size_x,
            //         size_y: wgd.size_y,
            //     };
            // },
        
            // draggable: 
            // {
            //     stop: function(event, ui) {
            
            //         var positions = this.serialize();
            //         $scope.col = positions.col;
        //         $scope.row = positions.row;
        //     }
        // }
    };

        // Wide - width, Tall - height, Template - Custom template

        $scope.items = [
        {
                wide: 2,
            tall: 2,
                    template: 'Template 1'
            },
                {
                        wide: 2,
            tall: 1,
                    template: 'Template 2'
            },
                {
                        wide: 2,
            tall: 1,
                    template: 'Template 3'
        },
            {
                template: ''
        },
            {
                template: ''
        },
            {
                template: ''
        },
            {
                template: ''
            },
        ];
    }
]);