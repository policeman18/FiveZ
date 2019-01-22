using System;
using System.Dynamic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using FiveZ.Shared;

namespace FiveZ.Client
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
            Loader.Init();
        }

        public void RegisterEventHandler(string _event, Delegate _action)
        {
            try
            {
                EventHandlers.Add(_event, _action);
            }
            catch (Exception ex)
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
            catch (Exception ex)
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

        public void RegisterTickHandler(Func<Task> _action)
        {
            try
            {
                Tick += _action;
            }
            catch (Exception ex)
            {
                Utils.Throw(ex);
            }
        }

        public void UnregisterTickHandler(Func<Task> _action)
        {
            try
            {
                Tick -= _action;
            }
            catch (Exception ex)
            {
                Utils.Throw(ex);
            }
        }

        public void SetNuiFocus(bool _focus, bool _cursor)
        {
            API.SetNuiFocus(_focus, _cursor);
        }

        public void SendNUIData(string type, string name, string data = null) => API.SendNuiMessage(JsonConvert.SerializeObject(new { type, name, data }));

        public void RegisterNUICallback(string _type, Action<ExpandoObject, CallbackDelegate> _callback)
        {
            API.RegisterNuiCallbackType(_type);
            RegisterEventHandler($"__cfx_nui:{_type}", _callback);
        }
    }
}
