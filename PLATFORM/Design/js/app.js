angular.module('app', ['ngGridster']);

angular.module('app')
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