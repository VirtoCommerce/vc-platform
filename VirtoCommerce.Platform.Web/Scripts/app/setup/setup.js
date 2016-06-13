angular.module('platformWebApp')
.config(['$stateProvider', function ($stateProvider) {
	$stateProvider
        .state('setupWizard', {
        	url: '/setupWizard',
        	templateUrl: '$(Platform)/Scripts/app/setup/templates/setupWizard.tpl.html',
        	controller: ['$scope', '$state', '$stateParams', 'platformWebApp.setupWizard', function ($scope, $state, $stateParams, setupWizard) {
        		setupWizard.loadStep(function (step) {
        			if (step) {
        				setupWizard.showStep(step);
        			};
        		});
        	}]
        });
}])
.factory('platformWebApp.setupWizard', ['$state', 'platformWebApp.settings', function ($state, settings) {	
	var wizardSteps = [];
	var wizard =
	{
		//switches the current step in the wizard to passed or next on the current
		showStep : function (step) {
			var state = step ? step.state : "workspace";
			settings.update([{ name: 'VirtoCommerce.SetupStep', value: state }]);
			$state.go(state);
		},

		findStep : function (state) {
			return _.find(wizardSteps, function (x) { return x.state == state; });
		},

		//registered step in the wizard
		registerStep : function (wizardStep) {
			wizardSteps.push(wizardStep);
			wizardSteps = _.sortBy(wizardSteps, function (x) { return x.priority; });
			var nextStep = undefined;
			for (var i = wizardSteps.length; i-- > 0;) {
				wizardSteps[i].nextStep = nextStep;
				nextStep = wizardSteps[i];
			}
		},

		loadStep : function (callback) {
			//Initial step
			var step = wizardSteps[0];
			//restore  saved setup step
			settings.getValues({ id: "VirtoCommerce.SetupStep" }, function (data) {
				if (angular.isArray(data) && data.length > 0) {
					step = wizard.findStep(data[0]);
				}
				callback(step);
			});
		}
	};
	return wizard;
}])
.run(
  ['$rootScope', '$state', 'platformWebApp.setupWizard', 'platformWebApp.settings', '$timeout', function ($rootScope, $state, setupWizard, settings, $timeout) {
  	//Try to run setup wizard
  	$rootScope.$on('loginStatusChanged', function (event, authContext) {
  		if (authContext.isAuthenticated) {
  			//timeout need because $state not fully loading in run method and need to wait little time
  			$timeout(function () {
  				setupWizard.loadStep(function (step) {
  					if (step) {
  						setupWizard.showStep(step);
  					};
  				});
  			}, 500);
  		}
  	});

  }]);
