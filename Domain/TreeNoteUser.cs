using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TreeNoteUser
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Code { get; set; }
    }
}
