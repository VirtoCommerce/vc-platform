angular.module('platformWebApp.notification', [ 
])
.factory('notificationService', function () {

	var id = 1;
	var alerts = [];

	function innerAlert(alert) {
		alert.id = id++;
		for (var i = 0; i < alerts.length; i++) {
			if (alerts[i].message === alert.message) {
				// same message already exists
				return alerts[i];
			}
		}
		alerts.push(alert);
		return alert;
	};

	var retVal = {
		getAllAlerts: function () {
			return alerts;
		},

		error: function (message) {
			return innerAlert({ type: 'error', message: message });
		},

		danger: function (message) {
			return innerAlert({ type: 'danger', message: message });
		},

		warning: function (message) {
			return innerAlert({ type: 'warning', message: message });
		},

		success: function (message) {
			return innerAlert({ type: 'success', message: message });
		},

		info: function (message) {
			return innerAlert({ type: 'info', message: message });
		},

		remove: function (alert) {
			for (var i = 0; i < alerts.length; i++) {
				if (alerts[i].id === alert.id) {
					alerts.splice(i, 1);
				}
			}
			// alert could not be found
			return false;
		},

		clear: function () {
			alerts = [];
		}
	};
	return retVal;

});
