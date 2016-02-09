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
                                       name: "customer.commands.login-on-behalf",
                                       icon: 'fa fa-key',
                                       executeMethod: function () {
                                           var newBlade = {
                                               id: 'memberDetailChild',
                                               currentEntityId: blade.currentEntityId,
                                               title: 'customer.blades.loginOnBehalf-list.title',
                                               titleValues: { name: blade.currentEntity.fullName },
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
                                           return this.name = (blade.currentEntity && blade.currentEntity.userState === 'Approved') ? 'customer.commands.reject-user' : 'customer.commands.approve-user';
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
                                       permission: 'platform:security:update'
                                   },
                                   {
                                       name: "customer.commands.change-password",
                                       icon: 'fa fa-refresh',
                                       executeMethod: function () {
                                           var newBlade = {
                                               id: 'accountDetailChild',
                                               currentEntityId: account.userName,
                                               title: blade.title,
                                               subtitle: "customer.blades.account-changePassword.subtitle",
                                               controller: 'platformWebApp.accountChangePasswordController',
                                               template: '$(Platform)/Scripts/app/security/blades/account-changePassword.tpl.html'
                                           };
                                           bladeNavigationService.showBlade(newBlade, blade);
                                       },
                                       canExecuteMethod: function () {
                                           return true;
                                       },
                                       permission: 'platform:security:update'
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
                dynamicProperties: [],
                addresses: [],
                phones: [],
                emails: []
            };

            if (blade.isOrganization) {
                newEntity.parentId = blade.parentBlade.currentEntity.id;
                fillDynamicProperties(newEntity, 'VirtoCommerce.Domain.Customer.Model.Organization');
            } else {
                newEntity.organizations = [];
                if (blade.parentBlade.currentEntity.id) {
                    newEntity.organizations.push(blade.parentBlade.currentEntity.id);
                }
                fillDynamicProperties(newEntity, 'VirtoCommerce.Domain.Customer.Model.Contact');
            }
        }
    }

    function fillDynamicProperties(newEntity, typeName) {
        dynamicPropertiesApi.query({ id: typeName }, function (results) {
            _.each(results, function (x) {
                x.displayNames = undefined;
                x.values = [];
            });
            newEntity.dynamicProperties = results;
            initializeBlade(newEntity);
        }, function (error) { bladeNavigationService.setError('Error ' + error.status, blade); });
    }

    function initializeBlade(data) {
        blade.currentEntity = angular.copy(data);
        blade.origEntity = data;
        blade.isLoading = false;
    };

    function isDirty() {
        return !angular.equals(blade.currentEntity, blade.origEntity);
    };

    $scope.saveChanges = function () {
        blade.isLoading = true;

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
                title: "customer.dialogs.customer-save.title",
                message: "customer.dialogs.customer-save.message"
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
            name: "platform.commands.save",
            icon: 'fa fa-save',
            executeMethod: function () {
                $scope.saveChanges();
            },
            canExecuteMethod: function () {
                return isDirty() && $scope.formScope && $scope.formScope.$valid;
            },
            permission: 'customer:update'
        },
        {
            name: "platform.commands.reset",
            icon: 'fa fa-undo',
            executeMethod: function () {
                angular.copy(blade.origEntity, blade.currentEntity);
                if (userStateCommand) {
                    userStateCommand.updateName();
                }
            },
            canExecuteMethod: isDirty,
            permission: 'customer:update'
        }
    ];

    // datepicker
    $scope.datepickers = {
        bd: false
    }
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