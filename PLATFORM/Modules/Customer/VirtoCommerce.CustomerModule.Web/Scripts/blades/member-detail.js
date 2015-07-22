angular.module('virtoCommerce.customerModule')
.controller('virtoCommerce.customerModule.memberDetailController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.customerModule.contacts', 'virtoCommerce.customerModule.organizations', 'platformWebApp.accounts', 'platformWebApp.dynamicProperties.api', 'platformWebApp.dialogService', function ($scope, bladeNavigationService, contacts, organizations, accounts, dynamicPropertiesApi, dialogService) {
    var blade = $scope.blade;
    blade.currentResource = blade.isOrganization ? organizations : contacts;
    var userStateCommand, customerAccount;

    blade.refresh = function (parentRefresh) {
        if (blade.currentEntityId) {
            blade.isLoading = true;

            blade.currentResource.get({ _id: blade.currentEntityId }, function (data) {
                initializeBlade(data);
                if (parentRefresh) {
                    blade.parentBlade.refresh();
                }

                if (!blade.isOrganization && data.emails.length > 0) {
                    accounts.get({ id: data.emails[0] }, function (account) {
                        if (account.userName) {
                            var needToAddToolbarCommands = !customerAccount;
                            customerAccount = account;
                            data.userState = account.userState;
                            initializeBlade(data);

                            if (needToAddToolbarCommands) {
                                blade.toolbarCommands.push(
                                   {
                                       name: "Login on behalf",
                                       icon: 'fa fa-key',
                                       executeMethod: function () {
                                           var newBlade = {
                                               id: 'memberDetailChild',
                                               currentEntityId: blade.currentEntityId,
                                               title: 'Login on behalf of ' + blade.currentEntity.fullName,
                                               controller: 'virtoCommerce.customerModule.loginOnBehalfListController',
                                               template: 'Modules/$(VirtoCommerce.Customer)/Scripts/blades/loginOnBehalf-list.tpl.html'
                                           };
                                           bladeNavigationService.showBlade(newBlade, blade);
                                       },
                                       canExecuteMethod: function () { return true; },
                                       permission: 'customer:loginOnBehalf'
                                   },
                                   userStateCommand = {
                                       updateName: function () {
                                           return this.name = (blade.currentEntity && blade.currentEntity.userState === 'Approved') ? 'Reject user' : 'Approve user';
                                       },
                                       icon: 'fa fa-dot-circle-o',
                                       executeMethod: function () {
                                           if (blade.currentEntity.userState === 'Approved') {
                                               blade.currentEntity.userState = 'Rejected';
                                           } else {
                                               blade.currentEntity.userState = 'Approved';
                                           }
                                           this.updateName();
                                       },
                                       canExecuteMethod: function () {
                                           return true;
                                       },
                                       permission: 'platform:security:manage'
                                   },
                                   {
                                       name: "Change password",
                                       icon: 'fa fa-refresh',
                                       executeMethod: function () {
                                           var newBlade = {
                                               id: 'accountDetailChild',
                                               currentEntityId: account.userName,
                                               title: blade.title,
                                               subtitle: "Change user's password",
                                               controller: 'platformWebApp.accountChangePasswordController',
                                               template: 'Scripts/common/security/blades/account-changePassword.tpl.html'
                                           };
                                           bladeNavigationService.showBlade(newBlade, blade);
                                       },
                                       canExecuteMethod: function () {
                                           return true;
                                       },
                                       permission: 'platform:security:manage'
                                   });
                            }

                            userStateCommand.updateName();
                        }
                    },
                    function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
                }
            },
            function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
        } else {
            var newEntity = {
                dynamicPropertyValues: [],
                addresses: [],
                phones: [],
                emails: []
            };

            if (blade.isOrganization) {
                newEntity.parentId = blade.parentBlade.currentEntity.id;
                initializeBlade(newEntity);
            } else {
                newEntity.organizations = [];
                if (blade.parentBlade.currentEntity.id) {
                    newEntity.organizations.push(blade.parentBlade.currentEntity.id);
                }

                dynamicPropertiesApi.query({ id: 'VirtoCommerce.Domain.Customer.Model.Contact' }, function (results) {
                    newEntity.dynamicPropertyValues = results;
                    initializeBlade(newEntity);
                }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
            }
        }
    }

    function initializeBlade(data) {
        // temporal workaround
        if (!blade.isOrganization && data.organizations.length > 0) {
            data.organization = data.organizations[0];
        }
        blade.currentEntity = angular.copy(data);
        blade.origEntity = data;
        blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity);
    };

    $scope.saveChanges = function () {
        blade.isLoading = true;

        // temporal workaround
        if (!blade.isOrganization) {
            blade.currentEntity.organizations = blade.currentEntity.organization ? [blade.currentEntity.organization] : [];
        }

        if (blade.currentEntityId) {
            if (customerAccount && customerAccount.userState !== blade.currentEntity.userState) {
                customerAccount.userState = blade.currentEntity.userState;
                customerAccount.$update(null, function () { }, function (error) {
                    bladeNavigationService.setError('Error ' + error.status, $scope.blade);
                });
            }

            blade.currentResource.update({}, blade.currentEntity, function (data) {
                blade.refresh(true);
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        } else {
            blade.currentResource.save({}, blade.currentEntity, function (data) {
                blade.title = data.displayName;
                blade.currentEntityId = data.id;
                initializeBlade(data);
                blade.parentBlade.refresh();
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
            });
        }
    };

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    blade.onClose = function (closeCallback) {
        closeChildrenBlades();
        if (isDirty()) {
            var dialog = {
                id: "confirmCurrentBladeClose",
                title: "Save changes",
                message: "The Customer has been modified. Do you want to save changes?"
            };
            dialog.callback = function (needSave) {
                if (needSave) {
                    $scope.saveChanges();
                }
                closeCallback();
            };
            dialogService.showConfirmationDialog(dialog);
        }
        else {
            closeCallback();
        }
    };

    function closeChildrenBlades() {
        angular.forEach(blade.childrenBlades.slice(), function (child) {
            bladeNavigationService.closeBlade(child);
        });
    }

    blade.headIcon = blade.isOrganization ? 'fa fa-university' : 'fa fa-user';

    blade.toolbarCommands = [
        {
            name: "Save",
            icon: 'fa fa-save',
            executeMethod: function () {
                $scope.saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty() && $scope.formScope && $scope.formScope.$valid;
            },
            permission: 'customer:manage'
        },
        {
            name: "Reset",
            icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntity);
                if (userStateCommand) {
                    userStateCommand.updateName();
                }
            },
            canExecuteMethod: function () {
                return isDirty();
            },
            permission: 'customer:manage'
        }
    ];

    // datepicker
    $scope.datepickers = {
        bd: false
    }

    $scope.showWeeks = true;
    $scope.toggleWeeks = function () {
        $scope.showWeeks = !$scope.showWeeks;
    };

    $scope.clear = function () {
        blade.currentEntity.birthDate = null;
    };
    $scope.today = new Date();

    $scope.open = function ($event, which) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.datepickers[which] = true;
    };

    $scope.dateOptions = {
        'year-format': "'yyyy'",
        'starting-day': 1
    };

    $scope.formats = ['shortDate', 'dd-MMMM-yyyy', 'yyyy/MM/dd'];
    $scope.format = $scope.formats[0];

    // other on load
    if (!blade.isOrganization) {
        $scope.organizations = organizations.query();
    }

    blade.refresh(false);
}]);