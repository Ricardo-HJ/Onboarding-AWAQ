create database OnBoardingAWAQ;

-- drop database OnBoardingAWAQ;

use OnBoardingAWAQ;

create table usuario(
	idUsuario int primary key auto_increment,
	nombre varchar(70),
	correo varchar(320),
	`contraseña` varchar(30)
);

insert into OnBoardingAWAQ.usuario (nombre, correo, contraseña) values
	("Pedro Jimenez", "pedroJ@pedro.com", "PedroJPedro"),
	("Rodrigo Perez", "rodrigoP@rodrigo.com", "RodrigoPRodrigo"),
	("Maria Rodriguez", "maria@gmail.com", "ContraM123"),
	("Juan Rulfo", "Rulfo_Juan@yahoo.com", "A143Vcew@19");