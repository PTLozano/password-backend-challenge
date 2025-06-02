<a id="readme-top"></a>

<br />
<div  align="center">
  <a href="https://github.com/PTLozano/password-backend-challenge">
   <h2>Password Backend Challenge</h2>
  </a>

  <p>
    Este projeto pode ser utilizado por outros sistemas para auxiliar na validação de senhas conforme a configuração das regras.
  </p>
</div>
<br />

<summary>👉 Índice</summary>
  <ol>
    <li><a href="#sobre-o-projeto">📄 Sobre o Projeto</a></li>
    <li>
      <a href="#estrutura">📂 Estrutura</a>
      <ul>
         <li><a href="#password-backend-challenge-src">📁 SRC</a></li>
         <ul>
           <li><a href="#password-backend-challenge-src-presentation">🧑🏻‍💻 Presentation</a></li>
           <li><a href="#password-backend-challenge-src-application">⚙️ Application</a></li>
           <li><a href="#password-backend-challenge-src-shared">🗂️ Shared</a></li>
         </ul>
      </ul>
      <ul>
         <li><a href="#password-backend-challenge-tests">🧪 TESTS</a></li>
         <ul>
           <li><a href="#password-backend-challenge-tests-project">🧪 Tests</a></li>
         </ul>
      </ul>
    </li>
    <li><a href="#documentacao">📑 Documentação</a></li>
    <li>
      <a href="#comecando">➡️ Começando</a>
      <ul>
        <li><a href="#tecnologias">🛠️ Tecnologias</a></li>
        <li><a href="#pre-requisitos">🚧 Pré-requisito</a></li>
        <li>
            <a href="#instalacao">⚙️ Instalação</a>
            <ul>
              <li><a href="#instalacao-local">💻 Build e Execução Local</a></li>
              <li><a href="#instalacao-docker">🐳 Execução via Docker</a></li>
            </ul>
        </li>
        <li><a href="#urls">🔗 URLs</a></li>
        <li>
            <a href="#seguranca">🔐 Segurança</a>
            <ul>
              <li><a href="#seguranca-api-key">🔑 API Key</a></li>
              <li><a href="#seguranca-rate-limit">🚪 Rate Limit</a></li>
            </ul>
        </li>
      </ul>
    </li>
    <li><a href="#testes">🧪 Testes</a></li>
  </ol>

<br />
<br /> 

<a id="sobre-o-projeto"></a>

# 📝 Sobre o Projeto

<p>Este projeto visa auxiliar que outros sistemas possam utilizar de uma forma simplificada a validação de senhas conforme
a necessidade, utilizando as regras de negócio definidas no projeto.</p>

<p>A estrutura do serviço está divida em projetos que estão especificados na próxima seção. 
O desenvolvimento foi feito desta forma para que seja possível a reutilização de código caso o projeto venha a crescer e 
para facilitar a manutenção do projeto.</p>

<p>O projeto foi desenvolvido pensando em possibilitar, a quem for utilizar, adaptar as regras de senha conforme a necessidade
de cada sistema, esta configuração é realizada utilizando o arquivo appsettings.json.</p>

<p>A ideia por trás do motor de regras que faz a validação é a de que independente de quais caracteres sejam utilizados, 
podendo até mesmo serem combinados em apenas uma regra, como por exemplo "ABCabc123!@#", o motor seja capaz de validar
e dar um retorno de sucesso ou de erro caso a senha não atenda às regras definidas.</p>

<p>O motor de regras foi feito utilizando o <code>Dictionary</code> para armazenar os caracteres que foram validados e verificar quantas
vezes já foram inseridos, foi utilizado pela sua característica de não permitir chavez (caracteres neste caso) duplicadas, já
tendo essa validação de forma nativa e sem a necessidade de novas validações de forma manual.<br>
Após percorrer a regra, chamada de complexidade internamente, o sistema valida a quantidade de cada caracter e a quantidade mínima e máxima,
se houver algum problema que não respeite a regra, o sistema irá encerrar no momento e retornará como não válido e qual o erro encontrado.<br>
Ao finalizar toda a validação, o sistema vai verificar a soma de todos os caracteres verificados e a quantidade de caracteres informados na senha,
caso a quantidade seja diferente é porque há caracteres não válidos e retornará informando o problema.</p>

<p></p>

<p align="right">(<a href="#readme-top">volta ao topo</a>)</p>

<a id="estrutura"></a>

# 📂 Estrutura

A estrutura do projeto é a seguinte:
<p id="password-backend-challenge-src">📁 SRC</p>
<ul>
    <li id="password-backend-challenge-src-presentation">
      <a href="https://github.com/PTLozano/password-backend-challenge/tree/main/src/PasswordBackendChallenge.Presentation">🧑🏻‍💻 Presentation</a>
      <p>Fornece a API para que seja realizado o envio da senha e configuração das regras da senha.</p>
    </li>
    <li id="password-backend-challenge-src-application">
      <a href="https://github.com/PTLozano/password-backend-challenge/tree/main/src/PasswordBackendChallenge.Application">⚙️ Application</a>
      <p>Responsável por receber a senha e as regras para validar e devolver com o status e mensagem de erro, caso ocorra.</p>
    </li>
    <li id="password-backend-challenge-src-shared">
      <a href="https://github.com/PTLozano/password-backend-challenge/tree/main/src/PasswordBackendChallenge.Shared">🗂️ Shared</a>
      <p>Classes e records que são compartilhados por todo o projeto.</p>
    </li>
</ul>
<p id="password-backend-challenge-tests">🧪 TESTS</p>
<ul>
    <li id="password-backend-challenge-tests-project">
      <a href="https://github.com/PTLozano/password-backend-challenge/tree/main/tests/PasswordBackendChallenge.Tests">🧪 Tests</a>
      <p>Testes dos projetos, validando regras, lógicas e integração.</p>
    </li>
</ul>

<p align="right">(<a href="#readme-top">volta ao topo</a>)</p>

<a id="documentacao"></a>

# 📑 Documentação

<p>A API que faz a validação se encontra no endereço <code>.../api/password/validate</code> (a URL vai depender de como e onde está sendo executado o projeto).<br>
Para conseguir utilizar é necessário fazer uma requisição POST para o endereço informado enviando o seguinte modelo JSON na estrutura do corpo:

```json
{
    "password": "Senha a ser validada"
}
```
Após a validação da senha será retornado um novo objeto informando se é uma senha válida ou inválida e, neste caso, qual o motivo.<br>
Exemplo de retorno de uma senha válida (código de retorno 200):
```json
{
    "isValid": true
}
```
Exemplo de retorno de uma senha inválida (código de retorno 400):
```json
{
    "isValid": false,
    "message": "Senha inválida, contém caracteres não permitidos"
}
```
</p>
<p>É possível fazer alterações nas regras de validação da senha, para isso é necessário alterar o arquivo appsettings.json do projeto Presentation.</p>
<p>Neste arquivo existe o <code>PasswordSettings</code> onde é possível definir a quantidade mínima e máxima de caracteres e a complexidade da senha.</p>
<p>A complexidade da senha permite definir a quantidade mínima, máxima e quais caracteres são permitidos, a quantidade máxima de caracteres repetidos para o tipo, identificador e se está habilitado.</p>
<p>Segue o exemplo desta seção no arquivo:</p>

```json
{
   ...,
   "PasswordSettings": {
      "MinimumPasswordLength": 9, // Quantidade mínima de caracteres da senha
      "MaximumPasswordLength": 40, // Quantidade máxima de caracteres da senha
      "Complexity": {
         "Uppercase": {
            "Enabled": true, // Habilita a validação da complexidade
            "MinimumLength": 1, // Quantidade mínima de caracteres para este tipo
            "MaximumLength": 10, // Quantidade máxima de caracteres para este tipo
            "MaximumRepeatCharCount": 1, // Quantidade máxima de caracteres repetidos para este tipo
            "Identifier": "letras maiúsculas", // Identificador para este tipo de complexidade
            "Characters": "ABCDEFGHIJKLMNOPQRSTUVWXYZ" // Caracteres permitidos para este tipo de complexidade
         },
         "Lowercase": {
            "Enabled": true,
            "MinimumLength": 1,
            "MaximumLength": 10,
            "MaximumRepeatCharCount": 1,
            "Identifier": "letras minúsculas",
            "Characters": "abcdefghijklmnopqrstuvwxyz"
         },
         "Digit": {
            "Enabled": true,
            "MinimumLength": 1,
            "MaximumLength": 10,
            "MaximumRepeatCharCount": 1,
            "Identifier": "números",
            "Characters": "0123456789"
         },
         "NonAlphanumeric": {
            "Enabled": true,
            "MinimumLength": 1,
            "MaximumLength": 10,
            "MaximumRepeatCharCount": 1,
            "Identifier": "caracteres especiais",
            "Characters": "!@#$%^&*()-+"
         }
      }
   }
}
```
> _**Nota:**_ É possível que em versões futuras do projeto estas configurações sejam adicionadas em um sistema de 
> configuração como o Consul, Delinea ou mesmo no ConfigMap do Kubernetes.

> _**Nota 2:**_ Para o uso local, o sistema usa a porta <code>5254</code> (definido no projeto Presentation, no arquivo <code>launchSettings.json</code> da
> pasta Properties) para a API, mas é possível alterar a porta de acordo com a necessidade do usuário.<br>
> Para utilizar no Docker, a porta <code>5000</code> é mapeada para a porta <code>8080</code> do container, mas também é possível alterar a porta de acordo com a necessidade do usuário,
> neste caso precisará alterar o parâmetro <code>-p</code> do comando <code>run</code> e no Dockerfile na raiz do projeto.<br>
> Outra porta que é mapeada é a <code>9096</code>, que é utilizada para as métricas da aplicação.


<p align="right">(<a href="#readme-top">volta ao topo</a>)</p>


<a id="comecando"></a>

# ➡️ Começando

O projeto utiliza as tecnologias da próxima seção. Para executar é necessário ter os pré-requisitos e permissões de
acesso,
conforme for o caso.

<p align="right">(<a href="#readme-top">volta ao topo</a>)</p>

<a id="tecnologias"></a>

## 🛠️ Tecnologias

![.NET 8](https://img.shields.io/badge/.NET-512BD4.svg?style=for-the-badge&logo=dotnet&logoColor=white)![Github](https://img.shields.io/badge/github-121013?style=for-the-badge&logo=github&logoColor=white)![Docker](https://img.shields.io/badge/Docker-2496ED.svg?style=for-the-badge&logo=Docker&logoColor=white)

<a id="pre-requisitos"></a>

## 🚧 Pré-requisitos

É possível executar o projeto de duas maneiras: fazendo o build e executando diretamente o projeto ou através de um container Docker.
Para isso é necessários atender a alguns pré-requisitos, são eles:

1. SDK do .NET 8: Certifique-se de que você tenha o SDK do .NET 8 (ou versão mais nova) instalado no seu ambiente de desenvolvimento.

2. Docker: Tenha o Docker (ou outro semelhante como o Podman) instalado e configurado na sua máquina local para criar e gerenciar containers.

<p align="right">(<a href="#readme-top">volta ao topo</a>)</p>

<a id="instalacao"></a>

## ⚙️ Instalação

<a id="instalacao-local"></a>
### 💻 Build e Execução Local
<p>Para executar o projeto localmente, siga os passos abaixo:</p>

1. Clone o repositório:
   ```sh
   git clone https://github.com/PTLozano/password-backend-challenge
   ```

2. Navegue até o diretório do projeto:
   ```sh
   cd password-backend-challenge
   ```
3. Restaure as dependências do projeto:
   ```sh
   dotnet restore
   ```
4. Compile o projeto:
   ```sh
   dotnet build
   ```
5. Entre no diretório do projeto Presentation:
   ```sh
   cd src/PasswordBackendChallenge.Presentation
   ```
6. Execute o projeto:
   ```sh
   dotnet run
   ```
7. Acesse a API no navegador ou através de ferramentas como Postman:
   ```
   http://localhost:5254/api/password/validate
   ```

<p align="right">(<a href="#readme-top">volta ao topo</a>)</p>

<a id="instalacao-docker"></a>
### 🐳 Execução via Docker
<p>Para executar o projeto em um contêiner Docker, siga os passos abaixo:</p>

1. Navegue até o diretório do projeto:
   ```sh
   cd password-backend-challenge
   ```

2. Crie a imagem Docker:
   ```sh
   docker build -t password-backend-challenge .
   ```
   
3. Execute o contêiner Docker:
   ```sh
   docker run -d -p 5000:8080 -p 9096:9096 password-backend-challenge
   ```
   
4. Acesse a API no navegador ou através de ferramentas como Postman:
   ```
   http://localhost:5000/api/password/validate
   ```

<p align="right">(<a href="#readme-top">volta ao topo</a>)</p>

<a id="urls"></a>

## 🔗 URLs

<p>O sistema possui os seguintes endpoints:</p>
<ul>
    <li>API principal do projeto: (POST) <code>/api/password/validate</code></li>
    <li>Swagger da aplicação (disponível em ambientes não produtivos): (GET) <code>/swagger</code></li>
    <li>Health Check para saber se a aplicação está no ar ou não: (GET) <code>/healthy</code></li>
    <li>Métricas da aplicação para que possam ser lida por sistemas externos: (GET) <code>/metrics</code></li>
</ul>

<p align="right">(<a href="#readme-top">volta ao topo</a>)</p>

<a id="seguranca"></a>

## 🔐 Segurança

<p>O projeto possui algumas medidas de segurança para proteger os endpoints e garantir que apenas usuários autorizados possam
acessar as funcionalidades além de poder limitar a quantidade de requisições para a API.</p>

              
<a id="seguranca-api-key"></a>
### 🔑 API Key
<p>O projeto possui a possibilidade de utilizar API Key para que seja possível proteger o acesso aos endpoints.<br>
O sistema vai validar se a chave de API está presente e é válida antes de processar a requisição.
</p>
<p>Por padrão o sistema não vem ativado, permitindo realizar as requisições sem a necessidade da chave, para ativar é necessário 
informar nas variáveis de ambiente o valor para a chave <code>x-api-key</code>.<br>
Após ativado, será necessário adicionar a chave de API no cabeçalho da requisição, utilizando o seguinte formato:</p>

```plaintext
   X-Api-Key: sua_chave_de_api
```
> _**Nota:**_ A API Key não é configurável através do arquivo appsettings.json como as demais configurações por questão de segurança.
> Não é recomendado adicionar tais informações sensíveis em arquivos de texto, pois podem ser acessada por pessoas não autorizadas.

<p align="right">(<a href="#readme-top">volta ao topo</a>)</p>

<a id="seguranca-rate-limit"></a>
### 🚪 Rate Limit

<p>O projeto possui a possibilidade de limitar a quantidade de requisições que podem ser feitas para a API, evitando abusos e
protegendo o sistema contra ataques de negação de serviço (DoS).</p>
<p>O sistema está configurado para que caso seja ativado, faça a verificação de qual endereço está sendo requisitado e se for 
um dos que está sendo limitado (no caso o de validação de senha), irá bloquear caso exceda a quantidade de requisições por período
considerando o IP do cliente.</p>
<p>Por padrão o sistema não vem ativado, permitindo realizar as requisições sem limites de quantidade de requisições.</p>
<p>Para ativar é necessário informar no arquivo appsettings o valor para o objeto <code>RateLimitSettings</code> como no exemplo a seguir:<br>

```json
{
   ...,
   "RateLimitSettings":{
      "Enabled": true, // Habilita o rate limit
      "PermitLimit": 10, // Quantidade de requisições permitidas
      "WindowInSeconds": 600 // Tempo em segundos para o limite de requisições
   }
}
```

<p align="right">(<a href="#readme-top">volta ao topo</a>)</p>

<a id="testes"></a>

# 🧪 Testes

O projeto inclui testes unitários para garantir que as funcionalidades estejam funcionando corretamente e prevenir
regressões no código. Os testes podem ser executados utilizando o seguinte comando:

```sh
   dotnet test
```

<p align="right">(<a href="#readme-top">volta ao topo</a>)</p>