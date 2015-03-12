angular.module('platformWebApp')
.factory('objCompareService', function () {

	//https://stamat.wordpress.com/2013/06/22/javascript-object-comparison/

	var specialChars = [ '$', '_' ];
	//Returns the object's class, Array, Date, RegExp, Object are of interest to us
	var getClass = function (val) {
		return Object.prototype.toString.call(val)
			.match(/^\[object\s(.*)\]$/)[1];
	};

	//Defines the type of the value, extended typeof
	var whatis = function (val) {

		if (val === undefined)
			return 'undefined';
		if (val === null)
			return 'null';

		var type = typeof val;

		if (type === 'object')
			type = getClass(val).toLowerCase();

		if (type === 'number') {
			if (val.toString().indexOf('.') > 0)
				return 'float';
			else
				return 'integer';
		}

		return type;
	};

	var compareObjects = function (a, b) {
		if (a === b)
			return true;

		if (Object.keys(a).length < Object.keys(b).length)
		{
			var tmp = a;
			a = b;
			b = tmp;
		}

		for (var i in a) {
			//ignore system properties and functions
			if (!_.contains(specialChars, i.charAt(0)) && whatis(a[i]) != 'function') {
				if (b.hasOwnProperty(i)) {
					if (!equal(a[i], b[i]))
						return false;
				} else {
					return false;
				}
			}
		}

		//for (var i in b) {
		//	if (!a.hasOwnProperty(i)) {
		//		return false;
		//	}
		//}
		return true;
	};

	var compareArrays = function (a, b) {
		if (a === b)
			return true;
		if (a.length !== b.length)
			return false;
		for (var i = 0; i < a.length; i++) {
			if (!equal(a[i], b[i])) return false;
		};
		return true;
	};

	var _equal = {};
	_equal.array = compareArrays;
	_equal.object = compareObjects;
	_equal.date = function (a, b) {
		return a.getTime() === b.getTime();
	};
	_equal.regexp = function (a, b) {
		return a.toString() === b.toString();
	};
	//	uncoment to support function as string compare
	//	_equal.fucntion =  _equal.regexp;
	/*
	* Are two values equal, deep compare for objects and arrays.
	* @param a {any}
	* @param b {any}
	* @return {boolean} Are equal?
	*/
	var equal = function (a, b) {
		var retVal = a === b;
		if (!retVal) {
			var atype = whatis(a), btype = whatis(b);
			if (atype === btype)
			{
				retVal = _equal.hasOwnProperty(atype) ? _equal[atype](a, b) : a == b;
			}
		}

		return retVal;
	}

	return {
		equal: equal
	};
});
