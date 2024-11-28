Feature: Recuperar produto por ID
  Como consumidor de API
  Quero recuperar um produto específico pelo ID
  Para que eu possa visualizar os detalhes do produto

  Scenario: Recuperar produto com sucesso
    Given que a API tem o produto com Id 1
      | Id | CategoryId | Name     | Description            | Price  | Image         |
      | 3  | 10         | Produto3 | Descricao do produto 1 | 100.50 | image1.jpg    |
    When solicito o produto pelo Id 1
    Then a resposta deve conter o produto com Id 3
    And a resposta deve incluir o nome "Produto3"

  Scenario: Produto nao encontrado
    Given que a API nao tem nenhum produto com Id 99
    When solicito o produto pelo Id 99
    Then a resposta deve ser nula
