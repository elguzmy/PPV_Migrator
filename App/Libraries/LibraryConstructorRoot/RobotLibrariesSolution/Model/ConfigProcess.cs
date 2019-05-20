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
    public static class ConfigProcess
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
                throw new Exception("Error inicializando ConfigProcess: " + ex.Message);
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
				throw new Exception("Error actualizando ConfigProcess desde archivo: " + ex.Message);
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
				throw new Exception("Error guardando ConfigProcess en archivo: " + ex.Message);
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
				throw new Exception("Error guardando ConfigProcess en archivo especificado: " + ex.Message);
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
                throw new Exception("No se pudo cambiar la ruta de ConfigProcess, puede que sea incorrecta: " + ex.Message);
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
		public static String ProcessName 
		{ 
			get	
			{
				return (String)_jsonValues["ProcessName"];
			} 
			
			set 
			{
				_jsonValues["ProcessName"] = value;
			}
		}

		public static String CompleteProcessName 
		{ 
			get	
			{
				return (String)_jsonValues["CompleteProcessName"];
			} 
			
			set 
			{
				_jsonValues["CompleteProcessName"] = value;
			}
		}

		public static String DirectoryInput 
		{ 
			get	
			{
				return (String)_jsonValues["DirectoryInput"];
			} 
			
			set 
			{
				_jsonValues["DirectoryInput"] = value;
			}
		}

		public static String DirectoryOutput 
		{ 
			get	
			{
				return (String)_jsonValues["DirectoryOutput"];
			} 
			
			set 
			{
				_jsonValues["DirectoryOutput"] = value;
			}
		}

		public static List<String> UsedApps 
		{ 
			get	
			{
				return (List<String>)_jsonValues["UsedApps"];
			} 
			
			set 
			{
				_jsonValues["UsedApps"] = value;
			}
		}

		public static String ppvCredential 
		{ 
			get	
			{
				return (String)_jsonValues["ppvCredential"];
			} 
			
			set 
			{
				_jsonValues["ppvCredential"] = value;
			}
		}

		public static String pathLibraries 
		{ 
			get	
			{
				return (String)_jsonValues["pathLibraries"];
			} 
			
			set 
			{
				_jsonValues["pathLibraries"] = value;
			}
		}

		public static String sheetLibraries 
		{ 
			get	
			{
				return (String)_jsonValues["sheetLibraries"];
			} 
			
			set 
			{
				_jsonValues["sheetLibraries"] = value;
			}
		}

		public static String PathInputWorkbook 
		{ 
			get	
			{
				return (String)_jsonValues["PathInputWorkbook"];
			} 
			
			set 
			{
				_jsonValues["PathInputWorkbook"] = value;
			}
		}

		public static String PathTemp_PPV_MigratorFiles 
		{ 
			get	
			{
				return (String)_jsonValues["PathTemp_PPV_MigratorFiles"];
			} 
			
			set 
			{
				_jsonValues["PathTemp_PPV_MigratorFiles"] = value;
			}
		}

		public static String SheetTemp_PPV_MigratorFiles 
		{ 
			get	
			{
				return (String)_jsonValues["SheetTemp_PPV_MigratorFiles"];
			} 
			
			set 
			{
				_jsonValues["SheetTemp_PPV_MigratorFiles"] = value;
			}
		}

		public static String PathTemp_PPV_CompleteList 
		{ 
			get	
			{
				return (String)_jsonValues["PathTemp_PPV_CompleteList"];
			} 
			
			set 
			{
				_jsonValues["PathTemp_PPV_CompleteList"] = value;
			}
		}

		public static String SheetTemp_PPV_CompleteList 
		{ 
			get	
			{
				return (String)_jsonValues["SheetTemp_PPV_CompleteList"];
			} 
			
			set 
			{
				_jsonValues["SheetTemp_PPV_CompleteList"] = value;
			}
		}

		public static String PathTemp_PPV_LibrariesList 
		{ 
			get	
			{
				return (String)_jsonValues["PathTemp_PPV_LibrariesList"];
			} 
			
			set 
			{
				_jsonValues["PathTemp_PPV_LibrariesList"] = value;
			}
		}

		public static String SheetTemp_PPV_LibrariesList 
		{ 
			get	
			{
				return (String)_jsonValues["SheetTemp_PPV_LibrariesList"];
			} 
			
			set 
			{
				_jsonValues["SheetTemp_PPV_LibrariesList"] = value;
			}
		}

		public static String PathDownloads 
		{ 
			get	
			{
				return (String)_jsonValues["PathDownloads"];
			} 
			
			set 
			{
				_jsonValues["PathDownloads"] = value;
			}
		}

		public static String UrlClientePVCISharePoint 
		{ 
			get	
			{
				return (String)_jsonValues["UrlClientePVCISharePoint"];
			} 
			
			set 
			{
				_jsonValues["UrlClientePVCISharePoint"] = value;
			}
		}
        

        #endregion
    }
}
