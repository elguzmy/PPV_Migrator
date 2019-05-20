using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities.Presentation.Metadata;

namespace Register
{
    public class RegisterLibrary : IRegisterMetadata
    {
        public void Register()
        {
            var a = Model.Config.DirectoryHistory;
        }
    }
}
