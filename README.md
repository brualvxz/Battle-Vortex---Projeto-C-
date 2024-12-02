# Battle Vortex

#### Integrantes: Vitor Reis e Bruno Vilela
------------------------------------------------------
### Descrição do Projeto:

O **Battle Vortex** é uma plataforma versátil e adaptável para o cadastro e exibição de eventos de diferentes tipos de jogos, sejam eles digitais ou físicos. A proposta é criar um sistema que organize torneios, gerencie equipes e jogadores, e forneça informações detalhadas sobre prêmios e patrocinadores. A plataforma é construída para suportar jogos como e-Sports (Overwatch, League of Legends) e esportes físicos (futebol, basquete), garantindo que as informações inseridas no sistema sejam aplicáveis ​​e consistentes para qualquer contexto de jogo.

Nosso foco principal é permitir que o usuário tenha uma visão ampla dos eventos de seus jogos favoritos, ajudando jogadores, organizadores e patrocinadores a encontrar oportunidades de competições, acompanhar classificações e descobrir novos talentos. A flexibilidade do sistema permite a adaptação de categorias e termos específicos para cada jogo, sem a necessidade de reestruturações muito complexas.

Para demonstração do sistema, o jogo **Overwatch** será utilizado como referência. Isso nos permitirá a criação de uma base sólida para testar o funcionamento dos cadastros e a organização das informações, com possibilidade de expansão para outros tipos de eventos futuramente.

------------------------------------------------------

### Requisitos Funcionais:

1. **Cadastro de Torneios**: Cadastro de Torneios com informações como nome, dados, local, descrição e regras.
2. **Cadastro de Equipes**: Registro de equipes com informações como nome, membros, logotipo e jogo favorito.
3. **Cadastro de Jogadores**: Registro de jogadores individuais, incluindo apelido, equipe e conquistas.
4. **Gerenciamento de Jogos**: Organização dos jogos dentro do torneio, adaptável para qualquer tipo de jogo.
5. **Gerenciamento de Prêmios**: Cadastro e acompanhamento dos prêmios oferecidos em cada torneio.
6. **Classificação de Jogadores e Equipes**: Geração de tabelas de classificação com base nos resultados dos torneios.
7. **Calendário de Eventos**: Exibição de datas, horários e fases dos campeonatos.
8. **Cadastro de Patrocinadores**: Cadastro de patrocinadores, incluindo informações planejadas dos eventos patrocinados.

------------------------------------------------------

### Requisitos Não Funcionais:

1. **Usabilidade**: Interface intuitiva e responsiva, ajustável a diferentes dispositivos
2. **Escalabilidade**: Sistema capaz de suportar um número crescente de usuários e eventos.
3. **Manutenibilidade**: Código organizado e documentado para facilitar atualizações.

------------------------------------------------------

### Histórias de Usuário:

**Histórias de Cadastro**: 

1. **Cadastro de Torneios**  
    
- Como organizador de torneios (Organizador)
- Posso cadastrar novos torneios, com informações completas sobre nome,    dados e regras
- Para organizar e gerenciar melhor o evento e permitir que jogadores e equipes se inscrevam.

2. **Cadastro de Equipes** 
    
- Como equipe de e-Sports (Usuário)
- Posso registrar a equipe com informações sobre membros e logotipo
- Para participar dos torneios e competições e obter visibilidade na plataforma.

3. **Cadastro de Jogadores** 
    
- Como jogador individual (Usuário)
- Posso me cadastrar e associar meu perfil a uma equipe
- Para participar dos torneios e acompanhar meu desempenho.

4. **Cadastro de Prêmios** 
 
- Como organizador de torneios (Organizador)
- Posso cadastrar os prêmios oferecidos nos torneios, como troféus e patrocínios
- Para atrair mais jogadores e patrocinadores ao evento.

5. **Cadastro de Patrocinadores** 
 
- Como patrocinador (Patrocinador)
- Posso registrar minha empresa, associar meu nome a torneios através dos patrocínios
- Para promover minha marca e apoiar o crescimento do cenário de e-Sports.

6. **Manutenção** 
 
- Como administrador (ADM)
- Posso ter acesso total a todas as informações e poder modifica-las 
- Para controlar a plataforma e promover uma manutenção bem estruturada

**História de Vizualização**:

1. **Visualização de Torneios**
    
- Como Torcedor (Usuário) ou Jogador (Usuário)
- Posso visualizar torneios e eventos
- Para acompanhar os torneios e me inscrever ou apoiar os eventos.

2. **Visualização de Equipes** 
    
- Como organizador (Ornanizador) ou torcedor (Usuário)
- Posso visualizar as equipes participantes dos torneios
- Para entender o contexto dos jogos e acompanhar o progresso de cada equipe.

3. **Visualização de Classificação** 
    
- Como jogador (Usuário) ou torcedor (Usuário)
- Posso visualizar a classificação das equipes e jogadores em cada torneio
- Para acompanhar o desempenho e as conquistas dos meus favoritos.

--------------------------------------------------------
## Tutorial de Uso do Programa de Gerenciamento de Torneios Battle Vortex

### Importe o Banco de Dados

**1.** Clone o repositório do projeto no seu ambiente local.

**2.** Certifique-se de que o MySQL está instalado e rodando no seu sistema.

**3.** Importe o Banco de Dados.

**4.** No diretório do projeto, na pasta "Banco de dados" você encontrará um arquivo SQL (eventosbv.sql).

**5.** Acesse seu gerenciador de banco de dados (MySQL Workbench, phpMyAdmin, etc.).

**6.** Crie um banco de dados com o nome eventosbv.

**7.** Importe o arquivo SQL para popular o banco de dados com as tabelas e dados necessários.

**8.** Abra o projeto no Visual Studio e compile-o para gerar o executável.

**9.** Execute o sistema e aproveite suas funcionalidades.

### Alterando Caminhos de Imagens no Banco de Dados com SQL
**Objetivo:**
> Observação, remova o () também ao inserir a nova raiz

Atualizar os caminhos das imagens nos registros existentes para apontar para a nova localização.

Comando SQL para Atualizar os Caminhos:

```
-- Atualiza o caminho das imagens para os torneios
UPDATE torneios 
SET logo = REPLACE(logo, 'D:\\Battle Vortex\\Imagens\\fotobanco', '(INSERIR AQUI A NOVA RAIZ)\\Battle Vortex\\Imagens\\fotobanco');

-- Atualiza o caminho das imagens para patrocinadores
UPDATE patrocinadores 
SET logo = REPLACE(logo, 'D:\\Battle Vortex\\Imagens\\fotobanco', '(INSERIR AQUI A NOVA RAIZ)\\Battle Vortex\\Imagens\\fotobanco');

-- Atualiza o caminho das imagens para jogadores
UPDATE jogadores 
SET foto = REPLACE(foto, 'D:\\Battle Vortex\\Imagens\\fotobanco', '(INSERIR AQUI A NOVA RAIZ)\\Battle Vortex\\Imagens\\fotobanco');

-- Atualiza o caminho das imagens para equipes
UPDATE equipes 
SET logo = REPLACE(logo, 'D:\\Battle Vortex\\Imagens\\fotobanco', '(INSERIR AQUI A NOVA RAIZ)\\Battle Vortex\\Imagens\\fotobanco');


```

### Alteração no Código do Projeto no Visual Studio 2022
**Passos:**

> Observação, remova o () também ao inserir a nova raiz

+ Abrir o projeto no Visual Studio 2022:

- Abra o Visual Studio 2022.
Vá em Arquivo > Abrir > Projeto/Solução e selecione o projeto "Battle Vortex".

* Localizar o caminho das imagens no código:

Utilize a função Localizar e Substituir pressionando <sub>Ctrl + Shift + F</sub>.
Pesquise pela linha de código

```
string pastaDestino = @"D:\Battle Vortex\Imagens\fotobanco";

```
#### Substitua pela nova raiz, onde as fotos ficarão salvas.

```
string pastaDestino = @"(INSIRA AQUI O NOVO CAMINHO)Battle Vortex\Imagens\fotobanco";


```
### Acessando o Programa
 
**1.1 Login**

Ao abrir o programa, você será solicitado a fazer login. Use suas credenciais (usuário e senha) para acessar o sistema
```
Nome de Usuário ou E-mail
Senha 
```
Se você ainda não tiver uma conta, entre em contato com o administrador para criar um login.

**1.2 Cadastro**

Caso não tenha um login você irá selecionar **"Cadastre-se"** para fazer o seu cadastro. Usando as credencias abaixo para acessar a área de login. 
```
Nome de Usuário
E-mail
Senha
Confirmar Senha
```
**2. Navegando pelo Sistema**

Após o login, você verá a tela inicial do programa. Aqui estão os principais módulos disponíveis no sistema.
```
Torneios.
Equipes.
Jogadores.
Patrocinadores.
Prêmios.
Ranking.
Usuários.
```
Cada módulo possui um conjunto de funcionalidades que facilitam o gerenciamento dos dados.

**3. Gerenciando Torneios**
   
**3.1 Criar um Novo Torneio**

Clique no menu **Participe dos Torneios.**
Clique no botão **Cadastrar novo Torneio.**
Preencha os campos obrigatórios:
```
Nome: Nome do torneio.
Data de Início: Data e hora em que o torneio começa.
Data de Fim: Data e hora de término do torneio.
Local: Local onde o torneio será realizado.
Descrição: Descrição do torneio.
Regras: Regras principais do torneio.
Vagas: Número de equipes que podem participar.
```
Clique em Cadastrar para criar o torneio.

**3.2 Visualizar Torneios**

Acesse o menu Torneios.
Você verá uma lista de todos os torneios cadastrados.
Para visualizar detalhes de um torneio, clique sobre o nome do torneio.

**3.3 Editar Torneio**

Selecione o torneio desejado.
Clique em **Alterar** para modificar os detalhes.
Após realizar as alterações, clique em Alterar para atualizar as informações.

**4. Gerenciando Equipes**

**4.1 Adicionar uma Nova Equipe**

No menu Equipes, clique em **Cadastrar uma nova Equipe.**
Preencha os campos obrigatórios:
```
Nome da Equipe: Nome da equipe.
Logo da Equipe: Selecione a imagem da logo da equipe.
E-mail de Contato: Coloque um E-mail para contato caso seja necessário.
Localidade: Cidade ou país de origem da equipe.
```
Clique em Cadastrar para registrar a equipe.

**4.2 Visualizar Equipes**
No menu Equipes, você verá uma lista de todas as equipes cadastradas.
Para ver os detalhes de uma equipe, clique no nome dela. Você poderá ver os jogadores e os torneios nos quais a equipe está inscrita.

**5. Gerenciando Jogadores**
   
**5.1 Adicionar Jogador**

Acesse o menu **Visualizar Jogadores.**
Clique em **Cadastre-se como Jogador.**
Preencha os campos obrigatórios:
```
Nome do Jogador: Nome completo.
Nickname: Nome ou apelido no jogo.
Equipe: Selecione a equipe à qual o jogador pertence.
Personagem Main: Escolha o personagem principal do jogador.
Conquistas: Liste as conquistas importantes do jogador.
Foto: Foto do jogador.
```
Clique em Cadastrar para adicionar o jogador.

**5.2 Editar Jogador**

No menu Jogadores, clique no jogador que deseja editar.
Altere as informações necessárias e clique em Alterar.

**6. Gerenciando Patrocinadores**

**6.1 Adicionar Patrocinador**

Acesse o menu **Consulte os Patrocinadores.**
Clique em **Cadastrar sua Marca.**
Preencha os campos obrigatórios:
```
Nome do Patrocinador: Nome da empresa ou patrocinador.
Logo: Selecione o logo do patrocinador.
Sobre a Empresa: Contar um pouco sobre a empresa que vai patrocinar.
Evento para Patrocinar: Selecione o evento que deseja patrocinar.
```
Clique em Cadastrar para registrar o patrocinador.

**7. Gerenciando Prêmios**

**7.1 Adicionar Prêmio**

Acesse o menu **Veja os Prêmios.**
Clique em **Cadastrar novo Prêmio.**
Preencha os seguintes campos:
```
Torneio: Selecione o torneio ao qual o prêmio está vinculado.
Origem do Prêmio: De onde o prêmio vem.
Descrição do Prêmio: Descrição do prêmio oferecido.
Tipo de Prêmio: Selecione se é prêmio principal, secundário ou terciário.
Foto do Prêmio: Selecione a imagem representativa do prêmio.
```
Clique em Cadastrar para adicionar o prêmio.

**8. Finalizando a Sessão**
    
Quando terminar de usar o programa, clique em Logout no canto superior direito para encerrar sua sessão.
