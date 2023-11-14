angular.module("platformWebApp")
    .filter('settingTranslate', ['platformWebApp.localizableSettingService', function (localizableSettingService) {
        return function (input, settingName) {
            const values = localizableSettingService.getValues(settingName);

            if (!values) {
                return input
            }

            const item = values.find(item => item.key === input);

            if (!item) {
                return input;
            }

            return item.value;
        };
    }]);
