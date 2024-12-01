Feature: Buscar todas as categorias
  Como consumidor de API
  Quero recuperar todas as categorias
  Para que eu possa ver a lista de categorias

  Scenario: Recuperar todas as categorias com sucesso
    Given que a API tem as seguintes categorias
      | Id | Name           | Description                                       |
      | 1  | Lanche         | Sanduiches e hamburgueres variados                |
      | 2  | Acompanhamento | Batatas fritas, saladas e outros acompanhamentos  |
    When solicito todas as categorias
    Then a resposta deve conter 2 categorias
    And a resposta devera incluir uma categoria com o nome "Lanche"