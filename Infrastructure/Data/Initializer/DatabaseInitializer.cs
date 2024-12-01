using Application.Interfaces;
using Dapper;
using Infrastructure.Data.Initializer.Helpers;

namespace Infrastructure.Data.Initializer
{
    public class DatabaseInitializer
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DatabaseInitializer(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void InitializeDatabase()
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Verifica se o esquema 'dbo' existe, e o cria caso não exista
                        var createSchemaSql = "CREATE SCHEMA IF NOT EXISTS dbo;";
                        connection.Execute(createSchemaSql, transaction: transaction);

                        // Verifica se a tabela existe
                        var tableExists = DatabaseHelper.TableExists(connection, "category", transaction);

                        if (!tableExists)
                        {
                            // Comando SQL para criar a tabela
                            var createTableSql = @"
                            CREATE TABLE dbo.Category (
                                id SERIAL PRIMARY KEY,
                                name VARCHAR(50) UNIQUE,
                                description TEXT
                            );";

                            // Executa o comando de criação da tabela
                            connection.Execute(createTableSql, transaction: transaction);

                            // Comando SQL para inserir dados iniciais
                            var seedDataSql = @"
                            INSERT INTO dbo.Category (name, description) VALUES
                            ('Lanche', 'Sanduíches e hambúrgueres variados'),
                            ('Acompanhamento', 'Batatas fritas, saladas e outros acompanhamentos'),
                            ('Bebida', 'Refrigerantes, sucos e outras bebidas'),
                            ('Sobremesa', 'Sobremesas diversas');";

                            // Executa o comando de inserção de dados
                            connection.Execute(seedDataSql, transaction: transaction);
                        }

                        tableExists = DatabaseHelper.TableExists(connection, "product", transaction);

                        if (!tableExists)
                        {
                            // Comando SQL para criar a tabela
                            var createTableSql = @"
                            CREATE TABLE dbo.Product (
                                id SERIAL PRIMARY KEY,
                                name VARCHAR(100),
                                description TEXT,
                                price NUMERIC(10, 2),
                                category_id INT REFERENCES dbo.Category(id),
                                image VARCHAR(400)
                            );";

                            // Executa o comando de criação da tabela
                            connection.Execute(createTableSql, transaction: transaction);

                            // Comando SQL para inserir dados iniciais
                            var seedDataSql = @"
                            INSERT INTO dbo.Product (name, description, price, category_id) VALUES
                            ('Cheeseburger', 'Hambúrguer com queijo', 10.00, 1),
                            ('Batata Frita', 'Porção de batatas fritas', 5.00, 2),
                            ('Refrigerante', 'Coca-Cola 350ml', 3.00, 3),
                            ('Sorvete', 'Casquinha de sorvete', 4.00, 4);";

                            // Executa o comando de inserção de dados
                            connection.Execute(seedDataSql, transaction: transaction);
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

    }
}
