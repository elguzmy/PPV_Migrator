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
    public static class Config
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
                throw new Exception("Error inicializando Config: " + ex.Message);
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
				throw new Exception("Error actualizando Config desde archivo: " + ex.Message);
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
				throw new Exception("Error guardando Config en archivo: " + ex.Message);
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
				throw new Exception("Error guardando Config en archivo especificado: " + ex.Message);
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
                throw new Exception("No se pudo cambiar la ruta de Config, puede que sea incorrecta: " + ex.Message);
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

		#region Max Numbers
		public static Int64 MaxRetryNumber 
		{ 
			get	
			{
				return (Int64)_jsonValues["MaxRetryNumber"];
			} 
			
			set 
			{
				_jsonValues["MaxRetryNumber"] = value;
			}
		}

		public static Int64 MaxHistoryLogDetails 
		{ 
			get	
			{
				return (Int64)_jsonValues["MaxHistoryLogDetails"];
			} 
			
			set 
			{
				_jsonValues["MaxHistoryLogDetails"] = value;
			}
		}

		public static Int64 MaxHistoryLog 
		{ 
			get	
			{
				return (Int64)_jsonValues["MaxHistoryLog"];
			} 
			
			set 
			{
				_jsonValues["MaxHistoryLog"] = value;
			}
		}

		#endregion

		#region Timeouts
		public static Int64 TimeOutInstant 
		{ 
			get	
			{
				return (Int64)_jsonValues["TimeOutInstant"];
			} 
			
			set 
			{
				_jsonValues["TimeOutInstant"] = value;
			}
		}

		public static Int64 TimeOutSmall 
		{ 
			get	
			{
				return (Int64)_jsonValues["TimeOutSmall"];
			} 
			
			set 
			{
				_jsonValues["TimeOutSmall"] = value;
			}
		}

		public static Int64 TimeOutMedium 
		{ 
			get	
			{
				return (Int64)_jsonValues["TimeOutMedium"];
			} 
			
			set 
			{
				_jsonValues["TimeOutMedium"] = value;
			}
		}

		public static Int64 TimeOutLarge 
		{ 
			get	
			{
				return (Int64)_jsonValues["TimeOutLarge"];
			} 
			
			set 
			{
				_jsonValues["TimeOutLarge"] = value;
			}
		}

		public static Int64 TimeOutSuperLarge 
		{ 
			get	
			{
				return (Int64)_jsonValues["TimeOutSuperLarge"];
			} 
			
			set 
			{
				_jsonValues["TimeOutSuperLarge"] = value;
			}
		}

		public static Int64 TimeOutEthernal 
		{ 
			get	
			{
				return (Int64)_jsonValues["TimeOutEthernal"];
			} 
			
			set 
			{
				_jsonValues["TimeOutEthernal"] = value;
			}
		}

		#endregion

		#region Formats
		public static String FormatMailSubject 
		{ 
			get	
			{
				return (String)_jsonValues["FormatMailSubject"];
			} 
			
			set 
			{
				_jsonValues["FormatMailSubject"] = value;
			}
		}

		public static String FormatMailText 
		{ 
			get	
			{
				return (String)_jsonValues["FormatMailText"];
			} 
			
			set 
			{
				_jsonValues["FormatMailText"] = value;
			}
		}

		public static String FormatLog 
		{ 
			get	
			{
				return (String)_jsonValues["FormatLog"];
			} 
			
			set 
			{
				_jsonValues["FormatLog"] = value;
			}
		}

		public static String FormatMailBody 
		{ 
			get	
			{
				return (String)_jsonValues["FormatMailBody"];
			} 
			
			set 
			{
				_jsonValues["FormatMailBody"] = value;
			}
		}

		#endregion

		#region Mail addresses
		public static String DirMailRobot 
		{ 
			get	
			{
				return (String)_jsonValues["DirMailRobot"];
			} 
			
			set 
			{
				_jsonValues["DirMailRobot"] = value;
			}
		}

		public static String DirMailPassword 
		{ 
			get	
			{
				return (String)_jsonValues["DirMailPassword"];
			} 
			
			set 
			{
				_jsonValues["DirMailPassword"] = value;
			}
		}

		public static String DirMailReporting 
		{ 
			get	
			{
				return (String)_jsonValues["DirMailReporting"];
			} 
			
			set 
			{
				_jsonValues["DirMailReporting"] = value;
			}
		}

		#endregion

		#region File Paths
		public static String PathLog 
		{ 
			get	
			{
				return (String)_jsonValues["PathLog"];
			} 
			
			set 
			{
				_jsonValues["PathLog"] = value;
			}
		}

		public static String PathLogDetails 
		{ 
			get	
			{
				return (String)_jsonValues["PathLogDetails"];
			} 
			
			set 
			{
				_jsonValues["PathLogDetails"] = value;
			}
		}

		public static String PathRecovery 
		{ 
			get	
			{
				return (String)_jsonValues["PathRecovery"];
			} 
			
			set 
			{
				_jsonValues["PathRecovery"] = value;
			}
		}

		public static String PathDatabaseInfo 
		{ 
			get	
			{
				return (String)_jsonValues["PathDatabaseInfo"];
			} 
			
			set 
			{
				_jsonValues["PathDatabaseInfo"] = value;
			}
		}

		public static String PathExecutionInfo 
		{ 
			get	
			{
				return (String)_jsonValues["PathExecutionInfo"];
			} 
			
			set 
			{
				_jsonValues["PathExecutionInfo"] = value;
			}
		}

		public static String PathConfigProcess 
		{ 
			get	
			{
				return (String)_jsonValues["PathConfigProcess"];
			} 
			
			set 
			{
				_jsonValues["PathConfigProcess"] = value;
			}
		}

		public static String PathStopHandler 
		{ 
			get	
			{
				return (String)_jsonValues["PathStopHandler"];
			} 
			
			set 
			{
				_jsonValues["PathStopHandler"] = value;
			}
		}

		#endregion

		#region Folder Paths
		public static String DirectoryHistory 
		{ 
			get	
			{
				return (String)_jsonValues["DirectoryHistory"];
			} 
			
			set 
			{
				_jsonValues["DirectoryHistory"] = value;
			}
		}

		public static String DirectoryHistoryLogDetails 
		{ 
			get	
			{
				return (String)_jsonValues["DirectoryHistoryLogDetails"];
			} 
			
			set 
			{
				_jsonValues["DirectoryHistoryLogDetails"] = value;
			}
		}

		public static String DirectoryHistoryLog 
		{ 
			get	
			{
				return (String)_jsonValues["DirectoryHistoryLog"];
			} 
			
			set 
			{
				_jsonValues["DirectoryHistoryLog"] = value;
			}
		}

		public static String DirectoryExceptionScreenshots 
		{ 
			get	
			{
				return (String)_jsonValues["DirectoryExceptionScreenshots"];
			} 
			
			set 
			{
				_jsonValues["DirectoryExceptionScreenshots"] = value;
			}
		}

		public static String DirectoryRecovery 
		{ 
			get	
			{
				return (String)_jsonValues["DirectoryRecovery"];
			} 
			
			set 
			{
				_jsonValues["DirectoryRecovery"] = value;
			}
		}

		public static String DirectoryTemplates 
		{ 
			get	
			{
				return (String)_jsonValues["DirectoryTemplates"];
			} 
			
			set 
			{
				_jsonValues["DirectoryTemplates"] = value;
			}
		}

		public static String DirectoryTemp 
		{ 
			get	
			{
				return (String)_jsonValues["DirectoryTemp"];
			} 
			
			set 
			{
				_jsonValues["DirectoryTemp"] = value;
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

		#endregion

		#region Control Options
		public static String ProcessState 
		{ 
			get	
			{
				return (String)_jsonValues["ProcessState"];
			} 
			
			set 
			{
				_jsonValues["ProcessState"] = value;
			}
		}

		public static String MessageIfSuccess 
		{ 
			get	
			{
				return (String)_jsonValues["MessageIfSuccess"];
			} 
			
			set 
			{
				_jsonValues["MessageIfSuccess"] = value;
			}
		}

		public static Boolean ContinueOnBusinessError 
		{ 
			get	
			{
				return (Boolean)_jsonValues["ContinueOnBusinessError"];
			} 
			
			set 
			{
				_jsonValues["ContinueOnBusinessError"] = value;
			}
		}

		public static Boolean ContinueOnSystemError 
		{ 
			get	
			{
				return (Boolean)_jsonValues["ContinueOnSystemError"];
			} 
			
			set 
			{
				_jsonValues["ContinueOnSystemError"] = value;
			}
		}

		public static Boolean SendMailWhenFinish 
		{ 
			get	
			{
				return (Boolean)_jsonValues["SendMailWhenFinish"];
			} 
			
			set 
			{
				_jsonValues["SendMailWhenFinish"] = value;
			}
		}

		public static Boolean LogInDatabase 
		{ 
			get	
			{
				return (Boolean)_jsonValues["LogInDatabase"];
			} 
			
			set 
			{
				_jsonValues["LogInDatabase"] = value;
			}
		}

		public static Boolean SetRecover 
		{ 
			get	
			{
				return (Boolean)_jsonValues["SetRecover"];
			} 
			
			set 
			{
				_jsonValues["SetRecover"] = value;
			}
		}

		public static Boolean TraceSource 
		{ 
			get	
			{
				return (Boolean)_jsonValues["TraceSource"];
			} 
			
			set 
			{
				_jsonValues["TraceSource"] = value;
			}
		}

		public static Boolean Exit 
		{ 
			get	
			{
				return (Boolean)_jsonValues["Exit"];
			} 
			
			set 
			{
				_jsonValues["Exit"] = value;
			}
		}

		#endregion        

        #endregion
    }
}
