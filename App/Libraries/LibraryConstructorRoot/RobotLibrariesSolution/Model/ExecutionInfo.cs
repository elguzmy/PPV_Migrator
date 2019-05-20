using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace Model
{
    public static class ExecutionInfo
    {
        private static string _pathConfig = "";
        private static JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
        {
            Culture = CultureInfo.CreateSpecificCulture("es-ES"),
            Formatting = Formatting.Indented
        };

        private static Dictionary<string, object> _jsonValues = null;

        #region Operations

		public static void Initialize(string parameterJsonPath)
        {
            try
            {
				_pathConfig = parameterJsonPath;
                Task _task = new Task(() => _jsonValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(_pathConfig), JsonSettings));

                _task.Start();
                _task.Wait();
                
                foreach(var _property in _jsonValues.Where(P => P.Value.GetType().Name.Equals("JArray")).ToDictionary(K => K.Key, V => V.Value).Keys.ToList())
                {
                    if(!(((JArray)_jsonValues[_property]).Count > 0))
                        _jsonValues[_property] = ((JArray)_jsonValues[_property]).ToObject<List<String>>();

                    else
                    {
                        string _theType = ((JArray)_jsonValues[_property])[0].Type.ToString();

                        if (_theType.ToLower().Equals("int64"))
                            _jsonValues[_property] = ((JArray)_jsonValues[_property]).ToObject<List<Int64>>();

                        else if (_theType.ToLower().Equals("boolean"))
                            _jsonValues[_property] = ((JArray)_jsonValues[_property]).ToObject<List<Boolean>>();

                        else
                            _jsonValues[_property] = ((JArray)_jsonValues[_property]).ToObject<List<String>>();
                    }
				}
            }
            catch (Exception ex)
            {
                throw new Exception("Error inicializando ExecutionInfo: " + ex.Message);
			}
		}

        public static void Update()
        {
            try
			{
                Task _task = new Task(() => _jsonValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(_pathConfig), JsonSettings));
                _task.Start();
                _task.Wait();
			}
			
			catch(Exception ex)
			{
				throw new Exception("Error actualizando ExecutionInfo desde archivo: " + ex.Message);
			}
        }
        public static void Save()
        {
			try
			{
                Task _task = new Task(() => File.WriteAllText(_pathConfig, JsonConvert.SerializeObject(_jsonValues, Formatting.Indented), Encoding.UTF8));
                _task.Start();
                _task.Wait();
			}
			
			catch(Exception ex)
			{
				throw new Exception("Error guardando ExecutionInfo en archivo: " + ex.Message);
			}
		}
        public static void Save(string parameterPathConfig)
        {
			try
			{
				ChangeSavingPath(parameterPathConfig);

                Task _task = new Task(() => Save());
                _task.Start();
                _task.Wait();
			}
			
			catch(Exception ex)
			{
				throw new Exception("Error guardando ExecutionInfo en archivo especificado: " + ex.Message);
			}
		}
        public static void ChangeSavingPath(string parameterPathConfig)
        {
            try
            {
                if (Path.GetExtension(parameterPathConfig) != ".json")
                    throw new Exception("El archivo especificado no es extensi�n 'json'");

                _pathConfig = parameterPathConfig;
            }

            catch(Exception ex)
            {
                throw new Exception("No se pudo cambiar la ruta de ExecutionInfo, puede que sea incorrecta: " + ex.Message);
            }
        }
		public static bool IsInitialized()
		{
			if(_jsonValues == null)
				return false;
			
			return true;
		}
        #endregion


        #region Properties
		public static Int64 ProcessId 
		{ 
			get	
			{
				return (Int64)_jsonValues["ProcessId"];
			} 
			
			set 
			{
				_jsonValues["ProcessId"] = value;
			}
		}

		public static Int64 ManualProcessId 
		{ 
			get	
			{
				return (Int64)_jsonValues["ManualProcessId"];
			} 
			
			set 
			{
				_jsonValues["ManualProcessId"] = value;
			}
		}

		public static Int64 ExecutionNumber 
		{ 
			get	
			{
				return (Int64)_jsonValues["ExecutionNumber"];
			} 
			
			set 
			{
				_jsonValues["ExecutionNumber"] = value;
			}
		}

		public static String CurrentItem 
		{ 
			get	
			{
				return (String)_jsonValues["CurrentItem"];
			} 
			
			set 
			{
				_jsonValues["CurrentItem"] = value;
			}
		}
        

        #endregion
    }
}
