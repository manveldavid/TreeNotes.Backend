﻿using Application.Common.Exceptions;
using Application.Common.Workers;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Actions.User.Queries.UserId
{
    public class UserIdQueryHandler : DbRequestHandler<TreeNoteUser>, IRequestHandler<UserIdQuery, Guid>
    {
        public UserIdQueryHandler(IDbContext<TreeNoteUser> context) : base(context) { }
        public async Task<Guid> Handle(UserIdQuery request, CancellationToken cancellationToken)
        {
            var result = await TreeNoteUserWorker.Finder
                .UserFromLoginPassword(_dbContext, request.Login, request.Password)
                .ToListAsync();
            var user = result.FirstOrDefault();

            #region Checks

            var resultUserExist = await TreeNoteUserWorker.Finder
                .UserFromLogin(_dbContext, request.Login)
                .ToListAsync();
            var userExist = resultUserExist.FirstOrDefault();
            if(userExist != null && user == null)
            {
                throw new TreeNoteUserWrongPasswordException(nameof(TreeNoteUser), $"log:{request.Login} pas:{request.Password}");
            }

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(TreeNoteUser), $"log:{request.Login} pas:{request.Password}");
            }

            #endregion

            return user.Id;
        }
    }
}
