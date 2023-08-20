using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Actions.User.Queries.UserFromPassCode
{
    public class UserFromPassCodeQuery:IRequest<TreeNoteUser>
    {
        public string PassCode { get; set; }
    }
}
