using Application.Interfaces;
using Domain;

namespace Application.Common.Workers
{
    public static class TreeNoteUserWorker
    {
        public static class Finder
        {
            public static IQueryable<TreeNoteUser> UserFromLoginPassword(IDbContext<TreeNoteUser> dbContext, string login, string password)
            {
                var code = Encoder.CodeFromLoginPassword(login, password);
                var entity = dbContext.Set.Where(x => x.Login == login && x.Code == code);
                return entity;
            }
            public static IQueryable<TreeNoteUser> UserFromLogin(IDbContext<TreeNoteUser> dbContext, string login)
            {
                var entity = dbContext.Set.Where(x => x.Login == login);
                return entity;
            }
        }
        public static class Encoder
        {
            private static readonly char AddingChar = '\n';
            public static string CodeFromLoginPassword(string login, string password)
            {
                var loginChars = login.ToList();
                var passwordChars = password.ToList();

                if (loginChars.Count > passwordChars.Count)
                {
                    while (loginChars.Count > passwordChars.Count)
                        passwordChars.Add(AddingChar);
                }
                else
                {
                    while (passwordChars.Count > loginChars.Count)
                        loginChars.Add(AddingChar);
                }

                var products = new List<int>();
                for (int i = 0; i < loginChars.Count; i++)
                {
                    int product = loginChars[i] * passwordChars[i];
                    products.Add(product);
                }
                string code = "";
                foreach (var product in products)
                {
                    code += product.ToString();
                }
                return code;
            }
        }
    }
}
