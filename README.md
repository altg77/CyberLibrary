# Cyber Library
![Logo](Imagens/white.png)
---

## Visão Geral

O Cyber Library é um sistema completo de gerenciamento de biblioteca, projetado para atender tanto bibliotecários quanto usuários finais. Ele oferece funcionalidades robustas para o gerenciamento de livros, empréstimos, usuários e categorias, além de fornecer uma interface intuitiva para a interação dos leitores com o acervo.

![Apresentavel](Imagens/Cyber.PNG)

---

## Funcionalidades

### Área do Usuário

* **Início**: Dashboard principal para o usuário, exibindo as últimas novidades e recomendações.
* **Meus Livros**: Lista dos livros atualmente em posse do usuário.
* **Empréstimos**: Histórico e status dos empréstimos ativos do usuário.
* **Devolvidos**: Registro dos livros que já foram devolvidos pelo usuário.

### Área do Bibliotecário

* **Dashboard**: Painel de controle centralizado com métricas e resumos importantes para o bibliotecário.
* **Empréstimos**: Gerenciamento completo de empréstimos, incluindo registro, devolução e consulta.
* **Cadastro de Livros**: Funcionalidade para adicionar, editar e remover livros do acervo.
* **Setores**: Gerenciamento dos diferentes setores ou prateleiras da biblioteca.
* **Categorias**: Criação e organização das categorias de livros.
* **Usuários**: Gerenciamento de usuários, incluindo cadastro, edição e consulta de permissões.
* **Relatórios**: Geração de relatórios sobre o acervo, empréstimos e atividades dos usuários.

---

## Como Iniciar

Siga os passos abaixo para configurar e executar o Cyber Library em seu ambiente local:

1.  **Crie a base de dados**: No seu sistema de gerenciamento de banco de dados (por exemplo, SQL Server, PostgreSQL, MySQL), crie uma nova base de dados com o nome `cyberlib`.
2.  **Aplique as migrações**: Navegue até o diretório raiz do projeto no seu terminal e execute o seguinte comando para aplicar as migrações do Entity Framework Core e criar o esquema do banco de dados:

    ```bash
    dotnet ef migrations add
    ```

    ```bash
    dotnet ef database update
    ```

3.  **Execute a aplicação**: Para iniciar o projeto em modo de desenvolvimento com hot reload, utilize:

    ```bash
    dotnet watch
    ```

    Ou, para executar a aplicação normalmente:

    ```bash
    dotnet run
    ```

    A aplicação estará disponível em `https://localhost:XXXX` (a porta será exibida no terminal).

---

## Contribuição

Contribuições são bem-vindas! Se você tiver alguma sugestão, melhoria ou encontrar um bug, sinta-se à vontade para abrir uma *issue* ou enviar um *pull request*.

---