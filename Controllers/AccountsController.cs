using BasicAuth.Helper;
using BasicAuth.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace BasicAuth.Controllers
{
    public class AccountsController : Controller
    {
        private IConfiguration _config;
        CommonHelper _helper;

        public AccountsController(IConfiguration config)
        {
            _config = config;
            _helper = new CommonHelper(_config);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            string connectionString = _config["ConnectionStrings:DefaultConnection"];

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                //Check if user already exists
                string checkQuery = "SELECT 1 FROM users WHERE email=@email OR phone_no=@phone";
                NpgsqlCommand checkCommand = new NpgsqlCommand(checkQuery, connection);
                
                    checkCommand.Parameters.AddWithValue("@email", viewModel.Email);
                    checkCommand.Parameters.AddWithValue("@phone", viewModel.PhoneNo);

                    using (NpgsqlDataReader reader = checkCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            TempData["msg"] = "User Already Exists";
                            return RedirectToAction("Register", "Accounts");
                        }
                    }
                

                // Insert new user
                string insertQuery = "INSERT INTO users (name, email, phone_no, password) VALUES (@name, @email, @phone, @password)";

                using (NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@name", viewModel.Name);
                    insertCommand.Parameters.AddWithValue("@email", viewModel.Email);
                    insertCommand.Parameters.AddWithValue("@phone", viewModel.PhoneNo);
                    insertCommand.Parameters.AddWithValue("@password", viewModel.Password); // ⚠ Later we will hash this

                    int result = insertCommand.ExecuteNonQuery();

                    if (result > 0)
                    {
                        TempData["msg"] = "User Added";
                        return RedirectToAction("Register", "Accounts");
                    }
                }
            }
            return RedirectToAction("Register", "Accounts");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
