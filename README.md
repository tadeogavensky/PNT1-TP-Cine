USE PNT1_TP1_Cine;
GO

-- 1. Deshabilitar restricciones
ALTER TABLE Tickets NOCHECK CONSTRAINT ALL;
ALTER TABLE Funciones NOCHECK CONSTRAINT ALL;
ALTER TABLE Usuarios NOCHECK CONSTRAINT ALL;
ALTER TABLE Peliculas NOCHECK CONSTRAINT ALL;

-- 2. Eliminar datos
DELETE FROM Tickets;
DELETE FROM Funciones;
DELETE FROM Usuarios;
DELETE FROM Salas;
DELETE FROM Peliculas;
DELETE FROM Generos;
DELETE FROM Roles;

-- 3. Resetear IDENTITY
DBCC CHECKIDENT ('Tickets', RESEED, 0);
DBCC CHECKIDENT ('Funciones', RESEED, 0);
DBCC CHECKIDENT ('Usuarios', RESEED, 0);
DBCC CHECKIDENT ('Salas', RESEED, 0);
DBCC CHECKIDENT ('Peliculas', RESEED, 0);
DBCC CHECKIDENT ('Generos', RESEED, 0);
DBCC CHECKIDENT ('Roles', RESEED, 0);

-- 4. Rehabilitar restricciones
ALTER TABLE Tickets CHECK CONSTRAINT ALL;
ALTER TABLE Funciones CHECK CONSTRAINT ALL;
ALTER TABLE Usuarios CHECK CONSTRAINT ALL;
ALTER TABLE Peliculas CHECK CONSTRAINT ALL;

-- 5. Datos reales

-- Roles
INSERT INTO Roles (Nombre) VALUES ('Admin'), ('Cliente');

-- Géneros
INSERT INTO Generos (Nombre) VALUES 
('Acción'), ('Comedia'), ('Drama'), ('Ciencia Ficción'), ('Terror'), 
('Romance'), ('Animación'), ('Documental');

-- Películas reales
INSERT INTO Peliculas (Titulo, Duracion, FechaEstreno, GeneroId, Imagen, Sinopsis) VALUES
('Dune: Part Two', 166, '2024-03-01', 4, 'https://m.media-amazon.com/images/I/81ZMkn8HGBL._UF894,1000_QL80_.jpg', 'Paul Atreides unites with Chani and the Fremen.'),
('Oppenheimer', 180, '2023-07-21', 3, 'https://http2.mlstatic.com/D_NQ_NP_731427-MLA70538550538_072023-O.webp', 'Biopic of J. Robert Oppenheimer, father of the atomic bomb.'),
('Barbie', 114, '2023-07-20', 2, 'https://upload.wikimedia.org/wikipedia/en/0/0b/Barbie_2023_poster.jpg', 'Barbie and Ken explore the real world.'),
('Avengers: Endgame', 181, '2019-04-26', 1, 'https://upload.wikimedia.org/wikipedia/en/0/0d/Avengers_Endgame_poster.jpg', 'The Avengers assemble to reverse Thanos'),
('It Chapter Two', 169, '2019-09-06', 5, 'https://m.media-amazon.com/images/I/51NB383CKkL._AC_SL1200_.jpg', 'The Losers Club return to face Pennywise.'),
('La La Land', 128, '2016-12-09', 6, 'https://upload.wikimedia.org/wikipedia/en/a/ab/La_La_Land_%28film%29.png', 'A jazz musician and actress fall in love.'),
('Inside Out', 95, '2015-06-19', 7, 'https://upload.wikimedia.org/wikipedia/en/0/0a/Inside_Out_%282015_film%29_poster.jpg', 'A young girl emotions guide her through change.'),
('The Social Dilemma', 94, '2020-01-26', 8, 'https://www.researchgate.net/publication/377589183/figure/fig3/AS:11431281223415679@1707725963442/The-poster-for-The-Social-Dilemma.jpg', 'A documentary about the dangers of social media.');

-- Hash de la contraseña "12345"
DECLARE @hash NVARCHAR(255) = 'AQAAAAIAAYagAAAAELqyTei/SuRE46F9fFoLOgGsgla8J4HfxQIIeXm9aV8PqGwjrdSXNoH2pKmXmGKQ2A==';

-- Usuarios
INSERT INTO Usuarios (Nombre, Apellido, Email, Contrasena, RolId) VALUES
('Juan', 'Perez', 'juan.perez@example.com', @hash, 2),
('Maria', 'Lopez', 'maria.lopez@example.com', @hash, 2),
('Carlos', 'Gomez', 'carlos.gomez@example.com', @hash, 2),
('Ana', 'Martinez', 'ana.martinez@example.com', @hash, 2),
('Admin', 'User', 'admin@example.com', @hash, 1),
('Super', 'Admin', 'superadmin@example.com', @hash, 1);

-- Salas
INSERT INTO Salas (Numero, Capacidad) VALUES 
(1, 100), (2, 150), (3, 200), (4, 120), (5, 180), (6, 220);

-- Funciones
INSERT INTO Funciones (FechaHora, PeliculaId, SalaId) VALUES
('2025-07-01 18:00:00', 1, 1),
('2025-07-01 20:30:00', 2, 2),
('2025-07-02 17:00:00', 3, 1),
('2025-07-02 19:30:00', 4, 3),
('2025-07-03 18:00:00', 5, 4),
('2025-07-03 20:30:00', 6, 5),
('2025-07-04 17:00:00', 7, 6),
('2025-07-04 19:30:00', 8, 2);

-- Tickets
INSERT INTO Tickets (FechaCompra, FuncionId, Numero, NumeroAsientos, Precio, UsuarioId) VALUES
('2025-06-20 10:00:00', 1, 101, 2, 500.00, 1),
('2025-06-21 11:30:00', 2, 102, 3, 750.00, 2),
('2025-06-22 14:00:00', 3, 103, 1, 250.00, 1),
('2025-06-23 15:00:00', 4, 104, 4, 1000.00, 3),
('2025-06-24 16:30:00', 5, 105, 2, 500.00, 4),
('2025-06-25 18:00:00', 6, 106, 3, 750.00, 2);
