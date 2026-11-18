# React + Vite

Este modelo inclui a configuração básica para usar React no Vite com HMR e algumas regras do Oxlint.

Há dois plugins oficiais disponíveis.

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react) usa [Oxc](https://oxc.rs).
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react-swc) usa [SWC](https://swc.rs/).

## React Compiler

O React Compiler vem desativado neste modelo devido ao impacto no desempenho durante o desenvolvimento e o build. Consulte a documentação para ativá-lo. [Instalação do React Compiler](https://react.dev/learn/react-compiler/installation)

## Como ampliar a configuração do Oxlint

Para aplicações em produção, recomendamos usar TypeScript e habilitar as regras de lint que verificam os tipos. Consulte o modelo em TypeScript para integrar essas regras do Oxlint ao projeto. [Modelo React + Vite com TypeScript](https://github.com/vitejs/vite/tree/main/packages/create-vite/template-react-ts)
