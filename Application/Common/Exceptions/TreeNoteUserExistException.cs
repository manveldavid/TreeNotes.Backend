using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class TreeNoteUserExistException:Exception
    {
        public TreeNoteUserExistException(string name, object key) :
            base($"Entity {name} ({key}) exist"){ }
    }
}
