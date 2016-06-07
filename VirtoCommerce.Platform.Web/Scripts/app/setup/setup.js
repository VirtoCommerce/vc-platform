angular.module('platformWebApp')
.config(['$stateProvider', function ($stateProvider) {
	$stateProvider
        .state('setupWizard', {
        	url: '/setupWizard',
        	templateUrl: '$(Platform)/Scripts/app/setup/templates/setupWizard.tpl.html',
        	controller: ['$scope', '$state', '$stateParams', 'platformWebApp.setupWizard', function ($scope, $state, $stateParams, setupWizard) {
        		var step = setupWizard.currentStep
        		if (!step)
        		{
        			step = setupWizard.wizardSteps[0];
        		}
        		setupWizard.nextStep(step.state);
        	}]
        });
}])
.factory('platformWebApp.setupWizard', ['$state', 'platformWebApp.settings', function ($state, settings) {	
	var retVal = {
		currentStep: undefined,
		wizardSteps: []
	};
	//switches the current step in the wizard to passed or next on the current
	retVal.nextStep = function (state) {
		if (state) {
			retVal.currentStep = _.find(retVal.wizardSteps, function (x) { return x.state == state; });
		}
		else {
			if (retVal.currentStep) {
				retVal.currentStep = retVal.currentStep.nextStep;
			}
		};

		settings.update([{ name: 'VirtoCommerce.SetupStep', value: retVal.currentStep ? retVal.currentStep.state : "n/a" }]);
		if (retVal.currentStep) {
			$state.go(retVal.currentStep.state)
		}
		else {
			$state.go("workspace");
		}
	};
	//registered step in the wizard
	retVal.registerStep = function(wizardStep) {
		retVal.wizardSteps.push(wizardStep);
		retVal.wizardSteps = _.sortBy(retVal.wizardSteps, function (x) { return x.priority; });
		var nextStep = undefined;
		for (var i = retVal.wizardSteps.length; i-- > 0;) {
			retVal.wizardSteps[i].nextStep = nextStep;
			nextStep = retVal.wizardSteps[i];
		}
		//Initial step
		retVal.currentStep = retVal.wizardSteps[0];
	};	
	return retVal;
}])
.run(
  ['$rootScope', '$state', 'platformWebApp.setupWizard', 'platformWebApp.settings', function ($rootScope, $state, setupWizard, settings) {
  	//Try to run setup wizard
  	$rootScope.$on('loginStatusChanged', function (event, authContext) {
  		//Try to display setup starting with previous saved step
  		if (settings.getValues({ id: "VirtoCommerce.SetupStep" }, function (data) {
			var stepState = setupWizard.currentStep.state;
			if (angular.isArray(data) && data.length > 0) {
				stepState = data[0];
  			}			
			setupWizard.nextStep(stepState);
  			//VirtoCommerce.SetupStep setting 204 not found response handling
  		}, function (error) { setupWizard.nextStep(setupWizard.currentStep.state); }));
  	});

  }]);
