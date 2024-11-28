Feature: Recuperar produtos
  Como consumidor de API
  Quero recuperar todos os produtos
  Para que eu possa visualizar a lista de produtos disponíveis

  Scenario: Recuperar todos os produtos com sucesso
    Given que a API tem os seguintes produtos
      | Id | CategoryId | Name    | Description         | Price | Image         |
      | 1  | 10         | Produto1 | Descricao do produto 1 | 100.50 | image1.jpg    |
      | 2  | 20         | Produto2 | Descricao do produto 2 | 200.75 | image2.jpg    |
    When solicito todos os produtos
    Then a resposta deve conter 2 produtos
    And a resposta deve incluir um produto com o nome "Produto1"

  Scenario: Recuperar produtos por lista de IDs
    Given que a API tem os seguintes produtos
      | Id | CategoryId | Name    | Description         | Price | Image         |
      | 1  | 10         | Produto1 | Descricao do produto 1 | 100.50 | image1.jpg    |
      | 2  | 20         | Produto2 | Descricao do produto 2 | 200.75 | image2.jpg    |
    When solicito produtos com os IDs "1,2"
    Then a resposta deve conter 2 produtos
    And a resposta deve incluir um produto com o nome "Produto2"
