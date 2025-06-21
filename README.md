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
('The Great Adventure', 120, '2023-01-15', 1, 'https://fastly.picsum.photos/id/1/5000/3333.jpg?hmac=Asv2DU3rA_5D1xSe22xZK47WEAN0wjWeFOhzd13ujW4', 'A thrilling adventure movie.'),
('Laugh Out Loud', 90, '2023-03-01', 2, 'https://fastly.picsum.photos/id/10/2500/1667.jpg?hmac=J04WWC_ebchx3WwzbM-Z4_KC_LeLBWr5LZMaAkWkF68', 'A hilarious comedy.'),
('Deep Emotions', 110, '2023-02-10', 3, 'https://fastly.picsum.photos/id/22/4434/3729.jpg?hmac=fjZdkSMZJNFgsoDh8Qo5zdA_nSGUAWvKLyyqmEt2xs0', 'A touching drama film.'),
('Future World', 130, '2023-04-20', 4, 'https://fastly.picsum.photos/id/21/3008/2008.jpg?hmac=T8DSVNvP-QldCew7WD4jj_S3mWwxZPqdF0CNPksSko4', 'A sci-fi journey to the future.'),
('Night Terrors', 100, '2023-05-10', 5, 'https://fastly.picsum.photos/id/27/3264/1836.jpg?hmac=p3BVIgKKQpHhfGRRCbsi2MCAzw8mWBCayBsKxxtWO8g', 'A horror film that will keep you awake.'),
('Love Forever', 115, '2023-06-01', 6, 'https://fastly.picsum.photos/id/30/1280/901.jpg?hmac=A_hpFyEavMBB7Dsmmp53kPXKmatwM05MUDatlWSgATE', 'A romantic story that melts hearts.'),
('Animated Fun', 80, '2023-07-15', 7, 'https://fastly.picsum.photos/id/32/4032/3024.jpg?hmac=n7I3OdGszMIwuGcvplNthgBmAxvAZ3rNBBSuDFZaItQ', 'A family-friendly animated movie.'),
('The Real World', 95, '2023-08-05', 8, 'https://fastly.picsum.photos/id/35/2758/3622.jpg?hmac=xIB3RTEGJ59FEnaQOXoaDgwX_K6PHAg57R0b4t7tiX0', 'An insightful documentary.');

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

