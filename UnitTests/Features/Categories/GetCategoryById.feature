Feature: Buscar categoria por Id
  Como consumidor de API
  Quero recuperar uma categoria pelo Id
  Para que eu possa ver as informações de uma categoria específica

  Scenario: Recuperar categoria com sucesso
    Given que a API tem a categoria com Id 1
      | Id | Name        | Description                        |
      | 1  | Lanche      | Sanduiches e hamburgueres variados |
    When solicito a categoria pelo Id 1
    Then a resposta deve conter a categoria com Id 1
    And a resposta devera incluir o nome "Lanche"
