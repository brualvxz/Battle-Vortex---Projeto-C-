-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Tempo de geração: 07-Nov-2024 às 12:35
-- Versão do servidor: 10.4.22-MariaDB
-- versão do PHP: 8.1.0

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
-- Estrutura da tabela `equipes`
--

CREATE TABLE `equipes` (
  `id` int(11) NOT NULL,
  `nome` varchar(100) NOT NULL,
  `logo` varchar(255) DEFAULT NULL,
  `localidade` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

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
-- Estrutura da tabela `equipes_inscritas`
--

CREATE TABLE `equipes_inscritas` (
  `id` int(11) NOT NULL,
  `torneio_id` int(11) NOT NULL,
  `equipe_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

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

-- --------------------------------------------------------

--
-- Estrutura da tabela `jogadores`
--

CREATE TABLE `jogadores` (
  `id` int(11) NOT NULL,
  `nome` varchar(100) NOT NULL,
  `nickname` varchar(100) DEFAULT NULL,
  `equipe_id` int(11) DEFAULT NULL,
  `personagemMain` varchar(100) DEFAULT NULL,
  `conquistas` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Estrutura da tabela `patrocinadores`
--

CREATE TABLE `patrocinadores` (
  `id` int(11) NOT NULL,
  `nome` varchar(100) NOT NULL,
  `conquistas` text NOT NULL,
  `logo` varchar(255) DEFAULT NULL,
  `evento_patrocinado` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Estrutura da tabela `personagens`
--

CREATE TABLE `personagens` (
  `id` int(11) NOT NULL,
  `nome` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Estrutura da tabela `premios`
--

CREATE TABLE `premios` (
  `id` int(11) NOT NULL,
  `torneio_id` int(11) NOT NULL,
  `descricao` varchar(255) NOT NULL,
  `valor` decimal(10,2) DEFAULT NULL,
  `tipo` enum('Dinheiro','Troféu','Patrocínio') DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Estrutura da tabela `rankings`
--

CREATE TABLE `rankings` (
  `id` int(11) NOT NULL,
  `torneio_id` int(11) NOT NULL,
  `equipe_id` int(11) NOT NULL,
  `posicao` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Estrutura da tabela `torneios`
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
  `status` enum('Em andamento','Concluído') DEFAULT 'Em andamento'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

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
-- Estrutura da tabela `usuarios`
--

CREATE TABLE `usuarios` (
  `id` int(11) NOT NULL,
  `nome` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `senha_hash` varchar(255) NOT NULL,
  `tipo` enum('Usuário','Administrador') DEFAULT 'Usuário',
  `status` enum('Ativo','Inativo') DEFAULT 'Ativo'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

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
-- Índices para tabela `equipes`
--
ALTER TABLE `equipes`
  ADD PRIMARY KEY (`id`);

--
-- Índices para tabela `equipes_inscritas`
--
ALTER TABLE `equipes_inscritas`
  ADD PRIMARY KEY (`id`),
  ADD KEY `torneio_id` (`torneio_id`),
  ADD KEY `equipe_id` (`equipe_id`);

--
-- Índices para tabela `jogadores`
--
ALTER TABLE `jogadores`
  ADD PRIMARY KEY (`id`),
  ADD KEY `equipe_id` (`equipe_id`);

--
-- Índices para tabela `patrocinadores`
--
ALTER TABLE `patrocinadores`
  ADD PRIMARY KEY (`id`),
  ADD KEY `evento_patrocinado` (`evento_patrocinado`);

--
-- Índices para tabela `personagens`
--
ALTER TABLE `personagens`
  ADD PRIMARY KEY (`id`);

--
-- Índices para tabela `premios`
--
ALTER TABLE `premios`
  ADD PRIMARY KEY (`id`),
  ADD KEY `torneio_id` (`torneio_id`);

--
-- Índices para tabela `rankings`
--
ALTER TABLE `rankings`
  ADD PRIMARY KEY (`id`),
  ADD KEY `torneio_id` (`torneio_id`),
  ADD KEY `equipe_id` (`equipe_id`);

--
-- Índices para tabela `torneios`
--
ALTER TABLE `torneios`
  ADD PRIMARY KEY (`id`);

--
-- Índices para tabela `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- AUTO_INCREMENT de tabelas despejadas
--

--
-- AUTO_INCREMENT de tabela `equipes`
--
ALTER TABLE `equipes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `equipes_inscritas`
--
ALTER TABLE `equipes_inscritas`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `jogadores`
--
ALTER TABLE `jogadores`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `patrocinadores`
--
ALTER TABLE `patrocinadores`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `personagens`
--
ALTER TABLE `personagens`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `premios`
--
ALTER TABLE `premios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `rankings`
--
ALTER TABLE `rankings`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `torneios`
--
ALTER TABLE `torneios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Restrições para despejos de tabelas
--

--
-- Limitadores para a tabela `equipes_inscritas`
--
ALTER TABLE `equipes_inscritas`
  ADD CONSTRAINT `equipes_inscritas_ibfk_1` FOREIGN KEY (`torneio_id`) REFERENCES `torneios` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `equipes_inscritas_ibfk_2` FOREIGN KEY (`equipe_id`) REFERENCES `equipes` (`id`) ON DELETE CASCADE;

--
-- Limitadores para a tabela `jogadores`
--
ALTER TABLE `jogadores`
  ADD CONSTRAINT `jogadores_ibfk_1` FOREIGN KEY (`equipe_id`) REFERENCES `equipes` (`id`) ON DELETE SET NULL;

--
-- Limitadores para a tabela `patrocinadores`
--
ALTER TABLE `patrocinadores`
  ADD CONSTRAINT `patrocinadores_ibfk_1` FOREIGN KEY (`evento_patrocinado`) REFERENCES `torneios` (`id`) ON DELETE SET NULL;

--
-- Limitadores para a tabela `premios`
--
ALTER TABLE `premios`
  ADD CONSTRAINT `premios_ibfk_1` FOREIGN KEY (`torneio_id`) REFERENCES `torneios` (`id`) ON DELETE CASCADE;

--
-- Limitadores para a tabela `rankings`
--
ALTER TABLE `rankings`
  ADD CONSTRAINT `rankings_ibfk_1` FOREIGN KEY (`torneio_id`) REFERENCES `torneios` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `rankings_ibfk_2` FOREIGN KEY (`equipe_id`) REFERENCES `equipes` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
