using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public abstract class DbRequestHandler<Entity> where Entity : class
    {
        protected readonly IDbContext<Entity> _dbContext;
        protected DbRequestHandler(IDbContext<Entity> dbContext) { _dbContext = dbContext; }
    }
}
