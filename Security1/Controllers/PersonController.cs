namespace Security1.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public partial class PersonController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        public const string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;
                                    AttachDbFilename=|DataDirectory|\Database1.mdf;
                                    Integrated Security=True;
                                    Connect Timeout=30;";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            using (var conn = GetDbConnection())
            {
                var command = conn.CreateCommand();
                command.CommandText = $"select * from dbo.person where id={id}";

                var dreader = await command.ExecuteReaderAsync(
                    System.Data.CommandBehavior.Default);

                var personList = new List<Models.Person>();

                while (dreader.Read())
                {
                    personList.Add(new Models.Person()
                    {
                        Id = dreader.GetInt32(0),
                        Name = dreader.GetString(1)
                    });
                }

                return Json(personList, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetPerson(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            using (var conn = GetDbConnection())
            {
                var command = conn.CreateCommand();
                command.CommandText = $"select * from dbo.person where id=@id";
                command.Parameters.Add(new SqlParameter("id", id));
                var dreader = await command.ExecuteReaderAsync(
                    System.Data.CommandBehavior.Default);

                var personList = new List<Models.Person>();

                while (dreader.Read())
                {
                    personList.Add(new Models.Person()
                    {
                        Id = dreader.GetInt32(0),
                        Name = dreader.GetString(1)
                    });
                }

                return Json(personList, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SqlConnection GetDbConnection()
        {
            var con = new SqlConnection();
            con.ConnectionString = connString;
            con.Open();

            return con;
        }
    }
}
