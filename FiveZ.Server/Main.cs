using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FiveZ.Shared;

namespace FiveZ.Server
{
    public class Main : BaseScript
    {
        static Main _instance = null;

        public static Main GetInstance()
        {
            return _instance;
        }

        public Main()
        {
            _instance = this;
            Loader.Init();        }

        public void RegisterEventHandler(string _event, Delegate _action)
        {
            try
            {
                EventHandlers.Add(_event, _action);
            }
            catch(Exception ex)
            {
                Utils.Throw(ex);
            }
        }

        public void UnregisterEventHandler(string _event, Delegate _action)
        {
            try
            {
                EventHandlers.Remove(_event);
            }
            catch(Exception ex)
            {
                Utils.Throw(ex);
            }
        }

        public void RegisterExport(string _export, Delegate _action)
        {
            try
            {
                Exports.Add(_export, _action);
            }
            catch (Exception ex)
            {
                Utils.Throw(ex);
            }
        }

        public ExportDictionary CallExport()
        {
            try
            {
                return Exports;
            }
            catch (Exception ex)
            {
                Utils.Throw(ex);
                return null;
            }
        }

        public void RegisterCommand(string _command, Delegate _action, bool _restricted)
        {
            API.RegisterCommand(_command, _action, _restricted);
        }

    }
}
