angular.module('platformWebApp')
.config(['$stateProvider', function ($stateProvider) {
    $stateProvider
        .state('setupWizard', {
            url: '/setupWizard',
            templateUrl: '$(Platform)/Scripts/app/setup/templates/setupWizard.tpl.html',
            controller: ['$scope', '$state', '$stateParams', 'platformWebApp.setupWizard', function ($scope, $state, $stateParams, setupWizard) {}]
        });
}])
.factory('platformWebApp.setupWizard', ['$state', 'platformWebApp.settings', function ($state, settings) {	
    var wizardSteps = [];
    var wizard =
    {
        //switches the current step in the wizard to passed or next on the current
        showStep: function (step) {
            var state = step ? step.state : "workspace";
            if (wizard.currentStep != step) {
                wizard.currentStep = step;
                settings.update([{ name: 'VirtoCommerce.SetupStep', value: state }], function () {
                    $state.go(state, step);
                });
            }
            else {
                $state.go(state, step);
            }  
        },

        findStepByState : function (state) {
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
        load : function () {
            //Initial step
            wizard.currentStep = wizardSteps[0];
            //load  saved setup step
            return settings.getValues({ id: "VirtoCommerce.SetupStep" }).$promise.then(function (data) {
                if (angular.isArray(data) && data.length > 0) {
                    wizard.currentStep = wizard.findStepByState(data[0]);
                    wizard.isCompleted = wizard.currentStep === undefined;
                }
                return wizard;
            });
        },
        currentStep: undefined,
        isCompleted: false
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
                    setupWizard.load().then(
                        function(wizard) {
                             if (!wizard.isCompleted) {
                                 wizard.showStep(wizard.currentStep);
                             }
                        });
                }, 500);
        }
    });

  }]);
