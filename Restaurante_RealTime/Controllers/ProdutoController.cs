using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Data;
using Web_restaurante_RealTime.Entidades;

namespace Web_restaurante_RealTime.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly string? _connectionString;

        //ctor -> criar o construtor 
        public ProdutoController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection OpenConnection()
        {
            IDbConnection dbConnection = new SqliteConnection(_connectionString);
            dbConnection.Open();
            return dbConnection;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            using IDbConnection dbConnection = OpenConnection();
            string sql = "select id, nome, descricao, imagemUrl from Produto where id = @id;";
            var produto = await dbConnection.QueryAsync<Produto>(sql, new { id });

            dbConnection.Close();

            if (produto == null)
                return NotFound();

            return Ok(produto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto produto)
        {
            using IDbConnection dbConnection = OpenConnection();
            string query = @"insert into produto values (@id, @nome, @descricao, @imagemUrl";

            await dbConnection.ExecuteAsync(query, produto);
            return Ok();
        }


    }
}
