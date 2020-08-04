using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using System;

namespace DbPeek.Services.Settings
{
    internal static class VsShellSettingsService
    {
        private static IServiceProvider _serviceProvider;
        private static SettingsManager _settingsManager;
        private static WritableSettingsStore _userSettingsStore;
        private const string CollectionName = "DbPeek";

        public static void Initialise(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _settingsManager = new ShellSettingsManager(_serviceProvider);
            _userSettingsStore = _settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

            HandleCollection();
        }

        private static void HandleCollection()
        {
            if (!_userSettingsStore.CollectionExists(CollectionName))
            {
                _userSettingsStore.CreateCollection(CollectionName);
            }
        }

        public static T ReadSetting<T>(string propertyName)
        {
            CreatePropertyIfDoesNotExist<T>(propertyName);

            object value;
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.String:
                    value = _userSettingsStore.GetString(CollectionName, propertyName);
                    return (T)Convert.ChangeType(value, typeof(T));
                case TypeCode.Boolean:
                    value = _userSettingsStore.GetBoolean(CollectionName, propertyName);
                    return (T)Convert.ChangeType(value, typeof(T));
                case TypeCode.Int32:
                    value = _userSettingsStore.GetInt32(CollectionName, propertyName);
                    return (T)Convert.ChangeType(value, typeof(T));
                case TypeCode.Int64:
                    value = _userSettingsStore.GetInt64(CollectionName, propertyName);
                    return (T)Convert.ChangeType(value, typeof(T));
                case TypeCode.UInt32:
                    value = _userSettingsStore.GetUInt32(CollectionName, propertyName);
                    return (T)Convert.ChangeType(value, typeof(T));
                case TypeCode.UInt64:
                    value = _userSettingsStore.GetUInt64(CollectionName, propertyName);
                    return (T)Convert.ChangeType(value, typeof(T));
                default:
                    value = _userSettingsStore.GetString(CollectionName, propertyName);
                    return (T)Convert.ChangeType(value, typeof(T));
            }
        }

        public static void WriteSetting<T>(string propertyName, T propertyValue)
        {
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.String:
                    _userSettingsStore.SetString(CollectionName, propertyName, Convert.ToString(propertyValue));
                    break;
                case TypeCode.Boolean:
                    _userSettingsStore.SetBoolean(CollectionName, propertyName, Convert.ToBoolean(propertyValue));
                    break;
                case TypeCode.Int32:
                    _userSettingsStore.SetInt32(CollectionName, propertyName, Convert.ToInt32(propertyValue));
                    break;
                case TypeCode.Int64:
                    _userSettingsStore.SetInt64(CollectionName, propertyName, Convert.ToInt64(propertyValue));
                    break;
                case TypeCode.UInt32:
                    _userSettingsStore.SetUInt32(CollectionName, propertyName, Convert.ToUInt32(propertyValue));
                    break;
                case TypeCode.UInt64:
                    _userSettingsStore.SetUInt64(CollectionName, propertyName, Convert.ToUInt64(propertyValue));
                    break;
                default:
                    _userSettingsStore.SetString(CollectionName, propertyName, Convert.ToString(propertyValue));
                    break;
            }
        }

        private static void CreatePropertyIfDoesNotExist<T>(string propertyName)
        {
            if (!_userSettingsStore.PropertyExists(CollectionName, propertyName))
            {
                WriteSetting(propertyName, default(T));
            }
        }
    }
}
