-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Tempo de geração: 28/11/2024 às 02:48
-- Versão do servidor: 10.4.32-MariaDB
-- Versão do PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Banco de dados: `eventosbv`
--

-- --------------------------------------------------------

--
-- Estrutura para tabela `equipes`
--

CREATE TABLE `equipes` (
  `id` int(11) NOT NULL,
  `nome` varchar(100) NOT NULL,
  `logo` text DEFAULT NULL,
  `localidade` varchar(100) NOT NULL,
  `capitao_id` int(11) DEFAULT NULL,
  `data_criacao` date DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `equipes`
--

INSERT INTO `equipes` (`id`, `nome`, `logo`, `localidade`, `capitao_id`, `data_criacao`, `email`) VALUES
(1, 'Equipe Alpha', NULL, 'Cidade A', NULL, NULL, NULL),
(2, 'Equipe Beta', NULL, 'Cidade B', NULL, NULL, NULL),
(3, 'Equipe Gamma', NULL, 'Cidade C', NULL, NULL, NULL),
(4, 'Equipe Delta', NULL, 'Cidade D', NULL, NULL, NULL),
(5, 'Equipe Epsilon', NULL, 'Cidade E', NULL, NULL, NULL),
(6, 'Equipe Zeta', NULL, 'Cidade F', NULL, NULL, NULL),
(7, 'noppa', 'D:\\Battle Vortex\\Imagens\\fotobanco\\jinx 1.gif', 'sp', NULL, '2024-11-24', 'vitor@gmail.com'),
(8, 'caio team', 'D:\\Battle Vortex\\Imagens\\fotobanco\\jinx.jpeg', 'sp', 13, '2024-11-24', 'caio@gmail.com'),
(9, 'noppa equipe', 'D:\\Battle Vortex\\Imagens\\fotobanco\\VAPORWAVE TOKYO CITY ANIME TEE ( 2 COLORS ).gif', 'go', 14, '2024-11-24', 'noppa1234@gmail.com');

--
-- Acionadores `equipes`
--
DELIMITER $$
CREATE TRIGGER `trigger_verifica_nome_equipe` BEFORE INSERT ON `equipes` FOR EACH ROW BEGIN
    DECLARE equipe_count INT;
    
    SELECT COUNT(*)
    INTO equipe_count
    FROM equipes
    WHERE nome = NEW.nome;

    IF equipe_count > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Erro: Já existe uma equipe com esse nome.';
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estrutura para tabela `equipes_inscritas`
--

CREATE TABLE `equipes_inscritas` (
  `id` int(11) NOT NULL,
  `torneio_id` int(11) NOT NULL,
  `equipe_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Acionadores `equipes_inscritas`
--
DELIMITER $$
CREATE TRIGGER `trigger_diminui_vagas` AFTER INSERT ON `equipes_inscritas` FOR EACH ROW BEGIN
    UPDATE torneios
    SET vagas = vagas - 1
    WHERE id = NEW.torneio_id;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trigger_verifica_vagas_disponiveis` BEFORE INSERT ON `equipes_inscritas` FOR EACH ROW BEGIN
    DECLARE vagas_restantes INT;

    -- Verifica quantas vagas ainda estão disponíveis no torneio
    SELECT vagas INTO vagas_restantes
    FROM torneios
    WHERE id = NEW.torneio_id;

    -- Se não houver vagas disponíveis, lança um erro
    IF vagas_restantes <= 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Erro: Não há mais vagas disponíveis para este torneio.';
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estrutura para tabela `equipes_jogadores`
--

CREATE TABLE `equipes_jogadores` (
  `id` int(11) NOT NULL,
  `jogador_id` int(11) NOT NULL,
  `equipe_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `eventos_patrocinadores`
--

CREATE TABLE `eventos_patrocinadores` (
  `id` int(11) NOT NULL,
  `torneio_id` int(11) NOT NULL,
  `patrocinador_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `jogadores`
--

CREATE TABLE `jogadores` (
  `id` int(11) NOT NULL,
  `nome` varchar(100) NOT NULL,
  `nickname` varchar(100) DEFAULT NULL,
  `equipe_id` int(11) DEFAULT NULL,
  `personagemMain_id` int(11) DEFAULT NULL,
  `conquistas` text DEFAULT NULL,
  `foto` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `jogadores`
--

INSERT INTO `jogadores` (`id`, `nome`, `nickname`, `equipe_id`, `personagemMain_id`, `conquistas`, `foto`) VALUES
(3, 'Carlos Pereira', 'ArrowMaster', 1, 3, 'Melhor Arqueiro 2023', NULL),
(7, 'Vitor', 'vitzx', 1, 2, 'Programador Infernal', 'D:\\Battle Vortex\\Imagens\\fotobanco\\Simon🎀.jpg'),
(9, 'caiopa', 'caiopa3', NULL, 10, 'dfkljsdfjsdfjkndfg', 'D:\\Battle Vortex\\Imagens\\fotobanco\\Results for quiz whats your secret turn on.jpg'),
(10, 'gato', 'rolinha', 1, 13, 'sou viadao', 'D:\\Battle Vortex\\Imagens\\fotobanco\\Staring_Cat_meme.png'),
(12, 'noppa', 'nopinha', 8, 19, 'melhor porcao do jogo', 'G:\\Battle Vortex\\Imagens\\fotobanco\\GbodDITXsAAYzZO.jpeg'),
(13, 'caiopa04', 'caiopaaa', 8, 14, 'Nao que eu me lembre', 'G:\\Battle Vortex\\Imagens\\fotobanco\\$$.jpg'),
(14, 'tsuru', 'tsuru come cu', 9, 14, 'asdasdadad', 'G:\\Battle Vortex\\Imagens\\fotobanco\\jinx.jpeg');

-- --------------------------------------------------------

--
-- Estrutura para tabela `patrocinadores`
--

CREATE TABLE `patrocinadores` (
  `id` int(11) NOT NULL,
  `nome` varchar(100) NOT NULL,
  `conquistas` text NOT NULL,
  `logo` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `patrocinadores`
--

INSERT INTO `patrocinadores` (`id`, `nome`, `conquistas`, `logo`) VALUES
(1, 'Empresa A', 'Apoio a eventos esportivos', 'logo_empresa_a.png'),
(2, 'Empresa B', 'Patrocínio de esportes regionais', 'logo_empresa_b.png');

-- --------------------------------------------------------

--
-- Estrutura para tabela `personagens`
--

CREATE TABLE `personagens` (
  `id` int(11) NOT NULL,
  `nome` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `personagens`
--

INSERT INTO `personagens` (`id`, `nome`) VALUES
(1, 'Ana'),
(2, 'Ashe'),
(3, 'Baptiste'),
(4, 'Bastion'),
(5, 'Brigitte'),
(6, 'D.Va'),
(7, 'Doomfist'),
(8, 'Echo'),
(9, 'Genji'),
(10, 'Hanzo'),
(11, 'Junkrat'),
(12, 'Lucio'),
(13, 'McCree'),
(14, 'Mei'),
(15, 'Orisa'),
(16, 'Pharah'),
(17, 'Reaper'),
(18, 'Reinhardt'),
(19, 'Roadhog'),
(20, 'Sigma'),
(21, 'Soldier: 76'),
(22, 'Sombra'),
(23, 'Symmetra'),
(24, 'Torbjorn'),
(25, 'Tracer'),
(26, 'Widowmaker'),
(27, 'Zarya'),
(28, 'Zenyatta');

-- --------------------------------------------------------

--
-- Estrutura para tabela `premios`
--

CREATE TABLE `premios` (
  `id` int(11) NOT NULL,
  `torneio_id` int(11) NOT NULL,
  `descricao` varchar(255) NOT NULL,
  `premio_principal` varchar(255) DEFAULT NULL,
  `premio_secundario` varchar(255) DEFAULT NULL,
  `patrocinador_id` int(11) DEFAULT NULL,
  `tipo_origem` enum('Evento','Patrocinador') NOT NULL DEFAULT 'Evento',
  `premio_terciario` varchar(255) DEFAULT NULL,
  `logo_premio_principal` text DEFAULT NULL,
  `logo_premio_secundario` text DEFAULT NULL,
  `logo_premio_terciario` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `premios`
--

INSERT INTO `premios` (`id`, `torneio_id`, `descricao`, `premio_principal`, `premio_secundario`, `patrocinador_id`, `tipo_origem`, `premio_terciario`, `logo_premio_principal`, `logo_premio_secundario`, `logo_premio_terciario`) VALUES
(4, 3, 'dadasd', 'trofeu', 'Sacanagemmmm', 2, 'Patrocinador', 'pinto', 'D:\\Battle Vortex\\Imagens\\fotobanco\\850348.jpg', 'D:\\Battle Vortex\\Imagens\\fotobanco\\829522.jpg', 'D:\\Battle Vortex\\Imagens\\fotobanco\\1281807.png'),
(5, 2, 'adffafaggsdg', '$90000', '$50000', NULL, 'Evento', '$500', 'D:\\Battle Vortex\\Imagens\\fotobanco\\gang souls.jpg', 'D:\\Battle Vortex\\Imagens\\fotobanco\\98a98ebf-72da-4fc5-b908-f0369227c408.jpg', 'D:\\Battle Vortex\\Imagens\\fotobanco\\j.jpg');

-- --------------------------------------------------------

--
-- Estrutura para tabela `rankings`
--

CREATE TABLE `rankings` (
  `id` int(11) NOT NULL,
  `torneio_id` int(11) NOT NULL,
  `equipe_id` int(11) NOT NULL,
  `posicao` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `rankings`
--

INSERT INTO `rankings` (`id`, `torneio_id`, `equipe_id`, `posicao`) VALUES
(1, 1, 1, 1),
(2, 1, 2, 2),
(3, 1, 3, 3),
(4, 1, 4, 4),
(5, 1, 5, 5),
(6, 2, 2, 1),
(7, 2, 3, 2),
(8, 2, 4, 3),
(9, 2, 5, 4),
(10, 2, 6, 5),
(11, 3, 1, 1),
(12, 3, 3, 2),
(13, 3, 4, 3),
(14, 3, 5, 4),
(15, 3, 6, 5),
(19, 1, 1, 1),
(20, 1, 2, 2),
(21, 1, 4, 3),
(22, 1, 4, 4),
(23, 1, 4, 5),
(24, 1, 4, 6),
(25, 1, 4, 1),
(26, 1, 4, 2),
(27, 1, 4, 3),
(28, 3, 2, 1),
(29, 3, 2, 2),
(30, 3, 2, 3);

-- --------------------------------------------------------

--
-- Estrutura para tabela `torneios`
--

CREATE TABLE `torneios` (
  `id` int(11) NOT NULL,
  `nome` varchar(100) NOT NULL,
  `data_inicio` datetime NOT NULL,
  `data_fim` datetime NOT NULL,
  `local` varchar(100) DEFAULT NULL,
  `descricao` text DEFAULT NULL,
  `regras` text DEFAULT NULL,
  `vagas` int(11) NOT NULL,
  `status` enum('Em andamento','Concluído') DEFAULT 'Em andamento',
  `logo` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `torneios`
--

INSERT INTO `torneios` (`id`, `nome`, `data_inicio`, `data_fim`, `local`, `descricao`, `regras`, `vagas`, `status`, `logo`) VALUES
(1, 'Torneio A', '2024-12-01 10:00:00', '2024-12-05 18:00:00', 'Estádio A', 'Descrição do Torneio A', 'Regras do Torneio A', 10, 'Concluído', 'D:\\Battle Vortex\\Imagens\\fotobanco\\jinx.jpeg'),
(2, 'Torneio B', '2024-12-10 10:00:00', '2024-12-12 18:00:00', 'Estádio B', 'Descrição do Torneio B', 'Regras do Torneio B', 8, 'Concluído', NULL),
(3, 'Torneio C', '2024-12-15 10:00:00', '2024-12-20 18:00:00', 'Estádio C', 'Descrição do Torneio C', 'Regras do Torneio C', 12, 'Concluído', NULL);

--
-- Acionadores `torneios`
--
DELIMITER $$
CREATE TRIGGER `trigger_verifica_nome_torneio` BEFORE INSERT ON `torneios` FOR EACH ROW BEGIN
    DECLARE torneio_count INT;

    SELECT COUNT(*)
    INTO torneio_count
    FROM torneios
    WHERE nome = NEW.nome;

    IF torneio_count > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Erro: Já existe um torneio com esse nome.';
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estrutura para tabela `usuarios`
--

CREATE TABLE `usuarios` (
  `id` int(11) NOT NULL,
  `nome` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `senha` varchar(255) NOT NULL,
  `tipo` enum('Usuário','Administrador','Patrocinador','Capitão') DEFAULT 'Usuário',
  `status` enum('Ativo','Inativo') DEFAULT 'Ativo',
  `jogador_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `usuarios`
--

INSERT INTO `usuarios` (`id`, `nome`, `email`, `senha`, `tipo`, `status`, `jogador_id`) VALUES
(1, 'vitor', 'vitorrei1276@gmail.com', '12345', 'Administrador', 'Ativo', NULL),
(2, 'user1', 'user1@example.com', 'senha123', 'Usuário', 'Ativo', NULL),
(3, 'user2', 'user2@example.com', 'senha456', 'Usuário', 'Ativo', NULL),
(4, 'noppa', 'noppa1276@gmail.com', '12345', 'Usuário', 'Ativo', 12),
(5, 'caio', 'caio@gmail.com', '12345', 'Usuário', 'Ativo', 13),
(6, 'noppa1', 'user1234@gmail.com', '12345', 'Usuário', 'Ativo', 14);

--
-- Acionadores `usuarios`
--
DELIMITER $$
CREATE TRIGGER `trigger_verifica_nome_usuario` BEFORE INSERT ON `usuarios` FOR EACH ROW BEGIN
    DECLARE usuario_count INT;

    -- Verifica se já existe um usuário com o mesmo nome
    SELECT COUNT(*)
    INTO usuario_count
    FROM usuarios
    WHERE nome = NEW.nome;

    -- Se encontrar um usuário com o mesmo nome, gera um erro
    IF usuario_count > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Erro: Já existe um usuário com esse nome.';
    END IF;
END
$$
DELIMITER ;

--
-- Índices para tabelas despejadas
--

--
-- Índices de tabela `equipes`
--
ALTER TABLE `equipes`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `nome` (`nome`),
  ADD KEY `capitao_id` (`capitao_id`);

--
-- Índices de tabela `equipes_inscritas`
--
ALTER TABLE `equipes_inscritas`
  ADD PRIMARY KEY (`id`),
  ADD KEY `torneio_id` (`torneio_id`),
  ADD KEY `equipe_id` (`equipe_id`);

--
-- Índices de tabela `equipes_jogadores`
--
ALTER TABLE `equipes_jogadores`
  ADD PRIMARY KEY (`id`),
  ADD KEY `jogador_id` (`jogador_id`),
  ADD KEY `equipe_id` (`equipe_id`);

--
-- Índices de tabela `eventos_patrocinadores`
--
ALTER TABLE `eventos_patrocinadores`
  ADD PRIMARY KEY (`id`),
  ADD KEY `torneio_id` (`torneio_id`),
  ADD KEY `patrocinador_id` (`patrocinador_id`);

--
-- Índices de tabela `jogadores`
--
ALTER TABLE `jogadores`
  ADD PRIMARY KEY (`id`),
  ADD KEY `equipe_id` (`equipe_id`),
  ADD KEY `personagemMain_id` (`personagemMain_id`);

--
-- Índices de tabela `patrocinadores`
--
ALTER TABLE `patrocinadores`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `personagens`
--
ALTER TABLE `personagens`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `premios`
--
ALTER TABLE `premios`
  ADD PRIMARY KEY (`id`),
  ADD KEY `torneio_id` (`torneio_id`),
  ADD KEY `patrocinador_id` (`patrocinador_id`);

--
-- Índices de tabela `rankings`
--
ALTER TABLE `rankings`
  ADD PRIMARY KEY (`id`),
  ADD KEY `torneio_id` (`torneio_id`),
  ADD KEY `equipe_id` (`equipe_id`);

--
-- Índices de tabela `torneios`
--
ALTER TABLE `torneios`
  ADD PRIMARY KEY (`id`);

--
-- Índices de tabela `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`),
  ADD KEY `jogador_id` (`jogador_id`);

--
-- AUTO_INCREMENT para tabelas despejadas
--

--
-- AUTO_INCREMENT de tabela `equipes`
--
ALTER TABLE `equipes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT de tabela `equipes_inscritas`
--
ALTER TABLE `equipes_inscritas`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de tabela `equipes_jogadores`
--
ALTER TABLE `equipes_jogadores`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `eventos_patrocinadores`
--
ALTER TABLE `eventos_patrocinadores`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `jogadores`
--
ALTER TABLE `jogadores`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de tabela `patrocinadores`
--
ALTER TABLE `patrocinadores`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de tabela `personagens`
--
ALTER TABLE `personagens`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT de tabela `premios`
--
ALTER TABLE `premios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de tabela `rankings`
--
ALTER TABLE `rankings`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT de tabela `torneios`
--
ALTER TABLE `torneios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de tabela `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- Restrições para tabelas despejadas
--

--
-- Restrições para tabelas `equipes`
--
ALTER TABLE `equipes`
  ADD CONSTRAINT `equipes_ibfk_1` FOREIGN KEY (`capitao_id`) REFERENCES `jogadores` (`id`);

--
-- Restrições para tabelas `equipes_inscritas`
--
ALTER TABLE `equipes_inscritas`
  ADD CONSTRAINT `equipes_inscritas_ibfk_1` FOREIGN KEY (`torneio_id`) REFERENCES `torneios` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `equipes_inscritas_ibfk_2` FOREIGN KEY (`equipe_id`) REFERENCES `equipes` (`id`) ON DELETE CASCADE;

--
-- Restrições para tabelas `equipes_jogadores`
--
ALTER TABLE `equipes_jogadores`
  ADD CONSTRAINT `equipes_jogadores_ibfk_1` FOREIGN KEY (`jogador_id`) REFERENCES `jogadores` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `equipes_jogadores_ibfk_2` FOREIGN KEY (`equipe_id`) REFERENCES `equipes` (`id`) ON DELETE CASCADE;

--
-- Restrições para tabelas `eventos_patrocinadores`
--
ALTER TABLE `eventos_patrocinadores`
  ADD CONSTRAINT `eventos_patrocinadores_ibfk_1` FOREIGN KEY (`torneio_id`) REFERENCES `torneios` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `eventos_patrocinadores_ibfk_2` FOREIGN KEY (`patrocinador_id`) REFERENCES `patrocinadores` (`id`) ON DELETE CASCADE;

--
-- Restrições para tabelas `jogadores`
--
ALTER TABLE `jogadores`
  ADD CONSTRAINT `jogadores_ibfk_1` FOREIGN KEY (`equipe_id`) REFERENCES `equipes` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `jogadores_ibfk_2` FOREIGN KEY (`personagemMain_id`) REFERENCES `personagens` (`id`) ON DELETE SET NULL;

--
-- Restrições para tabelas `premios`
--
ALTER TABLE `premios`
  ADD CONSTRAINT `premios_ibfk_1` FOREIGN KEY (`torneio_id`) REFERENCES `torneios` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `premios_ibfk_2` FOREIGN KEY (`patrocinador_id`) REFERENCES `patrocinadores` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `premios_ibfk_3` FOREIGN KEY (`patrocinador_id`) REFERENCES `patrocinadores` (`id`) ON DELETE SET NULL;

--
-- Restrições para tabelas `rankings`
--
ALTER TABLE `rankings`
  ADD CONSTRAINT `rankings_ibfk_1` FOREIGN KEY (`torneio_id`) REFERENCES `torneios` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `rankings_ibfk_2` FOREIGN KEY (`equipe_id`) REFERENCES `equipes` (`id`) ON DELETE CASCADE;

--
-- Restrições para tabelas `usuarios`
--
ALTER TABLE `usuarios`
  ADD CONSTRAINT `usuarios_ibfk_1` FOREIGN KEY (`jogador_id`) REFERENCES `jogadores` (`id`);

DELIMITER $$
--
-- Eventos
--
CREATE DEFINER=`root`@`localhost` EVENT `atualiza_status_torneios` ON SCHEDULE EVERY 1 DAY STARTS '2024-11-18 21:06:56' ON COMPLETION NOT PRESERVE ENABLE DO -- Atualiza o status dos torneios cujo data_fim já passou
    UPDATE torneios
    SET status = 'Concluído'
    WHERE data_fim < NOW() AND status != 'Concluído'$$

DELIMITER ;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
