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
    public static class StopHandler
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
                throw new Exception("Error inicializando StopHandler: " + ex.Message);
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
				throw new Exception("Error actualizando StopHandler desde archivo: " + ex.Message);
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
				throw new Exception("Error guardando StopHandler en archivo: " + ex.Message);
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
				throw new Exception("Error guardando StopHandler en archivo especificado: " + ex.Message);
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
                throw new Exception("No se pudo cambiar la ruta de StopHandler, puede que sea incorrecta: " + ex.Message);
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
		public static Boolean Stop 
		{ 
			get	
			{
				return (Boolean)_jsonValues["Stop"];
			} 
			
			set 
			{
				_jsonValues["Stop"] = value;
			}
		}

		public static Boolean Stopped 
		{ 
			get	
			{
				return (Boolean)_jsonValues["Stopped"];
			} 
			
			set 
			{
				_jsonValues["Stopped"] = value;
			}
		}

		public static Boolean LoggingStop 
		{ 
			get	
			{
				return (Boolean)_jsonValues["LoggingStop"];
			} 
			
			set 
			{
				_jsonValues["LoggingStop"] = value;
			}
		}
        

        #endregion
    }
}
