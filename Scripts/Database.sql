drop database if exists OnBoardingAWAQ;
create database OnBoardingAWAQ;

use OnBoardingAWAQ;

create table usuario(
	idUsuario int primary key auto_increment,
	nombre varchar(70),
	pais varchar(50),
	ciudad varchar(50),
	correo varchar(320),
	telefono varchar(15),
	contrasena varchar(30),
	superUsuario tinyint default 0
);

create table departamento(
	idDepartamento int primary key auto_increment,
	departamento varchar(20)
);

create table usuarioDepartamento(
	idDepartamento int,
	idUsuario int,
	foreign key (idDepartamento) references departamento(idDepartamento),
	foreign key (idUsuario) references usuario(idUsuario)
);

create table token(
	idToken int primary key auto_increment,
	token varchar(8),
	idUsuario int,
	generado datetime,
	foreign key (idUsuario) references usuario(idUsuario)
);

create table zona(
	idZona int primary key auto_increment,
	zona varchar(20)
);

create table zonaUsuario(
	idZona int,
	idUsuario int,
	progreso int,
	foreign key (idUsuario) references usuario(idUsuario),
	foreign key (idZona) references zona(idZona)
);

create table minijuego(
	idMinijuego int primary key auto_increment,
	minijuego varchar(50),
	aciertos int,
	completado tinyint,
	idZona int,
	foreign key (idZona) references zona(idZona)
);

create table pregunta(
	idPregunta int primary key auto_increment,
	pregunta varchar(200),
	segundos int,
	acierto tinyint,
	idMinijuego int,
	foreign key (idMinijuego) references minijuego(idMinijuego)
);

create table respuesta(
	idRespuesta int primary key auto_increment,
	respuesta varchar(100)
);

create table preguntaRespuesta(
	idPregunta int,
	idRespuesta int,
	correcta tinyint,
	foreign key (idPregunta) references pregunta(idPregunta),
	foreign key (idRespuesta) references respuesta(idRespuesta)
);

insert into usuario (nombre, pais, ciudad, correo, telefono, contrasena, superUsuario) values
	("Adrian", "Mexico", "Monterrey","atrevino136@gmail.com", "0451664", "AJTJ", 1),
	("Pedro Jimenez", "Mexico", "Monterrey", "pedroJ@pedro.com", "1111111", "PedroJPedro", 0),
	("Rodrigo Perez", "Colombia", "Medellin", "rodrigoP@rodrigo.com", "8888888", "RodrigoPRodrigo", 0),
	("Maria Rodriguez", "Colombia", "Medellin", "maria@gmail.com", "7777777", "ContraM123", 0),
	("Juan Rulfo", "España", "Barcelona", "Rulfo_Juan@yahoo.com", "6666666", "A143Vcew@19", 0),
    ("Ricardo", "Mexico", "CDMX", "rh9344001@gmail.com", "844555221", "sayra", 1);

insert into departamento (departamento) values 
	("TEDI"),
	("Recursos Humanos");

insert into usuarioDepartamento (idUsuario, idDepartamento) values
	(1,1),
	(2,2),
	(3,1),
	(4,1),
	(5,2);

insert into zona (zona) values
	("Ciudad"),
	("Montaña"),
	("Bosque"),
	("Rio");

insert into zonaUsuario(idZona, idUsuario, progreso) values
	(1,1,50),
	(2,1,0),
	(3,1,0),
	(4,1,0),
	(1,2,100),
	(2,2,5),
	(3,2,0),
	(4,2,0);

insert into minijuego (minijuego, aciertos, completado, idZona) values	
	("Goat", 3, 1, 2),
	("Rocks", 0, 0, 2),
	("Detener Excavacion", 0, 0, 2),
	("Berries", 3, 1, 3),
	("Incendio", 0, 0, 3),
	("Tala", 0, 1, 3);

insert into pregunta (pregunta, segundos, acierto, idMinijuego) values
	("¿Qué acción no está asociada con el principio de transparencia según el texto?", 0, 0, 6);

insert into respuesta (respuesta) values 
	("Responder de forma diligente a las demandas de información"),
	("Aceptar críticas constructivas"),
	("Ocultar información relevante"),
	("Asumir responsabilidad por los resultados");

insert into preguntaRespuesta (idPregunta, idRespuesta, correcta) values 
	(1,1,0),
	(1,1,0),
	(1,1,1),
	(1,1,0);

select * from usuario;

select * from departamento;

select * from usuarioDepartamento;

select * from zona;

select * from zonaUsuario;

select * from minijuego;

select * from pregunta;

select * from respuesta;

select * from preguntaRespuesta;