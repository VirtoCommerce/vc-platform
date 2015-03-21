angular.module('virtoCommerce.cartModule.resources.carts', [])
.factory('carts', ['$resource', function ($resource) {
    return $resource('api/cart/:id', { id: '@Id' }, {
        cartsSearch: { url: 'api/cart/carts' },
        getCart: { url: 'api/cart/carts/:id' },
        update: { method: 'POST', url: 'api/cart' }
    });

    //return {
    //    cartsSearch: getCartsMock,
    //    // update: updateMock
    //};

    //function getCartsMock(param1, param2) {
    //    var entries = [
    //        {
    //            id: 'id1',
    //            name: 'id1 name',
    //            customerName: 'customer name',
    //            items: [{ id: 'i01' }, { id: 'i88' }, { id: 'i05' }],
    //            total: '123.45 USD',
    //            createdDate: '1278903921551',
    //            modifiedDate: '2015-01-05T16:16:08.726Z'
    //        },
    //        {
    //            id: 'id2',
    //            name: 'id2 name',
    //            customerName: 'john doe name',
    //            items: [{ id: 'i01' }],
    //            total: '7123.45 USD',
    //            createdDate: '1228903921551',
    //            modifiedDate: '2015-01-08T16:16:08.726Z'
    //        }

    //    ];

    //    var retVal = {
    //        totalCount: entries.length,
    //        shopingCarts: entries
    //    }
    //    param2(retVal);
    //}

}]);