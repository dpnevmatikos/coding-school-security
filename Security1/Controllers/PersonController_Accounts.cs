namespace Security1.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using System.Threading.Tasks;
    using System.Data.SqlClient;

    public partial class PersonController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> CheckPassword(int id)
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

            return View(person);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckPassword(Models.Person person)
        {
            var simpleAES = new SimplerAES();
            var encryptedPassword = simpleAES.Encrypt(person.Password);
            var currentPassword = GetPersonPassword(person.Id);

            if (!currentPassword.Equals(encryptedPassword))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        private string GetPersonPassword(int personId)
        {
            using (var conn = GetDbConnection())
            {
                var command = conn.CreateCommand();
                command.CommandText = $"select password from dbo.person where id=@id";
                command.Parameters.Add(new SqlParameter("id", personId));
                var dreader = command.ExecuteReader(
                    System.Data.CommandBehavior.Default);

                dreader.Read();

                return dreader.GetString(0);
            }
        }
    }
}