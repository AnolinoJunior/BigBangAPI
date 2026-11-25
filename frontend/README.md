# Aplicação React da Big Bang API

Frontend em React e Vite para consultar e administrar personagens, episódios e localizações.

## Executar em desenvolvimento

Instale o Node.js e, nesta pasta, execute:

```bash
npm install
```

Crie um arquivo `.env` usando `.env.example` e configure a URL base da API:

```env
VITE_API_BASE_URL=http://localhost:5004/api
```

Inicie o servidor de desenvolvimento:

```bash
npm run dev
```

## Verificar e gerar os arquivos de publicação

```bash
npm run lint
npm run build
```

A compilação de produção é gerada na pasta `dist/`. Para publicar na Somee, envie o conteúdo dessa pasta para a pasta pública do site. Configure `VITE_API_BASE_URL` com a URL pública da API antes de gerar a compilação. Consulte o [guia de publicação](../DEPLOY.md).

Os valores iniciados por `VITE_` ficam públicos no navegador; não os utilize para guardar senhas ou tokens secretos.
