# Integração Contínua (CI)

O workflow de CI é automatizado com GitHub Actions e realiza as seguintes etapas:

1. **Quando é Executado:**
    - Inicia em push ou pull request para o branch `main`.

2. **Passos do Workflow:**
    - **Checkout do Repositório:** Obtém o código do repositório.
    - **Configuração do .NET Core:** Prepara o ambiente com a versão 6.x do .NET Core.
    - **Início dos Containers:** Inicia os containers Docker a partir do `docker-compose-integration.yml`.
    - **Restaurar Dependências:** Restaura as dependências do projeto.
    - **Compilar o Projeto:** Compila o projeto.
    - **Executar Testes:** Executa os testes e gera um relatório de resultados.
    - **Login no DockerHub:** Faz login no DockerHub usando credenciais seguras.
    - **Construir e Enviar Imagem Docker:** Constrói e envia a imagem Docker para o DockerHub.

    - [Documentação do GitHub Actions](https://docs.github.com/en/actions)
    - [Documentação do DockerHub](https://docs.docker.com/docker-hub/)
