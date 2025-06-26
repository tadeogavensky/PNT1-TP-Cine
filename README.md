# PNT1-TP-Cine

## RESETEO de la base de datos
USE PNT1_TP1_Cine_test;
GO

-- 1. Deshabilitar restricciones de claves foráneas temporalmente
ALTER TABLE Tickets NOCHECK CONSTRAINT ALL;
ALTER TABLE Funciones NOCHECK CONSTRAINT ALL;
ALTER TABLE Usuarios NOCHECK CONSTRAINT ALL;
ALTER TABLE Peliculas NOCHECK CONSTRAINT ALL;

-- 2. Borrar datos respetando el orden de dependencias
DELETE FROM Tickets;
DELETE FROM Funciones;
DELETE FROM Usuarios;
DELETE FROM Salas;
DELETE FROM Peliculas;
DELETE FROM Generos;
DELETE FROM Roles;

-- 3. Reseteo de IDENTITY (auto incremento)
DBCC CHECKIDENT ('Tickets', RESEED, 0);
DBCC CHECKIDENT ('Funciones', RESEED, 0);
DBCC CHECKIDENT ('Usuarios', RESEED, 0);
DBCC CHECKIDENT ('Salas', RESEED, 0);
DBCC CHECKIDENT ('Peliculas', RESEED, 0);
DBCC CHECKIDENT ('Generos', RESEED, 0);
DBCC CHECKIDENT ('Roles', RESEED, 0);

-- 4. Rehabilitar las restricciones de claves foráneas
ALTER TABLE Tickets CHECK CONSTRAINT ALL;
ALTER TABLE Funciones CHECK CONSTRAINT ALL;
ALTER TABLE Usuarios CHECK CONSTRAINT ALL;
ALTER TABLE Peliculas CHECK CONSTRAINT ALL;

## Dummy Data (INSERTS).

INSERT INTO Roles (Nombre) VALUES
('Admin'),
('Cliente');

INSERT INTO Generos (Nombre) VALUES
('Acción'),
('Comedia'),
('Drama'),
('Ciencia Ficción'),
('Terror'),
('Romance'),
('Animación'),
('Documental');

INSERT INTO Peliculas (Titulo, Duracion, FechaEstreno, GeneroId, Imagen, Sinopsis) VALUES
('The Great Adventure', 120, '2023-01-15', 1, 'https://image.tmdb.org/t/p/original/qJ2tW6WMUDux911r6m7haRef0WH.jpg', 'A thrilling adventure movie.'),               -- The Dark Knight (Adventure/Action)
('Laugh Out Loud', 90, '2023-03-01', 2, 'https://image.tmdb.org/t/p/original/4Kbxg7NQbl77FkXCDB9QarsjG5b.jpg', 'A hilarious comedy.'),                        -- Superbad (Comedy)
('Deep Emotions', 110, '2023-02-10', 3, 'https://image.tmdb.org/t/p/original/q6y0Go1tsGEsmtFryDOJo3dEmqu.jpg', 'A touching drama film.'),                     -- The Shawshank Redemption (Drama)
('Future World', 130, '2023-04-20', 4, 'https://image.tmdb.org/t/p/original/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg', 'A sci-fi journey to the future.'),             -- Interstellar (Sci-Fi)
('Night Terrors', 100, '2023-05-10', 5, 'https://image.tmdb.org/t/p/original/4GFPuL14eXi66V5LyDkXO32ruZY.jpg', 'A horror film that will keep you awake.'),     -- Hereditary (Horror)
('Love Forever', 115, '2023-06-01', 6, 'https://image.tmdb.org/t/p/original/uDO8zWDhfWwoFdKS4fzkUJt0Rf0.jpg', 'A romantic story that melts hearts.'),          -- La La Land (Romance)
('Animated Fun', 80, '2023-07-15', 7, 'https://image.tmdb.org/t/p/original/uXDfjJbdP4ijW5hWSBrPrlKpxab.jpg', 'A family-friendly animated movie.'),            -- Toy Story (Animation)
('The Real World', 95, '2023-08-05', 8, 'https://image.tmdb.org/t/p/original/6j7UZ5gFP5BbEfbHuJdLVvLrQ6u.jpg', 'An insightful documentary.');                 -- The Social Dilemma (Documentary)

INSERT INTO Usuarios (Nombre, Apellido, Email, Contrasena, RolId) VALUES
('Juan', 'Perez', 'juan.perez@example.com', 'password123', 2),
('Maria', 'Lopez', 'maria.lopez@example.com', 'password123', 2),
('Carlos', 'Gomez', 'carlos.gomez@example.com', 'password123', 2),
('Ana', 'Martinez', 'ana.martinez@example.com', 'password123', 2),
('Admin', 'User', 'admin@example.com', 'password123', 1),
('Super', 'Admin', 'superadmin@example.com', 'password123', 1);

INSERT INTO Salas (Numero, Capacidad) VALUES
(1, 100),
(2, 150),
(3, 200),
(4, 120),
(5, 180),
(6, 220);

INSERT INTO Funciones (FechaHora, PeliculaId, SalaId) VALUES
('2025-07-01 18:00:00', 1, 1),
('2025-07-01 20:30:00', 2, 2),
('2025-07-02 17:00:00', 3, 1),
('2025-07-02 19:30:00', 4, 3),
('2025-07-03 18:00:00', 5, 4),
('2025-07-03 20:30:00', 6, 5),
('2025-07-04 17:00:00', 7, 6),
('2025-07-04 19:30:00', 8, 2);

INSERT INTO Tickets (FechaCompra, FuncionId, Numero, NumeroAsientos, Precio, UsuarioId) VALUES
('2025-06-20 10:00:00', 1, 101, 2, 500.00, 1),
('2025-06-21 11:30:00', 2, 102, 3, 750.00, 2),
('2025-06-22 14:00:00', 3, 103, 1, 250.00, 1),
('2025-06-23 15:00:00', 4, 104, 4, 1000.00, 3),
('2025-06-24 16:30:00', 5, 105, 2, 500.00, 4),
('2025-06-25 18:00:00', 6, 106, 3, 750.00, 2);

