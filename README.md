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
