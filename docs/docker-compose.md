# Executando a API e o Banco de Dados com Docker Compose

1. **Certifique-se de que o Docker está instalado.**
    - Execute:
      ```bash
      docker --version
      ```
2. **Suba os Containers:**
    - Execute:
      ```bash
      docker compose -f docker/docker-compose.yml up -d
      ```
3. **Verifique o Status dos Containers:**
    - Use:
      ```bash
      docker compose ps
      ```
4. **Acesse a Aplicação:**
    - A API estará disponível em [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html).

5. **Para Parar e Remover os Containers:**
    - Execute:
      ```bash
      docker compose -f docker/docker-compose.yml down
      ```
