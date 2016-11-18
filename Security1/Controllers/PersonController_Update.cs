namespace Security1.Controllers
{
    using System.Net;
    using System.Linq;
    using System.Web.Mvc;
    using System.Threading.Tasks;
    using System.Data.SqlClient;
    using Filters;

    /// <summary>
    /// 
    /// </summary>
    public partial class PersonController
    {
        /// <summary>
        /// 
        /// </summary>
        private const string AuthCookieName = "authcookie";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            var person = default(Models.Person);

            using (var conn = GetDbConnection())
            {
                var command = conn.CreateCommand();
                command.CommandText = $"select * from dbo.person where id=@id";
                command.Parameters.Add(new SqlParameter("id", id));

                var dreader = await command.ExecuteReaderAsync(
                    System.Data.CommandBehavior.Default);

                dreader.Read();

                person = new Models.Person()
                {
                    Id = dreader.GetInt32(0),
                    Name = dreader.GetString(1)
                };
            }

            HttpContext.Response.SetCookie(
                new System.Web.HttpCookie(AuthCookieName, person.Name));

            return View(person);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [AllowCrossSiteRequests]
        [HttpPost]
        public async Task<ActionResult> Update(Models.Person person)
        {
            if (!HttpContext.Request.Cookies.AllKeys.Contains(AuthCookieName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            if (string.IsNullOrWhiteSpace(person.Password))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            var simpleAES = new SimplerAES();
            var encryptedPassword = simpleAES.Encrypt(person.Password);

            using (var conn = GetDbConnection())
            {
                var command = conn.CreateCommand();
                command.CommandText = @"update dbo.person set
                    name = @name, password = @password where id=@id";
                command.Parameters.AddWithValue("id", person.Id);
                command.Parameters.AddWithValue("name", person.Name);
                command.Parameters.AddWithValue("password", encryptedPassword);

                var rows = await command.ExecuteNonQueryAsync();

                return Json($"Rows affected = {rows}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateWithToken(Models.Person person)
        {
            return await Update(person);
        }
    }
}