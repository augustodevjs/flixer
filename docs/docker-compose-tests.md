# Executando Testes com Docker Compose

1. **Certifique-se de que o Docker está instalado.**
    - Execute:
      ```bash
      docker --version
      ```
2. **Suba os Containers de Teste:**
    - Execute:
      ```bash
      docker compose -f docker/docker-compose-integration.yml up -d
      ```
3. **Verifique o Status dos Containers:**
    - Use:
      ```bash
      docker compose ps
      ```
4. **Rode os Testes:**
    - Execute:
      ```bash
      dotnet test --no-build --verbosity normal
      ```
5. **Para Parar e Remover os Containers de Teste:**
    - Execute:
      ```bash
      docker compose -f docker/docker-compose-integration.yml down
      ```